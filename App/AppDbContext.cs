using App.Entities;
using Microsoft.EntityFrameworkCore;

namespace App
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {    
        }


        public DbSet<TopDish> TopDishes { get; set; }
        public DbSet<Stars> Stars { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories{ get; set; }
        public DbSet<Review> Reviews{ get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Restaurant>(r =>
            {
                r.HasMany(c => c.Categories)
                .WithOne(r => r.Restaurant)
                .HasForeignKey(k => k.RestaurantId);

                r.HasMany(r => r.Reviews)
                .WithOne(r => r.Restaurant)
                .HasForeignKey(k => k.RestaurantId);

                r.HasOne(a => a.Address)
                 .WithOne(r => r.Restaurant);


            });

            modelBuilder.Entity<Review>(r =>
            {
                r.HasOne(usr => usr.ReviewedBy)
                .WithMany(r => r.Reviews)
                .HasForeignKey(k => k.UserId);

                r.HasOne(s => s.Stars)
                .WithOne(r => r.Review);

            });

            modelBuilder.Entity<TopDish>(td =>
            {
                td.HasOne(c => c.Category)
                .WithMany(td => td.TopDish)
                .HasForeignKey(k => k.CategoryId);
            });




        }

    }
}
