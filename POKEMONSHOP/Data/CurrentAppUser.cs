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

        public string AppUser { get; set; }

        public CurrentAppUser()
        {   }

        /// <summary>
        /// Метод для получения текущего пользователя из контекста запроса или из куки
        /// </summary>
        public void GetCurrentUser()
        {
            this.AppUser = httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? string.Empty;

            // Если AppUser не был считан из контекста запроса, то получить его из куки
            if (this.AppUser.Length <= 0)
            {
                string userName = string.Empty;

                bool? trygetUserName = this.httpContextAccessor?.HttpContext?.Request?.Cookies?.TryGetValue("UserName", out userName);

                if (userName != null && userName.Length > 0)
                {
                    this.AppUser = userName;
                }
                else
                {
                    this.AppUser = string.Empty;
                }
            }
        }

    }
}
