using Lm.Streamthis.Catalog.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lm.Streamthis.Catalog.Infra;

public class UnitOfWork(StreamAspDbContext context) : IUnitOfWork
{
    public Task Commit(CancellationToken cancellationToken) =>
        context.SaveChangesAsync(cancellationToken);

    public Task Rollback(CancellationToken cancellationToken) =>
        Task.CompletedTask;
}
