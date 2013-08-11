﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

namespace gebase_alpha_0._2._1
{
    public partial class stdform : XtraForm
    {
        private readonly MainAppForm mainapp;
        public int state = 0;

        public stdform(MainAppForm mainform)
        {
            InitializeComponent();
            mainapp = mainform;
        }

        /* COMPLETED */

        private void hphone_Enter(object sender, EventArgs e)
        {
            //hphone.BeginInvoke(new MethodInvoker(delegate
            //{
                hphone.SelectionLength = hphone.Text.Length;
                hphone.SelectionStart = 5;
            //}));
        }

        private void mphone_Enter(object sender, EventArgs e)
        {
                mphone.SelectionLength = mphone.Text.Length;
                mphone.SelectionStart = 0;
        }

        private void addphone_Enter(object sender, EventArgs e)
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
            switch (state)
            {
                case 0:
                    {
                        Height = 522;
                        state = 1;
                    }
                    break;
                case 1:
                    {
                        Height = 380;
                        state = 0;
                    }
                    break;
                default:
                    break;
            }
        }

        /* COMPLETED */

        private void studentform_Load(object sender, EventArgs e)
        {
            switch (gebase_0._2._2_alpha.Properties.Settings.Default.StdFormType)
            {
                case "add":
                    {
                        stdcode.MongoInitiate(mainapp);
                        stdcode.GroupComboListFill(this);

                        cost.Text = "15000";
                        acceptdate.DateTime = DateTime.Now.Date;

                        mainapp.bandedStudentsGridView.Columns[0].Width = 200;
                        editokbutton.Visible = false;
                        addokbutton.Visible = true;
                    }
                    break;
                case "edit":
                    {
                        //stdcode.MongoInitiate(mainapp);
                        stdcode.GroupComboListFill(this);

                        int rowindex = mainapp.bandedStudentsGridView.FocusedRowHandle;
                        StdEditFormFill(mainapp, rowindex);
                        editokbutton.Visible = true;
                        addokbutton.Visible = false;
                    }
                    break;
                default:
                    break;
            }
        }

        private void addokbutton_Click(object sender, EventArgs e)
        {
            var query = Query.EQ("fullname", String.Format("{0} {1}", lname.Text, fname.Text));
            int exist = Convert.ToInt16(stdcode.gebase.GetCollection<stdcoll>("stds").Find(query).Count());
            if (exist > 0)
            {
                MessageBox.Show("Same user exist! Check fields pls.");
                return;
            }
            
            stdcode._stdentity = new stdcoll();
            stdcoll _stdnew = stdcode._stdentity;

            {
                _stdnew.fname = fname.Text;
                _stdnew.lname = lname.Text;
                _stdnew.fullname = String.Format("{0} {1}", lname.Text, fname.Text);
                _stdnew.email = email.Text;
                _stdnew.mphone = mphone.Text;
                _stdnew.hphone = hphone.Text;
                _stdnew.addphone = addphone.Text;
                _stdnew.level = level.Text;
                _stdnew.group = stgroup.Text;
                _stdnew.cost = Convert.ToInt32(cost.Text);
                _stdnew.topay = Convert.ToInt32(cost.Text);
                _stdnew.status = status.Text;

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

                _stdnew.accepted = acceptdate.DateTime;
                _stdnew.daysposs = possdays.Text;
                _stdnew.timeposs = posstime.Text;
                _stdnew.source = source.Text;
            }

            stdcode.stdcollection.Insert(stdcode._stdentity);
            stdcode.StdGridRefresh(mainapp);

            groupcode.MongoInitiate();
            groupcode.GroupStdCount(mainapp);

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
            string _id = mainapp.bandedStudentsGridView.GetRowCellValue(mainapp.bandedStudentsGridView.FocusedRowHandle, "_id").ToString();

            stdcode.stdcollection.Update(
                Query.EQ("_id", ObjectId.Parse(_id)),
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
            stdcode.StdGridRefresh(mainapp);
            groupcode.MongoInitiate();
            groupcode.GroupStdCount(mainapp);
            Close();
        }

        private void textEditGdays_Properties_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}