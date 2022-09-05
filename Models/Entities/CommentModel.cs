using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class CommentModel
    {
		[BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string AuthorId { get; set; }
		public DateTime Created { get; set; } = new DateTime();
		public int Likes { get; set; }
	}
}
