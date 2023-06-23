using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
    {

    }

    //Table
    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountRole> AccountRoles { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<University> Universities { get; set; }

    //Relationship
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Constraints Unique
        modelBuilder.Entity<Employee>()
            .HasIndex(e => new
            {
                e.NIK,
                e.Email,
                e.PhoneNumber
            }).IsUnique();

        //University - Education (One to Many)
        modelBuilder.Entity<University>()
            .HasMany(university => university.Educations)
            .WithOne(education => education.University)
            .HasForeignKey(Education => Education.UniversityGuid);

        //Education - Employee (One to One)
        modelBuilder.Entity<Education>()
        .HasOne(education => education.Employee)
        .WithOne(employee => employee.Education)
        .HasForeignKey<Education>(Education => Education.Guid);

        //Account - Employee (One to One)
        modelBuilder.Entity<Account>()
        .HasOne(account => account.Employee)
        .WithOne(employee => employee.Account)
        .HasForeignKey<Account>(Account => Account.Guid);

        //Employee - Booking (One to Many)
        modelBuilder.Entity<Employee>()
            .HasMany(employee => employee.Bookings)
            .WithOne(booking => booking.Employee)
            .HasForeignKey(Booking => Booking.EmployeeGuid);

        //Account - AccountRole (One to Many)
        modelBuilder.Entity<Account>()
            .HasMany(account => account.AccountRoles)
            .WithOne(accountrole => accountrole.Account)
            .HasForeignKey(AccountRole => AccountRole.AccountGuid);

        //Room - Booking (One to Many)
        modelBuilder.Entity<Room>()
            .HasMany(room => room.Bookings)
            .WithOne(booking => booking.Room)
            .HasForeignKey(Booking => Booking.RoomGuid);

        //Role - AccountRole (One to Many)
        modelBuilder.Entity<Role>()
            .HasMany(role => role.AccountRoles)
            .WithOne(accountrole => accountrole.Role)
            .HasForeignKey(AccountRole => AccountRole.RoleGuid);
    }

}//main
