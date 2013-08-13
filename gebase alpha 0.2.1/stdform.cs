using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace gebase_0._2._2_alpha
{
    public partial class Stdform : XtraForm
    {
        private readonly MainAppForm _mainapp;
        private int _state;

        public Stdform(MainAppForm mainform)
        {
            InitializeComponent();
            _mainapp = mainform;
        }

        /* COMPLETED */

// ReSharper disable once UnusedMember.Local
        private void hphone_Enter()
        {
            hphone.SelectionLength = hphone.Text.Length;
            hphone.SelectionStart = 5;
        }

// ReSharper disable once UnusedMember.Local
        private void mphone_Enter()
        {
                mphone.SelectionLength = mphone.Text.Length;
                mphone.SelectionStart = 0;
        }

// ReSharper disable once UnusedMember.Local
        private void addphone_Enter()
        {
                addphone.SelectionLength = addphone.Text.Length;
                addphone.SelectionStart = 0;
        }

        private void cancelbutton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void studentform_Shown(object sender, EventArgs e)
        {
            fname.Focus();
        }

        private void morebutton_Click(object sender, EventArgs e)
        {
            switch (_state)
            {
                case 0:
                    {
                        Height = 522;
                        _state = 1;
                    }
                    break;
                case 1:
                    {
                        Height = 380;
                        _state = 0;
                    }
                    break;
            }
        }

        /* COMPLETED */

        private void studentform_Load(object sender, EventArgs e)
        {
            switch (Properties.Settings.Default.StdFormType)
            {
                case "add":
                    {
                        Stdcode.MongoInitiate(_mainapp);
                        Stdcode.GroupComboListFill(this);

                        cost.Text = @"15000";
                        acceptdate.DateTime = DateTime.Now.Date;

                        _mainapp.bandedStudentsGridView.Columns[0].Width = 200;
                        editokbutton.Visible = false;
                        addokbutton.Visible = true;
                    }
                    break;
                case "edit":
                    {
                        //stdcode.MongoInitiate(mainapp);
                        Stdcode.GroupComboListFill(this);

                        int rowindex = _mainapp.bandedStudentsGridView.FocusedRowHandle;
                        StdEditFormFill(_mainapp, rowindex);
                        editokbutton.Visible = true;
                        addokbutton.Visible = false;
                    }
                    break;
            }
        }

        private void addokbutton_Click(object sender, EventArgs e)
        {
            var query = Query.EQ("fullname", String.Format("{0} {1}", lname.Text, fname.Text));
            int exist = Convert.ToInt16(Stdcode.Gebase.GetCollection<stdcoll>("stds").Find(query).Count());
            if (exist > 0)
            {
                MessageBox.Show(@"Same user exist! Check fields pls.");
                return;
            }
            
            Stdcode._stdentity = new stdcoll();
            stdcoll stdnew = Stdcode._stdentity;

            {
                stdnew.fname = fname.Text;
                stdnew.lname = lname.Text;
                stdnew.fullname = String.Format("{0} {1}", lname.Text, fname.Text);
                stdnew.email = email.Text;
                stdnew.mphone = mphone.Text;
                stdnew.hphone = hphone.Text;
                stdnew.addphone = addphone.Text;
                stdnew.level = level.Text;
                stdnew.group = stgroup.Text;
                stdnew.cost = Convert.ToInt32(cost.Text);
                stdnew.topay = Convert.ToInt32(cost.Text);
                stdnew.status = status.Text;

                //if (individualcheck.Checked)
                //{
                //    _stdnew.isindividual = true;
                //    _stdnew.isgroup = false;
                //}
                //    else 
                //{
                //    _stdnew.isgroup = true;
                //    _stdnew.isindividual = false;
                //}

                //if (intensivecheck.Checked)
                //    _stdnew.isintensive = true;
                //else _stdnew.isintensive = false;

                stdnew.accepted = acceptdate.DateTime;
                stdnew.daysposs = possdays.Text;
                stdnew.timeposs = posstime.Text;
                stdnew.source = source.Text;
            }

            Stdcode.Stdcollection.Insert(Stdcode._stdentity);
            Stdcode.StdGridRefresh(_mainapp);

            groupcode.MongoInitiate();
            groupcode.GroupStdCount(_mainapp);

            Close();
        }

        private void stgroup_QueryPopUp(object sender, CancelEventArgs e)
        {
            stgroup.Properties.PopupFormMinSize = new Size(25, stgroup.Properties.PopupFormMinSize.Height);
        }

        private void StdEditFormFill(MainAppForm mainapp, int rowindex)
        {
            fname.Text = mainapp.bandedStudentsGridView.GetRowCellValue(rowindex, "fname").ToString();
            lname.Text = mainapp.bandedStudentsGridView.GetRowCellValue(rowindex, "lname").ToString();
            email.Text = mainapp.bandedStudentsGridView.GetRowCellValue(rowindex, "email").ToString();
            mphone.Text = mainapp.bandedStudentsGridView.GetRowCellValue(rowindex, "mphone").ToString();
            hphone.Text = mainapp.bandedStudentsGridView.GetRowCellValue(rowindex, "hphone").ToString();
            addphone.Text = mainapp.bandedStudentsGridView.GetRowCellValue(rowindex, "addphone").ToString();
            // stlevel.Text = mainapp.bandedStudentsGridView.GetRowCellValue(rowindex, "level").ToString();
            stgroup.Text = mainapp.bandedStudentsGridView.GetRowCellValue(rowindex, "group").ToString();
            cost.Text = mainapp.bandedStudentsGridView.GetRowCellValue(rowindex, "cost").ToString();
            status.Text = mainapp.bandedStudentsGridView.GetRowCellValue(rowindex, "status").ToString();

            //if (Convert.ToBoolean(mainapp.bandedStudentsGridView.GetRowCellValue(rowindex, "isindividual")))
            //    individualcheck.Checked = true;
            //if (Convert.ToBoolean(mainapp.bandedStudentsGridView.GetRowCellValue(rowindex, "isintensive")))
            //    intensivecheck.Checked = true;

            acceptdate.DateTime = Convert.ToDateTime(mainapp.bandedStudentsGridView.GetRowCellValue(rowindex, "accepted"));
            //possdays.Text = mainapp.bandedStudentsGridView.GetRowCellValue(rowindex, "daysposs").ToString();
            //posstime.Text = mainapp.bandedStudentsGridView.GetRowCellValue(rowindex, "timeposs").ToString();
            source.Text = mainapp.bandedStudentsGridView.GetRowCellValue(mainapp.bandedStudentsGridView.FocusedRowHandle, "source").ToString();
        }

        private void editokbutton_Click(object sender, EventArgs e)
        {
            string id = _mainapp.bandedStudentsGridView.GetRowCellValue(_mainapp.bandedStudentsGridView.FocusedRowHandle, "_id").ToString();

            Stdcode.Stdcollection.Update(
                Query.EQ("_id", ObjectId.Parse(id)),
                MongoDB.Driver.Builders.Update
                .Set("fname", fname.Text)
                .Set("lname", lname.Text)
                .Set("fullname", String.Format("{0} {1}", lname.Text, fname.Text))
                .Set("email", email.Text)
                .Set("mphone", mphone.Text)
                .Set("hphone", hphone.Text)
                .Set("addphone", addphone.Text)
                //.Set("level", stlevel.Text)
                .Set("group", stgroup.Text)
                .Set("cost", cost.Text)
                .Set("status", status.Text)
                .Set("accepted", acceptdate.DateTime.Date)
                //.Set("daysposs", possdays.Text)
                //.Set("timeposs", posstime.Text)
                .Set("source", source.Text));
                //.Set("isintensive", intensivecheck.Checked)
                //.Set("isindividual", individualcheck.Checked));

            //Properties.Settings.Default.StdFilterFlag = status.Text;
            Stdcode.StdGridRefresh(_mainapp);
            groupcode.MongoInitiate();
            groupcode.GroupStdCount(_mainapp);
            Close();
        }

        private void textEditGdays_Properties_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}