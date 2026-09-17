using Domain.Entities.Academic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Academic;

public sealed class TextbookConfiguration : IEntityTypeConfiguration<Textbook>
{
    public void Configure(EntityTypeBuilder<Textbook> builder)
    {
        builder.ToTable("textbooks");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint unsigned").ValueGeneratedOnAdd();
        builder.Property(x => x.Title).HasColumnName("title").HasMaxLength(255).IsRequired();
        builder.Property(x => x.BookSet).HasColumnName("book_set").HasMaxLength(255);
        builder.HasIndex(x => x.Title).HasDatabaseName("idx_textbooks_title");
    }
}
