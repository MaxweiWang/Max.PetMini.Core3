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

        public CatList GetById(int id)
        {
            return catListRepository.GetById(id);
        }

    }
}
