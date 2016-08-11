using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Hurriyet.Caching;
using Hurriyet.Data.Infrastructure;
using Hurriyet.Data.Repositories;
using Hurriyet.Model;

namespace Hurriyet.Web.Api.Controllers
{
    /// <summary>
    /// Haber Servisi
    /// </summary>
    [RoutePrefix("api/product")]
    public class NewsController : ApiController
    {
        private readonly IUnitOfWork _unitOfwork;
        private readonly INewsRepository _productRepository;
        private readonly INewsCacheManager _productCachemanager;





        public NewsController(IUnitOfWork unitOfwork, INewsRepository productRepository, INewsCacheManager productCacheManager)
        {
            _unitOfwork = unitOfwork;
            _productRepository = productRepository;
            _productCachemanager = productCacheManager;
        }

        /// <summary>
        /// Haber listeleme servisi
        /// </summary>
        /// <param name="start">baslangic indexi</param>
        /// <param name="take">okunacak kayit sayisi</param>
        /// <param name="keyword">aranacak icerik</param>
        /// <returns></returns>
        [Route("list/{start}/{take}/{keyword?}"), HttpGet, AllowAnonymous, Description("Get News List")]
        public News[] List(int start, int take, string keyword = null)
        {
            return _productCachemanager.GetNews(start, take, keyword);

        }

        /// <summary>
        /// id ile haber getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("get/{id}"), HttpGet, AllowAnonymous, Description("Get News By Id")]
        public News Get(long id)
        {
            return _productRepository.GetById(id);
        }

        /// <summary>
        /// haber ekle
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [Route("add"), HttpPost, AllowAnonymous, Description("Add News")]
        public bool Add(News product)
        {
            try
            {
                _productRepository.Add(product);
                _unitOfwork.Commit();
                _productCachemanager.AddNews(product);
                return true;

            }
            catch (Exception ex)
            {
                // ex loglanabilir
                return false;
            }
        }

        /// <summary>
        /// haber guncelle
        /// </summary>
        /// <param name="product">haber objesi</param>
        /// <returns></returns>
        [Route("update"), HttpPost, AllowAnonymous, Description("Update News")]
        public bool Update(News product)
        {
            try
            {

                //var cachedNews = _productCachemanager.RemoveNews


                News oldNews = _productRepository.GetById(product.Id);
                oldNews.Name = product.Name;
                _productRepository.Update(oldNews);
                _unitOfwork.Commit();
                _productCachemanager.UpdateNews(oldNews);
                return true;

            }
            catch (Exception ex)
            {
                // ex loglanabilir
                return false;
            }
        }

        /// <summary>
        /// haber sil
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("delete/{id}"), HttpPost, AllowAnonymous, Description("Delete News")]
        public bool Delete(long id)
        {
            try
            {
                News oldNews = _productRepository.GetById(id);
                _productRepository.Delete(oldNews);
                _unitOfwork.Commit();
                _productCachemanager.RemoveNews(oldNews);
                return true;

            }
            catch (Exception ex)
            {
                // ex loglanabilir
                return false;
            }
        }



        [Route("addToCache"), HttpGet, AllowAnonymous, Description("Cachi doldur test icin")]
        public bool AddToCache(News product)
        {
            try
            {
                var products = _productRepository.GetAll().ToList();
                foreach (var item in products)
                {
                    _productCachemanager.AddNews(item);
                }
                return true;

            }
            catch (Exception ex)
            {
                // ex loglanabilir
                return false;
            }
        }


        [Route("listcount"), HttpGet, AllowAnonymous, Description("Get product count")]
        public int GetListCount()
        {
            return _productCachemanager.NewsCount();
        }

    }
}

