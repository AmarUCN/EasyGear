using DAL.DAO;
using DAL.Models;
using RestSharp;
using System;
using APIClient.DTO;
using DAL.DB;

namespace APIClient
{
    public class AccountAPIClient : AccountDAO
    {
        private readonly RestClient _client;

        public AccountAPIClient(string apiResourceUrl)
        {
            _client = new RestClient(apiResourceUrl);
        }

        public int Add(Account newAccount)
        {
            var request = new RestRequest("api/accounts", Method.Post);
            var accountToCreate = new Account
            {
                Email = newAccount.Email,
                Password = newAccount.Password // Plain text
            };

            request.AddJsonBody(accountToCreate);
            var response = _client.Execute<Account>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Created && response.Data != null)
            {
                return response.Data.AccountID;
            }

            throw new Exception("Error adding account");
        }


        public Account? GetById(int accountID)
        {
            var request = new RestRequest($"api/accounts/{accountID}", Method.Get);
            var response = _client.Execute<Account>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK && response.Data != null)
            {
                var account = response.Data;
                return new Account(account.AccountID, account.Email, account.Password);
            }

            return null;
        }


        public Account? Login(string email, string password)
        {
            var request = new RestRequest("api/accounts/login", Method.Post);
            var loginDto = new LoginDto { Email = email, Password = password };

            request.AddJsonBody(loginDto);
            var response = _client.Execute<Account>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK && response.Data != null)
            {
                var account = response.Data;
                return new Account(account.AccountID, account.Email, account.Password);
            }

            return null;
        }

    }
}

