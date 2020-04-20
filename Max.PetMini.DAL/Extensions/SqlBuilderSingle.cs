using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Max.PetMini.DAL.Extensions
{
    /// <summary>
    /// 用于单表查询的sql语句创建
    /// </summary>
    public class SqlBuilderSingle
    {
        private long number;
        private long Number { get { return ++number; } set { number = value; } }
        /// <summary>
        /// 用来存放where 后面跟随的sql
        /// </summary>
        public StringBuilder WhereSQL { get; set; }
        /// <summary>
        /// 存放动态参数
        /// </summary>
        public DynamicParameters Parameters { get; set; }
        public SqlBuilderSingle()
        {
            WhereSQL = new StringBuilder();
            Parameters = new DynamicParameters();
        }
        /// <summary>
        /// and columnName = value
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderSingle Equals(string columnName, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return this;
            }

            long temp = Number;
            WhereSQL.Append($" and [{columnName}] = @{columnName}{temp} ");
            Parameters.Add($"{columnName}{temp}", value);
            return this;
        }

        /// <summary>
        /// and columnName = value
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderSingle NotEquals(string columnName, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return this;
            }
            long temp = Number;
            WhereSQL.Append($" and [{columnName}] != @{columnName}{temp} ");
            Parameters.Add($"{columnName}{temp}", value);
            return this;
        }

        /// <summary>
        /// and columnName = value
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderSingle Equals(string columnName, decimal? value)
        {
            if (value != null && value != 0)
            {
                long temp = Number;
                WhereSQL.Append($" and [{columnName}] = @{columnName}{temp} ");
                Parameters.Add($"{columnName}{temp}", value);
            }
            return this;
        }
        /// <summary>
        /// and columnName = value
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderSingle Equals(string columnName, long? value)
        {
            if (value != null && value != 0)
            {
                long temp = Number;
                WhereSQL.Append($" and [{columnName}] = @{columnName}{temp} ");
                Parameters.Add($"{columnName}{temp}", value);
            }
            return this;
        }
        /// <summary>
        /// and columnName = value
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderSingle Equals(string columnName, int? value)
        {
            if (value == null || value == 0)
            {
                return this;
            }
            var temp = Number;
            WhereSQL.Append($" and [{columnName}] = @{columnName}{temp} ");
            Parameters.Add($"{columnName}{temp}", value);
            return this;
        }
        /// <summary>
        /// and columnName = value
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderSingle Equals(string columnName, bool value)
        {
            var temp = Number;
            WhereSQL.Append($" and [{columnName}] = @{columnName}{temp} ");
            Parameters.Add($"{columnName}{temp}", value);
            return this;
        }
        /// <summary>
        /// and columnName &lt; '%value%'
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderSingle LessThan(string columnName, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return this;
            }
            long temp = Number;
            WhereSQL.Append($" and [{columnName}] < @{columnName}{temp} ");
            Parameters.Add($"{columnName}{temp}", $"%{value}%");

            return this;
        }
        /// <summary>
        /// and columnName &gt; '%value%'
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderSingle GreetThan(string columnName, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return this;
            }
            long temp = Number;
            WhereSQL.Append($" and [{columnName}] > @{columnName}{temp} ");
            Parameters.Add($"{columnName}{temp}", $"%{value}%");

            return this;
        }
        /// <summary>
        /// and columnName::timestamp &gt; '%value%'
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderSingle GreetThanDate(string columnName, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return this;
            }
            long temp = Number;
            WhereSQL.Append($" and [{columnName}] > @{columnName}{temp}::TIMESTAMP ");
            Parameters.Add($"{columnName}{temp}", $"%{value}%");

            return this;
        }
        //::timestamp
        /// <summary>
        /// and columnName like '%value%'
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderSingle Like(string columnName, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return this;
            }
            long temp = Number;
            WhereSQL.Append($" and [{columnName}] like @{columnName}{temp} ");
            Parameters.Add($"{columnName}{temp}", $"%{value}%");

            return this;
        }
        /// <summary>
        /// and (columnNames[0] like '%value%' or columnNames[1] like '%value%' )
        /// </summary>
        /// <param name="columnNames"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlBuilderSingle LikeOr(IList<string> columnNames, string value)
        {
            if (string.IsNullOrEmpty(value) || columnNames == null || !columnNames.Any())
            {
                return this;
            }
            var stringBuilder = new StringBuilder();
            WhereSQL.Append(" and (");
            foreach (var item in columnNames)
            {
                var temp = Number;
                stringBuilder.Append($" or [{item}] like @{item}{temp} ");
                Parameters.Add($"{item}{temp}", $"%{value}%");
            }
            WhereSQL.Append(stringBuilder.ToString().TrimStart(" or".ToCharArray()));
            return this;
        }



        /// <summary>
        /// and columnName between beginValue and endValue
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="beginValue"></param>
        /// <param name="endValue"></param>
        /// <returns></returns>
        public SqlBuilderSingle BetweenDateByDay(string columnName, DateTime? beginValue, DateTime? endValue)
        {
            if (beginValue != null && endValue != null)
            {
                long temp1 = Number;
                long temp2 = Number;
                WhereSQL.Append($" and [{columnName}] between @{columnName}{temp1} and @{columnName}{temp2} ");
                Parameters.Add($"{columnName}{temp1}", beginValue.Value.ToString("yyyy-MM-dd"));
                Parameters.Add($"{columnName}{temp2}", endValue.Value.ToString("yyyy-MM-dd 23:59:59"));
            }
            return this;
        }
        /// <summary>
        /// and columnName between beginValue and endValue
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="beginValue"></param>
        /// <param name="endValue"></param>
        /// <returns></returns>
        public SqlBuilderSingle BetweenDateByAccurate(string columnName, decimal? beginValue, decimal? endValue)
        {
            if (beginValue != null && beginValue != 0 && endValue != null && endValue != 0)
            {
                long temp1 = Number;
                long temp2 = Number;
                WhereSQL.Append($" and [{columnName}] between @{columnName}{temp1} and @{columnName}{temp2} ");
                Parameters.Add($"{columnName}{temp1}", beginValue);
                Parameters.Add($"{columnName}{temp2}", endValue);
            }
            return this;
        }

        /// <summary>
        /// and columnName in (values[1],values[2])
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public SqlBuilderSingle In(string columnName, IEnumerable<long> values)
        {
            if (values == null)
            {
                return this;
            }
            long temp = Number;
            WhereSQL.Append($" and [{columnName}] in (@{columnName}{temp})");
            Parameters.Add($"{columnName}{temp}", values.ToList());
            return this;
        }

        /// <summary>
        /// 获得where后面的sql
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return " 1=1 "+ WhereSQL.ToString();
        }

    }
}
