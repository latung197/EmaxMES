using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System.Drawing.Imaging;
using System.IO;
using DevExpress.XtraTab;

namespace MES
{
    public partial class regdmdvt : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public regdmdvt()
        {
            InitializeComponent();
        }

        private void regdmdvt_Load(object sender, EventArgs e)
        {
            Grid_Set();
            Search_Data();
        }

        private void Grid_Set()
        {
            DbHelp.GridSet(Grid_dmdvt, View_dmdvt, "dmdvt_Code", "mã đơn vị tính", "100", false, false, true);
            DbHelp.GridSet(Grid_dmdvt, View_dmdvt, "dmdvt_Name", "tên đơn vị tính", "100", false, false, true);
            DbHelp.GridSet(Grid_dmdvt, View_dmdvt, "Owner", "대표자", "90", false, false, true);
            DbHelp.GridSet(Grid_dmdvt, View_dmdvt, "Short_Name", "줄임상호", "90", false, false, true);


            DbHelp.GridColumn_CheckBox(View_dmdvt, "Def_Ck");

            Grid_dmdvt.MouseWheelChk = false;
            Grid_dmdvt.PopMenuChk = false;
            Grid_dmdvt.AddRowYN = false;
        }

        private void View_dmdvt_DoubleClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(View_dmdvt.GetFocusedRowCellValue("dmdvt_Code").NullString()))
            {
                PopdmdvtForm Form = new PopdmdvtForm(View_dmdvt.GetFocusedRowCellValue("dmdvt_Code").NullString());
                Form.StartPosition = FormStartPosition.CenterScreen;

                if (Form.ShowDialog() == DialogResult.OK)
                {
                    Search_Data();
                }
            }
        }

        #region 버튼 이벤트
        private void btn_Insert_Click(object sender, EventArgs e)
        {
            PopdmdvtForm Form = new PopdmdvtForm();
            Form.StartPosition = FormStartPosition.CenterScreen;

            if (Form.ShowDialog() == DialogResult.OK)
            {
                Search_Data();
            }
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            Search_Data();
        }

        private void Search_Data()
        {
            try
            {
                SqlParam sp = new SqlParam("sp_dmdvt");
                sp.AddParam("Kind", "S");
                sp.AddParam("dmdvt_Code", "@");

                ret = DbHelp.Proc_Search(sp);

                if (ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                Grid_dmdvt.DataSource = ret.ReturnDataSet.Tables[0];
                Grid_dmdvt.RefreshDataSource();
                View_dmdvt.BestFitColumns();
            }
            catch (Exception)
            {

            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            SqlParam sp = new SqlParam("sp_dmdvt");
            sp.AddParam("Kind", "D");
            sp.AddParam("dmdvt_Code", View_dmdvt.GetFocusedRowCellValue("dmdvt_Code").NullString());

            ret = DbHelp.Proc_Save(sp);

            if (ret.ReturnChk != 0)
            {
                XtraMessageBox.Show(ret.ReturnMessage);
            }

            btn_Delete.sCHK = "Y";

            Search_Data();
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(Grid_dmdvt, this.Name);
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }
        #endregion

        #region 이미지
        private void pictureEdit_Sign_MouseClick_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Image_Help.Image_Right_Click(sender, e);
            }
        }

        private void pictureEdit_Logo_MouseClick_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Image_Help.Image_Right_Click(sender, e);
            }
        }
        #endregion

        #region 상속 함수

        protected override void btnClose()
        {
            btn_Close.PerformClick();
        }

        protected override void btnDelete()
        {
            btn_Delete.PerformClick();
        }

        protected override void btnSelect()
        {
            btn_Select.PerformClick();
        }

        protected override void btnExcel()
        {
            btn_Excel.PerformClick();
        }

        protected override void btnInsert()
        {
            btn_Insert.PerformClick();
        }

        protected override void btnSave()
        {
            btn_Save.PerformClick();
        }

        protected override void Control_TextChange(object sender, EventArgs e)
        {
            base.Control_TextChange(sender, e);

            btn_Insert.sUpdate = "Y";
            btn_Close.sUpdate = "Y";
        }


        #endregion
    }
}
