using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly userDbContext _context;

        public UnitOfWork(userDbContext context)
        {
            _context = context;
        }
        
        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
            => _context.Database.BeginTransactionAsync(cancellationToken);

        public Task BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
            => _context.Database.BeginTransactionAsync(isolationLevel, cancellationToken);

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => _context.SaveChangesAsync(cancellationToken);
    }
}
