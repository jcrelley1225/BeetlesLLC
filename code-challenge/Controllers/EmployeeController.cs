﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;
using System.Threading;

namespace challenge.Controllers
{
    [Route("api/employee")]
    public class EmployeeController : Controller
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            _logger.LogDebug($"Received employee create request for '{employee.FirstName} {employee.LastName}'");

            _employeeService.Create(employee);

            return CreatedAtRoute("getEmployeeById", new { id = employee.EmployeeId }, employee);
        }

        [HttpGet("{id}", Name = "getEmployeeById")]
        public IActionResult GetEmployeeById(String id)
        {
            _logger.LogDebug($"Received employee get request for '{id}'");

            var employee = _employeeService.GetById(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

		[HttpGet("{id}/reportingStructure", Name = "getReportingStructureByEmployeeById")]
		public async Task<IActionResult> GetReportingStructureByEmployeeById(String id, CancellationToken cancellationToken)
		{
			_logger.LogDebug($"Received employee reporting structure request request for '{id}'");

			ReportingStructure reportingStructure = await _employeeService.GetReportingStructureForEmployee(id, cancellationToken);

			if( reportingStructure == null )
				return NotFound();

			return Ok(reportingStructure);
		}

		[HttpPut("{id}")]
		public IActionResult ReplaceEmployee(String id, [FromBody]Employee newEmployee)
		{
			_logger.LogDebug($"Recieved employee update request for '{id}'");

			var existingEmployee = _employeeService.GetById(id);
			if (existingEmployee == null)
				return NotFound();

			_employeeService.Replace(existingEmployee, newEmployee);

			return Ok(newEmployee);
		}
	}
}
