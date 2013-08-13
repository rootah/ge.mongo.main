using System;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraEditors.Controls;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace gebase_0._2._2_alpha
{
    static class Stdcode
    {
        private static string _connectionString;
        private static MongoClient _client;
        private static MongoServer _server;
        public static MongoDatabase Gebase;
        public static MongoCollection<stdcoll> Stdcollection;

// ReSharper disable once InconsistentNaming
        public static stdcoll _stdentity;

        public static void MongoInitiate(MainAppForm mainapp)
        {
            _connectionString = Properties.Settings.Default.ServerOne/* + "," + Properties.Settings.Default.ServerTwo + "/?connect=replicaset"*/;
            _client = new MongoClient(_connectionString);
            _server = _client.GetServer();
            Gebase = _server.GetDatabase("gebase");
            Stdcollection = Gebase.GetCollection<stdcoll>("stds");

            StdGridRefresh(mainapp);
        }

        public static void StdGridRefresh(MainAppForm mainapp)
        {
            //if (mainapp.bandedStudentsGridView.RowCount > 0)
            //{
                var query = Query.EQ("status", Properties.Settings.Default.StdFilterFlag);

                var stdresult = new BindingList<stdcoll>(Stdcollection.Find(query).ToList());
                mainapp.gridStudents.DataSource = stdresult;

                int count = Convert.ToInt16(stdresult.Count());
                mainapp.ItemsCountStatusText.Caption = Convert.ToString(Properties.Settings.Default.StdFilterFlag) + @" students count: " + Convert.ToString(count);
            //}
            //else
            //{
            //    int count = 0;
            //    mainapp.ItemsCountStatusText.Caption = Convert.ToString(Properties.Settings.Default.StdFilterFlag) + " students count: " + Convert.ToString(count);
            //}
        }

        public static void GroupComboListFill(Stdform stdform)
        {
            MongoCollection<groupcolls> groupcoll = Gebase.GetCollection<groupcolls>("groups");

            var result = groupcoll.FindAll()
                 .SetFields(Fields.Include("number"))
                 .ToList();

            stdform.stgroup.Properties.DataSource = result;
            stdform.stgroup.Properties.DisplayMember = "number";
            stdform.stgroup.Properties.ValueMember = "number";
            stdform.stgroup.Properties.ShowHeader = false;

            var col = new LookUpColumnInfo("number", 25)
            {
                SortOrder = DevExpress.Data.ColumnSortOrder.Ascending
            };

            stdform.stgroup.Properties.Columns.Add(col);
        }
        
        public static void StdRemove(MainAppForm mainapp, string id)
        {
            Stdcollection.Remove(
                Query.EQ("_id", ObjectId.Parse(id)));
            StdGridRefresh(mainapp);
        }

// ReSharper disable once UnusedMember.Global
        public static void StdPause(MainAppForm mainapp, string id)
        {
            Stdcollection.Update(Query.EQ("_id", ObjectId.Parse(id)), 
                Update.Set("status", "paused"));

            StdGridRefresh(mainapp);
        }

// ReSharper disable once UnusedMember.Global
        public static void StdResume(MainAppForm mainapp, string id)
        {
            Stdcollection.Update(Query.EQ("_id", ObjectId.Parse(id)),
                Update.Set("status", "active"));

            StdGridRefresh(mainapp);
        }

        public static void StdActionButton(MainAppForm mainapp, string id, string status)
        {
            Stdcollection.Update(Query.EQ("_id", ObjectId.Parse(id)),
                Update.Set("status", status));

            StdGridRefresh(mainapp);
        }

        public static void StdTabShow(MainAppForm mainapp)
        {
            switch (Properties.Settings.Default.StdFilterFlag)
            {
                case "active":
                    mainapp.ActiveStudentsButton.Down = true;
                    StdGridRefresh(mainapp);
                    break;
                case "paused":
                    mainapp.PausedStudentsButton.Down = true;
                    StdGridRefresh(mainapp);
                    break;
                case "awaiting":
                    mainapp.AwaitingStudentsButton.Down = true;
                    StdGridRefresh(mainapp);
                    break;
                case "finished":
                    mainapp.FinishedStudentsButton.Down = true;
                    StdGridRefresh(mainapp);
                    break;
            }
        }

        public static void StdGridColHide(MainAppForm mainapp)
        {
            int i = 0;
            while (i <= 16)
            {
                if (mainapp != null && mainapp.bandedStudentsGridView.Columns.Count > i)
                    mainapp.bandedStudentsGridView.Columns[i].VisibleIndex = -1;
                i++;
            }
        }

        public static void StdActionButtonsSwitch(MainAppForm mainapp, string status)
        {
            switch (status)
            {
                case "active":
                    mainapp.PauseStudentButton.Enabled = true;
                    mainapp.EditStudentButton.Enabled = true;
                    mainapp.ResumeStudentButton.Enabled = false;
                    mainapp.FinishStudentButton.Enabled = true;
                    mainapp.StartStudentButton.Enabled = false;
                    mainapp.DeleteStudentButton.Enabled = true;
                    break;
                case "paused":
                    mainapp.PauseStudentButton.Enabled = false;
                    mainapp.EditStudentButton.Enabled = true;
                    mainapp.ResumeStudentButton.Enabled = true;
                    mainapp.FinishStudentButton.Enabled = false;
                    mainapp.StartStudentButton.Enabled = false;
                    mainapp.DeleteStudentButton.Enabled = true;
                    break;
                case "awaiting":
                    mainapp.PauseStudentButton.Enabled = false;
                    mainapp.EditStudentButton.Enabled = true;
                    mainapp.ResumeStudentButton.Enabled = false;
                    mainapp.FinishStudentButton.Enabled = false;
                    mainapp.StartStudentButton.Enabled = true;
                    mainapp.DeleteStudentButton.Enabled = true;
                    break;
                case "finished":
                    mainapp.PauseStudentButton.Enabled = false;
                    mainapp.EditStudentButton.Enabled = true;
                    mainapp.ResumeStudentButton.Enabled = false;
                    mainapp.FinishStudentButton.Enabled = false;
                    mainapp.StartStudentButton.Enabled = false;
                    mainapp.DeleteStudentButton.Enabled = true;
                    break;
            }
        }

        public static void GroupDetails(MainAppForm mainapp)
        {
            _connectionString = Properties.Settings.Default.ServerOne/* + "," + Properties.Settings.Default.ServerTwo + "/?connect=replicaset"*/;
            _client = new MongoClient(_connectionString);
            _server = _client.GetServer();
            Gebase = _server.GetDatabase("gebase");

            try
            {
                string number = Properties.Settings.Default.CurrentGroupNumber;

                string paneltext;
                if (number == "")
                    paneltext = "nothing selected";
                else paneltext = "group " + number + " details";

                mainapp.bandedDetailGroupGridView.GroupPanelText = paneltext;

                Stdcollection = Gebase.GetCollection<stdcoll>("stds");
                var query = Query.EQ("group", number);

                var stddetails = new BindingList<stdcoll>(Stdcollection.Find(query).ToList());
                mainapp.gridGroupDetail.DataSource = stddetails;
            }
            catch (Exception)
            {
                const string paneltext = @"nothing selected";
                mainapp.bandedDetailGroupGridView.GroupPanelText = paneltext;
            }
        }
    }
}
