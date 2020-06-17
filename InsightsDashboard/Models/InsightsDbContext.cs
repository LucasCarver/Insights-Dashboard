using System;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace InsightsDashboard.Models
{
    public partial class InsightsDbContext : DbContext
    {
        public IConfiguration Configuration { get; }
        public InsightsDbContext()
        {
        }

        public InsightsDbContext(DbContextOptions<InsightsDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<SeamlessMaster> SeamlessMaster { get; set; }
        public virtual DbSet<Tags> Tags { get; set; }
        public virtual DbSet<UserPreferences> UserPreferences { get; set; }
        public virtual DbSet<UserStartup> UserStartup { get; set; }
        public virtual DbSet<UserTags> UserTags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<SeamlessMaster>(entity =>
            {
                entity.HasKey(e => e.Identifier)
                    .HasName("PK__Seamless__821FB018F3E1244F");

                entity.Property(e => e.Identifier).HasMaxLength(20);

                entity.Property(e => e.Comment).HasMaxLength(200);
            });

            modelBuilder.Entity<Tags>(entity =>
            {
                entity.HasKey(e => e.TagId)
                    .HasName("PK__Tags__657CF9ACBCA51EEC");

                entity.Property(e => e.TagName).HasMaxLength(40);
            });

            modelBuilder.Entity<UserPreferences>(entity =>
            {
                entity.HasKey(e => e.PrefId)
                    .HasName("PK__UserPref__1F832A200E32A3EB");

                entity.Property(e => e.FollowedAlignment).HasMaxLength(80);

                entity.Property(e => e.FollowedCountry).HasMaxLength(80);

                entity.Property(e => e.StartUpDescription).HasMaxLength(250);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPreferences)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__UserPrefe__UserI__5EBF139D");
            });

            modelBuilder.Entity<UserStartup>(entity =>
            {
                entity.Property(e => e.Alignment).HasMaxLength(100);

                entity.Property(e => e.City).HasMaxLength(90);

                entity.Property(e => e.Comments).HasMaxLength(200);

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CompanyWebsite).HasMaxLength(250);

                entity.Property(e => e.Country).HasMaxLength(80);

                entity.Property(e => e.DateAdded).HasColumnType("date");

                entity.Property(e => e.Landscape).HasMaxLength(30);

                entity.Property(e => e.Raised).HasMaxLength(20);

                entity.Property(e => e.ReviewDate).HasColumnType("date");

                entity.Property(e => e.Scout).HasMaxLength(30);

                entity.Property(e => e.Source).HasMaxLength(40);

                entity.Property(e => e.StateProvince).HasMaxLength(90);

                entity.Property(e => e.Technology).HasMaxLength(60);

                entity.Property(e => e.Theme).HasMaxLength(90);

                entity.Property(e => e.TwoLineSummary).HasMaxLength(200);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserStartup)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserStart__UserI__6754599E");
            });

            modelBuilder.Entity<UserTags>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__UserTags__UserId__628FA481");

                entity.HasOne(d => d.UserTag)
                    .WithMany()
                    .HasForeignKey(d => d.UserTagId)
                    .HasConstraintName("FK__UserTags__UserTa__6383C8BA");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
