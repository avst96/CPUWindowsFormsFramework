using System.Data;

namespace CPUWindowsFormsFramework
{
    public class WindowsFormsUtiity
    {

        public static void SetListBinding(ComboBox lst, DataTable sourcedt, DataTable targetdt, string tablename)
        {
            lst.DataSource = sourcedt;
            lst.ValueMember = tablename + "ID";
            lst.DisplayMember = lst.Name.Substring(3);
            lst.DataBindings.Add("SelectedValue", targetdt, lst.ValueMember, false, DataSourceUpdateMode.OnPropertyChanged);
        }
        public static void SetControlBinding(Control ctrl, DataTable dt)
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
            }

            if (propertyname != "" && columnname != "")
            {
                ctrl.DataBindings.Add(propertyname, dt, columnname, true, DataSourceUpdateMode.OnPropertyChanged);
            }

        }

        public static void FormatGridForSearchResults(DataGridView grid)
        {
            grid.AllowUserToAddRows = false;
            grid.ReadOnly = true;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

    }
}
