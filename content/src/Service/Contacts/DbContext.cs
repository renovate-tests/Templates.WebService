using Microsoft.EntityFrameworkCore;
using MyVendor.MyService.Contacts;

// ReSharper disable once CheckNamespace
namespace MyVendor.MyService
{
    public partial class DbContext
    {
        public DbSet<ContactEntity> Contacts { get; set; }
        public DbSet<PokeEntity> Pokes { get; set; }
    }
}
