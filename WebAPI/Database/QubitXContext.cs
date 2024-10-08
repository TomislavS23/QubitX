﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

public partial class QubitXContext : DbContext
{
    public QubitXContext()
    {
    }

    public QubitXContext(DbContextOptions<QubitXContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseTag> CourseTags { get; set; }

    public virtual DbSet<CourseType> CourseTypes { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserCourse> UserCourses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:server");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.IdCourse).HasName("PK__course__FB82D9EA1E7C3B2E");

            entity.ToTable("course");

            entity.HasIndex(e => e.CourseTitle, "UQ__course__622E10B6CD60AB71").IsUnique();

            entity.Property(e => e.IdCourse).HasColumnName("id_course");
            entity.Property(e => e.CourseContent).HasColumnName("course_content");
            entity.Property(e => e.CourseTitle)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("course_title");
            entity.Property(e => e.IdCourseType).HasColumnName("id_course_type");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdCourseTypeNavigation).WithMany(p => p.Courses)
                .HasForeignKey(d => d.IdCourseType)
                .HasConstraintName("FK__course__id_cours__04E4BC85");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Courses)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK__course__id_user__03F0984C");
        });

        modelBuilder.Entity<CourseTag>(entity =>
        {
            entity.HasKey(e => e.IdCourseTag).HasName("PK__course_t__0F4728A45601370F");

            entity.ToTable("course_tag");

            entity.Property(e => e.IdCourseTag).HasColumnName("id_course_tag");
            entity.Property(e => e.IdCourse).HasColumnName("id_course");
            entity.Property(e => e.IdTag).HasColumnName("id_tag");

            entity.HasOne(d => d.IdCourseNavigation).WithMany(p => p.CourseTags)
                .HasForeignKey(d => d.IdCourse)
                .HasConstraintName("FK__course_ta__id_co__0B91BA14");

            entity.HasOne(d => d.IdTagNavigation).WithMany(p => p.CourseTags)
                .HasForeignKey(d => d.IdTag)
                .HasConstraintName("FK__course_ta__id_ta__0C85DE4D");
        });

        modelBuilder.Entity<CourseType>(entity =>
        {
            entity.HasKey(e => e.IdCourseType).HasName("PK__course_t__008F5007442A4CFC");

            entity.ToTable("course_type");

            entity.HasIndex(e => e.CourseTypeTitle, "UQ__course_t__318509552ACDA7ED").IsUnique();

            entity.Property(e => e.IdCourseType).HasColumnName("id_course_type");
            entity.Property(e => e.CourseTypeTitle)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("course_type_title");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.IdLog).HasName("PK__log__6CC851FE042FD841");

            entity.ToTable("log");

            entity.Property(e => e.IdLog).HasColumnName("id_log");
            entity.Property(e => e.LogLevel).HasColumnName("log_level");
            entity.Property(e => e.LogMessage)
                .IsUnicode(false)
                .HasColumnName("log_message");
            entity.Property(e => e.LogTimestamp)
                .HasColumnType("datetime")
                .HasColumnName("log_timestamp");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PK__role__3D48441D33EC2D62");

            entity.ToTable("role");

            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.RoleType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role_type");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.IdTag).HasName("PK__tag__6A2987F1EA89081A");

            entity.ToTable("tag");

            entity.HasIndex(e => e.TagTitle, "UQ__tag__9C9390612E6E7A89").IsUnique();

            entity.Property(e => e.IdTag).HasColumnName("id_tag");
            entity.Property(e => e.TagTitle)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tag_title");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__user__D2D14637ED73AF74");

            entity.ToTable("user");

            entity.HasIndex(e => e.Username, "UQ__user__F3DBC572F96451D3").IsUnique();

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.HashedPassword)
                .IsUnicode(false)
                .HasColumnName("hashed_password");
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("username");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .HasConstraintName("FK__user__id_role__00200768");
        });

        modelBuilder.Entity<UserCourse>(entity =>
        {
            entity.HasKey(e => e.IdUserCourse).HasName("PK__user_cou__3AEEB9EC59A8E4F7");

            entity.ToTable("user_course");

            entity.Property(e => e.IdUserCourse).HasColumnName("id_user_course");
            entity.Property(e => e.IdCourse).HasColumnName("id_course");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdCourseNavigation).WithMany(p => p.UserCourses)
                .HasForeignKey(d => d.IdCourse)
                .HasConstraintName("FK__user_cour__id_co__08B54D69");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.UserCourses)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK__user_cour__id_us__07C12930");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
