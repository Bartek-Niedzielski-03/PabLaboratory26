using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Department = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeactivatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Address_Street = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Address_City = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Address_PostalCode = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Address_Country = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Address_Type = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    ContactType = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    NIP = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    REGON = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    KRS = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Industry = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    EmployeeCount = table.Column<int>(type: "INTEGER", nullable: true),
                    AnnualRevenue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Website = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    PrimaryContactId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Organization_Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    Organization_KRS = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Organization_Website = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Mission = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Organization_PrimaryContactId = table.Column<Guid>(type: "TEXT", nullable: true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    MiddleName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: true),
                    Gender = table.Column<string>(type: "TEXT", nullable: true),
                    Position = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    EmployerId = table.Column<Guid>(type: "TEXT", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contact_Contact_EmployerId",
                        column: x => x.EmployerId,
                        principalTable: "Contact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Contact_Contact_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Contact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Contact_Contact_Organization_PrimaryContactId",
                        column: x => x.Organization_PrimaryContactId,
                        principalTable: "Contact",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contact_Contact_PrimaryContactId",
                        column: x => x.PrimaryContactId,
                        principalTable: "Contact",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactNotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ContactId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactNotes_Contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Color = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ContactId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactTags_Contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "11111111-1111-1111-1111-111111111111", null, "System administrator", "Administrator", "ADMINISTRATOR" },
                    { "22222222-2222-2222-2222-222222222222", null, "Sales manager", "SalesManager", "SALESMANAGER" },
                    { "33333333-3333-3333-3333-333333333333", null, "Salesperson", "Salesperson", "SALESPERSON" },
                    { "44444444-4444-4444-4444-444444444444", null, "Support agent", "SupportAgent", "SUPPORTAGENT" },
                    { "55555555-5555-5555-5555-555555555555", null, "Read only user", "ReadOnly", "READONLY" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "DeactivatedAt", "Department", "Email", "EmailConfirmed", "FirstName", "FullName", "LastLoginAt", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA", 0, "11111111-AAAA-BBBB-CCCC-111111111111", new DateTime(2026, 3, 31, 12, 0, 0, 0, DateTimeKind.Utc), null, "IT", "admin@crm.local", true, "System", "System Administrator", null, "Administrator", true, null, "ADMIN@CRM.LOCAL", "ADMIN@CRM.LOCAL", "AQAAAAIAAYagAAAAEC61ZTRkkgM1/v8xX5/OnUVBzLUkn9jxtT4vsBT88zn2r1lc3eiBCv7JBR+/UmxaJw==", null, false, "AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEEEE", "Active", false, "admin@crm.local" },
                    { "BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB", 0, "22222222-AAAA-BBBB-CCCC-222222222222", new DateTime(2026, 3, 31, 12, 0, 0, 0, DateTimeKind.Utc), null, "Sales", "sales@crm.local", true, "Anna", "Anna Sprzedaz", null, "Sprzedaz", true, null, "SALES@CRM.LOCAL", "SALES@CRM.LOCAL", "AQAAAAIAAYagAAAAEDhF7iyz2+8ISY6G82exZtcX19VSrIlH2d0xtubk9I+YRlrNxaOzf41Pcs32Mct0/Q==", null, false, "BBBBBBBB-CCCC-DDDD-EEEE-FFFFFFFFFFFF", "Active", false, "sales@crm.local" }
                });

            migrationBuilder.InsertData(
                table: "Contact",
                columns: new[] { "Id", "AnnualRevenue", "ContactType", "CreatedAt", "Email", "EmployeeCount", "Industry", "KRS", "NIP", "Name", "Phone", "PrimaryContactId", "REGON", "Status", "UpdatedAt", "Website" },
                values: new object[] { new Guid("516a34d7-ccfb-4f20-85f3-62bd0f3af271"), null, "Company", new DateTime(2026, 3, 31, 12, 0, 0, 0, DateTimeKind.Utc), "biuro@wsei.edu.pl", null, "edukacja", null, null, "WSEI", "123567123", null, null, "Active", new DateTime(2026, 3, 31, 12, 0, 0, 0, DateTimeKind.Utc), "https://wsei.edu.pl" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "11111111-1111-1111-1111-111111111111", "AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA" },
                    { "33333333-3333-3333-3333-333333333333", "BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB" }
                });

            migrationBuilder.InsertData(
                table: "Contact",
                columns: new[] { "Id", "Address_City", "Address_Country", "Address_PostalCode", "Address_Street", "Address_Type", "BirthDate", "ContactType", "CreatedAt", "Email", "EmployerId", "FirstName", "Gender", "LastName", "MiddleName", "OrganizationId", "Phone", "Position", "Status", "UpdatedAt" },
                values: new object[] { new Guid("3d54091d-abc8-49ec-9590-93ad3ed5458f"), "Kraków", "Poland", "25-009", "ul. Św. Filipa 17", "Correspondence", new DateTime(2001, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Person", new DateTime(2026, 3, 31, 12, 0, 0, 0, DateTimeKind.Utc), "adam@wsei.edu.pl", new Guid("516a34d7-ccfb-4f20-85f3-62bd0f3af271"), "Adam", "Male", "Nowak", null, null, "123456789", "Programista", "Active", new DateTime(2026, 3, 31, 12, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Contact",
                columns: new[] { "Id", "BirthDate", "ContactType", "CreatedAt", "Email", "EmployerId", "FirstName", "Gender", "LastName", "MiddleName", "OrganizationId", "Phone", "Position", "Status", "UpdatedAt" },
                values: new object[] { new Guid("b4dcb17c-f875-43f8-9d66-36597895a466"), new DateTime(2001, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Person", new DateTime(2026, 3, 31, 12, 0, 0, 0, DateTimeKind.Utc), "ewa@wsei.edu.pl", new Guid("516a34d7-ccfb-4f20-85f3-62bd0f3af271"), "Ewa", "Female", "Kowalska", null, null, "123123123", "Tester", "Blocked", new DateTime(2026, 3, 31, 12, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contact_EmployerId",
                table: "Contact",
                column: "EmployerId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_Organization_PrimaryContactId",
                table: "Contact",
                column: "Organization_PrimaryContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_OrganizationId",
                table: "Contact",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_PrimaryContactId",
                table: "Contact",
                column: "PrimaryContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactNotes_ContactId",
                table: "ContactNotes",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactTags_ContactId",
                table: "ContactTags",
                column: "ContactId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ContactNotes");

            migrationBuilder.DropTable(
                name: "ContactTags");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Contact");
        }
    }
}
