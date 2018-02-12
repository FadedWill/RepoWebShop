﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RepoWebShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using RepoWebShop.ViewModels;
using AutoMapper;
using RepoWebShop.Interfaces;
using RepoWebShop.Extensions;
using System.Threading.Tasks;
using RepoWebShop.States;

namespace RepoWebShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCartRepository _shoppingCart;
        private readonly IPieDetailRepository _pieDetailRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IEmailRepository _emailRespository;
        private readonly IMapper _mp;
        private readonly ICalendarRepository _calendarRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public OrderController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ICalendarRepository calendarRepository, IMapper mp, IEmailRepository emailRespository, IOrderRepository orderRepository, IPieDetailRepository pieDetailRepository, IShoppingCartRepository shoppingCart, IAccountRepository accountRepository)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _pieDetailRepository = pieDetailRepository;
            _orderRepository = orderRepository;
            _shoppingCart = shoppingCart;
            _accountRepository = accountRepository;
            _emailRespository = emailRespository;
            _mp = mp;
            _calendarRepository = calendarRepository;
        }

        public IActionResult EmailNotification(int id)
        {
            var viewModel = _orderRepository.GetEmailData(id, Request.HostUrl());
            return View(viewModel);
        }

        public IActionResult Status(string id)
        {
            if (Request.QueryString.HasValue)
                _shoppingCart.RenewId();
            Order order = _orderRepository.GetOrderByBookingId(id);
            OrderStatusViewModel orderstatus;
            if (order == null)
            {
                orderstatus = new OrderStatusViewModel()
                {
                    BookingId = id ?? string.Empty,
                    Notification = null,
                    Payment = null,
                    Progress = null
                };
            }
            else
            { 
                orderstatus = new OrderStatusViewModel()
                {
                    BookingId = id ?? string.Empty,
                    Notification = _orderRepository.GetEmailData(order.OrderId, Request.HostUrl()),
                    Payment = order.OrderPaymentStatus,
                    Progress = order.OrderProgressState
                };
            }
            return View(orderstatus);
        }

        [HttpGet]
        [Route("[Controller]/Pending/{id}")]
        public IActionResult Pending(string id)
        {
            if(Request.QueryString.HasValue)
                _shoppingCart.RenewId();
            return View("Pending", id);
        }

        [HttpGet]
        [Route("[Controller]/OrderComplete/{id}")]
        public IActionResult OrderComplete(int id)
        {
            Order order = _orderRepository.GetOrder(id);
            if (order == null)
                return NotFound();
            else
            {
                EmailNotificationViewModel viewModel = _orderRepository.ToEmailNotification(order, Request.HostUrl());
                return View(viewModel);
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("[Controller]/UpdateOrderWithReason/{subaction}/{id}")]
        public IActionResult UpdateOrderWithReason(string subaction, int id)
        {
            var order = _orderRepository.GetOrder(id);
            var orderViewModel = new UpdateOrderWithReasonViewModel();

            orderViewModel.Order = order;
            orderViewModel.OrderId = id;
            orderViewModel.Action = subaction;

            return View(orderViewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult Cancel(UpdateOrderWithReasonViewModel orderCancelled)
        {
            if (ModelState.IsValid)
            {
                _orderRepository.CancelOrder(orderCancelled.OrderId, orderCancelled.Reason);
                return Redirect("/Order/Cancelled");
            }
            orderCancelled.Order = _orderRepository.GetOrder(orderCancelled.OrderId);
            return View(orderCancelled);
        }


        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult CancelPayment(UpdateOrderWithReasonViewModel orderPaymentCancelled)
        {
            if (ModelState.IsValid)
            {
                _orderRepository.CancelPaymentOrder(orderPaymentCancelled.OrderId, orderPaymentCancelled.Reason);
                return Redirect("/Order/Cancelled");
            }
            orderPaymentCancelled.Order = _orderRepository.GetOrder(orderPaymentCancelled.OrderId);
            return View(orderPaymentCancelled);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult Refund(UpdateOrderWithReasonViewModel orderRefunded)
        {
            if (ModelState.IsValid)
            {
                _orderRepository.RefundOrder(orderRefunded.OrderId, orderRefunded.Reason);
                return Redirect("/Order/Refunded");
            }
            orderRefunded.Order = _orderRepository.GetOrder(orderRefunded.OrderId);
            return View(orderRefunded);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult Return(UpdateOrderWithReasonViewModel orderReturned)
        {
            if (ModelState.IsValid)
            {
                _orderRepository.ReturnOrder(orderReturned.OrderId, orderReturned.Reason);
                return Redirect("/Order/Returned");
            }
            orderReturned.Order = _orderRepository.GetOrder(orderReturned.OrderId);
            return View(orderReturned);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult InProgress()
        {
            return View(_orderRepository.GetOrdersInProgress());
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Cancelled()
        {
            return View(_orderRepository.GetOrdersCancelled());
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Completed()
        {
            return View(_orderRepository.GetOrdersCompleted());
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult PickedUp()
        {
            return View(_orderRepository.GetOrdersPickedUp());
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Returned()
        {
            return View(_orderRepository.GetOrdersReturned());
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Refunded()
        {
            return View(_orderRepository.GetOrdersRefunded());
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult NotYetPaid() 
        {
            return View(_orderRepository.GetOrdersPickedUpWithPendingPayment());
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult AllOrders()
        {
            return View(_orderRepository.GetAll());
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Detail(int id)
        {
            Order order = _orderRepository.GetOrder(id);
            if (order != null)
            {
                var items = _orderRepository.GetOrderDetails(order.OrderId);

                var orderDetails = new OrderDetailsViewModel(order, items);

                return View(orderDetails);
            }
            else
                return NotFound();
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var user = await _userManager.GetUser(_signInManager);
            if ((_shoppingCart.GetShoppingCartItems()).Count() > 0)
                if(user.PhoneNumberConfirmed)
                    return View(user);
                else
                    return Redirect($"/Account/VerifyNumber/Order/Checkout/");
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Checkout(Order order)
        {
            var items = _shoppingCart.GetShoppingCartItems();

            if (items.Count == 0)
            {
                ModelState.AddModelError("EmptyTrolley", "Tu carrito no puede estar vacío, agrega algunos productos.");
            }
            var _currentUser = await _userManager.GetUser(_signInManager);
            if(!_currentUser.PhoneNumberConfirmed)
            {
                ModelState.AddModelError("InvalidPhone", "El número de teléfono no esta verificado.");
                Redirect($"/Account/VerifyNumber/Order/Checkout/");
            }

            order.PhoneNumber = _currentUser.PhoneNumber;
            order.OrderTotal = _shoppingCart.GetShoppingCartTotal();

            if(order.OrderTotal > 500)
            {
                ModelState.AddModelError("MaxValueReached", "El monto de la reserva supera el límite admitido. Deberás usar Mercado Pago.");
            }

            Order anotherReservationInProgress = _orderRepository.LatestReservationInProgress(_currentUser);
            if (anotherReservationInProgress != null)
            {
                var link = $"<a target='_blank' href='{Request.HostUrl()}/Order/Status/{anotherReservationInProgress.FriendlyBookingId}'>{anotherReservationInProgress.FriendlyBookingId}</a>";
                ModelState.AddModelError("ReservationInProgress", $"Deberás retirar el pedido {link} para poder reservar otra vez. Alternativamente, podés comprar con Mercado Pago.");
            }

            if (ModelState.IsValid)
            {
                order.Registration = _currentUser;
                order.CustomerComments = _shoppingCart.GetShoppingCartComments();
                order.BookingId = _shoppingCart.GetShoppingCartId();
                order.Status = "reservation";
                order.PickUpTime = _calendarRepository.GetPickupEstimate(_shoppingCart.GetShoppingCartPreparationTime());
                order.DeliveryAddress = _shoppingCart.GetShoppingCartDeliveryAddress();
                _orderRepository.CreateOrder(order);
                _shoppingCart.ClearCart();
                await _emailRespository.SendOrderConfirmationAsync(order, Request.HostUrl(), null);
                return Redirect($"/Order/Status/{order.FriendlyBookingId}");
            }
            return View(_currentUser);
        }
        
        [Authorize]
        public async Task<IActionResult> CheckoutComplete()
        {
            var _currentUser = await _userManager.GetUser(_signInManager);
            var firstName = _currentUser?.FirstName ?? "Estimado/a";
            ViewBag.CheckoutCompleteMessage = firstName + ", gracias por tu reserva. Falta poco para que disfrutes de nuestras delicias!";

            return View();
        }
    }
}
