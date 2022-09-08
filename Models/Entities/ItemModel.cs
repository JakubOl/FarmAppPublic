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
		public DateTime Created { get; set; } = DateTime.Now;
		public string Price { get; set; }
		public string AuthorId { get; set; }
		public bool IsActive { get; set; }
		public string CategoryId { get; set; }
		public virtual CategoryModel Category { get; set; }
		public List<TagModel> Comments { get; set; } = new List<TagModel>();
		public int Likes { get; set; }
		public string ImageName { get; set; }

	}
}
