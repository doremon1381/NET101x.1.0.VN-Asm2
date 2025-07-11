using Asm2.DataTransferModels.Identity;
using IdentityModel;
using IdentityService;
using MedicalModel;
using MedicalService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Asm2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IIdentityServices _identityServices;
        private readonly IMedicalServices _medicalServices; // used to get user information from the db

        // refresh otken
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthenticationController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager
            , IIdentityServices identityServices
            , IConfiguration configuration
            , TokenValidationParameters tokenValidationParameters
            , IMedicalServices medicalServices)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _identityServices = identityServices;
            _tokenValidationParameters = tokenValidationParameters;

            _medicalServices = medicalServices;
        }

        [HttpGet("get-user-info")]
        [Authorize]
        public async Task<IActionResult> GetUserInfoAsync()
        {
            var user = _userManager.Users.FirstOrDefault();
            var sUser = User;

            return Ok(user.UserName);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid login's information!");

            var user = _userManager.FindByNameAsync(loginRequest.UserName).Result;
            if (user == null)
                return BadRequest($"User with this username: {loginRequest.UserName} does not exist.");

            if (!_userManager.CheckPasswordAsync(user, loginRequest.Password).Result)
                return Unauthorized("Invalid password.");

            var roles = await _medicalServices.FindPersonRolesAsync(user.Id);

            // Generate JWT token
            var token = await GenerateJwtTokenAsync(user, roles);

            // Implement login logic here
            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
                throw new Exception("Invalid request's data!");

            var existingIdentity = await _userManager.FindByEmailAsync(registerRequest.Email);
            var existingUser = await _medicalServices.GetUserByEmailAsync(registerRequest.Email);

            if (existingIdentity != null
                || existingUser != null)
                return BadRequest($"User with this email: {registerRequest.Email} already exists with this email.");

            await CreateNewUser(registerRequest);

            // Implement registration logic here
            return Created(nameof(Register), $"User {registerRequest.Email} is created successful");
        }

        [HttpPost("refresh-accesstoken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest requestForToken)
        {
            var result = await VerifyAndGenerateTokenAsync(requestForToken);
            if (result == null)
            {
                return BadRequest("Invalid refresh token.");
            }

            return Ok(result);
        }

        private async Task<TokenResult> VerifyAndGenerateTokenAsync(RefreshTokenRequest requestForToken)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            // check jwt format
            var tokenValidation = jwtTokenHandler.ReadJwtToken(requestForToken.AccessToken);

            // check validate expiry date
            var expiryDateFromRequestToken = long.Parse(tokenValidation.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiryDate = UnixTimeStampToDateTimeUtc(expiryDateFromRequestToken);
            if (expiryDate > DateTime.Now)
            {
                throw new Exception("Token has not expired yet!");
            }

            // check refresh token exists in the db
            var dbRefreshToken = _identityServices.FindRefreshToken(requestForToken.RefreshToken);
            if (dbRefreshToken == null)
                throw new Exception("Refresh token does not exist in the database!");
            else
            {
                // check validate identity id, if the refresh token is belong to that user
                var jti = tokenValidation.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (dbRefreshToken.JwtId != jti)
                    throw new Exception("Bearer token is not matched!");

                // check if the refresh token is expiried
                if (dbRefreshToken.DateExpires < DateTime.Now)
                    throw new Exception("Refresh token has expired! Please re-authenticate!");

                // check if the refresh token is used
                if (dbRefreshToken.IsRevoked)
                    throw new Exception("Refresh token is used! Please re-authenticate!");

                // update refresh token for one time only
                dbRefreshToken.IsRevoked = true;
                _identityServices.UpdateRefreshToken(dbRefreshToken);

                // generate new Token 
                var dbUserData = await _userManager.FindByIdAsync(dbRefreshToken.UserId);
                var personRoles = await _medicalServices.FindPersonRolesAsync(dbRefreshToken.UserId);

                var newTokenResponse = GenerateJwtTokenAsync(dbUserData, personRoles);
                // TODO:
                return await newTokenResponse;
            }
        }

        private static DateTime UnixTimeStampToDateTimeUtc(long unixTimeStamp)
        {
            // Convert Unix timestamp to DateTime in UTC
            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp);
            return dateTimeOffset.DateTime.ToLocalTime();
        }

        private async Task CreateNewUser([FromBody] RegisterRequest registerRequest)
        {
            ApplicationUser newUser = new ApplicationUser
            {
                UserName = registerRequest.Username,                
                Email = registerRequest.Email
            };

            var result = await _userManager.CreateAsync(newUser, registerRequest.Password);

            if (!result.Succeeded)
                throw new System.Exception($"User creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            // by default, every user is assigned to the USER role
            await _userManager.AddToRoleAsync(newUser, UserRoles.USER);

            // add user account

            // Assign the specified role to the new account
            // by default, one account needs to be a patient or a doctor
            // admin role is not allowed to assign to a new user
            var role = (registerRequest.Role[0].ToString().ToUpper() + registerRequest.Role.Substring(1).ToLower()) switch
            {
                "Patient" => nameof(PersonRole.Patient),
                "Doctor" => nameof(PersonRole.Doctor),
                _ => throw new Exception("Invalid role specified. Only 'Patient' or 'Doctor' roles are allowed.")
            };
            var user = new Person()
            {
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Roles = new List<PersonRole>()
                {
                    (PersonRole)Enum.Parse(typeof(PersonRole), role)
                },
                Id = newUser.Id
            };
            await _medicalServices.AddPersonAsync(user);
        }

        private async Task<TokenResult> GenerateJwtTokenAsync(ApplicationUser user, List<PersonRole> roles)
        {
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            // get key from appsettings.json
            var authSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var securityToken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                // by defaut, an access token is valid for 5 minutes
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:Expires"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(securityToken);

            // create new refresh token when first time login or get new access token using onetime refresh token
            var refreshToken = new RefreshToken()
            {
                JwtId = securityToken.Id,
                IsRevoked = false,
                UserId = user.Id,
                DateAdded = DateTime.Now,
                Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString(),
                //DateExpires = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:RefreshTokenExpires"])),
                DateExpires = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:RefreshTokenExpires"]))
            };

            // add refresh token to the db
            await _identityServices.AddRefreshTokenAsync(refreshToken);

            var response = new TokenResult()
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken.Token,
                Expires_in = securityToken.ValidTo.ToLocalTime()
            };

            return response;
        }
    }
}
