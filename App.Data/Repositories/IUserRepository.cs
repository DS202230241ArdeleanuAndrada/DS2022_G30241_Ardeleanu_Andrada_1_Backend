using App.Models;
using App.Models.Entities;
using System.Collections;

namespace App.Data.Repositories
{
    public interface IUserRepository
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task<IEnumerable<User>> GetAllUsers();
        Task<int> Create(CreateUserRequest request);
        Task<int> Update(UpdateUserRequest request);
        Task Delete(DeleteUserRequest request);
    }
}
