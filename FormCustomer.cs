﻿/*	
	DSA Lims - Laboratory Information Management System
    Copyright (C) 2018  Norwegian Radiation Protection Authority

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
// Authors: Dag Robole,

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace DSA_lims
{
    public partial class FormCustomer : Form
    {
        private Dictionary<string, object> p = new Dictionary<string, object>();

        public Guid CustomerId
        {
            get { return p.ContainsKey("id") ? (Guid)p["id"] : Guid.Empty; }
        }

        public string CustomerName
        {
            get { return p.ContainsKey("name") ? p["name"].ToString() : String.Empty; }
        }

        public FormCustomer()
        {
            InitializeComponent();

            Text = "DSA-Lims - New customer";
            using (SqlConnection conn = DB.OpenConnection())
            {
                UI.PopulateComboBoxes(conn, "csp_select_persons_short", new SqlParameter[] {}, cboxPerson);

                UI.PopulateComboBoxes(conn, "csp_select_companies_short", new[] {
                    new SqlParameter("instance_status_level", InstanceStatus.Active)
                }, cboxCompany);

                cboxInstanceStatus.DataSource = DB.GetIntLemmata(conn, "csp_select_instance_status");
            }            
            cboxInstanceStatus.SelectedValue = InstanceStatus.Active;
        }

        public FormCustomer(Guid customerId)
        {
            InitializeComponent();

            Text = "DSA-Lims - Edit customer";
            p["id"] = customerId;

            using (SqlConnection conn = DB.OpenConnection())
            {
                UI.PopulateComboBoxes(conn, "csp_select_persons_short", new SqlParameter[] { }, cboxPerson);

                UI.PopulateComboBoxes(conn, "csp_select_companies_short", new[] {
                    new SqlParameter("instance_status_level", InstanceStatus.Active)
                }, cboxCompany);

                cboxInstanceStatus.DataSource = DB.GetIntLemmata(conn, "csp_select_instance_status");

                SqlCommand cmd = new SqlCommand("csp_select_customer", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", p["id"]);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                        throw new Exception("Customer with ID " + p["id"] + " was not found");

                    reader.Read();
                    cboxPerson.SelectedValue = reader["person_id"];
                    cboxCompany.SelectedValue = reader["company_id"];
                    cboxInstanceStatus.SelectedValue = reader["instance_status_id"];
                    tbComment.Text = reader["comment"].ToString();
                    p["create_date"] = reader["create_date"];
                    p["created_by"] = reader["created_by"];
                    p["update_date"] = reader["update_date"];
                    p["updated_by"] = reader["updated_by"];
                }
            }
        }

        private void FormCustomer_Load(object sender, EventArgs e)
        {            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!Utils.IsValidGuid(cboxPerson.SelectedValue))
            {
                MessageBox.Show("Person is mandatory");
                return;
            }

            p["person_id"] = cboxPerson.SelectedValue;
            p["company_id"] = cboxCompany.SelectedValue;
            p["instance_status_id"] = cboxInstanceStatus.SelectedValue;
            p["comment"] = tbComment.Text.Trim();

            bool success;
            if (!p.ContainsKey("id"))
                success = InsertCustomer();
            else
                success = UpdateCustomer();

            DialogResult = success ? DialogResult.OK : DialogResult.Abort;
            Close();
        }

        private bool InsertCustomer()
        {
            SqlConnection connection = null;
            SqlTransaction transaction = null;

            try
            {
                p["create_date"] = DateTime.Now;
                p["created_by"] = Common.Username;
                p["update_date"] = DateTime.Now;
                p["updated_by"] = Common.Username;

                connection = DB.OpenConnection();
                transaction = connection.BeginTransaction();

                SqlCommand cmd = new SqlCommand("csp_insert_customer", connection, transaction);
                cmd.CommandType = CommandType.StoredProcedure;
                p["id"] = Guid.NewGuid();
                cmd.Parameters.AddWithValue("@id", p["id"]);
                cmd.Parameters.AddWithValue("@person_id", DB.MakeParam(typeof(Guid), p["person_id"]));
                cmd.Parameters.AddWithValue("@company_id", DB.MakeParam(typeof(Guid), p["company_id"]));
                cmd.Parameters.AddWithValue("@instance_status_id", p["instance_status_id"]);
                cmd.Parameters.AddWithValue("@comment", p["comment"]);
                cmd.Parameters.AddWithValue("@create_date", p["create_date"]);
                cmd.Parameters.AddWithValue("@created_by", p["created_by"]);
                cmd.Parameters.AddWithValue("@update_date", p["update_date"]);
                cmd.Parameters.AddWithValue("@updated_by", p["updated_by"]);
                cmd.ExecuteNonQuery();

                DB.AddAuditMessage(connection, transaction, "customer", (Guid)p["id"], AuditOperationType.Insert, JsonConvert.SerializeObject(p));

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                Common.Log.Error(ex);
                return false;
            }
            finally
            {
                connection?.Close();
            }

            return true;
        }

        private bool UpdateCustomer()
        {
            SqlConnection connection = null;
            SqlTransaction transaction = null;

            try
            {
                p["update_date"] = DateTime.Now;
                p["updated_by"] = Common.Username;

                connection = DB.OpenConnection();
                transaction = connection.BeginTransaction();

                SqlCommand cmd = new SqlCommand("csp_update_customer", connection, transaction);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", p["id"]);
                cmd.Parameters.AddWithValue("@company_id", DB.MakeParam(typeof(Guid), p["company_id"]));
                cmd.Parameters.AddWithValue("@instance_status_id", p["instance_status_id"]);
                cmd.Parameters.AddWithValue("@comment", p["comment"]);
                cmd.Parameters.AddWithValue("@update_date", p["update_date"]);
                cmd.Parameters.AddWithValue("@updated_by", p["updated_by"]);
                cmd.ExecuteNonQuery();

                DB.AddAuditMessage(connection, transaction, "customer", (Guid)p["id"], AuditOperationType.Update, JsonConvert.SerializeObject(p));

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                Common.Log.Error(ex);
                return false;
            }
            finally
            {
                connection?.Close();
            }

            return true;
        }        
    }
}
