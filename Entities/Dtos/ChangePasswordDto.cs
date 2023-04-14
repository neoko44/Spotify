using Core.Entities;
using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class ChangePasswordDto:IDto
    {
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        //public string Token { get; set; }
    }
}
