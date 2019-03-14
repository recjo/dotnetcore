using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using ecom.Models;
using ecom.Entities;
using ecom.Repositories;

namespace ecom.Services
{
    public class CheckoutService : ICheckoutService
    {
        private ICustomerRepository _customerRepository;
        private IShippingRepository _shippingRepository;
        private IOrderRepository _orderRepository;
        private IShoppingCartService _shoppingCartService;
        private IHttpContextAccessor _httpContextAccessor;
        private StoreDbContext _storeDB;

        public CheckoutService(ICustomerRepository customerRepository, IShippingRepository shippingRepository, IOrderRepository orderRepository, IShoppingCartService shoppingCartService, IHttpContextAccessor httpContextAccessor)
        {
            _customerRepository = customerRepository;
            _shippingRepository = shippingRepository;
            _orderRepository = orderRepository;
            _shoppingCartService = shoppingCartService;
            _httpContextAccessor = httpContextAccessor;
            _storeDB = new StoreDbContext();
        }

        public Customer GetCustomer()
        {
            //if user logged in, fetch their address
            var context = _httpContextAccessor.HttpContext;
            var uname = (string.IsNullOrWhiteSpace(context.User.Identity.Name) ? _shoppingCartService.GetCartId() : context.User.Identity.Name);


            //TODO delete
            uname = "joelr";


            var customerEntity = _customerRepository.GetCustomer(uname);
            if (customerEntity != null)
            {
                var customer = new Customer()
                {
                    Id = customerEntity.CustomerId,
                    Uname = customerEntity.Uname,
                    Email = customerEntity.Email,
                    Phone = customerEntity.Phone,
                    PostDate = customerEntity.PostDate,
                    BillingAddress = new Address() {
                        FirstName = customerEntity.FirstName,
                        LastName = customerEntity.LastName,
                        Address1 = customerEntity.Address1,
                        Address2 = customerEntity.Address2,
                        City = customerEntity.City,
                        State = customerEntity.State,
                        PostalCode = customerEntity.PostalCode,
                        Country = customerEntity.Country,
                    },
                    ShippingAddress = new Address() {
                        FirstName = customerEntity.ShippingFirstName,
                        LastName = customerEntity.ShippingLastName,
                        Address1 = customerEntity.ShippingAddress1,
                        Address2 = customerEntity.ShippingAddress2,
                        City = customerEntity.ShippingCity,
                        State = customerEntity.ShippingState,
                        PostalCode = customerEntity.ShippingPostalCode,
                        Country = customerEntity.ShippingCountry,
                    },
                    isLoggedIn = true,
                    StateMenuItems = getStateItems(),
                    CountryMenuItems = getCountryItems()
                };
                return customer;
            }
            return new Customer()
            {
                Uname = uname,
                StateMenuItems = getStateItems(),
                CountryMenuItems = getCountryItems()
            };
        }

        public void AddCustomer(Customer customer)
        {
            var customerEntity = new Customers()
            {
                CustomerId = customer.Id,
                Uname = customer.Uname,
                Email = customer.Email,
                Phone = customer.Phone,
                PostDate = customer.PostDate,
                FirstName = customer.BillingAddress.FirstName,
                LastName = customer.BillingAddress.LastName,
                Address1 = customer.BillingAddress.Address1,
                Address2 = customer.BillingAddress.Address2,
                City = customer.BillingAddress.City,
                State = customer.BillingAddress.State,
                PostalCode = customer.BillingAddress.PostalCode,
                Country = customer.BillingAddress.Country,
                ShippingFirstName = customer.ShippingAddress.FirstName,
                ShippingLastName = customer.ShippingAddress.LastName,
                ShippingAddress1 = customer.ShippingAddress.Address1,
                ShippingAddress2 = customer.ShippingAddress.Address2,
                ShippingCity = customer.ShippingAddress.City,
                ShippingState = customer.ShippingAddress.State,
                ShippingPostalCode = customer.ShippingAddress.PostalCode,
                ShippingCountry = customer.ShippingAddress.Country,
            };
            _customerRepository.AddCustomer(customerEntity);
        }

        public PaymentForm GetPaymentFormInfo()
        {
            var cart = _shoppingCartService.GetCart();
            var customer = GetCustomer();
            return new PaymentForm()
            {
                BillingAddress = customer.BillingAddress,
                ShippingAddress = customer.ShippingAddress,
                Cart = cart,
                ShipMenuItems = getShippingItems(cart.CartItems.Sum(c => c.Quantity)),
                ExpMonthMenuItems = getMonthItems(),
                ExpYearMenuItems = getYearItems()
            };
        }

        public string ProcessOrder(PaymentForm paymentForm)
        {
            var cart = _shoppingCartService.GetCart();
            var customer = GetCustomer();

            var orderEntity = new Orders()
            {
                Username = customer.Uname,
                OrderDate = DateTime.Now,
                FirstName = customer.BillingAddress.FirstName,
                LastName = customer.BillingAddress.LastName,
                Address1 = customer.BillingAddress.Address1,
                Address2 = (!string.IsNullOrEmpty(customer.BillingAddress.Address2) ? customer.BillingAddress.Address2: string.Empty),
                City = customer.BillingAddress.City,
                State = customer.BillingAddress.State,
                PostalCode = customer.BillingAddress.PostalCode,
                Country = customer.BillingAddress.Country,
                ShippingFirstName = customer.ShippingAddress.FirstName,
                ShippingLastName = customer.ShippingAddress.LastName,
                ShippingAddress1 = customer.ShippingAddress.Address1,
                ShippingAddress2 = (!string.IsNullOrEmpty(customer.ShippingAddress.Address2) ? customer.ShippingAddress.Address2: string.Empty),
                ShippingCity = customer.ShippingAddress.City,
                ShippingState = customer.ShippingAddress.State,
                ShippingPostalCode = customer.ShippingAddress.PostalCode,
                ShippingCountry = customer.ShippingAddress.Country,
                Phone = customer.Phone,
                Email = customer.Email,
                ShipVia = paymentForm.Cart.ShipVia,
                Last4 = paymentForm.Payment.CreditCardNumber.Substring(12, 4),
                ExpMo = Convert.ToInt32(paymentForm.Payment.ExpMonth),
                ExpYear = Convert.ToInt32(paymentForm.Payment.ExpYear),
                Ccv = Convert.ToInt32(paymentForm.Payment.SecurityCode),
                SubTotal = paymentForm.Cart.CartTotal,
                Tax = paymentForm.Cart.Tax,
                Shipping = paymentForm.Cart.ShipCost,
                Total = Decimal.Round(paymentForm.Cart.CartTotal + paymentForm.Cart.Tax + paymentForm.Cart.ShipCost, 3),
                OrderStatus = "CC Authorized",
                PaymentDate = DateTime.Now,
                ShippingDate = DateTime.Parse("1/1/1901 12:00:00 AM"),
                PaymentType = "Credit Card",
                OrderNumber = DateTime.Today.Year.ToString() + (DateTime.Today.Month.ToString().Length == 1 ? "0" + DateTime.Today.Month.ToString() : DateTime.Today.Month.ToString()) + (DateTime.Today.Day.ToString().Length == 1 ? "0" + DateTime.Today.Day.ToString() : DateTime.Today.Day.ToString()) + new Random().Next(999999).ToString(),
            };

            _orderRepository.AddOrder(orderEntity);

            foreach(var cartItem in cart.CartItems)
            {
                var orderDetailsEntity = new OrderDetails()
                {
                    OrderNumber = orderEntity.OrderNumber,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.Price,
                    ProductName = cartItem.ProductName,
                    SizeName = cartItem.SizeName,
                    ColorName = cartItem.ColorName,
                    Sku = cartItem.Sku
                };
                _orderRepository.AddOrderDetail(orderDetailsEntity);
            }

            //delete shopping cart
            _shoppingCartService.DeleteCart();

            return orderEntity.OrderNumber;
        }

        public Order GetOrder(string id)
        {
            var orderEntity = _orderRepository.GetOrder(id);
            var orderDetailsEntity = _orderRepository.GetOrderItems(id);
            return new Order() {
                Id = orderEntity.OrderId,
                Username = orderEntity.Username,
                OrderDate = orderEntity.OrderDate,
                BillingFirstName = orderEntity.FirstName,
                BillingLastName = orderEntity.LastName,
                BillingAddress1 = orderEntity.Address1,
                BillingAddress2 = orderEntity.Address2,
                BillingCity = orderEntity.City,
                BillingState = orderEntity.State,
                BillingPostalCode = orderEntity.PostalCode,
                BillingCountry = orderEntity.Country,
                ShippingFirstName = orderEntity.ShippingFirstName,
                ShippingLastName = orderEntity.ShippingLastName,
                ShippingAddress1 = orderEntity.ShippingAddress1,
                ShippingAddress2 = orderEntity.ShippingAddress2,
                ShippingCity = orderEntity.ShippingCity,
                ShippingState = orderEntity.ShippingState,
                ShippingPostalCode = orderEntity.ShippingPostalCode,
                ShippingCountry = orderEntity.ShippingCountry,
                Phone = orderEntity.Phone,
                Email = orderEntity.Email,
                ShipVia = orderEntity.ShipVia,
                Last4 = orderEntity.Last4,
                ExpMo = orderEntity.ExpMo,
                ExpYear = orderEntity.ExpYear,
                Ccv = orderEntity.Ccv,
                SubTotal = orderEntity.SubTotal,
                Tax = orderEntity.Tax,
                Shipping = orderEntity.Shipping,
                Total = orderEntity.Total,
                OrderStatus = orderEntity.OrderStatus,
                PaymentDate = orderEntity.PaymentDate,
                ShippingDate = orderEntity.ShippingDate,
                PaymentType = orderEntity.PaymentType,
                OrderNumber = orderEntity.OrderNumber,
                OrderItems = orderDetailsEntity.Select(od => 
                    new OrderItem()
                    {
                        Id = od.OrderDetailId,
                        ProductId = od.ProductId,
                        Quantity = od.Quantity,
                        Price = od.UnitPrice,
                        ProductName = od.ProductName,
                        SizeName = od.SizeName,
                        ColorName = od.ColorName,
                        Sku = od.Sku
                    }).ToList()
            };
        }

        protected List<SelectListItem> getShippingItems(int items)
        {
            var shipEntities = _shippingRepository.GetShipChargesPerCarrier(items);
            var list = new List<SelectListItem>();
            foreach (var item in shipEntities)
            {
                list.Add( new SelectListItem
                {
                    Value = item.Charge.ToString(),
                    Text = item.CarrierName
                });
            }
            return list;
        }

        protected List<SelectListItem> getStateItems()
        {
            var mItems = new List<SelectListItem>();
            mItems.Add(new SelectListItem { Value = "", Text = "", Selected = true });
            mItems.Add(new SelectListItem { Value = "AL", Text = "Alabama", Selected = false });
            mItems.Add(new SelectListItem { Value = "AK", Text = "Alaska", Selected = false });
            mItems.Add(new SelectListItem { Value = "AZ", Text = "Arizona", Selected = false });
            mItems.Add(new SelectListItem { Value = "AR", Text = "Arkansas", Selected = false });
            mItems.Add(new SelectListItem { Value = "CA", Text = "California", Selected = false });
            mItems.Add(new SelectListItem { Value = "CO", Text = "Colorado", Selected = false });
            mItems.Add(new SelectListItem { Value = "CT", Text = "Connecticut", Selected = false });
            mItems.Add(new SelectListItem { Value = "DE", Text = "Delaware", Selected = false });
            mItems.Add(new SelectListItem { Value = "DC", Text = "District of Columbia", Selected = false });
            mItems.Add(new SelectListItem { Value = "FL", Text = "Florida", Selected = false });
            mItems.Add(new SelectListItem { Value = "GA", Text = "Georgia", Selected = false });
            mItems.Add(new SelectListItem { Value = "HI", Text = "Hawaii", Selected = false });
            mItems.Add(new SelectListItem { Value = "ID", Text = "Idaho", Selected = false });
            mItems.Add(new SelectListItem { Value = "IL", Text = "Illinois", Selected = false });
            mItems.Add(new SelectListItem { Value = "IN", Text = "Indiana", Selected = false });
            mItems.Add(new SelectListItem { Value = "IA", Text = "Iowa", Selected = false });
            mItems.Add(new SelectListItem { Value = "KS", Text = "Kansas", Selected = false });
            mItems.Add(new SelectListItem { Value = "KY", Text = "Kentucky", Selected = false });
            mItems.Add(new SelectListItem { Value = "LA", Text = "Louisiana", Selected = false });
            mItems.Add(new SelectListItem { Value = "ME", Text = "Maine", Selected = false });
            mItems.Add(new SelectListItem { Value = "MD", Text = "Maryland", Selected = false });
            mItems.Add(new SelectListItem { Value = "MA", Text = "Massachusetts", Selected = false });
            mItems.Add(new SelectListItem { Value = "MI", Text = "Michigan", Selected = false });
            mItems.Add(new SelectListItem { Value = "MN", Text = "Minnesota", Selected = false });
            mItems.Add(new SelectListItem { Value = "MS", Text = "Mississippi", Selected = false });
            mItems.Add(new SelectListItem { Value = "MO", Text = "Missouri", Selected = false });
            mItems.Add(new SelectListItem { Value = "MT", Text = "Montana", Selected = false });
            mItems.Add(new SelectListItem { Value = "NE", Text = "Nebraska", Selected = false });
            mItems.Add(new SelectListItem { Value = "NV", Text = "Nevada", Selected = false });
            mItems.Add(new SelectListItem { Value = "NH", Text = "New Hampshire", Selected = false });
            mItems.Add(new SelectListItem { Value = "NJ", Text = "New Jersey", Selected = false });
            mItems.Add(new SelectListItem { Value = "NM", Text = "New Mexico", Selected = false });
            mItems.Add(new SelectListItem { Value = "NY", Text = "New York", Selected = false });
            mItems.Add(new SelectListItem { Value = "NC", Text = "North Carolina", Selected = false });
            mItems.Add(new SelectListItem { Value = "ND", Text = "North Dakota", Selected = false });
            mItems.Add(new SelectListItem { Value = "OH", Text = "Ohio", Selected = false });
            mItems.Add(new SelectListItem { Value = "OK", Text = "Oklahoma", Selected = false });
            mItems.Add(new SelectListItem { Value = "OR", Text = "Oregon", Selected = false });
            mItems.Add(new SelectListItem { Value = "PA", Text = "Pennsylvania", Selected = false });
            mItems.Add(new SelectListItem { Value = "RI", Text = "Rhode Island", Selected = false });
            mItems.Add(new SelectListItem { Value = "SC", Text = "South Carolina", Selected = false });
            mItems.Add(new SelectListItem { Value = "SD", Text = "South Dakota", Selected = false });
            mItems.Add(new SelectListItem { Value = "TN", Text = "Tennessee", Selected = false });
            mItems.Add(new SelectListItem { Value = "TX", Text = "Texas", Selected = false });
            mItems.Add(new SelectListItem { Value = "UT", Text = "Utah", Selected = false });
            mItems.Add(new SelectListItem { Value = "VT", Text = "Vermont", Selected = false });
            mItems.Add(new SelectListItem { Value = "VA", Text = "Virginia", Selected = false });
            mItems.Add(new SelectListItem { Value = "WA", Text = "Washington", Selected = false });
            mItems.Add(new SelectListItem { Value = "WV", Text = "West Virginia", Selected = false });
            mItems.Add(new SelectListItem { Value = "WI", Text = "Wisconsin", Selected = false });
            mItems.Add(new SelectListItem { Value = "WY", Text = "Wyoming", Selected = false });
            //mItems.Add(new SelectListItem { Value = "AE", Text = "Armed Forces America", Selected = false });
            return mItems;
        }

        protected List<SelectListItem> getCountryItems()
        {
            var mItems = new List<SelectListItem>();
            mItems.Add(new SelectListItem { Value = "", Text = "", Selected = true });
            mItems.Add(new SelectListItem { Value = "USA", Text = "United States", Selected = true });
            return mItems;
        }

        private List<SelectListItem> getMonthItems()
        {
                List<SelectListItem> MonthItems = new List<SelectListItem>();
                MonthItems.Add(new SelectListItem { Value = "01", Text = "Jan", Selected = true });
                MonthItems.Add(new SelectListItem { Value = "02", Text = "Feb", Selected = false });
                MonthItems.Add(new SelectListItem { Value = "03", Text = "Mar", Selected = false });
                MonthItems.Add(new SelectListItem { Value = "04", Text = "Apr", Selected = false });
                MonthItems.Add(new SelectListItem { Value = "05", Text = "May", Selected = false });
                MonthItems.Add(new SelectListItem { Value = "06", Text = "Jun", Selected = false });
                MonthItems.Add(new SelectListItem { Value = "07", Text = "Jul", Selected = false });
                MonthItems.Add(new SelectListItem { Value = "08", Text = "Aug", Selected = false });
                MonthItems.Add(new SelectListItem { Value = "09", Text = "Sep", Selected = false });
                MonthItems.Add(new SelectListItem { Value = "10", Text = "Oct", Selected = false });
                MonthItems.Add(new SelectListItem { Value = "11", Text = "Nov", Selected = false });
                MonthItems.Add(new SelectListItem { Value = "12", Text = "Dec", Selected = false });
                return MonthItems;
        }

        private List<SelectListItem> getYearItems()
        {
            List<SelectListItem> YearItems = new List<SelectListItem>();
            YearItems.Add(new SelectListItem { Value = "2019", Text = "2019", Selected = false });
            YearItems.Add(new SelectListItem { Value = "2020", Text = "2020", Selected = false });
            YearItems.Add(new SelectListItem { Value = "2021", Text = "2021", Selected = false });
            YearItems.Add(new SelectListItem { Value = "2022", Text = "2022", Selected = false });
            YearItems.Add(new SelectListItem { Value = "2023", Text = "2023", Selected = false });
            YearItems.Add(new SelectListItem { Value = "2024", Text = "2024", Selected = false });
            YearItems.Add(new SelectListItem { Value = "2025", Text = "2025", Selected = false });
            YearItems.Add(new SelectListItem { Value = "2026", Text = "2026", Selected = false });
            YearItems.Add(new SelectListItem { Value = "2027", Text = "2027", Selected = false });
            YearItems.Add(new SelectListItem { Value = "2028", Text = "2028", Selected = false });
            YearItems.Add(new SelectListItem { Value = "2029", Text = "2029", Selected = false });
            return YearItems;
        }

        /*public void ValidatePaymentForm(PaymentForm paymentForm, ModelStateDictionary ModelState)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("ErrorMessage", "Model is not Valid");
            }
        }*/
    }
}