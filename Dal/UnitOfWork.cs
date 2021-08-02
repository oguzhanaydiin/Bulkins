
using Bulkins.Dal.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkins.Dal
{
    public class UnitOfWork : IUnitOfWork
    {
        private DatabaseContext _databaseContext;

        public UnitOfWork(DatabaseContext context)
        {
            _databaseContext = context;
            UserRepository = new Repositories.Concrete.UserRepository(_databaseContext);
        }
        public IUserRepository UserRepository { get; private set; }

        public int Complete()
        {
            return _databaseContext.SaveChanges();
        }

        public void Dispose()
        {
            _databaseContext.Dispose();
        }
    }
}
