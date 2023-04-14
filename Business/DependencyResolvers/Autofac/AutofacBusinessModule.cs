using Autofac;
using Business.Abstract;
using Business.Concrete;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            builder.RegisterType<ClaimManager>().As<IClaimService>();
            builder.RegisterType<EfUserOperationDal>().As<IUserOperationDal>();

            builder.RegisterType<GetTokenManager>().As<IGetTokenService>();

            builder.RegisterType<GetAccessTokenManager>().As<IGetAccessTokenService>();
            builder.RegisterType<EfGetAccessTokenDal>().As<IGetAccessTokenDal>();

            builder.RegisterType<LibraryManager>().As<ILibraryService>();
            builder.RegisterType<EfLibraryDal>().As<ILibraryDal>();

            builder.RegisterType<FavoriteManager>().As<IFavoriteService>();
            builder.RegisterType<EfFavoriteDal>().As<IFavoriteDal>();

            builder.RegisterType<UserFavoriteManager>().As<IUserFavoriteService>();
            builder.RegisterType<EfUserFavoriteDal>().As<IUserFavoriteDal>();

            builder.RegisterType<UserLibraryManager>().As<IUserLibraryService>();
            builder.RegisterType<EfUserLibraryDal>().As<IUserLibraryDal>();

            builder.RegisterType<TrackPoolManager>().As<ITrackPoolService>();

            builder.RegisterType<UserPlaylistManager>().As<IUserPlaylistService>();
            builder.RegisterType<EfUserPlaylistDal>().As<IUserPlaylistDal>();

            builder.RegisterType<PlaylistManager>().As<IPlaylistService>();
            builder.RegisterType<EfPlaylistDal>().As<IPlaylistDal>();

            builder.RegisterType<FollowManager>().As<IFollowService>();
            builder.RegisterType<EfFollowDal>().As<IFollowDal>();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

        }
    }
}
