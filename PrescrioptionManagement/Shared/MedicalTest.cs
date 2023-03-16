using PrescrioptionManagement.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrescrioptionManagement.Shared
{
    public class MedicalTest
    {
        public int MedicalTestId { get; set; }
        public string? MedicalTestName { get; set; }



        //nav
        public virtual ICollection<TestEntry> TestEntries { get; set; } = new List<TestEntry>();
    }
}
