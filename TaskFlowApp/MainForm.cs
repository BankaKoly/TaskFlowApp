using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TaskFlowApp
{
    public partial class MainForm : Form
    {
        private List<Task> tasks;
        private List<Task> filteredTasks;

        public MainForm()
        {
            InitializeComponent();
            InitializeDataGrid();
            LoadTasks();
        }

        private void InitializeDataGrid()
        {
            // Налаштування DataGridView
            dataGridTasks.AutoGenerateColumns = false;
            dataGridTasks.AllowUserToAddRows = false;
            dataGridTasks.AllowUserToDeleteRows = false;
            dataGridTasks.ReadOnly = true;
            dataGridTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridTasks.MultiSelect = false;

            // Додавання колонок
            dataGridTasks.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Назва",
                DataPropertyName = "Name",
                Width = 200
            });

            dataGridTasks.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "Опис",
                DataPropertyName = "Description",
                Width = 300
            });

            dataGridTasks.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "Статус",
                DataPropertyName = "Status",
                Width = 100
            });

            dataGridTasks.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DueDate",
                HeaderText = "Термін",
                DataPropertyName = "DueDate",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd.MM.yyyy" }
            });

            dataGridTasks.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Tag",
                HeaderText = "Тег",
                DataPropertyName = "Tag",
                Width = 100
            });

            // Події для подвійного кліку (редагування)
            dataGridTasks.CellDoubleClick += DataGridTasks_CellDoubleClick;
        }

        private void LoadTasks()
        {
            try
            {
                tasks = XmlManager.LoadTasks();
                filteredTasks = new List<Task>(tasks);
                UpdateDataGrid();
                cmbStatus.SelectedIndex = 0; // "Всі"
                cmbSort.SelectedIndex = 0; // "Сортувати за датою"
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження завдань: {ex.Message}",
                    "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateDataGrid()
        {
            dataGridTasks.DataSource = null;
            dataGridTasks.DataSource = filteredTasks;
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            int totalTasks = filteredTasks.Count;
            int completedTasks = filteredTasks.Count(t => t.Status == "Зроблено");
            lblTitle.Text = $"Завдання ({completedTasks}/{totalTasks})";
        }

        private void FilterTasks()
        {
            filteredTasks = new List<Task>(tasks);

            // Фільтр за статусом
            if (cmbStatus.SelectedIndex > 0)
            {
                string selectedStatus = cmbStatus.SelectedItem.ToString();
                filteredTasks = filteredTasks.Where(t => t.Status == selectedStatus).ToList();
            }

            // Фільтр за пошуком
            if (!string.IsNullOrEmpty(txtSearch.Text) && txtSearch.Text != "Пошук")
            {
                string searchText = txtSearch.Text.ToLower();
                filteredTasks = filteredTasks.Where(t =>
                    t.Name.ToLower().Contains(searchText) ||
                    t.Description.ToLower().Contains(searchText) ||
                    t.Tag.ToLower().Contains(searchText)).ToList();
            }

            // Сортування
            if (cmbSort.SelectedIndex >= 0)
            {
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

            UpdateDataGrid();
        }

        private void SaveTasks()
        {
            try
            {
                XmlManager.SaveTasks(tasks);
                MessageBox.Show("Завдання збережено успішно!", "Збереження",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка збереження: {ex.Message}",
                    "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Події кнопок
        private void btnNewTask_Click(object sender, EventArgs e)
        {
            using (TaskForm taskForm = new TaskForm())
            {
                if (taskForm.ShowDialog() == DialogResult.OK)
                {
                    tasks.Add(taskForm.Task);
                    FilterTasks();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveTasks();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Функціональність вивантаження
            MessageBox.Show("Функція вивантаження буде реалізована пізніше",
                "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // НОВА ФУНКЦІОНАЛЬНІСТЬ ВИДАЛЕННЯ
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridTasks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Будь ласка, оберіть завдання для видалення.",
                    "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedTask = (Task)dataGridTasks.SelectedRows[0].DataBoundItem;

            DialogResult result = MessageBox.Show(
                $"Ви впевнені, що хочете видалити завдання \"{selectedTask.Name}\"?",
                "Підтвердження видалення",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Видаляємо з основного списку
                    tasks.Remove(selectedTask);

                    // Оновлюємо відображення
                    FilterTasks();

                    MessageBox.Show("Завдання видалено успішно!",
                        "Видалення", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при видаленні завдання: {ex.Message}",
                        "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Події фільтрів
        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterTasks();
        }

        private void cmbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterTasks();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FilterTasks();
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Пошук")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Пошук";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        // Подвійний клік для редагування
        private void DataGridTasks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < filteredTasks.Count)
            {
                Task selectedTask = filteredTasks[e.RowIndex];
                using (TaskForm taskForm = new TaskForm(selectedTask))
                {
                    if (taskForm.ShowDialog() == DialogResult.OK)
                    {
                        // Знаходимо оригінальне завдання в основному списку і оновлюємо його
                        int originalIndex = tasks.FindIndex(t => t == selectedTask);
                        if (originalIndex >= 0)
                        {
                            tasks[originalIndex] = taskForm.Task;
                            FilterTasks();
                        }
                    }
                }
            }
        }

        // Події меню
        private void усіЗавданняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cmbStatus.SelectedIndex = 0;
            lblTitle.Text = "Усі завдання";
        }

        private void сьогодніToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Фільтр завдань на сьогодні
            DateTime today = DateTime.Today;
            filteredTasks = tasks.Where(t => t.DueDate.Date == today).ToList();
            UpdateDataGrid();
            lblTitle.Text = "Сьогодні";
        }

        private void завершеноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filteredTasks = tasks.Where(t => t.Status == "Зроблено").ToList();
            UpdateDataGrid();
            lblTitle.Text = "Завершено";
        }

        private void тегиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Функція управління тегами буде реалізована пізніше",
                "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void налаштуванняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Функція налаштувань буде реалізована пізніше",
                "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Форма завантажена
        }
    }
}