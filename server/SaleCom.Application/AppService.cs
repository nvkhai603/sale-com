using AutoMapper;
using Microsoft.AspNetCore.Http;
using Nvk.Data;
using SaleCom.Application.Contracts;

namespace SaleCom.Application
{
    public abstract class AppService: IAppService
    {
        protected IMapper _mapper => _lazyServiceProvider.LazyGetRequiredService<IMapper>();
        protected ICurrentUser _currentUser => _lazyServiceProvider.LazyGetService<ICurrentUser>();
        protected IHttpContextAccessor _httpContextAccessor => _lazyServiceProvider.LazyGetRequiredService<IHttpContextAccessor>();
        protected ILazyServiceProvider _lazyServiceProvider;
        public AppService(ILazyServiceProvider lazyServiceProvider)
        {
            _lazyServiceProvider = lazyServiceProvider;
        }
    }
}
