using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;
using System.Threading;

namespace challenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
            return _employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == id);
        }

		public async Task<ReportingStructure> GetReportingStructureForEmployee(string id, CancellationToken cancellationToken)
		{
			Employee employee = employee = await _employeeContext
													.Employees
													.Include(e => e.DirectReports)
													.SingleOrDefaultAsync(e => e.EmployeeId == id, cancellationToken);

			if( employee == null )
			{
				return null;
			}

			IList<string> excludeIds		= new List<string>() { id } ;
			IList<string> directReportIds	= employee.DirectReports?.Select(s => s.EmployeeId).ToList();

			ReportingStructure reportingStructure = new ReportingStructure()
			{
				Employee        = $"{ employee.FirstName } {employee.LastName}",
				NumberOfReports = directReportIds.Count()
			};

			if( directReportIds.Count > 0 )
			{
				int numberOfReports = await GetNumberOfReports(directReportIds, excludeIds, cancellationToken);
				reportingStructure.NumberOfReports += numberOfReports;
			}

			return reportingStructure;												
		}

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }

		private async Task<int> GetNumberOfReports(IEnumerable<string> ids, IEnumerable<string> exlcudedIds, CancellationToken cancellationToken)
		{
			IEnumerable<string> filteredIds = ids.Except(exlcudedIds);

			if( filteredIds.Count() == 0 )
			{
				return 0;
			}

			//Some of this can be simplified if some assumptions can be made e.g. e.g. Employees can report to more than one boss 
			//an Employee cant have a direct report that is my boss etc.

			IList<string> directEmployeeIds = await _employeeContext
														.Employees
														.Where(e => filteredIds.Contains(e.EmployeeId))
														.SelectMany(e => e.DirectReports, (e, dr) => dr.EmployeeId)
														.Where(id => !exlcudedIds.Contains(id))
														.Distinct()
														.ToListAsync(cancellationToken);

			IEnumerable<string> newExclduedList = exlcudedIds.Union(ids);

			int numberOfReports = await GetNumberOfReports(directEmployeeIds, newExclduedList, cancellationToken);
			
			return directEmployeeIds.Count + numberOfReports;
		}
	}
}
