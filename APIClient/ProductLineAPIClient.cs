using DAL.DAO;
using DAL.Models;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIClient
{
    public class ProductLineAPIClient : ProductLineDAO
    {
        private readonly RestClient _client;

        public ProductLineAPIClient(string apiResourceUrl)
        {
            _client = new RestClient(apiResourceUrl);
        }

        // Create a new ProductLine
        public void AddProductLine(ProductLine productLine)
        {
            var request = new RestRequest("api/productline", Method.Post);
            request.AddJsonBody(productLine);

            var response = _client.Execute(request);

            if (response.IsSuccessful)
            {
                // Optionally handle response if needed
            }
            else
            {
                throw new Exception($"Failed to create ProductLine: {response.Content}");
            }
        }

        // Delete a ProductLine
        public bool DeleteProductLine(int id)
        {
            var request = new RestRequest($"api/productline/{id}", Method.Delete);

            var response = _client.Execute(request);

            if (response.IsSuccessful)
            {
                return true;
            }
            else
            {
                // Optionally handle error if needed
                throw new Exception($"Failed to delete ProductLine: {response.Content}");
            }
        }

        // Get all ProductLines
        public IEnumerable<ProductLine> GetAllProductLines()
        {
            var request = new RestRequest("api/productline", Method.Get);

            var response = _client.Execute<IEnumerable<ProductLine>>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Failed to retrieve ProductLines: {response.Content}");
            }
        }

        // Get a ProductLine by ID
        public ProductLine  ? GetProductLineById(int id)
        {
            var request = new RestRequest($"api/productline/{id}", Method.Get);

            var response = _client.Execute<ProductLine>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Failed to retrieve ProductLine: {response.Content}");
            }
        }

        // Update a ProductLine
        public bool UpdateProductLine(ProductLine productLine)
        {
            var request = new RestRequest($"api/productline/{productLine.Id}", Method.Put);
            request.AddJsonBody(productLine);

            var response = _client.Execute(request);

            if (response.IsSuccessful)
            {
                return true;
            }
            else
            {
                throw new Exception($"Failed to update ProductLine: {response.Content}");
            }
        }
    }
}


