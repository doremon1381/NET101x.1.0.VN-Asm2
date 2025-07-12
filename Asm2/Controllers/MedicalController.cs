using Asm2.Attributes;
using Asm2.DataTransferModels.Medical;
using DinkToPdf;
using DinkToPdf.Contracts;
using MedicalModel;
using MedicalService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Asm2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MedicalController : ControllerBase
    {
        private readonly IMedicalServices _medicalServices;
        private readonly IConverter _converter;

        public MedicalController(IMedicalServices medicalServices, IConverter converter)
        {
            _medicalServices = medicalServices;
            _converter = converter;
        }

        [HttpGet("/speciality/country/{countryCode}/{region}")]
        public async Task<IActionResult> GetAllHospitalInCountryAsync(string region, string countryCode = "VN")
        {
            var hospitals = await _medicalServices.GetAllHospitalInCountryAsync(region, countryCode);

            return hospitals.Count switch
            {
                0 => NotFound("No hospitals found in the specified country."),
                _ => Ok(hospitals)
            };
        }

        [HttpGet("/search/doctors")]
        public async Task<IActionResult> FindDoctorsFromParticularRegionAsync([FromBody] FindDoctorModel doctorModel)
        {
            // find doctors inside all hospitals of a particular region
            var doctors = await _medicalServices.FindDoctorsFromParticularRegionAsync(doctorModel.FirstName, doctorModel.LastName, doctorModel.HospitalName, doctorModel.Specialty, doctorModel.City, doctorModel.CountryCode);

            var result = doctors.Select(d => new DoctorModel()
            {
                Id = d.Id,
                Address = d.Address ?? "",
                Email = d.Email ?? "",
                Gender = d.Gender switch
                {
                    Gender.Male => "Nam",
                    Gender.Female => "Nữ",
                    Gender.Other => "Khác",
                    _ => throw new System.Exception("Gender's information is not valid!")
                },
                FullName = $"{d.LastName} {d.FirstName}",
                Nationality = d.Nationality ?? "",
                PhoneNumber = d.PhoneNumber ?? "",
                HospitalName = d.DoctorInfo.Hospital.Name ?? "",
                Specialty = d.DoctorInfo.Specialty == null ? new[] { "" } : d.DoctorInfo.Specialty.Split(","),
                SubSpecialties = d.DoctorInfo.SubSpecialties == null ? new[] { "" } : d.DoctorInfo?.SubSpecialties.Split(","),
                ExperienceInYrs = d.DoctorInfo.ExperienceInYrs == null ? "unknown" : d.DoctorInfo.ExperienceInYrs.ToString()
            }).ToList();

            return result.Count switch
            {
                0 => NotFound(),
                _ => Ok(result)
            };
        }

        /// <summary>
        /// Get information of a doctor by their ID. Every role can access this endpoint.
        /// </summary>
        /// <param name="doctorId"></param>
        /// <returns></returns>
        [HttpGet("/specialist/{doctorId}/details")]
        public async Task<IActionResult> GetDoctorInformationAsync(string doctorId)
        {
            var doctor = await _medicalServices.FindDoctorByIdAsync(doctorId);
            if (doctor == null)
                return NotFound("there is no doctor with this id!");

            var resultModel = new DoctorModel()
            {
                Id = doctor.Id,
                ProfileImg = doctor.DoctorInfo is null ? "" : doctor.DoctorInfo.ProfileImageUrl,
                Address = doctor.Address ?? "",
                FullName = $"{doctor.LastName} {doctor.FirstName}",
                PhoneNumber = doctor.PhoneNumber ?? "",
                Email = doctor.Email ?? "",
                Gender = doctor.Gender switch { Gender.Male => "Nam", Gender.Female => "Nữ", Gender.Other => "Khác", _ => throw new System.Exception("Gender is not found") },
                Specialty = doctor.DoctorInfo.Specialty == null ? new[] { "" } : doctor.DoctorInfo.Specialty.Split(","),
                SubSpecialties = doctor.DoctorInfo.SubSpecialties == null ? new[] { "" } : doctor.DoctorInfo?.SubSpecialties.Split(","),
                ExperienceInYrs = doctor.DoctorInfo.ExperienceInYrs == null ? "unknown" : doctor.DoctorInfo.ExperienceInYrs.ToString(),
                HospitalName = doctor.DoctorInfo.Hospital.Name ?? "",
                Nationality = doctor.Nationality
            };

            return Ok(resultModel);
        }

        [HttpGet("/odp/complete/patient")]
        public async Task<IActionResult> GetPatientInformationInPDFAsync()
        {
            var patientEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var patient = await _medicalServices.FindPatientByEmailAsync(patientEmail);

            var html = $@"
                <html>
                <head>
                    <meta charset=""utf-8"" />
                    <link href=""https://fonts.googleapis.com/css2?family=Roboto&display=swap"" rel=""stylesheet"">
                    <style>
                        body {{ font-family: 'Roboto', sans-serif; }}
                        h1 {{ color: navy; }}
                        ul {{ padding-left: 20px; list-style: none; }}
                    </style>
                </head>
                <body>
                    <h1>Patient Information</h1>
                    <p><strong>Patient:</strong> {$"{patient.LastName} {patient.FirstName}"}</p>
                    <p><strong>Details:</strong></p>
                    <ul>
                        <li><strong>Id:</strong> {patient.Id}</li>
                        <li><strong>Address:</strong> {patient.Address ?? ""}</li>
                        <li><strong>FullName:</strong> {$"{patient.LastName} {patient.FirstName}"}</li>
                        <li><strong>Email:</strong> {patient.Email ?? ""}</li>
                        <li><strong>PhoneNumber:</strong> {patient.PhoneNumber ?? ""}</li>
                        <li><strong>Gender:</strong> {patient.Gender switch { Gender.Male => "Nam", Gender.Female => "Nữ", Gender.Other => "Khác", _ => throw new System.Exception("Gender is not found") }}</li>
                        <li><strong>Nationality:</strong> {patient.Nationality}</li>
                    </ul>
                </body>
                </html>";

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait
                },
                Objects = {
                    new ObjectSettings { HtmlContent = html }
                }
            };

            var pdfBytes = _converter.Convert(doc);

            return patient switch
            {
                null => NotFound("Patient not found."),
                _ => File(pdfBytes, "application/pdf", "patient.pdf")
            };
        }
    }
}
