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


            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

        }
    }
}
