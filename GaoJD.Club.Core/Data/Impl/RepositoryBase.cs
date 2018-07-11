using GaoJD.Club.Utility;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GaoJD.Club.Core
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {

        public RepositoryBase()
        {

        }

        //定义数据访问上下文对象
        protected readonly ReadSqlServerContext _readDbContext;
        protected readonly WriteSqlServerContext _WriteDbContext;
        /// <summary>
        /// 通过构造函数注入得到数据上下文对象实例
        /// </summary>
        /// <param name="dbContext"></param>
        public RepositoryBase(ReadSqlServerContext readSqlServerContext, WriteSqlServerContext writeSqlServerContext)
        {
            this._readDbContext = readSqlServerContext;
            this._WriteDbContext = writeSqlServerContext;

            //_readDbContext = DbContextFactory.CallContext<ReadSqlServerContext>(WriteAndRead.Read);
            //_WriteDbContext = DbContextFactory.CallContext<WriteSqlServerContext>(WriteAndRead.Write);

        }

        #region  是否存在
        /// <summary>
        /// 根据条件查询是否存在
        /// <param name="condition">查询条件</param>
        /// </summary>
        /// <returns></returns>
        public bool IsExists(Expression<Func<TEntity, bool>> predicate)
        {
            return _readDbContext.Set<TEntity>().Any(predicate);
        }
        #endregion

        #region  查询
        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <returns></returns>
        public List<TEntity> GetAll()
        {
            return _readDbContext.Set<TEntity>().ToList();
        }

        /// <summary>
        /// 获取总数量
        /// </summary>
        /// <returns></returns>
        public long GetCount()
        {
            return _readDbContext.Set<TEntity>().LongCount();
        }


        /// <summary>
        /// 根据lambda表达式条件获取总数量
        /// </summary>
        /// <returns></returns>
        public long GetCountByQuery(Expression<Func<TEntity, bool>> predicate)
        {
            return _readDbContext.Set<TEntity>().Where(predicate).LongCount();
        }


        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <returns></returns>
        public TEntity GetById(int id, WriteAndRead writeAndRead = WriteAndRead.Read)
        {

            int a = writeAndRead == WriteAndRead.Read ? 0 : 1;
            TeacherClubContext context;
            if (writeAndRead == WriteAndRead.Read)
            {
                context = _readDbContext;
            }
            else
            {
                context = _WriteDbContext;
            }
            return context.Set<TEntity>().Find(id);
        }

        /// <summary>
        /// 根据lambda表达式条件获取单个实体
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        public TEntity GetItemByQuery(Expression<Func<TEntity, bool>> predicate, WriteAndRead writeAndRead = WriteAndRead.Read)
        {
            TeacherClubContext context;
            if (writeAndRead == WriteAndRead.Read)
            {
                context = _readDbContext;
            }
            else
            {
                context = _WriteDbContext;
            }
            return context.Set<TEntity>().SingleOrDefault(predicate);
        }

        /// <summary>
        /// 根据lambda表达式条件获取实体集合
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        public List<TEntity> GetListByQuery(Expression<Func<TEntity, bool>> predicate, bool isDesc = false, Expression<Func<TEntity, object>> orderBy = null)
        {
            var quartlist = _readDbContext.Set<TEntity>().Where(predicate);
            if (orderBy != null)
            {
                return quartlist.OrderBy(EFHelper.GetPropertyName<TEntity>(orderBy), isDesc).ToList();
            }
            return quartlist.ToList();
        }


        /// <summary>
        /// 根据lambda表达式条件获取实体集合
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        public List<TEntity> GetListByQueryExtensions(Expression<Func<TEntity, bool>> predicate, bool isDesc = false, string orderBy = null)
        {
            var quartlist = _readDbContext.Set<TEntity>().Where(predicate);
            if (!string.IsNullOrEmpty(orderBy))
            {
                return quartlist.OrderBy(orderBy, isDesc).ToList();
            }
            return quartlist.ToList();
        }


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="startPage">页码</param>
        /// <param name="pageSize">单页数据数</param>
        /// <param name="rowCount">行数</param>
        /// <param name="where">条件</param>
        /// <param name="order">排序</param>
        /// <returns></returns>
        public List<TEntity> GetPagedList(int startPage, int pageSize, out int rowCount, bool isDesc = false, Expression<Func<TEntity, bool>> where = null, Expression<Func<TEntity, object>> order = null)
        {
            var result = from p in _readDbContext.Set<TEntity>()
                         select p;
            if (where != null)
                result = result.Where(where);
            if (order != null)
                result = result.OrderBy(EFHelper.GetPropertyName<TEntity>(order), isDesc);

            rowCount = result.Count();
            return result.Skip((startPage - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="startPage">页码</param>
        /// <param name="pageSize">单页数据数</param>
        /// <param name="rowCount">行数</param>
        /// <param name="where">条件</param>
        /// <param name="order">排序</param>
        /// <returns></returns>
        public List<TEntity> GetPagedListExtensions(int startPage, int pageSize, out int rowCount, bool isDesc = false, Expression<Func<TEntity, bool>> where = null, string order = null)
        {
            var result = from p in _readDbContext.Set<TEntity>()
                         select p;
            if (where != null)
                result = result.Where(where);
            if (order != null)
                result = result.OrderBy(order, isDesc);

            rowCount = result.Count();
            return result.Skip((startPage - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// 执行Sql查询语句,返回受影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public int ExecuteSql(string sql, SqlParameter[] sqlParameters = null, WriteAndRead writeAndRead = WriteAndRead.Read)
        {
            TeacherClubContext context;
            if (writeAndRead == WriteAndRead.Read)
            {
                context = _readDbContext;
            }
            else
            {
                context = _WriteDbContext;
            }
            if (sqlParameters != null)
            {
                return context.Database.ExecuteSqlCommand(sql, sqlParameters);
            }
            else
            {
                return context.Database.ExecuteSqlCommand(sql);
            }

        }

        /// <summary>
        /// 执行存储过程，返回受影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public int ExecuteStoredProcedure(string sql, SqlParameter[] sqlParameters = null, WriteAndRead writeAndRead = WriteAndRead.Read)
        {
            TeacherClubContext context;
            if (writeAndRead == WriteAndRead.Read)
            {
                context = _readDbContext;
            }
            else
            {
                context = _WriteDbContext;
            }
            StringBuilder paraNames = new StringBuilder();
            if (sqlParameters != null)
            {
                foreach (var sqlPara in sqlParameters)
                {
                    paraNames.Append($" {sqlPara},");
                }
            }

            sql = paraNames.Length > 0 ?
                    $"exec {sql} {paraNames.ToString().Trim(',')}" : $"exec {sql} ";

            if (sqlParameters != null)
            {
                return context.Database.ExecuteSqlCommand(sql, sqlParameters);
            }
            else
            {
                return context.Database.ExecuteSqlCommand(sql);
            }

        }

        /// <summary>
        /// 执行Sql查询语句，返回实体
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public List<TEntity> ExecuteSqlQuery(string sql, SqlParameter[] sqlParameters = null, WriteAndRead writeAndRead = WriteAndRead.Read)

        {
            TeacherClubContext context;
            if (writeAndRead == WriteAndRead.Read)
            {
                context = _readDbContext;
            }
            else
            {
                context = _WriteDbContext;
            }
            if (sqlParameters != null)
            {
                return context.Set<TEntity>().FromSql<TEntity>(sql, sqlParameters).ToList();
            }
            else
            {
                return context.Set<TEntity>().FromSql<TEntity>(sql).ToList();
            }

        }

        /// <summary>
        /// 执行存储过程，返回实体
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public List<TEntity> ExecuteStoredProcedureQuery(string sql, SqlParameter[] sqlParameters = null, WriteAndRead writeAndRead = WriteAndRead.Read)
        {
            TeacherClubContext context;
            if (writeAndRead == WriteAndRead.Read)
            {
                context = _readDbContext;
            }
            else
            {
                context = _WriteDbContext;
            }
            StringBuilder paraNames = new StringBuilder();
            if (sqlParameters != null)
            {
                foreach (var sqlPara in sqlParameters)
                {
                    paraNames.Append($" {sqlPara},");
                }
            }

            sql = paraNames.Length > 0 ?
                    $"exec {sql} {paraNames.ToString().Trim(',')}" : $"exec {sql} ";
            if (sqlParameters != null)
            {
                return context.Set<TEntity>().FromSql<TEntity>(sql, sqlParameters).ToList();
            }
            else
            {
                return context.Set<TEntity>().FromSql<TEntity>(sql).ToList();
            }

        }


        #endregion

        #region 增删改

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        public TEntity Insert(TEntity entity, bool autoSave = true)
        {
            _WriteDbContext.Set<TEntity>().Add(entity);
            if (autoSave)
                Save();
            return entity;
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public TEntity Update(TEntity entity, bool autoSave = true)
        {
            //var aaa = CreateEqualityExpressionForId(5);
            _WriteDbContext.Set<TEntity>().Update(entity);
            if (autoSave)
                Save();
            return entity;
        }


        //private void EntityToEntity<T>(T pTargetObjSrc, T pTargetObjDest)
        //{
        //    foreach (var mItem in typeof(T).GetProperties())
        //    {
        //        mItem.SetValue(pTargetObjDest, mItem.GetValue(pTargetObjSrc, new object[] { }), null);
        //    }
        //}


        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public void Delete(TEntity entity, bool autoSave = true)
        {
            _WriteDbContext.Set<TEntity>().Remove(entity);
            if (autoSave)
                Save();
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public void Delete(int id, bool autoSave = true)
        {
            TEntity entity = _WriteDbContext.Set<TEntity>().Find(id);
            _WriteDbContext.Set<TEntity>().Remove(entity);
            if (autoSave)
                Save();
        }

        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        /// <param name="where">lambda表达式</param>
        /// <param name="autoSave">是否自动保存</param>
        public void Delete(Expression<Func<TEntity, bool>> where, bool autoSave = true)
        {

            List<TEntity> list = _WriteDbContext.Set<TEntity>().Where(where).ToList();
            _WriteDbContext.Set<TEntity>().RemoveRange(list);
            if (autoSave)
                Save();
        }

        /// <summary>
        /// 批量新增，实体类型默认为数据库表名(*注意：批量新增时实体类中字段的顺序一定要和数据库中列的顺序保持一致)
        /// </summary>
        /// <param name="entities">数据列表</param>
        public void AddList(List<TEntity> entities)
        {
            _WriteDbContext.Set<TEntity>().AddList<TEntity>(entities, _WriteDbContext.Database.GetDbConnection());
        }

        /// <summary>
        /// 批量添加(*注意：批量新增时实体类中字段的顺序一定要和数据库中列的顺序保持一致)
        /// </summary>
        /// <param name="tableName">批量新增的表名</param>
        /// <param name="entities">数据列表</param>
        /// <returns></returns>
        public void AddList(string tableName, List<TEntity> entities)
        {
            _WriteDbContext.Set<TEntity>().AddList<TEntity>(tableName, entities, _WriteDbContext.Database.GetDbConnection());
        }

        /// <summary>
        /// 批量添加(*注意：批量新增时DataTable中列的顺序一定要和数据库中列的顺序保持一致)
        /// </summary>
        /// <param name="tableName">批量新增的表名</param>
        /// <param name="dt">数据源</param>
        /// <returns></returns>
        public void AddList(string tableName, DataTable dt)
        {
            _WriteDbContext.Set<TEntity>().AddList(tableName, dt, _WriteDbContext.Database.GetDbConnection());
        }


        /// <summary>
        /// 事务性保存
        /// </summary>
        public void Save()
        {
            _WriteDbContext.SaveChanges();
        }

        #endregion

        /// <summary>
        /// 根据主键构建判断表达式
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(int id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));
            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(int))
                );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }

    }
}
