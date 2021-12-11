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
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        private ISortHelper<Category> _sortHelper;

        public CategoryRepository(
            RepositoryContext repositoryContext,
            ISortHelper<Category> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Category>> GetCategoriesAsync(CategoryParameters categoryParameters)
        {
            var categories = Enumerable.Empty<Category>().AsQueryable();

            ApplyFilters(ref categories, categoryParameters);

            PerformSearch(ref categories, categoryParameters.SearchTerm);

            var sortedCategories = _sortHelper.ApplySort(categories, categoryParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<Category>.ToPagedList
                (
                    sortedCategories,
                    categoryParameters.PageNumber,
                    categoryParameters.PageSize)
                );
        }

        public async Task<Category> GetCategoryByIdAsync(Guid id)
        {
            return await FindByCondition(category => category.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CategoryExistAsync(Category category)
        {
            return await FindByCondition(x => x.Name == category.Name)
                .AnyAsync();
        }

        public async Task CreateCategoryAsync(Category category)
        {
            await CreateAsync(category);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            await UpdateAsync(category);
        }

        public async Task UpdateCategoryAsync(IEnumerable<Category> categories)
        {
            await UpdateAsync(categories);
        }

        public async Task DeleteCategoryAsync(Category category)
        {
            await DeleteAsync(category);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Category> categories, CategoryParameters categoryParameters)
        {
            categories = FindAll();
            /*
            if (!string.IsNullOrWhiteSpace(categoryParameters.AppUserId))
            {
                categories = categories.Where(x => x.AppUserId == categoryParameters.AppUserId);
            }

            if (categoryParameters.MinBirthday != null)
            {
                categories = categories.Where(x => x.Birthday >= categoryParameters.MinBirthday);
            }

            if (categoryParameters.MaxBirthday != null)
            {
                categories = categories.Where(x => x.Birthday < categoryParameters.MaxBirthday);
            }

            if (categoryParameters.MinCreateAt != null)
            {
                categories = categories.Where(x => x.CreateAt >= categoryParameters.MinCreateAt);
            }

            if (categoryParameters.MaxCreateAt != null)
            {
                categories = categories.Where(x => x.CreateAt < categoryParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<Category> categories, string searchTerm)
        {
            if (!categories.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            categories = categories.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}
