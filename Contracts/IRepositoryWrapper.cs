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

        IBannerRepository Banner { get; }
        ICategoryBlogRepository CategoryBlog { get; }
        IBlogRepository Blog { get; }
        IOrderRepository Order { get; }
        ICommentRepository Comment { get; }
        ISubCategoryRepository SubCategory { get; }
        ICommercialRepository Commercial { get; }
        ICategoryRepository Category { get; }
        IEventRepository Event { get; }
        ISponsorRepository Sponsor { get; }
        IPartnerRepository Partner { get; }
        IPaymentRepository Payment { get; }
        IPlaceRepository Place { get; }
        IPromoteRepository Promote { get; }
        IPromoteEventRepository PromoteEvent { get; }
        IAnnualRateRepository AnnualRate { get; }

        IWorkstationRepository Workstation { get; }
        IEmailSenderRepository EmailSender { get; }
        string Path { set; }

        Task SaveAsync();
    }
}
