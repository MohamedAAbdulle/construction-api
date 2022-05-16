using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ConstructionApi.Data
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
        {

        }
        public DbSet<InventoryDb> Inventory { get; set; }
        public DbSet<InventoryHistoryDb> InventoryHistory { get; set; }
        public DbSet<WorkerDb> Worker { get; set; }
        public DbSet<ActiveWorkerDb> ActiveWorker { get; set; }
        public DbSet<SupplierDb> Supplier { get; set; }
        public DbSet<QuoteDb> SupplierInventories { get; set; }
        public DbSet<OrderDb> Order { get; set; }
        public DbSet<WorkerTypeDb> WorkerType { get; set; }
        public DbSet<ToolDb> Tool { get; set; }
        public DbSet<InUseToolDb> InUseTool { get; set; }
        public DbSet<DocumentDb> Document { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InUseToolDb>()
                .HasKey(nameof(InUseToolDb.ToolId), nameof(InUseToolDb.WorkerId));
        }




    }
}
