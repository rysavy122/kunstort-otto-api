using System.Collections.Generic;
using System.Threading.Tasks;
using App.Models;

namespace App.Interfaces
{
    public interface IKommentarService
    {
        Task<IEnumerable<Kommentar>> GetAllKommentare();
        Task<Kommentar> GetKommentarById(int id);
        Task<Kommentar> AddKommentar(Kommentar kommentar);
        Task<string> AddMedia(IFormFile media, int forschungsfrageId);
        Task<IEnumerable<string>> GetMediaByForschungsfrageId(int forschungsfrageId);
        Task<bool> DeleteMedia(string filename);
        Task<bool> DeleteKommentar(int id);

    }
}
