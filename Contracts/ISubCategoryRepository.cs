
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ISubCategoryRepository
    {
        Task<PagedList<SubCategory>> GetSubCategoriesAsync(SubCategoryParameters subCategoryParameters);

        Task<SubCategory> GetSubCategoryByIdAsync(Guid id);
        Task<bool> SubCategoryExistAsync(SubCategory subCategory);

        Task CreateSubCategoryAsync(SubCategory subCategory);
        Task UpdateSubCategoryAsync(SubCategory subCategory);
        Task UpdateSubCategoryAsync(IEnumerable<SubCategory> subCategories);
        Task DeleteSubCategoryAsync(SubCategory subCategory);
    }
}
