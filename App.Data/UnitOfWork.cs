using App.Data.Repositories;
using System.Data;

namespace App.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        private bool _disposed;

        private IUserRepository _userRepository;
        private IUserDeviceRepository _userDeviceRepository;

        public UnitOfWork(IDbConnection dbConnection)
        {
            _connection = dbConnection;
            _transaction = _connection.BeginTransaction();
        }

        public TRepository GetRepository<TIRepository, TRepository>()
            where TRepository : TIRepository
        {
            object[] args = new object[] { _transaction };
            return (TRepository)Activator.CreateInstance(typeof(TRepository), args);
        }

        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_transaction);
        public IUserDeviceRepository UserDeviceRepository => _userDeviceRepository ??= new UserDeviceRepository(_transaction);

        public IDbTransaction CurrentTransaction => _transaction;

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                Rollback();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = _connection.BeginTransaction();
                    ResetRepositories();
                }
            }
        }

        public void Rollback()
        {
            try
            {
                _transaction?.Rollback();
            }
            finally
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = _connection.BeginTransaction();
                    ResetRepositories();
                }
            }
        }

        private void ResetRepositories()
        {
            _userRepository = null;
            _userDeviceRepository = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
