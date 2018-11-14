using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ComicBookShared.Data
{
    // we can define the generic type at the class Level <TEntity>
    public abstract class BaseRepository<TEntity>
        // this constrains generic type TEntity to class types instead of every type possible
        // class is a 'reference' type. struct would be a 'value' type and new() is a reference type
        // that defines a default constructor
        where TEntity : class // struct, new()
    {
        // sub-classes need to access this property so we can't make it private
        protected Context Context { get; private set; }

        // BaseRepository(context)
        public BaseRepository(Context context)
        {
            Context = context;
        }

        // Get(id)
        // Since this is a generic base repository class, we cannot return a specific entity
        // as a type. Instead, we return a generic object, by convention starting with T and a word
        // describing the type
        public abstract TEntity Get(int id, bool includeRelatedEntities = true);

        // GetList()
        // abstract keyword delegates responsibility for returning to the sub-classes
        public abstract IList<TEntity> GetList();

        // Add(entity)
        public void Add(TEntity entity)
        {
            // Set is a DbContext method that return a DbSet instance
            Context.Set<TEntity>().Add(entity);
            Context.SaveChanges();
        }

        // Update(entity)
        public void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
        }

        // Delete(id)
        public void Delete(int id)
        {
            // instead of using a stub entity, we are using Set() to retrieve an entity using the
            // id parameter to remove it from the context. It's one extra query than creating a stub
            // but less code. For creating a stub: https://teamtreehouse.com/library/creating-a-generic-base-repository-class
            var set = Context.Set<TEntity>();
            var entity = set.Find(id);
            set.Remove(entity);
            Context.SaveChanges();
        }
    }
}
