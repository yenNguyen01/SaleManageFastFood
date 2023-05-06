using QLBH.Fastfood.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace QLBH.Fastfood.Data.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private QLBHFastfoodDbcontext DbContext;
        private IDbSet<T> dbEntity;

        public GenericRepository(QLBHFastfoodDbcontext DbContext)
        {
            this.DbContext = DbContext;
            dbEntity = DbContext.Set<T>();
        }
        public void Insert(T Model)
        {
            dbEntity.Add(Model);
            DbContext.SaveChanges();
        }

        public IEnumerable<T> GetAllData()
        {
            return (IEnumerable<T>)dbEntity.ToList();
        }
        public IEnumerable<T> GetAllData(Expression<Func<T, bool>> where)
        {
            return (IEnumerable<T>)dbEntity.Where<T>(where).ToList();
        }
        public T GetDataByID(int ID)
        {
            return dbEntity.Find(ID);
        }
        public void Update(T Model)
        {
            DbContext.Entry(Model).State = EntityState.Modified;
            DbContext.SaveChanges();
        }
        public void Delete(T Model)
        {
            DbContext.Entry(Model).State = EntityState.Modified;
            DbContext.SaveChanges();
        }
        public void Remove(T Model)
        {
            dbEntity.Remove(Model);
            DbContext.SaveChanges();
        }
        public void Save()
        {
            DbContext.SaveChanges();
        }
    }
}