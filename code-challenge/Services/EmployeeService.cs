using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public Employee Create(Employee employee)
        {
            if(employee != null)
            {
                _employeeRepository.Add(employee);
                _employeeRepository.SaveAsync().Wait();
            }

            return employee;
        }

		public async Task<CompensationResponse> GetCompensationById(string id, CancellationToken cancellationToken)
		{
			if( !String.IsNullOrEmpty(id) )
			{
				return await _employeeRepository.GetCompensationById(id, cancellationToken);
			}

			return null;
		}

		public Employee GetById(string id, bool includeCompensation)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetById(id, includeCompensation);
            }

            return null;
        }

		public async Task<ReportingStructure> GetReportingStructureForEmployee(string id, CancellationToken cancellationToken)
		{
			if( !String.IsNullOrEmpty(id) )
			{
				return await _employeeRepository.GetReportingStructureForEmployee(id, cancellationToken);
			}

			return null;
		}

		public Employee Replace(Employee originalEmployee, Employee newEmployee)
        {
            if(originalEmployee != null)
            {
                _employeeRepository.Remove(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    _employeeRepository.SaveAsync().Wait();

                    _employeeRepository.Add(newEmployee);
                    // overwrite the new id with previous employee id
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                }
                _employeeRepository.SaveAsync().Wait();
            }

            return newEmployee;
        }

		public async Task<CompensationResponse> ReplaceCompensation(Employee employee, Compensation compensation, CancellationToken cancellationToken)
		{
			if( employee.Compensation == null )
			{
				employee.Compensation = new Compensation();
			}

			employee.Compensation.EffectiveDate = compensation.EffectiveDate;
			employee.Compensation.Salary        = compensation.Salary;

			await _employeeRepository.SaveAsync(cancellationToken);

			return await GetCompensationById(employee.EmployeeId, cancellationToken);
		}
    }
}
