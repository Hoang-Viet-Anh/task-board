using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Infrastructure.Persistence.Configurations;

public class UserBoardConfiguration : IEntityTypeConfiguration<UserBoard>
{
    public void Configure(EntityTypeBuilder<UserBoard> builder)
    {
        builder.HasOne(ub => ub.User)
            .WithMany(u => u.UserBoards)
            .HasForeignKey(ub => ub.UserId);

        builder.HasOne(ub => ub.Board)
            .WithMany(u => u.UserBoards)
            .HasForeignKey(ub => ub.BoardId);
    }
}