namespace WinFormClient
{
    partial class UpdatePersonForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            buttonUpdate = new Button();
            BirthDate = new DateTimePicker();
            NationalCode = new TextBox();
            Lastname = new TextBox();
            FirstName = new TextBox();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(buttonUpdate);
            groupBox1.Controls.Add(BirthDate);
            groupBox1.Controls.Add(NationalCode);
            groupBox1.Controls.Add(Lastname);
            groupBox1.Controls.Add(FirstName);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.RightToLeft = RightToLeft.Yes;
            groupBox1.Size = new Size(718, 318);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "ویرایش کاربر";
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
            // 
            // buttonUpdate
            // 
            buttonUpdate.Location = new Point(6, 289);
            buttonUpdate.Name = "buttonUpdate";
            buttonUpdate.Size = new Size(159, 23);
            buttonUpdate.TabIndex = 4;
            buttonUpdate.Text = "ذخیره تغییرات";
            buttonUpdate.UseVisualStyleBackColor = true;
            buttonUpdate.Click += buttonUpdate_Click;
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
            NationalCode.TextChanged += NationalCode_TextChanged;
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
            // 
            // UpdatePersonForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(742, 342);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "UpdatePersonForm";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterParent;
            Text = "ویرایش کاربر";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private Button buttonUpdate;
        private DateTimePicker BirthDate;
        private TextBox NationalCode;
        private TextBox Lastname;
        private TextBox FirstName;
    }
}
