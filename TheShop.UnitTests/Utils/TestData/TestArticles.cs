using System;
using TheShop.Models.Entities;

namespace TheShop.UnitTests.Utils.TestData
{
	public static class TestArticles
	{
		public static Article Valid(int articleId)
		{
			return new Article
			{
				Id = articleId,
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
