﻿using RepoWebShop.Models;
using System.Collections.Generic;
using System.Linq;

namespace RepoWebShop.Interfaces
{
    public interface IShoppingCartRepository
    {
        ShoppingCartComment GetComments(string bookingId);
        string ClearComments(string bookingId);
        IQueryable<ShoppingCartItem> GetItems(string bookingId);     
        IQueryable<ShoppingCartItem> EmptyItems(string bookingId);
        DeliveryAddress GetDelivery(string bookingId);

        string GetShoppingCartComments();
        int GetShoppingCartPreparationTime();
        void AddComments(string comments);
        void ClearFromCart(int pieId);
        void AddToCart(Pie pie, int amount);
        int RemoveFromCart(Pie pie);
        List<ShoppingCartItem> GetShoppingCartItems();
        void ClearCart();
        decimal GetShoppingCartTotal();
        string GetShoppingCartId();
        DeliveryAddress GetShoppingCartDeliveryAddress();
        void RenewId();
        void RemoveDelivery();
        string GetMpPreference();
        void SetMpPreference(string preferenceId);
    }
}
