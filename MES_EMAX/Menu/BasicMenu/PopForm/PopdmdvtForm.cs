using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MES
{
    public partial class PopdmdvtForm : BaseForm
    {
        ReturnStruct ret = new ReturnStruct();

        public PopdmdvtForm(string dmdvt_Code = "")
        {
            InitializeComponent();

            pictureEdit_Logo.Properties.ContextMenuStrip = new ContextMenuStrip();
            pictureEdit_Sign.Properties.ContextMenuStrip = new ContextMenuStrip();

            txt_dmdvtCode.Text = dmdvt_Code;
            Search_Data();
        }

        private void PopdmdvtForm_Load(object sender, EventArgs e)
        {

        }

        private void Search_Data()
        {
            try
            {
                SqlParam sp = new SqlParam("sp_dmdvt");
                sp.AddParam("Kind", "S");
                sp.AddParam("dmdvt_Code", txt_dmdvtCode.Text);

                ret = DbHelp.Proc_Search(sp);

                if (ret.ReturnDataSet.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ret.ReturnDataSet.Tables[0].Rows[0];

                    foreach (DataColumn col in row.Table.Columns)
                    {
                        if (row[col].GetType().Name.Contains("Null"))
                        {
                            if (!(col.ToString().Contains("Img") || col.ToString().Contains("Date")))
                            {
                                row[col] = "";
                            }
                        }   
                    }

                    txt_dmdvtCode.Text = row["dmdvt_Code"].NullString();
                    txt_dmdvtName.Text = row["dmdvt_Name"].NullString();
                    txt_Owner.Text = row["Owner"].NullString();
                    txt_ShortNM.Text = row["Short_Name"].NullString();

                }

                if (!string.IsNullOrWhiteSpace(txt_dmdvtCode.Text))
                {
                    if (!string.IsNullOrWhiteSpace(txt_dmdvtName.Text))
                        txt_dmdvtCode.ReadOnly = true;

                    txt_dmdvtName.Focus();
                }
                else
                {
                    txt_dmdvtCode.ReadOnly = false;
                    txt_dmdvtCode.Focus();
                }

                btn_Insert.sUpdate = "N";
                btn_Close.sUpdate = "N";
            }
            catch (Exception)
            {

            }
        }

        private void chk_Main_CheckedChanged(object sender, EventArgs e)
        {
            Check_Changed();
        }

        private void chk_Main_EditValueChanged(object sender, EventArgs e)
        {
            Check_Changed();
        }

        private void Check_Changed()
        {
            SqlParam sp = new SqlParam("sp_dmdvt");
            sp.AddParam("Kind", "C");
            sp.AddParam("dmdvt_Code", txt_dmdvtCode.Text);

            ret = DbHelp.Proc_Search(sp);

            if (ret.ReturnChk != 0)
            {
                XtraMessageBox.Show(ret.ReturnMessage);
                return;
            }

            if (Convert.ToInt32(ret.ReturnDataSet.Tables[0].Rows[0][0].NumString()) > 0)
                chk_Main.Checked = false;
        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            if (btn_Insert.Result_Update == DialogResult.Yes)
            {
                if (!Save_Data())
                    return;
            }

            DbHelp.Clear_Panel(panelControl3);
            DbHelp.Clear_Panel(panel_H);

            txt_dmdvtNo.Text = "";
            txt_OfficeNo.Text = "";

            Search_Data();
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (Save_Data())
            {
                btn_Save.sCHK = "Y";
                Search_Data();
            }
        }

        private bool Check_Values()
        {
            if (string.IsNullOrWhiteSpace(txt_dmdvtCode.Text))
            {
                XtraMessageBox.Show("회사 코드는 필수값입니다.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txt_dmdvtName.Text))
            {
                XtraMessageBox.Show("회사명은 필수값입니다.");
                return false;
            }

            return true;
        }

        private bool Save_Data()
        {
            if (Check_Values())
            {
                SqlParam sp = new SqlParam("sp_dmdvt");
                sp.AddParam("Kind", "I");

                sp.AddParam("dmdvt_Code", txt_dmdvtCode.Text);
                sp.AddParam("dmdvt_Name", txt_dmdvtName.Text);
                sp.AddParam("dmdvt_No", txt_dmdvtNo.Text);
                sp.AddParam("Office_No", txt_OfficeNo.Text);
                sp.AddParam("Owner", txt_Owner.Text);
                sp.AddParam("UpTai", txt_UpTai.Text);
                sp.AddParam("UpJong", txt_UpJong.Text);
                sp.AddParam("Tel_No", txt_TelNo.Text);
                sp.AddParam("Fax_No", txt_FaxNo.Text);
                sp.AddParam("E_Mail", txt_EMail.Text);
                sp.AddParam("Home_Page", txt_HomePage.Text);
                sp.AddParam("Bill_Addr1", txt_BillAddr1.Text);
                sp.AddParam("Bill_Addr2", txt_BillAddr2.Text);
                sp.AddParam("Use_Ck", "Y");

                if (pictureEdit_Logo.Image != null)
                    sp.AddParam("Logo_Img", Image_Help.ImageToByte(pictureEdit_Logo.Image));
                if (pictureEdit_Sign.Image != null)
                    sp.AddParam("Sign_Img", Image_Help.ImageToByte(pictureEdit_Sign.Image));

                sp.AddParam("Short_Name", txt_ShortNM.Text);
                sp.AddParam("Def_Ck", chk_Main.EditValue);

                sp.AddParam("Reg_User", GlobalValue.sUserID);
                sp.AddParam("Up_User", GlobalValue.sUserID);

                ret = DbHelp.Proc_Search(sp);

                if (ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return false;
                }

                return true;
            }
            else
                return true;
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            SqlParam sp = new SqlParam("sp_dmdvt");
            sp.AddParam("Kind", "D");
            sp.AddParam("dmdvt_Code", txt_dmdvtCode.Text);

            ret = DbHelp.Proc_Save(sp);

            if (ret.ReturnChk != 0)
            {
                XtraMessageBox.Show(ret.ReturnMessage);
            }

            btn_Delete.sCHK = "Y";

            DbHelp.Clear_Panel(panelControl3);
            DbHelp.Clear_Panel(panel_H);

            txt_dmdvtNo.Text = "";
            txt_OfficeNo.Text = "";

            Search_Data();
            txt_dmdvtCode.Focus();
        }

        

        #region 상속 함수

        protected override void btnClose()
        {
            btn_Close.PerformClick();
        }

        protected override void btnDelete()
        {
            btn_Delete.PerformClick();
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

        private void btn_Close_Click(object sender, EventArgs e)
        {
            if (btn_Close.Result_Update == DialogResult.Yes)
            {
                if (!Save_Data())
                    return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void txt_dmdvtCode_Leave(object sender, EventArgs e)
        {
            Search_Data();
        }


        private void pictureEdit_Logo_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Image_Help.Image_Right_Click(sender, e);
            }
        }

        private void pictureEdit_Sign_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Image_Help.Image_Right_Click(sender, e);
            }
        }
    }
}