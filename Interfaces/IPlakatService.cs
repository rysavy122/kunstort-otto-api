using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Services;
using App.Models.DTOs;
using PlakatDto = App.Models.DTOs.PlakatDto;

namespace App.Interfaces
{
public interface IPlakatService
    {
        Task<PlakatDto> GetPlakatById(int plakatId);
        Task<PlakatDto> CreatePlakat(PlakatDto plakatDto);
        Task<bool> UpdatePlakat(int plakatId, string drawingJson);
        Task<bool> DeletePlakat(int plakatId);
        Task<bool> AddSticker(int plakatId, StickerDto stickerDto);
    }
}