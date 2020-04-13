using AccessData.Context;
using AccessData.Entities;
using App.Common.Classes.Base.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccessData.Repository.IMoveDetailRepository
{
    public class MoveDetailRepository : BaseCRUDRepository<MoveDetail>, IMoveDetailRepository
    {


        public DBContext Context
        {
            get
            {
                return (DBContext)_Database;
            }
        }

        public MoveDetailRepository(DBContext database)
            : base(database)
        {

        }

    }
}
