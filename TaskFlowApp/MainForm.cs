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
                MessageBox.Show($"Помилка завантаження завдань: {ex.Message}", "Помилка",
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
                MessageBox.Show($"Помилка збереження завдань: {ex.Message}", "Помилка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGrid()
        {
            dataGridTasks.AutoGenerateColumns = false;
            dataGridTasks.Columns.Clear();

            // Додаємо колонки
            dataGridTasks.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Назва",
                DataPropertyName = "Name",
                Width = 200,
                ReadOnly = true
            });

            dataGridTasks.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "Опис",
                DataPropertyName = "Description",
                Width = 250,
                ReadOnly = true
            });

            dataGridTasks.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "Статус",
                DataPropertyName = "Status",
                Width = 100,
                ReadOnly = true
            });

            dataGridTasks.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DueDate",
                HeaderText = "Термін",
                DataPropertyName = "DueDate",
                Width = 100,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd.MM.yyyy" }
            });

            dataGridTasks.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Tag",
                HeaderText = "Тег",
                DataPropertyName = "Tag",
                Width = 100,
                ReadOnly = true
            });

            // Додаємо кнопки редагування та видалення
            var editButtonColumn = new DataGridViewButtonColumn
            {
                Name = "Edit",
                HeaderText = "Дія",
                Text = "Редагувати",
                UseColumnTextForButtonValue = true,
                Width = 80
            };
            dataGridTasks.Columns.Add(editButtonColumn);

            var deleteButtonColumn = new DataGridViewButtonColumn
            {
                Name = "Delete",
                HeaderText = "",
                Text = "Видалити",
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
                    // Знаходимо індекс в основному списку
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
                var result = MessageBox.Show("Ви впевнені, що хочете видалити це завдання?",
                    "Підтвердження видалення", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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

            // Фільтр за статусом
            if (cmbStatus.SelectedIndex > 0)
            {
                string selectedStatus = cmbStatus.SelectedItem.ToString();
                filteredTasks = filteredTasks.Where(t => t.Status == selectedStatus).ToList();
            }

            // Фільтр за пошуком
            if (!string.IsNullOrWhiteSpace(txtSearch.Text) && txtSearch.Text != "Пошук")
            {
                string searchText = txtSearch.Text.ToLower();
                filteredTasks = filteredTasks.Where(t =>
                    t.Name.ToLower().Contains(searchText) ||
                    t.Description.ToLower().Contains(searchText) ||
                    t.Tag.ToLower().Contains(searchText)
                ).ToList();
            }

            // Сортування
            switch (cmbSort.SelectedIndex)
            {
                case 0: // За датою
                    filteredTasks = filteredTasks.OrderBy(t => t.DueDate).ToList();
                    break;
                case 1: // За назвою
                    filteredTasks = filteredTasks.OrderBy(t => t.Name).ToList();
                    break;
                case 2: // За статусом
                    filteredTasks = filteredTasks.OrderBy(t => t.Status).ToList();
                    break;
            }
        }

        private void UpdateButtonStates()
        {
            btnSave.Visible = isEditing;
            btnCancel.Visible = isEditing;
        }

        // Обробники подій меню
        private void усіЗавданняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "Усі завдання";
            cmbStatus.SelectedIndex = 0;
            ApplyFilters();
            UpdateTaskDisplay();
        }

        private void сьогодніToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "Сьогодні";
            filteredTasks = tasks.Where(t => t.DueDate.Date == DateTime.Today).ToList();
            UpdateTaskDisplay();
        }

        private void тегиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "Теги";
            // Тут можна реалізувати вибір за тегами
            ApplyFilters();
            UpdateTaskDisplay();
        }

        private void завершеноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "Завершено";
            filteredTasks = tasks.Where(t => t.Status == "Зроблено").ToList();
            UpdateTaskDisplay();
        }

        private void налаштуванняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Налаштування будуть додані в наступній версії", "Інформація",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Обробники подій кнопок
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
            MessageBox.Show("Зміни збережено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        // Обробники подій фільтрів
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

        // Обробники подій пошуку
        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Пошук")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Пошук";
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