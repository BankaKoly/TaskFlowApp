using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TaskFlowApp
{
    public partial class MainForm : Form
    {
        private List<TaskFlowApp.Task> tasks;
        private List<TaskFlowApp.Task> filteredTasks;
        private bool isDataModified = false;

        public MainForm()
        {
            InitializeComponent();
            tasks = new List<TaskFlowApp.Task>();
            filteredTasks = new List<TaskFlowApp.Task>();

            // Налаштовуємо початкові значення
            cmbStatus.SelectedIndex = 0; // "Всі"
            cmbSort.SelectedIndex = 0;   // "Сортувати за датою"

            // Автоматично завантажуємо дані
            LoadTasksFromXml();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            RefreshTaskList();
        }

        private void SetupDataGridView()
        {
            dataGridTasks.AutoGenerateColumns = false;
            dataGridTasks.Columns.Clear();

            // Додаємо колонки
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
                Width = 120
            });

            dataGridTasks.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Tag",
                HeaderText = "Тег",
                DataPropertyName = "Tag",
                Width = 150
            });

            // Налаштовуємо форматування дати
            dataGridTasks.CellFormatting += DataGridTasks_CellFormatting;

            // Додаємо обробник подвійного кліку для редагування
            dataGridTasks.CellDoubleClick += DataGridTasks_CellDoubleClick;
        }

        private void DataGridTasks_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridTasks.Columns[e.ColumnIndex].Name == "DueDate" && e.Value != null)
            {
                if (e.Value is DateTime dateTime)
                {
                    e.Value = dateTime.ToString("dd.MM.yyyy");
                    e.FormattingApplied = true;
                }
            }
        }

        private void DataGridTasks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < filteredTasks.Count)
            {
                EditTask(filteredTasks[e.RowIndex]);
            }
        }

        private void btnNewTask_Click(object sender, EventArgs e)
        {
            using (TaskForm taskForm = new TaskForm())
            {
                if (taskForm.ShowDialog() == DialogResult.OK)
                {
                    tasks.Add(taskForm.Task);
                    isDataModified = true;
                    RefreshTaskList();
                }
            }
        }

        private void EditTask(TaskFlowApp.Task taskToEdit)
        {
            using (TaskForm taskForm = new TaskForm(taskToEdit))
            {
                if (taskForm.ShowDialog() == DialogResult.OK)
                {
                    // Знаходимо індекс оригінальної задачі
                    int index = tasks.IndexOf(taskToEdit);
                    if (index >= 0)
                    {
                        tasks[index] = taskForm.Task;
                        isDataModified = true;
                        RefreshTaskList();
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveTasksToXml();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Змінено на "Вивантаження" згідно з вашим запитом
            LoadTasksFromXml();
        }

        private void SaveTasksToXml()
        {
            try
            {
                XmlManager.SaveTasks(tasks);
                isDataModified = false;
                MessageBox.Show("Дані успішно збережено!", "Збереження",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка збереження: {ex.Message}", "Помилка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTasksFromXml()
        {
            try
            {
                tasks = XmlManager.LoadTasks();
                isDataModified = false;
                RefreshTaskList();
                MessageBox.Show("Дані успішно завантажено!", "Завантаження",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження: {ex.Message}", "Помилка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                tasks = new List<TaskFlowApp.Task>();
                RefreshTaskList();
            }
        }

        private void RefreshTaskList()
        {
            ApplyFilters();
            ApplySorting();
            dataGridTasks.DataSource = null;
            dataGridTasks.DataSource = filteredTasks;
        }

        private void ApplyFilters()
        {
            filteredTasks = new List<TaskFlowApp.Task>(tasks);

            // Фільтр за статусом
            string selectedStatus = cmbStatus.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedStatus) && selectedStatus != "Всі")
            {
                filteredTasks = filteredTasks.Where(t => t.Status == selectedStatus).ToList();
            }

            // Фільтр пошуку
            string searchText = txtSearch.Text;
            if (!string.IsNullOrEmpty(searchText) && searchText != "Пошук")
            {
                filteredTasks = filteredTasks.Where(t =>
                    t.Name.ToLower().Contains(searchText.ToLower()) ||
                    t.Description.ToLower().Contains(searchText.ToLower()) ||
                    t.Tag.ToLower().Contains(searchText.ToLower())
                ).ToList();
            }
        }

        private void ApplySorting()
        {
            string selectedSort = cmbSort.SelectedItem?.ToString();

            switch (selectedSort)
            {
                case "Сортувати за датою":
                    filteredTasks = filteredTasks.OrderBy(t => t.DueDate).ToList();
                    break;
                case "Сортувати за назвою":
                    filteredTasks = filteredTasks.OrderBy(t => t.Name).ToList();
                    break;
                case "Сортувати за статусом":
                    filteredTasks = filteredTasks.OrderBy(t => t.Status).ToList();
                    break;
            }
        }

        // Обробники меню
        private void усіЗавданняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "Усі завдання";
            cmbStatus.SelectedIndex = 0; // "Всі"
            RefreshTaskList();
        }

        private void сьогодніToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "Сьогодні";
            // Фільтруємо задачі на сьогодні
            DateTime today = DateTime.Today;
            filteredTasks = tasks.Where(t => t.DueDate.Date == today).ToList();
            ApplySorting();
            dataGridTasks.DataSource = null;
            dataGridTasks.DataSource = filteredTasks;
        }

        private void завершеноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "Завершено";
            cmbStatus.SelectedIndex = 3; // "Зроблено"
            RefreshTaskList();
        }

        private void тегиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Показуємо унікальні теги
            var uniqueTags = tasks.Where(t => !string.IsNullOrEmpty(t.Tag))
                                 .Select(t => t.Tag)
                                 .Distinct()
                                 .ToList();

            string message = uniqueTags.Count > 0
                ? "Доступні теги:\n" + string.Join("\n", uniqueTags)
                : "Немає доступних тегів";

            MessageBox.Show(message, "Теги", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void налаштуванняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Налаштування поки не реалізовані", "Налаштування",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Обробники фільтрів
        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshTaskList();
        }

        private void cmbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshTaskList();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            RefreshTaskList();
        }

        // Обробники для placeholder тексту в пошуку
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

        // Перевірка перед закриттям форми
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (isDataModified)
            {
                DialogResult result = MessageBox.Show(
                    "У вас є незбережені зміни. Бажаєте зберегти їх перед закриттям?",
                    "Незбережені зміни",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SaveTasksToXml();
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }

            base.OnFormClosing(e);
        }
    }
}