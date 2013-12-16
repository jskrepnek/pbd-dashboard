﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using PBP.Twitter;
using PBP.TwitterHud.Web.App_Start;
using PBP.TwitterHud.Web.Properties;
using PBP.TwitterHud.Web.Services;

namespace PBP.TwitterHud.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            BuildContainer();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        private static void BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof (MvcApplication).Assembly);

            var twitter = new Twitter.Twitter(Settings.Default.consumerKey, Settings.Default.consumerSecret);

            builder.RegisterInstance(twitter).AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<GetPBPTweetsService>()
                .AsImplementedInterfaces()
                .WithParameter("users", Settings.Default.users.Cast<string>().ToArray());

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
