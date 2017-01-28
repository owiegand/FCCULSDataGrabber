namespace FCCULSDataGrabber
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.OutputOptions = new System.Windows.Forms.CheckedListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.LicFirstNameOptions = new System.Windows.Forms.ComboBox();
            this.LicLastNameOptions = new System.Windows.Forms.ComboBox();
            this.CallSignOptions = new System.Windows.Forms.ComboBox();
            this.LicFRNOptions = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.CurrentlySearchingFor = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.button3 = new System.Windows.Forms.Button();
            this.FilePathTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 77);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(107, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Attach CSV Data";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(384, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Output CSV Options";
            // 
            // OutputOptions
            // 
            this.OutputOptions.CheckOnClick = true;
            this.OutputOptions.FormattingEnabled = true;
            this.OutputOptions.Items.AddRange(new object[] {
            "License Name",
            "FRN",
            "Call Sign",
            "Status",
            "Experation Date",
            "License ID",
            "License URL Page",
            "Radio Service",
            "Status",
            "License Grant Date",
            "License Effective Date",
            "License Expiration Date",
            "License Cancellation Date",
            "Licensee Address",
            "Operator Class",
            "Group"});
            this.OutputOptions.Location = new System.Drawing.Point(387, 28);
            this.OutputOptions.Name = "OutputOptions";
            this.OutputOptions.Size = new System.Drawing.Size(176, 214);
            this.OutputOptions.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(387, 246);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(99, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Generate CSV";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 152);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "CSV File Mapping";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 174);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Lic First Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 198);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Lic Last Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 223);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Call Sign";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 249);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "FRN";
            // 
            // LicFirstNameOptions
            // 
            this.LicFirstNameOptions.Enabled = false;
            this.LicFirstNameOptions.FormattingEnabled = true;
            this.LicFirstNameOptions.Location = new System.Drawing.Point(95, 171);
            this.LicFirstNameOptions.Name = "LicFirstNameOptions";
            this.LicFirstNameOptions.Size = new System.Drawing.Size(121, 21);
            this.LicFirstNameOptions.TabIndex = 6;
            // 
            // LicLastNameOptions
            // 
            this.LicLastNameOptions.Enabled = false;
            this.LicLastNameOptions.FormattingEnabled = true;
            this.LicLastNameOptions.Location = new System.Drawing.Point(95, 195);
            this.LicLastNameOptions.Name = "LicLastNameOptions";
            this.LicLastNameOptions.Size = new System.Drawing.Size(121, 21);
            this.LicLastNameOptions.TabIndex = 6;
            // 
            // CallSignOptions
            // 
            this.CallSignOptions.FormattingEnabled = true;
            this.CallSignOptions.Location = new System.Drawing.Point(95, 220);
            this.CallSignOptions.Name = "CallSignOptions";
            this.CallSignOptions.Size = new System.Drawing.Size(121, 21);
            this.CallSignOptions.TabIndex = 6;
            // 
            // LicFRNOptions
            // 
            this.LicFRNOptions.FormattingEnabled = true;
            this.LicFRNOptions.Location = new System.Drawing.Point(95, 246);
            this.LicFRNOptions.Name = "LicFRNOptions";
            this.LicFRNOptions.Size = new System.Drawing.Size(121, 21);
            this.LicFRNOptions.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(384, 272);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(120, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Currently Searching For:";
            // 
            // CurrentlySearchingFor
            // 
            this.CurrentlySearchingFor.AutoSize = true;
            this.CurrentlySearchingFor.Location = new System.Drawing.Point(384, 288);
            this.CurrentlySearchingFor.Name = "CurrentlySearchingFor";
            this.CurrentlySearchingFor.Size = new System.Drawing.Size(67, 13);
            this.CurrentlySearchingFor.TabIndex = 8;
            this.CurrentlySearchingFor.Text = "Not Running";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(15, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(117, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "Select Save Location";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // FilePathTextBox
            // 
            this.FilePathTextBox.Location = new System.Drawing.Point(15, 42);
            this.FilePathTextBox.Name = "FilePathTextBox";
            this.FilePathTextBox.ReadOnly = true;
            this.FilePathTextBox.Size = new System.Drawing.Size(290, 20);
            this.FilePathTextBox.TabIndex = 10;
            this.FilePathTextBox.Text = "None";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 310);
            this.Controls.Add(this.FilePathTextBox);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.CurrentlySearchingFor);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.LicFRNOptions);
            this.Controls.Add(this.CallSignOptions);
            this.Controls.Add(this.LicLastNameOptions);
            this.Controls.Add(this.LicFirstNameOptions);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.OutputOptions);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "FCC ULS Searcher";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox OutputOptions;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox LicFirstNameOptions;
        private System.Windows.Forms.ComboBox LicLastNameOptions;
        private System.Windows.Forms.ComboBox CallSignOptions;
        private System.Windows.Forms.ComboBox LicFRNOptions;
        private System.Windows.Forms.Label CurrentlySearchingFor;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox FilePathTextBox;
    }
}

