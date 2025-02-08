using SchoolManage.DAL.Entities;
using SchoolManage.DAL.Seeds;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;


namespace SchoolManage.DAL;

public class SchoolManageDbContext : DbContext
{
    private readonly bool _seedDemoData;

    public SchoolManageDbContext(DbContextOptions contextOptions, bool seedDemoData = false)
        : base(contextOptions)
    {
        _seedDemoData = seedDemoData;
    }
    public DbSet<ActivityEntity> Activities => Set<ActivityEntity>();
    public DbSet<EvaluationEntity> Evaluations => Set<EvaluationEntity>();
    public DbSet<StudentEntity> Students => Set<StudentEntity>();
    public DbSet<SubjectEntity> Subjects => Set<SubjectEntity>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<StudentEntity>()
            .HasMany(student => student.Subjects)
            .WithMany(subject => subject.Students);

        modelBuilder.Entity<StudentEntity>()
            .HasMany(student => student.Evaluations)
            .WithOne(evaluation => evaluation.Students);

        modelBuilder.Entity<SubjectEntity>()
            .HasMany(subject => subject.Activities)
            .WithOne(activity => activity.Subjects);

        modelBuilder.Entity<SubjectEntity>()
            .HasMany(subject => subject.Students)
            .WithMany(student => student.Subjects);

        modelBuilder.Entity<EvaluationEntity>()
            .HasOne(evaluation => evaluation.Activities)
            .WithMany(activity => activity.Evaluations);

        modelBuilder.Entity<EvaluationEntity>()
            .HasOne(evaluation => evaluation.Students)
            .WithMany(student => student.Evaluations);

        modelBuilder.Entity<ActivityEntity>()
            .HasOne(activity => activity.Subjects)
            .WithMany(subject => subject.Activities)
            .HasForeignKey(activity => activity.SubjectsId);

        modelBuilder.Entity<ActivityEntity>()
            .HasMany(activity => activity.Evaluations)
            .WithOne(evaluation => evaluation.Activities);


        if (_seedDemoData)
        {
            SubjectSeeds.Seed(modelBuilder);
            StudentSeeds.Seed(modelBuilder);
            ActivitySeeds.Seed(modelBuilder);
            EvaluationSeeds.Seed(modelBuilder);
        }
    }

}
