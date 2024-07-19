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
        public DbSet<RestaurantCategory> RestaurantCategories{ get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Restaurant>(r =>
            {
                
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
                .WithMany(r => r.Reviews)
                .HasForeignKey(k => k.StarsId);

                r.Property(d => d.PostTime)
                .HasDefaultValueSql("getutcdate()");

            });

            modelBuilder.Entity<TopDish>(td =>
            {
                td.HasOne(c => c.Category)
                .WithMany(td => td.TopDish)
                .HasForeignKey(k => k.CategoryId);
            });


            modelBuilder.Entity<RestaurantCategory>(rc =>
            {
                rc.HasKey(k => new { k.RestaurantId, k.CategoryId });

                rc.HasOne(c => c.Category)
                .WithMany(r => r.RestaurantCategories)
                .HasForeignKey(k => k.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

                rc.HasOne(c => c.Restaurant)
               .WithMany(r => r.RestaurantCategories)
               .HasForeignKey(k => k.RestaurantId)
               .OnDelete(DeleteBehavior.Restrict);

            });
               
            modelBuilder.Entity<Category>()
                .HasData(

                new Category()
                {
                    Id = 1,
                    Name = "FastFood"
                },

                new Category()
                {
                    Id = 2,
                    Name = "FamilyStyle"
                },

                 new Category()
                 {
                     Id = 3,
                     Name = "Premium"
                 },

                  new Category()
                  {
                      Id = 4,
                      Name = "Cafeteria"
                  },

                   new Category()
                   {
                       Id = 5,
                       Name = "Pub"
                   },

                    new Category()
                    {
                        Id = 6,
                        Name = "FoodTruck"
                    },

                     new Category()
                     {
                         Id = 7,
                         Name = "CasualDining"
                     }


                );


            modelBuilder.Entity<Stars>()
                .HasData(

                new Stars()
                {
                    Id = 1,
                    Star = 1,
                    Rating = "GastroLoser"
                },

                new Stars()
                {
                    Id = 2,
                    Star = 2,
                    Rating = "Low"
                },

                new Stars()
                {
                    Id = 3,
                    Star = 3,
                    Rating = "Medium"
                },

                new Stars()
                {
                    Id = 4,
                    Star = 4,
                    Rating = "High"
                },

                new Stars()
                {
                    Id = 5,
                    Star = 5,
                    Rating = "GastroBeast"
                }
                );

        }

    }
}
