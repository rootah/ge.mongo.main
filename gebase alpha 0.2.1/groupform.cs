using System;
using System.Linq;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace gebase_0._2._2_alpha
{
    public partial class Groupform : DevExpress.XtraEditors.XtraForm
    {
        private int _state;
        private readonly MainAppForm _mainapp;

        private const string ConnectionString = "mongodb://localhost";
        private static readonly MongoClient Client = new MongoClient(ConnectionString);
        private static readonly MongoServer Server = Client.GetServer();
        private static readonly MongoDatabase Gebase = Server.GetDatabase("gebase");
        readonly MongoCollection<groupcolls> _groupcollection = Gebase.GetCollection<groupcolls>("groups");

        private groupcolls _groupentity;

        public Groupform(MainAppForm mainform)
        {
            InitializeComponent();
            _mainapp = mainform;
        }

        private void groupform_Load(object sender, EventArgs e)
        {
            switch (Properties.Settings.Default.GroupFormType)
            {
                case "add":
                    {
                        Text = @"group [add]";
                        simpleButtonEdit.Visible = false;
                        simpleButtonOk.Visible = true;
                        NewGroupNumber();
                    }
                    break;
                case "edit":
                    {
                        //if (mainapp._groupentity.time == "... custom")
                        if (_mainapp.bandedGroupGridView.GetRowCellValue(_mainapp.bandedGroupGridView.FocusedRowHandle, "time").ToString() == "... custom")
                        {
                            textEditGtime.EditValue = null;
                            Height = 361; _state = 1;
                        }
                        else
                        {
                            textEditGtime.Time = Convert.ToDateTime(_mainapp.bandedGroupGridView.GetRowCellValue(_mainapp.bandedGroupGridView.FocusedRowHandle, "time").ToString());
                            Height = 224; _state = 0;
                        }

                        Text = string.Format(@"group [edit : {0}]", Convert.ToString(_mainapp.bandedGroupGridView.GetRowCellValue(_mainapp.bandedGroupGridView.FocusedRowHandle, "number").ToString()));
                        simpleButtonEdit.Visible = true;
                        simpleButtonOk.Visible = false;

                        textEditGnum.Text = _mainapp.bandedGroupGridView.GetRowCellValue(_mainapp.bandedGroupGridView.FocusedRowHandle, "number").ToString();
                        textEditGteacher.Text = _mainapp.bandedGroupGridView.GetRowCellValue(_mainapp.bandedGroupGridView.FocusedRowHandle, "teacher").ToString();
                        textEditGlevel.Text = _mainapp.bandedGroupGridView.GetRowCellValue(_mainapp.bandedGroupGridView.FocusedRowHandle, "level").ToString();
                        textEditGdays.Text = _mainapp.bandedGroupGridView.GetRowCellValue(_mainapp.bandedGroupGridView.FocusedRowHandle, "days").ToString();
                        checkedhours.Text = _mainapp.bandedGroupGridView.GetRowCellValue(_mainapp.bandedGroupGridView.FocusedRowHandle, "hours").ToString();
                        textEditGstatus.Text = _mainapp.bandedGroupGridView.GetRowCellValue(_mainapp.bandedGroupGridView.FocusedRowHandle, "status").ToString();
                        textEditGstart.Text = _mainapp.bandedGroupGridView.GetRowCellValue(_mainapp.bandedGroupGridView.FocusedRowHandle, "start").ToString();

                        textEditGdays_Properties_EditValueChanged(null, null);
                        CustomTimeGet();
                    }
                    break;
            }
        }

        private void CustomTimeButton_Click(object sender, EventArgs e)
        {
            switch (Properties.Settings.Default.GroupFormType)
            {
                case "edit":
                    {
                        switch (_state)
                        {
                            case 0:
                                {
                                    textEditGtime.EditValue = null;

                                    Height = 361; _state = 1;
                                    CustomTimeGet();
                                }
                                break;

                            case 1:
                                {
                                    Height = 224; _state = 0;
                                    if (_mainapp.bandedGroupGridView.GetRowCellValue(_mainapp.bandedGroupGridView.FocusedRowHandle, "time").ToString() != "... custom")
                                        textEditGtime.Time = Convert.ToDateTime(_mainapp.bandedGroupGridView.GetRowCellValue(_mainapp.bandedGroupGridView.FocusedRowHandle, "time").ToString());
                                    else textEditGtime.EditValue = null;
                                }
                                break;
                        }
                    }
                    break;
                case "add":
                    {
                        switch (_state)
                        {
                            case 0:
                                {
                                    textEditGtime.EditValue = null;

                                    Height = 361; _state = 1;
                                }
                                break;

                            case 1:
                                {
                                    Height = 224; _state = 0;
                                }
                                break;
                        }
                    }
                    break;
            }
        }

        private void simpleButtonEdit_Click(object sender, EventArgs e)
        {
            string gtime;
            string groupkind;

            if (textEditGtime.EditValue == null)
            {
                gtime = "... custom";
                groupkind = "customtime";
            }
            else
            {
                gtime = textEditGtime.Time.ToShortTimeString();
                groupkind = "sametime";
            }

            Gebase.GetCollection<groupcolls>("groups").Update(
                Query.EQ("_id", ObjectId.Parse(_mainapp.bandedGroupGridView.GetRowCellValue(_mainapp.bandedGroupGridView.FocusedRowHandle, "_id").ToString())),
                MongoDB.Driver.Builders.Update
                .Set("teacher", textEditGteacher.Text)
                .Set("number", textEditGnum.Text)
                .Set("level", textEditGlevel.Text)
                .Set("days", textEditGdays.Text)
                .Set("hours", Convert.ToInt16(checkedhours.Text))
                .Set("start", textEditGstart.DateTime.ToShortDateString())
                .Set("status", textEditGstatus.Text)
                .Set("time", gtime)
                .Set("kind", groupkind)
                //.Set("suntime", suntime.Time.ToShortTimeString())
                //.Set("montime", montime.Time.ToShortTimeString())
                //.Set("tuetime", tuetime.Time.ToShortTimeString())
                //.Set("wedtime", wedtime.Time.ToShortTimeString())
                //.Set("thutime", thutime.Time.ToShortTimeString())
                //.Set("fritime", fritime.Time.ToShortTimeString())
                //.Set("sattime", sattime.Time.ToShortTimeString())

                .Set("suntime", suntime.Time.ToShortTimeString())
                .Set("montime", montime.Time.ToShortTimeString())
                .Set("tuetime", tuetime.Time.ToShortTimeString())
                .Set("wedtime", wedtime.Time.ToShortTimeString())
                .Set("thutime", thutime.Time.ToShortTimeString())
                .Set("fritime", fritime.Time.ToShortTimeString())
                .Set("sattime", sattime.Time.ToShortTimeString())
                );

            groupcode.GroupGridRefresh(_mainapp);
            _mainapp.bandedGroupGridView.FocusedRowHandle = Properties.Settings.Default.GroupSelectedRowIndex;
            _mainapp.StatusEventsText.Caption = @"Group " + textEditGnum.Text + @" edited";
            _mainapp.StatusEventsText.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            Close();
        }

        private void NewGroupNumber()
        {
            Query.Exists("number");
                int notnull = Convert.ToInt16(_groupcollection.FindAll().Count());
                var sortDescending = SortBy.Descending("number");

                if (notnull > 0)
                {
                    var maxresult = _groupcollection.FindAll()
                         .SetFields(Fields.Include("number")) // include name field
                         .SetSortOrder(sortDescending).SetLimit(1).First();

                    string oldfullnum = maxresult.number;
                    int index = oldfullnum.IndexOf("-", StringComparison.Ordinal);
                    int firsthalfoldnum = Convert.ToInt16(oldfullnum.Substring(0, index));
                    int firsthalfnewnum = firsthalfoldnum + 1;
                    string lasthalfoldnum = oldfullnum.Substring(index + 1, 2);
                    string newfullnum = Convert.ToString(firsthalfnewnum) + "-" + lasthalfoldnum;

                    textEditGnum.Text = newfullnum;
                }
                else textEditGnum.Text = "";
        }

        private void textEditGdays_Properties_EditValueChanged(object sender, EventArgs e)
        {
            if (textEditGdays.Properties.Items["mon"].CheckState == CheckState.Checked)
            {
                monlabel.Enabled = true;
                montime.Enabled = true;
            }
            else
            {
                monlabel.Enabled = false;
                montime.Enabled = false;
            }

            if (textEditGdays.Properties.Items["tue"].CheckState == CheckState.Checked)
            {
                tuelabel.Enabled = true;
                tuetime.Enabled = true;
            }
            else
            {
                tuelabel.Enabled = false;
                tuetime.Enabled = false;
            }

            if (textEditGdays.Properties.Items["wed"].CheckState == CheckState.Checked)
            {
                wedlabel.Enabled = true;
                wedtime.Enabled = true;
            }
            else
            {
                wedlabel.Enabled = false;
                wedtime.Enabled = false;
            }

            if (textEditGdays.Properties.Items["thu"].CheckState == CheckState.Checked)
            {
                thulabel.Enabled = true;
                thutime.Enabled = true;
            }
            else
            {
                thulabel.Enabled = false;
                thutime.Enabled = false;
            }

            if (textEditGdays.Properties.Items["fri"].CheckState == CheckState.Checked)
            {
                frilabel.Enabled = true;
                fritime.Enabled = true;
            }
            else
            {
                frilabel.Enabled = false;
                fritime.Enabled = false;
            }

            if (textEditGdays.Properties.Items["sat"].CheckState == CheckState.Checked)
            {
                satlabel.Enabled = true;
                sattime.Enabled = true;
            }
            else
            {
                satlabel.Enabled = false;
                sattime.Enabled = false;
            }

            if (textEditGdays.Properties.Items["sun"].CheckState == CheckState.Checked)
            {
                sunlabel.Enabled = true;
                suntime.Enabled = true;
            }
            else
            {
                sunlabel.Enabled = false;
                suntime.Enabled = false;
            }
        }

        private void simpleButtonOk_Click(object sender, EventArgs e)
        {
            string gtime;
            string groupkind;

            if (textEditGtime.EditValue == null)
            {
                gtime = "... custom";
                groupkind = "customtime";
            }
            else
            {
                gtime = textEditGtime.Time.ToShortTimeString();
                groupkind = "sametime";
            }
            
            int count = Convert.ToInt16(_groupcollection.FindAs<groupcolls>(Query.EQ("number", textEditGnum.Text)).Count());
            if (count > 0)
            {
                MessageBox.Show(@"Same group number exist!");
            }
            else
            {
                _groupentity = new groupcolls();
                //stdcolls _stdnew = stdcode._stdentity;

                _groupentity = new groupcolls
                {
                    teacher = textEditGteacher.Text,
                    number = textEditGnum.Text,
                    level = textEditGlevel.Text,
                    days = textEditGdays.Text,
                    hours = Convert.ToInt16(checkedhours.Text),
                    start = textEditGstart.DateTime.ToShortDateString(),
                    status = textEditGstatus.Text,
                    time = gtime,
                    kind = groupkind,
                    suntime = suntime.Time.ToShortTimeString(),
                    montime = montime.Time.ToShortTimeString(),
                    tuetime = tuetime.Time.ToShortTimeString(),
                    wedtime = wedtime.Time.ToShortTimeString(),
                    thutime = thutime.Time.ToShortTimeString(),
                    fritime = fritime.Time.ToShortTimeString(),
                    sattime = sattime.Time.ToShortTimeString()
                };
                //{
                //};

                _groupcollection.Insert(_groupentity);
                groupcode.GroupGridRefresh(_mainapp);
                
                _mainapp.bandedGroupGridView.FocusedRowHandle = Properties.Settings.Default.GroupSelectedRowIndex;
                _mainapp.StatusEventsText.Caption = @"Group " + textEditGnum.Text + @" created";
                _mainapp.StatusEventsText.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                //groupcode.StatusTextRefresh(mainapp);
                Close();
            }
        }

        private void textEditGtime_EditValueChanged(object sender, EventArgs e)
        {
            if (suntime.Enabled)
                suntime.Time = textEditGtime.Time;
            if (montime.Enabled)
                montime.Time = textEditGtime.Time;
            if (tuetime.Enabled)
                tuetime.Time = textEditGtime.Time;
            if (wedtime.Enabled)
                wedtime.Time = textEditGtime.Time;
            if (thutime.Enabled)
                thutime.Time = textEditGtime.Time;
            if (fritime.Enabled)
                fritime.Time = textEditGtime.Time;
            if (sattime.Enabled)
                sattime.Time = textEditGtime.Time;
        }

        private void CustomTimeGet()
        {
            if (suntime.Enabled)
            {
                suntime.Time = Convert.ToDateTime(_mainapp.bandedGroupGridView.GetRowCellValue(_mainapp.bandedGroupGridView.FocusedRowHandle, "suntime").ToString());
            }
            if (montime.Enabled)
            {
                montime.Time = Convert.ToDateTime(_mainapp.bandedGroupGridView.GetRowCellValue(_mainapp.bandedGroupGridView.FocusedRowHandle, "montime").ToString());
            }
            if (tuetime.Enabled)
            {
                tuetime.Time = Convert.ToDateTime(_mainapp.bandedGroupGridView.GetRowCellValue(_mainapp.bandedGroupGridView.FocusedRowHandle, "tuetime").ToString());
            }
            if (wedtime.Enabled)
            {
                wedtime.Time = Convert.ToDateTime(_mainapp.bandedGroupGridView.GetRowCellValue(_mainapp.bandedGroupGridView.FocusedRowHandle, "wedtime").ToString());
            }
            if (thutime.Enabled)
            {
                thutime.Time = Convert.ToDateTime(_mainapp.bandedGroupGridView.GetRowCellValue(_mainapp.bandedGroupGridView.FocusedRowHandle, "thutime").ToString());
            }
            if (fritime.Enabled)
            {
                fritime.Time = Convert.ToDateTime(_mainapp.bandedGroupGridView.GetRowCellValue(_mainapp.bandedGroupGridView.FocusedRowHandle, "fritime").ToString());
            }
            if (sattime.Enabled)
            {
                sattime.Time = Convert.ToDateTime(_mainapp.bandedGroupGridView.GetRowCellValue(_mainapp.bandedGroupGridView.FocusedRowHandle, "sattime").ToString());
            }
        }
    }
}