using System;
using Axoom.MyService.Contacts;
using Microsoft.EntityFrameworkCore;

namespace Axoom.MyService
{
    public class MyServiceDbContext : DbContext
    {
        public DbSet<ContactEntity> Contacts { get; set; }
        public DbSet<PokeEntity> Pokes { get; set; }

        public MyServiceDbContext(DbContextOptions options) : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactEntity>().HasMany(x => x.Pokes).WithOne(x => x.Contact);

            base.OnModelCreating(modelBuilder);
        }

        public event Action Disposed;

        public override void Dispose()
        {
            base.Dispose();
            Disposed?.Invoke();
        }
    }
}