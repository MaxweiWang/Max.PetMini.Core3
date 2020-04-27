using System;
using System.Collections.Generic;
using System.Text;

namespace Max.PetMini.BLL.Models
{
    public class CatListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageSrc { get; set; }
        public string OpenType { get; set; }
        public string NavigatorUrl { get; set; }
    }
}
