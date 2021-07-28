using System;
using TheShop.Models.Entities;

namespace TheShop.UnitTests.Utils.TestData
{
	public static class TestArticles
	{
		public static Article Valid()
		{
			return new Article
			{
				Id = 2,
				ExternalId = 1,
				Name = "Article from supplier 2",
				Price = 459,
				IsSold = true,
				SoldDate = DateTime.Now,
				BuyerUserId = 10,
				SupplierId = 2
			};
		}
	}
}
