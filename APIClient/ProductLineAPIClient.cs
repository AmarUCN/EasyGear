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
    public class ProductLineAPIClient : ProductLineDAO
    {
        RestClient _client;

        public ProductLineAPIClient(string apiResourceUrl)
        {
            _client = new RestClient(apiResourceUrl);
        }


        public void AddProductLine(ProductLine productLine)
        {
            var request = new RestRequest("api/ProductLine", Method.Post);
            request.AddJsonBody(productLine);

            var response = _client.Execute(request);
        }

        public bool DeleteProductLine(int productLineID)
        {
            var request = new RestRequest($"api/ProductLine/{productLineID}", Method.Delete);
            var response = _client.Execute<int>(request);
            if (response.IsSuccessful)
            {
                return true;
            }
            else
            {
                throw new Exception($"Failed to delete with ID {productLineID}. Error: {response.ErrorMessage}");
            }
        }

        public IEnumerable<ProductLine> GetAllProductLines()
        {
            throw new NotImplementedException();
        }

        public ProductLine? GetProductLineById(int productLineID)
        {
            var request = new RestRequest($"api/ProductLine/ {productLineID}", Method.Get);
            var response = _client.Execute<ProductLine>(request);

            return response.Data;
        }

        public bool UpdateProductLine(ProductLine productLine)
        {
            throw new NotImplementedException();
        }
    }
}
