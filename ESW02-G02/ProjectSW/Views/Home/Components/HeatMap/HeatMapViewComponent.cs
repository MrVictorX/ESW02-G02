using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectSW.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace ProjectSW.Models
{
    [ViewComponent(Name = "HeatMap")]
    public class HeatMapViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;


        public HeatMapViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Location> locations = new List<Location>();

            foreach (Adopter a in await _context.Adopter.ToListAsync())
            {

                locations.Add(GetLocationRequest(a.PostalCode));
            }

            return View(locations);
        }

        private Location GetLocationRequest(string postalcode)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://api.geonames.org/postalCodeLookupJSON?postalcode=" + postalcode + "&country=PT&username=tesing_software");
            request.Method = Http.Get;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new ApplicationException("Error: Response status " + response.StatusCode);
                }

                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            var locations = JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());
                            Location location = new Location { Lat = locations.postalcodes[0].lat, Lng = locations.postalcodes[0].lng };
                            return location;
                        }
                    }//end of reader
                }//End of stream
            }//End of response

            return null;
        }
    }
}
