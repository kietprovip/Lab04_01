using Lab04_01.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab04_01
{
    public partial class Form1 : Form
    {
        private readonly StudentContextDB contextDB= new StudentContextDB();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<Falcuty> dsKhoa = contextDB.Falcuty.ToList();
            DoDuLieuVaoComBoBox(dsKhoa);

            List<Student> dsSV = contextDB.Student.ToList();
            DoDuLieuVaoDGV(dsSV);
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
        private bool RangBuoc()
        {
            if (string.IsNullOrWhiteSpace(txtMSSV.Text))
            {
                MessageBox.Show("Mã sinh viên không được để trống ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMSSV.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Họ tên sinh viên không được để trống ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtHoTen.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtDTB.Text))
            {
                MessageBox.Show("HĐiểm trung bình không được để trống ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDTB.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(cmbKhoa.Text))
            {
                MessageBox.Show("Khoa không được để trống ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbKhoa.Focus();
                return false;
            }
            return true;
        }
        private void ResetControl()
        {
            txtMSSV.Clear();
            txtHoTen.Clear();
            txtDTB.Clear();
            cmbKhoa.Text = "Công nghệ thông tin";

        }

        private void dgvSV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvSV.Rows[e.RowIndex];
                txtMSSV.Text = row.Cells[0].Value.ToString();
                txtHoTen.Text = row.Cells[1].Value.ToString();
                
                cmbKhoa.Text = row.Cells[2].Value.ToString();
                txtDTB.Text = row.Cells[3].Value.ToString();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!RangBuoc())
                return;
            var maSV = txtMSSV.Text;
            var timSV = contextDB.Student.Find(maSV);
            if (timSV != null)
            {
                MessageBox.Show("Sinh viên đã tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var sinhvien = new Student
            {
                StudentID = txtMSSV.Text,
                FullName = txtHoTen.Text,
                AverageScore = float.Parse(txtDTB.Text),
                FalcutyID = (cmbKhoa.SelectedItem as Falcuty).FalcutyID,
            };
            contextDB.Student.Add(sinhvien);
            contextDB.SaveChanges();
            MessageBox.Show("Thêm sinh viên thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DoDuLieuVaoDGV(contextDB.Student.ToList());
            ResetControl();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (!RangBuoc())
                    return;
                var maSV = txtMSSV.Text;
                var sinhvien = contextDB.Student.Where(sv => sv.StudentID == maSV).SingleOrDefault();
                if (sinhvien != null)
                {
                    sinhvien.FullName = txtHoTen.Text;
                    sinhvien.AverageScore = float.Parse(txtDTB.Text);
                    sinhvien.FalcutyID = (cmbKhoa.SelectedItem as Falcuty).FalcutyID;
                    contextDB.SaveChanges();
                    DoDuLieuVaoDGV(contextDB.Student.ToList());
                    ResetControl();
                    MessageBox.Show("Sửa sinh viên thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }



            }
            catch
            {
                MessageBox.Show("Có lỗi trong qúa trình sửa sinh viên", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {

                var maSV = txtMSSV.Text;
                var sinhvien = contextDB.Student.Where(sv => sv.StudentID == maSV).FirstOrDefault();
                if (sinhvien != null)
                {
                    contextDB.Student.Remove(sinhvien);
                    contextDB.SaveChanges();
                    DoDuLieuVaoDGV(contextDB.Student.ToList());
                    ResetControl();
                    MessageBox.Show("Xóa sinh viên thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

            }
            catch
            {
                MessageBox.Show("Có lỗi trong qúa trình xóa sinh viên", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnKhoa_Click(object sender, EventArgs e)
        {
            formFalcuty quanLyKhoa = new formFalcuty();
            quanLyKhoa.FormClosed += new FormClosedEventHandler(QuanLyKhoa_FormClosed);

            quanLyKhoa.Show();
            this.Hide();
        }
        private void QuanLyKhoa_FormClosed(object sender, FormClosedEventArgs e)
        {
            cmbKhoa.DataSource = null;

            List<Falcuty> dsKhoa = contextDB.Falcuty.ToList();
            DoDuLieuVaoComBoBox(dsKhoa);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            formSearch timkiem = new formSearch();
            timkiem.Show();
            this.Hide();
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            int a;
            int b;
            int c;
        }
    }
}
