using AutoMapper;
using Models.DataAccess;
using Models.EntityFramework;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopNetMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Config for AutoMapper
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Grant, GrantRequestDto>()
                    .ForMember(d => d.CreatedAt, option => option.MapFrom(s => s.CreatedAt.ToString()))
                    .ForMember(d => d.UpdatedAt, option => option.MapFrom(s => s.UpdatedAt.ToString()));
                cfg.CreateMap<GrantRequestDto, Grant>();

                cfg.CreateMap<User, UserRequestDto>()
                    .ForMember(d => d.GrantName, option => option.MapFrom(s => s.Grant.GrantName))
                    .ForMember(d => d.CreatedAt, option => option.MapFrom(s => s.CreatedAt.ToString()))
                    .ForMember(d => d.UpdatedAt, option => option.MapFrom(s => s.UpdatedAt.ToString()));
                cfg.CreateMap<UserRequestDto, User>();

                cfg.CreateMap<Product, ProductRequestDto>()
                    .ForMember(d => d.CreatedAt, option => option.MapFrom(s => s.CreatedAt.ToString()))
                    .ForMember(d => d.UpdatedAt, option => option.MapFrom(s => s.UpdatedAt.ToString()));
                cfg.CreateMap<ProductRequestDto, Product>();
                
                cfg.CreateMap<Category, CategoryRequestDto>()
                    .ForMember(d => d.CreatedAt, option => option.MapFrom(s => s.CreatedAt.ToString()))
                    .ForMember(d => d.UpdatedAt, option => option.MapFrom(s => s.UpdatedAt.ToString()));
                cfg.CreateMap<CategoryRequestDto, Category>();

                cfg.CreateMap<Bill, BillRequestDto>()
                    .ForMember(d => d.CreatedAt, option => option.MapFrom(s => s.CreatedAt.ToString()))
                    .ForMember(d => d.UpdatedAt, option => option.MapFrom(s => s.UpdatedAt.ToString()));
                cfg.CreateMap<BillRequestDto, Bill>();

                cfg.CreateMap<Order, OrderRequestDto>()
                    .ForMember(d => d.CreatedAt, option => option.MapFrom(s => s.CreatedAt.ToString()))
                    .ForMember(d => d.UpdatedAt, option => option.MapFrom(s => s.UpdatedAt.ToString()));
                cfg.CreateMap<OrderRequestDto, Order>();

                cfg.CreateMap<Reply, RepliesRequestDto>()
                    .ForMember(d => d.CreatedAt, option => option.MapFrom(s => s.CreatedAt.ToString()))
                    .ForMember(d => d.UpdatedAt, option => option.MapFrom(s => s.UpdatedAt.ToString()));
                cfg.CreateMap<RepliesRequestDto, Reply>();
            });
        }
    }
}