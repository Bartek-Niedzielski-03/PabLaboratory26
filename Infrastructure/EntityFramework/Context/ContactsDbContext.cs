using AppCore.Entities;
using AppCore.Enums;
using AppCore.Users;
using Infrastructure.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Security;

namespace Infrastructure.EntityFramework.Context;

public class ContactsDbContext : IdentityDbContext<CrmUser, CrmRole, string>
{
    public DbSet<Person> People { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    public ContactsDbContext()
    {
    }

    public ContactsDbContext(DbContextOptions<ContactsDbContext> options)
        : base(options)
    {
    }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<CrmUser>(entity =>
        {
            entity.Property(u => u.FirstName).HasMaxLength(100);
            entity.Property(u => u.LastName).HasMaxLength(100);
            entity.Property(u => u.FullName).HasMaxLength(200);
            entity.Property(u => u.Department).HasMaxLength(100);
            entity.Property(u => u.Status).HasConversion<string>();
            entity.HasIndex(u => u.Email).IsUnique();
        });

        builder.Entity<CrmRole>(entity =>
        {
            entity.Property(r => r.Name).HasMaxLength(50);
            entity.Property(r => r.Description).HasMaxLength(200);
        });

        builder.Entity<Contact>()
            .HasDiscriminator<string>("ContactType")
            .HasValue<Person>("Person")
            .HasValue<Company>("Company")
            .HasValue<Organization>("Organization");

        builder.Entity<Contact>(entity =>
        {
            entity.Property(c => c.Email).HasMaxLength(200);
            entity.Property(c => c.Phone).HasMaxLength(20);
            entity.Property(c => c.Status).HasConversion<string>();
        });

        builder.Entity<Person>(entity =>
        {
            entity.Property(p => p.FirstName).HasMaxLength(100);
            entity.Property(p => p.LastName).HasMaxLength(100);
            entity.Property(p => p.MiddleName).HasMaxLength(100);
            entity.Property(p => p.Position).HasMaxLength(100);
            entity.Property(p => p.BirthDate).HasColumnType("date");
            entity.Property(p => p.Gender).HasConversion<string>();
        });

        builder.Entity<Company>(entity =>
        {
            entity.Property(c => c.Name).HasMaxLength(200);
            entity.Property(c => c.NIP).HasMaxLength(20);
            entity.Property(c => c.REGON).HasMaxLength(20);
            entity.Property(c => c.KRS).HasMaxLength(20);
            entity.Property(c => c.Industry).HasMaxLength(100);
            entity.Property(c => c.Website).HasMaxLength(200);
            entity.Property(c => c.AnnualRevenue).HasColumnType("decimal(18,2)");
        });

        builder.Entity<Organization>(entity =>
        {
            entity.Property(o => o.Name).HasMaxLength(200);
            entity.Property(o => o.Type).HasConversion<string>();
            entity.Property(o => o.KRS).HasMaxLength(20);
            entity.Property(o => o.Website).HasMaxLength(200);
            entity.Property(o => o.Mission).HasMaxLength(500);
        });

        builder.Entity<Person>()
            .HasOne(p => p.Employer)
            .WithMany(c => c.Employees)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Organization>()
            .HasMany(o => o.Members)
            .WithOne(p => p.Organization)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Contact>()
            .OwnsOne(c => c.Address, address =>
            {
                address.Property(a => a.Street).HasMaxLength(200);
                address.Property(a => a.City).HasMaxLength(100);
                address.Property(a => a.PostalCode).HasMaxLength(20);
                address.Property(a => a.Country).HasMaxLength(100);
                address.Property(a => a.Type).HasConversion<string>();
            });

        builder.Entity<Contact>()
            .OwnsMany(c => c.Tags, tag =>
            {
                tag.ToTable("ContactTags");
                tag.WithOwner().HasForeignKey("ContactId");
                tag.HasKey("Id");
                tag.Property(t => t.Name).HasMaxLength(100);
                tag.Property(t => t.Color).HasMaxLength(50);
            });

        builder.Entity<Contact>()
            .OwnsMany(c => c.Notes, note =>
            {
                note.ToTable("ContactNotes");
                note.WithOwner().HasForeignKey("ContactId");
                note.HasKey("Id");
                note.Property(n => n.Content).HasMaxLength(2000);
                note.Property(n => n.CreatedBy).HasMaxLength(100);
            });

        SeedContacts(builder);
        SeedIdentity(builder);
    }

    private static void SeedContacts(ModelBuilder builder)
    {
        var companyId = Guid.Parse("516A34D7-CCFB-4F20-85F3-62BD0F3AF271");
        var person1Id = Guid.Parse("3D54091D-ABC8-49EC-9590-93AD3ED5458F");
        var person2Id = Guid.Parse("B4DCB17C-F875-43F8-9D66-36597895A466");

        var createdAt = new DateTime(2026, 3, 31, 12, 0, 0, DateTimeKind.Utc);
        var updatedAt = new DateTime(2026, 3, 31, 12, 0, 0, DateTimeKind.Utc);

        builder.Entity<Company>().HasData(
            new Company
            {
                Id = companyId,
                Name = "WSEI",
                Industry = "edukacja",
                Phone = "123567123",
                Email = "biuro@wsei.edu.pl",
                Website = "https://wsei.edu.pl",
                Status = ContactStatus.Active,
                CreatedAt = createdAt,
                UpdatedAt = updatedAt
            }
        );

        builder.Entity<Person>().HasData(
            new
            {
                Id = person1Id,
                FirstName = "Adam",
                LastName = "Nowak",
                MiddleName = (string?)null,
                Gender = Gender.Male,
                Status = ContactStatus.Active,
                Email = "adam@wsei.edu.pl",
                Phone = "123456789",
                BirthDate = new DateTime(2001, 1, 11),
                Position = "Programista",
                CreatedAt = createdAt,
                UpdatedAt = updatedAt,
                EmployerId = companyId
            },
            new
            {
                Id = person2Id,
                FirstName = "Ewa",
                LastName = "Kowalska",
                MiddleName = (string?)null,
                Gender = Gender.Female,
                Status = ContactStatus.Blocked,
                Email = "ewa@wsei.edu.pl",
                Phone = "123123123",
                BirthDate = new DateTime(2001, 1, 11),
                Position = "Tester",
                CreatedAt = createdAt,
                UpdatedAt = updatedAt,
                EmployerId = companyId
            }
        );

        builder.Entity<Contact>()
            .OwnsOne(c => c.Address)
            .HasData(
                new
                {
                    ContactId = person1Id,
                    Street = "ul. Św. Filipa 17",
                    City = "Kraków",
                    PostalCode = "25-009",
                    Country = "Poland",
                    Type = AddressType.Correspondence
                }
            );
    }

    private static void SeedIdentity(ModelBuilder builder)
    {
        var adminRoleId = "11111111-1111-1111-1111-111111111111";
        var salesManagerRoleId = "22222222-2222-2222-2222-222222222222";
        var salespersonRoleId = "33333333-3333-3333-3333-333333333333";
        var supportAgentRoleId = "44444444-4444-4444-4444-444444444444";
        var readOnlyRoleId = "55555555-5555-5555-5555-555555555555";

        builder.Entity<CrmRole>().HasData(
            new CrmRole
            {
                Id = adminRoleId,
                Name = UserRole.Administrator.ToString(),
                NormalizedName = UserRole.Administrator.ToString().ToUpper(),
                Description = "System administrator"
            },
            new CrmRole
            {
                Id = salesManagerRoleId,
                Name = UserRole.SalesManager.ToString(),
                NormalizedName = UserRole.SalesManager.ToString().ToUpper(),
                Description = "Sales manager"
            },
            new CrmRole
            {
                Id = salespersonRoleId,
                Name = UserRole.Salesperson.ToString(),
                NormalizedName = UserRole.Salesperson.ToString().ToUpper(),
                Description = "Salesperson"
            },
            new CrmRole
            {
                Id = supportAgentRoleId,
                Name = UserRole.SupportAgent.ToString(),
                NormalizedName = UserRole.SupportAgent.ToString().ToUpper(),
                Description = "Support agent"
            },
            new CrmRole
            {
                Id = readOnlyRoleId,
                Name = UserRole.ReadOnly.ToString(),
                NormalizedName = UserRole.ReadOnly.ToString().ToUpper(),
                Description = "Read only user"
            }
        );

        var adminUser = new CrmUser
        {
            Id = "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA",
            UserName = "admin@crm.local",
            NormalizedUserName = "ADMIN@CRM.LOCAL",
            Email = "admin@crm.local",
            NormalizedEmail = "ADMIN@CRM.LOCAL",
            EmailConfirmed = true,
            FirstName = "System",
            LastName = "Administrator",
            FullName = "System Administrator",
            Department = "IT",
            Status = SystemUserStatus.Active,
            CreatedAt = new DateTime(2026, 3, 31, 12, 0, 0, DateTimeKind.Utc),
            SecurityStamp = "AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEEEE",
            ConcurrencyStamp = "11111111-AAAA-BBBB-CCCC-111111111111",
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnabled = true,
            AccessFailedCount = 0
        };
        
        adminUser.PasswordHash = "AQAAAAIAAYagAAAAEC61ZTRkkgM1/v8xX5/OnUVBzLUkn9jxtT4vsBT88zn2r1lc3eiBCv7JBR+/UmxaJw==";

        var salesUser = new CrmUser
        {
            Id = "BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB",
            UserName = "sales@crm.local",
            NormalizedUserName = "SALES@CRM.LOCAL",
            Email = "sales@crm.local",
            NormalizedEmail = "SALES@CRM.LOCAL",
            EmailConfirmed = true,
            FirstName = "Anna",
            LastName = "Sprzedaz",
            FullName = "Anna Sprzedaz",
            Department = "Sales",
            Status = SystemUserStatus.Active,
            CreatedAt = new DateTime(2026, 3, 31, 12, 0, 0, DateTimeKind.Utc),
            SecurityStamp = "BBBBBBBB-CCCC-DDDD-EEEE-FFFFFFFFFFFF",
            ConcurrencyStamp = "22222222-AAAA-BBBB-CCCC-222222222222",
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnabled = true,
            AccessFailedCount = 0
        };

        salesUser.PasswordHash = "AQAAAAIAAYagAAAAEDhF7iyz2+8ISY6G82exZtcX19VSrIlH2d0xtubk9I+YRlrNxaOzf41Pcs32Mct0/Q==";

        builder.Entity<CrmUser>().HasData(adminUser, salesUser);

        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                UserId = adminUser.Id,
                RoleId = adminRoleId
            },
            new IdentityUserRole<string>
            {
                UserId = salesUser.Id,
                RoleId = salespersonRoleId
            }
        );
    }
}