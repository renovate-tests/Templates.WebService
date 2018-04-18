using Axoom.MyService.Contacts;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace Axoom.MyService
{
    public partial class MyServiceDbContext
    {
        public DbSet<ContactEntity> Contacts { get; set; }
        public DbSet<PokeEntity> Pokes { get; set; }
    }
}
