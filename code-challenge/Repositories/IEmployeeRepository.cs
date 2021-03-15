using challenge.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public interface IEmployeeRepository
    {
        Employee GetById(String id, bool includeCompensation);
		Task<CompensationResponse> GetCompensationById(string id, CancellationToken cancellationToken);
		Task<ReportingStructure> GetReportingStructureForEmployee(string ids, CancellationToken cancellationToken);
		Employee Add(Employee employee);
        Employee Remove(Employee employee);
        Task SaveAsync();
		Task<int> SaveAsync(CancellationToken cancellationToken);
	}
}