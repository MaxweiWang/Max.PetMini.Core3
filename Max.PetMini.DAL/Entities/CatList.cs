using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace Max.PetMini.DAL.Entities
{
    [Table("CatList")]
    public class CatList
    {

        #region 属性
        /// <summary>
        /// 主键Id
        /// </summary>
        [ExplicitKey]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageSrc { get; set; }
        public string OpenType { get; set; }
        public string NavigatorUrl { get; set; }

        #endregion
    }
}
