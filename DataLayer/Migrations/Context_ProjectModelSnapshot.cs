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

            modelBuilder.Entity("DataLayer.Entities.Comments", b =>
                {
                    b.Property<long>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("CommentId"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("DissertationRef")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("InsertDateTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("Insert_DateTime");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("UserRef")
                        .HasColumnType("bigint");

                    b.HasKey("CommentId");

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

                    b.Property<DateTime?>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("DissertationFileAddress")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Dissertation_FileAddress");

                    b.Property<string>("DissertationFileName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Dissertation_FileName");

                    b.Property<string>("ProceedingsFileAddress")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Proceedings_FileAddress");

                    b.Property<string>("ProceedingsFileName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Proceedings_FileName");

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

                    b.HasKey("DissertationId");

                    b.HasIndex("StudentId");

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

            modelBuilder.Entity("DataLayer.Entities.Replay", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("CommentRef")
                        .HasColumnType("bigint");

                    b.Property<long?>("ReplayId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CommentRef");

                    b.HasIndex("ReplayId");

                    b.ToTable("Replay", (string)null);
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

                    b.Property<string>("Name_Persian")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
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

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.HasIndex("TeacherId");

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

                    b.Property<string>("College")
                        .HasColumnType("nvarchar(max)");

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

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
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
                        .HasConstraintName("FK__Comments__Disser__3B75D760");

                    b.HasOne("DataLayer.Entities.Users", "UserRefNavigation")
                        .WithMany("Comments")
                        .HasForeignKey("UserRef")
                        .HasConstraintName("FK__Comments__UserRe__3A81B327");

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

            modelBuilder.Entity("DataLayer.Entities.Replay", b =>
                {
                    b.HasOne("DataLayer.Entities.Comments", "CommentRefNavigation")
                        .WithMany("ReplayCommentRefNavigations")
                        .HasForeignKey("CommentRef")
                        .HasConstraintName("FK__Replay__CommentR__3E52440B");

                    b.HasOne("DataLayer.Entities.Comments", "ReplayNavigation")
                        .WithMany("ReplayReplayNavigations")
                        .HasForeignKey("ReplayId")
                        .HasConstraintName("FK__Replay__ReplayId__3F466844");

                    b.Navigation("CommentRefNavigation");

                    b.Navigation("ReplayNavigation");
                });

            modelBuilder.Entity("DataLayer.Entities.Teachers", b =>
                {
                    b.HasOne("DataLayer.Entities.Users", "Student")
                        .WithMany("TeacherStudents")
                        .HasForeignKey("StudentId")
                        .HasConstraintName("FK__UserTB_Us__Stude__276EDEB3");

                    b.HasOne("DataLayer.Entities.Users", "TeacherNavigation")
                        .WithMany("TeacherTeacherNavigations")
                        .HasForeignKey("TeacherId")
                        .HasConstraintName("FK__UserTB_Us__Teach__267ABA7A");

                    b.Navigation("Student");

                    b.Navigation("TeacherNavigation");
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

            modelBuilder.Entity("DataLayer.Entities.Comments", b =>
                {
                    b.Navigation("ReplayCommentRefNavigations");

                    b.Navigation("ReplayReplayNavigations");
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

                    b.Navigation("TeacherStudents");

                    b.Navigation("TeacherTeacherNavigations");
                });
#pragma warning restore 612, 618
        }
    }
}
