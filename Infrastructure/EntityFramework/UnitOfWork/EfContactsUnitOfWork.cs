using AppCore.Repositories;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.EntityFramework.UnitOfWork;

public class EfContactsUnitOfWork(
    IPersonRepository personRepository,
    ICompanyRepository companyRepository,
    IOrganizationRepository organizationRepository,
    ContactsDbContext context)
    : IContactUnitOfWork
{
    private IDbContextTransaction? _transaction;

    public IPersonRepository Persons => personRepository;
    public ICompanyRepository Companies => companyRepository;
    public IOrganizationRepository Organizations => organizationRepository;

    public Task<int> SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction is not null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction is not null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        await context.DisposeAsync();
    }
}