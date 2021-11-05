using System;
using System.Collections.Generic;
using System.Linq;
using eLearn.Modules.Users.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
using Shared.Infrastructure.Auth;

namespace eLearn.Modules.Users.Core.Persistence
{
    internal sealed class UsersDbContext : IdentityDbContext<
        AppUser, AppRole, long,
        IdentityUserClaim<long>,
        AppUserRole, IdentityUserLogin<long>,
        AppRoleClaim, IdentityUserToken<long>>,
        IUsersDbContext
    {
        internal string Schema => "Identity";

        public UsersDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(Schema);
            base.OnModelCreating(builder);
            ApplyIdentityConfiguration(builder);
            SeedData(builder);
        }

        public void ApplyIdentityConfiguration(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            foreach (var property in builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(23,2)");
            }

            builder.Entity<IdentityUserLogin<long>>(b => { b.ToTable("Core_UserLogin"); });

            builder.Entity<IdentityUserToken<long>>(b => { b.ToTable("Core_UserToken"); });

            builder.Entity<IdentityUserClaim<long>>(b =>
            {
                b.HasKey(x => x.Id);
                b.ToTable("Core_UserClaim");
            });

            builder.Entity<AppRoleClaim>(entity =>
            {
                entity.ToTable(name: "Core_RoleClaims");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleClaims)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void SeedData(ModelBuilder builder)
        {
            builder.Entity<AppRole>().HasData(
                new AppRole
                {
                    Id = 1L,
                    ConcurrencyStamp = "4776a1b2-dbe4-4056-82ec-8bed211d1454",
                    Name = RoleConstants.SuperAdmin,
                    NormalizedName = Strings.ToUpperCase(RoleConstants.SuperAdmin)
                },
                new AppRole
                {
                    Id = 2L,
                    ConcurrencyStamp = "00d172be-03a0-4856-8b12-26d63fcf4374",
                    Name = RoleConstants.Admin,
                    NormalizedName = Strings.ToUpperCase(RoleConstants.Admin)
                },
                new AppRole
                {
                    Id = 3L,
                    ConcurrencyStamp = "00d172be-03a0-4856-8b12-26d645cf4374",
                    Name = RoleConstants.Students,
                    NormalizedName = Strings.ToUpperCase(RoleConstants.Students)
                },
                new AppRole
                {
                    Id = 4L,
                    ConcurrencyStamp = "d4754388-8355-4018-b728-218018836817",
                    Name = RoleConstants.Guest,
                    NormalizedName = Strings.ToUpperCase(RoleConstants.Guest)
                },
                new AppRole
                {
                    Id = 5L,
                    ConcurrencyStamp = "d4754388-8355-4018-b728-218018896817",
                    Name = RoleConstants.Teacher,
                    NormalizedName = Strings.ToUpperCase(RoleConstants.Teacher)
                },
                new AppRole
                {
                    Id = 6L,
                    ConcurrencyStamp = "d4754388-8355-4018-b728-218338836817",
                    Name = RoleConstants.Staff,
                    NormalizedName = Strings.ToUpperCase(RoleConstants.Staff)
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
                    EmailConfirmed = true,
                    FirstName = "Bill",
                    LastName = "Yao",
                    IsActive = true,
                    LockoutEnabled = true,
                    NormalizedEmail = "SYSTEM@BIOWELLACADEMY.COM",
                    NormalizedUserName = "SYSTEM@BIOWELLACADEMY.COM",
                    PasswordHash =
                        "AQAAAAEAACcQAAAAEL2tZZGiftZt3LUZjK4VWKo9sieuP2LDgUpaKfX4J9EgvKMai3trjbmvENBQCGDJjw==",
                    PhoneNumberConfirmed = false,
                    SecurityStamp = "a9565acb-cee6-425f-9833-419a793f5fba",
                    TwoFactorEnabled = false,
                    LatestUpdatedOn =
                        new DateTimeOffset(new DateTime(2021, 11, 1, 4, 33, 39, 189, DateTimeKind.Unspecified),
                            new TimeSpan(0, 7, 0, 0, 0)),
                    UserGuid = new Guid("5f72f83b-7436-4221-869c-1b69b2e23aae"), 
                    UserName = "system@biowellacademy.com"
                },
                new AppUser
                {
                    Id = 2L,
                    AccessFailedCount = 0,
                    ConcurrencyStamp = "c83afcbc-312c-4589-bad7-8686bd4754c0",
                    CreatedOn = new DateTimeOffset(new DateTime(2021, 10, 31, 4, 33, 39, 190, DateTimeKind.Unspecified),
                        new TimeSpan(0, 7, 0, 0, 0)),
                    Email = "admin@biowellacademy.com",
                    EmailConfirmed = true,
                    FirstName = "Frank",
                    LastName = "Huang",
                    IsActive = true,
                    LockoutEnabled = true,
                    NormalizedEmail = "ADMIN@BIOWELLACADEMY.COM",
                    NormalizedUserName = "ADMIN@BIOWELLACADEMY.COM",
                    PasswordHash =
                        "AQAAAAEAACcQAAAAEL2tZZGiftZt3LUZjK4VWKo9sieuP2LDgUpaKfX4J9EgvKMai3trjbmvENBQCGDJjw==",
                    PhoneNumberConfirmed = false,
                    SecurityStamp = "d6847450-47f0-4c7a-9fed-0c66234bf61f",
                    TwoFactorEnabled = false,
                    LatestUpdatedOn =
                        new DateTimeOffset(new DateTime(2021, 11, 2, 4, 33, 39, 190, DateTimeKind.Unspecified),
                            new TimeSpan(0, 7, 0, 0, 0)),
                    UserGuid = new Guid("ed8210c3-24b0-4823-a744-80078cf12eb4"),
                    UserName = "admin@biowellacademy.com"
                }
            );
/*
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
            );*/
        }
    }
}