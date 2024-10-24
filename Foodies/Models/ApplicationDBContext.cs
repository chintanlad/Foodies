//using Microsoft.EntityFrameworkCore;

//namespace Foodies.Models
//{
//    public class ApplicationDBContext : DbContext
//    {
//        public DbSet<Admin> Admins { get; set; }
//        public DbSet<Customer> Customers { get; set; }
//        public DbSet<Meal> Meals { get; set; }
//        public DbSet<Menu> Menus { get; set; }
//        public DbSet<Restaurant> Restaurants { get; set; }
//        public DbSet<Reservation> Reservations { get; set; }

//        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        //{
//        //    optionsBuilder.UseLazyLoadingProxies().UseSqlServer(@"Data Source=(LocalDB)\\MSSQLLocalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
//        //}

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<Reservation>()
//                .HasOne(r => r.Meal)
//                .WithOne()
//                .HasForeignKey<Reservation>(m => m.Meal_Id)
//                .OnDelete(DeleteBehavior.Restrict);
//            base.OnModelCreating(modelBuilder);

//        }
//    }
//}




using Microsoft.EntityFrameworkCore;

namespace Foodies.Models
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }

        //public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseLazyLoadingProxies().UseSqlServer(@"Data Source=(LocalDB)\\MSSQLLocalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Meal)
                .WithOne()
                .HasForeignKey<Reservation>(m => m.Meal_Id)
                .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);
        }
    }
}
