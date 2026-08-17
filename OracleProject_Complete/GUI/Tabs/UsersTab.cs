using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace OracleProject
{
    /// <summary>
    /// Users tab: user management with inline editor.
    /// </summary>
    internal sealed class UsersTab : DashboardTabBase
    {
        private Panel toolbar;
        private Button btnAddUser, btnEditUser, btnDeactivateUser, btnRefreshUsers;
        private DataGridView dgvUsers;
        
        // Inline editor
        private Panel editorPanel;
        private ComboBox cboUserEmployee, cboUserRole;
        private TextBox txtUserUsername, txtUserPassword;
        private CheckBox chkUserActive;
        private Button btnSaveUser, btnCancelUser;
        private int? editingUserId;

        public UsersTab()
        {
            TabTitle = "Users";
            RequiredPermission = "USERS_VIEW";
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            BackColor = Color.FromArgb(247, 250, 252);
            Dock = DockStyle.Fill;

            // Toolbar
            toolbar = new Panel
            {
                BackColor = Color.White,
                Dock = DockStyle.Top,
                Height = 90
            };

            // Title
            var title = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                Location = new Point(20, 16),
                Size = new Size(300, 34),
                Text = "Application Users"
            };

            var subtitle = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(113, 128, 150),
                Location = new Point(22, 52),
                Size = new Size(600, 24),
                Text = "Create users, assign employee and choose role."
            };

            // Buttons
            btnAddUser = CreatePageButton("Add User", Color.FromArgb(49, 130, 206), 0, 20);
            btnAddUser.Size = new Size(115, 48);
            btnAddUser.Click += BtnAddUser_Click;

            btnEditUser = CreatePageButton("Edit", Color.FromArgb(72, 187, 120), 0, 20);
            btnEditUser.Size = new Size(90, 48);
            btnEditUser.Click += BtnEditUser_Click;

            btnDeactivateUser = CreatePageButton("Deactivate", Color.FromArgb(245, 101, 101), 0, 20);
            btnDeactivateUser.Size = new Size(125, 48);
            btnDeactivateUser.Click += BtnDeactivateUser_Click;

            btnRefreshUsers = CreatePageButton("Refresh", Color.FromArgb(107, 114, 128), 0, 20);
            btnRefreshUsers.Size = new Size(115, 48);
            btnRefreshUsers.Click += (s, e) => LoadUsers();

            toolbar.Controls.Add(title);
            toolbar.Controls.Add(subtitle);
            toolbar.Controls.Add(btnAddUser);
            toolbar.Controls.Add(btnEditUser);
            toolbar.Controls.Add(btnDeactivateUser);
            toolbar.Controls.Add(btnRefreshUsers);

            // Inline Editor Panel
            editorPanel = new Panel
            {
                BackColor = Color.White,
                Dock = DockStyle.Top,
                Height = 180,
                Visible = false
            };

            cboUserEmployee = CreateEditorCombo(20, 40, 300);
            txtUserUsername = CreateEditorTextBox(340, 40, 160);
            txtUserPassword = CreateEditorTextBox(520, 40, 160);
            txtUserPassword.UseSystemPasswordChar = true;
            cboUserRole = CreateEditorCombo(20, 110, 300);
            chkUserActive = new CheckBox
            {
                Location = new Point(340, 116),
                Size = new Size(120, 28),
                Text = "Active",
                Checked = true
            };
            btnSaveUser = CreatePageButton("Save", Color.FromArgb(45, 101, 181), 520, 108);
            btnSaveUser.Size = new Size(90, 48);
            btnSaveUser.Click += BtnSaveUser_Click;
            btnCancelUser = CreatePageButton("Cancel", Color.FromArgb(107, 114, 128), 620, 108);
            btnCancelUser.Size = new Size(90, 48);
            btnCancelUser.Click += (s, e) => HideEditor();

            editorPanel.Controls.Add(CreateEditorLabel("Employee", 20, 16));
            editorPanel.Controls.Add(cboUserEmployee);
            editorPanel.Controls.Add(CreateEditorLabel("Username", 340, 16));
            editorPanel.Controls.Add(txtUserUsername);
            editorPanel.Controls.Add(CreateEditorLabel("Password", 520, 16));
            editorPanel.Controls.Add(txtUserPassword);
            editorPanel.Controls.Add(CreateEditorLabel("Role", 20, 86));
            editorPanel.Controls.Add(cboUserRole);
            editorPanel.Controls.Add(chkUserActive);
            editorPanel.Controls.Add(btnSaveUser);
            editorPanel.Controls.Add(btnCancelUser);

            // Grid
            dgvUsers = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                Location = new Point(30, 145),
                ReadOnly = true,
                RowHeadersVisible = false,
                RowTemplate = { Height = 36 },
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            var headerStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = Color.FromArgb(247, 250, 252),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(113, 128, 150)
            };
            dgvUsers.ColumnHeadersDefaultCellStyle = headerStyle;
            dgvUsers.ColumnHeadersHeight = 46;

            var cellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.Black
            };
            dgvUsers.DefaultCellStyle = cellStyle;

            Controls.Add(dgvUsers);
            Controls.Add(editorPanel);
            Controls.Add(toolbar);

            // Layout
            toolbar.Resize += Toolbar_Resize;
            Resize += UsersTab_Resize;
        }

        private void Toolbar_Resize(object sender, EventArgs e)
        {
            // Position buttons on the right
            int right = toolbar.Width - 20;
            btnRefreshUsers.Location = new Point(right - btnRefreshUsers.Width, 20);
            right -= btnRefreshUsers.Width + 15;
            btnDeactivateUser.Location = new Point(right - btnDeactivateUser.Width, 20);
            right -= btnDeactivateUser.Width + 15;
            btnEditUser.Location = new Point(right - btnEditUser.Width, 20);
            right -= btnEditUser.Width + 15;
            btnAddUser.Location = new Point(right - btnAddUser.Width, 20);
        }

        private void UsersTab_Resize(object sender, EventArgs e)
        {
            LayoutEditor();
        }

        private void LayoutEditor()
        {
            dgvUsers.Width = ClientSize.Width - 60;
            dgvUsers.Top = editorPanel.Visible ? 330 : 145;
            dgvUsers.Height = Math.Max(200, ClientSize.Height - dgvUsers.Top - 30);
        }

        public override void ApplyPermissions()
        {
            btnAddUser.Visible = HasPermission("USERS_ADD");
            btnEditUser.Visible = HasPermission("USERS_EDIT");
            btnDeactivateUser.Visible = HasPermission("USERS_DELETE");
            btnRefreshUsers.Visible = HasPermission("USERS_VIEW");
        }

        public override void OnActivated()
        {
            LoadUsers();
        }

        public override void RefreshData()
        {
            LoadUsers();
        }

        private void LoadUsers()
        {
            const string sql = @"
                SELECT u.user_id AS ""User ID"",
                       u.username AS ""Username"",
                       e.employee_code || ' - ' || e.full_name AS ""Employee"",
                       r.role_name AS ""Role"",
                       r.role_code AS ""Role Code"",
                       CASE u.status WHEN 1 THEN 'ACTIVE' ELSE 'INACTIVE' END AS ""Status"",
                       u.created_at AS ""Created At"",
                       u.employee_id AS ""Employee ID"",
                       u.role_id AS ""Role ID""
                  FROM app_users u
                  JOIN employees e ON e.employee_id = u.employee_id
                  JOIN app_roles r ON r.role_id = u.role_id
                 ORDER BY u.user_id";

            dgvUsers.DataSource = OracleDb.Query(sql);
            HideGridColumn(dgvUsers, "User ID");
            HideGridColumn(dgvUsers, "Employee ID");
            HideGridColumn(dgvUsers, "Role ID");
        }

        private DataTable LoadEmployeeChoices(int? editingUserId)
        {
            return OracleDb.Query(@"
                SELECT e.employee_id,
                       e.employee_code || ' - ' || e.full_name AS display_name
                  FROM employees e
                 WHERE e.status = 1
                   AND NOT EXISTS (
                       SELECT 1
                         FROM app_users u
                        WHERE u.employee_id = e.employee_id
                          AND u.user_id <> NVL(:editingUserId, -1)
                   )
                 ORDER BY e.full_name",
                OracleDb.Parameter("editingUserId", editingUserId.HasValue ? (object)editingUserId.Value : DBNull.Value));
        }

        private DataTable LoadRoleChoices()
        {
            return OracleDb.Query(@"
                SELECT role_id,
                       role_name || ' (' || role_code || ')' AS display_name
                  FROM app_roles
                 WHERE status = 1
                 ORDER BY role_id");
        }

        private void BtnAddUser_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("USERS_ADD"))
                return;

            ShowEditor(null);
        }

        private void BtnEditUser_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("USERS_EDIT"))
                return;

            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show(this, "Please select a user.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var view = dgvUsers.SelectedRows[0].DataBoundItem as DataRowView;
            if (view != null)
                ShowEditor(view.Row);
        }

        private void ShowEditor(DataRow row)
        {
            bool isEdit = row != null;
            editingUserId = isEdit ? (int?)Convert.ToInt32(row["User ID"]) : null;
            FillCombo(cboUserEmployee, LoadEmployeeChoices(editingUserId), "EMPLOYEE_ID", "DISPLAY_NAME",
                isEdit ? (int?)Convert.ToInt32(row["Employee ID"]) : null);
            FillCombo(cboUserRole, LoadRoleChoices(), "ROLE_ID", "DISPLAY_NAME",
                isEdit ? (int?)Convert.ToInt32(row["Role ID"]) : null);

            txtUserUsername.Text = isEdit ? Convert.ToString(row["Username"]) : string.Empty;
            txtUserPassword.Text = string.Empty;
            chkUserActive.Checked = !isEdit ||
                string.Equals(Convert.ToString(row["Status"]), "ACTIVE", StringComparison.OrdinalIgnoreCase);

            editorPanel.Visible = true;
            LayoutEditor();
            txtUserUsername.Focus();
        }

        private void HideEditor()
        {
            editingUserId = null;
            txtUserUsername.Clear();
            txtUserPassword.Clear();
            editorPanel.Visible = false;
            LayoutEditor();
        }

        private void BtnSaveUser_Click(object sender, EventArgs e)
        {
            if (editingUserId.HasValue && !RequirePermission("USERS_EDIT"))
                return;
            if (!editingUserId.HasValue && !RequirePermission("USERS_ADD"))
                return;

            var employee = cboUserEmployee.SelectedItem as ComboItem;
            var role = cboUserRole.SelectedItem as ComboItem;
            string username = txtUserUsername.Text.Trim();
            string password = txtUserPassword.Text;

            if (employee == null)
            {
                MessageBox.Show(this, "Please choose an employee.", "Users",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show(this, "Please enter username.", "Users",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!editingUserId.HasValue && string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(this, "Please enter password.", "Users",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (role == null)
            {
                MessageBox.Show(this, "Please choose a role.", "Users",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (!editingUserId.HasValue)
                {
                    OracleDb.Execute(@"
                        INSERT INTO app_users (
                            user_id, employee_id, username,
                            password_hash, role_id, status
                        ) VALUES (
                            app_user_seq.NEXTVAL, :employeeId, :username,
                            RAWTOHEX(STANDARD_HASH(:password, 'SHA256')),
                            :roleId, :status
                        )",
                        OracleDb.Parameter("employeeId", employee.Id),
                        OracleDb.Parameter("username", username),
                        OracleDb.Parameter("password", password),
                        OracleDb.Parameter("roleId", role.Id),
                        OracleDb.Parameter("status", chkUserActive.Checked ? 1 : 0));
                }
                else if (string.IsNullOrWhiteSpace(password))
                {
                    OracleDb.Execute(@"
                        UPDATE app_users
                           SET employee_id = :employeeId,
                               username = :username,
                               role_id = :roleId,
                               status = :status
                         WHERE user_id = :userId",
                        OracleDb.Parameter("employeeId", employee.Id),
                        OracleDb.Parameter("username", username),
                        OracleDb.Parameter("roleId", role.Id),
                        OracleDb.Parameter("status", chkUserActive.Checked ? 1 : 0),
                        OracleDb.Parameter("userId", editingUserId.Value));
                }
                else
                {
                    OracleDb.Execute(@"
                        UPDATE app_users
                           SET employee_id = :employeeId,
                               username = :username,
                               password_hash = RAWTOHEX(STANDARD_HASH(:password, 'SHA256')),
                               role_id = :roleId,
                               status = :status
                         WHERE user_id = :userId",
                        OracleDb.Parameter("employeeId", employee.Id),
                        OracleDb.Parameter("username", username),
                        OracleDb.Parameter("password", password),
                        OracleDb.Parameter("roleId", role.Id),
                        OracleDb.Parameter("status", chkUserActive.Checked ? 1 : 0),
                        OracleDb.Parameter("userId", editingUserId.Value));
                }

                HideEditor();
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Save User Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDeactivateUser_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("USERS_DELETE"))
                return;

            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show(this, "Please select a user.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var view = dgvUsers.SelectedRows[0].DataBoundItem as DataRowView;
            if (view == null)
                return;

            var row = view.Row;
            string username = Convert.ToString(row["Username"]);
            if (string.Equals(username, Context?.CurrentUser, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(this, "You cannot deactivate your own login while you are using it.",
                    "Users", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show(this, "Deactivate user " + username + "? Data will be kept.",
                    "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                OracleDb.Execute(
                    "UPDATE app_users SET status = 0 WHERE user_id = :userId",
                    OracleDb.Parameter("userId", Convert.ToInt32(row["User ID"])));
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Deactivate User Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
