﻿using JS.Base.WS.API.Base;
using JS.Base.WS.API.DBContext;
using JS.Base.WS.API.DTO.Common;
using JS.Base.WS.API.DTO.Domain;
using JS.Base.WS.API.Global;
using JS.Base.WS.API.Helpers;
using JS.Base.WS.API.Models.Domain.Inventory;
using JS.Base.WS.API.Services.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace JS.Base.WS.API.Controllers.Domain
{

    [Authorize]
    [RoutePrefix("api/inventory")]
    public class InventoryController : ApiController
    {

        private MyDBcontext db;
        private Response response;
        private PaginationDTO pagination;

        private long currentUserId = CurrentUser.GetId();

        public InventoryController()
        {
            db = new MyDBcontext();
            response = new Response();
            pagination = new PaginationDTO();
        }



        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<InventoryDTO> GetAll()
        {

            var result = new List<InventoryDTO>();

            string[] roles = Global.Constants.ConfigurationParameter.AllowShowAll_Inventory_ByRole.Split(',');

            var userRole = db.UserRoles.Where(x => x.UserId == currentUserId).FirstOrDefault();

            if (roles.Contains(userRole.Role.ShortName))
            {
                result = db.Inventories.Where(x => x.IsActive == true).Select(y => new InventoryDTO()
                {
                    Id = y.Id,
                    Status = y.InventoryStatus.Description,
                    StatuShortName = y.InventoryStatus.ShortName,
                    StatusColour = y.InventoryStatus.Colour,
                    UserName = y.User.UserName,
                    Description = y.Description,
                    OpenDate = y.OpenDateFormatted,
                    ClosedDate = y.ClosedDateFormatted,

                }).OrderByDescending(x => x.Id).ToList();
            }
            else
            {
                result = db.Inventories.Where(x => x.IsActive == true && x.UserId == currentUserId).Select(y => new InventoryDTO()
                {
                    Id = y.Id,
                    Status = y.InventoryStatus.Description,
                    StatuShortName = y.InventoryStatus.ShortName,
                    StatusColour = y.InventoryStatus.Colour,
                    UserName = y.User.UserName,
                    Description = y.Description,
                    OpenDate = y.OpenDateFormatted,
                    ClosedDate = y.ClosedDateFormatted,

                }).OrderByDescending(x => x.Id).ToList();
            }

            foreach (var item in result)
            {
                var details = db.InventoryDetails.Where(x => x.InventoryId == item.Id).ToList();

                if (details.Count > 0)
                {
                    item.TotalAmount = details.Sum(y => y.TotalAmount);
                }
            }


            return result;
        }


        [HttpGet]
        [Route("GetById")]
        public Inventory GetById(long id)
        {
            var result = db.Inventories.Where(x => x.Id == id).FirstOrDefault();

            return result;
        }


        [HttpGet]
        [Route("GetItems")]
        public IHttpActionResult GetItems(string input)
        {

            if (string.IsNullOrEmpty(input))
            {
                response.Code = "400";
                response.Message = "Ingrese un criterio de busqueda válido";

                return Ok(response);
            }

            input = RemoveAccents(input).ToLower();

            var products = db.Products.Where(x => x.ExternalCode.Contains(input)
                                          || x.BarCode.Contains(input)
                                          || x.FormattedDescription.Contains(input)
                                          || x.Reference.Contains(input)).Select(y => new ProductDTO()
                                          {
                                              Id = y.Id,
                                              Description = y.Description,
                                              ExternalCode = y.ExternalCode,
                                              BarCode = y.BarCode,
                                              Cost = y.Cost,
                                              Price = y.Price,
                                              Reference = y.Reference,
                                              Existence = y.Existence,
                                          }).ToList();

            if (products.Count() == 0)
            {
                response.Code = "404";
                response.Message = "Items no encontrados";

                return Ok(response);
            }

            response.Data = products;

            return Ok(response);
        }


        [HttpGet]
        [Route("GetInventoryDetails")]
        public IHttpActionResult GetInventoryDetails(long inventoryId)
        {
            var result = db.InventoryDetails.Where(x => x.InventoryId == inventoryId).Select(y => new ProductDTO()
            {
                Id = y.Product.Id,
                InventoryDetailId = y.Id,
                Description = y.Product.Description,
                ExternalCode = y.Product.ExternalCode,
                BarCode = y.Product.BarCode,
                OldCost = y.OldCost,
                OldPrice = y.OldPrice,
                Cost = y.CurrentCost,
                Price = y.CurrentPrice,
                Reference = y.Product.Reference,
                Existence = y.Product.Existence,
                Quantity = y.Quantity,
                TotalAmount = y.TotalAmount,
                Difference = y.Difference,
                InventoryId = inventoryId,
                UserName = y.User.UserName,

                SectionId = y.SectionId,
                SectionDescription = y.Section.Description,
                TariffId = y.TariffId,
                TariffDescription = y.Tariff.Description,

            }).OrderByDescending(x => x.InventoryDetailId).ToList();

            response.Data = result;

            return Ok(response);
        }


        [HttpGet]
        [Route("GetInventoryDetails_Paginated")]
        public IHttpActionResult GetAllPaginated(long inventoryId, int pageNumber = 1, string filter = "")
        {

            var result = new List<ProductDTO>();

            int pageRow = Convert.ToInt32(Constants.ConfigurationParameter.TotalItemsPerPage_InventoryDetails);

            if (!string.IsNullOrEmpty(filter))
            {
                filter = RemoveAccents(filter).ToLower();

                result = db.InventoryDetails.Where(x => x.InventoryId == inventoryId && (
                                  x.Product.ExternalCode.Contains(filter)
                                  || x.Product.BarCode.Contains(filter)
                                  || x.Product.FormattedDescription.Contains(filter)
                                  || x.Product.Reference.Contains(filter)
                )).Select(y => new ProductDTO()
                {
                    Id = y.Product.Id,
                    InventoryDetailId = y.Id,
                    Description = y.Product.Description,
                    ExternalCode = y.Product.ExternalCode,
                    BarCode = y.Product.BarCode,
                    OldCost = y.OldCost,
                    OldPrice = y.OldPrice,
                    Cost = y.CurrentCost,
                    Price = y.CurrentPrice,
                    Quantity = y.Quantity,
                    TotalAmount = y.TotalAmount,
                    InventoryId = inventoryId,
                    UserName = y.User.UserName,
                    SectionId = y.SectionId,
                    SectionDescription = y.Section.Description,
                    TariffId = y.TariffId,
                    TariffDescription = y.Tariff.Description,

                }).OrderByDescending(x => x.InventoryDetailId).ToList();

                pagination.Records = result.Skip((pageNumber - 1) * pageRow).Take(pageRow);
                pagination.Pagination = new Pagination()
                {
                    PageNumber = pageNumber,
                    PageRow = pageRow,
                    TotalRecord = result.Count(),
                    TotalPage = (int)Math.Ceiling(result.Count() / (double)pageRow),
                };

                response.Data = pagination;

                return Ok(response);
            }

            if (pageNumber == 1)
            {
                result = db.InventoryDetails.Where(x => x.InventoryId == inventoryId).Select(y => new ProductDTO()
                {
                    Id = y.Product.Id,
                    InventoryDetailId = y.Id,
                    Description = y.Product.Description,
                    ExternalCode = y.Product.ExternalCode,
                    BarCode = y.Product.BarCode,
                    OldCost = y.OldCost,
                    OldPrice = y.OldPrice,
                    Cost = y.CurrentCost,
                    Price = y.CurrentPrice,
                    Quantity = y.Quantity,
                    TotalAmount = y.TotalAmount,
                    InventoryId = inventoryId,
                    UserName = y.User.UserName,
                    SectionId = y.SectionId,
                    SectionDescription = y.Section.Description,
                    TariffId = y.TariffId,
                    TariffDescription = y.Tariff.Description,

                }).OrderByDescending(x => x.InventoryDetailId).ToList();

                pagination.Records = result.Skip((pageNumber - 1) * pageRow).Take(pageRow);
                pagination.Pagination = new Pagination()
                {
                    PageNumber = pageNumber,
                    PageRow = pageRow,
                    TotalRecord = result.Count(),
                    TotalPage = (int)Math.Ceiling(result.Count() / (double)pageRow),
                };

                response.Data = pagination;
            }

            if (pageNumber > 1)
            {
                result = db.InventoryDetails.Where(x => x.InventoryId == inventoryId).Select(y => new ProductDTO()
                {
                    Id = y.Product.Id,
                    InventoryDetailId = y.Id,
                    Description = y.Product.Description,
                    ExternalCode = y.Product.ExternalCode,
                    BarCode = y.Product.BarCode,
                    OldCost = y.OldCost,
                    OldPrice = y.OldPrice,
                    Cost = y.CurrentCost,
                    Price = y.CurrentPrice,
                    Quantity = y.Quantity,
                    TotalAmount = y.TotalAmount,
                    InventoryId = inventoryId,
                    UserName = y.User.UserName,
                    SectionId = y.SectionId,
                    SectionDescription = y.Section.Description,
                    TariffId = y.TariffId,
                    TariffDescription = y.Tariff.Description,

                }).OrderByDescending(x => x.InventoryDetailId).Skip((pageNumber - 1) * pageRow).Take(pageRow).ToList();

                response.Data = result;
            }

            return Ok(response);
        }



        [HttpPost]
        [Route("Create")]
        public IHttpActionResult Create([FromBody] Inventory request)
        {

            var inventory = db.Inventories.Where(x => x.UserId == currentUserId
                                                && x.InventoryStatus.ShortName == Global.Constants.InventoryStatuses.Open
                                                && x.IsActive == true).ToList().LastOrDefault();

            if (inventory != null)
            {
                response.Code = "400";
                response.Message = "Estimado usuario usted ya tiene un inventario aperturado";

                return Ok(response);
            }

            if (string.IsNullOrEmpty(request.Description))
            {
                response.Code = "400";
                response.Message = "Favor ingrese un nombre de inventario valido";

                return Ok(response);
            }

            var status = db.InventoryStatuses.Where(x => x.ShortName == Global.Constants.InventoryStatuses.Open).FirstOrDefault();

            request.StatusId = status.Id;
            request.UserId = currentUserId;
            request.OpenDate = DateTime.Now;
            request.OpenDateFormatted = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
            request.CreationTime = DateTime.Now;
            request.CreatorUserId = currentUserId;
            request.IsActive = true;

            var result = db.Inventories.Add(request);
            db.SaveChanges();

            response.Message = "Inventario aperturado con éxito";

            return Ok(response);
        }


        [HttpPut]
        [Route("Update")]
        public IHttpActionResult Update([FromBody] Inventory request)
        {

            if (string.IsNullOrEmpty(request.Description))
            {
                response.Code = "400";
                response.Message = "Favor ingrese un nombre de inventario valido";

                return Ok(response);
            }

            var result = db.Inventories.Where(x => x.Id == request.Id).FirstOrDefault();

            result.Description = request.Description;
            result.LastModificationTime = DateTime.Now;
            result.LastModifierUserId = currentUserId;

            db.SaveChanges();

            response.Message = "Iventario actualizado con éxito";

            return Ok(response);
        }


        [HttpPost]
        [Route("SaveItem")]
        public IHttpActionResult SaveItem([FromBody] ProductDTO request)
        {
            var currentItem = db.Products.Where(x => x.Id == request.Id).FirstOrDefault();

            var currentInventory = db.Inventories.Where(x => x.Id == request.InventoryId).FirstOrDefault();
            decimal totalAmountInventory = currentInventory.TotalAmount;

            if (currentItem == null)
            {
                response.Code = "404";
                response.Message = "El item que intenta guardar no es válido";

                return Ok(response);
            }

            if (request.Origin == "EDIT")
            {
                var currentInventoryDetail = db.InventoryDetails.Where(x => x.Id == request.InventoryDetailId && x.ProductId == request.Id).FirstOrDefault();

                if (currentInventoryDetail != null)
                {
                    currentInventoryDetail.OldCost = currentItem.Cost;
                    currentInventoryDetail.OldPrice = currentItem.Price;
                    currentInventoryDetail.CurrentCost = request.Cost;
                    currentInventoryDetail.CurrentPrice = request.Price;
                    currentInventoryDetail.Quantity = request.Quantity;
                    currentInventoryDetail.SectionId = request.SectionId;
                    currentInventoryDetail.TariffId = request.TariffId;
                    currentInventoryDetail.Difference = (request.Quantity - currentItem.Existence);
                    currentInventoryDetail.TotalAmount = (request.Quantity * request.Cost);

                    currentInventoryDetail.LastModificationTime = DateTime.Now;
                    currentInventoryDetail.LastModifierUserId = currentUserId;

                    db.SaveChanges();
                }
            }

            if (request.Origin == "ADD")
            {
                var inventoryDetail = new InventoryDetail()
                {
                    InventoryId = request.InventoryId,
                    ProductId = request.Id,
                    OldCost = currentItem.Cost,
                    OldPrice = currentItem.Price,
                    CurrentCost = request.Cost,
                    CurrentPrice = request.Price,
                    Quantity = request.Quantity,
                    UserId = currentUserId,
                    SectionId = request.SectionId,
                    TariffId = request.TariffId,
                    Difference = (request.Quantity - currentItem.Existence),
                    TotalAmount = (request.Quantity * request.Cost),

                    CreationTime = DateTime.Now,
                    CreatorUserId = currentUserId,
                    IsActive = true,
                };

                var result = db.InventoryDetails.Add(inventoryDetail);
                db.SaveChanges();
            }

            response.Message = "Item guardado con éxito";

            return Ok(response);

        }



        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult Delete(long id)
        {
            var result = db.Inventories.Where(x => x.Id == id & x.IsActive == true).FirstOrDefault();

            if (result == null)
            {
                return NotFound();
            }

            result.IsActive = false;
            result.IsDeleted = true;
            result.DeletionTime = DateTime.Now;
            result.DeleterUserId = currentUserId;
            db.SaveChanges();

            response.Message = "Inventario eliminado con éxito";

            return Ok(response);
        }


        [HttpDelete]
        [Route("DeleteInventoryDetail")]
        public IHttpActionResult DeleteInventoryDetail(long id)
        {
            var item = db.InventoryDetails.Where(x => x.Id == id).FirstOrDefault();

            if (item == null)
            {
                response.Code = "404";
                response.Message = "El item que intenta eliminar no existe";

                return Ok(response);
            }

            var result = db.InventoryDetails.Remove(item);
            db.SaveChanges();

            response.Message = "item eliminado con éxito";

            return Ok(response);
        }


        [HttpGet]
        [Route("ClosedInventory")]
        public IHttpActionResult ClosedInventory(long id)
        {
            var status = db.InventoryStatuses.Where(x => x.ShortName == Global.Constants.InventoryStatuses.Closed).FirstOrDefault();

            var inventory = db.Inventories.Where(x => x.Id == id & x.IsActive == true).FirstOrDefault();

            if (inventory == null)
            {
                response.Code = "400";
                response.Message = "El inventario que intenta cerrar no existe";
                return Ok(response);
            }

            inventory.StatusId = status.Id;
            inventory.ClosedDate = DateTime.Now;
            inventory.ClosedDateFormatted = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

            db.SaveChanges();

            response.Message = "Inventario cerrado con éxito";

            return Ok(response);
        }



        [HttpGet]
        [Route("GenerateInventoryExcel")]
        [AllowAnonymous]
        public IHttpActionResult GenerateInventoryExcel(long id)
        {
            var inventoryService = new InventoryServices();

            inventoryService.GenerateInventoryExcel(id);

            return Ok();
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("GenerateInventoryPDF")]
        public IHttpActionResult GenerateInventoryPDF()
        {
            var inventoryService = new InventoryServices();

            inventoryService.GenerateInventoryDetailsPDF();

            return Ok();
        }


        [HttpGet]
        [Route("GetSection")]
        public IEnumerable<SectionDTO> GetSection()
        {
            var result = db.InventorySections.Where(x => x.IsActive == true).Select(y => new SectionDTO()
            {
                Id = y.Id,
                ShortName = y.ShortName,
                Description = y.Description,

            }).ToList();

            return result;
        }


        [HttpGet]
        [Route("GetTariff")]
        public IEnumerable<TariffDTO> GetTariff()
        {
            var result = db.InventoryTariffs.Where(x => x.IsActive == true).Select(y => new TariffDTO()
            {
                Id = y.Id,
                ShortName = y.ShortName,
                Description = y.Description,

            }).ToList();

            return result;
        }





        private string RemoveAccents(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }


    }
}
