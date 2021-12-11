
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IBlogRepository
    {
        Task<PagedList<Blog>> GetBlogsAsync(BlogParameters blogParameters);

        Task<Blog> GetBlogByIdAsync(Guid id);
        Task<bool> BlogExistAsync(Blog blog);

        Task CreateBlogAsync(Blog blog);
        Task UpdateBlogAsync(Blog blog);
        Task UpdateBlogAsync(IEnumerable<Blog> blogs);
        Task DeleteBlogAsync(Blog blog);
    }
}
