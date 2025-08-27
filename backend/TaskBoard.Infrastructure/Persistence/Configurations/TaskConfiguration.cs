using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskBoard.Infrastructure.Persistence.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<Domain.Entities.Task>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Task> builder)
    {
        builder.Property(t => t.Priority)
            .HasConversion<string>();

        builder.HasMany(t => t.TaskActivityLogs)
            .WithOne(l => l.Task)
            .HasForeignKey(l => l.TaskId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(t => t.UserTasks)
            .WithOne(ut => ut.Task)
            .HasForeignKey(ut => ut.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}