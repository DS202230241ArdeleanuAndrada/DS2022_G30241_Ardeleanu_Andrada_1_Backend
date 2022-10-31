using App.Data.Repositories;
using System.Data;

namespace App.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IUserDeviceRepository UserDeviceRepository { get; }
        IUserRepository UserRepository { get; }
        void Commit();
        void Rollback();
        public IDbTransaction CurrentTransaction { get; }
        TRepository GetRepository<TIRepository, TRepository>()
           where TRepository : TIRepository;
    }
}
