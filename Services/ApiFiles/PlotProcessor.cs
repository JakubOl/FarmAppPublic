using Newtonsoft.Json.Linq;

namespace Services.ApiFiles
{
    public class PlotProcessor
    {
        public static async Task<decimal[]> LoadPlot(string city, string plotNumber)
        {
            string apiKey = "8ae9a43f-ff83-4868-a80b-dcabe94f2d90";
            string url = $"https://plot.search.api.ongeo.pl/1.0/autocomplete/?api_key={apiKey}&query={city}%20{plotNumber}";

            try
            {
                HttpClient Client = new HttpClient();
                Client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "http://developer.github.com/v3/#user-agent-required");

                List<JObject> resultList = await Client.GetAsync(url).Result.Content.ReadAsAsync<List<JObject>>();

                var firstResult = resultList[0];

                var longitude = (decimal)firstResult["point"]["coordinates"][0];
                var latitude = (decimal)firstResult["point"]["coordinates"][1];

                return new decimal[] { longitude, latitude };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return null;
        }
    }
}
