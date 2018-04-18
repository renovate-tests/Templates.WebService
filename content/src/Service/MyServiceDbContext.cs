using Microsoft.EntityFrameworkCore;

namespace Axoom.MyService
{
    /// <summary>
    /// Describes the service's database model.
    /// Used as a combination of the Unit Of Work and Repository patterns.
    /// </summary>
    public partial class MyServiceDbContext : DbContext
    {
        // NOTE: Other parts of this class are in separate slice-specific files

        public MyServiceDbContext(DbContextOptions options)
            : base(options)
        {}
    }
}
