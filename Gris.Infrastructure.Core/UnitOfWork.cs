using Gris.Application.Core.Interfaces;
using GRis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gris.Infrastructure.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields

        private ApplicationDbContext _dbContext;

        #endregion Fields

        #region Ctor

        public UnitOfWork(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        #endregion Constructors

        #region IUnitOfWork Members

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        #endregion Methods
    }
}
