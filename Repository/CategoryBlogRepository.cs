using Contracts;
using Entities;
using Entities.Extensions;
using Entities.Helpers;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CategoryBlogRepository : RepositoryBase<CategoryBlog>, ICategoryBlogRepository
    {
        private ISortHelper<CategoryBlog> _sortHelper;

        public CategoryBlogRepository(
            RepositoryContext repositoryContext,
            ISortHelper<CategoryBlog> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<CategoryBlog>> GetCategoriesBlogAsync(CategoryBlogParameters categoryBlogParameters)
        {
            var blogcategories = Enumerable.Empty<CategoryBlog>().AsQueryable();

            ApplyFilters(ref blogcategories, categoryBlogParameters);

            PerformSearch(ref blogcategories, categoryBlogParameters.SearchTerm);

            var sortedCategoriesBlog = _sortHelper.ApplySort(blogcategories, categoryBlogParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<CategoryBlog>.ToPagedList
                (
                    sortedCategoriesBlog,
                    categoryBlogParameters.PageNumber,
                    categoryBlogParameters.PageSize)
                );
        }

        public async Task<CategoryBlog> GetCategoryBlogByIdAsync(Guid id)
        {
            return await FindByCondition(categoryBlog => categoryBlog.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CategoryBlogExistAsync(CategoryBlog categoryBlog)
        {
            return await FindByCondition(x => x.Name == categoryBlog.Name)
                .AnyAsync();
        }

        public async Task CreateCategoryBlogAsync(CategoryBlog categoryBlog)
        {
            await CreateAsync(categoryBlog);
        }

        public async Task UpdateCategoryBlogAsync(CategoryBlog categoryBlog)
        {
            await UpdateAsync(categoryBlog);
        }

        public async Task UpdateCategoryBlogAsync(IEnumerable<CategoryBlog> blogcategories)
        {
            await UpdateAsync(blogcategories);
        }

        public async Task DeleteCategoryBlogAsync(CategoryBlog categoryBlog)
        {
            await DeleteAsync(categoryBlog);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<CategoryBlog> blogcategories, CategoryBlogParameters categoryBlogParameters)
        {
            blogcategories = FindAll();
            /*
            if (!string.IsNullOrWhiteSpace(categoryBlogParameters.AppUserId))
            {
                blogcategories = blogcategories.Where(x => x.AppUserId == categoryBlogParameters.AppUserId);
            }

            if (categoryBlogParameters.MinBirthday != null)
            {
                blogcategories = blogcategories.Where(x => x.Birthday >= categoryBlogParameters.MinBirthday);
            }

            if (categoryBlogParameters.MaxBirthday != null)
            {
                blogcategories = blogcategories.Where(x => x.Birthday < categoryBlogParameters.MaxBirthday);
            }

            if (categoryBlogParameters.MinCreateAt != null)
            {
                blogcategories = blogcategories.Where(x => x.CreateAt >= categoryBlogParameters.MinCreateAt);
            }

            if (categoryBlogParameters.MaxCreateAt != null)
            {
                blogcategories = blogcategories.Where(x => x.CreateAt < categoryBlogParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<CategoryBlog> blogcategories, string searchTerm)
        {
            if (!blogcategories.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            blogcategories = blogcategories.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}
