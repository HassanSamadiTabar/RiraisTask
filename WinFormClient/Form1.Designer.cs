namespace WinFormClient
{
    partial class AddAndGetPersonForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            button1 = new Button();
            BirthDate = new DateTimePicker();
            NationalCode = new TextBox();
            Lastname = new TextBox();
            FirstName = new TextBox();
            dataGridView1 = new DataGridView();
            button2 = new Button();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(BirthDate);
            groupBox1.Controls.Add(NationalCode);
            groupBox1.Controls.Add(Lastname);
            groupBox1.Controls.Add(FirstName);
            groupBox1.Location = new Point(33, 68);
            groupBox1.Name = "groupBox1";
            groupBox1.RightToLeft = RightToLeft.Yes;
            groupBox1.Size = new Size(718, 318);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "افزودن کاربر";
            groupBox1.Enter += groupBox1_Enter;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(636, 217);
            label4.Name = "label4";
            label4.Size = new Size(54, 15);
            label4.TabIndex = 8;
            label4.Text = "تاریخ تولد";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(647, 157);
            label3.Name = "label3";
            label3.Size = new Size(43, 15);
            label3.TabIndex = 7;
            label3.Text = "کد ملی";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(621, 93);
            label2.Name = "label2";
            label2.Size = new Size(69, 15);
            label2.TabIndex = 6;
            label2.Text = "نام خانوادگی";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(669, 19);
            label1.Name = "label1";
            label1.Size = new Size(21, 15);
            label1.TabIndex = 5;
            label1.Text = "نام";
            label1.Click += label1_Click;
            // 
            // button1
            // 
            button1.Location = new Point(6, 289);
            button1.Name = "button1";
            button1.Size = new Size(159, 23);
            button1.TabIndex = 4;
            button1.Text = "افزودن کاربر";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // BirthDate
            // 
            BirthDate.Location = new Point(490, 235);
            BirthDate.Name = "BirthDate";
            BirthDate.Size = new Size(200, 23);
            BirthDate.TabIndex = 3;
            // 
            // NationalCode
            // 
            NationalCode.Location = new Point(542, 175);
            NationalCode.Name = "NationalCode";
            NationalCode.RightToLeft = RightToLeft.Yes;
            NationalCode.Size = new Size(148, 23);
            NationalCode.TabIndex = 2;
            // 
            // Lastname
            // 
            Lastname.Location = new Point(542, 111);
            Lastname.Name = "Lastname";
            Lastname.RightToLeft = RightToLeft.Yes;
            Lastname.Size = new Size(148, 23);
            Lastname.TabIndex = 1;
            // 
            // FirstName
            // 
            FirstName.Location = new Point(542, 37);
            FirstName.Name = "FirstName";
            FirstName.RightToLeft = RightToLeft.Yes;
            FirstName.Size = new Size(148, 23);
            FirstName.TabIndex = 0;
            FirstName.TextChanged += FirstName_TextChanged;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(33, 429);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(718, 371);
            dataGridView1.TabIndex = 1;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // button2
            // 
            button2.Location = new Point(592, 400);
            button2.Name = "button2";
            button2.Size = new Size(159, 23);
            button2.TabIndex = 9;
            button2.Text = "دریافت کاربران از سرور";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // AddAndGetPersonForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(797, 821);
            Controls.Add(button2);
            Controls.Add(dataGridView1);
            Controls.Add(groupBox1);
            Name = "AddAndGetPersonForm";
            Text = "AddAndGetPersonForm";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private TextBox NationalCode;
        private TextBox Lastname;
        private TextBox FirstName;
        private Button button1;
        private DateTimePicker BirthDate;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private DataGridView dataGridView1;
        private Button button2;
    }
}
