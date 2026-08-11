using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace OracleProject
{
    public class PermissionForm : Form
    {
        private readonly ComboBox _cboRoles;
        private readonly ListBox _lstAvailableUsers;
        private readonly ListBox _lstRoleUsers;
        private readonly Button _btnAddUser;
        private readonly Button _btnRemoveUser;
        private readonly TreeView _treePermissions;
        private readonly Button _btnSave;
        private readonly Button _btnRefresh;
        private readonly Button _btnClose;
        private readonly Label _lblStatus;
        private readonly bool _canManage;

        private readonly List<PermissionItem> _permissionItems = new List<PermissionItem>();
        private bool _loading;
        private bool _checkingTree;

        public PermissionForm(bool canManage = true)
        {
            _canManage = canManage;

            Text = "User Roles and Permissions";
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(760, 560);
            Size = new Size(980, 680);
            AutoScroll = true;
            BackColor = Color.FromArgb(247, 250, 252);
            Font = new Font("Segoe UI", 9F);

            _btnRefresh = CreateActionButton("Refresh", Color.FromArgb(107, 114, 128), Color.White);
            _btnSave = CreateActionButton("Save Rights", Color.FromArgb(45, 101, 181), Color.White);
            _btnClose = CreateActionButton("Close", Color.White, Color.FromArgb(55, 65, 81));
            _btnClose.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            _btnClose.FlatAppearance.BorderSize = 1;

            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 54,
                BackColor = Color.FromArgb(247, 250, 252)
            };

            var title = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Left,
                Width = 360,
                Padding = new Padding(18, 12, 0, 0),
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                Text = "User Roles and Permissions"
            };

            var headerActions = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                FlowDirection = FlowDirection.LeftToRight,
                Width = 360,
                Padding = new Padding(0, 12, 18, 0)
            };
            headerActions.Controls.Add(_btnRefresh);
            headerActions.Controls.Add(_btnSave);
            headerActions.Controls.Add(_btnClose);
            header.Controls.Add(headerActions);
            header.Controls.Add(title);
            Controls.Add(header);

            var main = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(18, 8, 18, 12),
                ColumnCount = 1,
                RowCount = 3
            };
            main.RowStyles.Add(new RowStyle(SizeType.Absolute, 210F));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            main.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            Controls.Add(main);

            var usersBox = CreateSection("Members");
            main.Controls.Add(usersBox, 0, 0);

            var usersLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 2,
                Padding = new Padding(12)
            };
            usersLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            usersLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 54F));
            usersLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            usersLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 185F));
            usersLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            usersLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            usersBox.Controls.Add(usersLayout);

            usersLayout.Controls.Add(CreateSmallHeader("Available Users:"), 0, 0);
            usersLayout.Controls.Add(CreateSmallHeader("Member of Role:"), 2, 0);
            usersLayout.Controls.Add(CreateSmallHeader("Selected Role:"), 3, 0);

            _lstAvailableUsers = new ListBox { Dock = DockStyle.Fill, SelectionMode = SelectionMode.MultiExtended };
            _lstRoleUsers = new ListBox { Dock = DockStyle.Fill, SelectionMode = SelectionMode.MultiExtended };
            usersLayout.Controls.Add(_lstAvailableUsers, 0, 1);
            usersLayout.Controls.Add(_lstRoleUsers, 2, 1);

            var movePanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(8, 54, 8, 0)
            };
            _btnAddUser = CreateMoveButton(">");
            _btnRemoveUser = CreateMoveButton("<");
            movePanel.Controls.Add(_btnAddUser);
            movePanel.Controls.Add(_btnRemoveUser);
            usersLayout.Controls.Add(movePanel, 1, 1);

            _cboRoles = new ComboBox
            {
                Dock = DockStyle.Top,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            usersLayout.Controls.Add(_cboRoles, 3, 1);

            var rightsBox = CreateSection("Rights");
            main.Controls.Add(rightsBox, 0, 1);

            _treePermissions = new TreeView
            {
                CheckBoxes = true,
                Dock = DockStyle.Fill,
                HideSelection = false,
                ShowLines = true
            };
            rightsBox.Controls.Add(_treePermissions);

            var footer = new Panel { Dock = DockStyle.Fill };
            main.Controls.Add(footer, 0, 2);

            _lblStatus = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Left,
                Width = 520,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.FromArgb(75, 85, 99)
            };
            footer.Controls.Add(_lblStatus);

            _cboRoles.SelectedIndexChanged += cboRoles_SelectedIndexChanged;
            _btnAddUser.Click += btnAddUser_Click;
            _btnRemoveUser.Click += btnRemoveUser_Click;
            _btnRefresh.Click += delegate { LoadAllData(); };
            _btnSave.Click += btnSave_Click;
            _btnClose.Click += delegate { Close(); };
            _treePermissions.BeforeCheck += treePermissions_BeforeCheck;
            _treePermissions.AfterCheck += treePermissions_AfterCheck;
            Load += delegate { LoadAllData(); };

            ApplyManagePermission();
        }

        public void ConfigureForEmbedded()
        {
            TopLevel = false;
            FormBorderStyle = FormBorderStyle.None;
            Dock = DockStyle.Fill;
            StartPosition = FormStartPosition.Manual;
            _btnClose.Visible = false;
            Padding = new Padding(0);
            MinimumSize = Size.Empty;
            AutoScroll = true;
        }

        public void RefreshPermissionData()
        {
            LoadAllData();
        }

        private void ApplyManagePermission()
        {
            _btnAddUser.Visible = _canManage;
            _btnRemoveUser.Visible = _canManage;
            _btnSave.Visible = _canManage;

            if (!_canManage)
                _lblStatus.Text = "View only. SETTINGS_MANAGE is required to change users or rights.";
        }

        private void SetReadyStatus()
        {
            _lblStatus.Text = _canManage
                ? "Ready."
                : "View only. SETTINGS_MANAGE is required to change users or rights.";
        }

        private GroupBox CreateSection(string title)
        {
            return new GroupBox
            {
                Dock = DockStyle.Fill,
                Text = title,
                Padding = new Padding(12),
                ForeColor = Color.FromArgb(55, 65, 81),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
        }

        private static Label CreateSmallHeader(string text)
        {
            return new Label
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                Text = text,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(55, 65, 81)
            };
        }

        private static Button CreateMoveButton(string text)
        {
            return new Button
            {
                Text = text,
                Width = 38,
                Height = 34,
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(220, 252, 231),
                ForeColor = Color.FromArgb(22, 101, 52),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold)
            };
        }

        private static Button CreateActionButton(string text, Color backColor, Color foreColor)
        {
            return new Button
            {
                Text = text,
                Width = 110,
                Height = 34,
                Margin = new Padding(6, 0, 0, 0),
                Cursor = Cursors.Hand,
                BackColor = backColor,
                ForeColor = foreColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                UseVisualStyleBackColor = false
            };
        }

        private void LoadAllData()
        {
            _loading = true;
            try
            {
                LoadRoles();
                LoadPermissions();

                if (_cboRoles.Items.Count > 0 && _cboRoles.SelectedIndex < 0)
                    _cboRoles.SelectedIndex = 0;

                LoadRoleDetails();
                SetReadyStatus();
            }
            catch (OracleException ex) when (ex.Number == 942)
            {
                MessageBox.Show(
                    "Permission tables were not found. Run ORACLE\\011_app_permissions.sql first.",
                    "Missing Permission Tables",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Permission Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _loading = false;
            }
        }

        private void LoadRoles()
        {
            RoleItem previous = _cboRoles.SelectedItem as RoleItem;
            _cboRoles.Items.Clear();

            DataTable roles = OracleDb.Query(@"
                SELECT role_code, role_name
                  FROM app_roles
                 WHERE status = 1
                 ORDER BY role_name");

            int selectedIndex = -1;
            for (int i = 0; i < roles.Rows.Count; i++)
            {
                var item = new RoleItem(
                    Convert.ToString(roles.Rows[i]["ROLE_CODE"]),
                    Convert.ToString(roles.Rows[i]["ROLE_NAME"]));
                _cboRoles.Items.Add(item);

                if (previous != null && item.RoleCode == previous.RoleCode)
                    selectedIndex = i;
            }

            if (selectedIndex >= 0)
                _cboRoles.SelectedIndex = selectedIndex;
        }

        private void LoadPermissions()
        {
            _permissionItems.Clear();

            DataTable permissions = OracleDb.Query(@"
                SELECT p.permission_code,
                       p.module_code,
                       m.module_name,
                       p.permission_name,
                       m.display_order AS module_order,
                       p.display_order AS permission_order
                  FROM app_permissions p
                  JOIN app_modules m ON m.module_code = p.module_code
                 WHERE p.status = 1
                   AND m.status = 1
                 ORDER BY m.display_order, p.display_order");

            foreach (DataRow row in permissions.Rows)
            {
                _permissionItems.Add(new PermissionItem(
                    Convert.ToString(row["PERMISSION_CODE"]),
                    Convert.ToString(row["MODULE_CODE"]),
                    Convert.ToString(row["MODULE_NAME"]),
                    Convert.ToString(row["PERMISSION_NAME"])));
            }
        }

        private void LoadRoleDetails()
        {
            RoleItem role = _cboRoles.SelectedItem as RoleItem;
            if (role == null)
                return;

            LoadUsersForRole(role.RoleCode);
            LoadPermissionTree(role.RoleCode);
        }

        private void LoadUsersForRole(string roleCode)
        {
            _lstAvailableUsers.Items.Clear();
            _lstRoleUsers.Items.Clear();

            DataTable users = OracleDb.Query(@"
                SELECT u.user_id,
                       u.username,
                       e.full_name,
                       r.role_code
                  FROM app_users u
                  JOIN employees e ON e.employee_id = u.employee_id
                  JOIN app_roles r ON r.role_id = u.role_id
                 WHERE u.status = 1
                   AND e.status = 1
                   AND r.status = 1
                 ORDER BY e.full_name");

            foreach (DataRow row in users.Rows)
            {
                var item = new UserItem(
                    Convert.ToInt32(row["USER_ID"]),
                    Convert.ToString(row["USERNAME"]),
                    Convert.ToString(row["FULL_NAME"]),
                    Convert.ToString(row["ROLE_CODE"]));

                if (item.RoleCode == roleCode)
                    _lstRoleUsers.Items.Add(item);
                else
                    _lstAvailableUsers.Items.Add(item);
            }
        }

        private void LoadPermissionTree(string roleCode)
        {
            _checkingTree = true;
            try
            {
                _treePermissions.Nodes.Clear();

                DataTable allowed = OracleDb.Query(@"
                    SELECT rp.permission_code
                      FROM app_role_permissions rp
                      JOIN app_roles r ON r.role_id = rp.role_id
                     WHERE UPPER(r.role_code) = UPPER(:roleCode)
                       AND is_allowed = 1",
                    OracleDb.Parameter("roleCode", roleCode));

                var allowedCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (DataRow row in allowed.Rows)
                    allowedCodes.Add(Convert.ToString(row["PERMISSION_CODE"]));

                var moduleNodes = new Dictionary<string, TreeNode>(StringComparer.OrdinalIgnoreCase);
                foreach (PermissionItem permission in _permissionItems)
                {
                    TreeNode moduleNode;
                    if (!moduleNodes.TryGetValue(permission.ModuleCode, out moduleNode))
                    {
                        moduleNode = new TreeNode(permission.ModuleName)
                        {
                            Tag = permission.ModuleCode
                        };
                        moduleNodes.Add(permission.ModuleCode, moduleNode);
                        _treePermissions.Nodes.Add(moduleNode);
                    }

                    var permissionNode = new TreeNode(permission.PermissionName)
                    {
                        Tag = permission.PermissionCode,
                        Checked = allowedCodes.Contains(permission.PermissionCode)
                    };
                    moduleNode.Nodes.Add(permissionNode);
                }

                foreach (TreeNode moduleNode in _treePermissions.Nodes)
                {
                    moduleNode.Checked = AreAllChildrenChecked(moduleNode);
                    moduleNode.Expand();
                }
            }
            finally
            {
                _checkingTree = false;
            }
        }

        private void cboRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loading)
                LoadRoleDetails();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (!_canManage)
                return;

            RoleItem role = _cboRoles.SelectedItem as RoleItem;
            if (role == null || _lstAvailableUsers.SelectedItems.Count == 0)
                return;

            AssignSelectedUsers(_lstAvailableUsers, role.RoleCode);
        }

        private void btnRemoveUser_Click(object sender, EventArgs e)
        {
            if (!_canManage)
                return;

            RoleItem role = _cboRoles.SelectedItem as RoleItem;
            if (role == null || _lstRoleUsers.SelectedItems.Count == 0)
                return;

            if (role.RoleCode == "REQUESTER")
            {
                MessageBox.Show(
                    "REQUESTER is the default role. Choose another role before removing a member.",
                    "Default Role",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            AssignSelectedUsers(_lstRoleUsers, "REQUESTER");
        }

        private void AssignSelectedUsers(ListBox list, string roleCode)
        {
            var selectedUsers = new List<UserItem>();
            foreach (object selected in list.SelectedItems)
                selectedUsers.Add((UserItem)selected);

            foreach (UserItem user in selectedUsers)
            {
                OracleDb.Execute(@"
                    UPDATE app_users
                       SET role_id = (
                           SELECT role_id
                             FROM app_roles
                            WHERE UPPER(role_code) = UPPER(:roleCode)
                       )
                     WHERE user_id = :userId",
                    OracleDb.Parameter("roleCode", roleCode),
                    OracleDb.Parameter("userId", user.UserId));
            }

            LoadRoleDetails();
            _lblStatus.Text = "User role updated.";
        }

        private void treePermissions_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (!_canManage && !_loading && !_checkingTree)
                e.Cancel = true;
        }

        private void treePermissions_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (_checkingTree)
                return;

            _checkingTree = true;
            try
            {
                foreach (TreeNode child in e.Node.Nodes)
                    child.Checked = e.Node.Checked;

                if (e.Node.Parent != null)
                    e.Node.Parent.Checked = AreAllChildrenChecked(e.Node.Parent);
            }
            finally
            {
                _checkingTree = false;
            }
        }

        private static bool AreAllChildrenChecked(TreeNode node)
        {
            if (node.Nodes.Count == 0)
                return node.Checked;

            foreach (TreeNode child in node.Nodes)
            {
                if (!child.Checked)
                    return false;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_canManage)
                return;

            RoleItem role = _cboRoles.SelectedItem as RoleItem;
            if (role == null)
                return;

            try
            {
                using (OracleConnection connection = OracleDb.OpenConnection())
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    foreach (TreeNode moduleNode in _treePermissions.Nodes)
                    {
                        foreach (TreeNode permissionNode in moduleNode.Nodes)
                        {
                            using (OracleCommand command = OracleDb.CreateCommand(connection, @"
                                MERGE INTO app_role_permissions target
                                USING (
                                    SELECT (
                                               SELECT role_id
                                                 FROM app_roles
                                                WHERE UPPER(role_code) = UPPER(:roleCode)
                                           ) AS role_id,
                                           :permissionCode AS permission_code,
                                           :isAllowed AS is_allowed
                                      FROM dual
                                ) source
                                ON (
                                    target.role_id = source.role_id
                                    AND target.permission_code = source.permission_code
                                )
                                WHEN MATCHED THEN UPDATE SET
                                    target.is_allowed = source.is_allowed,
                                    target.updated_at = SYSTIMESTAMP
                                WHEN NOT MATCHED THEN INSERT
                                    (role_id, permission_code, is_allowed, created_at)
                                VALUES
                                    (source.role_id, source.permission_code, source.is_allowed, SYSTIMESTAMP)",
                                OracleDb.Parameter("roleCode", role.RoleCode),
                                OracleDb.Parameter("permissionCode", Convert.ToString(permissionNode.Tag)),
                                OracleDb.Parameter("isAllowed", permissionNode.Checked ? 1 : 0)))
                            {
                                command.Transaction = transaction;
                                command.ExecuteNonQuery();
                            }
                        }
                    }

                    transaction.Commit();
                }

                _lblStatus.Text = "Rights saved for " + role.RoleName + ".";
                MessageBox.Show("Rights saved successfully.", "Permissions", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Save Permissions Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private sealed class RoleItem
        {
            public RoleItem(string roleCode, string roleName)
            {
                RoleCode = roleCode;
                RoleName = roleName;
            }

            public string RoleCode { get; private set; }
            public string RoleName { get; private set; }

            public override string ToString()
            {
                return RoleName + " (" + RoleCode + ")";
            }
        }

        private sealed class UserItem
        {
            public UserItem(int userId, string username, string fullName, string roleCode)
            {
                UserId = userId;
                Username = username;
                FullName = fullName;
                RoleCode = roleCode;
            }

            public int UserId { get; private set; }
            public string Username { get; private set; }
            public string FullName { get; private set; }
            public string RoleCode { get; private set; }

            public override string ToString()
            {
                return FullName + " (" + Username + ")";
            }
        }

        private sealed class PermissionItem
        {
            public PermissionItem(string permissionCode, string moduleCode, string moduleName, string permissionName)
            {
                PermissionCode = permissionCode;
                ModuleCode = moduleCode;
                ModuleName = moduleName;
                PermissionName = permissionName;
            }

            public string PermissionCode { get; private set; }
            public string ModuleCode { get; private set; }
            public string ModuleName { get; private set; }
            public string PermissionName { get; private set; }
        }
    }
}
