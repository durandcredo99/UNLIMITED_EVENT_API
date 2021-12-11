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
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        private ISortHelper<Comment> _sortHelper;

        public CommentRepository(
            RepositoryContext repositoryContext,
            ISortHelper<Comment> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Comment>> GetCommentsAsync(CommentParameters commentParameters)
        {
            var comments = Enumerable.Empty<Comment>().AsQueryable();

            ApplyFilters(ref comments, commentParameters);

            PerformSearch(ref comments, commentParameters.SearchTerm);

            var sortedComments = _sortHelper.ApplySort(comments, commentParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<Comment>.ToPagedList
                (
                    sortedComments,
                    commentParameters.PageNumber,
                    commentParameters.PageSize)
                );
        }

        public async Task<Comment> GetCommentByIdAsync(Guid id)
        {
            return await FindByCondition(comment => comment.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CommentExistAsync(Comment comment)
        {
            return await FindByCondition(x => x.Message == comment.Message)
                .AnyAsync();
        }

        public async Task CreateCommentAsync(Comment comment)
        {
            await CreateAsync(comment);
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            await UpdateAsync(comment);
        }

        public async Task UpdateCommentAsync(IEnumerable<Comment> comments)
        {
            await UpdateAsync(comments);
        }

        public async Task DeleteCommentAsync(Comment comment)
        {
            await DeleteAsync(comment);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Comment> comments, CommentParameters commentParameters)
        {
            comments = FindAll();
            /*
            if (!string.IsNullOrWhiteSpace(commentParameters.AppUserId))
            {
                comments = comments.Where(x => x.AppUserId == commentParameters.AppUserId);
            }

            if (commentParameters.MinBirthday != null)
            {
                comments = comments.Where(x => x.Birthday >= commentParameters.MinBirthday);
            }

            if (commentParameters.MaxBirthday != null)
            {
                comments = comments.Where(x => x.Birthday < commentParameters.MaxBirthday);
            }

            if (commentParameters.MinCreateAt != null)
            {
                comments = comments.Where(x => x.CreateAt >= commentParameters.MinCreateAt);
            }

            if (commentParameters.MaxCreateAt != null)
            {
                comments = comments.Where(x => x.CreateAt < commentParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<Comment> comments, string searchTerm)
        {
            if (!comments.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            comments = comments.Where(x => x.Message.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        #endregion

    }
}
