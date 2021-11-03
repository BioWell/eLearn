using System;
using eLearn.Modules.Users.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eLearn.Modules.Users.Core.Persistence
{
    internal class UsersDbContext : IdentityDbContext<AppUser, AppRole, long,
        IdentityUserClaim<long>, AppUserRole, IdentityUserLogin<long>, IdentityRoleClaim<long>, IdentityUserToken<long>>
    {
        internal string Schema => "Users";
        
        public UsersDbContext(DbContextOptions options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(Schema);
            base.OnModelCreating(builder);
            // builder.ApplyConfiguration(new AppUserCfg());
            // ...
            //
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            SeedData(builder);
        }

        private void SeedData(ModelBuilder builder)
        {
            builder.Entity<AppRole>().HasData(
                new AppRole
                {
                    Id = 1L,
                    ConcurrencyStamp = "4776a1b2-dbe4-4056-82ec-8bed211d1454",
                    Name = "SuperAdmin",
                    NormalizedName = "SUPERADMIN"
                },
                new AppRole
                {
                    Id = 2L,
                    ConcurrencyStamp = "00d172be-03a0-4856-8b12-26d63fcf4374",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new AppRole
                {
                    Id = 3L,
                    ConcurrencyStamp = "00d172be-03a0-4856-8b12-26d645cf4374",
                    Name = "Students",
                    NormalizedName = "STUDENTS"
                },
                new AppRole
                {
                    Id = 4L,
                    ConcurrencyStamp = "d4754388-8355-4018-b728-218018836817",
                    Name = "Guest",
                    NormalizedName = "GUEST"
                },
                new AppRole
                {
                    Id = 5L,
                    ConcurrencyStamp = "d4754388-8355-4018-b728-218018896817",
                    Name = "Teacher",
                    NormalizedName = "TEACHER"
                },
                new AppRole
                {
                    Id = 6L,
                    ConcurrencyStamp = "d4754388-8355-4018-b728-218338836817",
                    Name = "Staff",
                    NormalizedName = "STAFF"
                }
            );

            builder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = 1L, 
                    AccessFailedCount = 0,
                    ConcurrencyStamp = "101cd6ae-a8ef-4a37-97fd-04ac2dd630e4",
                    CreatedOn = new DateTimeOffset(new DateTime(2021, 10, 31, 4, 33, 39, 189, DateTimeKind.Unspecified),
                        new TimeSpan(0, 7, 0, 0, 0)),
                    Email = "system@biowellacademy.com",
                    EmailConfirmed = false,
                    FirstName = "Bill",
                    LastName = "Yao",
                    IsActive = true,
                    LockoutEnabled = true,
                    NormalizedEmail = "SYSTEM@BIOWELLACADEMY.COM",
                    NormalizedUserName = "SYSTEM@BIOWELLACADEMY.COM",
                    PasswordHash =
                        "AQAAAAEAACcQAAAAEAEqSCV8Bpg69irmeg8N86U503jGEAYf75fBuzvL00/mr/FGEsiUqfR0rWBbBUwqtw==",
                    PhoneNumberConfirmed = false,
                    SecurityStamp = "a9565acb-cee6-425f-9833-419a793f5fba",
                    TwoFactorEnabled = false,
                    LatestUpdatedOn =
                        new DateTimeOffset(new DateTime(2018, 5, 29, 4, 33, 39, 189, DateTimeKind.Unspecified),
                            new TimeSpan(0, 7, 0, 0, 0)),
                    UserGuid = new Guid("5f72f83b-7436-4221-869c-1b69b2e23aae"), UserName = "system@simplcommerce.com"
                },
                new AppUser
                {
                    Id = 2L, 
                    AccessFailedCount = 0,
                    ConcurrencyStamp = "c83afcbc-312c-4589-bad7-8686bd4754c0",
                    CreatedOn = new DateTimeOffset(new DateTime(2021, 10, 31, 4, 33, 39, 190, DateTimeKind.Unspecified),
                        new TimeSpan(0, 7, 0, 0, 0)),
                    Email = "admin@biowellacademy.com",
                    EmailConfirmed = false,
                    FirstName = "Frank",
                    LastName = "Huang",
                    IsActive = false,
                    LockoutEnabled = true,
                    NormalizedEmail = "ADMIN@BIOWELLACADEMY.COM",
                    NormalizedUserName = "ADMIN@BIOWELLACADEMY.COM",
                    PasswordHash =
                        "AQAAAAEAACcQAAAAEAEqSCV8Bpg69irmeg8N86U503jGEAYf75fBuzvL00/mr/FGEsiUqfR0rWBbBUwqtw==",
                    PhoneNumberConfirmed = false,
                    SecurityStamp = "d6847450-47f0-4c7a-9fed-0c66234bf61f",
                    TwoFactorEnabled = false,
                    LatestUpdatedOn =
                        new DateTimeOffset(new DateTime(2018, 5, 29, 4, 33, 39, 190, DateTimeKind.Unspecified),
                            new TimeSpan(0, 7, 0, 0, 0)),
                    UserGuid = new Guid("ed8210c3-24b0-4823-a744-80078cf12eb4"), UserName = "admin@simplcommerce.com"
                }
            );

            builder.Entity<AppUserRole>().HasData(
                new AppUserRole {UserId = 1, RoleId = 1},
                new AppUserRole {UserId = 2, RoleId = 1}
            );

            builder.Entity<Country>().HasData(
                new Country("CN")
                {
                    Code3 = "CHN", 
                    Name = "China", 
                    IsBillingEnabled = true, 
                    IsShippingEnabled = true,
                    IsCityEnabled = false, 
                    IsZipCodeEnabled = false, 
                    IsDistrictEnabled = true
                },
                new Country("US")
                {
                    Code3 = "USA", 
                    Name = "United States", 
                    IsBillingEnabled = true, 
                    IsShippingEnabled = true,
                    IsCityEnabled = true, 
                    IsZipCodeEnabled = true, 
                    IsDistrictEnabled = false
                }
            );
 
            builder.Entity<StateOrProvince>().HasData(
                new StateOrProvince(1) {CountryId = "CN", Name = "上海", Type = "直辖市"},
                new StateOrProvince(2) {CountryId = "US", Name = "Washington", Code = "WA"}
            );

            builder.Entity<District>().HasData(
                new District(1) {Name = "普陀区", StateOrProvinceId = 1, Type = "Province"},
                new District(2) {Name = "静安区", StateOrProvinceId = 1, Type = "Province"}
            );

            builder.Entity<Address>().HasData(
                new Address(1) {AddressLine1 = "李冰路 43号", ContactName = "杨志龙", CountryId = "CN", StateOrProvinceId = 1}
            );
        }
    }
}