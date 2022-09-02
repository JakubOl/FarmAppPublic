using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
	public class PlotModel
	{
		[BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		public string City { get; set; }
		public string PlotNumber { get; set; }
		public string Area { get; set; }
		public decimal Latitude { get; set; }
		public decimal Longitude { get; set; }
		public string Tillage { get; set; }
		public int TillageId { get; set; }
		public string Note { get; set; } 
	}
}
