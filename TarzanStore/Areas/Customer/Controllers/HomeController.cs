using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Tarzan.DataAccess.Repository.IRepository;
using Tarzan.Models;
using Tarzan.Models.ViewModels;
using Tarzan.Utility;

namespace TarzanStore.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        //public IActionResult Index()
        //{
        //    IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,ProductImages");
        //    return View(productList);
        //}
        public IActionResult Index(string search, string author, string category)
        {
            // Get all products with related entities
            var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,ProductImages");

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                productList = productList.Where(p => p.Title.Contains(search)
                                                    || p.Author.Contains(search)
                                                    || p.Category.Name.Contains(search));
            }

            // Apply author filter
            if (!string.IsNullOrEmpty(author))
            {
                productList = productList.Where(p => p.Author.Contains(author));
            }

            // Apply category filter
            if (!string.IsNullOrEmpty(category))
            {
                productList = productList.Where(p => p.Category.Name.Contains(category));
            }

            return View(productList.ToList());
        }
        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new() {
            
                Product = _unitOfWork.Product.Get(u => u.Id== productId, includeProperties: "Category,ProductImages"),
                Count = 1,
                ProductId = productId

            };
            return View(cart);
        }
        [HttpPost]
        [Authorize] // i add this becous any one want to add to the cart must be login 
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            //getting the user info
           var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart                
                .Get(u => u.ApplicationUserId == userId && u.ProductId == shoppingCart.ProductId);
            if(cartFromDb != null)
            {
                //shopping cart exists
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                _unitOfWork.Save();
            }
            else
            {
                //add cart record
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart,
                _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
            }
            TempData["success"] = "Cart updated successfully";
            

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}