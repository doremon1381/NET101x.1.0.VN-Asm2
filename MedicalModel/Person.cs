using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using Microsoft.EntityFrameworkCore.Abstraction;

namespace MedicalModel
{
    public class Person
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public Gender Gender { get; set; }
        public string Nationality { get; set; }
        public bool IsDeleted { get; set; } = false;

        [Required]
        public List<PersonRole> Roles { get; set; } // doctor or patient

        // additional information for doctors, e.g., profile image URL and role, if this person is a doctor
        public DoctorInfo DoctorInfo { get; set; }

        public List<Appointment> DoctorAppointments { get; set; } = new List<Appointment>();
        public List<Appointment> PatientAppointments { get; set; } = new List<Appointment>();
    }

    //[Owned]
    public class DoctorInfo
    {
        // URL to the profile image of the person
        public string ProfileImageUrl { get; set; }
        // specialty of the doctor, e.g., cardiology, neurology, etc.
        // A list of specialties can be stored as a comma-separated string or a separate table if needed.
        public string Specialty { get; set; }
        // sub-specialties of the doctor, e.g., interventional cardiology, pediatric neurology, etc.
        public string SubSpecialties { get; set; }
        public double? ExperienceInYrs { get; set; }
        // position of the doctor at the hospital, e.g., attending physician, resident, etc.
        public string PositionAtHospital { get; set; }
        // if this person is a doctor, this is the hospital they are assoiated with
        public string? HospitalId { get; set; }
        [ForeignKey(nameof(HospitalId))]
        public Hospital? Hospital { get; set; } // navigation property to the hospital if this person is a doctor
    }

    //public class DoctorContract
    //{
    //    public string Id { get; set; } = Guid.NewGuid().ToString();
    //    public string DoctorId { get; set; }
    //    [ForeignKey(nameof(DoctorId))]
    //    public Person Doctor { get; set; }

    //    //public string ContractDetails { get; set; } // details of the contract, e.g., salary, benefits, etc.
    //    //public DateTime StartDate { get; set; } // start date of the contract
    //    //public DateTime EndDate { get; set; } // end date of the contract, if null, the contract is ongoing

    //    public string PositionAtHospital { get; set; } // position of the doctor at the hospital, e.g., attending physician, resident, etc.
    //    public string HospitalId { get; set; }
    //    [ForeignKey(nameof(HospitalId))]
    //    public Hospital Hospital { get; set; }
    //}

    public enum Gender
    {
        Male,
        Female,
        Other
    }

    public enum PersonRole
    {
        Doctor,
        Patient,
        Guess
    }
}
