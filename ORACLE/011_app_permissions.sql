-- Role and permission setup for the Oracle Purchasing Management System.
-- Run this script as PURCHASING_USER after 009_app_users.sql.
--
-- APP_ROLES and APP_USERS are created in 009_app_users.sql.
-- This script adds modules, permissions, role-permission mappings and views
-- so the C# Dashboard can hide/show menus and buttons by role.

SET DEFINE OFF;
SET SERVEROUTPUT ON;
WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK;

PROMPT ===== Creating application roles and permissions =====

CREATE TABLE app_modules (
    module_code   VARCHAR2(40) PRIMARY KEY,
    module_name   VARCHAR2(120) NOT NULL,
    display_order NUMBER(4) DEFAULT 0 NOT NULL,
    status        NUMBER(1) DEFAULT 1 NOT NULL,
    created_at    TIMESTAMP DEFAULT SYSTIMESTAMP NOT NULL,

    CONSTRAINT ck_app_modules_status
        CHECK (status IN (0, 1))
);

CREATE TABLE app_permissions (
    permission_code VARCHAR2(80) PRIMARY KEY,
    module_code     VARCHAR2(40) NOT NULL,
    action_code     VARCHAR2(30) NOT NULL,
    permission_name VARCHAR2(150) NOT NULL,
    display_order   NUMBER(4) DEFAULT 0 NOT NULL,
    status          NUMBER(1) DEFAULT 1 NOT NULL,
    created_at      TIMESTAMP DEFAULT SYSTIMESTAMP NOT NULL,

    CONSTRAINT fk_app_perm_module
        FOREIGN KEY (module_code) REFERENCES app_modules(module_code),

    CONSTRAINT ck_app_perm_action
        CHECK (action_code IN (
            'VIEW',
            'ADD',
            'EDIT',
            'DELETE',
            'PRINT',
            'GENERATE',
            'EXPORT',
            'MANAGE'
        )),

    CONSTRAINT ck_app_perm_status
        CHECK (status IN (0, 1))
);

CREATE TABLE app_role_permissions (
    role_id         NUMBER NOT NULL,
    permission_code VARCHAR2(80) NOT NULL,
    is_allowed      NUMBER(1) DEFAULT 0 NOT NULL,
    created_at      TIMESTAMP DEFAULT SYSTIMESTAMP NOT NULL,
    updated_at      TIMESTAMP,

    CONSTRAINT pk_app_role_permissions
        PRIMARY KEY (role_id, permission_code),

    CONSTRAINT fk_app_role_perm_role
        FOREIGN KEY (role_id) REFERENCES app_roles(role_id),

    CONSTRAINT fk_app_role_perm_permission
        FOREIGN KEY (permission_code) REFERENCES app_permissions(permission_code),

    CONSTRAINT ck_app_role_perm_allowed
        CHECK (is_allowed IN (0, 1))
);

CREATE INDEX ix_app_permissions_module
    ON app_permissions(module_code, display_order);

CREATE INDEX ix_app_role_permissions_perm
    ON app_role_permissions(permission_code);

PROMPT ===== Loading application modules =====

INSERT INTO app_modules (module_code, module_name, display_order) VALUES ('OVERVIEW', 'Overview', 10);
INSERT INTO app_modules (module_code, module_name, display_order) VALUES ('ORDERS', 'Purchase Orders', 20);
INSERT INTO app_modules (module_code, module_name, display_order) VALUES ('SUPPLIERS', 'Suppliers', 30);
INSERT INTO app_modules (module_code, module_name, display_order) VALUES ('PRODUCTS', 'Products', 40);
INSERT INTO app_modules (module_code, module_name, display_order) VALUES ('REPORTS', 'Reports', 50);
INSERT INTO app_modules (module_code, module_name, display_order) VALUES ('USERS', 'Users', 80);
INSERT INTO app_modules (module_code, module_name, display_order) VALUES ('SETTINGS', 'Settings / Permissions', 90);

PROMPT ===== Loading permissions =====

INSERT INTO app_permissions VALUES ('OVERVIEW_VIEW', 'OVERVIEW', 'VIEW', 'Overview (View)', 10, 1, SYSTIMESTAMP);

INSERT INTO app_permissions VALUES ('ORDERS_VIEW', 'ORDERS', 'VIEW', 'Purchase Orders (View)', 10, 1, SYSTIMESTAMP);
INSERT INTO app_permissions VALUES ('ORDERS_ADD', 'ORDERS', 'ADD', 'Purchase Orders (Add)', 20, 1, SYSTIMESTAMP);
INSERT INTO app_permissions VALUES ('ORDERS_EDIT', 'ORDERS', 'EDIT', 'Purchase Orders (Edit)', 30, 1, SYSTIMESTAMP);
INSERT INTO app_permissions VALUES ('ORDERS_DELETE', 'ORDERS', 'DELETE', 'Purchase Orders (Delete)', 40, 1, SYSTIMESTAMP);
INSERT INTO app_permissions VALUES ('ORDERS_PRINT', 'ORDERS', 'PRINT', 'Purchase Orders (Print)', 50, 1, SYSTIMESTAMP);

INSERT INTO app_permissions VALUES ('SUPPLIERS_VIEW', 'SUPPLIERS', 'VIEW', 'Suppliers (View)', 10, 1, SYSTIMESTAMP);
INSERT INTO app_permissions VALUES ('SUPPLIERS_ADD', 'SUPPLIERS', 'ADD', 'Suppliers (Add)', 20, 1, SYSTIMESTAMP);
INSERT INTO app_permissions VALUES ('SUPPLIERS_EDIT', 'SUPPLIERS', 'EDIT', 'Suppliers (Edit)', 30, 1, SYSTIMESTAMP);
INSERT INTO app_permissions VALUES ('SUPPLIERS_DELETE', 'SUPPLIERS', 'DELETE', 'Suppliers (Delete)', 40, 1, SYSTIMESTAMP);

INSERT INTO app_permissions VALUES ('PRODUCTS_VIEW', 'PRODUCTS', 'VIEW', 'Products (View)', 10, 1, SYSTIMESTAMP);
INSERT INTO app_permissions VALUES ('PRODUCTS_ADD', 'PRODUCTS', 'ADD', 'Products (Add)', 20, 1, SYSTIMESTAMP);
INSERT INTO app_permissions VALUES ('PRODUCTS_EDIT', 'PRODUCTS', 'EDIT', 'Products (Edit)', 30, 1, SYSTIMESTAMP);
INSERT INTO app_permissions VALUES ('PRODUCTS_DELETE', 'PRODUCTS', 'DELETE', 'Products (Delete)', 40, 1, SYSTIMESTAMP);

INSERT INTO app_permissions VALUES ('REPORTS_VIEW', 'REPORTS', 'VIEW', 'Reports (View)', 10, 1, SYSTIMESTAMP);
INSERT INTO app_permissions VALUES ('REPORTS_GENERATE', 'REPORTS', 'GENERATE', 'Reports (Generate)', 20, 1, SYSTIMESTAMP);
INSERT INTO app_permissions VALUES ('REPORTS_EXPORT', 'REPORTS', 'EXPORT', 'Reports (Export)', 30, 1, SYSTIMESTAMP);

INSERT INTO app_permissions VALUES ('USERS_VIEW', 'USERS', 'VIEW', 'Users (View)', 10, 1, SYSTIMESTAMP);
INSERT INTO app_permissions VALUES ('USERS_ADD', 'USERS', 'ADD', 'Users (Add)', 20, 1, SYSTIMESTAMP);
INSERT INTO app_permissions VALUES ('USERS_EDIT', 'USERS', 'EDIT', 'Users (Edit)', 30, 1, SYSTIMESTAMP);
INSERT INTO app_permissions VALUES ('USERS_DELETE', 'USERS', 'DELETE', 'Users (Delete)', 40, 1, SYSTIMESTAMP);

INSERT INTO app_permissions VALUES ('SETTINGS_VIEW', 'SETTINGS', 'VIEW', 'Settings / Permissions (View)', 10, 1, SYSTIMESTAMP);
INSERT INTO app_permissions VALUES ('SETTINGS_MANAGE', 'SETTINGS', 'MANAGE', 'Settings / Permissions (Manage)', 20, 1, SYSTIMESTAMP);

PROMPT ===== Loading default permissions by role =====

-- Create every role/permission row. is_allowed = 1 means ALLOW, 0 means DENY.
INSERT INTO app_role_permissions (role_id, permission_code, is_allowed)
SELECT r.role_id,
       p.permission_code,
       0
  FROM app_roles r
 CROSS JOIN app_permissions p;

-- ADMIN: full access, including user and permission management.
UPDATE app_role_permissions
   SET is_allowed = 1,
       updated_at = SYSTIMESTAMP
 WHERE role_id = (SELECT role_id FROM app_roles WHERE role_code = 'ADMIN');

-- REQUESTER: can create/view/print purchase orders and view reports.
UPDATE app_role_permissions
   SET is_allowed = 1,
       updated_at = SYSTIMESTAMP
 WHERE role_id = (SELECT role_id FROM app_roles WHERE role_code = 'REQUESTER')
   AND permission_code IN (
       'OVERVIEW_VIEW',
       'ORDERS_VIEW',
       'ORDERS_ADD',
       'ORDERS_PRINT',
       'REPORTS_VIEW'
   );

-- PURCHASING_MANAGER: purchasing access, but no Settings unless Admin grants it.
UPDATE app_role_permissions
   SET is_allowed = 1,
       updated_at = SYSTIMESTAMP
 WHERE role_id = (SELECT role_id FROM app_roles WHERE role_code = 'PURCHASING_MANAGER')
   AND permission_code IN (
       'OVERVIEW_VIEW',
       'ORDERS_VIEW',
       'ORDERS_ADD',
       'ORDERS_EDIT',
       'ORDERS_DELETE',
       'ORDERS_PRINT',
       'SUPPLIERS_VIEW',
       'SUPPLIERS_ADD',
       'SUPPLIERS_EDIT',
       'SUPPLIERS_DELETE',
       'PRODUCTS_VIEW',
       'PRODUCTS_ADD',
       'PRODUCTS_EDIT',
       'PRODUCTS_DELETE',
       'REPORTS_VIEW',
       'REPORTS_GENERATE',
       'REPORTS_EXPORT'
   );

-- STOREKEEPER: can view orders, manage products, and view reports.
UPDATE app_role_permissions
   SET is_allowed = 1,
       updated_at = SYSTIMESTAMP
 WHERE role_id = (SELECT role_id FROM app_roles WHERE role_code = 'STOREKEEPER')
   AND permission_code IN (
       'OVERVIEW_VIEW',
       'ORDERS_VIEW',
       'ORDERS_PRINT',
       'PRODUCTS_VIEW',
       'PRODUCTS_ADD',
       'PRODUCTS_EDIT',
       'REPORTS_VIEW'
   );

-- ACCOUNTANT: can view master/purchasing data and generate/export reports.
UPDATE app_role_permissions
   SET is_allowed = 1,
       updated_at = SYSTIMESTAMP
 WHERE role_id = (SELECT role_id FROM app_roles WHERE role_code = 'ACCOUNTANT')
   AND permission_code IN (
       'OVERVIEW_VIEW',
       'ORDERS_VIEW',
       'SUPPLIERS_VIEW',
       'PRODUCTS_VIEW',
       'REPORTS_VIEW',
       'REPORTS_GENERATE',
       'REPORTS_EXPORT'
   );

PROMPT ===== Creating permission lookup views =====

CREATE OR REPLACE VIEW vw_app_role_permissions AS
SELECT r.role_id,
       r.role_code,
       r.role_name,
       m.module_code,
       m.module_name,
       p.action_code,
       p.permission_code,
       p.permission_name,
       NVL(rp.is_allowed, 0) AS is_allowed,
       m.display_order AS module_order,
       p.display_order AS permission_order
  FROM app_roles r
 CROSS JOIN app_permissions p
  JOIN app_modules m
    ON m.module_code = p.module_code
  LEFT JOIN app_role_permissions rp
    ON rp.role_id = r.role_id
   AND rp.permission_code = p.permission_code
 WHERE r.status = 1
   AND m.status = 1
   AND p.status = 1;

CREATE OR REPLACE VIEW vw_app_user_permissions AS
SELECT u.user_id,
       u.username,
       e.full_name,
       u.role_id,
       rp.role_code,
       rp.role_name,
       rp.module_code,
       rp.module_name,
       rp.action_code,
       rp.permission_code,
       rp.permission_name,
       rp.is_allowed,
       rp.module_order,
       rp.permission_order
  FROM app_users u
  JOIN employees e
    ON e.employee_id = u.employee_id
  JOIN vw_app_role_permissions rp
    ON rp.role_id = u.role_id
 WHERE u.status = 1
   AND e.status = 1;

COMMENT ON TABLE app_roles IS 'Application roles/groups used by APP_USERS.ROLE_ID';
COMMENT ON TABLE app_modules IS 'Application modules shown in the permission screen';
COMMENT ON TABLE app_permissions IS 'Individual module actions such as VIEW, ADD, EDIT and DELETE';
COMMENT ON TABLE app_role_permissions IS 'Allowed permissions for each application role';
COMMENT ON COLUMN app_role_permissions.is_allowed IS '0=DENY, 1=ALLOW';

COMMIT;

PROMPT ===== Application permissions created =====

SELECT r.role_id, r.role_code, rp.permission_code, rp.is_allowed
  FROM app_role_permissions rp
  JOIN app_roles r
    ON r.role_id = rp.role_id
 ORDER BY r.role_id, rp.permission_code;
