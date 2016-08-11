using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Hurriyet.Model;
using Hurriyet.Web.Models;

namespace Hurriyet.Web.Controllers
{
    public class HomeController : Controller
    {
        int pageSize = 10;


        public ActionResult Index(int currentPage = 0)
        {

            var tNews =  GetNewsList(currentPage);
            var tCount = GetListCount();
            tNews.Wait();
            tCount.Wait();
            int listCount = tCount.Result;
            News[] products = tNews.Result;
            

            NewsListViewModel viewModel = new NewsListViewModel();
            viewModel.News = products;
            viewModel.CurrentPageIndex = currentPage;
            viewModel.LastPage = (currentPage * pageSize) >= listCount - pageSize;

            return View(viewModel);
        }

        async Task<int> GetListCount()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUri"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response = await client.GetAsync("api/product/listcount").ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    int count = await response.Content.ReadAsAsync<int>();
                    return count;
                }
                return 0;
            }
        }
        async Task<News[]> GetNewsList(int currentPage)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUri"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // New code:
                HttpResponseMessage response = await client.GetAsync(string.Format("api/product/list/{0}/{1}",currentPage * pageSize,pageSize)).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    News[] products = await response.Content.ReadAsAsync<News[]> ();
                    return products;
                }
                return null;
            }
        }

    }
}