using Microsoft.AspNetCore.SignalR;
using SblTechTest.Models;
using SblTechTest.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

namespace SblTechTest.Hubs
{
    public interface IDatabaseSubscription
    {
        void Configure(string connectionString); 
    }

    public class  DatabaseSubscription  : IDatabaseSubscription
    {
        private bool disposedValue = false;
        private readonly IDatabaseRepository _repository;
        private readonly IHubContext<BuyerHub> _hubContext;
        private SqlTableDependency<Buyer> _tableDependency;

        public DatabaseSubscription(IDatabaseRepository repository, IHubContext<BuyerHub> hubContext)
        {
            _repository = repository;
            _hubContext = hubContext;
        }

        public void Configure(string connectionString)
        {
            _tableDependency = new  SqlTableDependency<Buyer>(connectionString,"Buyer",null,null,null,null,DmlTriggerType.All,false,false);
            
            _tableDependency.OnChanged += Changed;
            _tableDependency.OnError += TableDependency_OnError;
            _tableDependency.Start();

            Console.WriteLine("Waiting for receiving notifications...");
        }

        private void TableDependency_OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine($"SqlTableDependency error: {e.Error.Message}");
        }

        private void Changed(object sender, RecordChangedEventArgs<Buyer> e)
        {
            if (e.ChangeType != ChangeType.None)
            {
                _hubContext.Clients.All.SendAsync("UpdateBuyer", _repository.Buyers);
            }
        }

        #region IDisposable

        ~DatabaseSubscription()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _tableDependency.Stop();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
