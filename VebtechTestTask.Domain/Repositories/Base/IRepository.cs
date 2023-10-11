using VebtechTestTask.Domain.Entities.Base;

namespace VebtechTestTask.Domain.Repositories.Base;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity> CreateAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task<TEntity> GetByIdAsync(int Id);
}
