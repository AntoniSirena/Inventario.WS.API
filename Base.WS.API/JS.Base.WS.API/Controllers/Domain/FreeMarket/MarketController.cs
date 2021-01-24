﻿using JS.Base.WS.API.Base;
using JS.Base.WS.API.DBContext;
using JS.Base.WS.API.DTO.Response.Domain.FreeMarket;
using JS.Base.WS.API.Helpers;
using JS.Base.WS.API.Models.Domain;
using JS.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static JS.Base.WS.API.Global.Constants;

namespace JS.Base.WS.API.Controllers.Domain.FreeMarket
{

    [RoutePrefix("api/market")]
    [Authorize]
    public class MarketController : ApiController
    {

        private MyDBcontext db;
        private Response response;

        private long currentUserId = CurrentUser.GetId();

        public MarketController()
        {
            db = new MyDBcontext();
            response = new Response();
        }


        [HttpGet]
        [Route("GetById")]
        public MarketDTO GetById(long id)
        {
            var result = db.Markets.Where(y => y.Id == id && y.IsActive == true).Select(x => new MarketDTO()
            {
                Id = x.Id,
                Title = x.Title,
                Price = x.Price,
                CurrencyId = x.CurrencyId,
                Currency = x.Currency.ISO_Code,
                MarketTypeId = x.MarketTypeId,
                MarketType = x.MarketType.Description,
                ConditionId = x.ConditionId,
                Condition = x.ArticleCondition.Description,
                CategoryId = x.CategoryId,
                Category = x.Category.Description,
                SubCategoryId = x.SubCategoryId,
                SubCategory = x.SubCategory.Description,
                Ubication = x.Ubication,
                PhoneNumber = x.PhoneNumber,
                Img = x.Img,
                ImgPath = x.ImgPath,
                ContenTypeShort = x.ContenTypeShort,
                ContenTypeLong = x.ContenTypeLong,
                CreationDate = x.CreationDate,
                CreationTime = x.CreationTime,
                CreatorUserId = x.CreatorUserId,
                LastModificationTime = x.LastModificationTime,
                LastModifierUserId = x.LastModifierUserId,
                DeletionTime = x.DeletionTime,
                DeleterUserId = x.DeleterUserId,
                IsActive = x.IsActive,
                IsDeleted = x.IsDeleted,

            }).FirstOrDefault();

            result.Img = string.Concat(result.ContenTypeLong, ',', Utilities.JS_File.GetStrigBase64(result.ImgPath));

            return result;
        }


        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<MarketDTO> GetAll()
        {
            var result = new List<MarketDTO>();

            var userRole = db.UserRoles.Where(x => x.UserId == currentUserId).FirstOrDefault();

            string[] allowViewAllMarketsByRoles = ConfigurationParameter.AllowViewAllMarketsByRoles.Split(',');

            if (allowViewAllMarketsByRoles.Contains(userRole.Role.ShortName))
            {
                result = db.Markets.Where(y => y.IsActive == true).Select(x => new MarketDTO()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Price = x.Price,
                    CurrencyId = x.CurrencyId,
                    Currency = x.Currency.ISO_Code,
                    MarketTypeId = x.MarketTypeId,
                    MarketType = x.MarketType.Description,
                    ConditionId = x.ConditionId,
                    Condition = x.ArticleCondition.Description,
                    CategoryId = x.CategoryId,
                    Category = x.Category.Description,
                    SubCategoryId = x.SubCategoryId,
                    SubCategory = x.SubCategory.Description,
                    Ubication = x.Ubication,
                    PhoneNumber = x.PhoneNumber,
                    Img = x.Img,
                    ImgPath = x.ImgPath,
                    ContenTypeShort = x.ContenTypeShort,
                    ContenTypeLong = x.ContenTypeLong,
                    CreationDate = x.CreationDate,
                    CreationTime = x.CreationTime,
                    CreatorUserId = x.CreatorUserId,
                    LastModificationTime = x.LastModificationTime,
                    LastModifierUserId = x.LastModifierUserId,
                    DeletionTime = x.DeletionTime,
                    DeleterUserId = x.DeleterUserId,
                    IsActive = x.IsActive,
                    IsDeleted = x.IsDeleted,

                }).OrderByDescending(y => y.Id).ToList();

            }
            else
            {
                result = db.Markets.Where(c => c.CreatorUserId == currentUserId && c.IsActive == true).Select(x => new MarketDTO()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Price = x.Price,
                    CurrencyId = x.CurrencyId,
                    Currency = x.Currency.ISO_Code,
                    MarketTypeId = x.MarketTypeId,
                    MarketType = x.MarketType.Description,
                    ConditionId = x.ConditionId,
                    Condition = x.ArticleCondition.Description,
                    CategoryId = x.CategoryId,
                    Category = x.Category.Description,
                    SubCategoryId = x.SubCategoryId,
                    SubCategory = x.SubCategory.Description,
                    Ubication = x.Ubication,
                    PhoneNumber = x.PhoneNumber,
                    Img = x.Img,
                    ImgPath = x.ImgPath,
                    ContenTypeShort = x.ContenTypeShort,
                    ContenTypeLong = x.ContenTypeLong,
                    CreationDate = x.CreationDate,
                    CreationTime = x.CreationTime,
                    CreatorUserId = x.CreatorUserId,
                    LastModificationTime = x.LastModificationTime,
                    LastModifierUserId = x.LastModifierUserId,
                    DeletionTime = x.DeletionTime,
                    DeleterUserId = x.DeleterUserId,
                    IsActive = x.IsActive,
                    IsDeleted = x.IsDeleted,

                }).OrderByDescending(y => y.Id).ToList();

            }

            return result;
        }


        [HttpPost]
        [Route("Create")]
        public IHttpActionResult Create([FromBody] Market request)
        {

            if (string.IsNullOrEmpty(request.Img))
            {
                response.Code = "400";
                response.Message = "Estimado usuario es necesario subir una imagen para la portada de la publicación";

                return Ok(response);
            }


            var fileTypeAlloweds = ConfigurationParameter.ImgTypeAllowed.Split(',');

            string root = ConfigurationParameter.MarketImgDirectory;
            string[] arrayImgBase64 = request.Img.Split(',');
            string imgBase64 = arrayImgBase64[arrayImgBase64.Length - 1];

            string[] splitName1 = arrayImgBase64[0].Split('/');
            string[] splitName2 = splitName1[1].Split(';');
            string contentType = splitName2[0];

            //Validate contentType
            if (!fileTypeAlloweds.Contains(contentType))
            {
                response.Code = InternalResponseCodeError.Code324;
                response.Message = InternalResponseCodeError.Message324;

                return Ok(response);
            }

            request.Img = string.Empty;
            request.ImgPath = string.Empty;
            request.ContenTypeShort = contentType;
            request.ContenTypeLong = arrayImgBase64[0];
            request.CreationDate = DateTime.Now.ToString("dd/MM/yyyy");
            request.CreationTime = DateTime.Now;
            request.CreatorUserId = currentUserId;
            request.IsActive = true;

            var resp = db.Markets.Add(request);
            db.SaveChanges();


            //Save img
            var guid = Guid.NewGuid();
            var fileName = string.Concat("Market_img_", guid);
            var filePath = Path.Combine(root, fileName) + "." + contentType;

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            File.WriteAllBytes(filePath, Convert.FromBase64String(imgBase64));

            //update Novelty
            resp.ImgPath = filePath;
            db.SaveChanges();

            response.Data = new { Id = resp.Id };
            response.Message = "Artículo creado con éxito";

            return Ok(response);
        }


        [HttpPut]
        [Route("Update")]
        public IHttpActionResult Update([FromBody] Market request)
        {

            if (string.IsNullOrEmpty(request.Img))
            {
                response.Code = "400";
                response.Message = "Estimado usuario es necesario subir una imagen para la portada de la publicación";

                return Ok(response);
            }

            var fileTypeAlloweds = ConfigurationParameter.ImgTypeAllowed.Split(',');

            string root = string.Empty;
            string fileName = string.Empty;
            string filePath = string.Empty;

            string[] arrayImgBase64 = request.Img.Split(',');
            string imgBase64 = arrayImgBase64[arrayImgBase64.Length - 1];

            string[] splitName1 = arrayImgBase64[0].Split('/');
            string[] splitName2 = splitName1[1].Split(';');
            string contentType = splitName2[0];

            //Validate contentType
            if (!fileTypeAlloweds.Contains(contentType))
            {
                response.Code = InternalResponseCodeError.Code324;
                response.Message = InternalResponseCodeError.Message324;

                return Ok(response);
            }

            request.LastModifierUserId = currentUserId;
            request.LastModificationTime = DateTime.Now;
            request.IsActive = true;

            db.Entry(request).State = EntityState.Modified;
            db.SaveChanges();


            //Validate path
            if (string.IsNullOrEmpty(request.ImgPath))
            {
                var guid = Guid.NewGuid();
                root = ConfigurationParameter.MarketImgDirectory;
                fileName = string.Concat("Market_img_", guid);
                filePath = Path.Combine(root, fileName) + "." + contentType;
                request.ImgPath = filePath;

                var market = db.Markets.Where(x => x.Id == request.Id).FirstOrDefault();
                market.ImgPath = filePath;
                db.SaveChanges();
            }

            //Save img
            if (File.Exists(request.ImgPath))
            {
                File.Delete(request.ImgPath);
            }

            File.WriteAllBytes(request.ImgPath, Convert.FromBase64String(imgBase64));

            response.Message = "Artículo actualizado con éxito";

            return Ok(response);
        }


        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult Delete(long Id)
        {
            var result = db.Markets.Where(x => x.Id == Id & x.IsActive == true).FirstOrDefault();

            if (result == null)
            {
                return NotFound();
            }

            result.IsActive = false;
            result.IsDeleted = true;
            result.DeletionTime = DateTime.Now;
            result.DeleterUserId = currentUserId;
            db.SaveChanges();

            response.Message = "Artículo eliminado con éxito";

            return Ok(response);
        }


        [HttpGet]
        [Route("GetArticles")]
        public IEnumerable<Article> GetArticles(string marketTypeShortName, int categoryId, int subCategoryId)
        {
            var result = new List<Article>();

            var marketType = db.MarketTypes.Where(x => x.ShortName == marketTypeShortName).FirstOrDefault();

            if (categoryId > 0 || subCategoryId > 0)
            {
                if (categoryId > 0 && subCategoryId == 0)
                {
                    result = db.Markets.Where(x => x.MarketTypeId == marketType.Id
                        && x.CategoryId == categoryId
                        && x.IsActive == true)
                                .Select(y => new Article()
                                {
                                    Id = y.Id,
                                    Title = y.Title,
                                    Price = y.Price,
                                    CurrencyCode = y.Currency.ISO_Code,
                                    Condition = y.ArticleCondition.Description,
                                    Ubication = y.Ubication,
                                    PhoneNumber = y.PhoneNumber.ToString(),
                                    CreationDate = y.CreationDate,
                                })
                                .OrderByDescending(x => x.Id)
                                .ToList();
                }
                if (subCategoryId > 0 && categoryId == 0)
                {
                    result = db.Markets.Where(x => x.MarketTypeId == marketType.Id
                        && x.SubCategoryId == subCategoryId
                        && x.IsActive == true)
                                .Select(y => new Article()
                                {
                                    Id = y.Id,
                                    Title = y.Title,
                                    Price = y.Price,
                                    CurrencyCode = y.Currency.ISO_Code,
                                    Condition = y.ArticleCondition.Description,
                                    Ubication = y.Ubication,
                                    PhoneNumber = y.PhoneNumber.ToString(),
                                    CreationDate = y.CreationDate,
                                })
                                .OrderByDescending(x => x.Id)
                                .ToList();
                }
                if(categoryId > 0 && subCategoryId > 0)
                {
                    result = db.Markets.Where(x => x.MarketTypeId == marketType.Id
                    && x.CategoryId == categoryId
                    && x.SubCategoryId == subCategoryId
                    && x.IsActive == true)
                            .Select(y => new Article()
                            {
                                Id = y.Id,
                                Title = y.Title,
                                Price = y.Price,
                                CurrencyCode = y.Currency.ISO_Code,
                                Condition = y.ArticleCondition.Description,
                                Ubication = y.Ubication,
                                PhoneNumber = y.PhoneNumber.ToString(),
                                CreationDate = y.CreationDate,
                            })
                            .OrderByDescending(x => x.Id)
                            .ToList();
                }
            }
            else
            {
                result = db.Markets.Where(x => x.MarketTypeId == marketType.Id && x.IsActive == true)
                        .Select(y => new Article()
                        {
                            Id = y.Id,
                            Title = y.Title,
                            Price = y.Price,
                            CurrencyCode = y.Currency.ISO_Code,
                            Condition = y.ArticleCondition.Description,
                            Ubication = y.Ubication,
                            PhoneNumber = y.PhoneNumber.ToString(),
                            CreationDate = y.CreationDate,
                        })
                        .OrderByDescending(x => x.Id)
                        .Take(200)
                        .ToList();
            }

            return result;
        }



        [HttpGet]
        [Route("GetImageByArticleId")]
        [AllowAnonymous]
        public IHttpActionResult GetImageByArticleId(long id, int width, int height)
        {
            var article = db.Markets.Where(x => x.Id == id).FirstOrDefault();

            byte[] file = JS_File.GetImgBytes(article.ImgPath);

            if (width > 0 || height > 0)
            {
                MemoryStream memstr = new MemoryStream(file);
                Image img = Image.FromStream(memstr);

                file = JS_File.ResizeImage(img, width, height);
            }

            JS_File.DownloadFileImg(file);

            return Ok();
        }


        //Catalogue definition
        #region Catalogue definition

        [HttpGet]
        [Route("GetCurrencies")]
        public IEnumerable<CurrencyDTO> GetCurrencies()
        {
            var result = db.Currencies.Where(y => y.IsActive == true).Select(x => new CurrencyDTO()
            {
                Id = x.Id,
                ShortName = x.ISO_Code,
                Description = x.ISO_Currency,
            }).OrderBy(x => x.Description).ToList();

            return result;
        }


        [HttpGet]
        [Route("GetMarketTypes")]
        public IEnumerable<MarketTypeDTO> GetMarketTypes()
        {
            var result = db.MarketTypes.Where(y => y.IsActive == true).Select(x => new MarketTypeDTO()
            {
                Id = x.Id,
                ShortName = x.ShortName,
                Description = x.Description,
            }).OrderBy(x => x.Description).ToList();

            return result;
        }

        [HttpGet]
        [Route("GetConditions")]
        public IEnumerable<ConditionDTO> GetConditions()
        {
            var result = db.ArticleConditions.Where(y => y.IsActive == true).Select(x => new ConditionDTO()
            {
                Id = x.Id,
                ShortName = x.ShortName,
                Description = x.Description,
            }).OrderBy(x => x.Description).ToList();

            return result;
        }


        [HttpGet]
        [Route("GetCategories")]
        public IEnumerable<CategoryDTO> GetCategories()
        {
            var result = db.ArticleCategories.Where(y => y.IsActive == true).Select(x => new CategoryDTO()
            {
                Id = x.Id,
                ShortName = x.ShortName,
                Description = x.Description,
            }).OrderBy(x => x.Description).ToList();

            return result;
        }


        [HttpGet]
        [Route("GetSubCategories")]
        public IEnumerable<SubCategoryDTO> GetSubCategories(int categoryId)
        {
            var result = db.ArticleSubCategories.Where(y => y.CategoryId == categoryId && y.IsActive == true).Select(x => new SubCategoryDTO()
            {
                Id = x.Id,
                ShortName = x.ShortName,
                Description = x.Description,
            }).OrderBy(x => x.Description).ToList();

            return result;
        }

        #endregion
    }
}
