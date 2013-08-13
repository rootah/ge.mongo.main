using System.ComponentModel;
using System.Linq;
using MongoDB.Driver;

namespace gebase_0._2._2_alpha
{
    static class Paycode
    {
        private static string _connectionString;
        private static MongoClient _client;
        private static MongoServer _server;
        private static MongoDatabase _gebase;
        private static MongoCollection<stdcoll> _stdcollection;

        public static void MongoInitiate(MainAppForm mainapp)
        {
            _connectionString = Properties.Settings.Default.ServerOne/* + "," + Properties.Settings.Default.ServerTwo + "/?connect=replicaset"*/;
            _client = new MongoClient(_connectionString);
            _server = _client.GetServer();
            _gebase = _server.GetDatabase("gebase");
            _stdcollection = _gebase.GetCollection<stdcoll>("stds");

            PayGridRefresh(mainapp);
        }

// ReSharper disable once UnusedMember.Global
        private static void PayGridRefresh(MainAppForm mainapp)
        {
            var payresult = new BindingList<stdcoll>(_stdcollection.FindAll().ToList());
            mainapp.gridPayments.DataSource = payresult;
            PayColHide(mainapp);
        }

        private static void PayColHide(MainAppForm mainapp)
        {
            int i = 0;
            while (i <= 16)
            {
                mainapp.bandedPaymentsGridView.Columns[i].VisibleIndex = -1;
                i++;
            }

            int j = 19;

            while (j <= 22)
            {
                mainapp.bandedPaymentsGridView.Columns[j].VisibleIndex = -1;
                j++;
            }

            mainapp.bandedPaymentsGridView.Columns["group"].Width = 5;
        }
    }
}
