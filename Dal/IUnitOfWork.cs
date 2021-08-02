
using Bulkins.Dal.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkins.Dal
{
    public interface IUnitOfWork: IDisposable
    {
        IUserRepository UserRepository { get;  }

        int Complete();
    }
}
