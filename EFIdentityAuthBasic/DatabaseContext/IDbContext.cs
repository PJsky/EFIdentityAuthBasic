using EFIdentityAuthBasic.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EFIdentityAuthBasic.DatabaseContext
{
    public interface IDbContext
    {
        DbSet<User> Users { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}