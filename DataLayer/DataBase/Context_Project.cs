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
        

        //public DbSet<CommentSelfRelation> CommentSelfRelations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.TeacherStudents)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK__UserTB_Us__Stude__276EDEB3");

                entity.HasOne(d => d.TeacherNavigation)
                    .WithMany(p => p.TeacherTeacherNavigations)
                    .HasForeignKey(d => d.TeacherId)
                    .HasConstraintName("FK__UserTB_Us__Teach__267ABA7A");
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

            #region Set Admin

            #endregion
            base.OnModelCreating(modelBuilder);
        }

    }
}
