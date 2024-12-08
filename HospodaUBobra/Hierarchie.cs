using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace HospodaUBobra
{
    public partial class Hierarchie : Form
    {
        string connectionString = $"User Id=st69639;Password=Server2022;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";

        public Hierarchie()
        {
            InitializeComponent();
            comboBoxPivovary.DropDownStyle = ComboBoxStyle.DropDownList;
            LoadPivovary();
        }

        private void LoadHierarchyData(int pivovarId)
        {
            var hierarchyData = new List<string>();

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = @"
                SELECT HIERARCHY
                FROM HIERARCHIE_ZAMESTNANCI
                WHERE PIVOVAR_ID_PIVOVARU = :PivovarId";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("PivovarId", pivovarId));

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                hierarchyData.Add(reader.GetString(0));
                            }
                        }
                    }

                    DisplayHierarchy(hierarchyData);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Chyba při načítání hierarchie: {ex.Message}");
                }
            }
        }

        private void DisplayHierarchy(List<string> hierarchyData)
        {
            treeView.Nodes.Clear();

            foreach (var hierarchyPath in hierarchyData)
            {
                string[] nodes = hierarchyPath.Split(new[] { " -> " }, StringSplitOptions.RemoveEmptyEntries);
                TreeNode currentNode = null;

                foreach (string nodeName in nodes)
                {
                    if (currentNode == null)
                    {
                        var existingRoot = treeView.Nodes.Cast<TreeNode>()
                            .FirstOrDefault(n => n.Text == nodeName);

                        if (existingRoot == null)
                        {
                            currentNode = treeView.Nodes.Add(nodeName);
                        }
                        else
                        {
                            currentNode = existingRoot;
                        }
                    }
                    else
                    {
                        var existingChild = currentNode.Nodes.Cast<TreeNode>()
                            .FirstOrDefault(n => n.Text == nodeName);

                        if (existingChild == null)
                        {
                            currentNode = currentNode.Nodes.Add(nodeName);
                        }
                        else
                        {
                            currentNode = existingChild;
                        }
                    }
                }
            }

            treeView.ExpandAll(); 
        }
    


    public class Employee
        {
            public int ID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int PositionID { get; set; }
        }

        private void LoadPivovary()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = @"
                        SELECT P.ID_PIVOVARU, P.NAZEV 
                        FROM PIVOVARY P
                        WHERE EXISTS (
                            SELECT 1 FROM PRACOVNICI PR WHERE PR.PIVOVAR_ID_PIVOVARU = P.ID_PIVOVARU
                        )";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            comboBoxPivovary.Items.Clear();

                            var pivovars = new Dictionary<int, string>();

                            while (reader.Read())
                            {
                                int pivovarId = reader.GetInt32(0);
                                string pivovarName = reader.GetString(1);

                                pivovars.Add(pivovarId, pivovarName);

                                comboBoxPivovary.Items.Add(new KeyValuePair<int, string>(pivovarId, pivovarName));
                            }

                            comboBoxPivovary.DisplayMember = "Value"; 
                            comboBoxPivovary.ValueMember = "Key";

                            if (comboBoxPivovary.Items.Count > 0)
                            {
                                comboBoxPivovary.SelectedIndex = 0;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Chyba při načítání pivovarů: {ex.Message}");
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void AddChildNodes(TreeNode parentNode, Position parentPosition, Dictionary<int, Position> positionLookup, List<Employee> employees)
        {
            foreach (var childPosition in positionLookup.Values)
            {
                if (childPosition.ParentID == parentPosition.ID)
                {
                    var childNode = new TreeNode(childPosition.Name);
                    childNode.Tag = childPosition;
                    parentNode.Nodes.Add(childNode);
                    AddChildNodes(childNode, childPosition, positionLookup, employees);
                }
            }

            foreach (var employee in employees)
            {
                if (employee.PositionID == parentPosition.ID)
                {
                    var employeeNode = new TreeNode($"{employee.FirstName} {employee.LastName}");
                    employeeNode.Tag = employee;
                    parentNode.Nodes.Add(employeeNode);
                }
            }
        }

        public class Position
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int? ParentID { get; set; }
        }

        private void btnback_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void comboBoxPivovary_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPivovary.SelectedItem != null)
            {
                var selectedPivovar = (KeyValuePair<int, string>)comboBoxPivovary.SelectedItem;
                int pivovarId = selectedPivovar.Key;

                LoadHierarchyData(pivovarId);
                treeView.ExpandAll();
            }
        }
    }
}
