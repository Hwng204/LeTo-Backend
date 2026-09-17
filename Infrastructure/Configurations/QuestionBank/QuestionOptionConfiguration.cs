using Domain.Entities.QuestionBank;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.QuestionBank;

public sealed class QuestionOptionConfiguration : IEntityTypeConfiguration<QuestionOption>
{
    public void Configure(EntityTypeBuilder<QuestionOption> builder)
    {
        builder.ToTable("question_options");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.QuestionId).HasColumnName("question_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.Content).HasColumnName("content").HasColumnType("text").IsRequired();
        builder.Property(x => x.IsCorrect).HasColumnName("is_correct").HasColumnType("tinyint(1)").HasDefaultValue(false).IsRequired();
        builder.Property(x => x.SortOrder).HasColumnName("sort_order").HasColumnType("int unsigned").IsRequired();
        builder.Property(x => x.OptionKey).HasColumnName("option_key").HasMaxLength(20).IsRequired();
        builder.HasIndex(x => new { x.QuestionId, x.SortOrder })
            .IsUnique().HasDatabaseName("uq_question_options_order");
        builder.HasIndex(x => new { x.QuestionId, x.OptionKey })
            .IsUnique().HasDatabaseName("uq_question_options_key");
        builder.HasOne(x => x.Question).WithMany(x => x.Options)
            .HasForeignKey(x => x.QuestionId).HasConstraintName("fk_question_options_question")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
