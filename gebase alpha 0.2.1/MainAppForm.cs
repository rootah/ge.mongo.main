using System;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;

namespace gebase_0._2._2_alpha
{
    public partial class MainAppForm : RibbonForm
    {
        /*  MONGO VARS DECLARE */

        /*  END MONGO VARS DECLARE  */

        public MainAppForm()
        {
            InitializeComponent();
        }

        private void backstageViewButtonItem1_ItemClick(object sender, BackstageViewItemEventArgs e) /* COMPL */
        {
            Application.Exit();
        }

        private void ribbonControl_SelectedPageChanged(object sender, EventArgs e) /* COMPL */
        {
            if (ribbonControl.SelectedPage.Text == @"Payments")
            {
                JRN.Visible = true;
                Properties.Settings.Default.RibbonTabIndex = ribbonControl.SelectedPage.PageIndex;
                xtraTabControl1.SelectedTabPageIndex = ribbonControl.SelectedPage.PageIndex;
            }
            else if (ribbonControl.SelectedPage.Text == @"incoming")
            {
                xtraTabControl1.SelectedTabPageIndex = 4;
            }
            else if (ribbonControl.SelectedPage.Text == @"outgoing")
            {
                xtraTabControl1.SelectedTabPageIndex = 4;
            }
            else
            {
                JRN.Visible = false;
                Properties.Settings.Default.RibbonTabIndex = ribbonControl.SelectedPage.PageIndex;
                xtraTabControl1.SelectedTabPageIndex = ribbonControl.SelectedPage.PageIndex;
            }

            Properties.Settings.Default.Save();
            RibbonTabRefresh(); 
        }

        private void RibbonTabRefresh()
        {
            try
            {
                ribbonControl.SelectedPage = ribbonControl.Pages[Properties.Settings.Default.RibbonTabIndex];
            }
            catch
            {
                ribbonControl.SelectedPage = ribbonControl.Pages[0];
            }

            switch (Properties.Settings.Default.RibbonTabIndex)
            {
                case 0:
                    {
                        groupcode.MongoInitiate();
                        groupcode.GroupTabShow(this);
                        //groupcode.GroupDetailView(this);
                        groupcode.GroupGridColHide(this);
                        bandedGroupGridView.FocusedRowHandle = 0;
                        bandedGroupGridView_FocusedRowChanged(null, null);
                    }
                    break;
                case 1:
                    {
                        stdcode.MongoInitiate(this);
                        stdcode.StdTabShow(this);
                        stdcode.StdGridColHide(this);
                        bandedStudentsGridView.FocusedRowHandle = 0;
                        bandedStudentsGridView_FocusedRowChanged(null, null);
                    }
                    break;
                case 2:
                    {
                        SchedGrid.GoToToday();
                    }
                    break;
                case 3:
                    {
                        Paycode.MongoInitiate(this);
                    }
                    break;
            }
        }

        private void MainAppForm_Load(object sender, EventArgs e)
        {
            RibbonTabRefresh();
            xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
        }

        private void ActiveGroupButton_DownChanged(object sender, ItemClickEventArgs e)
        {            
            Properties.Settings.Default.GroupFilterFlag = "active";
            //Properties.Settings.Default.CurrentGroupStatus = "active";
            Properties.Settings.Default.Save();
            groupcode.GroupGridRefresh(this);
            
            //bandedGroupGridView.FocusedRowHandle = 0;

            bandedGroupGridView_FocusedRowChanged(null, null);
        }

        private void AwaitingGroupButton_DownChanged(object sender, ItemClickEventArgs e)
        {
            Properties.Settings.Default.GroupFilterFlag = "awaiting";
            //Properties.Settings.Default.CurrentGroupStatus = "awaiting";
            Properties.Settings.Default.Save();
            groupcode.GroupGridRefresh(this);
            
            //bandedGroupGridView.FocusedRowHandle = 0;

            bandedGroupGridView_FocusedRowChanged(null, null);
        }

        private void PausedGroupButton_DownChanged(object sender, ItemClickEventArgs e)
        {
            Properties.Settings.Default.GroupFilterFlag = "paused";
            //Properties.Settings.Default.CurrentGroupStatus = "paused";
            Properties.Settings.Default.Save();
            groupcode.GroupGridRefresh(this);
            bandedGroupGridView_FocusedRowChanged(null, null);
        }

        private void FinishedGroupButton_DownChanged(object sender, ItemClickEventArgs e)
        {
            Properties.Settings.Default.GroupFilterFlag = "finished";
            //Properties.Settings.Default.CurrentGroupStatus = "finished";
            Properties.Settings.Default.Save();
            groupcode.GroupGridRefresh(this);
            bandedGroupGridView_FocusedRowChanged(null, null);
        }

        private void PauseGroupButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            //int rowhandle = bandedGroupGridView.FocusedRowHandle;
            string id = bandedGroupGridView.GetRowCellValue(bandedGroupGridView.FocusedRowHandle, "_id").ToString();
            string number = bandedGroupGridView.GetRowCellValue(bandedGroupGridView.FocusedRowHandle, "number").ToString();
            groupcode.GroupAction(this, id, "paused", number);
        }

        private void ResumeGroupButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            //int rowhandle = bandedGroupGridView.FocusedRowHandle;
            string id = bandedGroupGridView.GetRowCellValue(bandedGroupGridView.FocusedRowHandle, "_id").ToString();
            string number = bandedGroupGridView.GetRowCellValue(bandedGroupGridView.FocusedRowHandle, "number").ToString();
            groupcode.GroupAction(this, id, "active", number);
        }

        private void StartGroupButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            //int rowhandle = bandedGroupGridView.FocusedRowHandle;
            string id = bandedGroupGridView.GetRowCellValue(bandedGroupGridView.FocusedRowHandle, "_id").ToString();
            string number = bandedGroupGridView.GetRowCellValue(bandedGroupGridView.FocusedRowHandle, "number").ToString();
            groupcode.GroupAction(this, id, "active", number);
        }

        private void FinishGroupButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            //int rowhandle = bandedGroupGridView.FocusedRowHandle;
            string id = bandedGroupGridView.GetRowCellValue(bandedGroupGridView.FocusedRowHandle, "_id").ToString();
            string number = bandedGroupGridView.GetRowCellValue(bandedGroupGridView.FocusedRowHandle, "number").ToString();
            groupcode.GroupAction(this, id, "finished", number);
        }

        private void DeleteGroupButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            string id = bandedGroupGridView.GetRowCellValue(bandedGroupGridView.FocusedRowHandle, "_id").ToString();
            string number = bandedGroupGridView.GetRowCellValue(bandedGroupGridView.FocusedRowHandle, "number").ToString();

            if (MessageBox.Show(String.Format("Group {0} will be deleted", number), @"Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                groupcode.GroupDel(this, id);
            }
        }

        private void AddGroupButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            Properties.Settings.Default.GroupFormType = "add";
            using (var groupadd = new groupform(this) { StartPosition = FormStartPosition.CenterParent })
            {
                groupadd.simpleButtonOk.Visible = true;
                groupadd.simpleButtonEdit.Visible = false;
                groupadd.ShowDialog();
            }
        }

        private void EditGroupButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            Properties.Settings.Default.GroupFormType = "edit";
            using (var groupadd = new groupform(this) { StartPosition = FormStartPosition.CenterParent })
            {
                groupadd.simpleButtonOk.Visible = true;
                groupadd.simpleButtonEdit.Visible = false;
                groupadd.ShowDialog();
            }
        }

        private void barButtonItem2_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            //Appointment lesson = GETimeTable.Storage.CreateAppointment(AppointmentType.Normal);
            //lesson.Start = Convert.ToDateTime(_groupentity.time);
            //lesson.Duration = TimeSpan.FromMinutes(_groupentity.hours * 45);
            //lesson.Subject = _groupentity.number;
            //lesson.Description = _groupentity.teacher;
            //GETimeTable.Storage.Appointments.Add(lesson);

            //MongoCollection<schedulercoll> schedcollection = gebase.GetCollection<schedulercoll>("sched");
            //var schedresult = schedcollection.FindAll().ToList();
            //SchedGrid.GoToToday();
           
            //GESchedulerStorage.Appointments.DataSource = schedresult;
            //GESchedulerStorage.Appointments.Mappings.AllDay = "AllDay";
            //GESchedulerStorage.Appointments.Mappings.Start = "StartTime";
            //GESchedulerStorage.Appointments.Mappings.End = "EndTime";
            //GESchedulerStorage.Appointments.Mappings.Subject = "Name";
            //GESchedulerStorage.Appointments.Mappings.Label = "Label";
            //GESchedulerStorage.Appointments.Mappings.Location = "Location";
            //GESchedulerStorage.Appointments.Mappings.RecurrenceInfo = "RecurrenceInfo";
            //GESchedulerStorage.Appointments.Mappings.ReminderInfo = "ReminderInfo";
            //GESchedulerStorage.Appointments.Mappings.Description = "Description";

            //var query = Query.EQ("status", Properties.Settings.Default.GroupFilterFlag);

        }

        /* STUDENTS TAB */

        private void AddStudentsButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            Properties.Settings.Default.StdFormType = "add";
            var stdadd = new stdform(this) {StartPosition = FormStartPosition.CenterParent};
            stdadd.Text = stdadd.Text + @" [new]";
            stdadd.ShowDialog();
        }

        private void DeleteStudentButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MessageBox.Show(@"Really delete?", @"Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string id = bandedStudentsGridView.GetRowCellValue(bandedStudentsGridView.FocusedRowHandle, "_id").ToString();
                stdcode.StdRemove(this, id);
            }
        }

        private void PauseStudentButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            string id = bandedStudentsGridView.GetRowCellValue(bandedStudentsGridView.FocusedRowHandle, "_id").ToString();
            stdcode.StdActionButton(this, id, "paused");
        }

        private void ResumeStudentButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            string id = bandedStudentsGridView.GetRowCellValue(bandedStudentsGridView.FocusedRowHandle, "_id").ToString();
            stdcode.StdActionButton(this, id, "active");
        }

        private void StartStudentButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            string id = bandedStudentsGridView.GetRowCellValue(bandedStudentsGridView.FocusedRowHandle, "_id").ToString();
            stdcode.StdActionButton(this, id, "active");
        }

        private void FinishStudentButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            string id = bandedStudentsGridView.GetRowCellValue(bandedStudentsGridView.FocusedRowHandle, "_id").ToString();
            stdcode.StdActionButton(this, id, "finished");
        }

        private void ActiveStudentsButton_DownChanged(object sender, ItemClickEventArgs e)
        {
            Properties.Settings.Default.StdFilterFlag = "active";
            Properties.Settings.Default.Save();

            stdcode.StdGridRefresh(this);
            bandedStudentsGridView_FocusedRowChanged(null, null);
        }

        private void PausedStudentsButton_DownChanged(object sender, ItemClickEventArgs e)
        {
            Properties.Settings.Default.StdFilterFlag = "paused";
            Properties.Settings.Default.Save();

            stdcode.StdGridRefresh(this);
            bandedStudentsGridView_FocusedRowChanged(null, null);
        }

        private void AwaitingStudentsButton_DownChanged(object sender, ItemClickEventArgs e)
        {
            Properties.Settings.Default.StdFilterFlag = "awaiting";
            Properties.Settings.Default.Save();

            stdcode.StdGridRefresh(this);
            bandedStudentsGridView_FocusedRowChanged(null, null);
        }

        private void FinishedStudentsButton_DownChanged(object sender, ItemClickEventArgs e)
        {
            Properties.Settings.Default.StdFilterFlag = "finished";
            Properties.Settings.Default.Save();

            stdcode.StdGridRefresh(this);
            bandedStudentsGridView_FocusedRowChanged(null, null);
        }

        private void EditStudentButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            Properties.Settings.Default.StdFormType = "edit";
            var stdadd = new stdform(this) {StartPosition = FormStartPosition.CenterParent};
            stdadd.Text = stdadd.Text + @" [edit]";
            
            stdadd.ShowDialog();
        }

        private void bandedStudentsGridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                string status = bandedStudentsGridView.GetRowCellValue(bandedStudentsGridView.FocusedRowHandle, "status").ToString();
                stdcode.StdActionButtonsSwitch(this, status);
            }
            catch
            {
                PauseStudentButton.Enabled = false;
                ResumeStudentButton.Enabled = false;
                FinishStudentButton.Enabled = false;
                StartStudentButton.Enabled = false;
                DeleteStudentButton.Enabled = false;
                EditStudentButton.Enabled = false;
            }
        }

        private void bandedGroupGridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                //Properties.Settings.Default.GroupSelectedRowIndex = bandedGroupGridView.FocusedRowHandle;
                Properties.Settings.Default.CurrentGroupNumber = bandedGroupGridView.GetRowCellValue(bandedGroupGridView.FocusedRowHandle, "number").ToString();
                //Properties.Settings.Default.GroupFilterFlag = bandedGroupGridView.GetRowCellValue(bandedGroupGridView.FocusedRowHandle, "status").ToString();
                //Properties.Settings.Default.CurrentGroupStatus = mainapp.bandedGroupGridView.GetRowCellValue(Properties.Settings.Default.GroupSelectedRowIndex, "status").ToString();
                Properties.Settings.Default.Save();

                groupcode.ActionButtonSwitch(this);
            }
            //groupcode.GetCurrentGroupData(this);
            //stdcode.GroupDetails(this);
            //}
            catch
            {
                Properties.Settings.Default.CurrentGroupNumber = "";
                Properties.Settings.Default.Save();

                //stdcode.GroupDetails(this);
                PauseGroupButton.Enabled = false;
                ResumeGroupButton.Enabled = false;
                FinishGroupButton.Enabled = false;
                StartGroupButton.Enabled = false;
                DeleteGroupButton.Enabled = false;
                EditGroupButton.Enabled = false;
            }

            stdcode.GroupDetails(this);
        }

        public void DetailGroupButton_DownChanged(object sender, ItemClickEventArgs e)
        {
            if (DetailGroupButton.Down)
            {
                Properties.Settings.Default.GroupDetailsShow = true;
                Properties.Settings.Default.Save();
                splitContainerControl1.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both;

                DetailGroupGridViewColHide();
            }
            else
            {
                Properties.Settings.Default.GroupDetailsShow = false;
                Properties.Settings.Default.Save();
                splitContainerControl1.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
            }
        }

        private void DetailGroupGridViewColHide()
        {
            int i = 0;

            while (i <= bandedDetailGroupGridView.Columns.Count - 1)
            {
                bandedDetailGroupGridView.Columns[i].Visible = false;
                i++;
            }

            bandedDetailGroupGridView.Columns["fullname"].Visible = true;
        }
    }
}