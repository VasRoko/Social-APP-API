using System;
using Microsoft.AspNetCore.Http;

namespace SocialApp.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static int CalculateAge(this DateTime dataTime)
        {
            var age = DateTime.Today.Year - dataTime.Year;

            if(dataTime.AddYears(age) > DateTime.Today)
            {
                age--;
            }

            return age;
        }
    }
}
