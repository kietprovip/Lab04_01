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
    
    public partial class formFalcuty : Form
    {
        private readonly StudentContextDB contextDB = new StudentContextDB();
        public formFalcuty()
        {
            InitializeComponent();
        }

        private void formFalcuty_Load(object sender, EventArgs e)
        {
            List<Falcuty> dsKhoa = contextDB.Falcuty.ToList();
            DoDuLieuVaoDGV(dsKhoa);
        }
        private void DoDuLieuVaoDGV(List<Falcuty> dsPB)
        {
            dgvKhoa.Rows.Clear();
            foreach (var item in dsPB)
            {
                int index = dgvKhoa.Rows.Add();
                dgvKhoa.Rows[index].Cells[0].Value = item.FalcutyID;
                dgvKhoa.Rows[index].Cells[1].Value = item.FalcutyName;
                dgvKhoa.Rows[index].Cells[2].Value = item.TotalProfessor;

            }
        }
        private bool RangBuoc()
        {
            if (string.IsNullOrWhiteSpace(txtMaKhoa.Text))
            {
                MessageBox.Show("Mã khoa không được để trống ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaKhoa.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTenKhoa.Text))
            {
                MessageBox.Show("Tên khoa không được để trống ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTenKhoa.Focus();
                return false;
            }
            return true;
        }
        private void ResetControl()
        {
            txtMaKhoa.Clear();
            txtTenKhoa.Clear();

            txtTotalGS.Clear();

        }

        private void dgvKhoa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvKhoa.Rows[e.RowIndex];
                txtMaKhoa.Text = row.Cells[0].Value.ToString();
                txtTenKhoa.Text = row.Cells[1].Value.ToString();
                txtTotalGS.Text = row.Cells[2].Value.ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!RangBuoc())
                return;
            int maKhoa = int.Parse(txtMaKhoa.Text);
            var khoa = contextDB.Falcuty.Find(maKhoa);
            if (khoa != null)
            {
                khoa.FalcutyName = txtTenKhoa.Text;
                khoa.TotalProfessor = int.Parse(txtTotalGS.Text);
                contextDB.SaveChanges();
                DoDuLieuVaoDGV(contextDB.Falcuty.ToList());
                ResetControl();
                MessageBox.Show("Sửa khoa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var Khoa = new Falcuty
            {
                FalcutyID = int.Parse(txtMaKhoa.Text),
                FalcutyName = txtTenKhoa.Text,
                TotalProfessor = int.Parse(txtTotalGS.Text),
            };
            contextDB.Falcuty.Add(Khoa);
            contextDB.SaveChanges();
            MessageBox.Show("Thêm khoa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DoDuLieuVaoDGV(contextDB.Falcuty.ToList());
            ResetControl();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {

                int maKhoa = int.Parse(txtMaKhoa.Text);
                var khoa = contextDB.Falcuty.Where(k => k.FalcutyID == maKhoa).FirstOrDefault();
                if (khoa != null)
                {
                    var sinhviens = contextDB.Student.Where(sv => sv.FalcutyID == maKhoa).ToList();
                    if (sinhviens.Count > 0)
                    {
                        MessageBox.Show("Khoa này đang có nhân viên, vui lòng chuyển họ sang phòng ban khác trước khi xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    contextDB.Falcuty.Remove(khoa);
                    contextDB.SaveChanges();
                    DoDuLieuVaoDGV(contextDB.Falcuty.ToList());

                    MessageBox.Show("Xóa khoa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }



            }
            catch
            {
                MessageBox.Show("Có lỗi trong qúa trình xóa khoa", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            var form1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();

            if (form1 != null)
            {
                form1.Show();
            }

            this.Close();
        }
    }
}
