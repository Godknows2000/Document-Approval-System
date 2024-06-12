using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentApprovalSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Department_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("DocType_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployerPemSec",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying", nullable: false),
                    Email = table.Column<string>(type: "character varying", nullable: true),
                    Phone = table.Column<string>(type: "character varying", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("EmployerPemSec_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployerSec",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying", nullable: false),
                    Phone = table.Column<string>(type: "character varying", nullable: false),
                    Address = table.Column<string>(type: "character varying", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("EmployerSec_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Title",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Title_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    LoginId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Mobile = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsEmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    SecurityStamp = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LastLoginDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    AuthenticatorKey = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    AuthRecoveryCodes = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    TwoFactorAuthEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false),
                    LockoutExpiryDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ActivationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EmployerSecId = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployerPemSecId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "User_CreatorId_fkey",
                        column: x => x.CreatorId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "User_EmployerPemSecId_fkey",
                        column: x => x.EmployerPemSecId,
                        principalTable: "EmployerPemSec",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "User_EmployerSecId_fkey",
                        column: x => x.EmployerSecId,
                        principalTable: "EmployerSec",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    UniqueId = table.Column<Guid>(type: "uuid", nullable: true),
                    Container = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Size = table.Column<int>(type: "integer", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NotesJson = table.Column<string>(type: "text", nullable: true),
                    Extension = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Id);
                    table.ForeignKey(
                        name: "Attachment_CreatorId_fkey",
                        column: x => x.CreatorId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmailConfig",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    TargetId = table.Column<int>(type: "integer", nullable: false),
                    SenderId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Username = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    SenderDisplayName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Hash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Host = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Port = table.Column<int>(type: "integer", nullable: false),
                    EnableSsl = table.Column<bool>(type: "boolean", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailConfig_User",
                        column: x => x.CreatorId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<Guid>(name: "Id ", type: "uuid", nullable: false),
                    AccountId = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Surname = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    TitleId = table.Column<int>(type: "integer", nullable: false),
                    DoB = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IdNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    EcNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Address = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ProfileStatusId = table.Column<int>(type: "integer", nullable: false),
                    AttachmentsJson = table.Column<string>(type: "text", nullable: true),
                    StatusId = table.Column<int>(type: "integer", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Position = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, defaultValueSql: "''::character varying"),
                    NotesJson = table.Column<string>(type: "text", nullable: true),
                    ProfilePictureId = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    DepartmentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Employee_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "Employee_Department",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Employee_ProfilePictureId_fkey",
                        column: x => x.ProfilePictureId,
                        principalTable: "Attachment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Employee_TitleId_fkey",
                        column: x => x.TitleId,
                        principalTable: "Title",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Employee_User",
                        column: x => x.Id,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DocRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DocTypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    RequestComments = table.Column<string>(type: "character varying", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Number = table.Column<string>(type: "character varying", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    StatusId = table.Column<int>(type: "integer", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    NotesJson = table.Column<string>(type: "character varying", nullable: true),
                    AttachmentsJson = table.Column<string>(type: "character varying", nullable: true),
                    SignatureUrl = table.Column<string>(type: "text", nullable: true),
                    SecretarySignatureUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("LeaveRequest_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "Creator",
                        column: x => x.CreatorId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Department",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "DocType",
                        column: x => x.DocTypeId,
                        principalTable: "DocType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Employee",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id ");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_CreatorId",
                table: "Attachment",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_DocRequest_CreatorId",
                table: "DocRequest",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_DocRequest_DepartmentId",
                table: "DocRequest",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocRequest_DocTypeId",
                table: "DocRequest",
                column: "DocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DocRequest_EmployeeId",
                table: "DocRequest",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailConfig_CreatorId",
                table: "EmailConfig",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_DepartmentId",
                table: "Employee",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_ProfilePictureId",
                table: "Employee",
                column: "ProfilePictureId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_TitleId",
                table: "Employee",
                column: "TitleId");

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatorId",
                table: "User",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_User_EmployerPemSecId",
                table: "User",
                column: "EmployerPemSecId");

            migrationBuilder.CreateIndex(
                name: "IX_User_EmployerSecId",
                table: "User",
                column: "EmployerSecId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocRequest");

            migrationBuilder.DropTable(
                name: "EmailConfig");

            migrationBuilder.DropTable(
                name: "DocType");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "Title");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "EmployerPemSec");

            migrationBuilder.DropTable(
                name: "EmployerSec");
        }
    }
}
