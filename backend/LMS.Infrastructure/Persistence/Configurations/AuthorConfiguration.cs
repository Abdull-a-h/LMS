using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Persistence.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        // TODO: keys, HasMaxLength (Name 150, Biography 1000), HasQueryFilter(x => x.IsActive),
        //       Books relationship with DeleteBehavior.Restrict.
    }
}
