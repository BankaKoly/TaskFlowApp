namespace TaskFlowApp
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem усіЗавданняToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сьогодніToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem завершеноToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem налаштуванняToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem тегиToolStripMenuItem;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button btnNewTask;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelFilter;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.ComboBox cmbSort;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDelete; // ДОДАНО КНОПКУ ВИДАЛИТИ
        private System.Windows.Forms.DataGridView dataGridTasks;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            усіЗавданняToolStripMenuItem = new ToolStripMenuItem();
            сьогодніToolStripMenuItem = new ToolStripMenuItem();
            тегиToolStripMenuItem = new ToolStripMenuItem();
            завершеноToolStripMenuItem = new ToolStripMenuItem();
            налаштуванняToolStripMenuItem = new ToolStripMenuItem();
            panelTop = new Panel();
            btnNewTask = new Button();
            txtSearch = new TextBox();
            lblTitle = new Label();
            panelFilter = new Panel();
            btnCancel = new Button();
            btnSave = new Button();
            btnDelete = new Button(); // ДОДАНО
            cmbSort = new ComboBox();
            cmbStatus = new ComboBox();
            dataGridTasks = new DataGridView();

            menuStrip1.SuspendLayout();
            panelTop.SuspendLayout();
            panelFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridTasks).BeginInit();
            SuspendLayout();

            // menuStrip1
            menuStrip1.BackColor = Color.FromArgb(240, 244, 248);
            menuStrip1.Items.AddRange(new ToolStripItem[] {
                усіЗавданняToolStripMenuItem,
                сьогодніToolStripMenuItem,
                тегиToolStripMenuItem,
                завершеноToolStripMenuItem,
                налаштуванняToolStripMenuItem
            });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(7, 2, 0, 2);
            menuStrip1.Size = new Size(1167, 27);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";

            // усіЗавданняToolStripMenuItem
            усіЗавданняToolStripMenuItem.Font = new Font("Segoe UI", 10F);
            усіЗавданняToolStripMenuItem.ForeColor = Color.FromArgb(51, 65, 85);
            усіЗавданняToolStripMenuItem.Name = "усіЗавданняToolStripMenuItem";
            усіЗавданняToolStripMenuItem.Size = new Size(100, 23);
            усіЗавданняToolStripMenuItem.Text = "Усі завдання";
            усіЗавданняToolStripMenuItem.Click += усіЗавданняToolStripMenuItem_Click;

            // сьогодніToolStripMenuItem
            сьогодніToolStripMenuItem.Font = new Font("Segoe UI", 10F);
            сьогодніToolStripMenuItem.ForeColor = Color.FromArgb(51, 65, 85);
            сьогодніToolStripMenuItem.Name = "сьогодніToolStripMenuItem";
            сьогодніToolStripMenuItem.Size = new Size(77, 23);
            сьогодніToolStripMenuItem.Text = "Сьогодні";
            сьогодніToolStripMenuItem.Click += сьогодніToolStripMenuItem_Click;

            // тегиToolStripMenuItem
            тегиToolStripMenuItem.Font = new Font("Segoe UI", 10F);
            тегиToolStripMenuItem.ForeColor = Color.FromArgb(51, 65, 85);
            тегиToolStripMenuItem.Name = "тегиToolStripMenuItem";
            тегиToolStripMenuItem.Size = new Size(48, 23);
            тегиToolStripMenuItem.Text = "Теги";
            тегиToolStripMenuItem.Click += тегиToolStripMenuItem_Click;

            // завершеноToolStripMenuItem
            завершеноToolStripMenuItem.Font = new Font("Segoe UI", 10F);
            завершеноToolStripMenuItem.ForeColor = Color.FromArgb(51, 65, 85);
            завершеноToolStripMenuItem.Name = "завершеноToolStripMenuItem";
            завершеноToolStripMenuItem.Size = new Size(92, 23);
            завершеноToolStripMenuItem.Text = "Завершено";
            завершеноToolStripMenuItem.Click += завершеноToolStripMenuItem_Click;

            // налаштуванняToolStripMenuItem
            налаштуванняToolStripMenuItem.Font = new Font("Segoe UI", 10F);
            налаштуванняToolStripMenuItem.ForeColor = Color.FromArgb(51, 65, 85);
            налаштуванняToolStripMenuItem.Name = "налаштуванняToolStripMenuItem";
            налаштуванняToolStripMenuItem.Size = new Size(113, 23);
            налаштуванняToolStripMenuItem.Text = "Налаштування";
            налаштуванняToolStripMenuItem.Click += налаштуванняToolStripMenuItem_Click;

            // panelTop
            panelTop.BackColor = Color.White;
            panelTop.Controls.Add(btnNewTask);
            panelTop.Controls.Add(txtSearch);
            panelTop.Controls.Add(lblTitle);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 27);
            panelTop.Margin = new Padding(4, 3, 4, 3);
            panelTop.Name = "panelTop";
            panelTop.Padding = new Padding(23, 23, 23, 23);
            panelTop.Size = new Size(1167, 92);
            panelTop.TabIndex = 1;

            // btnNewTask
            btnNewTask.BackColor = Color.FromArgb(59, 130, 246);
            btnNewTask.FlatAppearance.BorderSize = 0;
            btnNewTask.FlatStyle = FlatStyle.Flat;
            btnNewTask.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnNewTask.ForeColor = Color.White;
            btnNewTask.Location = new Point(875, 25);
            btnNewTask.Margin = new Padding(4, 3, 4, 3);
            btnNewTask.Name = "btnNewTask";
            btnNewTask.Size = new Size(140, 40);
            btnNewTask.TabIndex = 2;
            btnNewTask.Text = "Нова задача";
            btnNewTask.UseVisualStyleBackColor = false;
            btnNewTask.Click += btnNewTask_Click;

            // txtSearch
            txtSearch.Font = new Font("Segoe UI", 12F);
            txtSearch.ForeColor = Color.Gray;
            txtSearch.Location = new Point(467, 29);
            txtSearch.Margin = new Padding(4, 3, 4, 3);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(349, 29);
            txtSearch.TabIndex = 1;
            txtSearch.Text = "Пошук";
            txtSearch.TextChanged += txtSearch_TextChanged;
            txtSearch.Enter += txtSearch_Enter;
            txtSearch.Leave += txtSearch_Leave;

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitle.Location = new Point(23, 23);
            lblTitle.Margin = new Padding(4, 0, 4, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(186, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Усі завдання";

            // panelFilter
            panelFilter.BackColor = Color.White;
            panelFilter.Controls.Add(btnDelete); // ДОДАНО КНОПКУ ВИДАЛИТИ
            panelFilter.Controls.Add(btnCancel);
            panelFilter.Controls.Add(btnSave);
            panelFilter.Controls.Add(cmbSort);
            panelFilter.Controls.Add(cmbStatus);
            panelFilter.Dock = DockStyle.Top;
            panelFilter.Location = new Point(0, 119);
            panelFilter.Margin = new Padding(4, 3, 4, 3);
            panelFilter.Name = "panelFilter";
            panelFilter.Padding = new Padding(23, 23, 23, 23);
            panelFilter.Size = new Size(1167, 69);
            panelFilter.TabIndex = 2;

            // btnDelete - НОВА КНОПКА "ВИДАЛИТИ"
            btnDelete.BackColor = Color.FromArgb(239, 68, 68); // Червоний колір
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(677, 14);
            btnDelete.Margin = new Padding(4, 3, 4, 3);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(117, 35);
            btnDelete.TabIndex = 4;
            btnDelete.Text = "Видалити";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;

            // btnCancel - "Вивантаження"
            btnCancel.BackColor = Color.FromArgb(34, 197, 94);
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(957, 14);
            btnCancel.Margin = new Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(117, 35);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Вивантаження";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;

            // btnSave
            btnSave.BackColor = Color.FromArgb(59, 130, 246);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(817, 14);
            btnSave.Margin = new Padding(4, 3, 4, 3);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(117, 35);
            btnSave.TabIndex = 2;
            btnSave.Text = "Зберегти";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;

            // cmbSort
            cmbSort.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSort.Font = new Font("Segoe UI", 10F);
            cmbSort.FormattingEnabled = true;
            cmbSort.Items.AddRange(new object[] {
                "Сортувати за датою",
                "Сортувати за назвою",
                "Сортувати за статусом"
            });
            cmbSort.Location = new Point(233, 17);
            cmbSort.Margin = new Padding(4, 3, 4, 3);
            cmbSort.Name = "cmbSort";
            cmbSort.Size = new Size(209, 25);
            cmbSort.TabIndex = 1;
            cmbSort.SelectedIndexChanged += cmbSort_SelectedIndexChanged;

            // cmbStatus
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.Font = new Font("Segoe UI", 10F);
            cmbStatus.FormattingEnabled = true;
            cmbStatus.Items.AddRange(new object[] { "Всі", "До", "В Процесі", "Зроблено" });
            cmbStatus.Location = new Point(23, 17);
            cmbStatus.Margin = new Padding(4, 3, 4, 3);
            cmbStatus.Name = "cmbStatus";
            cmbStatus.Size = new Size(174, 25);
            cmbStatus.TabIndex = 0;
            cmbStatus.SelectedIndexChanged += cmbStatus_SelectedIndexChanged;

            // dataGridTasks
            dataGridTasks.AllowUserToAddRows = false;
            dataGridTasks.BackgroundColor = Color.White;
            dataGridTasks.BorderStyle = BorderStyle.None;
            dataGridTasks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridTasks.Dock = DockStyle.Fill;
            dataGridTasks.Location = new Point(0, 188);
            dataGridTasks.Margin = new Padding(4, 3, 4, 3);
            dataGridTasks.MultiSelect = false;
            dataGridTasks.Name = "dataGridTasks";
            dataGridTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridTasks.Size = new Size(1167, 562);
            dataGridTasks.TabIndex = 3;

            // MainForm
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(249, 250, 251);
            ClientSize = new Size(1167, 750);
            Controls.Add(dataGridTasks);
            Controls.Add(panelFilter);
            Controls.Add(panelTop);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(4, 3, 4, 3);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TaskFlow";
            Load += MainForm_Load;

            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelFilter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridTasks).EndInit();
            ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}