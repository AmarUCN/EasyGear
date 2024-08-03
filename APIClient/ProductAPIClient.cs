using DAL.DAO;
using DAL.DB;
using DAL.Models;
using RestSharp;
using System;
using System.Collections.Generic;

namespace APIClient
{
    public class ProductAPIClient : ProductDAO
    {
        RestClient _client;

        public ProductAPIClient(string apiResourceUrl)
        {
            _client = new RestClient(apiResourceUrl);
        }

        public void AddProduct(Product product)
        {
            var request = new RestRequest("api/Product", Method.Post);
            request.AddJsonBody(product);

            var response = _client.Execute(request);
            
        }

        public bool DeleteProduct(int productID)
        {
            var request = new RestRequest($"api/Product/{productID}", Method.Delete);
            var response = _client.Execute<int>(request);
            if (response.IsSuccessful)
            {
                return true;
            }
            else
            {
                throw new Exception($"Failed to delete with ID {productID}. Error: {response.ErrorMessage}");
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            var request = new RestRequest("api/Product", Method.Get);
            var response = _client.Execute<List<Product>>(request);


            return response.Data;
        }

        public Product GetProductById(int productID)
        {
            var request = new RestRequest($"api/Product/{productID}", Method.Get);
            var response = _client.Execute<Product>(request);

            return response.Data;
        }

        public bool UpdateProduct(Product product)
        {
            var request = new RestRequest($"api/Product/{product.Id}", Method.Put);
            request.AddJsonBody(product);

            var response = _client.Execute(request);

            if (response.IsSuccessful)
            {
                return true;
            }
            else
            {
                throw new Exception($"Failed to delete with ID {product}. Error: {response.ErrorMessage}");
            }
        }
    }
}
