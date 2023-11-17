using DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataBase
{
    public class Context_Project : IdentityDbContext<Users, Roles, long>
    {
        public Context_Project(DbContextOptions<Context_Project> Option) : base(Option)
        { }


        // --------------  DbSets  ---------------------------
        public virtual DbSet<Comments> Comments { get; set; } = null!;
        public virtual DbSet<Dissertations> Dissertations { get; set; } = null!;
        public virtual DbSet<KeyWord> KeyWords { get; set; } = null!;
        public virtual DbSet<Logs> Logs { get; set; } = null!;
        public virtual DbSet<Replay> Replays { get; set; } = null!;
        public virtual DbSet<Teachers> Teachers { get; set; } = null!;
        public virtual DbSet<Baslookup> Baslookups { get; set; }


        //public DbSet<CommentSelfRelation> CommentSelfRelations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);
                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasOne(d => d.CollegeRefNavigation).WithMany(p => p.Users)
                    .HasForeignKey(d => d.CollegeRef)
                    .HasConstraintName("FK_aspnetUsers_BasLookup");
            });
            
            modelBuilder.Entity<Comments>(entity =>
            {
                entity.Property(e => e.InsertDateTime).HasColumnName("Insert_DateTime");

                entity.HasOne(d => d.DissertationRefNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.DissertationRef)
                    .HasConstraintName("FK__Comments__Disser__3B75D760");

                entity.HasOne(d => d.UserRefNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserRef)
                    .HasConstraintName("FK__Comments__UserRe__3A81B327");
            });

            modelBuilder.Entity<Dissertations>(entity =>
            {
                entity.HasKey(e => e.DissertationId);

                entity.Property(e => e.DissertationId).HasColumnName("Dissertation_Id");

                entity.Property(e => e.AllowEdit).HasColumnName("Allow_Edit");

                entity.Property(e => e.DissertationFileAddress).HasColumnName("Dissertation_FileAddress");

                entity.Property(e => e.DissertationFileName).HasColumnName("Dissertation_FileName");

                entity.Property(e => e.ProceedingsFileAddress).HasColumnName("Proceedings_FileAddress");

                entity.Property(e => e.ProceedingsFileName).HasColumnName("Proceedings_FileName");

                entity.Property(e => e.StatusDissertation).HasColumnName("Status_Dissertation");

                entity.Property(e => e.TermNumber).HasColumnName("Term_Number");

                entity.Property(e => e.TitleEnglish).HasColumnName("Title_English");

                entity.Property(e => e.TitlePersian).HasColumnName("Title_Persian");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Dissertations)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK__Dissertat__Stude__34C8D9D1");
            });

            modelBuilder.Entity<KeyWord>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("KeyWord");

                entity.HasOne(d => d.DissertationRefNavigation)
                    .WithMany(p => p.KeyWords)
                    .HasForeignKey(d => d.DissertationRef)
                    .HasConstraintName("FK__KeyWord__Dissert__37A5467C");
            });

            modelBuilder.Entity<Replay>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("Replay");

                entity.HasOne(d => d.CommentRefNavigation)
                    .WithMany(p => p.ReplayCommentRefNavigations)
                    .HasForeignKey(d => d.CommentRef)
                    .HasConstraintName("FK__Replay__CommentR__3E52440B");

                entity.HasOne(d => d.ReplayNavigation)
                    .WithMany(p => p.ReplayReplayNavigations)
                    .HasForeignKey(d => d.ReplayId)
                    .HasConstraintName("FK__Replay__ReplayId__3F466844");
            });

            modelBuilder.Entity<Teachers>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.TeacherId).HasColumnName("TeacherID");

                entity.HasOne(d => d.StudentNavigation)
                    .WithMany(p => p.Teachers)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK__UserTB_Us__Stude__276EDEB3");
            });

            modelBuilder.Entity<Baslookup>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__BASLooku__3214EC07A4E6B86E");
                entity.ToTable("BASLookup");
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Title).HasMaxLength(500);
                entity.Property(e => e.Type).HasMaxLength(500);
            });

            #region Set Roles
            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasData(new List<Roles>()
                {
                    new Roles
                    {
                        Id= 1,
                        Name = Tools.RoleName_enum.Administrator.ToString(),
                        NormalizedName = Tools.RoleName_enum.Administrator.ToString().ToUpper(),
                        PersianName= "مالک"
                    },
                    new Roles
                    {
                        Id= 2,
                        Name = Tools.RoleName_enum.Student.ToString(),
                        NormalizedName = Tools.RoleName_enum.Student.ToString().ToUpper(),
                        PersianName = "دانشجو"
                    },
                    new Roles
                    {
                       Id= 3,
                       Name = Tools.RoleName_enum.GuideMaster.ToString(),
                       NormalizedName = Tools.RoleName_enum.GuideMaster.ToString().ToUpper(),
                       PersianName = "استاد راهنما"
                    },
                    new Roles
                    {
                        Id= 4,
                        Name = Tools.RoleName_enum.Adviser.ToString(),
                        NormalizedName = Tools.RoleName_enum.Adviser.ToString().ToUpper(),
                        PersianName = "مشاور"
                    },
                    new Roles
                    {
                        Id= 5,
                        Name = Tools.RoleName_enum.EducationExpert.ToString(),
                        NormalizedName = Tools.RoleName_enum.EducationExpert.ToString().ToUpper(),
                        PersianName = "کارشناس آموزش"
                    },
                    new Roles
                    {
                        Id= 6,
                        Name = Tools.RoleName_enum.PostgraduateEducationExpert.ToString(),
                        NormalizedName = Tools.RoleName_enum.PostgraduateEducationExpert.ToString().ToUpper(),
                        PersianName = "کارشناس تحصیلات تکمیلی"
                    },
                    new Roles
                    {
                        Id= 7,
                        Name = Tools.RoleName_enum.DissertationExpert.ToString(),
                        NormalizedName = Tools.RoleName_enum.DissertationExpert.ToString().ToUpper(),
                        PersianName = "کارشناس امور پایان نامه"
                    },
                });
            });

            #endregion

            #region Set Colleges Of University In Database
            modelBuilder.Entity<Baslookup>(entity =>
            {
                entity.HasData(new List<Baslookup>
            {
                new Baslookup
                {
                    Id = 1,
                    Code= 1,
                    Type="CollegesUni",
                    Title="مهندسي کامپيوتر"
                },
                new Baslookup
                {
                    Id = 2,
                    Code= 2,
                    Type="CollegesUni",
                    Title="مهندسي برق"
                },
                new Baslookup
                {
                    Id = 3,
                    Code= 3,
                    Type="CollegesUni",
                    Title="مهندسي مکانيک"
                },
                new Baslookup
                {
                    Id = 4,
                    Code= 4,
                    Type="CollegesUni",
                    Title="مهندسي عمران"
                },
                new Baslookup
                {
                    Id = 5,
                    Code= 5,
                    Type="CollegesUni",
                    Title="مهندسي معماري و شهرسازی"
                },
                new Baslookup
                {
                    Id = 6,
                    Code= 6,
                    Type="CollegesUni",
                    Title="مهندسي مواد و علوم ميان رشته‌ای"
                },
                new Baslookup
                {
                    Id = 7,
                    Code= 7,
                    Type="CollegesUni",
                    Title="حوضه علوم اسلامي"
                },
                new Baslookup
                {
                    Id = 8,
                    Code= 8,
                    Type="CollegesUni",
                    Title="مرکز آموزش الکترونيکي"
                },
                new Baslookup
                {
                    Id = 9,
                    Code= 9,
                    Type="CollegesUni",
                    Title="علوم پايه"
                },
                new Baslookup
                {
                    Id = 10,
                    Code= 10,
                    Type="CollegesUni",
                    Title="علوم ورزشي"
                },
                new Baslookup
                {
                    Id = 11,
                    Code= 11,
                    Type="CollegesUni",
                    Title="علوم انساني"
                },
            });
            });
            #endregion

            #region Set Dissertation Status
            modelBuilder.Entity<Baslookup>(entity =>
            {
                entity.HasData(new List<Baslookup>
                {
                    new Baslookup
                    {
                        Id = 12,
                        Code= 0,
                        Type = "DissertationStatus",
                         Description= "Register",
                        Title="ثبت اوليه پایان نامه"
                    },new Baslookup
                    {
                        Id = 13,
                        Code= 1,
                        Type = "DissertationStatus",
                        Description= "ConfirmationGuideMaster",
                        Title="تاییدیه استاد راهنمای اول"
                    },new Baslookup
                    {
                        Id = 14,
                        Code= 2,
                        Type = "DissertationStatus",
                        Description = "ConfirmationGuideMaster2", 
                        Title = "تاییدیه استاد راهنمای دوم"
                    },new Baslookup
                    {
                        Id = 15,
                        Code= 3,
                        Type = "DissertationStatus",
                        Description = "ConfirmationGuideMaster3", 
                        Title = "تاییدیه استاد راهنمای سوم"
                    },new Baslookup
                    {
                        Id = 16,
                        Code= 4,
                        Type = "DissertationStatus",
                        Description = "ConfirmationEducationExpert", 
                        Title = "تاییدیه کارشناس آموزش"
                    },new Baslookup
                    {
                        Id = 17,
                        Code= 5,
                        Type = "DissertationStatus",
                        Description = "ConfirmationPostgraduateEducationExpert",
                        Title = "تاییدیه کارشناس تحصیلات تکمیلی"
                    },new Baslookup
                    {
                        Id = 18,
                        Code= 6,
                        Type = "DissertationStatus",
                        Description = "ConfirmationDissertationExpert",
                        Title = "تاییدیه کارشناس امور پایان نامه"
                    },new Baslookup
                    {
                        Id = 19,
                        Code= -3333,
                        Type = "DissertationStatus",
                        Description= "ExpirDissertation",
                        Title="رد پایان نامه"
                    },
                });
            });
            #endregion

            base.OnModelCreating(modelBuilder);
        }

    }
}
