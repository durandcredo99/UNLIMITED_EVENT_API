
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ICategoryBlogRepository
    {
        Task<PagedList<CategoryBlog>> GetCategoriesBlogAsync(CategoryBlogParameters categoryBlogParameters);

        Task<CategoryBlog> GetCategoryBlogByIdAsync(Guid id);
        Task<bool> CategoryBlogExistAsync(CategoryBlog categoryBlog);

        Task CreateCategoryBlogAsync(CategoryBlog categoryBlog);
        Task UpdateCategoryBlogAsync(CategoryBlog categoryBlog);
        Task UpdateCategoryBlogAsync(IEnumerable<CategoryBlog> blogcategories);
        Task DeleteCategoryBlogAsync(CategoryBlog categoryBlog);
    }
}
