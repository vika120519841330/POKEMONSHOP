using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POKEMONSHOP.Data
{
    public class CurrentAppUser: ComponentBase
    {
        [Inject]
        public IHttpContextAccessor httpContextAccessor { get; set; }

        /// <summary>
        /// Метод для получения текущего пользователя из контекста запроса или из куки
        /// </summary>
        public string GetCurrentUser()
        {
            string user = httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? string.Empty;

            // Если AppUser не был считан из контекста запроса, то получить его из куки
            if (user.Length <= 0)
            {
                string userName = string.Empty;

                bool? trygetUserName = this.httpContextAccessor?.HttpContext?.Request?.Cookies?.TryGetValue("UserName", out userName);

                if (userName != null && userName.Length > 0)
                {
                    user = userName;
                }
                else
                {
                    user = string.Empty;
                }
            }

            return user;
        }
    }
}
