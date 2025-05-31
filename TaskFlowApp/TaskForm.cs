using System;
using System.Windows.Forms;

namespace TaskFlowApp
{
    public partial class TaskForm : Form
    {
        public Task Task { get; private set; }

        public TaskForm()
        {
            InitializeComponent();
            dtpDueDate.Value = DateTime.Now.AddDays(1);
        }

        public TaskForm(Task existingTask) : this()
        {
            // Конструктор для редагування існуючої задачі
            if (existingTask != null)
            {
                txtName.Text = existingTask.Name;
                txtDescription.Text = existingTask.Description;
                cmbStatus.SelectedItem = existingTask.Status;
                dtpDueDate.Value = existingTask.DueDate;
                txtTag.Text = existingTask.Tag;

                this.Text = "Редагувати задачу";
                btnOK.Text = "Зберегти";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                Task = new Task(
                    txtName.Text.Trim(),
                    txtDescription.Text.Trim(),
                    cmbStatus.SelectedItem.ToString(),
                    dtpDueDate.Value,
                    txtTag.Text.Trim()
                );

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Будь ласка, введіть назву задачі.", "Помилка валідації",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Будь ласка, введіть опис задачі.", "Помилка валідації",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDescription.Focus();
                return false;
            }

            if (cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Будь ласка, оберіть статус задачі.", "Помилка валідації",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbStatus.Focus();
                return false;
            }

            return true;
        }
    }
}