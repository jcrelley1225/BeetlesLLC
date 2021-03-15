using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Models
{
	public class CompensationResponse
	{
		public DateTimeOffset?	EffectiveDate	{ get; set; }
		public string			Employee		{ get; set; }
		public decimal?			Salary			{ get; set; }
	}
}
