using challenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace challenge.Services
{
    public interface IEmployeeService
    {
        Employee GetById(String id, bool loadCompensation);
		Task<CompensationResponse> GetCompensationById(string id, CancellationToken cancellationToken);
		Task<ReportingStructure> GetReportingStructureForEmployee(string id, CancellationToken cancellationToken);
		Employee Create(Employee employee);
        Employee Replace(Employee originalEmployee, Employee newEmployee);
		Task<CompensationResponse> ReplaceCompensation(Employee employee, Compensation compensation, CancellationToken cancellationToken);

	}
}
