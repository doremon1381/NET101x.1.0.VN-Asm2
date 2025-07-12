using System.ComponentModel.DataAnnotations;

namespace Asm2.DataTransferModels.Medical
{
    public class FindDoctorModel
    {
        [Required(ErrorMessage = "Country can not be empty")]
        public string CountryCode { get; set; }
        [Required(ErrorMessage = "Doctor's first name can not be empty")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Doctor's last name can not be empty")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "City can not be empty")]
        public string City { get; set; }
        //[Required(ErrorMessage = "Specialty can not be empty")]
        public string Specialty { get; set; }
        //[Required(ErrorMessage = "Hospital name can not be empty")]
        public string HospitalName { get; set; }
    }
}
