﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RepoWebShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using RepoWebShop.ViewModels;
using AutoMapper;
using RepoWebShop.Interfaces;
using Microsoft.EntityFrameworkCore;
using RepoWebShop.Extensions;

namespace RepoWebShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ShoppingCart _shoppingCart;
        private readonly IPieDetailRepository _pieDetailRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailRepository _emailRespository;
        private readonly IMapper _mp;
        private readonly ICalendarRepository _calendarRepository;

        public OrderController(ICalendarRepository calendarRepository, IMapper mp, IEmailRepository emailRespository, IOrderRepository orderRepository, IPieDetailRepository pieDetailRepository, ShoppingCart shoppingCart, UserManager<ApplicationUser> userManager)
        {
            _pieDetailRepository = pieDetailRepository;
            _orderRepository = orderRepository;
            _shoppingCart = shoppingCart;
            _userManager = userManager;
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
            return View("Pending", id);
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
        public IActionResult Checkout()
        {
            if (_shoppingCart.GetShoppingCartItems().Count() > 0)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public IActionResult Checkout(Order order)
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            if (_shoppingCart.ShoppingCartItems.Count == 0)
            {
                ModelState.AddModelError("", "Tu carrito no puede estar vacío, agrega algunos productos.");
            }

            if(!_shoppingCart.IsPhoneValidated())
            {
                ModelState.AddModelError("", "El número de teléfono no esta verificado.");
            }

            if (ModelState.IsValid)
            {
                order.OrderTotal = _shoppingCart.ShoppingCartItems.Select(x => x.Amount * x.Pie.Price).Sum();
                order.Registration = GetCurrentUser();
                order.CustomerComments = _shoppingCart.GetShoppingCartComments();
                order.BookingId = _shoppingCart.ShoppingCartId;
                order.Status = "reservation";
                order.PickUpTime = _calendarRepository.GetPickupEstimate(_shoppingCart.GetShoppingCartPreparationTime());

                _orderRepository.CreateOrder(order);
                _shoppingCart.ClearCart();


                _emailRespository.Send(order, Request.HostUrl(), null);

                return Redirect($"/Order/Status/{order.FriendlyBookingId}");
            }
            return View(order);
        }
        
        [Authorize]
        public IActionResult CheckoutComplete()
        {
            var firstName = GetCurrentUser()?.FirstName ?? "Estimado/a";
            ViewBag.CheckoutCompleteMessage = firstName + ", gracias por tu reserva. Falta poco para que disfrutes de nuestras delicias!";

            return View();
        }

        private ApplicationUser GetCurrentUser()
        {
            return _userManager.Users.FirstOrDefault(x => x.NormalizedUserName.ToLower() == HttpContext.User.Identity.Name.ToLower());
        }
    }
}
