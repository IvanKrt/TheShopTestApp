using System;
using TheShop.Models.Base;

namespace TheShop.Models.Entities
{
	public class Article : BaseModel
	{ 
		public int ExternalId { get; set; }

		public string Name { get; set; }

		public int Price { get; set; }

		public bool IsSold { get; set; }

		public DateTime SoldDate { get; set; }

		public int BuyerUserId { get; set; }

		public int SupplierId { get; set; }
	}
}
