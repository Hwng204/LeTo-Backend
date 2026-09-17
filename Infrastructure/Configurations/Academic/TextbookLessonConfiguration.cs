using Domain.Entities.Academic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Academic;

public sealed class TextbookLessonConfiguration : IEntityTypeConfiguration<TextbookLesson>
{
    public void Configure(EntityTypeBuilder<TextbookLesson> builder)
    {
        builder.ToTable("textbook_lessons");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.ChapterId).HasColumnName("chapter_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.Content).HasColumnName("content").HasColumnType("longtext");
        builder.Property(x => x.Title).HasColumnName("title").HasMaxLength(255).IsRequired();
        builder.Property(x => x.SortOrder).HasColumnName("sort_order").HasColumnType("int unsigned").IsRequired();
        builder.HasIndex(x => new { x.ChapterId, x.SortOrder })
            .IsUnique().HasDatabaseName("uq_textbook_lessons_order");
        builder.HasOne(x => x.Chapter).WithMany(x => x.Lessons)
            .HasForeignKey(x => x.ChapterId).HasConstraintName("fk_textbook_lessons_chapter")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
