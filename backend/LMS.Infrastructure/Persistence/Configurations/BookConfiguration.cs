using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        // TODO: keys, HasMaxLength (Title 250, Description 2000, ISBN 13), unique index on ISBN,
        //       HasQueryFilter(x => x.IsActive), Author FK with DeleteBehavior.Restrict.
    }
}
