﻿// <auto-generated />
using System;
using DataLayer.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataLayer.Migrations
{
    [DbContext(typeof(Context_Project))]
    partial class Context_ProjectModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DataLayer.Entities.Baslookup", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Aux")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Aux2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Code")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Title")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Type")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id")
                        .HasName("PK__BASLooku__3214EC07A4E6B86E");

                    b.ToTable("BASLookup", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Code = 1,
                            Title = "مهندسي کامپيوتر",
                            Type = "CollegesUni"
                        },
                        new
                        {
                            Id = 2L,
                            Code = 2,
                            Title = "مهندسي برق",
                            Type = "CollegesUni"
                        },
                        new
                        {
                            Id = 3L,
                            Code = 3,
                            Title = "مهندسي مکانيک",
                            Type = "CollegesUni"
                        },
                        new
                        {
                            Id = 4L,
                            Code = 4,
                            Title = "مهندسي عمران",
                            Type = "CollegesUni"
                        },
                        new
                        {
                            Id = 5L,
                            Code = 5,
                            Title = "مهندسي معماري و شهرسازی",
                            Type = "CollegesUni"
                        },
                        new
                        {
                            Id = 6L,
                            Code = 6,
                            Title = "مهندسي مواد و علوم ميان رشته‌ای",
                            Type = "CollegesUni"
                        },
                        new
                        {
                            Id = 7L,
                            Code = 7,
                            Title = "حوضه علوم اسلامي",
                            Type = "CollegesUni"
                        },
                        new
                        {
                            Id = 8L,
                            Code = 8,
                            Title = "مرکز آموزش الکترونيکي",
                            Type = "CollegesUni"
                        },
                        new
                        {
                            Id = 9L,
                            Code = 9,
                            Title = "علوم پايه",
                            Type = "CollegesUni"
                        },
                        new
                        {
                            Id = 10L,
                            Code = 10,
                            Title = "علوم ورزشي",
                            Type = "CollegesUni"
                        },
                        new
                        {
                            Id = 11L,
                            Code = 11,
                            Title = "علوم انساني",
                            Type = "CollegesUni"
                        },
                        new
                        {
                            Id = 12L,
                            Code = 0,
                            Description = "Register",
                            Title = "ثبت اوليه پایان نامه",
                            Type = "DissertationStatus"
                        },
                        new
                        {
                            Id = 13L,
                            Code = 1,
                            Description = "ConfirmationGuideMaster",
                            Title = "تاییدیه استاد راهنمای اول",
                            Type = "DissertationStatus"
                        },
                        new
                        {
                            Id = 14L,
                            Code = 2,
                            Description = "ConfirmationGuideMaster2",
                            Title = "تاییدیه استاد راهنمای دوم",
                            Type = "DissertationStatus"
                        },
                        new
                        {
                            Id = 15L,
                            Code = 3,
                            Description = "ConfirmationGuideMaster3",
                            Title = "تاییدیه استاد راهنمای سوم",
                            Type = "DissertationStatus"
                        },
                        new
                        {
                            Id = 16L,
                            Code = 4,
                            Description = "ConfirmationEducationExpert",
                            Title = "تاییدیه کارشناس آموزش",
                            Type = "DissertationStatus"
                        },
                        new
                        {
                            Id = 17L,
                            Code = 5,
                            Description = "ConfirmationPostgraduateEducationExpert",
                            Title = "تاییدیه کارشناس تحصیلات تکمیلی",
                            Type = "DissertationStatus"
                        },
                        new
                        {
                            Id = 18L,
                            Code = 6,
                            Description = "ConfirmationDissertationExpert",
                            Title = "تاییدیه کارشناس امور پایان نامه",
                            Type = "DissertationStatus"
                        },
                        new
                        {
                            Id = 19L,
                            Code = -3333,
                            Description = "ExpirDissertation",
                            Title = "رد پایان نامه",
                            Type = "DissertationStatus"
                        },
                        new
                        {
                            Id = 20L,
                            Code = 0,
                            Description = "جدول نقش های سیستم",
                            Title = "AspNetRoles",
                            Type = "App_Table"
                        },
                        new
                        {
                            Id = 21L,
                            Code = 1,
                            Description = "جدول تمام کاربران سیستم",
                            Title = "AspNetUsers",
                            Type = "App_Table"
                        },
                        new
                        {
                            Id = 22L,
                            Code = 2,
                            Description = "جدول تمام کامنت های سیستم",
                            Title = "Comments",
                            Type = "App_Table"
                        },
                        new
                        {
                            Id = 23L,
                            Code = 3,
                            Description = "جدول تمام پایان نامه های سیستم",
                            Title = "Dissertations",
                            Type = "App_Table"
                        },
                        new
                        {
                            Id = 24L,
                            Code = 4,
                            Description = "جدول تمام لاگ های سیستم",
                            Title = "Logs",
                            Type = "App_Table"
                        });
                });

            modelBuilder.Entity("DataLayer.Entities.Comments", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("DissertationRef")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("InsertDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<long?>("UserRef")
                        .HasColumnType("bigint");

                    b.HasKey("Id")
                        .HasName("PK__Comments__3214EC077B918307");

                    b.HasIndex("DissertationRef");

                    b.HasIndex("UserRef");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("DataLayer.Entities.Dissertations", b =>
                {
                    b.Property<long>("DissertationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Dissertation_Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("DissertationId"));

                    b.Property<string>("Abstract")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("AllowEdit")
                        .HasColumnType("bit")
                        .HasColumnName("Allow_Edit");

                    b.Property<string>("DissertationFileAddress")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Dissertation_FileAddress");

                    b.Property<string>("DissertationFileName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Dissertation_FileName");

                    b.Property<DateTime?>("EditDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProceedingsFileAddress")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Proceedings_FileAddress");

                    b.Property<string>("ProceedingsFileName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Proceedings_FileName");

                    b.Property<DateTime?>("RegisterDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("StatusDissertation")
                        .HasColumnType("int")
                        .HasColumnName("Status_Dissertation");

                    b.Property<long?>("StudentId")
                        .HasColumnType("bigint");

                    b.Property<string>("TermNumber")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Term_Number");

                    b.Property<string>("TitleEnglish")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Title_English");

                    b.Property<string>("TitlePersian")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Title_Persian");

                    b.Property<int>("UpdateCnt")
                        .HasColumnType("int");

                    b.HasKey("DissertationId");

                    b.HasIndex(new[] { "StudentId" }, "IX_Dissertations_StudentId");

                    b.ToTable("Dissertations");
                });

            modelBuilder.Entity("DataLayer.Entities.KeyWord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("DissertationRef")
                        .HasColumnType("bigint");

                    b.Property<string>("Word")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DissertationRef");

                    b.ToTable("KeyWord", (string)null);
                });

            modelBuilder.Entity("DataLayer.Entities.Logs", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Client")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Ip")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Level")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("DataLayer.Entities.Roles", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PersianName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Name = "Administrator",
                            NormalizedName = "ADMINISTRATOR",
                            PersianName = "مالک"
                        },
                        new
                        {
                            Id = 2L,
                            Name = "Student",
                            NormalizedName = "STUDENT",
                            PersianName = "دانشجو"
                        },
                        new
                        {
                            Id = 3L,
                            Name = "GuideMaster",
                            NormalizedName = "GUIDEMASTER",
                            PersianName = "استاد راهنما"
                        },
                        new
                        {
                            Id = 4L,
                            Name = "Adviser",
                            NormalizedName = "ADVISER",
                            PersianName = "مشاور"
                        },
                        new
                        {
                            Id = 5L,
                            Name = "EducationExpert",
                            NormalizedName = "EDUCATIONEXPERT",
                            PersianName = "کارشناس آموزش"
                        },
                        new
                        {
                            Id = 6L,
                            Name = "PostgraduateEducationExpert",
                            NormalizedName = "POSTGRADUATEEDUCATIONEXPERT",
                            PersianName = "کارشناس تحصیلات تکمیلی"
                        },
                        new
                        {
                            Id = 7L,
                            Name = "DissertationExpert",
                            NormalizedName = "DISSERTATIONEXPERT",
                            PersianName = "کارشناس امور پایان نامه"
                        });
                });

            modelBuilder.Entity("DataLayer.Entities.Teachers", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("StudentId")
                        .HasColumnType("bigint")
                        .HasColumnName("StudentID");

                    b.Property<long?>("TeacherId")
                        .HasColumnType("bigint")
                        .HasColumnName("TeacherID");

                    b.Property<long?>("TeacherNavigationId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.HasIndex("TeacherNavigationId");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("DataLayer.Entities.Users", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("College")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("CollegeRef")
                        .HasColumnType("bigint");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
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

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("CollegeRef");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex(new[] { "NormalizedEmail" }, "EmailIndex");

                    b.HasIndex(new[] { "NormalizedUserName" }, "UserNameIndex")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex1")
                        .HasFilter("([NormalizedUserName] IS NOT NULL)");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            AccessFailedCount = 0,
                            Active = true,
                            ConcurrencyStamp = "00c194af-4f8b-438c-a33f-543b93a48358",
                            EmailConfirmed = false,
                            FirstName = "Ali",
                            LastName = "Moosaei",
                            LockoutEnabled = false,
                            PasswordHash = "AQAAAAIAAYagAAAAECj4NT8lrikZFClrFPC8twPx+S1/oWchdVTHyKWMeCWBxYBGM6RQguQbnafnYrn+Lg==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "K7JCQNNN4ULGGODXGAHOHXHF2MHWMYZU",
                            TwoFactorEnabled = false,
                            UserName = "Admin"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<long>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<long>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<long>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<long>", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = 1L,
                            RoleId = 1L
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<long>", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("DataLayer.Entities.Comments", b =>
                {
                    b.HasOne("DataLayer.Entities.Dissertations", "DissertationRefNavigation")
                        .WithMany("Comments")
                        .HasForeignKey("DissertationRef")
                        .HasConstraintName("FK__Comments__Disser__7E37BEF6");

                    b.HasOne("DataLayer.Entities.Users", "UserRefNavigation")
                        .WithMany("Comments")
                        .HasForeignKey("UserRef")
                        .HasConstraintName("FK__Comments__UserRe__7D439ABD");

                    b.Navigation("DissertationRefNavigation");

                    b.Navigation("UserRefNavigation");
                });

            modelBuilder.Entity("DataLayer.Entities.Dissertations", b =>
                {
                    b.HasOne("DataLayer.Entities.Users", "Student")
                        .WithMany("Dissertations")
                        .HasForeignKey("StudentId")
                        .HasConstraintName("FK__Dissertat__Stude__34C8D9D1");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("DataLayer.Entities.KeyWord", b =>
                {
                    b.HasOne("DataLayer.Entities.Dissertations", "DissertationRefNavigation")
                        .WithMany("KeyWords")
                        .HasForeignKey("DissertationRef")
                        .HasConstraintName("FK__KeyWord__Dissert__37A5467C");

                    b.Navigation("DissertationRefNavigation");
                });

            modelBuilder.Entity("DataLayer.Entities.Teachers", b =>
                {
                    b.HasOne("DataLayer.Entities.Users", "StudentNavigation")
                        .WithMany("Teachers")
                        .HasForeignKey("StudentId")
                        .HasConstraintName("FK__UserTB_Us__Stude__276EDEB3");

                    b.HasOne("DataLayer.Entities.Users", "TeacherNavigation")
                        .WithMany()
                        .HasForeignKey("TeacherNavigationId");

                    b.Navigation("StudentNavigation");

                    b.Navigation("TeacherNavigation");
                });

            modelBuilder.Entity("DataLayer.Entities.Users", b =>
                {
                    b.HasOne("DataLayer.Entities.Baslookup", "CollegeRefNavigation")
                        .WithMany("Users")
                        .HasForeignKey("CollegeRef")
                        .HasConstraintName("FK_aspnetUsers_BasLookup");

                    b.Navigation("CollegeRefNavigation");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<long>", b =>
                {
                    b.HasOne("DataLayer.Entities.Roles", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<long>", b =>
                {
                    b.HasOne("DataLayer.Entities.Users", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<long>", b =>
                {
                    b.HasOne("DataLayer.Entities.Users", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<long>", b =>
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<long>", b =>
                {
                    b.HasOne("DataLayer.Entities.Users", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataLayer.Entities.Baslookup", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("DataLayer.Entities.Dissertations", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("KeyWords");
                });

            modelBuilder.Entity("DataLayer.Entities.Users", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Dissertations");

                    b.Navigation("Teachers");
                });
#pragma warning restore 612, 618
        }
    }
}
