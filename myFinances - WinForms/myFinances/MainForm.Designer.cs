namespace myFinances
{
    partial class MainForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ColumnAction = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Persentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 65);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(142, 37);
            this.button1.TabIndex = 0;
            this.button1.Text = "Добавить доход";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(691, 66);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(142, 36);
            this.button2.TabIndex = 1;
            this.button2.Text = "Отметить расход";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(316, 74);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.comboBox1.Size = new System.Drawing.Size(232, 21);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(408, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(758, 25);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "Настройки";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnAction,
            this.ColumnName,
            this.ColumnSum,
            this.ColumnComment,
            this.ColumnData,
            this.Persentage});
            this.dataGridView1.Location = new System.Drawing.Point(12, 108);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(821, 483);
            this.dataGridView1.TabIndex = 5;
            // 
            // ColumnAction
            // 
            this.ColumnAction.Frozen = true;
            this.ColumnAction.HeaderText = "";
            this.ColumnAction.Name = "ColumnAction";
            this.ColumnAction.ReadOnly = true;
            this.ColumnAction.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnAction.Width = 25;
            // 
            // ColumnName
            // 
            this.ColumnName.Frozen = true;
            this.ColumnName.HeaderText = "Категория";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.ReadOnly = true;
            this.ColumnName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnName.Width = 160;
            // 
            // ColumnSum
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N0";
            dataGridViewCellStyle5.NullValue = null;
            this.ColumnSum.DefaultCellStyle = dataGridViewCellStyle5;
            this.ColumnSum.Frozen = true;
            this.ColumnSum.HeaderText = "Сумма";
            this.ColumnSum.Name = "ColumnSum";
            this.ColumnSum.ReadOnly = true;
            this.ColumnSum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnSum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnSum.Width = 60;
            // 
            // ColumnComment
            // 
            this.ColumnComment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColumnComment.Frozen = true;
            this.ColumnComment.HeaderText = "Комментарий";
            this.ColumnComment.Name = "ColumnComment";
            this.ColumnComment.ReadOnly = true;
            this.ColumnComment.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnComment.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnComment.Width = 390;
            // 
            // ColumnData
            // 
            this.ColumnData.Frozen = true;
            this.ColumnData.HeaderText = "Дата";
            this.ColumnData.Name = "ColumnData";
            this.ColumnData.ReadOnly = true;
            this.ColumnData.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnData.Width = 65;
            // 
            // Persentage
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Format = "N2";
            dataGridViewCellStyle6.NullValue = null;
            this.Persentage.DefaultCellStyle = dataGridViewCellStyle6;
            this.Persentage.Frozen = true;
            this.Persentage.HeaderText = "Процент";
            this.Persentage.MinimumWidth = 60;
            this.Persentage.Name = "Persentage";
            this.Persentage.ReadOnly = true;
            this.Persentage.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Persentage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Persentage.Width = 60;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(12, 609);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(145, 20);
            this.dateTimePicker1.TabIndex = 6;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(316, 609);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(145, 20);
            this.dateTimePicker2.TabIndex = 7;
            this.dateTimePicker2.ValueChanged += new System.EventHandler(this.dateTimePicker2_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(172, 609);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "label2";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(691, 604);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(142, 25);
            this.button4.TabIndex = 9;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 641);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Управление финансами";
            this.Load += new System.EventHandler(this.myFinances_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button button1;
        public System.Windows.Forms.Button button2;
        public System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.DataGridViewButtonColumn ColumnAction;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSum;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnComment;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnData;
        private System.Windows.Forms.DataGridViewTextBoxColumn Persentage;
    }
}

