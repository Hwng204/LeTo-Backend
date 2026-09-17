using Domain.Entities.QuestionBank;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.QuestionBank;

public sealed class QuestionTaskDetailConfiguration : IEntityTypeConfiguration<QuestionTaskDetail>
{
    public void Configure(EntityTypeBuilder<QuestionTaskDetail> builder)
    {
        builder.ToTable("question_task_details", table => table.HasCheckConstraint("ck_question_task_details_count", "question_count > 0"));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.QuestionTaskId).HasColumnName("question_task_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.CognitiveLevel).HasColumnName("cognitive_level").HasMaxLength(50).IsRequired();
        builder.Property(x => x.QuestionCount).HasColumnName("question_count").HasColumnType("int unsigned").IsRequired();
        builder.Property(x => x.QuestionType).HasColumnName("question_type").HasMaxLength(50).IsRequired();
        builder.HasIndex(x => new { x.QuestionTaskId, x.CognitiveLevel, x.QuestionType })
            .IsUnique().HasDatabaseName("uq_question_task_details_target");
        builder.HasOne(x => x.QuestionTask).WithMany(x => x.Details)
            .HasForeignKey(x => x.QuestionTaskId).HasConstraintName("fk_question_task_details_task")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
