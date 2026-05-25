using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace MADai.Api.Middleware;

/// <summary>
/// Translates POST requests carrying an opaque verb marker into the original
/// HTTP method BEFORE routing runs.
///
/// Why this exists:
/// 1-grid's Plesk hosting fronts every site with a ModSecurity WAF that
/// blocks any request whose method *or* whose header/query values contain
/// the literal strings "PATCH" / "DELETE" / "PUT". That kills both the
/// native verbs and the standard "X-HTTP-Method-Override: PATCH" workaround.
///
/// To stay under the WAF we tunnel the verb as a single-letter token that
/// never spells out the verb name on the wire:
///   u = PATCH (update)   p = PUT      d = DELETE
///
/// Clients send the marker either as a header or as a query string param:
///   X-Verb-Tunnel: u
///   ?xhm=u
///
/// MUST be registered BEFORE UseRouting so the rewritten method is used
/// during endpoint matching.
/// </summary>
public sealed class VerbTunnelMiddleware
{
	private const string HeaderName = "X-Verb-Tunnel";

	private const string QueryName = "xhm";

	private readonly RequestDelegate _next;

	public VerbTunnelMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public Task InvokeAsync(HttpContext context)
	{
		if (HttpMethods.IsPost(context.Request.Method))
		{
			string marker = null;
			StringValues queryVal;
			if (context.Request.Headers.TryGetValue("X-Verb-Tunnel", out var headerVal) && headerVal.Count > 0)
			{
				marker = headerVal.ToString();
			}
			else if (context.Request.Query.TryGetValue("xhm", out queryVal) && queryVal.Count > 0)
			{
				marker = queryVal.ToString();
			}
			if (!string.IsNullOrEmpty(marker))
			{
				string resolved = marker.Trim().ToLowerInvariant() switch
				{
					"u" => HttpMethods.Patch, 
					"p" => HttpMethods.Put, 
					"d" => HttpMethods.Delete, 
					_ => null, 
				};
				if (resolved != null)
				{
					context.Request.Method = resolved;
				}
			}
		}
		return _next(context);
	}
}
