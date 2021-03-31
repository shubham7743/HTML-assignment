using HMS.BAL;
using HMS.BAL.Interface;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace HMS.WebApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IHotelManager, HotelManager>();
            container.RegisterType<IRoomManager, RoomManager>();
            container.RegisterType<IBookingManager, BookingManager>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}