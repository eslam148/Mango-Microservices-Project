﻿namespace Mango.Web.Utitlity
{
    public class SD
    {
        public static string CouponApiBase {  get; set; }
        public static string AuthApiBase {  get; set; }

        public enum ApiType
        { 
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
