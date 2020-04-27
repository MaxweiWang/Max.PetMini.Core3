using Max.PetMini.BLL.Models;
using Max.PetMini.DAL.Entities;
using Max.PetMini.DAL.Models;
using Max.PetMini.DAL.Repositories;
using Max.PetMini.Extension.Exceptions;
using Max.PetMini.Extension.Models;
using NAutowired.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Max.PetMini.BLL.Services
{
    [Service]
    public class PetTestService : Service
    {
        [Autowired]
        private readonly CatListRepository catListRepository;

        /// <summary>
        /// 通过id来获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CatListModel GetById(int id)
        {
            var catlist= catListRepository.GetById(id);
            return new CatListModel { Id = catlist.Id, Name = catlist.Name, ImageSrc = catlist.ImageSrc, NavigatorUrl = catlist.NavigatorUrl, OpenType = catlist.OpenType };
        }

    }
}
