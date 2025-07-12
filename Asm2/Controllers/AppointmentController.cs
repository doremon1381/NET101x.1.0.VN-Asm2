using Asm2.Attributes;
using Asm2.DataTransferModels.Medical;
using MedicalModel;
using MedicalService;
using MedicalService.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Asm2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        private readonly IMedicalServices _medicalServices;

        public AppointmentController(IMedicalServices medicalServices)
        {
            _medicalServices = medicalServices;
        }

        // Example method to create a new appointment (to be implemented)
        [HttpPost("/schedule/book")]
        [PersonRoleAccepted(Role = PersonRole.Patient)]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentRequestModel appointment)
        {
            // I dont want to send patient id along with the request body
            // because I think from authentication step, the server know the identity of that request
            // that is a feature of token base authentication. And because of that, it makes no sense
            // if the patient still need to send his/her-self id in request body
            // long to short, I can take patient Id from IdentityClaims which define the user inside HttpContext

            // I use email as username, so search by name identifier also means search by email
            var patientEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var patient = await _medicalServices.FindPatientByEmailAsync(patientEmail);

            // Logic to create a new appointment
            var result = await _medicalServices.CreateAppointmentAsync(appointment, patient.Id);

            return Ok(new
            {
                DoctorId = result.DoctorId,
                AppointmentId = result.Id,
                DoctorName = $"{result.Doctor.LastName} {result.Doctor.FirstName}",
                Description = result.Description,
                Status = result.Status.ToString()
            });
        }

        // Add more methods as needed for your application.
        [HttpGet("/booking/unitId/{id}/details")]
        [PersonRoleAccepted(Role = PersonRole.Doctor)]
        public async Task<IActionResult> GetAppointmentForDoctorByIdAsync(string id)
        {
            var appointment = await _medicalServices.FindAppointmentByIdAsync(id);

            if (appointment == null)
                return NotFound("id for this appointment is wrong or mismatch!");

            var result = new DoctorAppointmentModel()
            {
                AppointmentId = appointment.Id,
                PatientName = $"{appointment.Patient.LastName} {appointment.Patient.FirstName}",
                StartDate = appointment.BookingSchedule.Start.ToLocalTime(),
                // one or two day after the booking has been received
                EndDate = appointment.BookingSchedule.Start.AddDays(1),
                // booking will be handler in one or two days
                ExpireAt = appointment.BookingSchedule.Start.AddDays(2),
                Status = appointment.Status.ToString()
            };

            return Ok(result);
        }

        [HttpGet("/booking/unitId/{appointmentId}/records")]
        [PersonRoleAccepted(Role = PersonRole.Patient)]
        public async Task<IActionResult> GetAppointmentForPatientByIdAsync(string appointmentId)
        {
            var patientEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var appointment = await _medicalServices.FindAppointmentForPatientByIdAsync(appointmentId, patientEmail);

            if (appointment == null)
                //|| appointment.Diagnosis == null)
                return NotFound("There is no appointment or that appointment's diagnosis was not found!");

            var result = new PatientAppoinmentModel()
            {
                AppointmentId = appointment.Id,
                PatientName = $"{appointment.Patient.LastName} {appointment.Patient.FirstName}",
                DoctorName = appointment.Doctor == null ? "" : $"{appointment.Doctor.LastName} {appointment.Doctor.FirstName}",
                Date = appointment.Diagnosis == null ? null : appointment.Diagnosis.DiagnosisDate,
                Desciption = appointment.Diagnosis == null ? "" : appointment.Diagnosis.Description,
                Status = appointment.Status.ToString()
            };

            return Ok(result);
        }
    }
}
