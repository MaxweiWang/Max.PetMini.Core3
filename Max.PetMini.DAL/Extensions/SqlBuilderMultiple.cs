using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Max.PetMini.DAL.Extensions
{

    /// <summary>
    /// 用于多表查询的sql语句创建
    /// </summary>
    public class SqlBuilderMultiple
    {
        private long number;
        private long Number { get { return ++number; } set { number = value; } }
        private bool FirstGroupBy { get; set; } = true;

        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// 排序规则
        /// </summary>
        public string Sort { get; set; } = "desc";

        /// <summary>
        /// 查询列
        /// </summary>
        public StringBuilder QueryColumn { get; set; }

        /// <summary>
        /// 用来存放where 后面跟随的sql
        /// </summary>
        public StringBuilder QuerySql { get; set; }
        /// <summary>
        /// 存放动态参数
        /// </summary>
        public DynamicParameters Parameters { get; set; }
        /// <summary>
        /// 存放from 和where之间的sql
        /// </summary>
        public StringBuilder Table { get; set; }
        private List<string> Tables { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        public SqlBuilderMultiple(string tableName)
        {
            QuerySql = new StringBuilder();
            QueryColumn = new StringBuilder();
            Parameters = new DynamicParameters();
            Table = new StringBuilder($"[{tableName}]");
            Tables = new List<string> {
        tableName
      };
        }

        /// <summary>
        /// 添加查询的列
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="column">列名</param>
        /// <param name="query">是否查询此列</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        public SqlBuilderMultiple AddColumn(string table, string column, string alias = null, bool query = true)
        {
            if (!query)
            {
                return this;
            }
            var firstAdd = true;
            if (QueryColumn.Length > 0)
            {
                //移除末尾字符
                QueryColumn.Remove(QueryColumn.Length - 1, 1);
                firstAdd = false;
            }
            if (string.IsNullOrEmpty(alias))
            {
                QueryColumn.Append($", [{table}].[{column}] ");
            }
            else
            {
                QueryColumn.Append($", [{table}].[{column}] as [{alias}] ");
            }
            if (firstAdd)
            {
                //移除开始字符
                QueryColumn.Remove(0, 2);
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <param name="alias"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public SqlBuilderMultiple Sum(string table, string column, string alias = null, bool query = true)
        {
            if (!query)
            {
                return this;
            }
            var firstAdd = true;
            if (QueryColumn.Length > 0)
            {
                //移除末尾字符
                QueryColumn.Remove(QueryColumn.Length - 1, 1);
                firstAdd = false;
            }
            if (string.IsNullOrEmpty(alias))
            {
                QueryColumn.Append($", sum([{table}].[{column}]) ");
                return this;
            }
            else
            {
                QueryColumn.Append($", sum([{table}].[{column}]) as [{alias}] ");
            }
            if (firstAdd)
            {
                //移除开始字符
                QueryColumn.Remove(0, 2);
            }
            return this;
        }

        /// <summary>
        /// 子查询列
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="alias"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public SqlBuilderMultiple AddChildQueryColumn(string queryString, string alias, bool query = true)
        {
            if (!query)
            {
                return this;
            }
            var firstAdd = true;
            if (QueryColumn.Length > 0)
            {
                //移除末尾字符
                QueryColumn.Remove(QueryColumn.Length - 1, 1);
                firstAdd = false;
            }

            QueryColumn.Append($", ({queryString}) as [{alias}] ");
            if (firstAdd)
            {
                //移除开始字符
                QueryColumn.Remove(0, 2);
            }
            return this;
        }

        /// <summary>
        /// left join tableName on tableName.id=tableNameOther.tableName_id
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="on"></param>
        /// <param name="isJoin"></param>
        /// <returns></returns>
        public SqlBuilderMultiple LeftJoin(string tableName, string on, bool isJoin = true)
        {
            if (!isJoin)
            {
                return this;
            }
            Table.Append($" left join [{tableName}] on {on}");
            Tables.Add(tableName);
            return this;
        }

        /// <summary>
        /// right join tableName on tableName.id=tableNameOther.tableName_id
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="on"></param>
        /// <param name="isJoin"></param>
        /// <returns></returns>
        public SqlBuilderMultiple RightJoin(string tableName, string on, bool isJoin = true)
        {
            if (!isJoin)
            {
                return this;
            }
            Table.Append($" right join [{tableName}] on {on}");
            Tables.Add(tableName);
            return this;
        }

        /// <summary>
        /// inner join tableName on tableName.id = tableNameOther.tableName_id
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="on"></param>
        /// <param name="isJoin"></param>
        /// <returns></returns>
        public SqlBuilderMultiple InnerJoin(string tableName, string on, bool isJoin = true)
        {
            if (!isJoin)
            {
                return this;
            }
            Table.Append($" inner join [{tableName}] on {on} ");
            Tables.Add(tableName);
            return this;
        }

        /// <summary>
        /// inner join tableName on tableName.id = tableNameOther.tableName_id
        /// </summary>
        /// <param name="slaveTableName"></param>
        /// <param name="masterTableField"></param>
        /// <param name="slaveTableField"></param>
        /// <param name="isJoin"></param>
        /// <returns></returns>
        public SqlBuilderMultiple InnerJoin(string slaveTableName, string masterTableField, string slaveTableField, bool isJoin = true)
        {
            return InnerJoin(slaveTableName, $"{Tables[0]}.{masterTableField} = {slaveTableName}.{slaveTableField}", isJoin);
        }

        /// <summary>
        /// and tableName.columnName = value
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderMultiple Equals(string tableName, string columnName, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return this;
            }
            long temp = Number;
            QuerySql.Append($" and [{tableName}].[{columnName}]= @{tableName}_{columnName}{temp} ");
            Parameters.Add($"{tableName}_{columnName}{temp}", value);

            return this;
        }

        /// <summary>
        /// and tableName.columnName = value
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderMultiple Equals(string tableName, string columnName, decimal? value)
        {
            if (value == null || value == 0)
            {
                return this;
            }
            long temp = Number;
            QuerySql.Append($" and [{tableName}].[{columnName}]= @{tableName}_{columnName}{temp} ");
            Parameters.Add($"{tableName}_{columnName}{temp}", value);

            return this;
        }

        /// <summary>
        /// and tableName.columnName = value
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderMultiple Equals(string tableName, string columnName, int? value)
        {
            if (value == null || value == 0)
            {
                return this;
            }
            long temp = Number;
            QuerySql.Append($" and [{tableName}].[{columnName}] = @{tableName}_{columnName}{temp} ");
            Parameters.Add($"{tableName}_{columnName}{temp}", value);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderMultiple Equals(string tableName, string columnName, bool? value)
        {
            if (value == null)
            {
                return this;
            }
            long temp = Number;
            QuerySql.Append($" and [{tableName}].[{columnName}] = @{tableName}_{columnName}{temp} ");
            Parameters.Add($"{tableName}_{columnName}{temp}", value);
            return this;
        }

        /// <summary>
        /// and tableName.columnName = value
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderMultiple Equals(string tableName, string columnName, long? value)
        {
            if (value == null || value == 0)
            {
                return this;
            }

            long temp = Number;
            QuerySql.Append($" and [{tableName}].[{columnName}] = @{tableName}_{columnName}{temp} ");
            Parameters.Add($"{tableName}_{columnName}{temp}", value);

            return this;
        }

        /// <summary>
        /// and tableName.columnName != value
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderMultiple ForceNotEquals(string tableName, string columnName, long value)
        {
            long temp = Number;
            QuerySql.Append($" and [{tableName}].[{columnName}]<> @{tableName}_{columnName}{temp} ");
            Parameters.Add($"{tableName}_{columnName}{temp}", value);

            return this;
        }


        /// <summary>
        /// and tableName.columnName != value (Optional)
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderMultiple NotEquals(string tableName, string columnName, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return this;
            }
            long temp = Number;
            QuerySql.Append($" and [{tableName}].[{columnName}] <> @{tableName}_{columnName}{temp} ");
            Parameters.Add($"{tableName}_{columnName}{temp}", value);

            return this;
        }

        /// <summary>
        /// and tableName.columnName != value (Optional)
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderMultiple NotEquals(string tableName, string columnName, decimal? value)
        {
            if (value == null || value == 0)
            {
                return this;
            }
            long temp = Number;
            QuerySql.Append($" and [{tableName}].[{columnName}]<> @{tableName}_{columnName}{temp} ");
            Parameters.Add($"{tableName}_{columnName}{temp}", value);

            return this;
        }

        /// <summary>
        /// and tableName.columnName <![CDATA[<>]]> value (Optional)
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderMultiple NotEquals(string tableName, string columnName, int? value)
        {
            if (value == null || value == 0)
            {
                return this;
            }
            long temp = Number;
            QuerySql.Append($" and [{tableName}].[{columnName}] <> @{tableName}_{columnName}{temp} ");
            Parameters.Add($"{tableName}_{columnName}{temp}", value);
            return this;
        }

        /// <summary>
        /// and tableName.columnName <![CDATA[<>]]> value (Optional)
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderMultiple NotEquals(string tableName, string columnName, bool? value)
        {
            if (value == null)
            {
                return this;
            }
            long temp = Number;
            QuerySql.Append($" and [{tableName}].[{columnName}] <> @{tableName}_{columnName}{temp} ");
            Parameters.Add($"{tableName}_{columnName}{temp}", value);
            return this;
        }

        /// <summary>
        /// and tableName.columnName <![CDATA[<>]]> value (Optional)
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderMultiple NotEquals(string tableName, string columnName, long? value)
        {
            if (value == null || value == 0)
            {
                return this;
            }

            long temp = Number;
            QuerySql.Append($" and [{tableName}].[{columnName}] <> @{tableName}_{columnName}{temp} ");
            Parameters.Add($"{tableName}_{columnName}{temp}", value);

            return this;
        }


        /// <summary>
        /// 大于
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderMultiple Greater(string tableName, string columnName, DateTime? value)
        {
            if (value == null)
            {
                return this;
            }

            long temp = Number;
            QuerySql.Append($" and [{tableName}].[{columnName}] > @{tableName}_{columnName}{temp} ");
            Parameters.Add($"{tableName}_{columnName}{temp}", value);

            return this;
        }
        /// <summary>
        /// 小于
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderMultiple Less(string tableName, string columnName, DateTime? value)
        {
            if (value == null)
            {
                return this;
            }

            long temp = Number;
            QuerySql.Append($" and [{tableName}].[{columnName}] < @{tableName}_{columnName}{temp} ");
            Parameters.Add($"{tableName}_{columnName}{temp}", value);

            return this;
        }

        /// <summary>
        ///  and ( tableName.columnName = value1 or tableName.columnName = value2)
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public SqlBuilderMultiple EqualsOr(string tableName, string columnName, IList<string> values)
        {
            if (values == null)
            {
                return this;
            }
            if (!values.Any())
            {
                QuerySql.Append(" and 1 != 1");
                return this;
            }
            var stringBuilder = new StringBuilder();
            QuerySql.Append(" and (");
            foreach (var value in values)
            {
                var temp = Number;
                stringBuilder.Append($" or [{tableName}].[{columnName}] = @{tableName}_{columnName}{temp}");
                Parameters.Add($"{tableName}_{columnName}{temp}", value);
            }
            QuerySql.Append(stringBuilder.ToString().TrimStart(" or".ToCharArray()));
            QuerySql.Append(") ");
            return this;
        }

        /// <summary>
        /// and tableName.columnName like "%value%"
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderMultiple Like(string tableName, string columnName, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return this;
            }
            long temp = Number;
            QuerySql.Append($" and [{tableName}].[{columnName}] like @{tableName}_{columnName}{temp} ");
            Parameters.Add($"{tableName}_{columnName}{temp}", $"%{value}%");
            return this;
        }

        /// <summary>
        /// and (tableName.columnName like "%value%" or like "%value2%")
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnNames"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderMultiple LikeOr(string tableName, IList<string> columnNames, string value)
        {
            if (string.IsNullOrEmpty(value) || columnNames == null || !columnNames.Any())
            {
                return this;
            }

            var stringBuilder = new StringBuilder();
            QuerySql.Append(" and (");
            foreach (var item in columnNames)
            {
                var temp = Number;
                stringBuilder.Append($" or [{tableName}].[{item}] like @{tableName}_{item}{temp} ");
                Parameters.Add($"{tableName}_{item}{temp}", $"%{value}%");
            }
            QuerySql.Append(stringBuilder.ToString().TrimStart(" or".ToCharArray()));
            QuerySql.Append(") ");

            return this;
        }
        /// <summary>
        /// and (columnNames[0] like "%value%" or columnNames[1] like "%value%" )
        /// </summary>
        /// <param name="columnNames"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderMultiple LikeOr(IList<string> columnNames, string value)
        {
            if (string.IsNullOrEmpty(value) || columnNames == null || !columnNames.Any())
            {
                return this;
            }
            var stringBuilder = new StringBuilder();
            QuerySql.Append(" and (");
            foreach (var item in columnNames)
            {
                var temp = Number;
                stringBuilder.Append($" or {item} like @{item}{temp} ");
                Parameters.Add($"{item}{temp}", $"%{value}%");
            }
            QuerySql.Append(stringBuilder.ToString().TrimStart(" or".ToCharArray()));
            QuerySql.Append(" ) ");
            return this;
        }
        /// <summary>
        /// and tableName.columnName is not null
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderMultiple NotNull(string tableName, string columnName, bool value)
        {
            if (!value)
            {
                return this;
            }
            QuerySql.Append($" and [{tableName}].[{columnName}] is not null ");
            return this;
        }

        /// <summary>
        /// and tableName.columnName between beginValue and endValue
        /// 精确到天进行查询
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="beginValue"></param>
        /// <param name="endValue"></param>
        /// <returns></returns>
        public SqlBuilderMultiple BetweenDateByDay(string tableName, string columnName, DateTime? beginValue, DateTime? endValue)
        {
            if (beginValue == null || endValue == null)
            {
                return this;
            }

            long temp1 = Number, temp2 = Number;
            QuerySql.Append($" and [{tableName}].[{columnName}] between @{tableName}_{columnName}{temp1} and @{tableName}_{columnName}{temp2} ");
            Parameters.Add($"{tableName}_{columnName}{temp1}", beginValue.Value.ToString("yyyy-MM-dd"));
            Parameters.Add($"{tableName}_{columnName}{temp2}", endValue.Value.ToString("yyyy-MM-dd 23:59:59"));
            return this;
        }

        /// <summary>
        /// 精确到时分秒进行查询
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="beginValue"></param>
        /// <param name="endValue"></param>
        /// <returns></returns>
        public SqlBuilderMultiple BetweenDateByAccurate(string tableName, string columnName, DateTime? beginValue, DateTime? endValue)
        {
            if (beginValue == null || endValue == null)
            {
                return this;
            }

            long temp1 = Number, temp2 = Number;
            QuerySql.Append($" and [{tableName}].[{columnName}] between @{tableName}_{columnName}{temp1} and @{tableName}_{columnName}{temp2} ");
            Parameters.Add($"{tableName}_{columnName}{temp1}", beginValue);
            Parameters.Add($"{tableName}_{columnName}{temp2}", endValue);
            return this;
        }
        /// <summary>
        /// and tableName.columnName between beginValue and endValue
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="beginValue"></param>
        /// <param name="endValue"></param>
        /// <returns></returns>
        public SqlBuilderMultiple Between(string tableName, string columnName, decimal? beginValue, decimal? endValue)
        {
            if (beginValue == null || endValue == null)
            {
                return this;
            }

            long temp1 = Number, temp2 = Number;
            QuerySql.Append($" and [{tableName}].[{columnName}] between @{tableName}_{columnName}{temp1} and @{tableName}_{columnName}{temp2} ");
            Parameters.Add($"{tableName}_{columnName}{temp1}", beginValue);
            Parameters.Add($"{tableName}_{columnName}{temp2}", endValue);

            return this;
        }

        /// <summary>
        /// and (sql) between value and value2
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="beginValue"></param>
        /// <param name="endValue"></param>
        /// <returns></returns>
        public SqlBuilderMultiple Between(string sql, int? beginValue, int? endValue)
        {
            if (beginValue == null || endValue == null)
            {
                return this;
            }

            long temp1 = Number, temp2 = Number;
            QuerySql.Append($" and ({sql}) between @{temp1} and @{temp2} ");
            Parameters.Add($"{temp1}", beginValue);
            Parameters.Add($"{temp2}", endValue);

            return this;
        }

        /// <summary>
        /// and tableName.columnName in (values[1],values[2])
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public SqlBuilderMultiple NotIn<T>(string tableName, string columnName, IList<T> values)
        {
            if (values == null)
            {
                return this;
            }
            long temp = Number;
            QuerySql.Append($" and [{tableName}].[{columnName}] not in (@{tableName}_{columnName}{temp})");
            Parameters.Add($"{tableName}_{columnName}{temp}", values);
            return this;
        }

        /// <summary>
        /// and tableName.columnName in (values[1],values[2])
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public SqlBuilderMultiple In(string tableName, string columnName, IList<string> values)
        {
            if (values == null)
            {
                return this;
            }
            if (!values.Any())
            {
                QuerySql.Append($" and 1 != 1");
                return this;
            }
            long temp = Number;
            QuerySql.Append($" and [{tableName}].[{columnName}] in (@{tableName}_{columnName}{temp})");
            Parameters.Add($"{tableName}_{columnName}{temp}", values);
            return this;
        }

        /// <summary>
        /// and tableName.columnName in (values[1],values[2])
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public SqlBuilderMultiple In<T>(string tableName, string columnName, IList<T> values)
          where T : struct
        {
            if (values == null)
            {
                return this;
            }
            if (!values.Any())
            {
                QuerySql.Append($" and 1 != 1");
                return this;
            }
            long temp = Number;
            QuerySql.Append($" and [{tableName}].[{columnName}] in (@{tableName}_{columnName}{temp})");
            Parameters.Add($"{tableName}_{columnName}{temp}", values);
            return this;
        }
        /// <summary>
        /// 获得where后面的sql，并做了去 and 处理
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string queryString = QuerySql.ToString();
            return queryString.TrimStart(" and".ToCharArray());
        }

        /// <summary>
        /// 给所有用到的表加上 and table.delete_at is null
        /// </summary>
        /// <returns></returns>
        public SqlBuilderMultiple DeleteAtIsNull()
        {
            foreach (var item in Tables)
            {
                QuerySql.Append($" and [{item}].delete_at is null ");
            }
            return this;
        }
        /// <summary>
        /// 用于补充不确定是否需要DeleteAtIsNull 的表，例如自定义语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public SqlBuilderMultiple DeleteAtIsNull(string tableName)
        {
            QuerySql.Append($" and [{tableName}].delete_at is null ");
            return this;
        }

        /// <summary>
        /// group by
        /// </summary>
        /// <param name="table"></param>
        /// <param name="cloumn"></param>
        /// <param name="groupBy"></param>
        /// <returns></returns>
        public SqlBuilderMultiple GroupBy(string table, string cloumn, bool groupBy = true)
        {
            if (FirstGroupBy)
            {
                FirstGroupBy = false;
                QuerySql.Append($" group by [{table}].[{cloumn}]");
                return this;
            }
            QuerySql.Append($", [{table}].[{cloumn}]");
            return this;
        }
    }
}
