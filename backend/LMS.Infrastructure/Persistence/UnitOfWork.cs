using LMS.Application.Common.Interfaces;

namespace LMS.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly LmsDbContext _db;

    public UnitOfWork(LmsDbContext db) => _db = db;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _db.SaveChangesAsync(cancellationToken);
}
