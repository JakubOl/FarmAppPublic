using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Services.ApiFiles
{
    public class PlotProcessor : IPlotProcessor
    {
        private readonly IConfiguration _config;
        public PlotProcessor(IConfiguration config)
        {
            _config = config;
        }
        public async Task<decimal[]> LoadPlot(string city, string plotNumber)
        {
            string apiKey = _config.GetValue<string>("ApiKey");
            string url = $"https://plot.search.api.ongeo.pl/1.0/autocomplete/?api_key={apiKey}&query={city}%20{plotNumber}";

            try
            {
                HttpClient Client = new HttpClient();
                Client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "http://developer.github.com/v3/#user-agent-required");

                List<JObject> resultList = await Client.GetAsync(url).Result.Content.ReadAsAsync<List<JObject>>();

                if (resultList is null) return null;

                var firstResult = resultList[0];

                var longitude = (decimal)firstResult["point"]["coordinates"][0];
                var latitude = (decimal)firstResult["point"]["coordinates"][1];

                return new decimal[] { longitude, latitude };
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
