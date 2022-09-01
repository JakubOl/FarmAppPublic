using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
	public class WorkModel
	{
		[BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		public string WorkName { get; set; }
		public bool IsDone { get; set; } = false;
		public int Time { get; set; }
	}
}
