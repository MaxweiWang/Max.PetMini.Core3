namespace Max.PetMini.Extension.Models
{
    /// <summary>
    /// 分页查询
    /// </summary>
    public class PageParameter
    {
        /// <summary>
        /// 当前页大小
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 页码
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// 偏移记录数
        /// </summary>
        public int Offset
        {
            get { return PageSize * (PageNumber - 1); }
        }
    }
}
