using Domain.Entities.Academic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Academic;

public sealed class TextbookChapterConfiguration : IEntityTypeConfiguration<TextbookChapter>
{
    public void Configure(EntityTypeBuilder<TextbookChapter> builder)
    {
        builder.ToTable("textbook_chapters");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.TextbookId).HasColumnName("textbook_id").HasColumnType("bigint unsigned").IsRequired();
        builder.Property(x => x.Title).HasColumnName("title").HasMaxLength(255).IsRequired();
        builder.Property(x => x.SortOrder).HasColumnName("sort_order").HasColumnType("int unsigned").IsRequired();
        builder.HasIndex(x => new { x.TextbookId, x.SortOrder })
            .IsUnique().HasDatabaseName("uq_textbook_chapters_order");
        builder.HasOne(x => x.Textbook).WithMany(x => x.Chapters)
            .HasForeignKey(x => x.TextbookId).HasConstraintName("fk_textbook_chapters_textbook")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
