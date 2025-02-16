using Identity.Application.Handlers.UserHandlers.RegisterUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.IServices;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterUserCommand command);
}
