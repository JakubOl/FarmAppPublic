using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class ItemModel
    {
		[BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime ProductionDate { get; set; }
		public DateTime Created { get; set; } = new DateTime();
		public string Price { get; set; }
		public string AuthorId { get; set; }
		public bool IsActive { get; set; } = false;
		public string TypeId { get; set; }
		public virtual TypeModel Type { get; set; }
		public List<string> Comments { get; set; } = new List<string>();
		public int Likes { get; set; }
	}
}
