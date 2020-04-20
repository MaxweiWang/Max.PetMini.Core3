using Max.PetMini.Extension.Models;
using System;
using System.Linq;

namespace Max.PetMini.Extension
{
    /// <summary>
    /// 分页模型拓展
    /// </summary>
    public static class PageCollectionExtension
    {
        /// <summary>
        /// 将分页包裹住的模型进行转换
        /// </summary>
        /// <typeparam name="FromType"></typeparam>
        /// <typeparam name="ToType"></typeparam>
        /// <param name="pageCollection"></param>
        /// <param name="convert"></param>
        /// <returns></returns>
        public static PageCollection<ToType> ToPageCollection<FromType, ToType>(this PageCollection<FromType> pageCollection, Func<FromType, ToType> convert)
          where FromType : class, new()
          where ToType : class, new()
        {
            PageCollection<ToType> result = new PageCollection<ToType>()
            {
                Collection = pageCollection.Collection.Select(convert).ToList(),
                PageNumber = pageCollection.PageNumber,
                PageSize = pageCollection.PageSize,
                TotalCount = pageCollection.TotalCount
            };
            return result;
        }
    }
}
