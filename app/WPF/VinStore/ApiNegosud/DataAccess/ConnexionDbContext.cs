using System.ComponentModel;
using ApiNegosud.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiNegosud.DataAccess
{
    public class ConnexionDbContext : DbContext
    {
        /*appel constructeur de base , DbContext est responsable de la gestion de la connexion à la bdd*/
        public ConnexionDbContext(DbContextOptions options) : base(options) { }
        /* pour les classes il ya une table qui représente  la bdd */
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<ClientOrder> ClientOrder { get; set; }
        public DbSet<ClientOrderLine> ClientOrderLine { get; set; }
        public DbSet<Family> Family { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Provider> Provider { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<ProviderOrder> ProviderOrder { get; set; }
        public DbSet<ProviderOrderLine> ProviderOrderLine { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<InventoryLigne> InventoryLigne { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.DateProduction)
                .HasColumnType("date")
                .IsRequired();
            modelBuilder.Entity<ClientOrder>()
             .Property(co => co.OrderStatus)
             .HasConversion<string>();
            modelBuilder.Entity<ProviderOrder>()
            .Property(co => co.ProviderOrderStatus)
            .HasConversion<string>();
            modelBuilder.Entity<Inventory>()
           .Property(co => co.StatusInventory)
           .HasConversion<string>();
        }
        
    }
}