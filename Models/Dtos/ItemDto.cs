using Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class ItemDto
    {
		public string Id { get; set; }	
		[Required]
		public string Title { get; set; }
		public string Description { get; set; }
		[Required]
		public string Price { get; set; }
		public bool IsActive { get; set; }
		[Required]
		public string CategoryId { get; set; }
	}
}
