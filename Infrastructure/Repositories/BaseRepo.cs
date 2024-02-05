using Infrastructure.Contexts;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public abstract class BaseRepo<TEntity> where TEntity : class
{
    private readonly UserContext _userContext;

    protected BaseRepo(UserContext userContext)
    {
        _userContext = userContext;
    }


    //Create
    public virtual TEntity Create(TEntity entity)
    {
        try
        {
            _userContext.Set<TEntity>().Add(entity);
            _userContext.SaveChanges();
            return entity;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }


    //ReadAll
    public virtual IEnumerable<TEntity> GetAll()
    {
        try
        {
            return _userContext.Set<TEntity>().ToList();
            
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }


    //ReadOne

    public virtual TEntity GetOne(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var result = _userContext.Set<TEntity>().FirstOrDefault(predicate);
            return result!;

        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    //Update

    public TEntity Update(Expression<Func<TEntity, bool>> expression, TEntity entity)
    {
        try
        {
            var entityToUpdate = _userContext.Set<TEntity>().FirstOrDefault(expression);
            if (entityToUpdate != null)
            {
                _userContext.Entry(entityToUpdate).CurrentValues.SetValues(entity);
                _userContext.SaveChanges();

                return entityToUpdate;
            }

        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    //Delete

    public virtual bool Delete(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entity = _userContext.Set<TEntity>().FirstOrDefault(predicate);
            if (entity != null)
            {
                _userContext.Set<TEntity>().Remove(entity);
                _userContext.SaveChanges();

                return true;
            }

        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }


    public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            return _userContext.Set<TEntity>().Any(predicate);

        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }

}
