-- Application users for the Oracle Purchasing Management System
-- Run this script as PURCHASING_USER after 002_purchasing_management.sql.
-- Sample password for the classroom users below is: 1234

SET DEFINE OFF;
SET SERVEROUTPUT ON;
WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK;

PROMPT ===== Creating application users table =====

CREATE TABLE app_roles (
    role_id      NUMBER PRIMARY KEY,
    role_code    VARCHAR2(30) NOT NULL UNIQUE,
    role_name    VARCHAR2(100) NOT NULL,
    description  VARCHAR2(255),
    status       NUMBER(1) DEFAULT 1 NOT NULL,
    created_at   TIMESTAMP DEFAULT SYSTIMESTAMP NOT NULL,

    CONSTRAINT ck_app_roles_status
        CHECK (status IN (0, 1))
);

INSERT INTO app_roles (role_id, role_code, role_name, description) VALUES
    (1, 'ADMIN', 'Admin', 'Full system administrator with access to users, settings and all modules');

INSERT INTO app_roles (role_id, role_code, role_name, description) VALUES
    (2, 'REQUESTER', 'Requester', 'Creates purchase requests and views own purchasing work');

INSERT INTO app_roles (role_id, role_code, role_name, description) VALUES
    (3, 'PURCHASING_MANAGER', 'Purchasing Manager', 'Manages purchasing setup, orders, suppliers and approvals');

INSERT INTO app_roles (role_id, role_code, role_name, description) VALUES
    (4, 'STOREKEEPER', 'Storekeeper', 'Views orders and manages product/stock receiving work');

INSERT INTO app_roles (role_id, role_code, role_name, description) VALUES
    (5, 'ACCOUNTANT', 'Accountant', 'Views purchasing records and works with reports/finance');

CREATE TABLE app_users (
    user_id       NUMBER PRIMARY KEY,
    employee_id   NUMBER NOT NULL UNIQUE,
    username      VARCHAR2(50) NOT NULL UNIQUE,
    password_hash VARCHAR2(64) NOT NULL,
    role_id       NUMBER NOT NULL,
    status        NUMBER(1) DEFAULT 1 NOT NULL, -- 0: INACTIVE, 1: ACTIVE
    created_at    TIMESTAMP DEFAULT SYSTIMESTAMP NOT NULL,

    CONSTRAINT fk_app_user_employee
        FOREIGN KEY (employee_id) REFERENCES employees(employee_id),

    CONSTRAINT fk_app_user_role
        FOREIGN KEY (role_id) REFERENCES app_roles(role_id),

    CONSTRAINT ck_app_user_status
        CHECK (status IN (0, 1))
);

CREATE SEQUENCE app_user_seq
    START WITH 1
    INCREMENT BY 1
    NOCYCLE;

PROMPT ===== Creating sample application users =====

MERGE INTO employees target
USING (
    SELECT 'EMP000' AS employee_code,
           'System Admin' AS full_name,
           'admin@example.com' AS email,
           'Administration' AS department,
           'System Administrator' AS job_title,
           1 AS status
    FROM dual
) source
ON (target.employee_code = source.employee_code)
WHEN MATCHED THEN UPDATE SET
    target.full_name = source.full_name,
    target.email = source.email,
    target.department = source.department,
    target.job_title = source.job_title,
    target.status = source.status
WHEN NOT MATCHED THEN INSERT
    (employee_id, employee_code, full_name, email, department, job_title, status)
VALUES
    (employee_seq.NEXTVAL, source.employee_code, source.full_name, source.email,
     source.department, source.job_title, source.status);

MERGE INTO app_users target
USING (
    SELECT employee_id, 'admin' AS username,
           RAWTOHEX(STANDARD_HASH('1234', 'SHA256')) AS password_hash,
           (SELECT role_id FROM app_roles WHERE role_code = 'ADMIN') AS role_id
    FROM employees
    WHERE employee_code = 'EMP000'
) source
ON (target.username = source.username)
WHEN MATCHED THEN UPDATE SET
    target.employee_id = source.employee_id,
    target.password_hash = source.password_hash,
    target.role_id = source.role_id,
    target.status = 1
WHEN NOT MATCHED THEN INSERT
    (user_id, employee_id, username, password_hash, role_id, status)
VALUES
    (app_user_seq.NEXTVAL, source.employee_id, source.username,
     source.password_hash, source.role_id, 1);

MERGE INTO app_users target
USING (
    SELECT employee_id, 'sokdara' AS username,
           RAWTOHEX(STANDARD_HASH('1234', 'SHA256')) AS password_hash,
           (SELECT role_id FROM app_roles WHERE role_code = 'REQUESTER') AS role_id
    FROM employees
    WHERE employee_code = 'EMP001'
) source
ON (target.username = source.username)
WHEN MATCHED THEN UPDATE SET
    target.employee_id = source.employee_id,
    target.password_hash = source.password_hash,
    target.role_id = source.role_id,
    target.status = 1
WHEN NOT MATCHED THEN INSERT
    (user_id, employee_id, username, password_hash, role_id, status)
VALUES
    (app_user_seq.NEXTVAL, source.employee_id, source.username,
     source.password_hash, source.role_id, 1);

MERGE INTO app_users target
USING (
    SELECT employee_id, 'vanna' AS username,
           RAWTOHEX(STANDARD_HASH('1234', 'SHA256')) AS password_hash,
           (SELECT role_id FROM app_roles WHERE role_code = 'PURCHASING_MANAGER') AS role_id
    FROM employees
    WHERE employee_code = 'EMP002'
) source
ON (target.username = source.username)
WHEN MATCHED THEN UPDATE SET
    target.employee_id = source.employee_id,
    target.password_hash = source.password_hash,
    target.role_id = source.role_id,
    target.status = 1
WHEN NOT MATCHED THEN INSERT
    (user_id, employee_id, username, password_hash, role_id, status)
VALUES
    (app_user_seq.NEXTVAL, source.employee_id, source.username,
     source.password_hash, source.role_id, 1);

MERGE INTO app_users target
USING (
    SELECT employee_id, 'sopheak' AS username,
           RAWTOHEX(STANDARD_HASH('1234', 'SHA256')) AS password_hash,
           (SELECT role_id FROM app_roles WHERE role_code = 'STOREKEEPER') AS role_id
    FROM employees
    WHERE employee_code = 'EMP003'
) source
ON (target.username = source.username)
WHEN MATCHED THEN UPDATE SET
    target.employee_id = source.employee_id,
    target.password_hash = source.password_hash,
    target.role_id = source.role_id,
    target.status = 1
WHEN NOT MATCHED THEN INSERT
    (user_id, employee_id, username, password_hash, role_id, status)
VALUES
    (app_user_seq.NEXTVAL, source.employee_id, source.username,
     source.password_hash, source.role_id, 1);

MERGE INTO app_users target
USING (
    SELECT employee_id, 'maly' AS username,
           RAWTOHEX(STANDARD_HASH('1234', 'SHA256')) AS password_hash,
           (SELECT role_id FROM app_roles WHERE role_code = 'ACCOUNTANT') AS role_id
    FROM employees
    WHERE employee_code = 'EMP004'
) source
ON (target.username = source.username)
WHEN MATCHED THEN UPDATE SET
    target.employee_id = source.employee_id,
    target.password_hash = source.password_hash,
    target.role_id = source.role_id,
    target.status = 1
WHEN NOT MATCHED THEN INSERT
    (user_id, employee_id, username, password_hash, role_id, status)
VALUES
    (app_user_seq.NEXTVAL, source.employee_id, source.username,
     source.password_hash, source.role_id, 1);

COMMIT;

PROMPT ===== Application users created =====

SELECT u.username, u.role_id, r.role_code, u.status
  FROM app_users u
  JOIN app_roles r
    ON r.role_id = u.role_id
 ORDER BY u.user_id;
COMMENT ON TABLE app_roles IS 'Application roles/groups used by APP_USERS.ROLE_ID';
COMMENT ON COLUMN app_users.role_id IS 'Foreign key to APP_ROLES.ROLE_ID';
COMMENT ON COLUMN app_users.status IS '0: INACTIVE, 1: ACTIVE';
