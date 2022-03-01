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
    public class BlogRepository : RepositoryBase<Blog>, IBlogRepository
    {
        private ISortHelper<Blog> _sortHelper;

        public BlogRepository(
            RepositoryContext repositoryContext,
            ISortHelper<Blog> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Blog>> GetBlogsAsync(BlogParameters blogParameters)
        {
            var blogs = Enumerable.Empty<Blog>().AsQueryable();

            ApplyFilters(ref blogs, blogParameters);

            PerformSearch(ref blogs, blogParameters.SearchTerm);

            var sortedBlogs = _sortHelper.ApplySort(blogs, blogParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<Blog>.ToPagedList
                (
                    sortedBlogs,
                    blogParameters.PageNumber,
                    blogParameters.PageSize)
                );
        }

        public async Task<Blog> GetBlogByIdAsync(Guid id)
        {
            return await FindByCondition(blog => blog.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> BlogExistAsync(Blog blog)
        {
            return await FindByCondition(x => x.Title == blog.Title)
                .AnyAsync();
        }

        public async Task CreateBlogAsync(Blog blog)
        {
            await CreateAsync(blog);
        }

        public async Task UpdateBlogAsync(Blog blog)
        {
            await UpdateAsync(blog);
        }

        public async Task UpdateBlogAsync(IEnumerable<Blog> blogs)
        {
            await UpdateAsync(blogs);
        }

        public async Task DeleteBlogAsync(Blog blog)
        {
            await DeleteAsync(blog);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Blog> blogs, BlogParameters blogParameters)
        {
            blogs = FindAll()
                .Include(x => x.CategoryBlog);

            if (blogParameters.OfCategoryBlogId != null && blogParameters.OfCategoryBlogId != new Guid())
            {
                blogs = blogs.Where(x => x.CategoryBlogId == blogParameters.OfCategoryBlogId);
            }

            if (!string.IsNullOrWhiteSpace(blogParameters.OrganizedBy))
            {
                blogs = blogs.Where(x => x.AppUserId == blogParameters.OrganizedBy);
            }
            /*
            if (!string.IsNullOrWhiteSpace(blogParameters.AppUserId))
            {
                blogs = blogs.Where(x => x.AppUserId == blogParameters.AppUserId);
            }

            if (blogParameters.MinBirthday != null)
            {
                blogs = blogs.Where(x => x.Birthday >= blogParameters.MinBirthday);
            }

            if (blogParameters.MaxBirthday != null)
            {
                blogs = blogs.Where(x => x.Birthday < blogParameters.MaxBirthday);
            }

            if (blogParameters.MinCreateAt != null)
            {
                blogs = blogs.Where(x => x.CreateAt >= blogParameters.MinCreateAt);
            }

            if (blogParameters.MaxCreateAt != null)
            {
                blogs = blogs.Where(x => x.CreateAt < blogParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<Blog> blogs, string searchTerm)
        {
            if (!blogs.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            blogs = blogs.Where(x => x.Title.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}
