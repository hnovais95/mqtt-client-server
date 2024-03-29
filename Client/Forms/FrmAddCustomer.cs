﻿using System;
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

        private void Button1_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    var customer = new CustomerDTO
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

                    var result = Client.NotificationCenter.PublishAndWaitCallback(ClientCommand.AddCustomer, customer, 5000);

                    if (result.ResultCode == RequestResultCode.Success)
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            MessageBox.Show("Cliente adicionado com sucesso!", "Adicionar cliente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Close();
                        });
                    }
                    else
                    {
                        throw new Exception(result.Message);
                    }
                }
                catch (Exception e)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        Console.WriteLine($"Erro ao adicionar cliente. Exc.: {e}");
                        MessageBox.Show($"Erro ao adicionar cliente: {e.Message}", "Adicionar cliente", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    });
                }
            });
        }
    }
}
