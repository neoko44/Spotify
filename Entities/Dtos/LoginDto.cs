using Core.Entities.Abstract;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class LoginDto:IDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
