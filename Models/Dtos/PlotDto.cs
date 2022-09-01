using Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class PlotDto
    {
		[Required]
		public string City { get; set; }

		[Required]
		[RegularExpression(@"^[0-9/]{1,40}$",
		ErrorMessage = "Characters are not allowed.")]
		public string PlotNumber { get; set; }

		[RegularExpression(@"^[0-9\.,]{1,6}$",
		ErrorMessage = "Characters are not allowed.")]
		public string Area { get; set; }
		public string Tillage { get; set; }
		public List<WorkModel> Works { get; set; }
	}
}
