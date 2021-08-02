
using Bulkins.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkins.Dal.Repositories.Abstract
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> GetTopUsers(int count);
        IEnumerable<User> GetAllUsers();
        void BulkInsert(List<User> users);
        void Update(User user, string value);
        int BulkUpdate(int count);
    }
}
