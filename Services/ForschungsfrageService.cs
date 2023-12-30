using System;
using App.Data;
using App.Models;
namespace App.Interfaces
{
	public class ForschungsfrageService : IForschungsfrageService
	{

    private readonly OttoDbContext? _context;

        public ForschungsfrageService(OttoDbContext context)
        {
            _context = context;
        }

        // CREATE FORSCHUNGSFRAGE
        public Forschungsfrage CreateForschungsfrage(Forschungsfrage forschungsfrage)
        {
            if (_context == null || forschungsfrage == null)
            {
                return null;
            }

            _context.Forschungsfragen.Add(forschungsfrage);
            _context.SaveChanges();
            return forschungsfrage;
        }

        //GET ALL FORSCHUNGSFRAGEN
        public IEnumerable<Forschungsfrage> GetAllForschungsfragen()
        {
            return _context?.Forschungsfragen.ToList() ?? Enumerable.Empty<Forschungsfrage>();
        }


        // GET SINGLE FORSCHUNGSFRAGE
        public Forschungsfrage? GetForschungsfrageById(int id)
        {
            return _context?.Forschungsfragen.FirstOrDefault(m => m.Id == id);
        }
        //GET LATEST FORSCHUNGSFRAGE
        public Forschungsfrage GetLatestForschungsfrage()
        {
            return _context?.Forschungsfragen.OrderByDescending(m => m.Id).FirstOrDefault();
        }

        // EDIT FORSCHUNGSFRAGE
        public Forschungsfrage UpdateForschungsfrage(int id, Forschungsfrage forschungsfrage)
        {
            var existingForschungsfrage = _context?.Forschungsfragen.FirstOrDefault(f => f.Id == id);
            if (existingForschungsfrage != null)
            {
                existingForschungsfrage.Title = forschungsfrage.Title;
                _context?.SaveChanges();
            }
            return existingForschungsfrage;
        }


        // DELETE FORSCHUNGSFRAGE
        public void DeleteForschungsfrage(int id)
        {
            var forschungsfrage = _context?.Forschungsfragen.FirstOrDefault(m => m.Id == id);
            if (forschungsfrage != null)
            {
                _context.Forschungsfragen.Remove(forschungsfrage);
                _context.SaveChanges();
            }
        }
    }
}
