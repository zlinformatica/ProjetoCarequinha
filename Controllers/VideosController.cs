using Microsoft.AspNetCore.Mvc;
using ProjetoCarequinha.Services;
using ProjetoCarequinha.Models;

public class VideosController : Controller
{
    private readonly VideoService _videoService;

    public VideosController(VideoService videoService)
    {
        _videoService = videoService;
    }

    [HttpGet]
    public IActionResult Index([FromQuery] int? id, [FromQuery] string? search)
    {
        var aulas = _videoService.GetAll();
        var termo = (search ?? string.Empty).Trim();

        if (!string.IsNullOrEmpty(termo))
        {
            // Permite "1", "Aula 1", "aula 001", etc.
            var digitos = new string(termo.Where(char.IsDigit).ToArray());

            aulas = aulas
                .Where(a =>
                    // match por número (contém dígitos)
                    (!string.IsNullOrEmpty(digitos) && a.Id.ToString().Contains(digitos)) ||
                    // match por título (case-insensitive)
                    (!string.IsNullOrEmpty(a.Title) &&
                        a.Title.Contains(termo, StringComparison.CurrentCultureIgnoreCase))
                )
                .ToList();
        }

        // Se foi passado um id, seleciona-o; se pesquisou e não passou id, escolhe a 1ª da lista
        VideoAula? selecionada = null;
        if (id.HasValue)
            selecionada = aulas.FirstOrDefault(a => a.Id == id.Value);
        else if (!string.IsNullOrEmpty(termo))
            selecionada = aulas.FirstOrDefault();

        ViewBag.Aulas = aulas;
        ViewBag.Search = termo;

        return View(selecionada);
    }
}
