using Microsoft.AspNetCore.SignalR;
using SblTechTest.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SblTechTest.Hubs
{
    public class BuyerHub : Hub
    {
        private readonly IDatabaseRepository _repository;

        public BuyerHub(IDatabaseRepository repository)
        {
            _repository = repository;
        }

        public Task BuyTicket(string buyerName, string testerKey)
        {
            _repository.AddBuyer(buyerName, testerKey);
            return Clients.All.SendAsync("UpdateBuyer", _repository.Buyers);
        }

        //public async Task SellProduct(string product, int quantity)
        //{
        //    await _repository.SellProduct(product, quantity);
        //    await Clients.All.SendAsync("UpdateCatalog", _repository.Products);
        //}
    }
}
