﻿using System.Linq;
using TheShop.Models.Entities;
using TheShop.Repositories.Context;
using TheShop.Repositories.Interfaces;

namespace TheShop.Repositories
{
	public class ArticleRepository : BaseRepository<Article>, IArticleRepository
	{
		public ArticleRepository(IDBContext context) : base(context)
		{
		}

		protected override int GetLastId()
		{
			if (DBContext.Articles.Count > 0)
			{
				return DBContext.Articles.Max(_ => _.Id);
			}

			return 0;
		}

		public override int Create(Article article)
		{
			var lastId = GetLastId();
			DBContext.Articles.Add(new Article
			{
				Id = ++lastId,
				ExternalId = article.ExternalId,
				Price = article.Price,
				SupplierId = article.SupplierId,
				Name = article.Name
			});

			return lastId;
		}

		public override Article GetById(int id)
		{
			return DBContext.Articles.FirstOrDefault(_ => _.Id == id);
		}

		public Article GetByExternalId(int externalId)
		{
			return DBContext.Articles.FirstOrDefault(_ => _.ExternalId == externalId);
		}
	}
}
