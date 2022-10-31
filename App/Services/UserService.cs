using App.Data;
using App.Models;
using App.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;

        public UserService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            return await _uow.UserRepository.Login(request);
        }

        public async Task<int> Create(CreateUserRequest request)
        {
            var result = await _uow.UserRepository.Create(request);
            _uow.Commit();
            return result;
        }

        public async Task<int> Update(UpdateUserRequest request)
        {
            var result = await _uow.UserRepository.Update(request);
            _uow.Commit();
            return result;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var result = await _uow.UserRepository.GetAllUsers();
            return result;
        }

        public async Task Delete(DeleteUserRequest request)
        {
            await _uow.UserRepository.Delete(request);
            _uow.Commit();
        }
    }
}
