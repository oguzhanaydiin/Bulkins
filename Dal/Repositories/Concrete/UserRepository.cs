
using Bulkins.Dal.Repositories.Abstract;
using Bulkins.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkins.Dal.Repositories.Concrete
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext context): base(context)
        {
            
        }
        public IEnumerable<User> GetAllUsers()
        {
            return DatabaseContext.Users.ToList();
        }

        public IEnumerable<User> GetTopUsers(int count)
        {
            return DatabaseContext.Users.Take(count);
        }

        public void BulkInsert(List<User> users)
        {
            DatabaseContext.Users.AddRange(users);
        }

        public void Update(User user, string value)
        {
            try
            {
                DatabaseContext.Configuration.AutoDetectChangesEnabled = false;

                DatabaseContext.Users.Attach(user);

                user.Name = value;

                DatabaseContext.SaveChanges();
            }
            finally
            {
                DatabaseContext.Configuration.AutoDetectChangesEnabled = true;
            }
        }

        public int BulkUpdate(int count)
        {
            try
            {
                DatabaseContext.Configuration.AutoDetectChangesEnabled = false;

                var users = DatabaseContext.Users.Take(count);

                if (users.Count()<count)
                {
                    DatabaseContext.Configuration.AutoDetectChangesEnabled = true;
                    return 1;
                }
                else
                {
                    foreach (var user in users)
                    {
                        DatabaseContext.Users.Attach(user);

                        user.Name = "degis";
                    }

                    DatabaseContext.SaveChanges();
                }

                
            }
            finally
            {
                DatabaseContext.Configuration.AutoDetectChangesEnabled = true;
            }

            return 0;
        }

        public DatabaseContext DatabaseContext { get { return _context as DatabaseContext; } }
    }
}
