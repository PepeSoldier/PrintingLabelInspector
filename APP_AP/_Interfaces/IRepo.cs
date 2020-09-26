using MDL_BASE.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MDL_AP.Interfaces
{ 
    public interface IRepo : IModelEntity
    {
        IQueryable<IModelEntity> GetList();
        IModelEntity GetById(int id);
        int Add(IModelEntity entity);
        int Update(IModelEntity entity);
        int AddOrUpdate(IModelEntity entity);
        void Delete(IModelEntity entityToDelete);
    }
}
