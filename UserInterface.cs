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
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DSA_lims
{
    public static partial class UI    
    {
        public static void PopulateComboBoxes(SqlConnection conn, string procedure, SqlParameter[] sqlParams, params ComboBox[] cbn)
        {
            List<Lemma<Guid, string>> list = new List<Lemma<Guid, string>>();
            list.Add(new Lemma<Guid, string>(Guid.Empty, ""));

            using (SqlDataReader reader = DB.GetDataReader(conn, procedure, CommandType.StoredProcedure, sqlParams))
            {
                while (reader.Read())
                    list.Add(new Lemma<Guid, string>(Guid.Parse(reader["id"].ToString()), reader["name"].ToString()));
            }

            foreach (ComboBox cb in cbn)
            {
                cb.DataSource = new List<Lemma<Guid, string>>(list);
                cb.DisplayMember = "Name";
                cb.ValueMember = "Id";
            }
        }

        public static void PopulatePreparationUnits(SqlConnection conn, DataGridView grid)
        {
            grid.DataSource = DB.GetDataTable(conn, "csp_select_preparation_units", CommandType.StoredProcedure);

            grid.Columns["id"].Visible = false;

            grid.Columns["name"].HeaderText = "Name";
        }

        public static void PopulateActivityUnits(SqlConnection conn, DataGridView grid)
        {
            grid.DataSource = DB.GetDataTable(conn, "csp_select_activity_units_flat", CommandType.StoredProcedure);

            grid.Columns["id"].Visible = false;

            grid.Columns["name"].HeaderText = "Unit name";
            grid.Columns["convert_factor"].HeaderText = "Conv.fact.";
            grid.Columns["uniform_activity_unit_name"].HeaderText = "Uniform unit";
        }

        public static void PopulateQuantityUnits(SqlConnection conn, DataGridView grid)
        {
            grid.DataSource = DB.GetDataTable(conn, "csp_select_quantity_units", CommandType.StoredProcedure);

            grid.Columns["id"].Visible = false;

            grid.Columns["name"].HeaderText = "Name";
        }

        public static void PopulateActivityUnitTypes(SqlConnection conn, DataGridView grid)
        {
            grid.DataSource = DB.GetDataTable(conn, "csp_select_activity_unit_types", CommandType.StoredProcedure);

            grid.Columns["id"].Visible = false;

            grid.Columns["name"].HeaderText = "Name";
        }

        public static void PopulateProjectsMain(SqlConnection conn, DataGridView grid)
        {
            grid.DataSource = DB.GetDataTable(conn, "csp_select_projects_main_flat", CommandType.StoredProcedure,
                new SqlParameter("@instance_status_level", InstanceStatus.Deleted));

            grid.Columns["id"].Visible = false;
            grid.Columns["comment"].Visible = false;
            grid.Columns["create_date"].Visible = false;
            grid.Columns["created_by"].Visible = false;
            grid.Columns["update_date"].Visible = false;
            grid.Columns["updated_by"].Visible = false;

            grid.Columns["name"].HeaderText = "Name";
            grid.Columns["instance_status_name"].HeaderText = "Status";            
        }

        public static void PopulateProjectsSub(SqlConnection conn, Guid project_main_id, DataGridView grid)
        {
            grid.DataSource = DB.GetDataTable(conn, "csp_select_projects_sub_flat", CommandType.StoredProcedure,            
                new SqlParameter("@project_main_id", project_main_id),
                new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
            );

            grid.Columns["id"].Visible = false;
            grid.Columns["comment"].Visible = false;
            grid.Columns["create_date"].Visible = false;
            grid.Columns["created_by"].Visible = false;
            grid.Columns["update_date"].Visible = false;
            grid.Columns["updated_by"].Visible = false;
            grid.Columns["project_main_name"].Visible = false;

            grid.Columns["name"].HeaderText = "Name";
            grid.Columns["instance_status_name"].HeaderText = "Status";
        }

        public static void PopulateLaboratories(SqlConnection conn, int instanceStatusLevel, DataGridView grid)
        {                        
            grid.DataSource = DB.GetDataTable(conn, "csp_select_laboratories_flat", CommandType.StoredProcedure,
                new SqlParameter("@instance_status_level", instanceStatusLevel));
        
            grid.Columns["id"].Visible = false;
            grid.Columns["assignment_counter"].Visible = false;
            grid.Columns["comment"].Visible = false;
            grid.Columns["create_date"].Visible = false;
            grid.Columns["created_by"].Visible = false;
            grid.Columns["update_date"].Visible = false;
            grid.Columns["updated_by"].Visible = false;

            grid.Columns["name"].HeaderText = "Name";
            grid.Columns["name_prefix"].HeaderText = "Prefix";
            grid.Columns["address"].HeaderText = "Address";
            grid.Columns["email"].HeaderText = "Email";
            grid.Columns["phone"].HeaderText = "Phone";
            grid.Columns["instance_status_name"].HeaderText = "Status";
        }

        public static void PopulateUsers(SqlConnection conn, int instanceStatusLevel, DataGridView grid)
        {                        
            grid.DataSource = DB.GetDataTable(conn, "csp_select_accounts_flat", CommandType.StoredProcedure,
                new SqlParameter("@instance_status_level", instanceStatusLevel));

            grid.Columns["id"].Visible = false;            
            grid.Columns["create_date"].Visible = false;            
            grid.Columns["update_date"].Visible = false;

            grid.Columns["name"].HeaderText = "Name";
            grid.Columns["email"].HeaderText = "Email";
            grid.Columns["phone"].HeaderText = "Phone";
            grid.Columns["address"].HeaderText = "Address";
            grid.Columns["instance_status_name"].HeaderText = "Status";
        }

        public static void PopulateNuclides(SqlConnection conn, DataGridView grid)
        {                        
            grid.DataSource = DB.GetDataTable(conn, "csp_select_nuclides_flat", CommandType.StoredProcedure,
                new SqlParameter("@instance_status_level", InstanceStatus.Deleted));
        
            grid.Columns["id"].Visible = false;
            grid.Columns["comment"].Visible = false;
            grid.Columns["created_by"].Visible = false;
            grid.Columns["create_date"].Visible = false;
            grid.Columns["updated_by"].Visible = false;
            grid.Columns["update_date"].Visible = false;

            grid.Columns["zas"].HeaderText = "Id (ZAS)";
            grid.Columns["name"].HeaderText = "Name";
            grid.Columns["protons"].HeaderText = "Protons";
            grid.Columns["neutrons"].HeaderText = "Neutrons";
            grid.Columns["meta_stable"].HeaderText = "Meta stable";
            grid.Columns["half_life_year"].HeaderText = "T 1/2 (Years)";
            grid.Columns["instance_status_name"].HeaderText = "Status";

            grid.Columns["half_life_year"].DefaultCellStyle.Format = "E5";
        }

        public static void PopulateGeometries(SqlConnection conn, DataGridView grid)
        {            
            grid.DataSource = DB.GetDataTable(conn, "csp_select_preparation_geometries_flat", CommandType.StoredProcedure,
                new SqlParameter("@instance_status_level", InstanceStatus.Deleted));
        
            grid.Columns["id"].Visible = false;
            grid.Columns["comment"].Visible = false;
            grid.Columns["created_by"].Visible = false;
            grid.Columns["create_date"].Visible = false;
            grid.Columns["updated_by"].Visible = false;
            grid.Columns["update_date"].Visible = false;

            grid.Columns["name"].HeaderText = "Name";
            grid.Columns["min_fill_height_mm"].HeaderText = "Min Fillh. (mm)";
            grid.Columns["max_fill_height_mm"].HeaderText = "Max Fillh. (mm)";
            grid.Columns["instance_status_name"].HeaderText = "Status";
            // FIXME 
        }        

        public static void PopulateCounties(SqlConnection conn, DataGridView grid)
        {
            grid.DataSource = DB.GetDataTable(conn, "csp_select_counties_flat", CommandType.StoredProcedure,
                new SqlParameter("@instance_status_level", InstanceStatus.Deleted));
        
            grid.Columns["id"].Visible = false;
            grid.Columns["created_by"].Visible = false;
            grid.Columns["create_date"].Visible = false;
            grid.Columns["updated_by"].Visible = false;
            grid.Columns["update_date"].Visible = false;

            grid.Columns["name"].HeaderText = "Name";
            grid.Columns["county_number"].HeaderText = "Number";
            grid.Columns["instance_status_name"].HeaderText = "Status";
        }        

        public static void PopulateMunicipalities(SqlConnection conn, Guid cid, DataGridView grid)
        {            
            grid.DataSource = DB.GetDataTable(conn, "csp_select_municipalities_for_county_flat", CommandType.StoredProcedure,
                new SqlParameter("@county_id", cid),
                new SqlParameter("@instance_status_level", InstanceStatus.Deleted));
        
            grid.Columns["id"].Visible = false;
            grid.Columns["county_name"].Visible = false;
            grid.Columns["created_by"].Visible = false;
            grid.Columns["create_date"].Visible = false;
            grid.Columns["updated_by"].Visible = false;
            grid.Columns["update_date"].Visible = false;

            grid.Columns["name"].HeaderText = "Name";
            grid.Columns["municipality_number"].HeaderText = "Number";
            grid.Columns["instance_status_name"].HeaderText = "Status";
        }        

        public static void PopulateStations(SqlConnection conn, DataGridView grid)
        {
            grid.DataSource = DB.GetDataTable(conn, "csp_select_stations_flat", CommandType.StoredProcedure,
                new SqlParameter("@instance_status_level", InstanceStatus.Deleted));

            grid.Columns["id"].Visible = false;
            grid.Columns["comment"].Visible = false;
            grid.Columns["created_by"].Visible = false;
            grid.Columns["create_date"].Visible = false;
            grid.Columns["updated_by"].Visible = false;
            grid.Columns["update_date"].Visible = false;

            grid.Columns["name"].HeaderText = "Name";
            grid.Columns["latitude"].HeaderText = "Latitude";
            grid.Columns["longitude"].HeaderText = "Longitude";
            grid.Columns["altitude"].HeaderText = "Altitude";
            grid.Columns["instance_status_name"].HeaderText = "Status";
        }        

        public static void PopulateSampleStorage(SqlConnection conn, DataGridView grid)
        {         
            grid.DataSource = DB.GetDataTable(conn, "csp_select_sample_storages_flat", CommandType.StoredProcedure,
                new SqlParameter("@instance_status_level", InstanceStatus.Deleted));
        
            grid.Columns["id"].Visible = false;
            grid.Columns["comment"].Visible = false;
            grid.Columns["created_by"].Visible = false;
            grid.Columns["create_date"].Visible = false;
            grid.Columns["updated_by"].Visible = false;
            grid.Columns["update_date"].Visible = false;

            grid.Columns["name"].HeaderText = "Name";
            grid.Columns["address"].HeaderText = "Address";
            grid.Columns["instance_status_name"].HeaderText = "Status";
        }        

        public static void PopulateSamplers(SqlConnection conn, DataGridView grid)
        {
            grid.DataSource = DB.GetDataTable(conn, "csp_select_samplers_flat", CommandType.StoredProcedure,
                new SqlParameter("@instance_status_level", InstanceStatus.Deleted));

            grid.Columns["id"].Visible = false;

            grid.Columns["person_name"].HeaderText = "Sampl.Name";
            grid.Columns["person_email"].HeaderText = "Sampl.Email";
            grid.Columns["person_phone"].HeaderText = "Sampl.Phone";
            grid.Columns["person_address"].HeaderText = "Sampl.Address";
            grid.Columns["company_name"].HeaderText = "Comp.Name";
            grid.Columns["company_email"].HeaderText = "Comp.Email";
            grid.Columns["company_phone"].HeaderText = "Comp.Phone";
            grid.Columns["company_address"].HeaderText = "Comp.Address";
            grid.Columns["instance_status_name"].HeaderText = "Status";
        }        

        public static void PopulateSamplingMethods(SqlConnection conn, DataGridView grid)
        {
            grid.DataSource = DB.GetDataTable(conn, "csp_select_sampling_methods_flat", CommandType.StoredProcedure,
                new SqlParameter("@instance_status_level", InstanceStatus.Deleted));

            grid.Columns["id"].Visible = false;
            grid.Columns["comment"].Visible = false;
            grid.Columns["created_by"].Visible = false;
            grid.Columns["create_date"].Visible = false;
            grid.Columns["updated_by"].Visible = false;
            grid.Columns["update_date"].Visible = false;

            grid.Columns["name"].HeaderText = "Name";
            grid.Columns["instance_status_name"].HeaderText = "Status";
        }        

        /*public static void PopulateSamples(SqlConnection conn, DataGridView grid)
        {
            grid.DataSource = DB.GetDataTable(conn, "csp_select_samples_informative", CommandType.StoredProcedure,
                new SqlParameter("@instance_status_level", InstanceStatus.Deleted));

            grid.Columns["id"].Visible = false;
            grid.Columns["merge_to"].Visible = false;

            grid.Columns["number"].HeaderText = "Sample number";
            grid.Columns["external_id"].HeaderText = "Ex.Id";
            grid.Columns["laboratory_name"].HeaderText = "Laboratory";
            grid.Columns["sample_type_name"].HeaderText = "Type";
            grid.Columns["sample_component_name"].HeaderText = "Component";
            grid.Columns["project_name"].HeaderText = "Project";
            grid.Columns["sample_storage_name"].HeaderText = "Storage";
            grid.Columns["reference_date"].HeaderText = "Ref.date";
            grid.Columns["instance_status_name"].HeaderText = "Status";
            grid.Columns["locked_by"].HeaderText = "Locked by";
            grid.Columns["split_from"].HeaderText = "Split from";            
            grid.Columns["merge_from"].HeaderText = "Merge from";

            grid.Columns["reference_date"].DefaultCellStyle.Format = Utils.DateTimeFormatNorwegian;
        }*/

        public static void PopulatePreparationMethods(SqlConnection conn, DataGridView grid)
        {
            grid.DataSource = DB.GetDataTable(conn, "csp_select_preparation_methods_flat", CommandType.StoredProcedure,
                new SqlParameter("@instance_status_level", InstanceStatus.Deleted));

            grid.Columns["id"].Visible = false;
            grid.Columns["comment"].Visible = false;
            grid.Columns["created_by"].Visible = false;
            grid.Columns["create_date"].Visible = false;
            grid.Columns["updated_by"].Visible = false;
            grid.Columns["update_date"].Visible = false;

            grid.Columns["name"].HeaderText = "Name";
            grid.Columns["description_link"].HeaderText = "Desc. link";
            grid.Columns["destructive"].HeaderText = "Destructive";
            grid.Columns["instance_status_name"].HeaderText = "Status";
        }        

        public static void PopulateAnalysisMethods(SqlConnection conn, DataGridView grid)
        {
            grid.DataSource = DB.GetDataTable(conn, "csp_select_analysis_methods_flat", CommandType.StoredProcedure,
                new SqlParameter("@instance_status_level", InstanceStatus.Deleted));

            grid.Columns["id"].Visible = false;
            grid.Columns["comment"].Visible = false;
            grid.Columns["created_by"].Visible = false;
            grid.Columns["create_date"].Visible = false;
            grid.Columns["updated_by"].Visible = false;
            grid.Columns["update_date"].Visible = false;

            grid.Columns["name"].HeaderText = "Name";
            grid.Columns["description_link"].HeaderText = "Desc. link";
            grid.Columns["specter_reference_regexp"].HeaderText = "Spec.Ref RE";
            grid.Columns["instance_status_name"].HeaderText = "Status";
        }        

        private static void AddSampleTypeChildren(List<SampleTypeModel> sampleTypeList, TreeNode tnode)
        {
            List<SampleTypeModel> children = sampleTypeList.FindAll(x => x.ParentId == Guid.Parse(tnode.Name));
            foreach (SampleTypeModel st in children)
            {
                TreeNode n = tnode.Nodes.Add(st.Id.ToString(), st.Name);
                n.ToolTipText = n.FullPath + Environment.NewLine + Environment.NewLine + "Common name: " + st.NameCommon + Environment.NewLine + "Latin name: " + st.NameLatin;
                n.Tag = n.FullPath;
                AddSampleTypeChildren(sampleTypeList, n);
            }
        }

        public static void PopulateSampleTypes(SqlConnection conn, TreeView tree)
        {            
            try
            {
                tree.Nodes.Clear();
                List<SampleTypeModel> roots = Common.SampleTypeList.FindAll(x => x.ParentId == Guid.Empty);
                foreach (SampleTypeModel st in roots)
                {
                    TreeNode n = tree.Nodes.Add(st.Id.ToString(), st.Name);
                    n.ToolTipText = n.FullPath + Environment.NewLine + Environment.NewLine + "Common name: " + st.NameCommon + Environment.NewLine + "Latin name: " + st.NameLatin;
                    n.Tag = n.FullPath;
                }

                foreach(TreeNode tnode in tree.Nodes)                
                    AddSampleTypeChildren(Common.SampleTypeList, tnode);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
        }

        private static void AddSampleTypeChildren(TreeNodeCollection tnc, List<Lemma<Guid, string>> list)
        {
            foreach (TreeNode tn in tnc)
            {
                Lemma<Guid, string> st = new Lemma<Guid, string>(Guid.Parse(tn.Name), tn.Text + " -> " + tn.Tag.ToString());
                list.Add(st);
                AddSampleTypeChildren(tn.Nodes, list);
            }
        }

        public static void PopulateSampleTypes(TreeView tree, params ComboBox[] cbn)
        {
            foreach (ComboBox cb in cbn)
            {
                List<Lemma<Guid, string>> sampleTypeList = new List<Lemma<Guid, string>>();
                sampleTypeList.Add(new Lemma<Guid, string>(Guid.Empty, ""));
                AddSampleTypeChildren(tree.Nodes, sampleTypeList);
                sampleTypeList.Sort(delegate (Lemma<Guid, string> l1, Lemma<Guid, string> l2)
                {
                    return l1.Name.CompareTo(l2.Name);
                });
                cb.DataSource = sampleTypeList;
                cb.DisplayMember = "Name";
                cb.ValueMember = "Id";
            }
        }

        public static void PopulateSampleTypePrepMeth(SqlConnection conn, TreeNode tnode, ListBox lb, ListBox lbInherited)
        {
            Guid sampleTypeId = Guid.Parse(tnode.Name);
            string sampleTypeName = tnode.Text;

            string query = @"
select pm.id, pm.name from preparation_method pm	
    inner join sample_type_x_preparation_method stpm on stpm.preparation_method_id = pm.id
    inner join sample_type st on stpm.sample_type_id = st.id and st.id = @sample_type_id
order by name";

            lb.Items.Clear();
            using (SqlDataReader reader = DB.GetDataReader(conn, query, CommandType.Text, new SqlParameter("@sample_type_id", sampleTypeId)))
            {
                while (reader.Read())
                {
                    Lemma<Guid, string> st = new Lemma<Guid, string>(Guid.Parse(reader["id"].ToString()), reader["name"].ToString());
                    lb.Items.Add(st);
                }
            }            

            lbInherited.Items.Clear();
            while (tnode.Parent != null)
            {
                tnode = tnode.Parent;
                sampleTypeId = Guid.Parse(tnode.Name);
                using (SqlDataReader reader = DB.GetDataReader(conn, query, CommandType.Text, new SqlParameter("@sample_type_id", sampleTypeId)))
                {
                    while (reader.Read())
                    {
                        Lemma<Guid, string> st = new Lemma<Guid, string>(Guid.Parse(reader["id"].ToString()), reader["name"].ToString());
                        lbInherited.Items.Add(st);
                    }
                }
            }
        }

        public static void PopulateSampleComponents(SqlConnection conn, Guid sampleTypeId, ListBox lb)
        {
            lb.Items.Clear();            

            using (SqlDataReader reader = DB.GetDataReader(conn, "csp_select_sample_components_for_sample_type", CommandType.StoredProcedure, 
                new SqlParameter("@sample_type_id", sampleTypeId)))
            {
                while (reader.Read())
                {
                    Lemma<Guid, string> sampleComponent = new Lemma<Guid, string>(
                        Guid.Parse(reader["id"].ToString()),
                        reader["name"].ToString());

                    lb.Items.Add(sampleComponent);
                }
            }
        }

        public static void PopulateSampleComponentsAscending(SqlConnection conn, Guid sampleTypeId, TreeNode tnode, ComboBox cbox)
        {
            List<Lemma<Guid, string>> comps = new List<Lemma<Guid, string>>();
            comps.Add(new Lemma<Guid, string>(Guid.Empty, ""));
            AddSampleTypeComponentsAscending(conn, sampleTypeId, tnode, comps);
            cbox.DataSource = comps;
            cbox.DisplayMember = "Name";
            cbox.ValueMember = "Id";
        }

        private static void AddSampleTypeComponentsAscending(SqlConnection conn, Guid sampleTypeId, TreeNode tnode, List<Lemma<Guid, string>> comps)
        {
            using (SqlDataReader reader = DB.GetDataReader(conn, "csp_select_sample_components_for_sample_type", CommandType.StoredProcedure,
                    new SqlParameter("@sample_type_id", sampleTypeId)))
            {
                while (reader.Read())
                    comps.Add(new Lemma<Guid, string>(Guid.Parse(reader["id"].ToString()), reader["name"].ToString()));
            }

            if (tnode.Parent != null)
            {
                Guid parentId = Guid.Parse(tnode.Parent.Name);
                AddSampleTypeComponentsAscending(conn, parentId, tnode.Parent, comps);
            }
        }

        public static void PopulateAnalMethNuclides(SqlConnection conn, Guid analysisMethodId, ListBox lb)
        {
            lb.Items.Clear();
            using (SqlDataReader reader = DB.GetDataReader(conn, "csp_select_nuclides_for_analysis_method", CommandType.StoredProcedure,
                new SqlParameter("@analysis_method_id", analysisMethodId)))
            {
                while (reader.Read())
                {
                    Lemma<Guid, string> n = new Lemma<Guid, string>(Guid.Parse(reader["id"].ToString()), reader["name"].ToString());
                    lb.Items.Add(n);
                }
            }
        }

        public static void PopulatePrepMethAnalMeths(SqlConnection conn, Guid preparationMethodId, ListBox lb)
        {
            lb.Items.Clear();
            using (SqlDataReader reader = DB.GetDataReader(conn, "csp_select_analysis_methods_for_preparation_method", CommandType.StoredProcedure,
                new SqlParameter("@preparation_method_id", preparationMethodId)))
            {
                while (reader.Read())
                {
                    Lemma<Guid, string> n = new Lemma<Guid, string>(Guid.Parse(reader["id"].ToString()), reader["name"].ToString());
                    lb.Items.Add(n);
                }
            }
        }

        public static void PopulateCustomers(SqlConnection conn, int statusLevel, DataGridView grid)
        {
            grid.DataSource = DB.GetDataTable(conn, "csp_select_customers_flat", CommandType.StoredProcedure,
                new SqlParameter("@instance_status_level", statusLevel));

            grid.Columns["id"].Visible = false;

            grid.Columns["person_name"].HeaderText = "Cont.Name";
            grid.Columns["person_email"].HeaderText = "Cont.Email";
            grid.Columns["person_phone"].HeaderText = "Cont.Phone";
            grid.Columns["person_address"].HeaderText = "Cont.Address";
            grid.Columns["company_name"].HeaderText = "Comp.Name";
            grid.Columns["company_email"].HeaderText = "Comp.Email";
            grid.Columns["company_phone"].HeaderText = "Comp.Phone";
            grid.Columns["company_address"].HeaderText = "Comp.Address";
            grid.Columns["instance_status_name"].HeaderText = "Status";
        }

        public static void PopulateOrders(SqlConnection conn, int statusLevel, Guid laboratoryId, DataGridView grid)
        {
            string query = @"
select id, name, customer_name, customer_company
from assignment a 
where laboratory_id = @laboratory_id and instance_status_id <= @instance_status_level 
order by create_date desc";

            grid.DataSource = DB.GetDataTable(conn, query, CommandType.Text,
                new SqlParameter("@laboratory_id", laboratoryId),
                new SqlParameter("@instance_status_level", statusLevel));

            grid.Columns["id"].Visible = false;            

            grid.Columns["name"].HeaderText = "Name";            
            grid.Columns["customer_name"].HeaderText = "Customer";
            grid.Columns["customer_company"].HeaderText = "Cust.Company";
        }

        public static void PopulateOrderContent(SqlConnection conn, Guid selectedOrder, TreeView tree, Guid sampleTypeId, TreeView treeSampleTypes, bool useCommentToolTips)
        {
            tree.Nodes.Clear();

            SqlDataReader astReader = null;

            try
            {
                if (sampleTypeId == Guid.Empty)
                {
                    astReader = DB.GetDataReader(conn, "csp_select_assignment_sample_types", CommandType.StoredProcedure, 
                        new SqlParameter("@assignment_id", selectedOrder));
                }
                else
                {
                    astReader = DB.GetDataReader(conn, "csp_select_assignment_sample_types_for_sample_type", CommandType.StoredProcedure, 
                        new SqlParameter("@assignment_id", selectedOrder),
                        new SqlParameter("@sample_type_id", sampleTypeId));
                }
                
                while (astReader.Read())
                {
                    string txt = astReader["sample_count"].ToString() + ", " + astReader["sample_type_name"].ToString();
                    if (astReader["sample_component_name"] != DBNull.Value)
                        txt += ", " + astReader["sample_component_name"].ToString();
                    TreeNode[] nodes = treeSampleTypes.Nodes.Find(astReader["sample_type_id"].ToString(), true);
                    if (nodes.Length > 0)
                        txt += " (" + nodes[0].FullPath + ")";

                    TreeNode tnode = tree.Nodes.Add(astReader["id"].ToString(), txt);
                    tnode.ToolTipText = useCommentToolTips ? astReader["sample_comment"].ToString() : "";
                    tnode.NodeFont = new Font(tree.Font.FontFamily, tree.Font.Size, FontStyle.Bold);
                }
            }
            catch(Exception ex)
            {
                Common.Log.Error(ex);
            }
            finally
            {
                astReader?.Close();
            }

            foreach(TreeNode tnode in tree.Nodes)
            {
                Guid orderSampleTypeId = Guid.Parse(tnode.Name);

                using (SqlDataReader reader = DB.GetDataReader(conn, "csp_select_assignment_preparation_methods", CommandType.StoredProcedure, 
                    new SqlParameter("@assignment_sample_type_id", orderSampleTypeId)))
                {
                    while (reader.Read())
                    {                        
                        string txt = reader["count"].ToString() + ", " + reader["preparation_method_name"].ToString();
                        if (reader["preparation_laboratory_id"] != DBNull.Value)
                            txt += " (" + reader["preparation_laboratory_name"].ToString() + ")";

                        TreeNode tn = tnode.Nodes.Add(reader["id"].ToString(), txt);
                        tn.ToolTipText = useCommentToolTips ? reader["comment"].ToString() : "";
                    }
                }
            }
            
            foreach (TreeNode tnode in tree.Nodes)
            {
                foreach (TreeNode tn in tnode.Nodes)
                {
                    Guid orderPrepMethId = Guid.Parse(tn.Name);

                    using (SqlDataReader reader = DB.GetDataReader(conn, "csp_select_assignment_analysis_methods", CommandType.StoredProcedure,
                        new SqlParameter("@assignment_preparation_method_id", orderPrepMethId)))
                    {
                        while (reader.Read())
                        {
                            string txt = reader["count"].ToString() + ", " + reader["analysis_method_name"].ToString();
                            TreeNode tn2 = tn.Nodes.Add(reader["id"].ToString(), txt);
                            tn2.ToolTipText = useCommentToolTips ? reader["comment"].ToString() : "";
                        }
                    }
                }
            }

            tree.ExpandAll();
        }        

        public static void PopulateOrderContentForSampleTypeName(SqlConnection conn, Guid selectedOrder, TreeView tree, Guid sampleTypeId, TreeView treeSampleTypes, bool useCommentToolTips)
        {
            tree.Nodes.Clear();

            SqlDataReader astReader = null;

            try
            {
                SqlCommand cmd = new SqlCommand("select path from sample_type where id = @id", conn);
                cmd.Parameters.AddWithValue("@id", sampleTypeId);
                object o = cmd.ExecuteScalar();
                string sampleTypeName = o.ToString();

                astReader = DB.GetDataReader(conn, "csp_select_assignment_sample_types_for_sample_type_name", CommandType.StoredProcedure,
                    new SqlParameter("@assignment_id", selectedOrder),
                    new SqlParameter("@sample_type_name", sampleTypeName));

                while (astReader.Read())
                {
                    string txt = astReader["sample_count"].ToString() + ", " + astReader["sample_type_name"].ToString();
                    if (astReader["sample_component_name"] != DBNull.Value)
                        txt += ", " + astReader["sample_component_name"].ToString();
                    TreeNode[] nodes = treeSampleTypes.Nodes.Find(astReader["sample_type_id"].ToString(), true);
                    if (nodes.Length > 0)
                        txt += " (" + nodes[0].FullPath + ")";

                    TreeNode tnode = tree.Nodes.Add(astReader["id"].ToString(), txt);
                    tnode.ToolTipText = useCommentToolTips ? astReader["sample_comment"].ToString() : "";
                    tnode.NodeFont = new Font(tree.Font.FontFamily, tree.Font.Size, FontStyle.Bold);
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
            }
            finally
            {
                astReader?.Close();
            }

            foreach (TreeNode tnode in tree.Nodes)
            {
                Guid orderSampleTypeId = Guid.Parse(tnode.Name);

                using (SqlDataReader reader = DB.GetDataReader(conn, "csp_select_assignment_preparation_methods", CommandType.StoredProcedure,
                    new SqlParameter("@assignment_sample_type_id", orderSampleTypeId)))
                {
                    while (reader.Read())
                    {
                        string txt = reader["count"].ToString() + ", " + reader["preparation_method_name"].ToString();
                        if (reader["preparation_laboratory_id"] != DBNull.Value)
                            txt += " (" + reader["preparation_laboratory_name"].ToString() + ")";

                        TreeNode tn = tnode.Nodes.Add(reader["id"].ToString(), txt);
                        tn.ToolTipText = useCommentToolTips ? reader["comment"].ToString() : "";
                    }
                }
            }

            foreach (TreeNode tnode in tree.Nodes)
            {
                foreach (TreeNode tn in tnode.Nodes)
                {
                    Guid orderPrepMethId = Guid.Parse(tn.Name);

                    using (SqlDataReader reader = DB.GetDataReader(conn, "csp_select_assignment_analysis_methods", CommandType.StoredProcedure,
                        new SqlParameter("@assignment_preparation_method_id", orderPrepMethId)))
                    {
                        while (reader.Read())
                        {
                            string txt = reader["count"].ToString() + ", " + reader["analysis_method_name"].ToString();
                            TreeNode tn2 = tn.Nodes.Add(reader["id"].ToString(), txt);
                            tn2.ToolTipText = useCommentToolTips ? reader["comment"].ToString() : "";
                        }
                    }
                }
            }

            tree.ExpandAll();
        }

        public static void PopulateAnalysisResults(SqlConnection conn, Guid analysisId, DataGridView grid)
        {
            grid.DataSource = DB.GetDataTable(conn, "csp_select_analysis_results_for_analysis_informative", CommandType.StoredProcedure,
                    new SqlParameter("@analysis_id", analysisId));

            grid.Columns["id"].Visible = false;
            grid.Columns["uniform_activity"].Visible = false;
            grid.Columns["uniform_activity_name"].Visible = false;

            grid.Columns["nuclide_name"].HeaderText = "Nucl.";
            grid.Columns["activity"].HeaderText = "Act.";
            grid.Columns["activity_uncertainty_abs"].HeaderText = "Unc.Abs.";
            grid.Columns["activity_approved"].HeaderText = "Act.Appr.";
            grid.Columns["detection_limit"].HeaderText = "Det.Lim.";
            grid.Columns["detection_limit_approved"].HeaderText = "Det.Lim.Appr.";
            grid.Columns["accredited"].HeaderText = "Accredited";
            grid.Columns["reportable"].HeaderText = "Reportable";
        }

        public static void PopulatePersons(SqlConnection conn, DataGridView grid)
        {
            grid.DataSource = DB.GetDataTable(conn, "csp_select_persons", CommandType.StoredProcedure);

            grid.Columns["id"].Visible = false;
            grid.Columns["create_date"].Visible = false;
            grid.Columns["update_date"].Visible = false;

            grid.Columns["name"].HeaderText = "Name";            
            grid.Columns["email"].HeaderText = "Email";
            grid.Columns["phone"].HeaderText = "Phone";
            grid.Columns["address"].HeaderText = "Address";            
        }

        public static void PopulateCompanies(SqlConnection conn, DataGridView grid)
        {
            grid.DataSource = DB.GetDataTable(conn, "csp_select_companies_flat", CommandType.StoredProcedure, 
                new SqlParameter("@instance_status_level", InstanceStatus.Deleted));

            grid.Columns["id"].Visible = false;
            grid.Columns["comment"].Visible = false;
            grid.Columns["create_date"].Visible = false;
            grid.Columns["created_by"].Visible = false;
            grid.Columns["update_date"].Visible = false;
            grid.Columns["updated_by"].Visible = false;

            grid.Columns["name"].HeaderText = "Name";
            grid.Columns["email"].HeaderText = "Email";
            grid.Columns["phone"].HeaderText = "Phone";
            grid.Columns["address"].HeaderText = "Address";
            grid.Columns["instance_status_name"].HeaderText = "Status";
        }

        public static void PopulateOrderYears(SqlConnection conn, ComboBox cbox)
        {
            List<string> years = new List<string>();
            years.Add("");

            using (SqlDataReader reader = DB.GetDataReader(conn, "select distinct year(create_date) as 'year' from assignment order by year", CommandType.Text))
            {
                while(reader.Read())
                {
                    years.Add(reader["year"].ToString());
                }
            }
            cbox.DataSource = years;
        }

        public static void PopulateOrderWorkflowStatus(SqlConnection conn, ComboBox cbox)
        {
            List<Lemma<int, string>> stats = new List<Lemma<int, string>>();
            stats.Add(new Lemma<int, string>(0, ""));

            using (SqlDataReader reader = DB.GetDataReader(conn, "csp_select_workflow_status", CommandType.StoredProcedure))
            {
                while (reader.Read())
                {
                    stats.Add(new Lemma<int, string>(Convert.ToInt32(reader["id"]), reader["name"].ToString()));
                }
            }
            cbox.DataSource = stats;
        }

        public static void PopulateRoles(SqlConnection conn, Guid userId, ListBox lb)
        {
            List<Lemma<Guid, string>> roles = new List<Lemma<Guid, string>>();

            string query = @"
select id, name 
from role r
    inner join account_x_role axr on axr.role_id = r.id and axr.account_id = @account_id
";
            using (SqlDataReader reader = DB.GetDataReader(conn, query, CommandType.Text, new[] {
                new SqlParameter("@account_id", userId)
            }))
            {
                while (reader.Read())
                {
                    roles.Add(new Lemma<Guid, string>(Guid.Parse(reader["id"].ToString()), reader["name"].ToString()));
                }
            }

            lb.DataSource = roles;
        }
    }
}
