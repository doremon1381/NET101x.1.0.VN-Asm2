using MedicalModel;
using System.ComponentModel.DataAnnotations;

namespace Asm2.DataTransferModels.Medical
{
    public class DoctorModel : PersonModel
    {
        public string ProfileImg { get; set; }

        public string[] Specialty { get; set; }
        public string[] SubSpecialties { get; set; }
        public string ExperienceInYrs { get; set; }
    }

    public class PatientModel : PersonModel
    {

    }

    public abstract class PersonModel 
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string HospitalName { get; set; }
    }
}
