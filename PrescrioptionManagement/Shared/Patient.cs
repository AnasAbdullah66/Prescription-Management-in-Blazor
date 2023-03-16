using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PrescrioptionManagement.Server.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PrescrioptionManagement.Shared
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string? PatientName { get; set; }
        public int PatientAge { get; set; }
        public string? PatientPicture { get; set; }


        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Prescribed Date")]
        public DateTime PrescribedDate { get; set; }

        public bool Followup { get; set; }

        public string? DoctorName { get; set; }

        //nav
        public virtual ICollection<TestEntry>? TestEntries { get; set; } = new List<TestEntry>();

    }
}
