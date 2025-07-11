using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalModel
{

    public class Diagnosis
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Description { get; set; } // description of the diagnosis
        public DateTime DiagnosisDate { get; set; } // date of the diagnosis
        public bool IsDeleted { get; set; } = false;

        public string AppointmentId { get; set; } // foreign key to the appointment
        [ForeignKey(nameof(AppointmentId))]
        public Appointment Appointment { get; set; } // navigation property to the appointment
    }
}
