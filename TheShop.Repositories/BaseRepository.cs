using System.Collections.Generic;
using TheShop.Models.Base;
using TheShop.Repositories.Context;
using TheShop.Repositories.Interfaces;

namespace TheShop.Repositories
{
	public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseModel
	{
		public BaseRepository(IDBContext context)
		{
			DBContext = context;
		}

		protected abstract int GetLastId();

		public IDBContext DBContext { get; set; }

		public virtual int Create(TEntity entity)
		{
			throw new System.NotImplementedException();
		}

		public virtual IEnumerable<TEntity> GetAll()
		{
			throw new System.NotImplementedException();
		}

		public virtual TEntity GetById(int id)
		{
			throw new System.NotImplementedException();
		}

		public virtual void Remove(int id)
		{
			throw new System.NotImplementedException();
		}

		public virtual bool Update(TEntity entity)
		{
			throw new System.NotImplementedException();
		}
	}
}
