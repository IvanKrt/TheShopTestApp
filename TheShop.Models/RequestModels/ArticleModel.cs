using System;
using System.Text;
using TheShop.Models.Entities;

namespace TheShop.Models.RequestModels
{
	public class ArticleModel
	{
		public int ID { get; set; }

		public string Name_of_article { get; set; }

		public int ArticlePrice { get; set; }

		public Article ConvertToArticle()
		{
			var supplierId = ParseNameToSupplierId(Name_of_article);

			if (supplierId == 0)
			{
				return null;
			}

			return new Article()
			{
				ExternalId = ID,
				Price = ArticlePrice,
				SupplierId = supplierId,
				Name = Name_of_article
			};
		}

		private int ParseNameToSupplierId(string nameOfArticle)
		{
			var idAsArrayString = new StringBuilder();

			for (int i = Name_of_article.Length - 1; i >= 0; i--)
			{
				if (Char.IsDigit(Name_of_article[i]))
				{
					idAsArrayString.Insert(0, Name_of_article[i]);
				}
				else
				{
					break;
				}
			}

			return Int32.TryParse(idAsArrayString.ToString(), out int result) ? result : 0;
		}
	}
}
