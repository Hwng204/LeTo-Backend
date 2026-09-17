using Domain.Entities.QuestionBank;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.QuestionBank;

public sealed class ExamVariantQuestionConfiguration : IEntityTypeConfiguration<ExamVariantQuestion>
{
    public void Configure(EntityTypeBuilder<ExamVariantQuestion> builder)
    {
        builder.ToTable("exam_variant_questions", table => table.HasCheckConstraint("ck_exam_variant_questions_position", "position_no > 0"));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.ExamVariantId).HasColumnName("exam_variant_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.ExamSetQuestionId).HasColumnName("exam_set_question_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.PositionNo).HasColumnName("position_no").HasColumnType("int unsigned").IsRequired();
        builder.Property(x => x.OptionOrderJson).HasColumnName("option_order_json").HasColumnType("json");
        builder.HasIndex(x => new { x.ExamVariantId, x.PositionNo })
            .IsUnique().HasDatabaseName("uq_exam_variant_questions_position");
        builder.HasIndex(x => new { x.ExamVariantId, x.ExamSetQuestionId })
            .IsUnique().HasDatabaseName("uq_exam_variant_questions_question");
        builder.HasOne(x => x.ExamVariant).WithMany(x => x.Questions)
            .HasForeignKey(x => x.ExamVariantId).HasConstraintName("fk_exam_variant_questions_variant")
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.ExamSetQuestion).WithMany(x => x.VariantQuestions)
            .HasForeignKey(x => x.ExamSetQuestionId).HasConstraintName("fk_exam_variant_questions_set_question")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
