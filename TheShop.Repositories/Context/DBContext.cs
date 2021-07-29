using System.Collections.Generic;
using TheShop.Models.Entities;

namespace TheShop.Repositories.Context
{
	public class DBContext
	{
		public List<Article> Articles { get; set; }

		public DBContext()
		{
			Articles = new List<Article>();
		}

		public bool SaveChanges()
		{
			return true;
		}
	}
}
