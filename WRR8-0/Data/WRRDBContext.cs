using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WRR8_0.Data
{
    //inherit from IdentityDbContext
    public class WRRDbContext : IdentityDbContext
    {
        public WRRDbContext(DbContextOptions<WRRDbContext> options)
            : base(options)
        {
        }

    }
}
