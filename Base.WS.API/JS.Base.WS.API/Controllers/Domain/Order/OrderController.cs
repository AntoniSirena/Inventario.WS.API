﻿using JS.Base.WS.API.Base;
using JS.Base.WS.API.DBContext;
using JS.Base.WS.API.DTO.Request.Domain;
using JS.Base.WS.API.Helpers;
using JS.Base.WS.API.Models.Domain.PurchaseTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JS.Base.WS.API.Controllers.Domain.Order
{
    [RoutePrefix("api/order")]
    [Authorize]
    public class OrderController : ApiController
    {

        private MyDBcontext db;
        private Response response;

        private long currentUserId = CurrentUser.GetId();

        public OrderController()
        {
            db = new MyDBcontext();
            response = new Response();
        }


        [HttpPost]
        [Route("Create")]
        public IHttpActionResult Create([FromBody] CreateOrderDTO request)
        {

            var article = db.Markets.Where(x => x.Id == request.ArticleId).FirstOrDefault();

            #region Validations

            if (request.Quantity < 1)
            {
                response.Code = "400";
                response.Message = "Favor ingrese la cantidad que desea comprar";
                return Ok(response);
            }

            if (request.Quantity < article.MinQuantity)
            {
                response.Code = "400";
                response.Message = string.Concat("La cantidad a comprar debe ser igual ó mayor a ", article.MinQuantity.ToString());
                return Ok(response);
            }

            if (request.Quantity > article.MaxQuantity)
            {
                response.Code = "400";
                response.Message = string.Concat("La cantidad a comprar debe ser menor ó igual a ", article.MaxQuantity.ToString());
                return Ok(response);
            }

            if (article.UseStock)
            {
                if (request.Quantity > article.Stock)
                {
                    response.Code = "400";
                    response.Message = string.Concat("Este producto no tiene la cantidad suficiente para venderle ", request.Quantity.ToString(), ". Existencia actual ", article.Stock.ToString());
                    return Ok(response);
                }
            }

            #endregion


            var currentOrder = db.PurchaseTransactions.Where(x => x.UserId == currentUserId
                                                             && x.TransactionType.ShortName == Global.Constants.PurchaseTransactionTypes.Order
                                                             && x.Status.ShortName == Global.Constants.PurchaseTransactionStatus.InProcess
                                                             ).OrderByDescending(x => x.Id).FirstOrDefault();
            var order = new PurchaseTransaction();
            var detail = new PurchaseTransactionDetail();

            if (currentOrder == null)
            {

                int inProcessId = db.PurchaseTransactionStatus.Where(x => x.ShortName == Global.Constants.PurchaseTransactionStatus.InProcess).Select(x => x.Id).FirstOrDefault();
                int transactionTypeId = db.PurchaseTransactionTypes.Where(x => x.ShortName == Global.Constants.PurchaseTransactionTypes.Order).Select(x => x.Id).FirstOrDefault();

                order.Discount = 0;
                decimal amount = (request.Quantity * article.Price) - order.Discount;

                order.StatusId = inProcessId;
                order.TransactionId = transactionTypeId;
                order.UserId = currentUserId;
                order.CurrencyISONumber = Global.Constants.Currencies.ISONumberRD;
                order.Tax = amount * (decimal)0.18;
                order.Amount = amount - order.Tax;
                order.TotalAmount = order.Amount + order.Tax;
                order.TotalAmountPending = 0;
                order.Comment = null;
                order.CreationTime = DateTime.Now;
                order.CreatorUserId = currentUserId;
                order.IsActive = true;

                db.PurchaseTransactions.Add(order);
                db.SaveChanges();

                decimal taxDetail = (article.Price * request.Quantity) * (decimal)0.18;

                detail.TransactionId = order.Id;
                detail.ArticleId = request.ArticleId;
                detail.Quantity = request.Quantity;
                detail.Tax = taxDetail;
                detail.Price = ((article.Price * request.Quantity) - taxDetail);
                detail.TotalAmount = (detail.Price + taxDetail);
                detail.CreationTime = DateTime.Now;
                detail.CreatorUserId = currentUserId;
                detail.IsActive = true;

                detail = db.PurchaseTransactionDetails.Add(detail);
                db.SaveChanges();

                response.Message = "Artículo agregado con éxito";
            }
            else
            {

                detail = db.PurchaseTransactionDetails.Where(x => x.ArticleId == request.ArticleId && x.TransactionId == currentOrder.Id).FirstOrDefault();

                if (detail == null)
                {
                    detail = new PurchaseTransactionDetail();

                    currentOrder.Discount = currentOrder.Discount + 0;
                    decimal amount = (request.Quantity * article.Price) - currentOrder.Discount;
                    decimal tax = (amount * (decimal)0.18);

                    decimal taxDetail = (article.Price * request.Quantity) * (decimal)0.18;

                    currentOrder.Tax += tax;
                    currentOrder.Amount += (amount - tax);
                    currentOrder.TotalAmount = (currentOrder.Amount + currentOrder.Tax);
                    currentOrder.TotalAmountPending += 0;
                    db.SaveChanges();

                    detail.TransactionId = currentOrder.Id;
                    detail.ArticleId = request.ArticleId;
                    detail.Quantity = request.Quantity;
                    detail.Tax = taxDetail;
                    detail.Price = ((article.Price * request.Quantity) - taxDetail);
                    detail.TotalAmount = (detail.Price + taxDetail);
                    detail.CreationTime = DateTime.Now;
                    detail.CreatorUserId = currentUserId;
                    detail.IsActive = true;

                    detail = db.PurchaseTransactionDetails.Add(detail);
                    db.SaveChanges();

                    response.Message = "Artículo agregado con éxito";
                }
                else
                { 
                    request.Quantity = (request.Quantity - detail.Quantity);

                    if (request.Quantity >= 1)
                    {
                        currentOrder.Discount = currentOrder.Discount + 0;
                        decimal amount = (request.Quantity * article.Price) - currentOrder.Discount;
                        decimal tax = (amount * (decimal)0.18);

                        decimal taxDetail = (article.Price * request.Quantity) * (decimal)0.18;

                        currentOrder.Tax += tax;
                        currentOrder.Amount += (amount - tax);
                        currentOrder.TotalAmount = (currentOrder.Amount + currentOrder.Tax);
                        currentOrder.TotalAmountPending += 0;
                        db.SaveChanges();

                        detail.Quantity += request.Quantity;
                        detail.Tax += taxDetail;
                        detail.Price += ((article.Price * request.Quantity) - taxDetail);
                        detail.TotalAmount = (detail.Price + detail.Tax);
                        db.SaveChanges();
                    }
                    
                    if(request.Quantity < 0)
                    {
                        request.Quantity = Math.Abs(request.Quantity);

                        currentOrder.Discount = currentOrder.Discount + 0;
                        decimal amount = (request.Quantity * article.Price) - currentOrder.Discount;
                        decimal tax = (amount * (decimal)0.18);

                        decimal taxDetail = (article.Price * request.Quantity) * (decimal)0.18;

                        currentOrder.Tax -= tax;
                        currentOrder.Amount -= (amount - tax);
                        currentOrder.TotalAmount = (currentOrder.Amount + currentOrder.Tax);
                        currentOrder.TotalAmountPending -= 0;
                        db.SaveChanges();

                        detail.Quantity -= request.Quantity;
                        detail.Tax -= taxDetail;
                        detail.Price -= ((article.Price * request.Quantity) - taxDetail);
                        detail.TotalAmount = (detail.Price + detail.Tax);
                        db.SaveChanges();
                    }

                    response.Message = "Cantidad actualizada con éxito";
                }

            }


            return Ok(response);
        }


        [HttpGet]
        [Route("GetCurrentArticleQuantity")]
        public IHttpActionResult GetCurrentArticleQuantity(long articleId)
        {
            decimal Quantity = 0;
            bool ShowButtonDeleteItem = false;

            var currentOrder = db.PurchaseTransactions.Where(x => x.UserId == currentUserId
                                                 && x.TransactionType.ShortName == Global.Constants.PurchaseTransactionTypes.Order
                                                 && x.Status.ShortName == Global.Constants.PurchaseTransactionStatus.InProcess
                                                 ).OrderByDescending(x => x.Id).FirstOrDefault();

            if (currentOrder != null)
            {
                var article = currentOrder.ArticlesDetails.Where(x => x.ArticleId == articleId).FirstOrDefault();

                if (article != null)
                {
                    Quantity = article.Quantity;
                    ShowButtonDeleteItem = true;
                }
            }

            return Ok(new { Quantity = Quantity, ShowButtonDeleteItem = ShowButtonDeleteItem });
        }

    }

}
