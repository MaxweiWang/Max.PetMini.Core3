using System;
using System.Collections.Generic;
using System.Text;

namespace Max.PetMini.DAL.Models
{

    public class BootstrapDataGridInput
    {

        public BootstrapDataGridInput()
        {
            this.Page = 0;
            this.Rows = 10;
        }
        /// <summary>
        /// 页码
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// 显示行数
        /// </summary>
        public int Rows { get; set; }

        /// <summary>
        /// 排序方向
        /// </summary>
        public string order { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string sort { get; set; }
    }
}
