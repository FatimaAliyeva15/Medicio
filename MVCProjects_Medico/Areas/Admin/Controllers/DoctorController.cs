using Microsoft.AspNetCore.Mvc;
using MVCProjects_MedicoBusiness.Exceptions.Doctor;
using MVCProjects_MedicoBusiness.Services.Abstracts;
using MVCProjects_MedicoCore.Models;
using FileNotFoundException = MVCProjects_MedicoBusiness.Exceptions.Doctor.FileNotFoundException;

namespace MVCProjects_Medico.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DoctorController : Controller
    {

        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        public IActionResult Index()
        {
            List<Doctor> doctors = _doctorService.GetAllDoctors();
            return View(doctors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Doctor doctor)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                _doctorService.AddDoctor(doctor);
            }
            catch(FileContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch(FileSizeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var existDoctor = _doctorService.GetDoctor(x => x.Id == id);
            if (existDoctor == null) return NotFound();

            try
            {
                _doctorService.DeleteDoctor(id);
            }
            catch(EntityNotFoundException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch(FileNotFoundException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            var doctor = _doctorService.GetDoctor(x => x.Id == id);
            if (doctor == null) return NotFound();

            return View(doctor);
        }

        [HttpPost]
        public IActionResult Update(int id, Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                _doctorService.UpdateDoctor(id, doctor);
            }
            catch (EntityNotFoundException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (FileContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (FileSizeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch(NullReferenceException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
