using Microsoft.AspNetCore.Mvc;
using _23WebC_Nhom4_TW02.Models;


namespace _23WebC_Nhom4_TW02.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/category")]
    public class CategoryController : Controller
    {
        private readonly ICategoryDao _categoryDao;

        public CategoryController(ICategoryDao categoryDao)
        {
            _categoryDao = categoryDao;
        }

        [HttpPost("add")]
        public IActionResult AddCategoryAjax([FromBody] Category category)
        {
            if (string.IsNullOrWhiteSpace(category.CategoryName))
                return BadRequest("Tên danh mục không được để trống.");

            bool success = _categoryDao.AddCategory(category);
            if (success)
                return Ok(new { message = "Thêm thành công", category });
            return StatusCode(500, "Lỗi khi thêm danh mục.");
        }
    }
}
