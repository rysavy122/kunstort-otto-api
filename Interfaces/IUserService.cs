using System;

namespace App.Interfaces;

public interface IUserService
{
    Task SaveUserAsync(string email, string role);
    Task DeleteUserAsync(string email, string role);

}
