using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OracleProject
{
    /// <summary>
    /// Holds the current user's context: identity, role, and permissions.
    /// Shared across all dashboard tabs.
    /// </summary>
    internal sealed class DashboardContext
    {
        public DashboardContext(string username, string roleCode)
        {
            CurrentUser = username ?? "Admin";
            CurrentRoleCode = string.IsNullOrWhiteSpace(roleCode) ? "REQUESTER" : roleCode;
            AllowedPermissions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        public string CurrentUser { get; }
        public string CurrentRoleCode { get; }
        public HashSet<string> AllowedPermissions { get; set; }

        public bool HasPermission(string permissionCode)
        {
            return AllowedPermissions.Contains(permissionCode);
        }

        public bool RequirePermission(string permissionCode, IWin32Window owner)
        {
            if (HasPermission(permissionCode))
                return true;

            MessageBox.Show(
                owner,
                "Your role does not have permission: " + permissionCode,
                "Permission Denied",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return false;
        }

        public void AllowAllKnownPermissions()
        {
            AllowedPermissions.Clear();
            var all = new[]
            {
                "OVERVIEW_VIEW",
                "ORDERS_VIEW", "ORDERS_ADD", "ORDERS_EDIT", "ORDERS_DELETE", "ORDERS_PRINT",
                "SUPPLIERS_VIEW", "SUPPLIERS_ADD", "SUPPLIERS_EDIT", "SUPPLIERS_DELETE",
                "PRODUCTS_VIEW", "PRODUCTS_ADD", "PRODUCTS_EDIT", "PRODUCTS_DELETE",
                "REPORTS_VIEW", "REPORTS_GENERATE", "REPORTS_EXPORT",
                "USERS_VIEW", "USERS_ADD", "USERS_EDIT", "USERS_DELETE",
                "SETTINGS_VIEW", "SETTINGS_MANAGE",
                "FORM001_VIEW", "FORM001_ADD", "FORM001_EDIT", "FORM001_DELETE"
            };
            foreach (var p in all)
                AllowedPermissions.Add(p);
        }
    }
}
