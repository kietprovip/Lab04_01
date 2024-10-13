using Lab04_01.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab04_01
{
    public partial class formSearch : Form
    {
        private readonly StudentContextDB contextDB = new StudentContextDB();
        public formSearch()
        {
            InitializeComponent();
        }

        private void formSearch_Load(object sender, EventArgs e)
        {
            List<Falcuty> dsKhoa = contextDB.Falcuty.ToList();
            DoDuLieuVaoComBoBox(dsKhoa);

            List<Student> dsSV = contextDB.Student.ToList();
            DoDuLieuVaoDGV(dsSV);
            txtResult.Text= dgvSV.Rows.Count.ToString();
        }
        private void DoDuLieuVaoComBoBox(List<Falcuty> dsKhoa)
        {

            cmbKhoa.DataSource = dsKhoa;
            cmbKhoa.ValueMember = "FalcutyID";
            cmbKhoa.DisplayMember = "FalcutyName";
        }
        private void DoDuLieuVaoDGV(List<Student> dsSV)
        {
            dgvSV.Rows.Clear();
            foreach (var item in dsSV)
            {
                int index = dgvSV.Rows.Add();
                dgvSV.Rows[index].Cells[0].Value = item.StudentID;
                dgvSV.Rows[index].Cells[1].Value = item.FullName;
                dgvSV.Rows[index].Cells[2].Value = item.Falcuty.FalcutyName;
                dgvSV.Rows[index].Cells[3].Value = item.AverageScore;
            }
        }

        private void ResetControl()
        {
            txtMSSV.Clear();
            txtHoTen.Clear();
            cmbKhoa.Text = "Công nghệ thông tin";

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMSSV.Text) && string.IsNullOrEmpty(txtHoTen.Text) && string.IsNullOrEmpty(cmbKhoa.Text))
            {
                MessageBox.Show("Vui lòng nhập dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            List<Student> dsSV = contextDB.Student.ToList();
            if (!string.IsNullOrEmpty(txtMSSV.Text))
            {
                string searchTerm1 = txtMSSV.Text.ToLower();
                dsSV = dsSV.Where(sv => sv.StudentID.ToLower().StartsWith(searchTerm1)).ToList();
            }

            if (!string.IsNullOrEmpty(txtHoTen.Text))
            {
                string searchTerm2 = txtHoTen.Text.ToLower();
                dsSV = dsSV.Where(sv => sv.FullName.ToLower().StartsWith(searchTerm2)).ToList();
            }

            if (!string.IsNullOrEmpty(cmbKhoa.Text))
            {
                string searchTerm3 = cmbKhoa.Text.ToLower();
                dsSV = dsSV.Where(sv => sv.Falcuty.FalcutyName.ToLower().StartsWith(searchTerm3)).ToList();
            }

            DoDuLieuVaoDGV(dsSV);
            txtResult.Text = dgvSV.Rows.Count.ToString();

        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            var form1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();

            if (form1 != null)
            {
                form1.Show();
            }

            this.Close();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            ResetControl();
        }
    }
}
