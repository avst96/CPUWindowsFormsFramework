﻿using System.Data;

namespace CPUWindowsFormsFramework
{
    public class WindowsFormsUtility
    {

        public static void SetListBinding(ComboBox lst, DataTable sourcedt, BindingSource? bindsource, string tablename)
        {
            lst.DataSource = sourcedt;
            lst.ValueMember = tablename + "ID";
            lst.DisplayMember = lst.Name.Substring(3);
            if (bindsource != null)
            {
                lst.DataBindings.Add("SelectedValue", bindsource, lst.ValueMember, false, DataSourceUpdateMode.OnPropertyChanged);
            }
        }
        public static void SetControlBinding(Control ctrl, BindingSource bindsource)
        {
            string propertyname = "";
            string columnname = ctrl.Name.ToLower().Substring(3);
            string controltype = ctrl.Name.Substring(0, 3);

            switch (controltype)
            {
                case "txt":
                case "lbl":
                    propertyname = "Text";
                    break;
                case "dtp":
                    propertyname = "Value";
                    break;
                case "chk":
                    propertyname = "Checked";
                    break;
            }

            if (propertyname != "" && columnname != "")
            {
                ctrl.DataBindings.Add(propertyname, bindsource, columnname, true, DataSourceUpdateMode.OnPropertyChanged);
            }

        }

        public static void FormatGridForSearchResults(DataGridView grid)
        {
            grid.AllowUserToAddRows = false;
            grid.ReadOnly = true;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DoFormatGrid(grid);
        }
        public static void FormatGridForEdit(DataGridView grid)
        {
            grid.EditMode = DataGridViewEditMode.EditOnEnter;
            DoFormatGrid(grid);
        }

        private static void DoFormatGrid(DataGridView grid)
        {
            foreach (DataGridViewColumn column in grid.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            grid.RowHeadersWidth = 25;
            foreach (DataGridViewColumn col in grid.Columns)
            {
                if (col.Name.EndsWith("Id") || col.Name.EndsWith("ID"))
                {
                    col.Visible = false;
                }
            }
        }


        public static int GetIdFromGrid(DataGridView grid, int rowindex, string columnname)
        {
            int id = 0;
            if (grid.Rows.Count > rowindex && grid.Columns.Contains(columnname) && grid.Rows[rowindex].Cells[columnname].Value != DBNull.Value)
            {
                if (grid.Columns[columnname].ValueType == typeof(int))
                {
                    id = (int)grid.Rows[rowindex].Cells[columnname].Value;
                }
            }
            return id;
        }

        public static int GetIdFromComboBox(ComboBox lst)
        {
            int value = 0;
            if (lst.SelectedValue != null && lst.SelectedValue is int)
            {
                value = (int)lst.SelectedValue;
            }
            return value;
        }
        public static void AddComboBoxToGrid(DataGridView grid, DataTable datasource, string tablename, string displaymember)
        {
            DataGridViewComboBoxColumn c = new();
            c.DataSource = datasource;
            c.DisplayMember = displaymember;
            c.ValueMember = tablename + "Id";
            c.DataPropertyName = c.ValueMember;
            c.HeaderText = tablename;
            grid.Columns.Insert(0, c);
        }
        public static void AddDeleteButtonToGrid(DataGridView grid, string deletecolname)
        {
            grid.Columns.Add(new DataGridViewButtonColumn() { Text = "X", HeaderText = "Delete", UseColumnTextForButtonValue = true, Name = deletecolname });
        }

        public static bool IsFormOpen(Type formtype, int pkvalue = 0)
        {
            bool exists = false;
            foreach (Form frm in Application.OpenForms)
            {
                int frmpkvalue = 0;
                if (frm.Tag != null && frm.Tag is int)
                {
                    frmpkvalue = (int)frm.Tag;
                }
                if (frm.GetType() == formtype && frmpkvalue == pkvalue)
                {
                    frm.Activate();
                    exists = true;
                    break;
                }
            }
            return exists;
        }
        public static void SetUpNav(ToolStrip ts)
        {
            ts.Items.Clear();
            foreach (Form f in Application.OpenForms)
            {
                if (f.IsMdiChild)
                {
                    ToolStripButton btn = new(f.Text);
                    btn.Tag = f;
                    btn.Click += Btn_Click;
                    ts.Items.Add(btn);
                    ts.Items.Add(new ToolStripSeparator());
                }
            }
        }
        private static void Btn_Click(object? sender, EventArgs e)
        {
            if (sender != null && sender is ToolStripButton btn)
            {
                if (btn.Tag != null && btn.Tag is Form)
                {
                    ((Form)btn.Tag).Activate();
                }
            }
        }
        public static bool IsKeyIntOrControl(char c)
        {
            bool b = true;
            if (!char.IsControl(c) && !char.IsDigit(c))
            {
                b = false;
            }
            return b;
        }
        public static void GridErrorMsg(DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("A data input error occured. Ensure you are entering the correct type of input.", Application.ProductName);
            e.ThrowException = false;
        }
        public static DialogResult MessageBoxWithReset(string exmsg)
        {
           return MessageBox.Show($"{exmsg}{Environment.NewLine}Do you want to reset the grid to show only records that were saved?", Application.ProductName, MessageBoxButtons.YesNo);
        }
    }
}
