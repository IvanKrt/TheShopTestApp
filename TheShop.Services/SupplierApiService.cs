using System.Collections.Generic;
using TheShop.Models.RequestModels;
using TheShop.Services.Interfaces;

namespace TheShop.Services
{
	public class SupplierApiService : ISupplierApiService
	{
		public List<SupplierModel> GetSuppliers()
		{
			var suppliers = new List<SupplierModel>() {
				new SupplierModel
				{
					Articles = new List<ArticleModel>()
						{
							new ArticleModel
							{
								ID= 1,
								Name_of_article= "Article from supplier1",
								ArticlePrice = 458
							}
						}
				},
				new SupplierModel
				{
					Articles = new List<ArticleModel>()
						{
							new ArticleModel
							{
								ID= 1,
								Name_of_article= "Article from supplier2",
								ArticlePrice = 459
							}
						}
				},
				new SupplierModel
				{
					Articles = new List<ArticleModel>()
						{
							new ArticleModel
							{
								ID= 1,
								Name_of_article= "Article from supplier3",
								ArticlePrice = 460
							}
						}
				}
			};

			return suppliers;
		}
	}
}
