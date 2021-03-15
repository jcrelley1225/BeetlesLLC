using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Models
{
	public class Compensation
	{
		[Key]
		public string			EmployeeID		{ get; set; }

		[Required(AllowEmptyStrings =false)]
		public DateTimeOffset?	EffectiveDate	{ get; set; }

		[Required(AllowEmptyStrings = false)]
		public decimal?			Salary			{ get; set; }
	}
}
