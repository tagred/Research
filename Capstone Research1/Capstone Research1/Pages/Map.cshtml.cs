using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MaxMind.GeoIP2;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Capstone_Research1.Pages
{
    public class Index1Model : PageModel
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public string MapboxAccessToken { get; }
        public string GoogleApiKey { get; }

        public double InitialLatitude { get; set; } = 0;
        public double InitialLongitude { get; set; } = 0;
        public int InitialZoom { get; set; } = 1;

        public Index1Model(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

            MapboxAccessToken = configuration["Mapbox:AccessToken"];
            GoogleApiKey = configuration["google:ApiKey"];
        }

        public void OnGet()
        {
            try
            {
                using (var reader = new DatabaseReader(_hostingEnvironment.WebRootPath + "\\GeoLite2-City.mmdb"))
                {
                    // Determine the IP Address of the request
                    var ipAddress = HttpContext.Connection.RemoteIpAddress;
                    // Get the city from the IP Address
                    var city = reader.City(ipAddress);

                    if (city?.Location?.Latitude != null && city?.Location?.Longitude != null)
                    {
                        InitialLatitude = city.Location.Latitude.Value;
                        InitialLongitude = city.Location.Longitude.Value;
                        InitialZoom = 9;
                    }
                }
            }
            catch (Exception e)
            {
                // Just suppress errors. If we could not retrieve the location for whatever reason
                // there is not reason to notify the user. We'll just simply not know their current
                // location and won't be able to center the map on it
            }
        }

    }
}
