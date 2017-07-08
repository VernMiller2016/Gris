using Gris.Application.Core.Contracts.Paging;
using Gris.Application.Core.Interfaces;
using Gris.Domain.Core.Models;
using Gris.Infrastructure.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Gris.Application.Core.Services
{
    public class ProgramService : IProgramService
    {
        private IProgramRepository _programRepository;
        private IPaySourceRepository _paySourceRepository;
        private IUnitOfWork _unitOfWork;

        public ProgramService(IProgramRepository programRepository, IPaySourceRepository paySourceRepository, IUnitOfWork unitOfWork)
        {
            _programRepository = programRepository;
            _paySourceRepository = paySourceRepository;
            _unitOfWork = unitOfWork;
        }

        public void AddProgram(Program program)
        {
            _programRepository.Add(program);
            _unitOfWork.Commit();
        }

        public Program GetById(int id)
        {
            return _programRepository.OneOrDefault(t => t.Id == id, t => t.PaySources);
        }

        public IEnumerable<Program> GetPrograms(PagingInfo pagingInfo = null)
        {
            if (pagingInfo == null)
                return _programRepository.Get(null, null, t => t.PaySources);
            else
            {
                int total = 0;
                var result = _programRepository.FilterWithPaging(null, (list => list.OrderBy(p => p.Name))
                    , out total, pagingInfo.PageIndex, AppSettings.PageSize, t => t.PaySources);
                pagingInfo.Total = total;
                return result;
            }
        }

        public void Remove(Program Program)
        {
            _programRepository.Delete(Program);
            _unitOfWork.Commit();
        }

        public void UpdateProgram(Program Program)
        {
            _programRepository.Update(Program);
            _unitOfWork.Commit();
        }
    }
}