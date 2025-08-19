using System.Text.Json;
using ProjetoCarequinha.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ProjetoCarequinha.Services
{
    public class VideoService
    {
        private readonly IMemoryCache _cache;
        private readonly IWebHostEnvironment _env;
        private const string CacheKey = "VideoAulas";

        public VideoService(IMemoryCache cache, IWebHostEnvironment env)
        {
            _cache = cache;
            _env = env;
        }

        public List<VideoAula> GetAll()
        {
            if (!_cache.TryGetValue(CacheKey, out List<VideoAula> aulas))
            {
                var filePath = Path.Combine(_env.WebRootPath, "Data", "aulas.json");
                var json = File.ReadAllText(filePath);
                aulas = JsonSerializer.Deserialize<List<VideoAula>>(json) ?? new List<VideoAula>();

                _cache.Set(CacheKey, aulas, TimeSpan.FromHours(1));
            }
            return aulas;
        }

        public VideoAula? GetById(int id) => GetAll().FirstOrDefault(v => v.Id == id);
    }
}
