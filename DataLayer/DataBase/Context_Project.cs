﻿using DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataBase
{
    public class Context_Project : IdentityDbContext<Users, Roles, ulong>
    {
        public Context_Project(DbContextOptions<Context_Project> Option) : base(Option)
        { }


        // --------------  DbSets  ---------------------------
        public DbSet<Comments>? Comments { get; set; }
        public DbSet<Dissertations>? Dissertations { get; set; }
        public DbSet<KeyWord>? KeyWords { get; set; }
        public DbSet<Logs>? Logs { get; set; }
        public DbSet<Entities.Confirmations> Confirmation { get; set; }
        public DbSet<ConfirmationsDissertations>? ConfirmationsDissertations { get; set; }
        public DbSet<User_User_Relation> User_User_Relation { get; set; }
        public DbSet<Comment_User> CommentsUser { get; set; }

        //public DbSet<CommentSelfRelation> CommentSelfRelations { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("dbo");

            // First Confing
            builder.Entity<Users>().ToTable("Users");


            //Users Confing
            builder.Entity<Users>().Property(t => t.College).IsRequired(false);
            builder.Entity<Users>().Ignore(t => t.PhoneNumber);
            builder.Entity<Users>().Ignore(t => t.PhoneNumberConfirmed);
            builder.Entity<Users>().Property(t => t.NationalCode).IsRequired(false);
            builder.Entity<Users>().Property(t => t.Email).IsRequired(true);
            builder.Entity<Users>().Property(t => t.FirstName).IsRequired(false);
            builder.Entity<Users>().Property(t => t.LastName).IsRequired(false);

            // Comments Confing
            builder.Entity<Comments>().Property(t => t.Description).IsRequired(true);

            // Dissertation
            builder.Entity<Dissertations>().Property(t => t.Dissertation_FileName).IsRequired(true);

            // Role
            builder.Entity<Roles>().ToTable("Roles");
            builder.Entity<Roles>().Property(t => t.Name_Persian).IsRequired(true);
            #region Data For Role
            builder.Entity<Roles>().HasData(new Entities.Roles()
            {
                Id = 2,
                Name = Tools.RoleName_enum.Administrator.ToString(),
                NormalizedName = Tools.RoleName_enum.Administrator.ToString().ToUpper(),
                Name_Persian = "مالک"
            });
            builder.Entity<Roles>().HasData(new Entities.Roles()
            {
                Id = 3,
                Name = Tools.RoleName_enum.Student.ToString(),
                NormalizedName = Tools.RoleName_enum.Student.ToString().ToUpper(),
                Name_Persian = "دانشجو"
            });
            builder.Entity<Roles>().HasData(new Entities.Roles()
            {
                Id = 4,
                Name = Tools.RoleName_enum.GuideMaster.ToString(),
                NormalizedName = Tools.RoleName_enum.GuideMaster.ToString().ToUpper(),
                Name_Persian = "استاد راهنما"
            });
            builder.Entity<Roles>().HasData(new Entities.Roles()
            {
                Id = 5,
                Name = Tools.RoleName_enum.Adviser.ToString(),
                NormalizedName = Tools.RoleName_enum.Adviser.ToString().ToUpper(),
                Name_Persian = "مشاور"
            });
            builder.Entity<Roles>().HasData(new Entities.Roles()
            {
                Id = 6,
                Name = Tools.RoleName_enum.EducationExpert.ToString(),
                NormalizedName = Tools.RoleName_enum.EducationExpert.ToString().ToUpper(),
                Name_Persian = "کارشناس آموزش"
            });
            builder.Entity<Roles>().HasData(new Entities.Roles()
            {
                Id = 7,
                Name = Tools.RoleName_enum.PostgraduateEducationExpert.ToString(),
                NormalizedName = Tools.RoleName_enum.PostgraduateEducationExpert.ToString().ToUpper(),
                Name_Persian = "کارشناس تحصیلات تکمیلی"
            });
            builder.Entity<Roles>().HasData(new Entities.Roles()
            {
                Id = 8,
                Name = Tools.RoleName_enum.DissertationExpert.ToString(),
                NormalizedName = Tools.RoleName_enum.DissertationExpert.ToString().ToUpper(),
                Name_Persian = "کارشناس امور پایان نامه"
            });
            #endregion

            // Logs
            builder.Entity<Logs>().HasKey(x => x.Id);
            builder.Entity<Logs>().Property(x => x.Message).IsRequired(true);

            // Data For Confirmations
            #region Data Confirmations
            builder.Entity<Confirmations>().HasData(new Confirmations()
            {
                Id = 1,
                Code_Dissertation_Confirmation = Tools.Dissertation_Confirmations.ConfirmationGuideMaster,
                Name = Tools.Dissertation_Confirmations.ConfirmationGuideMaster.ToString(),
                PersianName = "تاییدیه استاد راهنمای اول"
            });
            builder.Entity<Confirmations>().HasData(new Confirmations()
            {
                Id = 2,
                Code_Dissertation_Confirmation = Tools.Dissertation_Confirmations.ConfirmationGuideMaster2,
                Name = Tools.Dissertation_Confirmations.ConfirmationGuideMaster2.ToString(),
                PersianName = "تاییدیه استاد راهنمای دوم"
            });
            builder.Entity<Confirmations>().HasData(new Confirmations()
            {
                Id = 3,
                Code_Dissertation_Confirmation =
                 Tools.Dissertation_Confirmations.ConfirmationGuideMaster3,
                Name =
                  Tools.Dissertation_Confirmations.ConfirmationGuideMaster3.ToString(),
                PersianName = "تاییدیه استاد راهنمای سوم"
            });
            builder.Entity<Confirmations>().HasData(new Confirmations()
            {
                Id = 4,
                Code_Dissertation_Confirmation =
                Tools.Dissertation_Confirmations.ConfirmationEducationExpert,
                Name =
                 Tools.Dissertation_Confirmations.ConfirmationEducationExpert.ToString(),
                PersianName = "تاییدیه کارشناس آموزش"
            });
            builder.Entity<Confirmations>().HasData(new Confirmations()
            {
                Id = 5,
                Code_Dissertation_Confirmation =
                Tools.Dissertation_Confirmations.ConfirmationPostgraduateEducationExpert,
                Name = 
                Tools.Dissertation_Confirmations.ConfirmationPostgraduateEducationExpert.ToString(),
                PersianName ="تاییدیه کارشناس تحصیلات تکمیلی"
            });
            builder.Entity<Confirmations>().HasData(new Confirmations()
            {
                Id = 6,
                Code_Dissertation_Confirmation=
                Tools.Dissertation_Confirmations.ConfirmationDissertationExpert,
                Name = 
                Tools.Dissertation_Confirmations.ConfirmationDissertationExpert.ToString(),
                PersianName = "تاییدیه کارشناس امور پایان نامه"
            });
            #endregion

        }
    }
}
