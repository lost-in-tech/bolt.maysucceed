using Bolt.Endeavor.Extensions.Bus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Bolt.Endeavor.Extensions.Mvc;

internal sealed class CurrentTenantProvider(
    IHttpContextAccessor context, 
    IDataKeySettings options) 
    : ICurrentTenantProvider
{
    public string Get()
    {
        if (context.HttpContext == null) return string.Empty;

        var tenant = context.HttpContext.GetRouteValue(options.TenantRouteName)?.ToString();

        if (!string.IsNullOrWhiteSpace(tenant)) return tenant;
        
        if(context.HttpContext.Request.Query.TryGetValue(options.TenantQueryName, out var qTenant))
        {
            return qTenant.ToString();
        }

        if (context.HttpContext.Request.Headers.TryGetValue(options.TenantHeaderName, out var hTenant))
        {
            return hTenant.ToString();
        }

        return string.Empty;
        
        
        
    }
}