using System.Text.Json;
using System.Web;
using ProjetoCarequinha.Models;

namespace ProjetoCarequinha.Services
{
    public class VideoAulaService
    {
        private readonly string _jsonPath;

        public VideoAulaService(IWebHostEnvironment env)
        {
            _jsonPath = Path.Combine(env.WebRootPath, "Data", "aulas.json");
            if (!File.Exists(_jsonPath))
                File.WriteAllText(_jsonPath, "[]"); // inicializa vazio
        }

        public List<VideoAula> GetAll()
        {
            var json = File.ReadAllText(_jsonPath);
            var aulas = JsonSerializer.Deserialize<List<VideoAula>>(json) ?? new List<VideoAula>();

            return aulas.OrderBy(a => a.Id).ToList(); // ordena sempre pelo Id crescente
        }

        public void SaveAll(List<VideoAula> aulas)
        {
            var aula_class = aulas.OrderBy(a => a.Id).ToList();
            var json = JsonSerializer.Serialize(aula_class, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_jsonPath, json);
        }
        // Método para extrair o Index da URL
        private int GetIndexFromUrl(string url)
        {
            var uri = new Uri(url);
            var query = HttpUtility.ParseQueryString(uri.Query);

            string[] keys = query.AllKeys;
            for (int i = keys.Length - 1; i >= 0; i--)
            {
                if (string.Equals(keys[i], "Index", StringComparison.OrdinalIgnoreCase))
                {
                    if (int.TryParse(query[keys[i]], out int indexValue))
                        return indexValue;
                }
            }
            return 0; // valor padrão caso não encontre
        }

        public void Add(VideoAula aula)
        {
            var aulas = GetAll();

            // Recupera o Id a partir do Index da URL
            if (!string.IsNullOrEmpty(aula.YouTubeUrl))
                aula.Id = GetIndexFromUrl(aula.YouTubeUrl);
            else
                aula.Id = aulas.Count > 0 ? aulas.Max(a => a.Id) + 1 : 1;

            // Atualiza se já existir, ou adiciona se não existir
            var existingIndex = aulas.FindIndex(a => a.Id == aula.Id);
            if (existingIndex != -1)
            {
                aulas[existingIndex] = aula; // Atualiza
            }
            else
            {
                aulas.Add(aula); // Adiciona novo
            }

            SaveAll(aulas);
        }

        public void Update(VideoAula aula)
        {
            var aulas = GetAll();
            var index = aulas.FindIndex(a => a.Id == aula.Id);
            if (index != -1)
            {
                aulas[index] = aula;
                SaveAll(aulas);
            }
        }

        public void Delete(int id)
        {
            var aulas = GetAll();
            aulas.RemoveAll(a => a.Id == id);
            SaveAll(aulas);
        }
    }
}
