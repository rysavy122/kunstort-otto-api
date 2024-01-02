using System.Collections.Generic;
using System.Threading.Tasks;
using App.Models;

namespace App.Interfaces
{
    public interface IKommentarService
    {
        Task<IEnumerable<Kommentar>> GetAllKommentare();
        Task<Kommentar> AddKommentar(Kommentar kommentar);
        Task<Kommentar> GetKommentarById(int id);

    }
}
