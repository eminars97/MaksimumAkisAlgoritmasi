namespace ShortestPath
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.listBoxCost = new System.Windows.Forms.ListBox();
            this.comboBoxBegin = new System.Windows.Forms.ComboBox();
            this.comboBoxEnd = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnShortestPath = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnMinCut = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(699, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Kapasite Tablosu";
            // 
            // listBoxCost
            // 
            this.listBoxCost.FormattingEnabled = true;
            this.listBoxCost.ItemHeight = 16;
            this.listBoxCost.Location = new System.Drawing.Point(702, 39);
            this.listBoxCost.Name = "listBoxCost";
            this.listBoxCost.Size = new System.Drawing.Size(223, 260);
            this.listBoxCost.TabIndex = 2;
            // 
            // comboBoxBegin
            // 
            this.comboBoxBegin.FormattingEnabled = true;
            this.comboBoxBegin.Location = new System.Drawing.Point(701, 342);
            this.comboBoxBegin.Name = "comboBoxBegin";
            this.comboBoxBegin.Size = new System.Drawing.Size(104, 24);
            this.comboBoxBegin.TabIndex = 3;
            // 
            // comboBoxEnd
            // 
            this.comboBoxEnd.FormattingEnabled = true;
            this.comboBoxEnd.Location = new System.Drawing.Point(820, 342);
            this.comboBoxEnd.Name = "comboBoxEnd";
            this.comboBoxEnd.Size = new System.Drawing.Size(104, 24);
            this.comboBoxEnd.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(701, 319);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Başlangıç";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(820, 318);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Bitiş";
            // 
            // btnShortestPath
            // 
            this.btnShortestPath.Location = new System.Drawing.Point(701, 381);
            this.btnShortestPath.Name = "btnShortestPath";
            this.btnShortestPath.Size = new System.Drawing.Size(223, 37);
            this.btnShortestPath.TabIndex = 7;
            this.btnShortestPath.Text = "EN FAZLA AKIŞI HESAPLA";
            this.btnShortestPath.UseVisualStyleBackColor = true;
            this.btnShortestPath.Click += new System.EventHandler(this.btnShortestPath_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 635);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 17);
            this.label4.TabIndex = 8;
            // 
            // btnMinCut
            // 
            this.btnMinCut.Location = new System.Drawing.Point(702, 434);
            this.btnMinCut.Name = "btnMinCut";
            this.btnMinCut.Size = new System.Drawing.Size(222, 36);
            this.btnMinCut.TabIndex = 9;
            this.btnMinCut.Text = "MIN-CUT HESAPLA";
            this.btnMinCut.UseVisualStyleBackColor = true;
            this.btnMinCut.Click += new System.EventHandler(this.btnMinCut_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 671);
            this.Controls.Add(this.btnMinCut);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnShortestPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxEnd);
            this.Controls.Add(this.comboBoxBegin);
            this.Controls.Add(this.listBoxCost);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormMain_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBoxCost;
        private System.Windows.Forms.ComboBox comboBoxBegin;
        private System.Windows.Forms.ComboBox comboBoxEnd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnShortestPath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnMinCut;
    }
}