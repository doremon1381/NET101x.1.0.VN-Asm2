using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService
{
    public class IdentityServices : IIdentityServices
    {
        private readonly IdentityDbContext _identityDbContext;
        /// <summary>
        /// IdentityServices constructor initializes the identity services if dependency injection is used
        /// </summary>
        /// <param name="identityDbContext"></param>
        public IdentityServices(IdentityDbContext identityDbContext = null)
        {
            // This constructor can be used to initialize any services or dependencies
            // related to identity management.
            if (identityDbContext == null)
            {
                // Initialize the identity database context if provided
                // This could include setting up repositories, services, etc.
                var options = new DbContextOptionsBuilder<IdentityDbContext>().Options;
                identityDbContext = new IdentityDbContext(options);

                _identityDbContext = identityDbContext;
            }
            else
                _identityDbContext = identityDbContext;
        }
        public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _identityDbContext.RefreshTokens.AddAsync(refreshToken);
            await _identityDbContext.SaveChangesAsync();
        }

        public RefreshToken FindRefreshToken(string refreshToken)
        {
            return _identityDbContext.RefreshTokens
                .FirstOrDefault(rt => rt.Token == refreshToken && rt.IsRevoked == false);
        }

        public bool HasRoles()
        {
            return _identityDbContext.Roles.Any();
        }

        public bool HasUsers()
        {
            return _identityDbContext.Users.Any();
        }

        public void UpdateRefreshToken(RefreshToken refreshToken)
        {
            _identityDbContext.Update(refreshToken);
            _identityDbContext.SaveChanges();
        }

        public List<ApplicationUser> GetAllUsers()
        {
            return _identityDbContext.Users.ToList();
        }
    }

    public interface IIdentityServices
    {
        Task AddRefreshTokenAsync(RefreshToken refreshToken);
        RefreshToken FindRefreshToken(string refreshToken);
        void UpdateRefreshToken(RefreshToken refreshToken);
        bool HasRoles();
        bool HasUsers();
        //List<ApplicationUser> GetAllUsers();
    }
}
