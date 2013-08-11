using System.ComponentModel;
using System.Linq;
using MongoDB.Driver;

namespace gebase_0._2._2_alpha
{
    static class Paycode
    {
        public static string ConnectionString;
        public static MongoClient Client;
        public static MongoServer Server;
        public static MongoDatabase Gebase;
        public static MongoCollection<stdcoll> Stdcollection;

        public static void MongoInitiate(MainAppForm mainapp)
        {
            ConnectionString = gebase_0._2._2_alpha.Properties.Settings.Default.ServerOne/* + "," + Properties.Settings.Default.ServerTwo + "/?connect=replicaset"*/;
            Client = new MongoClient(ConnectionString);
            Server = Client.GetServer();
            Gebase = Server.GetDatabase("gebase");
            Stdcollection = Gebase.GetCollection<stdcoll>("stds");

            // *** doesn't work!!! PayGridRefresh(mainapp);
        }

        public static void PayGridRefresh(MainAppForm mainapp)
        {
            //var query = Query.EQ("status", Properties.Settings.Default.StdFilterFlag);

            var payresult = new BindingList<stdcoll>(Stdcollection.FindAll().ToList());
            mainapp.gridPayments.DataSource = payresult;
            PayColHide(mainapp);
        }

        public static void PayColHide(MainAppForm mainapp)
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
