using Contracts;
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
        private ICategoryBlogRepository _categoryBlogRepository;
        private IBlogRepository _blogRepository;
        private ICommandRepository _commandRepository;
        private ICommentRepository _commentRepository;
        private ICommercialRepository _commercialRepository;
        private ICategoryRepository _categoryRepository;
        private IEventRepository _eventRepository;
        private IPaymentRepository _paymentRepository;
        private IPaymentTypeRepository _paymentTypeRepository;
        private IPlaceRepository _placeRepository;
        private ISponsorRepository _sponsorRepository;

        private IAppUserRepository _appUser;
        private IMailRepository _mail;
        private IWebHostEnvironment _webHostEnvironment;
        private IWorkstationRepository _workstation;

        private readonly IConfiguration _configuration;
        private IHttpContextAccessor _httpContextAccessor;
        private IOptions<EmailSettings> _emailSettings;

        private readonly ISortHelper<CategoryBlog> _categoryBlogSortHelper;
        private readonly ISortHelper<Blog> _blogSortHelper;
        private readonly ISortHelper<Command> _commandSortHelper;
        private readonly ISortHelper<Comment> _commentSortHelper;
        private readonly ISortHelper<Commercial> _commercialSortHelper;
        private readonly ISortHelper<Category> _categorySortHelper;
        private readonly ISortHelper<Event> _eventSortHelper;
        private readonly ISortHelper<Payment> _paymentSortHelper;
        private readonly ISortHelper<PaymentType> _paymentTypeSortHelper;
        private readonly ISortHelper<Place> _placeSortHelper;
        private readonly ISortHelper<Sponsor> _sponsorSortHelper;


        private RepositoryContext _repoContext;
        private UserManager<AppUser> _userManager;
        private RoleManager<Workstation> _roleManager;

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

        public ICommandRepository Command
        {
            get
            {
                if (_commandRepository == null)
                {
                    _commandRepository = new CommandRepository(_repoContext, _commandSortHelper);
                }
                return _commandRepository;
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


        public IPaymentTypeRepository PaymentType
        {
            get
            {
                if (_paymentTypeRepository == null)
                {
                    _paymentTypeRepository = new PaymentTypeRepository(_repoContext, _paymentTypeSortHelper);
                }
                return _paymentTypeRepository;
            }
        }

        public IPlaceRepository Place
        {
            get
            {
                if (_placeRepository == null)
                {
                    _placeRepository = new PlaceRepository(_repoContext, _placeSortHelper);
                }
                return _placeRepository;
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
                    _appUser = new AppUserRepository(_repoContext, _userManager);
                }
                return _appUser;
            }
        }


        public IMailRepository Mail
        {
            get
            {
                if (_mail == null)
                {
                    _mail = new MailRepository(_emailSettings);
                }
                return _mail;
            }
        }

        public IWorkstationRepository Workstation
        {
            get
            {
                if (_workstation == null)
                {
                    _workstation = new WorkstationRepository(_repoContext, _roleManager);
                }
                return _workstation;
            }
        }




        public RepositoryWrapper(
            UserManager<AppUser> userManager,
            RoleManager<Workstation> roleManager,
            RepositoryContext repositoryContext,
            IOptions<EmailSettings> options,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration,
            ISortHelper<Category> categorySortHelper,
            ISortHelper<CategoryBlog> categoryBlogSortHelper,
            ISortHelper<Blog> blogSortHelper,
            ISortHelper<Command> commandSortHelper,
            ISortHelper<Comment> commentSortHelper,
            ISortHelper<Commercial> commercialSortHelper,
            ISortHelper<Event> eventSortHelper,
            ISortHelper<Payment> paymentSortHelper,
            ISortHelper<PaymentType> paymentTypeSortHelper,
            ISortHelper<Place> placeSortHelper,
            ISortHelper<Sponsor> sponsorSortHelper,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _repoContext = repositoryContext;

            _categorySortHelper = categorySortHelper;
            _categoryBlogSortHelper = categoryBlogSortHelper;
            _blogSortHelper = blogSortHelper;
            _commandSortHelper = commandSortHelper;
            _commentSortHelper = commentSortHelper;
            _commercialSortHelper = commercialSortHelper;
            _eventSortHelper = eventSortHelper;
            _paymentSortHelper = paymentSortHelper;
            _placeSortHelper = placeSortHelper;
            _paymentTypeSortHelper = paymentTypeSortHelper;
            _sponsorSortHelper = sponsorSortHelper;
            

            _emailSettings = options;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task SaveAsync()
        {
            await _repoContext.SaveChangesAsync();
        }
    }
}
