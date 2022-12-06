using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Meeting_App.Data.ResourceCenter.Tables;

namespace Meeting_App.Data.ResourceCenter.RCDbContexts
{
    public partial class RCDbContexts : DbContext
    {
        public RCDbContexts()
        {
        }

        public RCDbContexts(DbContextOptions<RCDbContexts> options)
            : base(options)
        {
        }

        public virtual DbSet<DPT> DPT { get; set; }
        public virtual DbSet<DPT_DETAIL> DPT_DETAIL { get; set; }
        public virtual DbSet<DPT_TYPE> DPT_TYPE { get; set; }
        public virtual DbSet<RESOURCES> RESOURCES { get; set; }
        public virtual DbSet<ROLES> ROLES { get; set; }
        public virtual DbSet<USERS> USERS { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=172.16.15.14;Initial Catalog=Rc_project;user id=rovaid;password=asd@123;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DPT>(entity =>
            {
                entity.HasKey(e => e.DPT_ID)
                    .HasName("DPT_ID_PK");

                entity.HasIndex(e => e.DPT_FOLDER)
                    .HasName("DPT_FOLDER_UQ")
                    .IsUnique();

                entity.HasIndex(e => e.DPT_NAME)
                    .HasName("DPT_NAME_UQ")
                    .IsUnique();

                entity.Property(e => e.DPT_ID).ValueGeneratedNever();

                entity.Property(e => e.DPT_CREATED_ON)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DPT_FOLDER)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.DPT_NAME)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.DPT_UPDATED_ON).HasColumnType("datetime");

                entity.HasOne(d => d.DPT_PARENTNavigation)
                    .WithMany(p => p.InverseDPT_PARENTNavigation)
                    .HasForeignKey(d => d.DPT_PARENT)
                    .HasConstraintName("DPT_PARENT_FK");

                entity.HasOne(d => d.DPT_TYPENavigation)
                    .WithMany(p => p.DPT)
                    .HasForeignKey(d => d.DPT_TYPE)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("DPT_TYPE_FK");
            });

            modelBuilder.Entity<DPT_DETAIL>(entity =>
            {
                entity.HasKey(e => e.DPTD_ID)
                    .HasName("DPTD_DETAIL_PK");

                entity.Property(e => e.DPTD_ID).ValueGeneratedNever();

                entity.Property(e => e.DPTD_BACK_DATE).HasColumnType("date");

                entity.Property(e => e.DPTD_CM_DATE).HasColumnType("date");

                entity.Property(e => e.DPTD_CS_DATE).HasColumnType("date");

                entity.Property(e => e.DPTD_MOVING_DATE).HasColumnType("date");

                entity.Property(e => e.DPTD_OTHER_DATE).HasColumnType("date");

                entity.Property(e => e.DPTD_SF_DATE).HasColumnType("date");

                entity.Property(e => e.DPTD_SH_DATE).HasColumnType("date");

                entity.Property(e => e.DPTD_SL_DATE).HasColumnType("date");

                entity.Property(e => e.DPTD_SPD_DATE).HasColumnType("date");

                entity.Property(e => e.DPTD_SPSHD_DATE).HasColumnType("date");

                entity.Property(e => e.DPTD_SREGULAION_DATE).HasColumnType("date");

                entity.HasOne(d => d.DPTD_RC_)
                    .WithMany(p => p.DPT_DETAIL)
                    .HasForeignKey(d => d.DPTD_RC_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("DPTD_RC_ID");
            });

            modelBuilder.Entity<DPT_TYPE>(entity =>
            {
                entity.HasKey(e => e.DT_ID)
                    .HasName("DT_ID_PK");

                entity.HasIndex(e => e.DT_NAME)
                    .HasName("DT_NAME_UQ")
                    .IsUnique();

                entity.Property(e => e.DT_ID).ValueGeneratedNever();

                entity.Property(e => e.DT_CREATED_ON)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DT_NAME)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.DT_UPDATED_ON).HasColumnType("datetime");
            });

            modelBuilder.Entity<RESOURCES>(entity =>
            {
                entity.HasKey(e => e.RESOURCE_ID)
                    .HasName("RESOURCE_ID_PK");

                entity.Property(e => e.RESOURCE_ID).ValueGeneratedNever();

                entity.Property(e => e.RESOURCE_CREATED_ON)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RESOURCE_DESCRIPTION).HasColumnType("text");

                entity.Property(e => e.RESOURCE_FILE_PDF)
                    .IsRequired()
                    .HasMaxLength(700);

                entity.Property(e => e.RESOURCE_FILE_WORD)
                    .IsRequired()
                    .HasMaxLength(700);

                entity.Property(e => e.RESOURCE_TITLE)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.RESOURCE_UPDATED_ON).HasColumnType("datetime");

                entity.HasOne(d => d.RESOURCE_CREATED_BYNavigation)
                    .WithMany(p => p.RESOURCES)
                    .HasForeignKey(d => d.RESOURCE_CREATED_BY)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RESOURCE_USER_ID_PK");

                entity.HasOne(d => d.RESOURCE_DPTNavigation)
                    .WithMany(p => p.RESOURCES)
                    .HasForeignKey(d => d.RESOURCE_DPT)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RESOURCE_DPT_FK");
            });

            modelBuilder.Entity<ROLES>(entity =>
            {
                entity.HasKey(e => e.ROLE_ID)
                    .HasName("ROLE_ID_PK");

                entity.HasIndex(e => e.ROLE_NAME)
                    .HasName("ROLE_ID_UQ")
                    .IsUnique();

                entity.Property(e => e.ROLE_ID).ValueGeneratedNever();

                entity.Property(e => e.ROLE_CREATED_ON)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ROLE_NAME)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ROLE_UPDATED_ON).HasColumnType("datetime");

                entity.HasOne(d => d.ROLE_DPTNavigation)
                    .WithMany(p => p.ROLES)
                    .HasForeignKey(d => d.ROLE_DPT)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ROLE_DDPT_FK");
            });

            modelBuilder.Entity<USERS>(entity =>
            {
                entity.HasKey(e => e.USER_ID)
                    .HasName("USER_ID_PK");

                entity.HasIndex(e => e.USER_USERNAME)
                    .HasName("USER_USERNAME_UQ")
                    .IsUnique();

                entity.Property(e => e.USER_ID).ValueGeneratedNever();

                entity.Property(e => e.USER_CREATED_ON)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.USER_NAME).HasMaxLength(100);

                entity.Property(e => e.USER_PASSWORD)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.USER_PHONE).HasMaxLength(15);

                entity.Property(e => e.USER_UPDATED_ON).HasColumnType("datetime");

                entity.Property(e => e.USER_USERNAME)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.remember_token).HasMaxLength(100);

                entity.HasOne(d => d.USER_ROLENavigation)
                    .WithMany(p => p.USERS)
                    .HasForeignKey(d => d.USER_ROLE)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("USER_ROLE_FK");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
