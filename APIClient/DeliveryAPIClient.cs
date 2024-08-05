using DAL.DAO;
using DAL.Models;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIClient
{
    public class DeliveryAPIClient : DeliveryDAO
    {
        private readonly RestClient _client;

        public DeliveryAPIClient(string apiResourceUrl)
        {
            _client = new RestClient(apiResourceUrl);
        }

        public int AddOrder(Delivery delivery)
        {
            var request = new RestRequest("api/delivery", Method.Post)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddJsonBody(delivery);

            var response = _client.Execute(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Error adding delivery: {response.Content}");
            }

            // Extract and return the ID from the Location header or response content
            var locationHeader = response.Headers.FirstOrDefault(h => h.Name.Equals("Location", StringComparison.OrdinalIgnoreCase));
            if (locationHeader != null)
            {
                var location = locationHeader.Value.ToString();
                var idString = location.Split('/').Last(); // Extract ID from URL
                if (int.TryParse(idString, out var id))
                {
                    return id;
                }
            }

            throw new Exception("Unable to retrieve the ID of the newly created delivery.");
        }


        public bool DeleteOrder(int id)
        {
            var request = new RestRequest($"api/delivery/{id}", Method.Delete);

            var response = _client.Execute(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error deleting delivery: {response.Content}");
            }
            return response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }

        public IEnumerable<Delivery> GetAllOrders()
        {
            var request = new RestRequest("api/delivery", Method.Get);

            var response = _client.Execute<List<Delivery>>(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error retrieving deliveries: {response.Content}");
            }
            return response.Data;
        }

        public Delivery? GetOrderById(int id)
        {
            var request = new RestRequest($"api/delivery/{id}", Method.Get);

            var response = _client.Execute<Delivery>(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error retrieving delivery: {response.Content}");
            }
            return response.Data;
        }

        public bool UpdateOrder(Delivery delivery)
        {
            var request = new RestRequest($"api/delivery/{delivery.Id}", Method.Put)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddJsonBody(delivery);

            var response = _client.Execute(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error updating delivery: {response.Content}");
            }
            return response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }
    }
}


