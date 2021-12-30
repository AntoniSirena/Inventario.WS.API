﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JS.Base.WS.API.DTO.Response.Domain.FreeMarket
{
    public class Article
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string CurrencyCode { get; set; }
        public string Condition { get; set; }
        public string ConditionShortName { get; set; }
        public string Ubication { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public int QuantitySold { get; set; }
        public string CreationDate { get; set; }

        public string ProductType { get; set; }
        public bool UseStock { get; set; }
        public decimal Stock { get; set; }
        public decimal MinQuantity { get; set; }
        public decimal MaxQuantity { get; set; }

        public List<ImgDetail> ImgDetail { get; set; }
    }

    public class ArticleData
    {
        public List<Article> Article { get; set; }
        public long TotalRecord { get; set; }
        public long TotalRecordByPage { get; set; }
        public int PageNumber { get; set; }
    }

    public class Seller
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Mail { get; set; }
    }

    public class ArticleFullData
    {
        public Article Article { get; set; }
        public Seller Seller { get; set; }
    }

    public class ImgDetail
    {
        public long Id { get; set; }
    }
}