using System.Collections.Generic;

namespace _23WebC_Nhom4_TW02.Models
{
    public interface ICategoryDao
    {
        List<Category> GetAllCategories();
        Category GetCategoryById(int id);
        bool AddCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(int id);
    }
}
