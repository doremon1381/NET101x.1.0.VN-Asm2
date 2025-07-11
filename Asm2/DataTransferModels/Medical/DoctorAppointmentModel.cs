using MedicalModel;
using System;

namespace Asm2.DataTransferModels.Medical
{
    public class DoctorAppointmentModel
    {
        public string PatientName { get; set; }
        public string AppointmentId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DateTime ExpireAt { get; set; }
        public string Status { get; set; }
    }

    public class PatientAppoinmentModel
    {
        public string AppointmentId { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public DateTime? Date { get; set; }
        public string Status { get; set; }
        public string Desciption { get; set; }
    }
}
