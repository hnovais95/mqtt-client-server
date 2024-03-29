﻿using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dommel;
using Server.Domain;

namespace Server.Infra.Db
{

    public class CustomerRepository: IRepository<Customer>
    {
        public async Task<IEnumerable<Customer>> GetAll()
        {
            using var conn = DbFactory.CreateConnection();
            return await conn.GetAllAsync<Customer>();
        }

        public void Insert(Customer customer, int timeout = 0)
        {
            using var conn = DbFactory.CreateConnection();
            conn.Open();

            var sql = new StringBuilder("INSERT INTO customers(");
            sql.Append(" customer_id,");
            sql.Append(" company_name,");
            sql.Append(" contact_name,");
            sql.Append(" contact_title,");
            sql.Append(" address,");
            sql.Append(" city,");
            sql.Append(" region,");
            sql.Append(" postal_code,");
            sql.Append(" country,");
            sql.Append(" phone,");
            sql.Append(" fax");
            sql.Append(") values (");
            sql.Append(" @customer_id,");
            sql.Append(" @company_name,");
            sql.Append(" @contact_name,");
            sql.Append(" @contact_title,");
            sql.Append(" @address,");
            sql.Append(" @city,");
            sql.Append(" @region,");
            sql.Append(" @postal_code,");
            sql.Append(" @country,");
            sql.Append(" @phone,");
            sql.Append(" @fax");
            sql.Append(')');

            var cmd = DbFactory.CreateCommand(sql.ToString(), conn);
            cmd.CommandTimeout = timeout;
            cmd.Parameters.Add(DbFactory.CreateParameter("customer_id", customer.CustomerID, DbType.String));
            cmd.Parameters.Add(DbFactory.CreateParameter("company_name", customer.CompanyName, DbType.String));
            cmd.Parameters.Add(DbFactory.CreateParameter("contact_name", customer.ContactName, DbType.String));
            cmd.Parameters.Add(DbFactory.CreateParameter("contact_title", customer.ContactTitle, DbType.String));
            cmd.Parameters.Add(DbFactory.CreateParameter("address", customer.Address, DbType.String));
            cmd.Parameters.Add(DbFactory.CreateParameter("city", customer.City, DbType.String));
            cmd.Parameters.Add(DbFactory.CreateParameter("region", customer.Region, DbType.String));
            cmd.Parameters.Add(DbFactory.CreateParameter("postal_code", customer.PostalCode, DbType.String));
            cmd.Parameters.Add(DbFactory.CreateParameter("country", customer.Country, DbType.String));
            cmd.Parameters.Add(DbFactory.CreateParameter("phone", customer.Phone, DbType.String));
            cmd.Parameters.Add(DbFactory.CreateParameter("fax", customer.Fax, DbType.String));

            if (cmd.ExecuteNonQuery() == 0)
            {
                throw new Exception("Erro na inserção do registro na tabela customers.");
            }
        }
    }
}