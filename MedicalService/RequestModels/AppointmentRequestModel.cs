using Asm2.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalService.RequestModels
{
    public class AppointmentRequestModel
    {
        //[Required(ErrorMessage = "PatientId can not be empty!")]
        //public string PatientId { get; set; }
        [Required(ErrorMessage = "DoctorId can not be empty!")]
        public string DoctorId { get; set; }
        [Required(ErrorMessage = "Appointment date can not be empty!")]
        [ValidateDateFormat]
        [ValidateDateTimeValue]
        public string AppointmentDate { get; set; }

        // description of the appointment, e.g., reason for visit
        public string Description { get; set; }
    }
}
