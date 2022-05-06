using AppEmailFernando.Components;
using AppEmailFernando.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AppEmailFernando
{
    public partial class EmailForm : Form
    {
        private static string BaseBoardPath => "rodape_backup.txt";

        private static string HeaderPath => "cabecalho_backup.txt";

        private List<CourseCoupon> _items = new List<CourseCoupon>();
        private List<CourseCoupon> _filteredItems = new List<CourseCoupon>();

        private bool _orderAscending = false;

        public EmailForm()
        {
            InitializeComponent();

            KeyPreview = true;
            StartPosition = FormStartPosition.CenterScreen;

            ReadEmailData();
        }

        private void EmailForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.F8:
                    MarkAllItems(false);
                    break;
                case Keys.F5:
                    Generate();
                    break;
                case Keys.F4:
                    MarkAllItems(true);
                    break;
                case Keys.F1:
                    CopyEmailResult();
                    break;
                case Keys.F6:
                    Reset();
                    break;
                case Keys.Escape:
                    Close();
                    break;
                default:
                    break;
            }
        }

        public void PopulateItems(List<CourseCoupon> cupom)
        {
            _items = new List<CourseCoupon>(cupom).OrderBy(x => x.CourseName).ToList();
                    
            foreach(var item in _items)
            {
                item.Marked = false;
            }

            UpdateItemsList();
        }

        private void textBoxHeader_TextChanged(object sender, EventArgs e)
        {
            UpdateEmailResult();
        }

        private void textBoxBaseBoard_TextChanged(object sender, EventArgs e)
        {
            UpdateEmailResult();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            UpdateItemsList();
        }

        #region ItemManagment

        private void UpdateItemsList()
        {
            _filteredItems = new List<CourseCoupon>(_items);

            if(textBoxSearch.Text.Length > 0)
                _filteredItems = _filteredItems.FindAll(x => x.CourseName.ToLower().IndexOf(textBoxSearch.Text.ToLower()) != -1 || x.Marked);

            if(_orderAscending)
                _filteredItems = _filteredItems.OrderByDescending(x => x.CourseName).ToList();
            else
                _filteredItems = _filteredItems.OrderBy(x => x.CourseName).ToList();

            checkedListBox.Items.Clear();

            foreach(var item in _filteredItems)
                checkedListBox.Items.Add($"({item.MaxRedemptions}) - {item.CourseName}", item.Marked);

            UpdateEmailResult();
        }

        private void MarkAllItems(bool marked)
        {
            foreach(var item in _filteredItems)
                item.Marked = marked;

            UpdateItemsList();
        }

        private void checkedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int index = this.checkedListBox.SelectedIndex;

            if(index < 0 || index >= _filteredItems.Count)
                return;

            _filteredItems[index].Marked = (e.NewValue == CheckState.Checked);

            var originalItem = _items.Find(x => x.CourseId == _filteredItems[index].CourseId);

            if(originalItem != null)
                originalItem.Marked = _filteredItems[index].Marked;

            UpdateEmailResult();
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            _orderAscending = !_orderAscending;

            UpdateItemsList();
        }

        private void btnDesmarcaTudo_Click(object sender, EventArgs e)
        {
            MarkAllItems(false);
        }

        private void btnMarkList_Click(object sender, EventArgs e)
        {
            MarkAllItems(true);
        }

        private void btnUnmarkAll_Click(object sender, EventArgs e)
        {
            MarkAllItems(false);
        }

        #endregion

        #region GenerateEmail

        private void Generate()
        {
            foreach(var item in _items)
            {
                if(item.Marked && item.MaxRedemptions == 0)
                {
                    AutoClosingMessageBox.Show($@"O curso ""{item.CourseName}"" não tem nenhum cupom com saldo disponivel!", "Erro", 3000);

                    return;
                }
            }

            CopyEmailResult();

            foreach(var item in _items)
            {
                if(item.Marked)
                {
                    item.MaxRedemptions--;

                    var filteredItem = _filteredItems.Find(x => x.CourseId == item.CourseId);

                    if(filteredItem != null)
                        filteredItem.MaxRedemptions = item.MaxRedemptions;
                }
            }

            try
            {
                CourseCoupon.Save(_items);
            }
            catch(Exception e)
            {
                MessageBox.Show($"Erro ao salvar alterações: {e.Message}");

                RevertRedemptions();

                MessageBox.Show("Cupons usados foram devolvidos!");

                return;
            }

            checkedListBox.Items.Clear();
            UpdateItemsList();

            AutoClosingMessageBox.Show("SUCESSO", "SUCESSO", 250);
        }
      
        private void RevertRedemptions()
        {
            foreach(var item in _items)
            {
                if(item.Marked)
                {
                    item.MaxRedemptions++;

                    var filteredItem = _filteredItems.Find(x => x.CourseId == item.CourseId);

                    if(filteredItem != null)
                        filteredItem.MaxRedemptions = item.MaxRedemptions;
                }
            }
        }

        private void CopyEmailResult()
        {
            System.Windows.Forms.Clipboard.SetText(textBoxResult.Text);
        }

        private string GetCuponsString()
        {
            string result = string.Empty;

            foreach(var item in _items.Where(x => x.Marked))
                result += $"{item.CourseCuponUrl}{Environment.NewLine}";

            return result;
        }

        private void UpdateEmailResult()
        {
            textBoxResult.Text = $"{textBoxHeader.Text}{Environment.NewLine}{GetCuponsString()}{Environment.NewLine}{textBoxBaseBoard.Text}";
        }

        private void Reset()
        {
            textBoxSearch.Text = string.Empty;
            MarkAllItems(false);
            textBoxSearch.Focus();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            CopyEmailResult();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            Generate();
        }

        #endregion

        #region ReadEmailData

        private void ReadEmailData()
        {
            try
            {
                if(File.Exists(HeaderPath))
                {
                    using(StreamReader reader = File.OpenText(HeaderPath))
                    {
                        textBoxHeader.Text = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Falha ao ler dados do cabeçalho: {e.Message}");
            }

            try
            {
                if(File.Exists(HeaderPath))
                {
                    using(StreamReader reader = File.OpenText(BaseBoardPath))
                    {
                        textBoxBaseBoard.Text = reader.ReadToEnd();
                    }
                }    
            }
            catch (Exception e)
            {
                MessageBox.Show($"Falha ao ler dados do rodapé: {e.Message}");
            }
        }

        private void SaveEmailData()
        {
            try
            {
                using(StreamWriter writer = new StreamWriter(BaseBoardPath))
                {
                    writer.Write(textBoxBaseBoard.Text);
                }
            }
            catch(Exception e)
            {
                MessageBox.Show($"Falha ao ler dados do rodapé: {e.Message}");
            }

            try
            {
                using(StreamWriter writer = new StreamWriter(HeaderPath))
                {
                    writer.Write(textBoxHeader.Text);
                }
            }
            catch(Exception e)
            {
                MessageBox.Show($"Falha ao ler dados do cabeçalho: {e.Message}");
            }
        }

        private void EmailForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveEmailData();

        }

        #endregion   
    }
}
