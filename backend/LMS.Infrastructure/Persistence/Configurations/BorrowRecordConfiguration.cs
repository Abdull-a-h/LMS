using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Persistence.Configurations;

public class BorrowRecordConfiguration : IEntityTypeConfiguration<BorrowRecord>
{
    public void Configure(EntityTypeBuilder<BorrowRecord> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.BorrowedAt)
            .IsRequired();

        builder.Property(b => b.DueDate)
            .IsRequired();

        // ReturnedAt is nullable: null means the book is still on loan.
        builder.Property(b => b.ReturnedAt);

        // Indexes that back the common queries: a member's borrows, a book's borrows,
        // and the daily overdue scan (DueDate).
        builder.HasIndex(b => b.MemberId);
        builder.HasIndex(b => b.BookId);
        builder.HasIndex(b => b.DueDate);

        // Borrow records are never physically deleted, so deletes are restricted.
        builder.HasOne(b => b.Book)
            .WithMany(book => book.BorrowRecords)
            .HasForeignKey(b => b.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Member)
            .WithMany(m => m.BorrowRecords)
            .HasForeignKey(b => b.MemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
