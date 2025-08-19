using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ProjetoCarequinha.Services;   // <- ESSENCIAL para enxergar VideoAulaService
using ProjetoCarequinha.Models;

namespace ProjetoCarequinha.Controllers
{
    public class VideoAulaAdminController : Controller
    {
        private readonly VideoAulaService _videoAulaService;

        public VideoAulaAdminController(VideoAulaService videoAulaService)
        {
            _videoAulaService = videoAulaService;
        }
        private bool IsAdmin() => HttpContext.Session.GetString("IsAdmin") == "true";

        public IActionResult Index()
        {
            if (!IsAdmin())
                return View("~/Views/Shared/AccessDenied.cshtml");

            var aulas = _videoAulaService.GetAll();
            return View(aulas);
        }

        public IActionResult Create()
        {
            if (!IsAdmin())
                return View("~/Views/Shared/AccessDenied.cshtml");

            return View();
        }

        [HttpPost]
        public IActionResult Create(VideoAula aula)
        {
            if (!IsAdmin())
                return View("~/Views/Shared/AccessDenied.cshtml");

            if (ModelState.IsValid)
            {
                _videoAulaService.Add(aula);
                return RedirectToAction(nameof(Index));
            }
            return View(aula);
        }

        public IActionResult Edit(int id)
        {
            if (!IsAdmin())
                return View("~/Views/Shared/AccessDenied.cshtml");

            var aula = _videoAulaService.GetAll().FirstOrDefault(a => a.Id == id);
            if (aula == null) return NotFound();
            return View(aula);
        }

        [HttpPost]
        public IActionResult Edit(VideoAula aula)
        {
            if (!IsAdmin())
                return View("~/Views/Shared/AccessDenied.cshtml");

            if (ModelState.IsValid)
            {
                _videoAulaService.Update(aula);
                return RedirectToAction(nameof(Index));
            }
            return View(aula);
        }

        public IActionResult Delete(int id)
        {
            if (!IsAdmin())
                return View("~/Views/Shared/AccessDenied.cshtml");

            _videoAulaService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
