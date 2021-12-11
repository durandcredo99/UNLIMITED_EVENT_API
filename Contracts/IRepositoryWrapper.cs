using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryWrapper
    {
        IFileRepository File { get; }

        IAccountRepository Account { get; }
        IAppUserRepository AppUser { get; }

        ICategoryBlogRepository CategoryBlog { get; }
        IBlogRepository Blog { get; }
        ICommandRepository Command { get; }
        ICommentRepository Comment { get; }
        ICommercialRepository Commercial { get; }
        ICategoryRepository Category { get; }
        IEventRepository Event { get; }
        ISponsorRepository Sponsor { get; }
        IPaymentRepository Payment { get; }
        IPaymentTypeRepository PaymentType { get; }
        IPlaceRepository Place { get; }
        IWorkstationRepository Workstation { get; }
        IMailRepository Mail { get; }
        string Path { set; }

        Task SaveAsync();
    }
}
