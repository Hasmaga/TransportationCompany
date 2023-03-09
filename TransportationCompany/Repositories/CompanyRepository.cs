using AutoMapper;
using System.Security.Claims;
using TransportationCompany.DbContexts;
using TransportationCompany.Enum;
using TransportationCompany.Model;

namespace TransportationCompany.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CompanyRepository(ApplicationDbContext db, IMapper mapper, ILogger<CompanyRepository> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<PassengerLogin> GetAccountLogin()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            }
            var acc = await _db.PassengerLogins.FindAsync(result);
            if (acc == null)
            {
                throw new Exception(ErrorCode.NOT_AUTHORIZED);
            }
            return acc;
        }       
    }
}
