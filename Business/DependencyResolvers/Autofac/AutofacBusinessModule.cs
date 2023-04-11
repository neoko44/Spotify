using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Abstract;
using Core.Entities.Concrete;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();


            builder.RegisterType<ClaimManager>().As<IClaimService>();
            builder.RegisterType<EfUserOperationDal>().As<IUserOperationDal>();


            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

        }
    }
}
