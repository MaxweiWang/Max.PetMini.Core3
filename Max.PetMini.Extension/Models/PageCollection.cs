﻿using System.Collections.Generic;

namespace Max.PetMini.Extension.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageCollection<T>
    {
        /// <summary>
        /// 当前第几页，从1开始
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// 一共多少记录
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 一共多少页
        /// </summary>
        public int PageCount
        {
            get
            {
                if (TotalCount % PageSize == 0)
                {
                    return TotalCount / PageSize;
                }
                else
                {
                    return TotalCount / PageSize + 1;
                }
            }
        }

        /// <summary>
        /// 内部包裹的数据
        /// </summary>
        public IList<T> Collection { get; set; }
    }
}
