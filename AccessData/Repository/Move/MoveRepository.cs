using AccessData.Context;
using AccessData.Entities;
using App.Common.Classes.Base.Repositories;
using Common.Classes.BussinesLogic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AccessData.Repository
{
    public class MoveRepository : BaseCRUDRepository<Move>, IMoveRepository
    {
        public DBContext Context
        {
            get
            {
                return (DBContext)_Database;
            }
        }

        public MoveRepository(DBContext database)
            : base(database)
        {

        }
    }
}
