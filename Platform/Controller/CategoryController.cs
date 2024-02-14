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
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork<Category> _category;
        private readonly IHostingEnvironment _hosting;

        public CategoryController(IUnitOfWork<Category> category, IHostingEnvironment hosting)
        {
            _category = category;
            _hosting = hosting;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var Categories = _category.Entity.GetAll();
                if (Categories != null)
                {
                    foreach (var category in Categories)
                    {
                        category.image = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}{category.image}";
                    }
                    return Ok(Categories);
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
                var category = _category.Entity.GetById(id);
                if (category != null)
                {
                    category.image = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}{category.image}";
                    return Ok(category);
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
        public IActionResult SaveNew([FromForm] AddCategoryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var category = new Category
                    {
                        image = UploadServices.UploadFile(model.image, "CategoriesFile", _hosting),
                        name = model.name,
                    };
                    _category.Entity.Insert(category);
                    _category.Complete();
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
                _category.Entity.Delete(id);
                _category.Complete();
                return Ok("Product Deleted Successfuly");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromForm] AddCategoryViewModel model)
        {
            try
            {
                if (ModelState.IsValid && id != null)
                {

                    var category = new Category
                    {
                        id = id,
                        image = UploadServices.UploadFile(model.image, "CategoriesFile", _hosting),
                        name = model.name,
                    };

                    _category.Entity.Update(category);
                    _category.Complete();
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
