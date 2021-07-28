using System.Collections.Generic;
using TheShop.Models.Entities;

namespace TheShop.Repositories.Context
{
	public interface IDBContext
	{
		List<Article> Articles { get; set; }

		bool SaveChanges();
	}
}
