using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hurriyet.Model;

namespace Hurriyet.Caching
{
    public class NewsCacheManager : INewsCacheManager
    {
        IRedisTypedClient<News> _client;
        IRedisList<News> _productList;
        IRedisClientsManager _redisManager;


        public NewsCacheManager(IRedisClientsManager redisManager)
        {
            _redisManager = redisManager;
            
        }
        public void AddNews(News product)
        {
            _productList.Add(product);
            


        }
        public void RemoveNews(News product)
        {
            _productList.Remove(product);
            //_client.Save();
        }

        public News[] GetNews(int start, int take,string key = null)
        {
            if (key != null)
                return _productList.Where(p => p.Name.Contains(key)).Skip(start).Take(take).ToArray();
            
            var list = _productList.Skip(start).Take(take).ToArray();
            return list;

        }

        public int NewsCount()
        {
            return _productList.Count;
        }

        public void InitCache()
        {
            _client = _redisManager.GetClient().As<News>();
            _productList = _client.Lists["urn:products"];
        }

        public void UpdateNews(News product)
        {
            News p = _productList.Where(pp => pp.Id == product.Id ).SingleOrDefault();
            if(p != null)
            {
                p.Name = product.Name;
                p.UpdatedDate = product.UpdatedDate;

            }
        }
    }
    public interface INewsCacheManager
    {
        void RemoveNews(News product);
        void AddNews(News product);

        void UpdateNews(News product);

        News[] GetNews(int start, int take, string key = null);

        int NewsCount();
        void InitCache();
    }
}
