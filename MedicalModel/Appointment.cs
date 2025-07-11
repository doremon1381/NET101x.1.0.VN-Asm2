using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalModel
{
    public class Appointment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        //public DateTime AppointmentDate { get; set; }
        // description of the appointment, e.g., reason for visit
        public string Description { get; set; }
        // status of the appointment, e.g., confirmed, cancelled, etc.
        public AppointmentStatus Status { get; set; }
        public bool IsDeleted { get; set; } = false;
        public BookingSchedule BookingSchedule { get; set; }

        public string PatientId { get; set; } // foreign key to the patient
        [ForeignKey(nameof(PatientId))]
        public Person Patient { get; set; } // navigation property to the patient

        public Diagnosis Diagnosis { get; set; }

        public string DoctorId { get; set; } // foreign key to the doctor
        [ForeignKey(nameof(DoctorId))]
        public Person Doctor { get; set; } // navigation property to the doctor
    }

    public class BookingSchedule
    {
        public DateTime Start { get; set; }
        public DateTime? End { get; set; } // at the creation time, the end of this appointment may not be decided
        public DateTime ExpireAt { get; set; }
        public AppointmentPriority Priority { get; set; }

    }

    public enum AppointmentStatus
    {
        Confirmed,
        Handling, // a meeting with doctor is on going
        Cancelled,
        Rescheduled,
        Completed
    }

    public enum AppointmentPriority 
    {
         Normal,
         Urgent 
    }
}
