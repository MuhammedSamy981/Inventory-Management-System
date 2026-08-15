using System.Reflection;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using InventoryManagementSystem.Infrastructure.Identity;

namespace InventoryManagementSystem.Infrastructure.Data
{
        public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
        {
           public virtual DbSet<AuditLog> AuditLogs { get; set; }
           public virtual DbSet<Payment> Payments { get; set; }
           public virtual DbSet<Item> Items { get; set; }
           public virtual DbSet<Operation> Operations { get; set; }
           public virtual DbSet<Person> People { get; set; }
           public virtual DbSet<Inventory> Inventories { get; set; }
           public virtual DbSet<InventoryItem> InventoryItems { get; set; }

           public ApplicationDbContext(DbContextOptions options) : base(options)
           {


           }

           protected override void OnModelCreating(ModelBuilder modelBuilder)
           {

                        /*             modelBuilder.Entity<ApplicationUser>()
                                       .HasMany(e => e.Inventories)
                                       .WithOne()
                               .HasForeignKey(e => e.ApplicationUserId)
                               .OnDelete(DeleteBehavior.NoAction);

                                                 modelBuilder.Entity<ApplicationUser>()
                                       .HasMany(e => e.Operations)
                                       .WithOne()
                               .HasForeignKey(e => e.ApplicationUserId)
                               .OnDelete(DeleteBehavior.NoAction);*/

                modelBuilder.Entity<Person>()
                      .HasMany(e => e.Inventories)
                      .WithOne(e => e.Manager)
                      .HasForeignKey(e => e.ManagerId)
                      .OnDelete(DeleteBehavior.NoAction);

                modelBuilder.Entity<Person>()
                      .HasMany(e => e.Operations)
                      .WithOne(e => e.Person)
                      .HasForeignKey(e => e.PersonId)
                      .OnDelete(DeleteBehavior.NoAction);


                modelBuilder.Entity<Inventory>()
                      .HasMany(e => e.Operations)
                      .WithOne(e => e.Inventory)
                      .HasForeignKey(e => e.InventoryId)
                      .OnDelete(DeleteBehavior.NoAction);

                modelBuilder.Entity<Inventory>()
                      .HasMany(e => e.InventoryItems)
                      .WithOne(e => e.Inventory)
                      .HasForeignKey(e => e.InventoryId)
                      .OnDelete(DeleteBehavior.NoAction);

                modelBuilder.Entity<Item>()
                      .HasMany(e => e.Operations)
                      .WithOne(e => e.Item)
                      .HasForeignKey(e => e.ItemId)
                      .OnDelete(DeleteBehavior.NoAction);

                modelBuilder.Entity<Item>()
                       .HasMany(e => e.InventoryItems)
                       .WithOne(e => e.Item)
                       .HasForeignKey(e => e.ItemId)
                       .OnDelete(DeleteBehavior.NoAction);

                modelBuilder.Entity<Payment>()
                       .Property(p => p.Amount)
                       .HasPrecision(18, 2);

                modelBuilder.Entity<InventoryItem>()
                       .HasKey(e => new { e.InventoryId, e.ItemId });

                        base.OnModelCreating(modelBuilder);
                }



        }

}