using App.Models;
using App.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Services
{
    public interface IUserService
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task<IEnumerable<User>> GetAllUsers();
        Task<int> Create(CreateUserRequest request);
        Task<int> Update(UpdateUserRequest request);
        Task Delete(DeleteUserRequest request);
    }
}
