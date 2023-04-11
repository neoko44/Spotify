using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public class Messages
    {
        public static string UserNameNull = "Kullanıcı adı boş";
        public static string PasswordNull = "Şifre boş";
        public static string EmailNull = "Email boş";
        public static string FirstNameNull = "Adınız boş";
        public static string LastNameNull = "Soyadınız boş";
        public static string MailExists = "Mail sistemde mevcut";
        public static string UserNameExists = "Kullanıcı adı sistemde mevcut";
        public static string UserRegistered = "Kullanıcı kayıt oldu";
        public static string AccessTokenCreated = "Access token üretildi";
        public static string RoleAdded = "Rol eklendi";

        public static string AccessTokenError { get; internal set; }
        public static string AccessTokenNotFound { get; internal set; }
    }
}
