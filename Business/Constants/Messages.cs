using Core.Entities.Concrete;
using Entities.Concrete;
using Entities.Concrete.JsonEntity;
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

        public static string AccessTokenError = "Token Hatası";
        public static string AccessTokenNotFound = "Token Bulunamadı";
        public static string LibraryCreated = "Kitaplık Üretildi";
        public static string NewPassMustDifferent = "Yeni şifreniz eskisiyle aynı olamaz";
        public static string PassChangeSuccess = "Şifre değiştirme başarılı";
        public static string PasswordNotMatch = "Şifreler Eşleşmiyor";
        public static string UserNotFound = "Kullanıcı Bulunamadı";
        public static string PasswordError = "Şifre Hatalı";
        public static string SuccessfulLogin = "Giriş Başarılı";

        public static string PlayListTracksGetSuccess = "Parça listesi başarıyla alındı";
        public static string Failed = "Hata";

        public static string TrackNotFound = "Parça bulunamadı";
        public static string TrackAddedToFavorites = "Parça beğenilenlere eklendi";

        public static string TrackAlreadyExists = "Parça zaten eklenmiş";

        public static string LibraryNotFound = "Kütüphane Bulunamadı";
        public static string PlaylistCreated = "Çalma listesi oluşturuldu";

        public static string PlaylistAlreadyExists = "Çalma listesi zaten mevcut";

        public static string PlaylistCreatedAndTrackAdded = "Çalma listesi oluşturuldu ve parça eklendi";
        public static string AddedToPlaylist = "Çalma listesine eklendi";

        public static string PlaylistNotFound = "Çalma listesi bulunamadı";

        public static string NameMustBeDifferent = "Ad eskisiyle aynı";

        public static string SuccessfullyUpdated = "Başarıyla Güncellendi";

        public static string AlbumNotFound = "Albüm bulunamadı";
        public static string AlbumAdded = "Albüm çalma listesine eklendi";
        public static string AlbumGetSuccess = "Albüm getirme başarılı";

        public static string PlaylistDeleted = "Çalma listesi silindi";

        public static string TrackRemovedFromFavorites = "Parça favorilerden silindi";

        public static string PlaylistAlreadyFollowing = "Çalma listesi zaten takip ediliyor";

        public static string PlaylistFollowed = "Çalma listesi arıtk takip ediliyor";

        public static string CantFollowYourself = "Kendi çalma listenizi takip edemezsiniz";

        public static string PlaylistUnfollowed = "Çalma listesi takip listesinden çıkarıldı";
    }
}
