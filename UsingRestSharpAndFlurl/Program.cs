using Flurl;
using Flurl.Http;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UsingRestSharpAndFlurl
{
    public class Program
    {
        private static void Main()
        {
            FlurlPostCode.GetPostcode().GetAwaiter().GetResult();

            FlurlPostCode.GetPostcodes().GetAwaiter().GetResult();

            FlurlPostCode.GetAstronomyPictureOfTheDay().GetAwaiter().GetResult();

            FlurlPostCode.GetGithubData().GetAwaiter().GetResult();

            RestSharpPostCode.GetAstronomyPictureOfTheDay();

            RestSharpPostCode.GetGithubFeed();

            RestSharpPostCode.GetPostcode();

            RestSharpPostCode.GetPostcodes();
        }

        private class FlurlPostCode
        {
            public static async Task GetPostcode()
            {
                var singleGeocodeResponse = await "https://api.postcodes.io"
                    .AppendPathSegment("postcodes")
                    .AppendPathSegment("IP1 3JR")
                    .GetJsonAsync();

                Console.WriteLine(singleGeocodeResponse);
            }

            public static async Task GetAstronomyPictureOfTheDay()
            {
                var singleGeocodeResponse = await "https://api.nasa.gov/"
                    .AppendPathSegments("planetary", "apod")
                    .SetQueryParam("api_key", "DEMO_KEY")
                    .GetJsonAsync();

                Console.WriteLine(singleGeocodeResponse);
            }


            public static async Task GetGithubData()
            {
                var singleGeocodeResponse = await "https://api.github.com/"
                    .AppendPathSegments("users", "jeremylindsayni")
                    .WithBasicAuth("jeremylindsayni", "<<my password>>")
                    .WithHeader("user-agent", "csharp-console-app")
                    .GetJsonAsync();

                Console.WriteLine(singleGeocodeResponse);
            }

            public static async Task GetPostcodes()
            {
                var postcodes = new PostCodeCollection { Postcodes = new List<string> { "OX49 5NU", "M32 0JG" } };

                var url = await "https://api.postcodes.io"
                    .AppendPathSegment("postcodes")
                    .PostJsonAsync(postcodes)
                    .ReceiveJson<GeocodeResponseCollection>();

                Console.WriteLine(url);
            }
        }

        private class RestSharpPostCode
        {
            public static void GetPostcode()
            {
                // instantiate the RestClient with the base API url
                var client = new RestClient("https://api.postcodes.io");

                // specify the resource, e.g. https://api.postcodes.io/postcodes/OX495NU
                var getRequest = new RestRequest("postcodes/{postcode}");
                getRequest.AddUrlSegment("postcode", "OX495NU");

                // send the GET request and return an object which contains the API's JSON response
                var singleGeocodeResponseContainer = client.Execute(getRequest);

                // get the API's JSON response
                var singleGeocodeResponse = singleGeocodeResponseContainer.Content;

            }

            public static void GetAstronomyPictureOfTheDay()
            {
                // instantiate the RestClient with the base API url
                var client = new RestClient("https://api.nasa.gov/");

                // specify the resource, e.g. https://api.nasa.gov/planetary/apod
                var getRequest = new RestRequest("planetary/apod");

                // Add the authentication key which NASA expects to be passed as a parameter
                getRequest.AddQueryParameter("api_key", "DEMO_KEY");

                // send the GET request and return an object which contains the API's JSON response
                var pictureOfTheDayResponseContainer = client.Execute(getRequest);

                // get the API's JSON response
                var pictureOfTheDayJson = pictureOfTheDayResponseContainer.Content;
            }

            public static void GetGithubFeed()
            {
                // instantiate the RestClient with the base API url
                var client = new RestClient("https://api.github.com/");

                // pass in user id and password 
                client.Authenticator = new HttpBasicAuthenticator("jeremylindsayni", "[[my password]]");

                // specify the resource that requires authentication, e.g. https://api.github.com/users/jeremylindsayni
                var getRequest = new RestRequest("users/jeremylindsayni");

                // send the GET request and return an object which contains the API's JSON response
                var response = client.Execute(getRequest);
            }

            public static void GetPostcodes()
            {
                // instantiate the RestClient with the base API url
                var client = new RestClient("https://api.postcodes.io");

                // specify the resource, e.g. https://api.postcodes.io/postcodes
                var postRequest = new RestRequest("postcodes", Method.POST, DataFormat.Json);

                // instantiate and hydrate a POCO object with the list postcodes we want geocode data for
                var postcodes = new RestSharpPostCodeCollection { postcodes = new List<string> { "IP1 3JR", "M32 0JG" } };

                // add this POCO object to the request body, RestSharp automatically serialises it to JSON
                postRequest.AddJsonBody(postcodes);

                // send the POST request and return an object which contains a strongly typed response
                var bulkGeocodeResponseContainer = client.Execute<GeocodeResponseCollection>(postRequest);

                // get the strongly typed response
                var bulkGeocodeResponse = bulkGeocodeResponseContainer.Data;
            }
        }
    }
}