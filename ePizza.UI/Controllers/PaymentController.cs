using ePizza.UI.Models;
using ePizza.UI.Models.Requests;
using ePizza.UI.Models.Responses;
using ePizza.UI.Models.ViewModels;
using ePizza.UI.RazorPay;
using ePizza.UI.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Transactions;

namespace ePizza.UI.Controllers
{
    public class PaymentController : BaseController
    {

        private readonly RazorPayConfigurationModel _configuration;
        private readonly IRazorPayService _razorPayService;
        private readonly IHttpClientFactory _httpClientFactory;


        public PaymentController(IOptions<RazorPayConfigurationModel> configuration,
            IRazorPayService razorPayService, IHttpClientFactory _httpClientFactory)
        {
            this._configuration = configuration.Value;
            this._razorPayService = razorPayService;
            this._httpClientFactory = _httpClientFactory;
        }
        public IActionResult Index()
        {

            PaymentModel payment = new PaymentModel();

            CartItemsResponseModelDto cartDetails = TempData.Peek<CartItemsResponseModelDto>("CartDetails");

            if (cartDetails != null)
            {
                // proceed with Razor pay integration
                //payment.RazorPayKey = _configuration["RazorPay:key"]!;  --this line not recmmonded as its hardcoding the razore key, so use like below line
                payment.RazorPayKey = _configuration.Key;
                payment.Currency = "INR";
                payment.GrantTotal = cartDetails.GrantTotal;
                payment.Description = "Hope you'will enjoy this meal";
                payment.Cart = cartDetails;
                payment.Receipt = Guid.NewGuid().ToString(); // guid will give you unique value every time
                payment.OrderId = _razorPayService.CreateOrder(payment.GrantTotal * 100, payment.Currency, payment.Receipt);

                return View(payment);

            }


            return View();
        }

        //Iformcollection will collet the information from Razor pay 
        public async Task<IActionResult> Status(IFormCollection form)
        {

            /// api is there to populate paymentinfo in my db
            /// 
            string paymentId = form["rzp_paymentid"];
            string orderId = form["rzp_orderid"];
            string signature = form["rzp_signature"];
            string transactionId = form["rzp_Receipt"];
            string currency = form["rzp_Currency"];

            bool issignatureverified = _razorPayService.VerifySignature(signature, orderId, paymentId);

            if (issignatureverified)
            {
                var paymentDetails = _razorPayService.GetPayment(paymentId);
                string status = paymentDetails["status"];

                // call api to store payment info in my db

                var request = GetPaymentRequest(paymentId, orderId, signature, transactionId, currency);


                var jsonRequest = JsonConvert.SerializeObject(request);

                var client = _httpClientFactory.CreateClient(ApplicationConstants.EPizzaClientName);

                var response
                     = await client.PostAsJsonAsync("api/Payment", jsonRequest);

                response.EnsureSuccessStatusCode();

                Response.Cookies.Delete("CartId");
                TempData.Remove("CartDetails");
                TempData.Remove("Address");

                return RedirectToAction("Receipt");


            }

            // display a receipt to end user saying payment completed, we will deliver u shortly

            return RedirectToAction("Receipt");
        }

        public IActionResult Receipt()
        {
            return View();
        }

        private MakePaymentRequestModelDto GetPaymentRequest(
             string paymentId, string orderid, string transactionid, string currency, string status)
        {
            CartItemsResponseModelDto cart = TempData.Peek<CartItemsResponseModelDto>("CartDetails");

            AddressViewModel addressViewModel = TempData.Peek<AddressViewModel>("Address");


            return new MakePaymentRequestModelDto
            {
                CartId = Guid.Parse(Request.Cookies["CartId"])!,
                Total = cart.Total,
                Currency = currency,
                PaymentId = paymentId,
                Status = status,
                TransactionId = transactionid,
                Tax = cart.Tax,
                Email = CurrentUser.Email,
                GrandTotal = cart.GrantTotal,
                UserId = CurrentUser.UserId,
                OrderRequest = new OrderRequestModelDto()
                {
                    City = addressViewModel.City,
                    Locality = addressViewModel.Locality,
                    Street = addressViewModel.Street,
                    UserId = CurrentUser.UserId,
                    OrderId = orderid,
                    PaymentId = paymentId,
                    PhoneNumber = addressViewModel.PhoneNumber,
                    ZipCode = addressViewModel.ZipCode,
                    OrderItems = GetOrderItems(cart.CartItems)
                }
            };
        }

        private List<OrderItemsRequestDto> GetOrderItems(List<ItemsResponseModelDto> items)
        {
            List<OrderItemsRequestDto> orderItems = [];


            // Linq foreach method used here
            items.ForEach(x => orderItems.Add(new OrderItemsRequestDto()
            {
                ItemId = x.ItemId,
                Quantity = x.Quantity,
                Total = x.ItemTotal,
                UnitPrice = x.UnitPrice
            }));


            return orderItems;
        }


    }
}
