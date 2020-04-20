using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Max.PetMini.DAL.Entities
{
    [Table("TExpress")]
    public class Express
    {
        #region 数据结构

        public struct ZpOrQtStruct
        {
            /// <summary>
            /// 自配		
            /// </summary>
            public static readonly string ZP = "自配";
            /// <summary>
            /// 其它		
            /// </summary>
            public static readonly string QT = "其它";
        }

        /// <summary>
        /// 是否前台展示枚举
        /// </summary>
        public struct StayOnTopStruct
        {
            /// <summary>
            /// 显示		
            /// </summary>
            public static readonly string Yes = "Y";
            /// <summary>
            /// 不显示		
            /// </summary>
            public static readonly string No = "N";
        }

        /// <summary>
        /// 是否支持一票多件		
        /// </summary>
        public struct IsMultiPackageStruct
        {
            /// <summary>
            /// 否		
            /// </summary>
            public static readonly string No = "N";
            /// <summary>
            /// 是		
            /// </summary>
            public static readonly string Yes = "Y";
        }
        /// <summary>
        /// 状态		
        /// </summary>
        public struct StatusStruct
        {
            /// <summary>
            /// 有效		
            /// </summary>
            public static readonly string Active = "A";
            /// <summary>
            /// 无效		
            /// </summary>
            public static readonly string Passive = "P";
            /// <summary>
            /// 删除		
            /// </summary>
            public static readonly string Delete = "X";
        }

        #endregion

        #region 属性
        /// <summary>
        /// 主键Id
        /// </summary>
        [ExplicitKey]
        public int ExpressId { get; set; }
        public int InterfaceId { get; set; }
        /// <summary>
        /// 快递公司编码
        /// </summary>
        public string ExpressCode { get; set; }
        /// <summary>
        /// 快递公司名称
        /// </summary>
        public string ExpressName { get; set; }
        public string IsMultiPackage { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public int UpdateUserId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 是否前台展示
        /// </summary>
        public string IsDisplayOnWeb { get; set; }
        #endregion
    }
}
