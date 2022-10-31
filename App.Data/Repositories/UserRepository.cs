using App.Models;
using App.Models.Entities;
using Dapper;
using System.Collections;
using System.Data;

namespace App.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        protected IDbTransaction Transaction { get; private set; }
        protected IDbConnection Connection { get { return Transaction.Connection; } }
        public UserRepository(IDbTransaction transaction)
        {
            Transaction = transaction;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var parameters = new DynamicParameters(new
            {
                Username = request.Username,
                Password = request.Password
            });

            var result = await Connection.QueryFirstAsync<LoginResponse>(
              "SP_User_GetRole",
              param: parameters,
              commandType: CommandType.StoredProcedure,
              commandTimeout: 60,
              transaction: Transaction
            );

            return result;
        }

        public async Task<int> Create(CreateUserRequest request)
        {
            var parameters = new DynamicParameters(new
            {
                Name = request.Name,
                Username = request.Username,
                Password = request.Password
            });

            var result = await Connection.QueryAsync<int>(
              "SP_User_Create",
              param: parameters,
              commandType: CommandType.StoredProcedure,
              commandTimeout: 60,
              transaction: Transaction
            );

            return result.FirstOrDefault();
        }

        public async Task<int> Update(UpdateUserRequest request)
        {
            var parameters = new DynamicParameters(new
            {
                Id=request.Id,
                Name = request.Name,
                Username = request.Username,
                Password = request.Password
            });

            var result = await Connection.QueryAsync<int>(
              "SP_User_Update",
              param: parameters,
              commandType: CommandType.StoredProcedure,
              commandTimeout: 60,
              transaction: Transaction
            );

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var result = await Connection.QueryAsync<User>(
              "SP_User_GetAllUsers",
              commandType: CommandType.StoredProcedure,
              commandTimeout: 60,
              transaction: Transaction
            );

            return result;
        }

        public async Task Delete(DeleteUserRequest request)
        {
            var parameters = new DynamicParameters(new
            {
                Id = request.Id
            });

            await Connection.QueryAsync<int>(
              "SP_User_Delete",
              param: parameters,
              commandType: CommandType.StoredProcedure,
              commandTimeout: 60,
              transaction: Transaction
            );
        }
    }
}
