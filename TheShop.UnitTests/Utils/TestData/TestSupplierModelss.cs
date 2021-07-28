using System.Collections.Generic;
using TheShop.Models.RequestModels;

namespace TheShop.UnitTests.Utils.TestData
{
	public static class TestSupplierModels
	{
		public static List<SupplierModel> List()
		{
			return new List<SupplierModel>() {
				new SupplierModel
				{
					Articles = new List<ArticleModel>()
						{
							TestArticleModels.ArticleModel(1, "Article from supplier1", 458)
						}
				},
				new SupplierModel
				{
					Articles = new List<ArticleModel>()
						{
							TestArticleModels.ArticleModel(1, "Article from supplier2", 459)
						}
				},
				new SupplierModel
				{
					Articles = new List<ArticleModel>()
						{
							TestArticleModels.ArticleModel(1, "Article from supplier3", 460)
						}
				}
			};
		}
	}
}
