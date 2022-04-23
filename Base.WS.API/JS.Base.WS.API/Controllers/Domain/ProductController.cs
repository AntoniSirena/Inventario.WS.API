using JS.Base.WS.API.Base;
using JS.Base.WS.API.DBContext;
using JS.Base.WS.API.DTO.Common;
using JS.Base.WS.API.DTO.Domain;
using JS.Base.WS.API.Helpers;
using JS.Base.WS.API.Models.Domain;
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
    [RoutePrefix("api/product")]
    public class ProductController : ApiController
    {

        private MyDBcontext db;
        private Response response;
        private PaginationDTO pagination;

        private long currentUserId = CurrentUser.GetId();

        public ProductController()
        {
            db = new MyDBcontext();
            response = new Response();
            pagination = new PaginationDTO();
        }


        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<ProductDTO> GetAll()
        {
            var result = db.Products.Where(x => x.IsActive == true).Select(y => new ProductDTO()
            {
                Id = y.Id,
                Description = y.Description,
                ExternalCode = y.ExternalCode,
                BarCode = y.BarCode,
                Cost = y.Cost,
                Price = y.Price,

            }).OrderByDescending(x => x.Id).ToList();

            return result;
        }

        [HttpGet]
        [Route("GetAllPaginated")]
        public IHttpActionResult GetAllPaginated(int pageNumber = 1, int pageRow = 10, string filter = "")
        {
            var result = new List<ProductDTO>();

            if (pageNumber == 1)
            {
                result = db.Products.Where(x => x.IsActive == true).Select(y => new ProductDTO()
                {
                    Id = y.Id,
                    Description = y.Description,
                    ExternalCode = y.ExternalCode,
                    BarCode = y.BarCode,
                    Cost = y.Cost,
                    Price = y.Price,

                }).OrderByDescending(x => x.Id).ToList();


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
            else
            {
                result = db.Products.Where(x => x.IsActive == true).Select(y => new ProductDTO()
                {
                    Id = y.Id,
                    Description = y.Description,
                    ExternalCode = y.ExternalCode,
                    BarCode = y.BarCode,
                    Cost = y.Cost,
                    Price = y.Price,

                }).OrderByDescending(x => x.Id).Skip((pageNumber - 1) * pageRow).Take(pageRow).ToList();

                response.Data = result;
            }

            return Ok(response);
        }


        [HttpGet]
        [Route("GetById")]
        public Product GetById(long id)
        {
            var result = db.Products.Where(x => x.Id == id).FirstOrDefault();

            return result;
        }


        [HttpPost]
        [Route("UploadProducts")]
        public IHttpActionResult UploadProducts([FromBody] List<ProductDTO> products)
        {

            foreach (var item in products)
            {
                var product = new Product();

                product.ExternalCode = item.ExternalCode;
                product.BarCode = item.BarCode;
                product.Description = item.Description;
                product.FormattedDescription = RemoveAccents(item.Description).ToLower();
                product.Cost = item.Cost;
                product.Price = item.Price;
                product.CreationTime = DateTime.Now;
                product.CreatorUserId = currentUserId;
                product.IsActive = true;

                var result = db.Products.Add(product);
                db.SaveChanges();
            }

            response.Message = "Productos guardados con éxito";

            return Ok(response);
        }



        [HttpPost]
        [Route("Create")]
        public IHttpActionResult Create([FromBody] Product request)
        {

            if (string.IsNullOrEmpty(request.Description))
            {
                response.Code = "400";
                response.Message = "Favor ingrese un producto valido";

                return Ok(response);
            }

            request.FormattedDescription = RemoveAccents(request.Description).ToLower();
            request.CreationTime = DateTime.Now;
            request.CreatorUserId = currentUserId;
            request.IsActive = true;

            var result = db.Products.Add(request);
            db.SaveChanges();

            response.Message = "Producto creado con éxito";

            return Ok(response);
        }


        [HttpPut]
        [Route("Update")]
        public IHttpActionResult Update([FromBody] Product request)
        {

            if (string.IsNullOrEmpty(request.Description))
            {
                response.Code = "400";
                response.Message = "Favor ingrese un producto valido";

                return Ok(response);
            }

            var result = db.Products.Where(x => x.Id == request.Id).FirstOrDefault();

            result.Description = request.Description;
            result.FormattedDescription = RemoveAccents(request.Description).ToLower();
            result.ExternalCode = request.ExternalCode;
            result.BarCode = request.BarCode;
            result.Cost = request.Cost;
            result.Price = request.Price;
            result.LastModificationTime = DateTime.Now;
            result.LastModifierUserId = currentUserId;

            db.SaveChanges();

            response.Message = "Producto actualizado con éxito";

            return Ok(response);
        }


        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult Delete(long id)
        {
            var result = db.Products.Where(x => x.Id == id & x.IsActive == true).FirstOrDefault();

            if (result == null)
            {
                return NotFound();
            }

            result.IsActive = false;
            result.IsDeleted = true;
            result.DeletionTime = DateTime.Now;
            result.DeleterUserId = currentUserId;
            db.SaveChanges();

            response.Message = "Producto eliminado con éxito";

            return Ok(response);
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
