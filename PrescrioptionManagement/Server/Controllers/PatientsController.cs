using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrescrioptionManagement.Server.Models;
using PrescrioptionManagement.Shared;

namespace PrescrioptionManagement.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly PrescriptionDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PatientsController(PrescriptionDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this._env = env;
        }
        [HttpGet]
        [Route("GetTests")]
        public async Task<ActionResult<IEnumerable<MedicalTest>>> GetTests()
        {
            return await _context.MedicalTests.ToListAsync();
        }


        [HttpGet]
        [Route("GetPatients")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetClients()
        {
            return await _context.Patients.Include(c => c.TestEntries).ThenInclude(b => b.MedicalTest).ToListAsync();
          
        }

        [HttpGet("{id}")]
        public async Task<Patient> GetPatientById(int id)
        {
            return await _context.Patients.Where(x => x.PatientId == id).Include(c => c.TestEntries).ThenInclude(b => b.MedicalTest).FirstOrDefaultAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PrescriptionVM prescriptionVM)
        {

            if (ModelState.IsValid)
            {
                Patient patient = new Patient()
                {
                    PatientName = prescriptionVM.PatientName,
                    PatientAge = prescriptionVM.PatientAge,
                    PrescribedDate = prescriptionVM.PrescribedDate,
                    DoctorName = prescriptionVM.DoctorName,
                    Followup = prescriptionVM.Followup
                };

                //Image
                if (prescriptionVM.PictureFile is not null)
                {
                    string webroot = _env.WebRootPath;
                    string folder = "Images";
                    string imgFileName = Guid.NewGuid().ToString() + Path.GetExtension(prescriptionVM.PictureFile.FileName);
                    string fileToWrite = Path.Combine(webroot, folder, imgFileName);

                    using (var stream = new FileStream(fileToWrite, FileMode.Create))
                    {
                        await prescriptionVM.PictureFile.CopyToAsync(stream);
                        patient.PatientPicture = imgFileName;
                    }
                }
                else
                {
                    patient.PatientPicture = "default.jpg";
                }
                _context.Patients.Add(patient);

                if (prescriptionVM.TestList.Count() > 0)
                {
                    foreach (MedicalTest test in prescriptionVM.TestList)
                    {
                        _context.TestEntries.Add(new TestEntry
                        {
                            Patient = patient,
                            PatientId = patient.PatientId,
                            MedicalTestId = test.MedicalTestId
                        });
                    }
                }


                await _context.SaveChangesAsync();
                return Ok(patient);
            }
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] PrescriptionVM prescriptionVM)
        {

            if (ModelState.IsValid)
            {
                Patient patient = _context.Patients.Find(prescriptionVM.PatientId);
                patient.PatientName = prescriptionVM.PatientName;
                patient.PatientAge = prescriptionVM.PatientAge;
                patient.PrescribedDate = prescriptionVM.PrescribedDate;
                patient.DoctorName = prescriptionVM.DoctorName;
                patient.Followup = prescriptionVM.Followup;


                //Image
                if (prescriptionVM.PictureFile is not null)
                {
                    string webroot = _env.WebRootPath;
                    string folder = "Images";
                    string imgFileName = Guid.NewGuid().ToString() + Path.GetExtension(prescriptionVM.PictureFile.FileName);
                    string fileToWrite = Path.Combine(webroot, folder, imgFileName);

                    using (var stream = new FileStream(fileToWrite, FileMode.Create))
                    {
                        await prescriptionVM.PictureFile.CopyToAsync(stream);
                        patient.PatientPicture = imgFileName;
                    }
                }

                var testExists = _context.TestEntries.Where(x => x.PatientId == prescriptionVM.PatientId).ToList();
                if (testExists is not null)
                {
                    foreach (var entry in testExists)
                    {
                        _context.Remove(entry);
                    }
                }


                if (prescriptionVM.TestList.Count() > 0)
                {
                    foreach (MedicalTest test in prescriptionVM.TestList)
                    {
                        _context.TestEntries.Add(new TestEntry
                        {
                            PatientId = patient.PatientId,
                            MedicalTestId = test.MedicalTestId
                        });
                    }
                }

                _context.Entry(patient).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(patient);
            }
            return NotFound();
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            Patient patient = _context.Patients.Find(id);

            var testExists = _context.TestEntries.Where(x => x.PatientId == patient.PatientId).ToList();
            if (testExists is not null)
            {
                foreach (var entry in testExists)
                {
                    _context.Remove(entry);
                }
            }
            _context.Remove(patient);
            try
            {
                await _context.SaveChangesAsync();

                return new OkObjectResult(patient);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }


    }
}
