using System;
using System.Collections.Generic;

namespace MedicalModel
{
    public class Hospital
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Region { get; set; } // region where the hospital is located
        public string Country { get; set; } // country where the hospital is located
        public bool IsDeleted { get; set; } = false;

        public List<Person> Doctor { get; set; } = new List<Person>();
    }
}
