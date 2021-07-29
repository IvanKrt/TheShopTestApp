namespace TheShop.Models.RequestModels
{
	public class Supplier1Model
	{
		public bool ArticleInInventory(int id)
		{
			return true;
		}

		public ArticleModel GetArticle(int id)
		{
			return new ArticleModel()
			{
				ID = 1,
				Name_of_article = "Article from supplier1",
				ArticlePrice = 458
			};
		}
	}
}
