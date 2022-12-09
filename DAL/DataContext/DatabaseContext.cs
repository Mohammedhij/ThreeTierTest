using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static DAL.DataContext.DatabaseContext;



namespace DAL.DataContext
{
    public class DatabaseContext : DbContext
    {
        public class OptionsBuild
        {
            public OptionsBuild()
            {
                Settings = new AppConfiguration();

                OptionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();

                OptionsBuilder.UseSqlServer(Settings.SqlConnectionString);

                DataBaseOptions = OptionsBuilder.Options;

            
            }
            public DbContextOptionsBuilder<DatabaseContext> OptionsBuilder { get; set; }
            public DbContextOptions<DatabaseContext> DataBaseOptions { get; set; }
            public AppConfiguration Settings { get; set; }
        }

       public static OptionsBuild Options= new OptionsBuild();


        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Applicants> applicants { get; set; }

        public DbSet<Application> application { get; set; }
        
        public DbSet<ApplicationStatus> applicationStatus { get; set; }

        public DbSet<Grade> grades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //SET CUSTOM DEFAULT VALUE ON CREATION
            //MODIFIED DATE: 
            DateTime modifiedDate = new DateTime(1900, 1, 1);

            #region Applicant
            modelBuilder.Entity<Applicants>().ToTable("applicant");
            //Primary Key & Identity Column
            modelBuilder.Entity<Applicants>().HasKey(ap => ap.Applicant_ID);
            modelBuilder.Entity<Applicants>().Property(ap => ap.Applicant_ID).UseIdentityColumn(1, 1).IsRequired().HasColumnName("applicant_id");
            //COLUMN SETTINGS
            modelBuilder.Entity<Applicants>().Property(ap => ap.Applicant_Name).IsRequired(true).HasMaxLength(100).HasColumnName("applicant_name");
            modelBuilder.Entity<Applicants>().Property(ap => ap.Applicant_Surname).IsRequired(true).HasMaxLength(100).HasColumnName("applicant_surname");
            modelBuilder.Entity<Applicants>().Property(ap => ap.Applicant_BirthDate).IsRequired(true).HasColumnName("applicant_birthdate");
            modelBuilder.Entity<Applicants>().Property(ap => ap.Contact_Email).IsRequired(false).HasMaxLength(250).HasColumnName("contact_email");//(no Applicant_)Will be guardians/parents Email
            modelBuilder.Entity<Applicants>().Property(ap => ap.Contact_Number).IsRequired(true).HasMaxLength(25).HasColumnName("contact_number");//(no Applicant_)Might not be the applicants email, could be guardians/parents
            modelBuilder.Entity<Applicants>().Property(ap => ap.applicant_creationDate).IsRequired(true).HasDefaultValue(DateTime.UtcNow).HasColumnName("applicant_creationdate");
            modelBuilder.Entity<Applicants>().Property(ap => ap.applicant_ModifiedDate).IsRequired(true).HasDefaultValue(modifiedDate).HasColumnName("applicant_modifieddate");
            //RelationShips
            modelBuilder.Entity<Applicants>()
                   .HasMany<Application>(app => app.Applications)
                   .WithOne(ap => ap.Applicants)
                   .HasForeignKey(app => app.Applicant_ID)
                   .OnDelete(DeleteBehavior.Restrict);//Can't delete an applicants info Ever, We can update it though.
            #endregion

            #region Application Status
            modelBuilder.Entity<ApplicationStatus>().ToTable("application_status");
            //Primary Key & Identity Column
            modelBuilder.Entity<ApplicationStatus>().HasKey(apps => apps.ApplicationStatus_ID);
            modelBuilder.Entity<ApplicationStatus>().Property(apps => apps.ApplicationStatus_ID).UseIdentityColumn(1, 1).IsRequired().HasColumnName("application_status_id");
            //COLUMN SETTINGS
            modelBuilder.Entity<ApplicationStatus>().HasIndex(apps => apps.ApplicationStatus_Name).IsUnique(); // Application Status Name is Unique
            modelBuilder.Entity<ApplicationStatus>().Property(apps => apps.ApplicationStatus_Name).IsRequired(true).HasMaxLength(100).HasColumnName("application_status_name");
            modelBuilder.Entity<ApplicationStatus>().Property(apps => apps.ApplicationStatus_CreationDate).IsRequired(true).HasDefaultValue(DateTime.UtcNow).HasColumnName("application_status_creationdate");
            modelBuilder.Entity<ApplicationStatus>().Property(ap => ap.ApplicationStatus_ModifiedDate).IsRequired(true).HasDefaultValue(modifiedDate).HasColumnName("application_status_modifieddate");

            //RelationShips
            modelBuilder.Entity<ApplicationStatus>()
                   .HasMany<Application>(app => app.Applications)
                   .WithOne(apps => apps.ApplicationStatus)
                   .HasForeignKey(app => app.ApplicationStatus_ID)
                   .OnDelete(DeleteBehavior.Restrict);//Can't delete an ApplicationStatus, We can update it though.
            #endregion

            #region Grade
            modelBuilder.Entity<Grade>().ToTable("grade");
            //Primary Key & Identity Column
            modelBuilder.Entity<Grade>().HasKey(g => g.Grade_ID);
            modelBuilder.Entity<Grade>().Property(g => g.Grade_ID).UseIdentityColumn(1, 1).IsRequired().HasColumnName("grade_id");
            //COLUMN SETTINGS
            modelBuilder.Entity<Grade>().Property(g => g.Grade_Name).IsRequired(true).HasMaxLength(100).HasColumnName("grade_name");
            modelBuilder.Entity<Grade>().Property(g => g.Grade_Number).IsRequired(true).HasColumnName("grade_number");
            modelBuilder.Entity<Grade>().HasIndex(g => g.Grade_Name).IsUnique(); // Grade Name is Unique
            modelBuilder.Entity<Grade>().HasIndex(g => g.Grade_Number).IsUnique(); // Grade Number is Unique
            modelBuilder.Entity<Grade>().Property(g => g.Grade_Capacity).IsRequired(true).HasColumnName("grade_capacity");
            modelBuilder.Entity<Grade>().Property(g => g.Grade_CreationDate).IsRequired(true).HasDefaultValue(DateTime.UtcNow).HasColumnName("grade_creationdate");
            modelBuilder.Entity<Grade>().Property(g => g.Grade_ModifiedDate).IsRequired(true).HasDefaultValue(modifiedDate).HasColumnName("grade_modifieddate");

            //RelationShips
            modelBuilder.Entity<Grade>()
                   .HasMany<Application>(g => g.Applications)
                   .WithOne(app => app.Grade)
                   .HasForeignKey(app => app.Grade_ID)
                   .OnDelete(DeleteBehavior.Restrict);//Can't delete a Grade Ever, We can update it though.
            #endregion

            #region Application
            modelBuilder.Entity<Application>().ToTable("application");
            //Primary Key & Identity Column
            modelBuilder.Entity<Application>().HasKey(app => app.Application_ID);
            modelBuilder.Entity<Application>().Property(app => app.Application_ID).UseIdentityColumn(1, 1).IsRequired().HasColumnName("application_id");
            //COLUMN SETTINGS
            modelBuilder.Entity<Application>().Property(app => app.Applicant_ID).IsRequired(true).HasColumnName("applicant_id");
            modelBuilder.Entity<Application>().Property(app => app.Grade_ID).IsRequired(true).HasColumnName("grade_id");
            modelBuilder.Entity<Application>().Property(app => app.ApplicationStatus_ID).IsRequired(true).HasColumnName("application_status_id");
            modelBuilder.Entity<Application>().Property(app => app.Application_CreationDate).IsRequired(true).HasDefaultValue(DateTime.UtcNow).HasColumnName("application_creationdate");
            modelBuilder.Entity<Application>().Property(app => app.Application_ModifiedDate).IsRequired(true).HasDefaultValue(modifiedDate).HasColumnName("application_modifieddate");
            modelBuilder.Entity<Application>().Property(app => app.SchoolYear).IsRequired(true).HasColumnName("application_schoolyear");
            //Relationships

            //Applicant link
            modelBuilder.Entity<Application>()
                 .HasOne<Applicants>(app => app.Applicants)
                 .WithMany(ap => ap.Applications)//CAN HAVE MANY APPLICATIONS
                 .HasForeignKey(app => app.Applicant_ID)
                 .OnDelete(DeleteBehavior.NoAction);//Can delete an application.

            //Grade link
            modelBuilder.Entity<Application>()
                 .HasOne<Grade>(app => app.Grade)
                 .WithMany(ap => ap.Applications)//Grade is linked to many applications
                 .HasForeignKey(app => app.Grade_ID)
                 .OnDelete(DeleteBehavior.NoAction);//Can delete an application.

            //Status link
            modelBuilder.Entity<Application>()
                .HasOne<ApplicationStatus>(app => app.ApplicationStatus)
                .WithMany(ap => ap.Applications)//Status is linked to many applications
                .HasForeignKey(app => app.ApplicationStatus_ID)
                .OnDelete(DeleteBehavior.NoAction);//Can delete an application.
            #endregion
        }



    }
}
