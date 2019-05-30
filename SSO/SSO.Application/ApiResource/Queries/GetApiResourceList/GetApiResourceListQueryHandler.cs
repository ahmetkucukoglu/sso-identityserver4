namespace SSO.Application.ApiResource.Queries.GetApiResourceList
{
    using Domain.EntityFramework;
    using MediatR;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetApiResourceListQueryHandler : IRequestHandler<GetApiResourceListQuery, IEnumerable<ApiResourceListItem>>
    {
        private readonly AuthDbContext _authContext;

        public GetApiResourceListQueryHandler(AuthDbContext authContext)
        {
            _authContext = authContext;
        }

        public Task<IEnumerable<ApiResourceListItem>> Handle(GetApiResourceListQuery request, CancellationToken cancellationToken)
        {
            var apiResources = _authContext.ApiResources.ToList();

            var result = apiResources.Select((x) => new ApiResourceListItem
            {
                Description = x.Description,
                DisplayName = x.DisplayName,
                Enabled = x.Enabled,
                Name = x.Name
            });

            return Task.FromResult(result);
        }
    }
}
