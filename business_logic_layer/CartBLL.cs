﻿using System;
using business_logic_layer.ViewModel;
using Data_layer;
using Data_layer.Context;
using Data_layer.Context.Data;

namespace business_logic_layer
{
    public class CartBLL
    {

        private readonly CartDAL _cartDAL;

        public CartBLL()
        {
            _cartDAL = new CartDAL();
        }

        public async Task<CartModel> AddCart(CartModel cart, string sessionId, string userID)
        {
            Guid? userId = null;
            if (!string.IsNullOrWhiteSpace(userID))
                userId = Guid.Parse(userID);

            CartEnityModel cartFormat = new CartEnityModel()
            {
                productId = cart.productId,
                Title = cart.Title,
                Price = cart.Price,
                Kilo = cart.Kilo,
                Description = cart.Description,
                ImageUrl = cart.ImageUrl,
                Quantity = cart.Quantity,
                CategoryName = cart.CategoryName,
                SessionId = sessionId,
                UserId = userId
            };

            await _cartDAL.AddProduct(cartFormat);
            return cart;
        }

        public string GetDefaultUserId()
        {
            return _cartDAL.GetDefaultUserId().ToString();
        }


        public async Task<List<CartModel>> GetCart(string sessionId, string? userID)
        {
            Guid? userId = null;
            if (!string.IsNullOrWhiteSpace(userID))
                userId = Guid.Parse(userID);
            var cartEntities = await _cartDAL.GetCartItems(sessionId, userId);
            return cartEntities.Select(item => new CartModel
            {
                productId = item.productId,
                Title = item.Title,
                Price = item.Price,
                Kilo = item.Kilo,
                Description = item.Description,
                ImageUrl = item.ImageUrl,
                Quantity = item.Quantity,
                CategoryName = item.CategoryName
            }).ToList();
        }

        public async Task ClearCart(string sessionId, string userID)
        {
            Guid? userId = null;
            if (!string.IsNullOrWhiteSpace(userID))
                userId = Guid.Parse(userID);
            await _cartDAL.ClearCart(sessionId, userId);
        }


        public async Task RemoveCart(Guid productId, string userID, string sessionId)
        {
            Guid? userId = null;
            if (!string.IsNullOrWhiteSpace(userID))
                userId = Guid.Parse(userID);

            await _cartDAL.RemoveProduct(productId, userId, sessionId);
        }


        public async Task<CartModel> UpdateCartQuantity(Guid productId, string userID, string sessionId)
        {
            Guid? userId = null;
            if (!string.IsNullOrWhiteSpace(userID))
                userId = Guid.Parse(userID);

            var updatedProduct = await _cartDAL.UpdateProductQuantity(productId, userId, sessionId);
            if (updatedProduct == null)
            {
                return null;
            }

          
            return new CartModel
            {
                productId = updatedProduct.productId,
                Title = updatedProduct.Title,
                Price = updatedProduct.Price,
                Kilo = updatedProduct.Kilo,
                CategoryName = updatedProduct.CategoryName,
                ImageUrl = updatedProduct.ImageUrl,
                Description = updatedProduct.Description,
                Quantity = updatedProduct.Quantity
              
            };
        }



    }
}

