using DAL.DAO;
using DAL.Models;
using RestSharp;
using System.Collections.Generic;

namespace APIClient
{
    public class BasketAPIClient : BasketDAO
    {
        private readonly RestClient _client;

        public BasketAPIClient(string apiResourceUrl)
        {
            _client = new RestClient(apiResourceUrl);
        }

        public void AddBasket(Basket basket)
        {
            var request = new RestRequest("api/basket", Method.Post)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddJsonBody(basket);

            var response = _client.Execute(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error adding basket: {response.Content}");
            }
        }

        public IEnumerable<Basket> GetAllBaskets()
        {
            var request = new RestRequest("api/basket", Method.Get);

            var response = _client.Execute<List<Basket>>(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error retrieving baskets: {response.Content}");
            }
            return response.Data;
        }

        public Basket? GetBasketById(int basketId)
        {
            var request = new RestRequest($"api/basket/{basketId}", Method.Get);

            var response = _client.Execute<Basket>(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error retrieving basket: {response.Content}");
            }
            return response.Data;
        }

        public bool RemoveBasket(int basketId)
        {
            var request = new RestRequest($"api/basket/{basketId}", Method.Delete);

            var response = _client.Execute(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error deleting basket: {response.Content}");
            }
            return response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }
    }
}




