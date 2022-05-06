using AppEmailFernando.Entities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AppEmailFernando
{
    public partial class CourseCouponForm : Form
    {
        private List<CourseCoupon> _items = null;

        public CourseCouponForm()
        {
            InitializeComponent();

            btnDelete.Enabled = false;
            StartPosition = FormStartPosition.CenterScreen;

            KeyPreview = true;
            btnSave.Enabled = false;
            btnNew.Enabled = true;

            SetButtonsState(false);
        }
 
        private void CuponsForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    DeleteCurrentFromList();
                    break;
                case Keys.F1:
                    CreateNewItem();
                    break;
                case Keys.F2:
                    Save();
                    break;
                case Keys.Escape:
                    Close();
                    break;
                default:
                    break;
            }
        }

        private void SetButtonsState(bool enable)
        {
            textBoxCourseId.Enabled = enable;
            textBoxCourseName.Enabled = enable;
            textBoxCuponCode.Enabled = enable;
            textBoxCuponUrl.Enabled = enable;
            textBoxCuponUses.Enabled = enable;
            btnDelete.Enabled = enable;

            if (!enable)
            {
                textBoxCourseId.Text = string.Empty;
                textBoxCourseName.Text = string.Empty;
                textBoxCuponCode.Text = string.Empty;
                textBoxCuponUrl.Text = string.Empty;
                textBoxCuponUses.Text = string.Empty;
            }
        }

        #region CRUD

        public void PopulateList(List<CourseCoupon> items)
        {
            _items = new List<CourseCoupon>(items);

            foreach (var item in items)
                listBox.Items.Add($"{item.CourseId} - {item.CourseName}");
        }

        private void DeleteCurrentFromList()
        {
            SetButtonsState(false);

            int index = listBox.SelectedIndex;

            if (index < 0 || index >= _items.Count)
                return;

            _items.RemoveAt(index);

            listBox.Items.Clear();
            PopulateList(_items);
            btnSave.Enabled = true;
        }

        private void CreateNewItem()
        {
            _items.Add(new CourseCoupon());
            listBox.Items.Add("0 -");
            btnSave.Enabled = true;
        }

        private bool Save()
        {
            try
            {
                CourseCoupon.Save(_items);
                btnSave.Enabled = false;

                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show($"Falha ao salvar: {e.Message}");
                return false;
            }
        }

        #endregion

        #region Events

        private void btnNew_Click(object sender, System.EventArgs e)
        {
            CreateNewItem();
        }

        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            DeleteCurrentFromList();
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            Save();
        }

        private void listBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ListBox listBox = sender as ListBox;

            int index = listBox.SelectedIndex;

            if (index < 0 || index >= _items.Count)
            {
                SetButtonsState(false);
                return;
            }

            textBoxCourseId.Text = _items[index].CourseId.ToString();
            textBoxCourseName.Text = _items[index].CourseName;
            textBoxCuponCode.Text = _items[index].CouponCode.ToString();
            textBoxCuponUrl.Text = _items[index].CourseCuponUrl;
            textBoxCuponUses.Text = _items[index].MaxRedemptions.ToString();

            SetButtonsState(true);
        }

        #endregion

        #region FieldsValidation

        private void textBoxCourseId_Validate(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
                e.Handled = true;
        }

        private void textBoxCourseName_Validate(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ';')
                e.Handled = true;
        }

        private void textBoxCuponCode_Validate(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
                e.Handled = true;
        }

        private void textBoxCuponUrl_Validate(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ';')
                e.Handled = true;
        }

        private void textBoxCuponUses_Validate(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
                e.Handled = true;
        }

        #endregion
     
        #region TextChanged

        private void UpdateListBoxItem(int index)
        {
            if (index < 0 || index >= _items.Count)
                return;

            if (index >= listBox.Items.Count)
                return;

            listBox.SelectedIndexChanged -= listBox_SelectedIndexChanged;
            listBox.Items[index] =  $"{_items[index].CourseId} - {_items[index].CourseName}";
            listBox.SelectedIndexChanged += listBox_SelectedIndexChanged;
        }

        private void textBoxCourseId_TextChanged(object sender, System.EventArgs e)
        {
            int index = listBox.SelectedIndex;

            if (index < 0 || index >= _items.Count)
                return;

            TextBox textBox = sender as TextBox;

            if(textBox.Text != _items[index].CourseId.ToString())
            {
                if (textBox.Text.Length == 0)
                {
                    _items[index].CourseId = 0;
                    btnSave.Enabled = true;
                }
                else if (ulong.TryParse(textBox.Text, out ulong fromText))
                {
                    _items[index].CourseId = fromText;
                    btnSave.Enabled = true;
                }
                else
                {
                    _items[index].CourseId = 0;
                    textBox.Text = "0";
                    MessageBox.Show(@"Falha ao reconhecer campo ""Id do Curso""");
                }

                UpdateListBoxItem(index);
            }
        }

        private void textBoxCourseName_TextChanged(object sender, System.EventArgs e)
        {
            int index = listBox.SelectedIndex;

            if (index < 0 || index >= _items.Count)
                return;

            TextBox textBox = sender as TextBox;

            if (textBox.Text != _items[index].CourseName)
            {
                _items[index].CourseName = textBox.Text;
                btnSave.Enabled = true;
                UpdateListBoxItem(index);
            }
        }

        private void textBoxCuponCode_TextChanged(object sender, System.EventArgs e)
        {
            int index = listBox.SelectedIndex;

            if (index < 0 || index >= _items.Count)
                return;

            TextBox textBox = sender as TextBox;

            if (textBox.Text != _items[index].CouponCode.ToString())
            {
                if (textBox.Text.Length == 0)
                {
                    _items[index].CouponCode = 0;
                    btnSave.Enabled = true;
                }
                else if (ulong.TryParse(textBox.Text, out ulong fromText))
                {
                    _items[index].CouponCode = fromText;
                    btnSave.Enabled = true;
                }
                else
                {
                    _items[index].CouponCode = 0;
                    textBox.Text = "0";
                    MessageBox.Show(@"Falha ao reconhecer campo ""Código do Cupom""");
                }
            }
        }

        private void textBoxCuponUrl_TextChanged(object sender, System.EventArgs e)
        {
            int index = listBox.SelectedIndex;

            if (index < 0 || index >= _items.Count)
                return;

            TextBox textBox = sender as TextBox;

            if (textBox.Text != _items[index].CourseCuponUrl)
            {
                _items[index].CourseCuponUrl = textBox.Text;
                btnSave.Enabled = true;
            }
        }

        private void textBoxCuponUses_TextChanged(object sender, System.EventArgs e)
        {
            int index = listBox.SelectedIndex;

            if (index < 0 || index >= _items.Count)
                return;

            TextBox textBox = sender as TextBox;

            if (textBox.Text != _items[index].MaxRedemptions.ToString())
            {
                if (textBox.Text.Length == 0)
                {
                    _items[index].MaxRedemptions = 0;
                    btnSave.Enabled = true;
                }
                else if (ulong.TryParse(textBox.Text, out ulong fromText))
                {
                    _items[index].MaxRedemptions = fromText;
                    btnSave.Enabled = true;
                }
                else
                {
                    _items[index].MaxRedemptions = 0;
                    textBox.Text = "0";
                    MessageBox.Show(@"Falha ao reconhecer campo ""Número de usos disponíveis do Cupom""");
                }
            }
        }

        #endregion

        private void CourseCouponForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnSave.Enabled)
            {
                var result = MessageBox.Show("Salvar alterações", "Você deseja salvar as alterações?", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                    e.Cancel = !Save();
            }
        }
    }
}
