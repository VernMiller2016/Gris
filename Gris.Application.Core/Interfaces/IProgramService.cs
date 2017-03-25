using Gris.Domain.Core.Models;
using System.Collections.Generic;

namespace Gris.Application.Core.Interfaces
{
    public interface IProgramService
    {
        IEnumerable<Program> GetPrograms();

        Program GetById(int id);

        void AddProgram(Program program);

        void UpdateProgram(Program program);

        void Remove(Program program);

        /// <summary>
        /// Get all the available paysources, which are "not related yet" to any program.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PaySource> GetAvailablePaySourcesNotRelatedToPrograms();
    }
}