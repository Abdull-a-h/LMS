using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Persistence.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        // TODO: keys, HasMaxLength on string columns, unique index on Email,
        //       HasQueryFilter(x => x.IsActive), Role conversion, relationships.
    }
}
