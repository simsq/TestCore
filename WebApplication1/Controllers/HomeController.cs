using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private UserDbContext _userDbContext;

        public HomeController(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }


        public void Add1()
        {
            _userDbContext.Blogs.Add(new Blog
            {
                Info = "日志-事物"

            });
            _userDbContext.SaveChanges();
        }
        public void Add2()
        {
            _userDbContext.Products.Add(new Product
            {
                CategoryId = 1,
                Info = "产品-事物",
                Name = "名称-事物"
            });
            _userDbContext.SaveChanges();
        }


        public string Add()
        {
            //跨上下文事物
            using (var context = _userDbContext.Database.BeginTransaction())
            {
                Add1();
                Add2();
                context.Commit();
            }


            //简单事物
            using (var context = _userDbContext.Database.BeginTransaction())
            {
                var blog = _userDbContext.Blogs.Find(1);
                blog.Info = "111111111";

                var product = _userDbContext.Blogs.Find(0);

                _userDbContext.SaveChanges();
                context.Commit();
            }

            //_userDbContext.Blogs.Add(new Blog
            //{
            //    Info = "日志",
            //});


            //_userDbContext.Categories.Add(new Category
            //{
            //    Info = "分类",
            //    Name = "名称",
            //    ProductsList = new List<Product>
            //    {
            //         new Product
            //         {
            //             Info="产品信息",
            //             Name="产品名称产品名称产品名称产品名称产品名称产品名称产品名称产品名称产品名称产品名称产品名称产品名称产品名称产品名称产品名称产品名称产品名称产品名称产品名称产品名称产品名称产品名称产品名称"
            //         }
            //    }
            //});
            //var info = _userDbContext.SaveChanges();

            return JsonConvert.SerializeObject("ok");
        }


        public IActionResult Index()
        {

            return View();
        }

    }
}
