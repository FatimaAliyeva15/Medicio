using Microsoft.AspNetCore.Mvc;
using MVCProjects_MedicoBusiness.Services.Abstracts;
using MVCProjects_MedicoCore.Models;
using System.Diagnostics;

namespace MVCProjects_Medico.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDoctorService _service;

        public HomeController(IDoctorService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            List<Doctor> doctors = _service.GetAllDoctors();
            return View(doctors);
        }

    }
}