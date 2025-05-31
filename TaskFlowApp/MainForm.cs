using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace TaskFlowApp
{
    public partial class MainForm : Form
    {
        private List<Task> tasks;
        private List<Task> filteredTasks;
        private bool isEditing = false;
        private int editingIndex = -1;

        public MainForm()
        {
            InitializeComponent();
            tasks = new List<Task>();
            filteredTasks = new List<Task>();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadTasks();
            SetupDataGrid();
            UpdateTaskDisplay();
            UpdateButtonStates();
        }

        private void LoadTasks()
        {
            try
            {
                tasks = XmlManager.LoadTasks();
                filteredTasks = new List<Task>(tasks);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������� ������������ �������: {ex.Message}", "�������",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                tasks = new List<Task>();
                filteredTasks = new List<Task>();
            }
        }

        private void SaveTasks()
        {
            try
            {
                XmlManager.SaveTasks(tasks);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������� ���������� �������: {ex.Message}", "�������",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGrid()
        {
            dataGridTasks.AutoGenerateColumns = false;
            dataGridTasks.Columns.Clear();

            // ������ �������
            dataGridTasks.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "�����",
                DataPropertyName = "Name",
                Width = 200,
                ReadOnly = true
            });

            dataGridTasks.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "����",
                DataPropertyName = "Description",
                Width = 250,
                ReadOnly = true
            });

            dataGridTasks.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "������",
                DataPropertyName = "Status",
                Width = 100,
                ReadOnly = true
            });

            dataGridTasks.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DueDate",
                HeaderText = "�����",
                DataPropertyName = "DueDate",
                Width = 100,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd.MM.yyyy" }
            });

            dataGridTasks.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Tag",
                HeaderText = "���",
                DataPropertyName = "Tag",
                Width = 100,
                ReadOnly = true
            });

            // ������ ������ ����������� �� ���������
            var editButtonColumn = new DataGridViewButtonColumn
            {
                Name = "Edit",
                HeaderText = "ĳ�",
                Text = "����������",
                UseColumnTextForButtonValue = true,
                Width = 80
            };
            dataGridTasks.Columns.Add(editButtonColumn);

            var deleteButtonColumn = new DataGridViewButtonColumn
            {
                Name = "Delete",
                HeaderText = "",
                Text = "��������",
                UseColumnTextForButtonValue = true,
                Width = 80
            };
            dataGridTasks.Columns.Add(deleteButtonColumn);

            dataGridTasks.CellClick += DataGridTasks_CellClick;
        }

        private void DataGridTasks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < filteredTasks.Count)
            {
                if (e.ColumnIndex == dataGridTasks.Columns["Edit"].Index)
                {
                    EditTask(e.RowIndex);
                }
                else if (e.ColumnIndex == dataGridTasks.Columns["Delete"].Index)
                {
                    DeleteTask(e.RowIndex);
                }
            }
        }

        private void EditTask(int filteredIndex)
        {
            if (filteredIndex >= 0 && filteredIndex < filteredTasks.Count)
            {
                var taskToEdit = filteredTasks[filteredIndex];
                var taskForm = new TaskForm(taskToEdit);

                if (taskForm.ShowDialog() == DialogResult.OK)
                {
                    // ��������� ������ � ��������� ������
                    int originalIndex = tasks.IndexOf(taskToEdit);
                    if (originalIndex >= 0)
                    {
                        tasks[originalIndex] = taskForm.Task;
                        SaveTasks();
                        ApplyFilters();
                        UpdateTaskDisplay();
                    }
                }
            }
        }

        private void DeleteTask(int filteredIndex)
        {
            if (filteredIndex >= 0 && filteredIndex < filteredTasks.Count)
            {
                var result = MessageBox.Show("�� �������, �� ������ �������� �� ��������?",
                    "ϳ����������� ���������", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    var taskToDelete = filteredTasks[filteredIndex];
                    tasks.Remove(taskToDelete);
                    SaveTasks();
                    ApplyFilters();
                    UpdateTaskDisplay();
                }
            }
        }

        private void UpdateTaskDisplay()
        {
            dataGridTasks.DataSource = null;
            dataGridTasks.DataSource = filteredTasks;
            dataGridTasks.Refresh();
        }

        private void ApplyFilters()
        {
            filteredTasks = new List<Task>(tasks);

            // Գ���� �� ��������
            if (cmbStatus.SelectedIndex > 0)
            {
                string selectedStatus = cmbStatus.SelectedItem.ToString();
                filteredTasks = filteredTasks.Where(t => t.Status == selectedStatus).ToList();
            }

            // Գ���� �� �������
            if (!string.IsNullOrWhiteSpace(txtSearch.Text) && txtSearch.Text != "�����")
            {
                string searchText = txtSearch.Text.ToLower();
                filteredTasks = filteredTasks.Where(t =>
                    t.Name.ToLower().Contains(searchText) ||
                    t.Description.ToLower().Contains(searchText) ||
                    t.Tag.ToLower().Contains(searchText)
                ).ToList();
            }

            // ����������
            switch (cmbSort.SelectedIndex)
            {
                case 0: // �� �����
                    filteredTasks = filteredTasks.OrderBy(t => t.DueDate).ToList();
                    break;
                case 1: // �� ������
                    filteredTasks = filteredTasks.OrderBy(t => t.Name).ToList();
                    break;
                case 2: // �� ��������
                    filteredTasks = filteredTasks.OrderBy(t => t.Status).ToList();
                    break;
            }
        }

        private void UpdateButtonStates()
        {
            btnSave.Visible = isEditing;
            btnCancel.Visible = isEditing;
        }

        // ��������� ���� ����
        private void ����������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "�� ��������";
            cmbStatus.SelectedIndex = 0;
            ApplyFilters();
            UpdateTaskDisplay();
        }

        private void �������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "�������";
            filteredTasks = tasks.Where(t => t.DueDate.Date == DateTime.Today).ToList();
            UpdateTaskDisplay();
        }

        private void ����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "����";
            // ��� ����� ���������� ���� �� ������
            ApplyFilters();
            UpdateTaskDisplay();
        }

        private void ���������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "���������";
            filteredTasks = tasks.Where(t => t.Status == "��������").ToList();
            UpdateTaskDisplay();
        }

        private void ������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("������������ ������ ����� � �������� ����", "����������",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ��������� ���� ������
        private void btnNewTask_Click(object sender, EventArgs e)
        {
            var taskForm = new TaskForm();
            if (taskForm.ShowDialog() == DialogResult.OK)
            {
                tasks.Add(taskForm.Task);
                SaveTasks();
                ApplyFilters();
                UpdateTaskDisplay();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveTasks();
            isEditing = false;
            UpdateButtonStates();
            MessageBox.Show("���� ���������!", "����", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            LoadTasks();
            isEditing = false;
            editingIndex = -1;
            UpdateButtonStates();
            ApplyFilters();
            UpdateTaskDisplay();
        }

        // ��������� ���� �������
        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
            UpdateTaskDisplay();
        }

        private void cmbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
            UpdateTaskDisplay();
        }

        // ��������� ���� ������
        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "�����")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "�����";
                txtSearch.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
            UpdateTaskDisplay();
        }
    }
}