using System;
using System.Data.Entity;
using TestTask.DAL.Entities;

namespace TestTask.DAL.EF
{
    public class UserContext : DbContext, IDisposable
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Relative> Relatives { get; set; }

        public UserContext()
            : base("DefaultConnection") { }

        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    new UserContext().Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}