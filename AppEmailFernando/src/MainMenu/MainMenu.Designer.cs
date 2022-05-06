namespace AppEmailFernando
{
    partial class MainMenu
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnManage = new System.Windows.Forms.Button();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnEmail = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnManage
            // 
            this.btnManage.Location = new System.Drawing.Point(662, 7);
            this.btnManage.Margin = new System.Windows.Forms.Padding(2);
            this.btnManage.Name = "btnManage";
            this.btnManage.Size = new System.Drawing.Size(102, 24);
            this.btnManage.TabIndex = 0;
            this.btnManage.Text = "Gerenciar";
            this.btnManage.UseVisualStyleBackColor = true;
            this.btnManage.Click += new System.EventHandler(this.btnManage_Click);
            // 
            // textBoxPath
            // 
            this.textBoxPath.Enabled = false;
            this.textBoxPath.Location = new System.Drawing.Point(81, 9);
            this.textBoxPath.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(577, 20);
            this.textBoxPath.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Working File";
            // 
            // btnEmail
            // 
            this.btnEmail.Location = new System.Drawing.Point(10, 34);
            this.btnEmail.Margin = new System.Windows.Forms.Padding(2);
            this.btnEmail.Name = "btnEmail";
            this.btnEmail.Size = new System.Drawing.Size(754, 28);
            this.btnEmail.TabIndex = 8;
            this.btnEmail.Text = "Ir para ambiente de email";
            this.btnEmail.UseVisualStyleBackColor = true;
            this.btnEmail.Click += new System.EventHandler(this.btnEmail_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 73);
            this.Controls.Add(this.btnEmail);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxPath);
            this.Controls.Add(this.btnManage);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximumSize = new System.Drawing.Size(791, 152);
            this.MinimumSize = new System.Drawing.Size(791, 112);
            this.Name = "MainMenu";
            this.Text = "AppEmail";
            this.Shown += new System.EventHandler(this.MainMenu_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnManage;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEmail;
    }
}

