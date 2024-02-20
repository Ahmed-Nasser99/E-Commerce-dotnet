using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Platform.Dtos;
using Platform.Model;
using Platform.Repositry;
using Platform.Services;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Platform.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork<Product> _product;
        private readonly IHostingEnvironment _hosting;
        public ProductController(IUnitOfWork<Product> product, IHostingEnvironment hosting)
        {
            _product = product;
            _hosting = hosting;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var products = _product.Entity.GetAll(x=>x.subcategory,x => x.brand);
                if (products != null)
                {
                    foreach (var product in products)
                    {
                        product.image = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}{product.image}";
                        product.subcategory.image = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}{product.subcategory.image}";
                        product.brand.image = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}{product.brand.image}";
                    }
                    return Ok(products);
                }
                else
                {
                    return BadRequest("There's No Data");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var product = _product.Entity.GetById(id, x => x.subcategory, x => x.brand);
                if (product != null)
                {
                    product.image = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}{product.image}";
                    product.subcategory.image = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}{product.subcategory.image}";
                    product.brand.image = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}{product.brand.image}";
                    return Ok(product);
                }
                else
                {
                    return BadRequest("There's No Data");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult SaveNew([FromForm] AddProductViewModel model) {
            try
            {
                if (ModelState.IsValid)
                {
                    var product = new Product
                    {
                        title = model.title,
                        description = model.description,
                        price = model.price,
                        quantity = model.quantity,
                        image = UploadServices.UploadFile(model.image, "ProducsFile", _hosting),
                        brandid = model.brandid,
                        subcategoryid = model.subcategoryid
                    };
                    _product.Entity.Insert(product);
                    _product.Complete();
                    return Ok("Data Inserted Successfuly");

                }
                else
                {
                    return BadRequest("Check Your Data");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }

        }
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _product.Entity.Delete(id);
                _product.Complete();
                return Ok("Product Deleted Successfuly");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public IActionResult Update(Guid id,[FromForm] AddProductViewModel model)
        {
            try
            {
                if (ModelState.IsValid && id != null)
                {

                    var product = new Product
                    {
                        id = id,   
                        title = model.title,
                        description = model.description,
                        price = model.price,
                        quantity = model.quantity,
                        image = UploadServices.UploadFile(model.image, "ProducsFile", _hosting),
                        brandid = model.brandid,
                        subcategoryid = model.subcategoryid
                    };

                    _product.Entity.Update(product);
                    _product.Complete();
                    return Ok("Data Updated Successfuly");

                }
                else
                {
                    return BadRequest("Check Your Data");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
