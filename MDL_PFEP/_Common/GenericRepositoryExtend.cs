using MDL_BASE.Interface;
using MDL_PFEP.Interface;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MDL_PFEP.Repo
{
    public class GenericRepositoryName<TEntity> where TEntity:class,IModelEntityName
    {
        internal IDbContextPFEP context;
        internal DbSet<TEntity> dbSet;

        public GenericRepositoryName(IDbContextPFEP context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public List<string> GetNameList(string Prefix)
        {
            return dbSet.Where(x => x.Name.StartsWith(Prefix)).Select(x => x.Name).Take(5).ToList();
        }


    }
}