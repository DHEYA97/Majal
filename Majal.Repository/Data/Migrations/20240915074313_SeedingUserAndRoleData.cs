using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Majal.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingUserAndRoleData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0F99E50D-B7AC-414C-83C9-F7036CD735D0", "06419EB4-5A67-4E86-98CA-85B29EF34909", true, false, "ContentWriter", "CONTENTWRITER" },
                    { "92b75286-d8f8-4061-9995-e6e23ccdee94", "f51e5a91-bced-49c2-8b86-c2e170c0846c", false, false, "Admin", "ADMIN" },
                    { "9eaa03df-8e4f-4161-85de-0f6e5e30bfd4", "5ee6bc12-5cb0-4304-91e7-6a00744e042a", true, false, "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsDisabled", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "6dc6528a-b280-4770-9eae-82671ee81ef7", 0, "99d2bbc6-bc54-4248-a172-a77de3ae4430", "admin@majal.com", true, "Admin", false, "Majal", false, null, "ADMIN@MAJAL.COM", "ADMIN@MAJAL.COM", "AQAAAAIAAYagAAAAEIs0MM9znH/mlULKgNSryXYQv2WcpeDB+mmBdEdWN2eqKhC7xJpDtxfZtIGx0l6PEw==", null, false, "55BF92C9EF0249CDA210D85D1A851BC9", false, "admin@majal.com" },
                    { "7905b7d2-1010-440c-bc43-a3839e864aaf", 0, "fb1072bb-6cf8-426b-84bd-0133045dfe91", "member@majal.com", true, "Member", false, "Majal", false, null, "MEMBER@MAJAL.COM", "MEMBER@MAJAL.COM", "AQAAAAIAAYagAAAAEGPCxBHHzqpTdNd6RsjoXvnqdm5T+m28bDRFiKEawLhwwLomAYtVqwfz4pyA8HEhXQ==", null, false, "78356252-8c27-4c09-8e16-34b5210ccf89", false, "member@majal.com" },
                    { "89c3955b-145f-4928-9261-a3bba73c43be", 0, "c41ca1ca-d166-4c9a-977a-19feea318fe0", "content@majal.com", true, "ContentWriter", false, "Majal", false, null, "CONTENT@MAJAL.COM", "CONTENT@MAJAL.COM", "AQAAAAIAAYagAAAAEFeBmogyAParP20OLckKht8+8T806SyGlPGfuDd/LDCVYe+Ez36J2EZEcREAfi8DEw==", null, false, "264aa87f-6f29-4542-a4e6-9743d7078549", false, "content@majal.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "92b75286-d8f8-4061-9995-e6e23ccdee94", "6dc6528a-b280-4770-9eae-82671ee81ef7" },
                    { "9eaa03df-8e4f-4161-85de-0f6e5e30bfd4", "7905b7d2-1010-440c-bc43-a3839e864aaf" },
                    { "0F99E50D-B7AC-414C-83C9-F7036CD735D0", "89c3955b-145f-4928-9261-a3bba73c43be" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "92b75286-d8f8-4061-9995-e6e23ccdee94", "6dc6528a-b280-4770-9eae-82671ee81ef7" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9eaa03df-8e4f-4161-85de-0f6e5e30bfd4", "7905b7d2-1010-440c-bc43-a3839e864aaf" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "0F99E50D-B7AC-414C-83C9-F7036CD735D0", "89c3955b-145f-4928-9261-a3bba73c43be" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0F99E50D-B7AC-414C-83C9-F7036CD735D0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "92b75286-d8f8-4061-9995-e6e23ccdee94");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9eaa03df-8e4f-4161-85de-0f6e5e30bfd4");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7905b7d2-1010-440c-bc43-a3839e864aaf");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "89c3955b-145f-4928-9261-a3bba73c43be");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetRoles");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
