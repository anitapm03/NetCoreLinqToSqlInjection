using Microsoft.AspNetCore.Mvc;
using NetCoreLinqToSqlInjection.Repositories;
using NetCoreLinqToSqlInjection.Models;
using System.Reflection.Metadata.Ecma335;

namespace NetCoreLinqToSqlInjection.Controllers
{
    public class DoctoresController : Controller
    {
        private IRepositoryDoctores repo;

        public DoctoresController(IRepositoryDoctores repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {
            List<Doctor> doctores = this.repo.GetDoctores();
            return View(doctores);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Doctor doctor)
        {
            this.repo.InsertarDoctor(doctor.IdDoctor, doctor.Apellido,
                doctor.Especialidad, doctor.Salario, doctor.IdHospital);
            return RedirectToAction("Index");
        }

        public IActionResult DoctoresEspecialidad()
        {
            List<Doctor> doctores = this.repo.GetDoctores();
            return View(doctores);
        }

        [HttpPost]
        public IActionResult DoctoresEspecialidad(string especialidad)
        {
            List<Doctor> doctores = this.repo.GetDoctoresEspecialidad(especialidad);
            if(doctores == null)
            {
                ViewData["MENSAJE"] = "No existen doctores con esa especialidad";
                return View();
            }
            else
            {
                return View(doctores);
            }
        }

        public IActionResult Delete(int iddoctor)
        {
            this.repo.DeleteDoctor(iddoctor);
            return RedirectToAction("Index");
        }

        public IActionResult ModificarDoctor(int idDoctor)
        {
            Doctor doc = this.repo.FindDoctor(idDoctor);
            return View(doc); 
        }

        [HttpPost]
        public IActionResult ModificarDoctor(Doctor doc)
        {
            //Doctor doc =
            return RedirectToAction("Index");
        }
    }
}
