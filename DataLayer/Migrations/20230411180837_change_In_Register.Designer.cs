﻿// <auto-generated />
using System;
using DataLayer.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataLayer.Migrations
{
    [DbContext(typeof(Context_Project))]
    [Migration("20230411180837_change_In_Register")]
    partial class change_In_Register
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("dbo")
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DataLayer.Entities.Comments", b =>
                {
                    b.Property<decimal>("Comment_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Comment_Id"));

                    b.Property<decimal?>("CommentsComment_Id")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("DissertationsDissertation_Id")
                        .HasColumnType("decimal(20,0)");

                    b.Property<DateTime>("Insert_DateTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("UesrId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Comment_Id");

                    b.HasIndex("CommentsComment_Id");

                    b.HasIndex("DissertationsDissertation_Id");

                    b.HasIndex("UesrId");

                    b.ToTable("Comments", "dbo");
                });

            modelBuilder.Entity("DataLayer.Entities.Confirmations", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ConfirmationsDissertationsId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PersianName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ConfirmationsDissertationsId");

                    b.ToTable("Confirmations", "dbo");
                });

            modelBuilder.Entity("DataLayer.Entities.ConfirmationsDissertations", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("DissertationsDissertation_Id")
                        .HasColumnType("decimal(20,0)");

                    b.Property<bool>("IsConfirm")
                        .HasColumnType("bit");

                    b.Property<string>("Time")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("UserId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("DissertationsDissertation_Id");

                    b.HasIndex("UserId");

                    b.ToTable("ConfirmationsDissertations", "dbo");
                });

            modelBuilder.Entity("DataLayer.Entities.Dissertations", b =>
                {
                    b.Property<decimal>("Dissertation_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Dissertation_Id"));

                    b.Property<string>("Abstract")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Allow_Edit")
                        .HasColumnType("bit");

                    b.Property<string>("Dissertation_FileAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Dissertation_FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Insert_DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Proceedings_FileAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Proceedings_FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("StudentId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("Term_Number")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title_English")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title_Persian")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Dissertation_Id");

                    b.HasIndex("StudentId");

                    b.ToTable("Dissertations", "dbo");
                });

            modelBuilder.Entity("DataLayer.Entities.KeyWord", b =>
                {
                    b.Property<decimal>("KeyWord_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("KeyWord_Id"));

                    b.Property<decimal?>("DissertationsDissertation_Id")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("Word_English")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Word_Persion")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("KeyWord_Id");

                    b.HasIndex("DissertationsDissertation_Id");

                    b.ToTable("KeyWords", "dbo");
                });

            modelBuilder.Entity("DataLayer.Entities.Logs", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Client")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ip")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Level")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Method")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("System")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Time")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Logs", "dbo");
                });

            modelBuilder.Entity("DataLayer.Entities.Roles", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"));

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Name_Persian")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("Roles", "dbo");

                    b.HasData(
                        new
                        {
                            Id = 2m,
                            Name = "Administrator",
                            Name_Persian = "مالک",
                            NormalizedName = "ADMINISTRATOR"
                        },
                        new
                        {
                            Id = 3m,
                            Name = "Student",
                            Name_Persian = "دانشجو",
                            NormalizedName = "STUDENT"
                        },
                        new
                        {
                            Id = 4m,
                            Name = "GuideMaster",
                            Name_Persian = "استاد راهنما",
                            NormalizedName = "GUIDEMASTER"
                        },
                        new
                        {
                            Id = 5m,
                            Name = "Adviser",
                            Name_Persian = "مشاور",
                            NormalizedName = "ADVISER"
                        },
                        new
                        {
                            Id = 6m,
                            Name = "EducationExpert",
                            Name_Persian = "کارشناس آموزش",
                            NormalizedName = "EDUCATIONEXPERT"
                        },
                        new
                        {
                            Id = 7m,
                            Name = "PostgraduateEducationExpert",
                            Name_Persian = "کارشناس تحصیلات تکمیلی",
                            NormalizedName = "POSTGRADUATEEDUCATIONEXPERT"
                        },
                        new
                        {
                            Id = 8m,
                            Name = "DissertationExpert",
                            Name_Persian = "کارشناس امور پایان نامه",
                            NormalizedName = "DISSERTATIONEXPERT"
                        });
                });

            modelBuilder.Entity("DataLayer.Entities.Users", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("College")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NationalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<decimal?>("UsersId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("UsersId");

                    b.ToTable("Users", "dbo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<ulong>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("RoleId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", "dbo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<ulong>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("UserId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", "dbo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<ulong>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("UserId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", "dbo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<ulong>", b =>
                {
                    b.Property<decimal>("UserId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("RoleId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", "dbo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<ulong>", b =>
                {
                    b.Property<decimal>("UserId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", "dbo");
                });

            modelBuilder.Entity("DataLayer.Entities.Comments", b =>
                {
                    b.HasOne("DataLayer.Entities.Comments", null)
                        .WithMany("Comments_Replay")
                        .HasForeignKey("CommentsComment_Id");

                    b.HasOne("DataLayer.Entities.Dissertations", null)
                        .WithMany("Comments")
                        .HasForeignKey("DissertationsDissertation_Id");

                    b.HasOne("DataLayer.Entities.Users", "Uesr")
                        .WithMany()
                        .HasForeignKey("UesrId");

                    b.Navigation("Uesr");
                });

            modelBuilder.Entity("DataLayer.Entities.Confirmations", b =>
                {
                    b.HasOne("DataLayer.Entities.ConfirmationsDissertations", null)
                        .WithMany("Confirmation_List")
                        .HasForeignKey("ConfirmationsDissertationsId");
                });

            modelBuilder.Entity("DataLayer.Entities.ConfirmationsDissertations", b =>
                {
                    b.HasOne("DataLayer.Entities.Dissertations", null)
                        .WithMany("ConfirmationsDissertations")
                        .HasForeignKey("DissertationsDissertation_Id");

                    b.HasOne("DataLayer.Entities.Users", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataLayer.Entities.Dissertations", b =>
                {
                    b.HasOne("DataLayer.Entities.Users", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("DataLayer.Entities.KeyWord", b =>
                {
                    b.HasOne("DataLayer.Entities.Dissertations", null)
                        .WithMany("KeyWords")
                        .HasForeignKey("DissertationsDissertation_Id");
                });

            modelBuilder.Entity("DataLayer.Entities.Users", b =>
                {
                    b.HasOne("DataLayer.Entities.Users", null)
                        .WithMany("Teachers")
                        .HasForeignKey("UsersId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<ulong>", b =>
                {
                    b.HasOne("DataLayer.Entities.Roles", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<ulong>", b =>
                {
                    b.HasOne("DataLayer.Entities.Users", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<ulong>", b =>
                {
                    b.HasOne("DataLayer.Entities.Users", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<ulong>", b =>
                {
                    b.HasOne("DataLayer.Entities.Roles", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataLayer.Entities.Users", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<ulong>", b =>
                {
                    b.HasOne("DataLayer.Entities.Users", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataLayer.Entities.Comments", b =>
                {
                    b.Navigation("Comments_Replay");
                });

            modelBuilder.Entity("DataLayer.Entities.ConfirmationsDissertations", b =>
                {
                    b.Navigation("Confirmation_List");
                });

            modelBuilder.Entity("DataLayer.Entities.Dissertations", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("ConfirmationsDissertations");

                    b.Navigation("KeyWords");
                });

            modelBuilder.Entity("DataLayer.Entities.Users", b =>
                {
                    b.Navigation("Teachers");
                });
#pragma warning restore 612, 618
        }
    }
}
