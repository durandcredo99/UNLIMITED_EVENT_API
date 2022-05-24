using Contracts;
using DinkToPdf.Contracts;
using Entities;
using Entities.Helpers;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private IFileRepository _fileRepository;
        private IAccountRepository _authenticationRepository;
        private IBannerRepository _bannerRepository;
        private ICategoryBlogRepository _categoryBlogRepository;
        private IBlogRepository _blogRepository;
        private IOrderRepository _orderRepository;
        private ICommentRepository _commentRepository;
        private ISubCategoryRepository _subCategoryRepository;
        private ICommercialRepository _commercialRepository;
        private ICategoryRepository _categoryRepository;
        private IDinkToPdfRepository _dinkToPdfRepository;
        private IEventRepository _eventRepository;
        private IPaymentRepository _paymentRepository;
        private IQrCodeRepository _qrCodeRepository;
        private IPlaceRepository _placeRepository;
        private ISponsorRepository _sponsorRepository;
        private IPartnerRepository _partnerRepository;
        private IPromoteRepository _promoteRepository;
        private IPromoteEventRepository _promoteEventRepository;
        private IAnnualRateRepository _annualRateRepository;

        private IAppUserRepository _appUser;
        private IEmailSenderRepository _emailSender;
        private IWebHostEnvironment _webHostEnvironment;
        private IWorkstationRepository _workstation;

        private readonly IConverter _converter;
        private readonly IConfiguration _configuration;
        private IHttpContextAccessor _httpContextAccessor;
        private IOptions<EmailSettings> _emailSettings;

        private readonly ISortHelper<AppUser> _appUserSortHelper;
        private readonly ISortHelper<Banner> _bannerSortHelper;
        private readonly ISortHelper<CategoryBlog> _categoryBlogSortHelper;
        private readonly ISortHelper<Blog> _blogSortHelper;
        private readonly ISortHelper<Order> _orderSortHelper;
        private readonly ISortHelper<Comment> _commentSortHelper;
        private readonly ISortHelper<SubCategory> _subCategorySortHelper;
        private readonly ISortHelper<Commercial> _commercialSortHelper;
        private readonly ISortHelper<Category> _categorySortHelper;
        private readonly ISortHelper<Event> _eventSortHelper;
        private readonly ISortHelper<Payment> _paymentSortHelper;
        private readonly ISortHelper<Place> _placeSortHelper;
        private readonly ISortHelper<Sponsor> _sponsorSortHelper;
        private readonly ISortHelper<Partner> _partnerSortHelper;
        private readonly ISortHelper<Promote> _promoteSortHelper;
        private readonly ISortHelper<PromoteEvent> _promoteEventSortHelper;
        private readonly ISortHelper<AnnualRate> _annualRateSortHelper;
        private readonly ISortHelper<Workstation> _workstationSortHelper;


        private RepositoryContext _repoContext;
        private UserManager<AppUser> _userManager;
        private RoleManager<Workstation> _roleManager;
        private readonly EmailConfiguration _emailConfig;

        private string filePath;
        public string Path
        {
            set { filePath = value; }
        }

        public IFileRepository File
        {
            get
            {
                if (_fileRepository == null)
                {
                    _fileRepository = new FileRepository(_webHostEnvironment, filePath);
                }
                return _fileRepository;
            }
        }

        public IAccountRepository Account
        {
            get
            {
                if (_authenticationRepository == null)
                {
                    _authenticationRepository = new AccountRepository(_repoContext, _userManager, _roleManager, _configuration, _httpContextAccessor);
                }
                return _authenticationRepository;
            }
        }
        public IBannerRepository Banner
        {
            get
            {
                if (_bannerRepository == null)
                {
                    _bannerRepository = new BannerRepository(_repoContext, _bannerSortHelper);
                }
                return _bannerRepository;
            }
        }

        public IBlogRepository Blog
        {
            get
            {
                if (_blogRepository == null)
                {
                    _blogRepository = new BlogRepository(_repoContext, _blogSortHelper);
                }
                return _blogRepository;
            }
        }

        public ICategoryBlogRepository CategoryBlog
        {
            get
            {
                if (_categoryBlogRepository == null)
                {
                    _categoryBlogRepository = new CategoryBlogRepository(_repoContext, _categoryBlogSortHelper);
                }
                return _categoryBlogRepository;
            }
        }

        public IOrderRepository Order
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new OrderRepository(_repoContext, _orderSortHelper);
                }
                return _orderRepository;
            }
        }

        public IDinkToPdfRepository PdfService
        {
            get
            {
                if (_dinkToPdfRepository == null)
                {
                    _dinkToPdfRepository = new DinkToPdfRepository(_converter);
                }
                return _dinkToPdfRepository;
            }
        }

        public IQrCodeRepository QrCode
        {
            get
            {
                if (_qrCodeRepository == null)
                {
                    _qrCodeRepository = new QrCodeRepository();
                }
                return _qrCodeRepository;
            }
        }

        public ICommentRepository Comment
        {
            get
            {
                if (_commentRepository == null)
                {
                    _commentRepository = new CommentRepository(_repoContext, _commentSortHelper);
                }
                return _commentRepository;
            }
        }

        public ISubCategoryRepository SubCategory
        {
            get
            {
                if (_subCategoryRepository == null)
                {
                    _subCategoryRepository = new SubCategoryRepository(_repoContext, _subCategorySortHelper);
                }
                return _subCategoryRepository;
            }
        }

        public ICommercialRepository Commercial
        {
            get
            {
                if (_commercialRepository == null)
                {
                    _commercialRepository = new CommercialRepository(_repoContext, _commercialSortHelper);
                }
                return _commercialRepository;
            }
        }

        public ICategoryRepository Category
        {
            get
            {
                if (_categoryRepository == null)
                {
                    _categoryRepository = new CategoryRepository(_repoContext, _categorySortHelper);
                }
                return _categoryRepository;
            }
        }

        public IEventRepository Event
        {
            get
            {
                if (_eventRepository == null)
                {
                    _eventRepository = new EventRepository(_repoContext, _eventSortHelper);
                }
                return _eventRepository;
            }
        }

        public IPaymentRepository Payment
        {
            get
            {
                if (_paymentRepository == null)
                {
                    _paymentRepository = new PaymentRepository(_repoContext, _paymentSortHelper);
                }
                return _paymentRepository;
            }
        }
        public IPlaceRepository Place
        {
            get
            {
                if (_placeRepository == null)
                {
                    _placeRepository = new PlaceRepository(_repoContext, _placeSortHelper, _webHostEnvironment);
                }
                return _placeRepository;
            }
        }

       public IPartnerRepository Partner
        {
            get
            {
                if (_partnerRepository == null)
                {
                    _partnerRepository = new PartnerRepository(_repoContext, _partnerSortHelper);
                }
                return _partnerRepository;
            }
        }

        public IPromoteRepository Promote
        {
            get
            {
                if (_promoteRepository == null)
                {
                    _promoteRepository = new PromoteRepository(_repoContext, _promoteSortHelper);
                }
                return _promoteRepository;
            }
        }

        public IPromoteEventRepository PromoteEvent
        {
            get
            {
                if (_promoteEventRepository == null)
                {
                    _promoteEventRepository = new PromoteEventRepository(_repoContext, _promoteEventSortHelper);
                }
                return _promoteEventRepository;
            }
        }

        public IAnnualRateRepository AnnualRate
        {
            get
            {
                if (_annualRateRepository == null)
                {
                    _annualRateRepository = new AnnualRateRepository(_repoContext, _annualRateSortHelper);
                }
                return _annualRateRepository;
            }
        }

        public ISponsorRepository Sponsor
        {
            get
            {
                if (_sponsorRepository == null)
                {
                    _sponsorRepository = new SponsorRepository(_repoContext, _sponsorSortHelper);
                }
                return _sponsorRepository;
            }
        }

        public IAppUserRepository AppUser
        {
            get
            {
                if (_appUser == null)
                {
                    _appUser = new AppUserRepository(_repoContext, _appUserSortHelper, _userManager, _roleManager);
                }
                return _appUser;
            }
        }


        public IEmailSenderRepository EmailSender
        {
            get
            {
                if (_emailSender == null)
                {
                    _emailSender = new EmailSenderRepository(_emailConfig);
                }
                return _emailSender;
            }
        }


        public IWorkstationRepository Workstation
        {
            get
            {
                if (_workstation == null)
                {
                    _workstation = new WorkstationRepository(_repoContext, _workstationSortHelper, _roleManager);
                }
                return _workstation;
            }
        }





        public RepositoryWrapper(
            UserManager<AppUser> userManager,
            RoleManager<Workstation> roleManager,
            RepositoryContext repositoryContext,
            EmailConfiguration emailConfig,
            IOptions<EmailSettings> options,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration,
            IConverter converter,
            ISortHelper<AppUser> appUserSortHelper,
            ISortHelper<Banner> bannerSortHelper,
            ISortHelper<Category> categorySortHelper,
            ISortHelper<CategoryBlog> categoryBlogSortHelper,
            ISortHelper<Blog> blogSortHelper,
            ISortHelper<Order> orderSortHelper,
            ISortHelper<Comment> commentSortHelper,
            ISortHelper<SubCategory> subCategorySortHelper,
            ISortHelper<Commercial> commercialSortHelper,
            ISortHelper<Event> eventSortHelper,
            ISortHelper<Payment> paymentSortHelper,
            ISortHelper<Place> placeSortHelper,
            ISortHelper<Sponsor> sponsorSortHelper,
            ISortHelper<Partner> partnerSortHelper,
            ISortHelper<Promote> promoteSortHelper,
            ISortHelper<PromoteEvent> promoteEventSortHelper,
            ISortHelper<AnnualRate> annualRateSortHelper,
            ISortHelper<Workstation> workstationSortHelper,

            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _converter = converter;
            _configuration = configuration;
            _repoContext = repositoryContext;

            _appUserSortHelper = appUserSortHelper;
            _bannerSortHelper = bannerSortHelper;
            _categorySortHelper = categorySortHelper;
            _categoryBlogSortHelper = categoryBlogSortHelper;
            _blogSortHelper = blogSortHelper;
            _orderSortHelper = orderSortHelper;
            _commentSortHelper = commentSortHelper;
            _subCategorySortHelper = subCategorySortHelper;
            _commercialSortHelper = commercialSortHelper;
            _eventSortHelper = eventSortHelper;
            _paymentSortHelper = paymentSortHelper;
            _placeSortHelper = placeSortHelper;
            _sponsorSortHelper = sponsorSortHelper;
            _partnerSortHelper = partnerSortHelper;
            _promoteSortHelper = promoteSortHelper;
            _promoteEventSortHelper = promoteEventSortHelper;
            _annualRateSortHelper = annualRateSortHelper;
            _workstationSortHelper = workstationSortHelper;

            _emailConfig = emailConfig;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task SaveAsync()
        {
            await _repoContext.SaveChangesAsync();
        }
    }
}
