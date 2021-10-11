using InformationCards.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace InformationCards.DataLayer
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
        {
        }

        public DbSet<InformationCard> InformationCards { get; set; }
    }
}
