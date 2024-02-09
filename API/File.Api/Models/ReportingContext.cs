using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace File.Api.Models
{
    public partial class ReportingContext : DbContext
    {
        public ConfigurationManager _configurationManager { get; set; }

        public ReportingContext()
        {
        }

        //public ReportingContext(DbContextOptions<ReportingContext> options, ConfigurationManager configurationManager)
        //    : base(options)
        //{
        //    _configurationManager = configurationManager;
        //}

        public virtual DbSet<TransmissionStatusReport> TransmissionStatusReports { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var cs = "Server=localhost;Database=SafetyReporting;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;";
            optionsBuilder.UseSqlServer(cs, x => x.CommandTimeout(1800));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransmissionStatusReport>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.PartitionToggleField })
                    .HasName("PK_TransmissionStatusReportId");

                entity.ToTable("TransmissionStatusReport");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.PartitionToggleField)
                    .HasColumnName("partition_toggle_field")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Compound).HasMaxLength(100);

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.CountryCod)
                    .HasMaxLength(2)
                    .IsFixedLength();

                entity.Property(e => e.CrouserAssigned).HasColumnName("CROUserAssigned");

                entity.Property(e => e.CrouserName).HasColumnName("CROUserName");

                entity.Property(e => e.DateSponsorNotified).HasColumnType("datetime");

                entity.Property(e => e.DocumentAccessStatusAtSite).HasMaxLength(50);

                entity.Property(e => e.DocumentHistoryStatus).HasMaxLength(100);

                entity.Property(e => e.DocumentName).HasMaxLength(500);

                entity.Property(e => e.DocumentType).HasMaxLength(100);

                entity.Property(e => e.DocumentUploadDate).HasColumnType("datetime");

                entity.Property(e => e.Drug).HasMaxLength(100);

                entity.Property(e => e.EventCountry).HasMaxLength(100);

                entity.Property(e => e.EventDate).HasColumnType("datetime");

                entity.Property(e => e.EventProtocol).HasMaxLength(100);

                entity.Property(e => e.Mcn)
                    .HasMaxLength(100)
                    .HasColumnName("MCN");

                entity.Property(e => e.NameOfInvestigator).HasMaxLength(150);

                entity.Property(e => e.PackageDate).HasColumnType("datetime");

                entity.Property(e => e.Protocol).HasMaxLength(100);

                entity.Property(e => e.RecipientCtprole)
                    .HasMaxLength(150)
                    .HasColumnName("RecipientCTPRole");

                entity.Property(e => e.RecipientEmail).HasMaxLength(255);

                entity.Property(e => e.RecipientName).HasMaxLength(150);

                entity.Property(e => e.Sdhid).HasColumnName("SDHId");

                entity.Property(e => e.SentTransmissionDate).HasColumnType("datetime");

                entity.Property(e => e.SiteDetails).HasMaxLength(500);

                entity.Property(e => e.SiteFax).HasMaxLength(100);

                entity.Property(e => e.SiteMailingAddress).HasMaxLength(250);

                entity.Property(e => e.SitePhone).HasMaxLength(100);

                entity.Property(e => e.Slid).HasColumnName("SLId");

                entity.Property(e => e.Spid).HasColumnName("SPId");

                entity.Property(e => e.SponsorCausalityAssessmentFlag)
                    .HasMaxLength(3)
                    .IsFixedLength();

                entity.Property(e => e.TransmissionStatus).HasMaxLength(50);

                entity.Property(e => e.UnanticipatedFlag).HasMaxLength(100);

                entity.Property(e => e.UserName).HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

    public class DbContextFactory : IDbContextFactory<ReportingContext>
    {
        public ReportingContext CreateDbContext()
        {
            return new ReportingContext();
        }
    }
}
