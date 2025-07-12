using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalModel
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public string Messsage { get; set; }
        public string MessageTemplate { get; set; }
        public string Level { get; set; }

        public DateTime TimeStamp { get; set; }
        public string Exception { get; set; }
        public string Properties { get; set; } // Xml property
        public string LogEvent { get; set; }
    }
}
