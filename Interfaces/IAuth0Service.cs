using System;
namespace App.Interfaces
{
    public interface IAuth0Service
    {
        Task<string> GetManagementApiTokenAsync();
    }
}

