using Abp.Domain.Entities;
using Abp.EntityFramework;
using CzuczenLand.EntityFramework;
using CzuczenLand.EntityFramework.Repositories;

namespace CzuczenLand.ExtendingFunctionalities.Repositories.CustomRepository;

public class CustomRepositoryBase<TEntity, TKey> : CzuczenLandRepositoryBase<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    public CustomRepositoryBase(IDbContextProvider<CzuczenLandDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}