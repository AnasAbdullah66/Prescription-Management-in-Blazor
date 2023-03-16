using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using PrescrioptionManagement.Server.Models;

namespace PrescrioptionManagement.Shared
{
    public class PrescriptionVM
    {
        public int PatientId { get; set; }
        public string? PatientName { get; set; }
        public int PatientAge { get; set; }
        public string? PatientPicture { get; set; }

        public IFormFile? PictureFile { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Prescribed Date")]
        public DateTime PrescribedDate { get; set; }

        public bool Followup { get; set; }

        public string? DoctorName { get; set; }

        //
        public List<MedicalTest> TestList { get; set; } = new List<MedicalTest>();
        public virtual ICollection<TestEntry>? TestEntrys { get; set; } = new List<TestEntry>();
    }
}
