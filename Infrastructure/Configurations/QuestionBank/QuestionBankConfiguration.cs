using Domain.Entities.QuestionBank;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuestionBankEntity = Domain.Entities.QuestionBank.QuestionBank;

namespace Infrastructure.Configurations.QuestionBank;

public sealed class QuestionBankConfiguration : IEntityTypeConfiguration<QuestionBankEntity>
{
    public void Configure(EntityTypeBuilder<QuestionBankEntity> builder)
    {
        builder.ToTable("question_banks", table => table.HasCheckConstraint("ck_question_banks_type", "bank_type IN ('exam', 'practice')"));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.SchoolBranchId).HasColumnName("school_branch_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.SubjectId).HasColumnName("subject_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.GradeLevelId).HasColumnName("grade_level_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.BankType).HasColumnName("bank_type").HasMaxLength(20).IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(32).IsRequired();
        builder.HasIndex(x => new { x.SchoolBranchId, x.SubjectId, x.GradeLevelId, x.BankType })
            .IsUnique().HasDatabaseName("uq_question_banks_scope");
        builder.HasIndex(x => new { x.SubjectId, x.GradeLevelId })
            .HasDatabaseName("idx_question_banks_subject_grade");
        builder.HasOne(x => x.SchoolBranch).WithMany()
            .HasForeignKey(x => x.SchoolBranchId).HasConstraintName("fk_question_banks_branch")
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Subject).WithMany()
            .HasForeignKey(x => x.SubjectId).HasConstraintName("fk_question_banks_subject")
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.GradeLevel).WithMany()
            .HasForeignKey(x => x.GradeLevelId).HasConstraintName("fk_question_banks_grade")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
