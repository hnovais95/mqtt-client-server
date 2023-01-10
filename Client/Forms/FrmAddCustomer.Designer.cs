
namespace Client
{
    partial class FrmAddCustomer
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
            this.txtCustomerID = new System.Windows.Forms.TextBox();
            this.txtContactName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCompanyName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtContactTitle = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCountry = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtZipcode = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtRegion = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtFax = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID";
            // 
            // txtCustomerID
            // 
            this.txtCustomerID.Location = new System.Drawing.Point(30, 81);
            this.txtCustomerID.Name = "txtCustomerID";
            this.txtCustomerID.Size = new System.Drawing.Size(202, 27);
            this.txtCustomerID.TabIndex = 1;
            this.txtCustomerID.TextChanged += new System.EventHandler(this.FrmAddCustomer_TextChanged);
            // 
            // txtContactName
            // 
            this.txtContactName.Location = new System.Drawing.Point(30, 147);
            this.txtContactName.Name = "txtContactName";
            this.txtContactName.Size = new System.Drawing.Size(202, 27);
            this.txtContactName.TabIndex = 3;
            this.txtContactName.TextChanged += new System.EventHandler(this.FrmAddCustomer_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Nome";
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.Location = new System.Drawing.Point(30, 223);
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.Size = new System.Drawing.Size(202, 27);
            this.txtCompanyName.TabIndex = 5;
            this.txtCompanyName.TextChanged += new System.EventHandler(this.FrmAddCustomer_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 200);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Nome da empresa";
            // 
            // txtContactTitle
            // 
            this.txtContactTitle.Location = new System.Drawing.Point(30, 291);
            this.txtContactTitle.Name = "txtContactTitle";
            this.txtContactTitle.Size = new System.Drawing.Size(202, 27);
            this.txtContactTitle.TabIndex = 7;
            this.txtContactTitle.TextChanged += new System.EventHandler(this.FrmAddCustomer_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 268);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Profissão";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(30, 362);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(202, 27);
            this.txtAddress.TabIndex = 9;
            this.txtAddress.TextChanged += new System.EventHandler(this.FrmAddCustomer_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 339);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 20);
            this.label5.TabIndex = 8;
            this.label5.Text = "Endereço";
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(286, 362);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(202, 27);
            this.txtPhone.TabIndex = 19;
            this.txtPhone.TextChanged += new System.EventHandler(this.FrmAddCustomer_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(286, 339);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 20);
            this.label6.TabIndex = 18;
            this.label6.Text = "Telefone";
            // 
            // txtCountry
            // 
            this.txtCountry.Location = new System.Drawing.Point(286, 291);
            this.txtCountry.Name = "txtCountry";
            this.txtCountry.Size = new System.Drawing.Size(202, 27);
            this.txtCountry.TabIndex = 17;
            this.txtCountry.TextChanged += new System.EventHandler(this.FrmAddCustomer_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(286, 268);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 20);
            this.label7.TabIndex = 16;
            this.label7.Text = "País";
            // 
            // txtZipcode
            // 
            this.txtZipcode.Location = new System.Drawing.Point(286, 223);
            this.txtZipcode.Name = "txtZipcode";
            this.txtZipcode.Size = new System.Drawing.Size(202, 27);
            this.txtZipcode.TabIndex = 15;
            this.txtZipcode.TextChanged += new System.EventHandler(this.FrmAddCustomer_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(286, 200);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 20);
            this.label8.TabIndex = 14;
            this.label8.Text = "CEP";
            // 
            // txtRegion
            // 
            this.txtRegion.Location = new System.Drawing.Point(286, 147);
            this.txtRegion.Name = "txtRegion";
            this.txtRegion.Size = new System.Drawing.Size(202, 27);
            this.txtRegion.TabIndex = 13;
            this.txtRegion.TextChanged += new System.EventHandler(this.FrmAddCustomer_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(286, 124);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 20);
            this.label9.TabIndex = 12;
            this.label9.Text = "Estado";
            // 
            // txtCity
            // 
            this.txtCity.Location = new System.Drawing.Point(286, 81);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(202, 27);
            this.txtCity.TabIndex = 11;
            this.txtCity.TextChanged += new System.EventHandler(this.FrmAddCustomer_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(286, 58);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(56, 20);
            this.label10.TabIndex = 10;
            this.label10.Text = "Cidade";
            // 
            // txtFax
            // 
            this.txtFax.Location = new System.Drawing.Point(542, 81);
            this.txtFax.Name = "txtFax";
            this.txtFax.Size = new System.Drawing.Size(202, 27);
            this.txtFax.TabIndex = 21;
            this.txtFax.TextChanged += new System.EventHandler(this.FrmAddCustomer_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(542, 58);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(30, 20);
            this.label11.TabIndex = 20;
            this.label11.Text = "Fax";
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(542, 147);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(202, 48);
            this.button1.TabIndex = 22;
            this.button1.Text = "Cadastrar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // FrmAddCustomer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtFax);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtCountry);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtZipcode);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtRegion);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtCity);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtContactTitle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtCompanyName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtContactName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCustomerID);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAddCustomer";
            this.Text = "Cadastrar cliente";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCustomerID;
        private System.Windows.Forms.TextBox txtContactName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCompanyName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtContactTitle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCountry;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtZipcode;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtRegion;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtCity;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtFax;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button1;
    }
}