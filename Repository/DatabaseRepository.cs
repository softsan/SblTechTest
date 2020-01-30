using SblTechTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SblTechTest.Repository
{
    public interface IDatabaseRepository
    {
        IEnumerable<Buyer> Buyers { get; }

        Event Events { get; }
        Task AddBuyer(string buyerName, string testerkey);
 
    }

    public class DatabaseRepository : IDatabaseRepository
    {
        private Func<dbInterviewContext> _contextFactory;

        public IEnumerable<Buyer> Buyers => GetBuyer();

        public Event Events => GetEvent();
        public DatabaseRepository(Func<dbInterviewContext> context)
        {
            _contextFactory = context;
        }

        public Task AddBuyer(string buyerName,string testerkey)
        {
            using (var context = _contextFactory.Invoke())
            {
                if (context.Buyer.Any(x => x.TesterKey == testerkey))
                {
                    var currentBuyer = context.Buyer.FirstOrDefault(x => x.TesterKey == testerkey);
                    context.Update(currentBuyer);
                }
                else
                {
                    context.Add(new Buyer { BuyerName = buyerName, EventId = 1, TesterKey= testerkey });
                }

                context.SaveChanges();
            }

            return Task.FromResult(true);
        }

       

        private IEnumerable<Buyer> GetBuyer()
        {
            using (var context = _contextFactory.Invoke())
            {
                return context.Buyer.ToList();
            }
        }

        
        private  Event GetEvent()
        {
            using (var context = _contextFactory.Invoke())
            {
                return context.Event.FirstOrDefault(e=>e.EventId==1);
            }
             
        }
    }
}
