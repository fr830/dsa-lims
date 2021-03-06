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
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DSA_lims
{
    public partial class FormSelectDate : Form
    {
        private DateTime mDate = new DateTime();

        public DateTime SelectedDate 
        { 
            get { return mDate; }
        }

        public FormSelectDate()
        {
            InitializeComponent();
        }

        private void FormSelectDate_Load(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            dtSelectDate.Value = new DateTime(now.Year, now.Month, now.Day, 12, 0, 0);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            mDate = dtSelectDate.Value;
            DialogResult = DialogResult.OK;
            Close();
        }        
    }
}
