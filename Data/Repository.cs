using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SQLite;

namespace Shop.Data
{
    public interface IRepository<T> where T : class, new()
    {
        T Get(int id);
        List<T> GetAll();
        List<T> GetWhere(Expression<Func<T, bool>> predicate);

        int Insert(T entity);
        int Update(T entity);
        int Delete(T entity);
    }

    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private readonly SQLiteConnection database;

        public Repository(SQLiteConnection database)
        {
            this.database = database;
        }

        public T Get(int id) =>
             database.Find<T>(id);

        public List<T> GetAll() =>
             database.Table<T>().ToList();

        public List<T> GetWhere(Expression<Func<T, bool>> predicate) =>
            database.Table<T>().Where(predicate).ToList();

        public int Insert(T entity) =>
             database.Insert(entity);

        public int Update(T entity) =>
             database.Update(entity);

        public int Delete(T entity) =>
             database.Delete(entity);
    }
}