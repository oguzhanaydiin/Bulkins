using System;
using System.Data.Entity;
using Bulkins.Domain.Models;

namespace Bulkins.Dal
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext():base("BulkinsContext")
        {

        }
        public DbSet<User> Users { get; set; }
    }
}
