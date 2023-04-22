using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace CzuczenLand.EntityFramework.Repositories;

public abstract class CzuczenLandRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<CzuczenLandDbContext, TEntity, TPrimaryKey>
    where TEntity : class, IEntity<TPrimaryKey>
{
    protected CzuczenLandRepositoryBase(IDbContextProvider<CzuczenLandDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    //add common methods for all repositories
}

public abstract class CzuczenLandRepositoryBase<TEntity> : CzuczenLandRepositoryBase<TEntity, int>
    where TEntity : class, IEntity<int>
{
    protected CzuczenLandRepositoryBase(IDbContextProvider<CzuczenLandDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    //do not add any method here, add to the class above (since this inherits it)
}