using AppEmailFernando.Entities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AppEmailFernando
{
    public partial class MainMenu : Form
    {
        private List<CourseCoupon> _items = null;

        public MainMenu()
        {
            InitializeComponent();

            btnManage.Enabled = false;
            btnEmail.Enabled = false;

            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private bool Open()
        {
            try
            {
                _items = CourseCoupon.Open();

                btnManage.Enabled = true;
                btnEmail.Enabled = true;
                textBoxPath.Text = $"{ Environment.CurrentDirectory}\\{ CourseCoupon.Path }";
            }
            catch(Exception ex)
            {
                _items = null;
                
                textBoxPath.Text = $@"Falha ao importar ""{Environment.CurrentDirectory}\\{CourseCoupon.Path }""";

                MessageBox.Show($"Falha ao abrir: {ex.Message}");

                return false;
            }

            return true;
        }

        private void btnEmail_Click(object sender, EventArgs e)
        {
            if(!Open())
                return;

            EmailForm form = new EmailForm();

            Hide();
            form.PopulateItems(_items);
            form.ShowDialog();
            Show();

            form.Close();
        }

        private void MainMenu_Shown(object sender, EventArgs e)
        {
            Open();
        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            if(!Open())
                return;

            CourseCouponForm form = new CourseCouponForm();

            Hide();
            form.PopulateList(_items);
            form.ShowDialog();
            Show();

            form.Close();
        }
    }
}
