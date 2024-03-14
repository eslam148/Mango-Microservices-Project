using AutoMapper;
 
namespace Mango.Services.AuthAuthAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                //config.CreateMap<CouponDto, Coupon>();
                //config.CreateMap<Coupon, CouponDto>();

            });
            return mappingConfig;
        }
    }
}
