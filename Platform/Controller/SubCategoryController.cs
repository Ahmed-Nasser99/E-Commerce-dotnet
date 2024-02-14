using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Platform.Dtos;
using Platform.Model;
using Platform.Repositry;
using Platform.Services;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Platform.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly IUnitOfWork<SubCategory> _subCategory;
        private readonly IHostingEnvironment _hosting;

        public SubCategoryController(IUnitOfWork<SubCategory> subCategory, IHostingEnvironment hosting)
        {
            _subCategory = subCategory;
            _hosting = hosting;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var subCategories = _subCategory.Entity.GetAll();
                if (subCategories != null)
                {
                    foreach (var subCategory in subCategories)
                    {
                        subCategory.image = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}{subCategory.image}";
                    }
                    return Ok(subCategories);
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
                var subCategory = _subCategory.Entity.GetById(id);
                if (subCategory != null)
                {
                    subCategory.image = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}{subCategory.image}";
                    return Ok(subCategory);
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
        public IActionResult SaveNew([FromForm] AddSubcategoryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var subCategory = new SubCategory
                    {
                        image = UploadServices.UploadFile(model.image, "SubCategoriesFile", _hosting),
                        name = model.name,
                        categoryid = model.categoryid
                    };
                    _subCategory.Entity.Insert(subCategory);
                    _subCategory.Complete();
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
                _subCategory.Entity.Delete(id);
                _subCategory.Complete();
                return Ok("Product Deleted Successfuly");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromForm] AddSubcategoryViewModel model)
        {
            try
            {
                if (ModelState.IsValid && id != null)
                {

                    var subCategory = new SubCategory
                    {
                        id = id,
                        image = UploadServices.UploadFile(model.image, "BrandsFile", _hosting),
                        name = model.name,
                        categoryid = model.categoryid
                    };

                    _subCategory.Entity.Update(subCategory);
                    _subCategory.Complete();
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
