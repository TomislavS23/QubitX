using System;
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

    public virtual DbSet<CourseSection> CourseSections { get; set; }

    public virtual DbSet<CourseTag> CourseTags { get; set; }

    public virtual DbSet<CourseType> CourseTypes { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SectionType> SectionTypes { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserCourse> UserCourses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.IdCourse).HasName("PK__course__FB82D9EA756A7E05");

            entity.ToTable("course");

            entity.HasIndex(e => e.Title, "UQ__course__E52A1BB30A267CA8").IsUnique();

            entity.Property(e => e.IdCourse).HasColumnName("id_course");
            entity.Property(e => e.IdCourseType).HasColumnName("id_course_type");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");

            entity.HasOne(d => d.IdCourseTypeNavigation).WithMany(p => p.Courses)
                .HasForeignKey(d => d.IdCourseType)
                .HasConstraintName("FK__course__id_cours__44FF419A");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Courses)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK__course__id_user__440B1D61");
        });

        modelBuilder.Entity<CourseSection>(entity =>
        {
            entity.HasKey(e => e.IdCourseSection).HasName("PK__course_s__B63C6936947A544A");

            entity.ToTable("course_section");

            entity.Property(e => e.IdCourseSection).HasColumnName("id_course_section");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.IdCourse).HasColumnName("id_course");
            entity.Property(e => e.IdSectionType).HasColumnName("id_section_type");

            entity.HasOne(d => d.IdCourseNavigation).WithMany(p => p.CourseSections)
                .HasForeignKey(d => d.IdCourse)
                .HasConstraintName("FK__course_se__id_co__52593CB8");

            entity.HasOne(d => d.IdSectionTypeNavigation).WithMany(p => p.CourseSections)
                .HasForeignKey(d => d.IdSectionType)
                .HasConstraintName("FK__course_se__id_se__534D60F1");
        });

        modelBuilder.Entity<CourseTag>(entity =>
        {
            entity.HasKey(e => e.IdCourseTag).HasName("PK__course_t__0F4728A4D5A67056");

            entity.ToTable("course_tag");

            entity.Property(e => e.IdCourseTag).HasColumnName("id_course_tag");
            entity.Property(e => e.IdCourse).HasColumnName("id_course");
            entity.Property(e => e.IdTag).HasColumnName("id_tag");

            entity.HasOne(d => d.IdCourseNavigation).WithMany(p => p.CourseTags)
                .HasForeignKey(d => d.IdCourse)
                .HasConstraintName("FK__course_ta__id_co__4E88ABD4");

            entity.HasOne(d => d.IdTagNavigation).WithMany(p => p.CourseTags)
                .HasForeignKey(d => d.IdTag)
                .HasConstraintName("FK__course_ta__id_ta__4F7CD00D");
        });

        modelBuilder.Entity<CourseType>(entity =>
        {
            entity.HasKey(e => e.IdCourseType).HasName("PK__course_t__008F500777BD0C07");

            entity.ToTable("course_type");

            entity.HasIndex(e => e.CourseType1, "UQ__course_t__56F86B466BB4CABE").IsUnique();

            entity.Property(e => e.IdCourseType).HasColumnName("id_course_type");
            entity.Property(e => e.CourseType1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("course_type");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.IdLog).HasName("PK__log__6CC851FE816CAEE9");

            entity.ToTable("log");

            entity.Property(e => e.IdLog).HasColumnName("id_log");
            entity.Property(e => e.Content)
                .IsUnicode(false)
                .HasColumnName("content");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PK__role__3D48441DECACB133");

            entity.ToTable("role");

            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.RoleType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role_type");
        });

        modelBuilder.Entity<SectionType>(entity =>
        {
            entity.HasKey(e => e.IdSectionType).HasName("PK__section___29FA8A96C72B81A6");

            entity.ToTable("section_type");

            entity.HasIndex(e => e.SectionType1, "UQ__section___01059213A3739CD2").IsUnique();

            entity.Property(e => e.IdSectionType).HasColumnName("id_section_type");
            entity.Property(e => e.SectionType1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("section_type");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.IdTag).HasName("PK__tag__6A2987F1503BF8BA");

            entity.ToTable("tag");

            entity.HasIndex(e => e.TagTitle, "UQ__tag__9C93906167B154A0").IsUnique();

            entity.Property(e => e.IdTag).HasColumnName("id_tag");
            entity.Property(e => e.TagTitle)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("tag_title");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__user__D2D1463768A987E2");

            entity.ToTable("user");

            entity.HasIndex(e => e.Username, "UQ__user__F3DBC5723557D14A").IsUnique();

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
                .HasConstraintName("FK__user__id_role__403A8C7D");
        });

        modelBuilder.Entity<UserCourse>(entity =>
        {
            entity.HasKey(e => e.IdUserCourse).HasName("PK__user_cou__3AEEB9ECD64C36A7");

            entity.ToTable("user_course");

            entity.Property(e => e.IdUserCourse).HasColumnName("id_user_course");
            entity.Property(e => e.IdCourse).HasColumnName("id_course");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdCourseNavigation).WithMany(p => p.UserCourses)
                .HasForeignKey(d => d.IdCourse)
                .HasConstraintName("FK__user_cour__id_co__4BAC3F29");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.UserCourses)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK__user_cour__id_us__4AB81AF0");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
