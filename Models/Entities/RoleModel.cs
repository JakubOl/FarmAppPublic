using AspNetCore.Identity.MongoDbCore.Models;

namespace Models.Entities
{
	public class RoleModel : MongoIdentityRole<Guid>
	{

		public string RoleName { get; set; }
	}
}
