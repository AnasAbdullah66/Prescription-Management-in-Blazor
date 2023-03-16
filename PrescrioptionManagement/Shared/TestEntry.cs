using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using PrescrioptionManagement.Shared;

namespace PrescrioptionManagement.Server.Models
{
    public class TestEntry
    {
        public int TestEntryId { get; set; }
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        [ForeignKey("MedicalTest")]
        public int MedicalTestId { get; set; }


        //nav
        public virtual Patient? Patient { get; set; }
        public virtual MedicalTest? MedicalTest { get; set; }
    }
}
