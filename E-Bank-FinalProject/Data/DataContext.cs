using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using E_Bank_FinalProject.Models;

namespace E_Bank_FinalProject.Data
{
    public class DataContext : DbContext
    {
        public DataContext (DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<E_Bank_FinalProject.Models.User> User { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<E_Bank_FinalProject.Models.User>().ToTable("User");
            modelBuilder.Entity<E_Bank_FinalProject.Models.UserRoles>().ToTable("UserRoles");
            modelBuilder.Entity<E_Bank_FinalProject.Models.Role>().ToTable("Role");
            modelBuilder.Entity<E_Bank_FinalProject.Models.Account>().ToTable("Account");
            modelBuilder.Entity<E_Bank_FinalProject.Models.CreditCard>().ToTable("CreditCard");

        }

        public DbSet<E_Bank_FinalProject.Models.Role>? Role { get; set; }
        public DbSet<E_Bank_FinalProject.Models.UserRoles>? UserRoles { get; set; }
        public DbSet<E_Bank_FinalProject.Models.Account>? Account { get; set; }
        public DbSet<E_Bank_FinalProject.Models.CreditCard>? CreditCard { get; set; }

    }
}
