using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuickDeals.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Persistance
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Deal> Deals { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //***************************************************************
            builder.Entity<Rating>()
                .HasKey(k => new { k.DealId, k.UserId });

            //User can like/dislike many deals
            builder.Entity<Rating>()
                .HasOne(u => u.User)
                .WithMany(d => d.DealRating)
                .HasForeignKey(fk => fk.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            //A deal can get likes/dislikes from many users
            builder.Entity<Rating>()
                .HasOne(u => u.Deal)
                .WithMany(d => d.DealRating)
                .HasForeignKey(fk => fk.DealId)
                .OnDelete(DeleteBehavior.NoAction);
            //******************************************************************

            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(au => au.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(ar => ar.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        }

    }
}
