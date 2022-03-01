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
    public class SubCategoryRepository : RepositoryBase<SubCategory>, ISubCategoryRepository
    {
        private ISortHelper<SubCategory> _sortHelper;

        public SubCategoryRepository(
            RepositoryContext repositoryContext,
            ISortHelper<SubCategory> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<SubCategory>> GetSubCategoriesAsync(SubCategoryParameters subCategoryParameters)
        {
            var categories = Enumerable.Empty<SubCategory>().AsQueryable();

            ApplyFilters(ref categories, subCategoryParameters);

            PerformSearch(ref categories, subCategoryParameters.SearchTerm);

            var sortedSubCategories = _sortHelper.ApplySort(categories, subCategoryParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<SubCategory>.ToPagedList
                (
                    sortedSubCategories,
                    subCategoryParameters.PageNumber,
                    subCategoryParameters.PageSize)
                );
        }

        public async Task<SubCategory> GetSubCategoryByIdAsync(Guid id)
        {
            return await FindByCondition(subCategory => subCategory.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> SubCategoryExistAsync(SubCategory subCategory)
        {
            return await FindByCondition(x => x.Name == subCategory.Name)
                .AnyAsync();
        }

        public async Task CreateSubCategoryAsync(SubCategory subCategory)
        {
            await CreateAsync(subCategory);
        }

        public async Task UpdateSubCategoryAsync(SubCategory subCategory)
        {
            await UpdateAsync(subCategory);
        }

        public async Task UpdateSubCategoryAsync(IEnumerable<SubCategory> categories)
        {
            await UpdateAsync(categories);
        }

        public async Task DeleteSubCategoryAsync(SubCategory subCategory)
        {
            await DeleteAsync(subCategory);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<SubCategory> categories, SubCategoryParameters subCategoryParameters)
        {
            categories = FindAll();
            /*
            if (!string.IsNullOrWhiteSpace(subCategoryParameters.AppUserId))
            {
                categories = categories.Where(x => x.AppUserId == subCategoryParameters.AppUserId);
            }

            if (subCategoryParameters.MinBirthday != null)
            {
                categories = categories.Where(x => x.Birthday >= subCategoryParameters.MinBirthday);
            }

            if (subCategoryParameters.MaxBirthday != null)
            {
                categories = categories.Where(x => x.Birthday < subCategoryParameters.MaxBirthday);
            }

            if (subCategoryParameters.MinCreateAt != null)
            {
                categories = categories.Where(x => x.CreateAt >= subCategoryParameters.MinCreateAt);
            }

            if (subCategoryParameters.MaxCreateAt != null)
            {
                categories = categories.Where(x => x.CreateAt < subCategoryParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<SubCategory> categories, string searchTerm)
        {
            if (!categories.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            categories = categories.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}
