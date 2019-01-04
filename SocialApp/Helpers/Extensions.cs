using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SocialApp.Domain;

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

        public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            var textFormater = new JsonSerializerSettings();
            textFormater.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, textFormater));
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
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
