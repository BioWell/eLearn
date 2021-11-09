using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eLearn.Modules.Users.Core.Persistence.Migrations
{
    public partial class Users_Entities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Identity");

            migrationBuilder.CreateTable(
                name: "Core_Country",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Code3 = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IsBillingEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsShippingEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsCityEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsZipCodeEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsDistrictEnabled = table.Column<bool>(type: "bit", nullable: false),
                    StateOrProvinceId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Core_Role",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Core_User",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshTokenHash = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Culture = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ExtensionData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LatestUpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Core_StateOrProvince",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CountryId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_StateOrProvince", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Core_StateOrProvince_Core_Country_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "Identity",
                        principalTable: "Core_Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Core_RoleClaims",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Group = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Core_RoleClaims_Core_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "Core_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Core_UserClaim",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_UserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Core_UserClaim_Core_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Core_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Core_UserLogin",
                schema: "Identity",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_UserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_Core_UserLogin_Core_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Core_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Core_UserRole",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_Core_UserRole_Core_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "Core_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Core_UserRole_Core_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Core_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Core_UserToken",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_UserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_Core_UserToken_Core_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Core_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Core_District",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateOrProvinceId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_District", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Core_District_Core_StateOrProvince_StateOrProvinceId",
                        column: x => x.StateOrProvinceId,
                        principalSchema: "Identity",
                        principalTable: "Core_StateOrProvince",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Core_Address",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactName = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    AddressLine1 = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    AddressLine2 = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    City = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    DistrictId = table.Column<long>(type: "bigint", nullable: true),
                    StateOrProvinceId = table.Column<long>(type: "bigint", nullable: false),
                    CountryId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Core_Address_Core_Country_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "Identity",
                        principalTable: "Core_Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Core_Address_Core_District_DistrictId",
                        column: x => x.DistrictId,
                        principalSchema: "Identity",
                        principalTable: "Core_District",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Core_Address_Core_StateOrProvince_StateOrProvinceId",
                        column: x => x.StateOrProvinceId,
                        principalSchema: "Identity",
                        principalTable: "Core_StateOrProvince",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Core_UserAddress",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressType = table.Column<int>(type: "int", nullable: false),
                    LastUsedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    AddressId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_UserAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Core_UserAddress_Core_Address_AddressId",
                        column: x => x.AddressId,
                        principalSchema: "Identity",
                        principalTable: "Core_Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Core_UserAddress_Core_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Core_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "Core_Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1L, "4776a1b2-dbe4-4056-82ec-8bed211d1454", "", "SuperAdmin", "SUPERADMIN" },
                    { 2L, "00d172be-03a0-4856-8b12-26d63fcf4374", "", "Admin", "ADMIN" },
                    { 3L, "00d172be-03a0-4856-8b12-26d645cf4374", "", "Students", "STUDENTS" },
                    { 4L, "d4754388-8355-4018-b728-218018836817", "", "Guest", "GUEST" },
                    { 5L, "d4754388-8355-4018-b728-218018896817", "", "Teacher", "TEACHER" },
                    { 6L, "d4754388-8355-4018-b728-218338836817", "", "Staff", "STAFF" }
                });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "Core_User",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedOn", "Culture", "Email", "EmailConfirmed", "ExtensionData", "FirstName", "ImageUrl", "IsActive", "LastName", "LatestUpdatedOn", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiryTime", "RefreshTokenHash", "SecurityStamp", "TwoFactorEnabled", "UserGuid", "UserName" },
                values: new object[,]
                {
                    { 1L, 0, "101cd6ae-a8ef-4a37-97fd-04ac2dd630e4", new DateTimeOffset(new DateTime(2021, 10, 31, 4, 33, 39, 189, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, "system@biowellacademy.com", true, "", "Bill", "", true, "Yao", new DateTimeOffset(new DateTime(2021, 11, 1, 4, 33, 39, 189, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), true, null, "SYSTEM@BIOWELLACADEMY.COM", "SYSTEM@BIOWELLACADEMY.COM", "AQAAAAEAACcQAAAAEL2tZZGiftZt3LUZjK4VWKo9sieuP2LDgUpaKfX4J9EgvKMai3trjbmvENBQCGDJjw==", null, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "a9565acb-cee6-425f-9833-419a793f5fba", false, new Guid("5f72f83b-7436-4221-869c-1b69b2e23aae"), "system@biowellacademy.com" },
                    { 2L, 0, "c83afcbc-312c-4589-bad7-8686bd4754c0", new DateTimeOffset(new DateTime(2021, 10, 31, 4, 33, 39, 190, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, "admin@biowellacademy.com", true, "", "Frank", "", true, "Huang", new DateTimeOffset(new DateTime(2021, 11, 2, 4, 33, 39, 190, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), true, null, "ADMIN@BIOWELLACADEMY.COM", "ADMIN@BIOWELLACADEMY.COM", "AQAAAAEAACcQAAAAEL2tZZGiftZt3LUZjK4VWKo9sieuP2LDgUpaKfX4J9EgvKMai3trjbmvENBQCGDJjw==", null, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "d6847450-47f0-4c7a-9fed-0c66234bf61f", false, new Guid("ed8210c3-24b0-4823-a744-80078cf12eb4"), "admin@biowellacademy.com" }
                });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "Core_UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 1L, 1L });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "Core_UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 1L, 2L });

            migrationBuilder.CreateIndex(
                name: "IX_Core_Address_CountryId",
                schema: "Identity",
                table: "Core_Address",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_Address_DistrictId",
                schema: "Identity",
                table: "Core_Address",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_Address_StateOrProvinceId",
                schema: "Identity",
                table: "Core_Address",
                column: "StateOrProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_District_StateOrProvinceId",
                schema: "Identity",
                table: "Core_District",
                column: "StateOrProvinceId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "Identity",
                table: "Core_Role",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Core_RoleClaims_RoleId",
                schema: "Identity",
                table: "Core_RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_StateOrProvince_CountryId",
                schema: "Identity",
                table: "Core_StateOrProvince",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "Identity",
                table: "Core_User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "Identity",
                table: "Core_User",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Core_UserAddress_AddressId",
                schema: "Identity",
                table: "Core_UserAddress",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_UserAddress_UserId",
                schema: "Identity",
                table: "Core_UserAddress",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_UserClaim_UserId",
                schema: "Identity",
                table: "Core_UserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_UserLogin_UserId",
                schema: "Identity",
                table: "Core_UserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_UserRole_RoleId",
                schema: "Identity",
                table: "Core_UserRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Core_RoleClaims",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Core_UserAddress",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Core_UserClaim",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Core_UserLogin",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Core_UserRole",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Core_UserToken",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Core_Address",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Core_Role",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Core_User",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Core_District",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Core_StateOrProvince",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Core_Country",
                schema: "Identity");
        }
    }
}
