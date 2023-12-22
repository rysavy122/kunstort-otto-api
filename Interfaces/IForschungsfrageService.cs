using System;
using App.Models;

namespace App.Interfaces
{
	public interface IForschungsfrageService
	{
        IEnumerable<Forschungsfrage> GetAllForschungsfragen();
        Forschungsfrage GetForschungsfrageById(int id);
        Forschungsfrage CreateForschungsfrage(Forschungsfrage forschungsfrage);
        Forschungsfrage UpdateForschungsfrage(int id, Forschungsfrage forschungsfrage);
        void DeleteForschungsfrage(int id);
    }
}

