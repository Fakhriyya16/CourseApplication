using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        void Create(T entity);
        T Update(int? Id, T entity);
        void Delete(T entity);
        T GetById(int? id);
        List<T> GetAll();
        List<T> GetAllByExpression(Func<T, bool> predicate);
        T GetByExpression(Func<T, bool> predicate);
        List<T> Search(Func<T, bool> predicate);
    }
}
