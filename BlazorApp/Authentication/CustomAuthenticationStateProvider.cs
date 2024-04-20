using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace BlazorApp.Authentication
{
	public class CustomAuthenticationStateProvider : AuthenticationStateProvider
	{
		private readonly ProtectedSessionStorage _sessionStorage;
		private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

		public CustomAuthenticationStateProvider(ProtectedSessionStorage sessionStorage)
		{
			_sessionStorage = sessionStorage;
		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			try
			{
				var userSessionStorageResult = await _sessionStorage.GetAsync<UserSession>("UserSession");
				var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;

				if (userSession == null)
					return await Task.FromResult(new AuthenticationState(_anonymous));

				var claims = new Claim[]
				{
					new(ClaimTypes.NameIdentifier, userSession.Id.ToString()),
					new(ClaimTypes.Name, userSession.Name),
					new(ClaimTypes.Role, userSession.Role),
					new("CompanyId", userSession.CompanyId.ToString() ?? ""),
					new("CompanyName", userSession.CompanyName ?? "")
				};

				var identity = new ClaimsIdentity(claims, "CustomAuth");
				var claimsPrinicipal = new ClaimsPrincipal(identity);
				return await Task.FromResult(new AuthenticationState(claimsPrinicipal));
			}
			catch
			{
				return await Task.FromResult(new AuthenticationState(_anonymous));
			}
		}

		public async Task UpdateAuthenticationState(UserSession? userSession)
		{
			ClaimsPrincipal claimsPrincipal;

			if (userSession != null)
			{
				await _sessionStorage.SetAsync("UserSession", userSession);
				var claims = new Claim[]
				{
					new(ClaimTypes.NameIdentifier, userSession.Id.ToString()),
					new(ClaimTypes.Name, userSession.Name),
					new(ClaimTypes.Role, userSession.Role),
					new("CompanyId", userSession.CompanyId.ToString() ?? ""),
					new("CompanyName", userSession.CompanyName ?? "")
				};
				var identity = new ClaimsIdentity(claims, "CustomAuth");
				claimsPrincipal = new ClaimsPrincipal(identity);
			}
			else
			{
				await _sessionStorage.DeleteAsync("UserSession");
				claimsPrincipal = _anonymous;
			}

			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
		}
	}
}
