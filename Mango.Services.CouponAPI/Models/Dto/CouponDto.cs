namespace Mango.Services.CouponAPI.Models.Dto
{
    public class CouponDto
    {
        public int CouponId { get; set; }
        public string Couponcode { get; set; }
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }

    }
}
