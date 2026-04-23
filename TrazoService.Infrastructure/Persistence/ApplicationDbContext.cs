using Microsoft.EntityFrameworkCore;
using TrazoService.Domain.Entities;

namespace TrazoService.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<Condition> Conditions { get; set; } = null!;
    public DbSet<ConditionGroup> ConditionGroups { get; set; } = null!;
    public DbSet<ConnectionStep> ConnectionSteps { get; set; } = null!;
    public DbSet<Container> Containers { get; set; } = null!;
    public DbSet<ContainerField> ContainerFields { get; set; } = null!;
    public DbSet<DataSet> DataSets { get; set; } = null!;
    public DbSet<DataSetHistory> DataSetHistories { get; set; } = null!;
    public DbSet<DataSetRow> DataSetRows { get; set; } = null!;
    public DbSet<Field> Fields { get; set; } = null!;
    public DbSet<Procedure> Procedures { get; set; } = null!;
    public DbSet<ProcedureHistory> ProcedureHistories { get; set; } = null!;
    public DbSet<Step> Steps { get; set; } = null!;
    public DbSet<StepField> StepFields { get; set; } = null!;
    public DbSet<StepRole> StepRoles { get; set; } = null!;
    public DbSet<StepUser> StepUsers { get; set; } = null!;
    public DbSet<WorkFlowsProccesType> WorkFlowsProccesTypes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<DataSet>()
            .Property(d => d.ValueNumber)
            .HasPrecision(18, 2);

        // Configuración para evitar ciclos de cascada en ConnectionSteps
        modelBuilder.Entity<ConnectionStep>()
            .HasOne(c => c.Step)
            .WithMany()
            .HasForeignKey(c => c.StepId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ConnectionStep>()
            .HasOne(c => c.TargetStep)
            .WithMany()
            .HasForeignKey(c => c.TargetStepId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configuración para ProcedureHistory
        modelBuilder.Entity<ProcedureHistory>()
            .HasOne(ph => ph.Procedure)
            .WithMany()
            .HasForeignKey(ph => ph.ProcedureId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProcedureHistory>()
            .HasOne(ph => ph.NodeStep)
            .WithMany()
            .HasForeignKey(ph => ph.NodeStepId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configuración para DataSet (evitar ciclos entre Field y Procedure)
        modelBuilder.Entity<DataSet>()
            .HasOne(ds => ds.Procedure)
            .WithMany()
            .HasForeignKey(ds => ds.ProcedureId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DataSet>()
            .HasOne(ds => ds.Field)
            .WithMany()
            .HasForeignKey(ds => ds.FieldId)
            .OnDelete(DeleteBehavior.Restrict);

        // Add custom configurations here if needed
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
