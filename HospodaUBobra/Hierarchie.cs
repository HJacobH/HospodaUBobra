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
            LoadHierarchyData();
        }

        private void LoadHierarchyData()
        {
            var positions = new List<Position>();
            var employees = new List<Employee>();

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Query to fetch positions
                    string positionQuery = "SELECT ID_POZICE, NAZEV_POZICE, PARENT_ID FROM POZICE_PRACOVNIKA";
                    using (OracleCommand positionCmd = new OracleCommand(positionQuery, conn))
                    {
                        using (OracleDataReader reader = positionCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                positions.Add(new Position
                                {
                                    ID = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    ParentID = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2)
                                });
                            }
                        }
                    }

                    // Query to fetch employees
                    string employeeQuery = "SELECT ID_ZAMESTNANCE, JMENO, PRIJMENI, POZICE FROM ZAMESTNANCI";
                    using (OracleCommand employeeCmd = new OracleCommand(employeeQuery, conn))
                    {
                        using (OracleDataReader reader = employeeCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                employees.Add(new Employee
                                {
                                    ID = reader.GetInt32(0),
                                    FirstName = reader.GetString(1),
                                    LastName = reader.GetString(2),
                                    PositionID = reader.GetInt32(3)
                                });
                            }
                        }
                    }

                    // Build the hierarchy in the TreeView
                    BuildTreeView(positions, employees);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading hierarchy data: {ex.Message}");
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public class Employee
        {
            public int ID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int PositionID { get; set; }
        }

        private void BuildTreeView(List<Position> positions, List<Employee> employees)
        {
            // Create a dictionary for easy lookup
            var positionLookup = new Dictionary<int, Position>();
            foreach (var position in positions)
            {
                positionLookup[position.ID] = position;
            }

            // Build the tree for positions
            foreach (var position in positions)
            {
                if (position.ParentID == null)
                {
                    // Top-level node
                    var rootNode = new TreeNode(position.Name);
                    rootNode.Tag = position;
                    treeView.Nodes.Add(rootNode);
                    AddChildNodes(rootNode, position, positionLookup, employees);
                }
            }
        }

        private void AddChildNodes(TreeNode parentNode, Position parentPosition, Dictionary<int, Position> positionLookup, List<Employee> employees)
        {
            // Add child positions
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

            // Add employees under the current position
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
    }
}
