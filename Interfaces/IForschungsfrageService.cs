using System;
using App.Models;
namespace App.Interfaces
{
	public interface IForschungsfrageService
	{
        IEnumerable<Forschungsfrage> GetAllForschungsfragen();
        Forschungsfrage GetForschungsfrageById(int id);
        Forschungsfrage GetLatestForschungsfrage();
        Task<Forschungsfrage> CreateForschungsfrage(Forschungsfrage forschungsfrage, IFormFile image);
        Forschungsfrage UpdateForschungsfrage(int id, Forschungsfrage forschungsfrage);
        Forschungsfrage UpdateBackgroundColor(int id, string backgroundColor);
        void DeleteForschungsfrage(int id);
    }
}

 