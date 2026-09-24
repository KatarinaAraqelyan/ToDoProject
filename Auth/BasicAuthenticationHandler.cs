using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore; 
using Microsoft.Extensions.Options;
using ToDoProject.Data;

namespace ToDoProject.Auth;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "Basic";

    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string? header = Request.Headers.Authorization;
        if (string.IsNullOrWhiteSpace(header))
        {
            return AuthenticateResult.NoResult();
        }

        if (!AuthenticationHeaderValue.TryParse(header, out AuthenticationHeaderValue? parsed))
        {
            return AuthenticateResult.NoResult();
        }

        if (!string.Equals(parsed.Scheme, SchemeName, StringComparison.OrdinalIgnoreCase)
            || string.IsNullOrWhiteSpace(parsed.Parameter))
        {
            return AuthenticateResult.NoResult();
        }

        string decoded;
        try
        {
            decoded = Encoding.UTF8.GetString(Convert.FromBase64String(parsed.Parameter));
        }
        catch
        {
            return AuthenticateResult.Fail("Invalid Base64 string.");
        }

        int split = decoded.IndexOf(':');
        if (split < 1)
        {
            return AuthenticateResult.Fail("Invalid Basic credentials.");
        }

        string userName = decoded.Substring(0, split);
        string password = decoded.Substring(split + 1);

        var db = Context.RequestServices.GetRequiredService<ToDoContext>();

        var user = await db.Users
            .FirstOrDefaultAsync(u => u.UserName == userName, Context.RequestAborted);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return AuthenticateResult.Fail("Invalid username or password.");
        }

        var identity = new ClaimsIdentity(
            new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            },
            Scheme.Name);

        return AuthenticateResult.Success(
            new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name));
    }
}