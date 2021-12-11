
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ICommentRepository
    {
        Task<PagedList<Comment>> GetCommentsAsync(CommentParameters commentParameters);

        Task<Comment> GetCommentByIdAsync(Guid id);
        Task<bool> CommentExistAsync(Comment comment);

        Task CreateCommentAsync(Comment comment);
        Task UpdateCommentAsync(Comment comment);
        Task UpdateCommentAsync(IEnumerable<Comment> comments);
        Task DeleteCommentAsync(Comment comment);
    }
}
