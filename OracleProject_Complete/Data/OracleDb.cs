using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace OracleProject
{
    /// <summary>
    /// Small Oracle data-access layer shared by the WinForms screens.
    /// SQL stays parameterized here so user input is never concatenated into SQL.
    /// </summary>
    internal static class OracleDb
    {
        private static string _activeConnectionString;

        private static string ConnectionString
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_activeConnectionString))
                    return _activeConnectionString;

                var setting = ConfigurationManager.ConnectionStrings["OracleDb"];
                if (setting == null || string.IsNullOrWhiteSpace(setting.ConnectionString))
                    throw new ConfigurationErrorsException("Connection string 'OracleDb' is missing from App.config.");
                return setting.ConnectionString;
            }
        }

        internal static OracleConnection OpenConnection()
        {
            var connection = new OracleConnection(ConnectionString);
            connection.Open();
            return connection;
        }

        internal static bool AuthenticateAppUser(
            string username,
            string password,
            out string displayName,
            out string roleCode,
            out string error)
        {
            displayName = null;
            roleCode = null;

            try
            {
                DataTable app_users = Query(@"
                    SELECT e.full_name, r.role_code
                    FROM app_users u
                    JOIN employees e ON e.employee_id = u.employee_id
                    JOIN app_roles r ON r.role_id = u.role_id
                    WHERE UPPER(u.username) = UPPER(:username)
                      AND u.password_hash = RAWTOHEX(STANDARD_HASH(:password, 'SHA256'))
                      AND u.status = 1
                      AND e.status = 1
                      AND r.status = 1",
                    VarcharParameter("username", username),
                    VarcharParameter("password", password));

                if (app_users.Rows.Count == 0)
                {
                    error = "Invalid application username or password.";
                    return false;
                }

                displayName = Convert.ToString(app_users.Rows[0]["FULL_NAME"]);
                roleCode = Convert.ToString(app_users.Rows[0]["ROLE_CODE"]).ToUpperInvariant();
                error = null;
                return true;
            }
            catch (OracleException ex) when (ex.Number == 942)
            {
                error = "APP_USERS or EMPLOYEES table was not found.";
                return false;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        internal static HashSet<string> GetAllowedPermissions(string roleCode)
        {
            var permissions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            DataTable rows = Query(@"
                SELECT permission_code
                  FROM vw_app_role_permissions
                 WHERE UPPER(role_code) = UPPER(:roleCode)
                   AND is_allowed = 1",
                Parameter("roleCode", roleCode));

            foreach (DataRow row in rows.Rows)
                permissions.Add(Convert.ToString(row["PERMISSION_CODE"]));

            return permissions;
        }

        internal static bool TestConnection(out string error)
        {
            try
            {
                using (var connection = OpenConnection())
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT 1 FROM dual";
                    command.ExecuteScalar();
                }

                error = null;
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        internal static bool TestConnection(string username, string password, out string error)
        {
            try
            {
                var builder = new OracleConnectionStringBuilder(ConnectionString)
                {
                    UserID = username,
                    Password = password
                };

                using (var connection = new OracleConnection(builder.ConnectionString))
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = "SELECT 1 FROM dual";
                    command.ExecuteScalar();
                }

                _activeConnectionString = builder.ConnectionString;
                error = null;
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        internal static DataTable Query(string sql, params OracleParameter[] parameters)
        {
            using (var connection = OpenConnection())
            using (var command = CreateCommand(connection, sql, parameters))
            using (var adapter = new OracleDataAdapter(command))
            {
                var table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        internal static int Execute(string sql, params OracleParameter[] parameters)
        {
            using (var connection = OpenConnection())
            using (var transaction = connection.BeginTransaction())
            using (var command = CreateCommand(connection, sql, parameters))
            {
                command.Transaction = transaction;
                try
                {
                    int affected = command.ExecuteNonQuery();
                    transaction.Commit();
                    return affected;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        internal static object Scalar(string sql, params OracleParameter[] parameters)
        {
            using (var connection = OpenConnection())
            using (var command = CreateCommand(connection, sql, parameters))
            {
                return command.ExecuteScalar();
            }
        }

        internal static OracleCommand CreateCommand(
            OracleConnection connection,
            string sql,
            params OracleParameter[] parameters)
        {
            var command = connection.CreateCommand();
            command.BindByName = true;
            command.CommandText = sql;

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    if (parameter != null)
                        command.Parameters.Add(parameter);
                }
            }

            return command;
        }

        internal static OracleParameter Parameter(string name, object value)
        {
            return new OracleParameter(name, value ?? DBNull.Value);
        }

        private static OracleParameter VarcharParameter(string name, string value)
        {
            return new OracleParameter(name, OracleDbType.Varchar2, 255)
            {
                Value = value ?? string.Empty
            };
        }
    }
}
