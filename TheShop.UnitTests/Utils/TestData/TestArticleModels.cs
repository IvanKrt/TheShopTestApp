using TheShop.Models.RequestModels;

namespace TheShop.UnitTests.Utils.TestData
{
	public static class TestArticleModels
	{
		public static ArticleModel ArticleModel(int id, string name, int price)
		{
			return new ArticleModel() { 
				ID = id,
				Name_of_article = name,
				ArticlePrice = price
			};

		}
	}
}
