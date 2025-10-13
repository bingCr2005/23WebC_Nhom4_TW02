using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace _23WebC_Nhom4_TW02.Models
{
    public class CategoryDao : ICategoryDao
    {
        private readonly string connectionString;

        public CategoryDao(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Category> GetAllCategories()
        {
            var list = new List<Category>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT CategoryID, CategoryName, Slug, Description, IsActive, CreatedAt FROM Categories";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Category
                    {
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        CategoryName = reader["CategoryName"].ToString(),
                        Slug = reader["Slug"].ToString(),
                        Description = reader["Description"].ToString(),
                        IsActive = Convert.ToBoolean(reader["IsActive"]),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                    });
                }
                reader.Close();
            }

            return list;
        }

        public Category GetCategoryById(int id)
        {
            Category category = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT CategoryID, CategoryName, Slug, Description, IsActive, CreatedAt FROM Categories WHERE CategoryID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    category = new Category
                    {
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        CategoryName = reader["CategoryName"].ToString(),
                        Slug = reader["Slug"].ToString(),
                        Description = reader["Description"].ToString(),
                        IsActive = Convert.ToBoolean(reader["IsActive"]),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                    };
                }
                reader.Close();
            }

            return category;
        }

        public bool AddCategory(Category category)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Categories (CategoryName, Slug, Description, IsActive, CreatedAt)
                                 VALUES (@name, @slug, @desc, @active, @created)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", category.CategoryName);
                cmd.Parameters.AddWithValue("@slug", category.Slug);
                cmd.Parameters.AddWithValue("@desc", category.Description);
                cmd.Parameters.AddWithValue("@active", category.IsActive);
                cmd.Parameters.AddWithValue("@created", category.CreatedAt);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool UpdateCategory(Category category)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Categories 
                                 SET CategoryName = @name, Slug = @slug, Description = @desc, IsActive = @active
                                 WHERE CategoryID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", category.CategoryName);
                cmd.Parameters.AddWithValue("@slug", category.Slug);
                cmd.Parameters.AddWithValue("@desc", category.Description);
                cmd.Parameters.AddWithValue("@active", category.IsActive);
                cmd.Parameters.AddWithValue("@id", category.CategoryID);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteCategory(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Categories WHERE CategoryID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
