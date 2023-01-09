using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Models;

namespace Client
{
    public partial class FrmAddCustomer : Form
    {
        public FrmAddCustomer()
        {
            InitializeComponent();
        }

        private void FrmAddCustomer_TextChanged(object sender, EventArgs e)
        {
            var textBoxList = Controls.OfType<TextBox>();
            var hasAnyEmptyOrNull = textBoxList.Where(x => string.IsNullOrEmpty(x.Text)).Count() > 0;
            button1.Enabled = !hasAnyEmptyOrNull;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var customer = new CustomerModel
            {
                CustomerID = txtCustomerID.Text,
                CompanyName = txtCompanyName.Text,
                ContactName = txtContactName.Text,
                ContactTitle = txtContactTitle.Text,
                Address = txtAddress.Text,
                City = txtCity.Text,
                Region = txtRegion.Text,
                PostalCode = txtZipcode.Text,
                Country = txtCountry.Text,
                Phone = txtPhone.Text,
                Fax = txtFax.Text,
            };
            Client.NotificationCenter.Publish(ClientPublishCommand.AddCustomer, customer);
        }
    }
}
