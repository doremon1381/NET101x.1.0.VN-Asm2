using MedicalModel;
using MedicalService.RequestModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalService
{
    public class MedicalServices : IMedicalServices
    {
        private readonly MedicalDbContext _medicalDbContext;

        public MedicalServices(MedicalDbContext medicalDbContext)
        {
            // Constructor logic if needed
            // from dependecy injection
            if (medicalDbContext != null)
            {
                _medicalDbContext = medicalDbContext;
            }
            else
            {
                // for general case
                var options = new DbContextOptionsBuilder<MedicalDbContext>().Options;
                _medicalDbContext = new MedicalDbContext(options);
            }
        }

        public async Task AddPersonAsync(Person person)
        {
            _medicalDbContext.Persons.Add(person);
            await _medicalDbContext.SaveChangesAsync();
        }

        public async Task<Appointment> CreateAppointmentAsync(AppointmentRequestModel appointment, string patientId)
        {
            var doctor = _medicalDbContext.Persons.FirstOrDefault(p => p.Id == appointment.DoctorId);
            if (doctor == null)
                throw new Exception($"There is doctor with this id: {appointment.DoctorId}");

            // TODO: check if there is one or more appointment in this day or 
            var duplicateAppointments = _medicalDbContext.Appointments.Where(a => a.PatientId == patientId
                    && a.DoctorId == doctor.Id
                    && (a.BookingSchedule.Start.AddHours(-12) < DateTime.Parse(appointment.AppointmentDate)
                        || a.BookingSchedule.Start.AddHours(12) > DateTime.Parse(appointment.AppointmentDate))).ToList();
            if (duplicateAppointments.Any())
                throw new Exception("We're already set for you an appointment around that time, one appointment with its next can not be too close!");

            var newAppointment = new Appointment
            {
                DoctorId = doctor.Id,
                Description = appointment.Description,
                Status = AppointmentStatus.Confirmed,
                PatientId = patientId,
                BookingSchedule = new BookingSchedule()
                {
                    Start = DateTime.Parse(appointment.AppointmentDate),
                    ExpireAt = DateTime.Parse(appointment.AppointmentDate).AddDays(2),
                    Priority = AppointmentPriority.Normal,
                    End = null
                }
            };

            _medicalDbContext.Appointments.Add(newAppointment);

            await _medicalDbContext.SaveChangesAsync();

            return newAppointment;
        }

        public async Task<Appointment> FindAppointmentByIdAsync(string id)
        {
            var appointment = await _medicalDbContext.Appointments
                .Include(a => a.Patient).Include(a => a.Diagnosis)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            return appointment;
        }

        public async Task<Appointment> FindAppointmentForPatientByIdAsync(string appointmentId, string patientEmail)
        {
            var patient = _medicalDbContext.Persons.FirstOrDefault(p => p.Email == patientEmail);
            if (patient == null)
                throw new Exception($"Patient can not be found with this email: {patientEmail}");

            var appointment = await _medicalDbContext.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Diagnosis)
                .FirstOrDefaultAsync(x => x.Id == appointmentId 
                                       && x.PatientId == patient.Id
                                       && x.IsDeleted == false);

            return appointment;
        }

        public async Task<Person> FindDoctorByIdAsync(string doctorId)
        {
            var doctor = await _medicalDbContext.Persons.Include(p => p.DoctorInfo)
                .Where(p => ((string)(object)p.Roles).Contains(PersonRole.Doctor.ToString())
                            && p.IsDeleted == false)
                .FirstOrDefaultAsync(p => p.Id.Equals(doctorId));

            return doctor;
        }

        public async Task<List<Person>> FindDoctorsFromParticularRegionAsync(string doctorFirstName, string doctorLastName, string hospitalName, string specialty, string region, string countryCode = "VN")
        {
            var doctors = await _medicalDbContext.Persons
                .Include(p => p.DoctorInfo.Hospital)
                .Where(p => ((string)(object)p.Roles).Contains(PersonRole.Doctor.ToString())
                         && p.DoctorInfo.Hospital.Country.ToUpper().Contains(countryCode.ToUpper())
                         && p.DoctorInfo.Hospital.Region.ToUpper().Contains(region.ToUpper())
                         && p.IsDeleted == false)
                .ToListAsync();

            var result = doctors.Where(p => p.FirstName.ToLower().Equals(doctorFirstName.ToLower().Trim())
                         && p.LastName.ToLower().Equals(doctorLastName.ToLower().Trim())
                         && p.DoctorInfo.Specialty.ToLower().Contains(specialty.ToLower().Trim())).ToList();

            return result;
        }

        public async Task<Person> FindPatientByEmailAsync(string email)
        {
            return await _medicalDbContext.Persons.FirstOrDefaultAsync(p => p.Email.Equals(email) && p.IsDeleted == false);
        }

        public async Task<Person> FindPatientByIdAsync(string patientId)
        {
            return await _medicalDbContext.Persons
                .FirstOrDefaultAsync(p => p.Id == patientId
                                       && p.IsDeleted == false);
        }

        public async Task<List<PersonRole>> FindPersonRolesAsync(string id)
        {
            var person = await _medicalDbContext.Persons.FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);

            return person.Roles;
        }

        public async Task<List<Hospital>> GetAllHospitalInCountryAsync(string region, string countryCode = "VN")
        {
            // TODO: by default, inside this database only has Vietnam hospitals
            return await _medicalDbContext.Hospitals
                .Where(h => h.Country.ToUpper().Equals(countryCode.ToUpper())
                         && h.Region.ToUpper().Equals(region.ToUpper()) && h.IsDeleted == false)
                .ToListAsync();
        }

        public Task<Person> GetUserByEmailAsync(string email)
        {
            return _medicalDbContext.Persons
                .FirstOrDefaultAsync(p => p.Email.Equals(email) && p.IsDeleted == false);
        }
    }

    public interface IMedicalServices
    {
        Task AddPersonAsync(Person person);
        Task<Appointment> CreateAppointmentAsync(AppointmentRequestModel appointment, string patientId);
        Task<Person> FindDoctorByIdAsync(string doctorId);
        Task<Person> FindPatientByIdAsync(string patientId);
        Task<List<Person>> FindDoctorsFromParticularRegionAsync(string doctorFirstName, string doctorLastName, string hospitalName, string specialty, string city, string country);
        Task<List<Hospital>> GetAllHospitalInCountryAsync(string region, string countryCode);
        Task<Person> GetUserByEmailAsync(string email);
        Task<List<PersonRole>> FindPersonRolesAsync(string id);
        Task<Person> FindPatientByEmailAsync(string email);
        Task<Appointment> FindAppointmentByIdAsync(string id);
        Task<Appointment> FindAppointmentForPatientByIdAsync(string appointmentId, string patientEmail);
    }
}
