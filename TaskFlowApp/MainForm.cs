using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TaskFlowApp
{
    public partial class MainForm : Form
    {
        private List<Task> tasks;
        private List<Task> filteredTasks;
        private BindingSource bindingSource;

        public MainForm()
        {
            InitializeComponent();
            tasks = new List<Task>();
            filteredTasks = new List<Task>();
            bindingSource = new BindingSource();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitializeDataGrid();
            LoadTasks();
            ApplyFilters();
        }

        private void InitializeDataGrid()
        {
            dataGridTasks.AutoGenerateColumns = false;
            dataGridTasks.Columns.Clear();

            // Колонка назви
            DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.DataPropertyName = "Name";
            nameColumn.HeaderText = "Назва";
            nameColumn.Width = 250;
            dataGridTasks.Columns.Add(nameColumn);

            // Колонка опису
            DataGridViewTextBoxColumn descColumn = new DataGridViewTextBoxColumn();
            descColumn.DataPropertyName = "Description";
            descColumn.HeaderText = "Опис";
            descColumn.Width = 300;
            dataGridTasks.Columns.Add(descColumn);

            // Колонка статусу
            DataGridViewComboBoxColumn statusColumn = new DataGridViewComboBoxColumn();
            statusColumn.DataPropertyName = "Status";
            statusColumn.HeaderText = "Статус";
            statusColumn.Width = 120;
            statusColumn.Items.AddRange(new string[] { "До", "В Процесі", "Зроблено" });
            dataGridTasks.Columns.Add(statusColumn);

            // Колонка дати
            DataGridViewTextBoxColumn dateColumn = new DataGridViewTextBoxColumn();
            dateColumn.DataPropertyName = "DueDate";
            dateColumn.HeaderText = "Термін";
            dateColumn.Width = 120;
            dateColumn.DefaultCellStyle.Format = "dd.MM.yyyy";
            dataGridTasks.Columns.Add(dateColumn);

            // Колонка тегів
            DataGridViewTextBoxColumn tagColumn = new DataGridViewTextBoxColumn();
            tagColumn.DataPropertyName = "Tag";
            tagColumn.HeaderText = "Теги";
            tagColumn.Width = 150;
            dataGridTasks.Columns.Add(tagColumn);

            dataGridTasks.DataSource = bindingSource;
        }

        private void LoadTasks()
        {
            try
            {
                tasks = XmlManager.LoadTasks();
                if (tasks.Count == 0)
                {
                    // Додаємо приклади задач
                    tasks.Add(new Task("Розробити дизайн проекту", "Створити макет інтерфейсу", "Дизайн", DateTime.Now.AddDays(2), "Дизайн"));
                    tasks.Add(new Task("Написати документацію", "Підготувати технічну документацію", "Робота", DateTime.Now.AddDays(-1), "Написання"));
                    tasks.Add(new Task("Підготувати Презентацію", "Створити слайди для презентації", "Робота", DateTime.Now.AddDays(1), "Робота"));
                    SaveTasks();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tasks = new List<Task>();
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
                MessageBox.Show($"Помилка збереження: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilters()
        {
            filteredTasks = new List<Task>(tasks);

            // Фільтр за статусом
            if (cmbStatus.SelectedItem != null && cmbStatus.SelectedItem.ToString() != "Всі")
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
            if (cmbSort.SelectedItem != null)
            {
                string sortBy = cmbSort.SelectedItem.ToString();
                switch (sortBy)
                {
                    case "Сортувати за назвою":
                        filteredTasks = filteredTasks.OrderBy(t => t.Name).ToList();
                        break;
                    case "Сортувати за статусом":
                        filteredTasks = filteredTasks.OrderBy(t => t.Status).ToList();
                        break;
                    default: // За датою
                        filteredTasks = filteredTasks.OrderBy(t => t.DueDate).ToList();
                        break;
                }
            }

            bindingSource.DataSource = filteredTasks;
            bindingSource.ResetBindings(false);
        }

        // Обробники подій меню
        private void усіЗавданняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "Усі завдання";
            cmbStatus.SelectedIndex = 0;
            ApplyFilters();
        }

        private void сьогодніToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "Сьогодні";
            filteredTasks = tasks.Where(t => t.DueDate.Date == DateTime.Now.Date).ToList();
            bindingSource.DataSource = filteredTasks;
            bindingSource.ResetBindings(false);
        }

        private void тегиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "Теги";
            MessageBox.Show("Функція управління тегами буде додана в наступних версіях", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void завершеноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "Завершено";
            filteredTasks = tasks.Where(t => t.Status == "Зроблено").ToList();
            bindingSource.DataSource = filteredTasks;
            bindingSource.ResetBindings(false);
        }

        private void налаштуванняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Функція налаштувань буде додана в наступних версіях", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Обробники подій фільтрів
        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void cmbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        // Обробники подій пошуку
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        // Обробники подій кнопок
        private void btnNewTask_Click(object sender, EventArgs e)
        {
            TaskForm taskForm = new TaskForm();
            if (taskForm.ShowDialog() == DialogResult.OK)
            {
                tasks.Add(taskForm.Task);
                SaveTasks();
                ApplyFilters();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Зберігаємо зміни з DataGrid назад до списку задач
                bindingSource.EndEdit();

                // Оновлюємо оригінальний список задач
                foreach (Task filteredTask in filteredTasks)
                {
                    Task originalTask = tasks.FirstOrDefault(t =>
                        t.Name == filteredTask.Name &&
                        t.Description == filteredTask.Description);

                    if (originalTask != null)
                    {
                        originalTask.Status = filteredTask.Status;
                        originalTask.DueDate = filteredTask.DueDate;
                        originalTask.Tag = filteredTask.Tag;
                    }
                }

                SaveTasks();
                MessageBox.Show("Зміни збережено успішно!", "Збереження", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка збереження: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            LoadTasks();
            ApplyFilters();
            MessageBox.Show("Зміни скасовано", "Скасування", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}