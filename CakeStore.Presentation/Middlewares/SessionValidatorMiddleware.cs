using CakeStore.Domain.Entities.Identity;
using CakeStore.Infrastructure.DAL.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Shared.Common.Helpers;
using Shared.Data.Abstraction;

namespace CakeStoreApp.Middlewares;

public class SessionValidatorMiddleware : IAuthorizationMiddlewareResultHandler
{
    private readonly IAuthorizationMiddlewareResultHandler _handler;
    private readonly IRepository<UserSession, Guid, IdentityContext> _sessionsRepository;


    public SessionValidatorMiddleware(IRepository<UserSession, Guid, IdentityContext> sessionsRepository)
    {
        _handler = new AuthorizationMiddlewareResultHandler();
        _sessionsRepository = sessionsRepository;
    }

    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (!authorizeResult.Forbidden || authorizeResult.Challenged)
        {
            if (context.User.Identity is { IsAuthenticated: false })
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var sessionId = Guid.Parse(context.User.Claims
                .FirstOrDefault(w => w.Type == Constants.JwtClaimIdentifiers.SessionId)!.Value);

            var userSession = await _sessionsRepository.FirstOrDefaultAsync(w => w.Id == sessionId 
                && w.StatusId == 1 || w.IsActive);

            if (userSession == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }

        await _handler.HandleAsync(next, context, policy, authorizeResult);
    }
}