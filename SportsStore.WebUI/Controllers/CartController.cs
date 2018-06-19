using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;
        private IOrderProcessor orderProcessor;

        public CartController(IProductRepository repo, IOrderProcessor proc)
        {
            repository = repo;
            orderProcessor = proc;
        }


        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToRouteResult AddToCart(Cart cart, int productId, string returnUrl)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }


        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId, string returnUrl)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        [HttpPost]
        public ActionResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Koszyk jest pusty!");
            }

            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                var total = cart.ComputeTotalValue();
                cart.Clear();
                return RedirectToAction("ValidateCommand", new { totalPrice = total });
            }
            else
            {
                return View(shippingDetails);
            }
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        public ActionResult ValidateCommand(string totalPrice)
        {
            bool useSandbox = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSandbox"]);
            var paypal = new PayPalModel();
            var products = new List<string>();
            products.Add("product1");
            products.Add("product2");
            products.Add("product3");
            products.Add("product4");

            paypal.cmd = "_xclick";
            paypal.business = ConfigurationManager.AppSettings["business"];

            if (useSandbox)
            {
                ViewBag.actionURL = ConfigurationManager.AppSettings["test_url"];
            }
            else
            {
                ViewBag.actionURL = ConfigurationManager.AppSettings["Prod_url"];
            }


            paypal.cancel_return = ConfigurationManager.AppSettings["cancel_return"];
            paypal.returnURL = ConfigurationManager.AppSettings["returnURL"];
            paypal.notify_url = ConfigurationManager.AppSettings["notify_url"];
            paypal.currency_code = ConfigurationManager.AppSettings["currency_code"];


            paypal.item_name = "Zamówienie ze strony SportStore";
            paypal.amount = totalPrice;
            return View(paypal);
        }

        public ActionResult Completed()
        {
            return View();
        }

    }
        
    }
