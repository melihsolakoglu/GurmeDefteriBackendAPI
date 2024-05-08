using Microsoft.AspNetCore.Http;

namespace GurmeDefteriWebUI.Services
{
    public class SessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddSession(string sessionKey, string sessionValue)
        {
            _httpContextAccessor.HttpContext.Session.SetString(sessionKey, sessionValue);
        }

        public void RemoveSession(string sessionKey)
        {
            _httpContextAccessor.HttpContext.Session.Remove(sessionKey);
        }

        public string GetSessionValue(string sessionKey)
        {
            return _httpContextAccessor.HttpContext.Session.GetString(sessionKey);
        }
    }
}
