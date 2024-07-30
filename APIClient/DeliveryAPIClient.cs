using DAL.DAO;
using DAL.DB;
using DAL.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIClient
{
    public class DeliveryAPIClient : DeliveryDAO
    {
        RestClient _client;
        public DeliveryAPIClient(string apiResourceUrl) 
        {
            _client = new RestClient(apiResourceUrl);
        }

        public void AddOrder(Delivery order)
        {
            var request = new RestRequest("api/Delivery", Method.Post);
            request.AddJsonBody(order);

            var response = _client.Execute<Delivery>(request);

            if (response.IsSuccessful)
            {
                // Set the OrderNumber from the response
                var createdOrder = response.Data;
                if (createdOrder != null)
                {
                    order.OrderNumber = createdOrder.OrderNumber;
                }
            }
            else
            {
                throw new Exception($"Failed to add order. Error: {response.ErrorMessage}");
            }
        }


        public bool DeleteOrder(int id)
        {
            var request = new RestRequest($"api/Delivery/{id}", Method.Delete);
            var response = _client.Execute<int>(request);
            if (response.IsSuccessful)
            {
                return true;
            }
            else
            {
                throw new Exception($"Failed to delete with ID {id}. Error: {response.ErrorMessage}");
            }
        }

        public IEnumerable<Delivery> GetAllOrders()
        {
            var request = new RestRequest("api/Delivery", Method.Get);
            var response = _client.Execute<List<Delivery>>(request);


            return response.Data;
        }

        public Delivery? GetOrderById(int orderNumber)
        {
            var request = new RestRequest($"api/Order/ {orderNumber}", Method.Get);
            var response = _client.Execute<Delivery>(request);

            return response.Data;
        }

        public bool UpdateOrder(Delivery delivery)
        {
            var request = new RestRequest($"api/Delivery/{delivery.OrderNumber}", Method.Put);
            request.AddJsonBody(delivery);
            var response = _client.Execute(request);
            if (response.IsSuccessful)
            {
                return true;
            }
            else
            {
                throw new Exception($"Failed to update order with OrderNumber {delivery.OrderNumber}. Error: {response.ErrorMessage}");
            }
        }
    }
}
