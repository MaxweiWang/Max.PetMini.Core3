using Max.PetMini.DAL.Extensions;
using Max.PetMini.Extension.Models;
using Dapper;
using Dapper.Contrib.Extensions;
using NAutowired.Core.Attributes;
using Snowflake.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Max.PetMini.DAL.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class Repository<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        [Autowired]
        protected readonly IdWorker idWorker;
        //默认60S的数据库执行超时时间
        private readonly int commandTimeout = 60;

        /// <summary>
        /// 
        /// </summary>
        [Autowired]
        protected DbConnectionFactory dbConnectionFactory;

        /// <summary>
        /// 主库
        /// </summary>
        protected IDbConnection Master { get { return dbConnectionFactory.Master; } }

        ///// <summary>
        ///// 从库
        ///// </summary>
        //protected IDbConnection Slave { get { return dbConnectionFactory.Slave; } }
     
        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName => GetTableName(typeof(TEntity)) ?? typeof(TEntity).Name;

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="entity">对象</param>
        /// <returns></returns>
        public long Insert(TEntity entity)
        {
            return Master.Insert(entity, commandTimeout: commandTimeout);
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(TEntity entity)
        {
            return Master.Update(entity, commandTimeout: commandTimeout);
        }


        /// <summary>
        /// 查询所有对象
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll()
        {
            string sql = $"select * from {TableName}";
            return Master.Query<TEntity>(sql);
        }

        /// <summary>
        /// 根据条件和分页查询所有对象
        /// </summary>
        /// <param name="whereString">查询语句</param>
        /// <param name="parameters">参数</param>
        /// <param name="page">分页参数</param>
        /// <param name="orderBy">排序字段 default:id </param>
        /// <param name="sort">排序方式(asc/desc) default:desc </param>
        /// <returns></returns>
        protected IEnumerable<TEntity> GetAll(string whereString, object parameters = null, PageParameter page = null,
          string orderBy = "", string sort = "desc")
        {
            string sql = $"select * from {TableName} {WhereStringConvert(whereString)} {PageString(page, orderBy, sort)}";
            return Master.Query<TEntity>(sql, parameters);
        }

        /// <summary>
        /// 适用于多表连接查询的方法
        /// </summary>
        /// <param name="unionTable">TableA inner join TableB on TableA.X = TableB.x</param>
        /// <param name="whereString">propertyA = xxx and propertyB = xxx</param>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="orderBy"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        //TODO: unionTable这个参数名需要修改的更贴切一点
        protected IEnumerable<TEntity> GetAll(string whereString, string unionTable, string orderBy = "", object parameters = null,
          PageParameter page = null, string sort = "desc")
        {
            string sql = $"select {TableName}.* from {unionTable} {WhereStringConvert(whereString)} {PageString(page, orderBy, sort)}";
            return Master.Query<TEntity>(sql, parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryString"></param>
        /// <param name="whereString"></param>
        /// <param name="unionTable"></param>
        /// <param name="orderBy"></param>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        protected IEnumerable<T> GetAll<T>(string queryString, string whereString, string unionTable, string orderBy = "", object parameters = null,
          PageParameter page = null, string sort = "desc") where T : class, new()
        {
            string sql = $"select {queryString} from {unionTable} {WhereStringConvert(whereString)} {PageString(page, orderBy, sort)}";
            return Master.Query<T>(sql, parameters);
        }

        private string PageString(PageParameter page, string orderBy, string sort)
        {
            if (string.IsNullOrWhiteSpace(sort)) { sort = "desc"; }
            var sql = string.Empty;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                sql = $"order by {orderBy} {sort} ";
            }
            if (page != null)
            {
                if (string.IsNullOrWhiteSpace(orderBy))
                {
                    throw new ArgumentNullException(orderBy);
                }
                page.PageNumber = page.PageNumber < 1 ? 1 : page.PageNumber;
                page.PageSize = page.PageSize < 1 ? 1 : page.PageSize;
                sql = $"{sql} OFFSET {page.Offset} ROWS FETCH NEXT {page.PageSize} ROWS ONLY";
            }
            return sql;
        }


        /// <summary>
        /// 适用于多表的查询,支持SqlBuilderMultiple
        /// </summary>
        /// <param name="sqlBuilderMultiple"></param>
        /// <returns></returns>
        protected IEnumerable<TEntity> GetAll(SqlBuilderMultiple sqlBuilderMultiple)
        {
            return GetAll(sqlBuilderMultiple.ToString(), sqlBuilderMultiple.Table.ToString(), sqlBuilderMultiple.OrderBy, sqlBuilderMultiple.Parameters, null, sqlBuilderMultiple.Sort);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilderMultiple"></param>
        /// <returns></returns>
        protected IEnumerable<T> GetAll<T>(SqlBuilderMultiple sqlBuilderMultiple) where T : class, new()
        {
            return GetAll<T>(sqlBuilderMultiple.QueryColumn.ToString(), sqlBuilderMultiple.ToString(), sqlBuilderMultiple.Table.ToString(), sqlBuilderMultiple.OrderBy, sqlBuilderMultiple.Parameters, null, sqlBuilderMultiple.Sort);
        }

        /// <summary>
        /// 适用于单表的查询,支持SqlBuilderSingle
        /// </summary>
        /// <param name="sqlBuilderSingle"></param>
        /// <returns></returns>
        protected IEnumerable<TEntity> GetAll(SqlBuilderSingle sqlBuilderSingle)
        {
            return GetAll(whereString: sqlBuilderSingle.ToString(), parameters: sqlBuilderSingle.Parameters);
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public PageCollection<TEntity> GetPageCollection(PageParameter page)
        {
            var result = this.GetAll(whereString: null, page: page);
            int count = this.Count(whereString: null);
            return new PageCollection<TEntity>
            {
                TotalCount = count,
                PageNumber = page.PageNumber,
                PageSize = page.PageSize,
                Collection = result.ToList()
            };
        }

        /// <summary>
        /// 适用于多表查询的分页
        /// </summary>
        /// <param name="unionTable">TableA inner join TableB on TableA.x = TableB.x</param>
        /// <param name="whereString">propertyA = xxx and propertyB = xxx</param>
        /// <param name="page"></param>
        /// <param name="parameters"></param>
        /// <param name="orderBy"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        protected PageCollection<TEntity> GetPageCollection(PageParameter page, string whereString, string unionTable, string orderBy,
          object parameters = null, string sort = "desc")
        {
            var result = this.GetAll(whereString, unionTable, orderBy, parameters, page, sort);
            return new PageCollection<TEntity>
            {
                TotalCount = Count(whereString, unionTable, parameters),
                PageNumber = page.PageNumber,
                PageSize = page.PageSize,
                Collection = result.ToList()
            };
        }

        /// <summary>
        /// 适用于多表查询的分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <param name="queryString"></param>
        /// <param name="whereString"></param>
        /// <param name="unionTable"></param>
        /// <param name="orderBy"></param>
        /// <param name="parameters"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        protected PageCollection<T> GetPageCollection<T>(PageParameter page, string queryString, string whereString, string unionTable, string orderBy,
          object parameters = null, string sort = "desc") where T : class, new()
        {
            var result = this.GetAll<T>(queryString, whereString, unionTable, orderBy, parameters, page, sort);
            return new PageCollection<T>
            {
                TotalCount = Count(whereString, unionTable, parameters),
                PageNumber = page.PageNumber,
                PageSize = page.PageSize,
                Collection = result.ToList()
            };
        }

        /// <summary>
        /// 多表分页方法支持传入sqlbuilder
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sqlBuilderMultiple"></param>
        /// <returns></returns>
        protected PageCollection<TEntity> GetPageCollection(PageParameter page, SqlBuilderMultiple sqlBuilderMultiple)
        {
            return this.GetPageCollection(page, sqlBuilderMultiple.ToString(), sqlBuilderMultiple.Table.ToString(), sqlBuilderMultiple.OrderBy,
              sqlBuilderMultiple.Parameters, sqlBuilderMultiple.Sort);
        }

        /// <summary>
        /// 多表分页方法支持传入sqlbuilder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <param name="sqlBuilderMultiple"></param>
        /// <returns></returns>
        protected PageCollection<T> GetPageCollection<T>(PageParameter page, SqlBuilderMultiple sqlBuilderMultiple) where T : class, new()
        {
            return this.GetPageCollection<T>(page, sqlBuilderMultiple.QueryColumn.ToString(), sqlBuilderMultiple.ToString(), sqlBuilderMultiple.Table.ToString(), sqlBuilderMultiple.OrderBy,
              sqlBuilderMultiple.Parameters, sqlBuilderMultiple.Sort);
        }

        /// <summary>
        /// 带有分页的单表查询结果集
        /// </summary>
        /// <param name="page">分页参数</param>
        /// <param name="whereString">条件语句</param>
        /// <param name="parameters">参数</param>
        /// <param name="orderBy">排序字段 </param>
        /// <param name="sort">排序方式(asc/desc) default:desc </param>
        /// <returns></returns>
        protected PageCollection<TEntity> GetPageCollection(PageParameter page, string whereString,
          string orderBy, object parameters = null, string sort = "desc")
        {
            var result = this.GetAll(whereString: whereString, parameters: parameters, page: page, orderBy: orderBy, sort: sort);
            int count = this.Count(whereString, parameters);
            return new PageCollection<TEntity>
            {
                TotalCount = count,
                PageNumber = page.PageNumber,
                PageSize = page.PageSize,
                Collection = result.ToList()
            };
        }

        /// <summary>
        /// 单表分页方法支持传入sqlbuilder
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sqlBuilderSingle"></param>
        /// <param name="orderBy"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        protected PageCollection<TEntity> GetPageCollection(PageParameter page, SqlBuilderSingle sqlBuilderSingle,
      string orderBy, string sort = "desc")
        {
            return this.GetPageCollection(page, sqlBuilderSingle.ToString(), orderBy, sqlBuilderSingle.Parameters, sort);
        }

        /// <summary>
        /// 用于多表的Count
        /// </summary>
        /// <param name="whereString"></param>
        /// <param name="unionTable"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private int Count(string whereString, string unionTable, object parameters = null)
        {
            string sql = $"select count(1) from {unionTable} {WhereStringConvert(whereString)}";
            return Master.ExecuteScalar<int>(sql, parameters);
        }

        /// <summary>
        /// 支出传入SqlBuilderMultiple
        /// </summary>
        /// <param name="sqlBuilderMultiple"></param>
        /// <returns></returns>
        private int Count(SqlBuilderMultiple sqlBuilderMultiple)
        {
            return Count(sqlBuilderMultiple.ToString(), sqlBuilderMultiple.Table.ToString(), sqlBuilderMultiple.Parameters);
        }


        /// <summary>
        /// 获取对象数量
        /// </summary>
        /// <returns></returns>
        protected int Count(string whereString, object parameters = null)
        {
            string sql = $"select count(1) from {TableName} {WhereStringConvert(whereString)}";
            return Master.ExecuteScalar<int>(sql, parameters);
        }

        /// <summary>
        /// Sum
        /// </summary>
        /// <param name="sumColumn"></param>
        /// <param name="whereString"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected int Sum(string sumColumn, string whereString, object parameters = null)
        {
            string sql = $"select sum({sumColumn}) from {TableName} {WhereStringConvert(whereString)}";
            return Master.ExecuteScalar<int>(sql, parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            string sql = $"select count(1) from {TableName}";
            return Master.ExecuteScalar<int>(sql);
        }

        /// <summary>
        /// 支出传入SqlBuliderSingle
        /// </summary>
        /// <param name="sqlBuilderSingle"></param>
        /// <returns></returns>
        protected int Count(SqlBuilderSingle sqlBuilderSingle)
        {
            return Count(sqlBuilderSingle.WhereSQL.ToString(), sqlBuilderSingle.Parameters);
        }



        /// <summary>  
        /// 获取表名  
        /// </summary>  
        /// <param name="type"></param>  
        /// <returns></returns>  
        protected static string GetTableName(Type type)
        {
            string tableName = null;
            object[] attributes = type.GetCustomAttributes(false);
            foreach (var attr in attributes)
            {
                if (attr is TableAttribute)
                {
                    TableAttribute tableAttribute = attr as TableAttribute;
                    tableName = tableAttribute.Name;
                    break;
                }
            }
            return tableName;
        }

        private string WhereStringConvert(string whereString)
        {
            return string.IsNullOrWhiteSpace(whereString) ? string.Empty : $"where {whereString}";
        }
    }
}
