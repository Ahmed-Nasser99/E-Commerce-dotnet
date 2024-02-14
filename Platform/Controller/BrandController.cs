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
    public class BrandController : ControllerBase
    {
        private readonly IUnitOfWork<Brand> _brand;
        private readonly IHostingEnvironment _hosting;

        public BrandController(IUnitOfWork<Brand> brand,IHostingEnvironment hosting)
        {
            _brand = brand;
            _hosting = hosting;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var brands = _brand.Entity.GetAll();
                if (brands != null)
                {
                    foreach (var brand in brands)
                    {
                        brand.image = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}{brand.image}";
                    }
                    return Ok(brands);
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
                var brand = _brand.Entity.GetById(id);
                if (brand != null)
                {
                    brand.image = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}{brand.image}";
                    return Ok(brand);
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
        public IActionResult SaveNew([FromForm] AddBrandViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var brand = new Brand
                    {
                        image = UploadServices.UploadFile(model.image, "BrandsFile", _hosting),
                        name = model.name,
                    };
                    _brand.Entity.Insert(brand);
                    _brand.Complete();
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
                _brand.Entity.Delete(id);
                _brand.Complete();
                return Ok("Product Deleted Successfuly");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromForm] AddBrandViewModel model)
        {
            try
            {
                if (ModelState.IsValid && id != null)
                {

                    var brand = new Brand
                    {
                        id = id,
                        image = UploadServices.UploadFile(model.image, "BrandsFile", _hosting),
                        name = model.name,
                    };

                    _brand.Entity.Update(brand);
                    _brand.Complete();
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
