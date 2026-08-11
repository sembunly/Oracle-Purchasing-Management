-- Application users for the Oracle Purchasing Management System
-- Run this script as PURCHASING_USER after purchasing_management.sql.
-- Sample password for the classroom users below is: 1234

SET DEFINE OFF;
SET SERVEROUTPUT ON;
WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK;

PROMPT ===== Creating application users table =====

CREATE TABLE app_users (
    user_id       NUMBER PRIMARY KEY,
    employee_id   NUMBER NOT NULL UNIQUE,
    username      VARCHAR2(50) NOT NULL UNIQUE,
    password_hash VARCHAR2(64) NOT NULL,
    role_code     VARCHAR2(30) NOT NULL,
    status        NUMBER(1) DEFAULT 1 NOT NULL,
    created_at    TIMESTAMP DEFAULT SYSTIMESTAMP NOT NULL,

    CONSTRAINT fk_app_user_employee
        FOREIGN KEY (employee_id) REFERENCES employees(employee_id),

    CONSTRAINT ck_app_user_role
        CHECK (role_code IN (
            'REQUESTER',
            'PURCHASING_MANAGER',
            'STOREKEEPER',
            'ACCOUNTANT'
        )),

    CONSTRAINT ck_app_user_status
        CHECK (status IN (0, 1))
);

CREATE SEQUENCE app_user_seq
    START WITH 1
    INCREMENT BY 1
    NOCYCLE;

PROMPT ===== Creating sample application users =====

MERGE INTO app_users target
USING (
    SELECT employee_id, 'sokdara' AS username,
           RAWTOHEX(STANDARD_HASH('1234', 'SHA256')) AS password_hash,
           'REQUESTER' AS role_code
    FROM employees
    WHERE employee_code = 'EMP001'
) source
ON (target.username = source.username)
WHEN MATCHED THEN UPDATE SET
    target.employee_id = source.employee_id,
    target.password_hash = source.password_hash,
    target.role_code = source.role_code,
    target.status = 1
WHEN NOT MATCHED THEN INSERT
    (user_id, employee_id, username, password_hash, role_code, status)
VALUES
    (app_user_seq.NEXTVAL, source.employee_id, source.username,
     source.password_hash, source.role_code, 1);

MERGE INTO app_users target
USING (
    SELECT employee_id, 'vanna' AS username,
           RAWTOHEX(STANDARD_HASH('1234', 'SHA256')) AS password_hash,
           'PURCHASING_MANAGER' AS role_code
    FROM employees
    WHERE employee_code = 'EMP002'
) source
ON (target.username = source.username)
WHEN MATCHED THEN UPDATE SET
    target.employee_id = source.employee_id,
    target.password_hash = source.password_hash,
    target.role_code = source.role_code,
    target.status = 1
WHEN NOT MATCHED THEN INSERT
    (user_id, employee_id, username, password_hash, role_code, status)
VALUES
    (app_user_seq.NEXTVAL, source.employee_id, source.username,
     source.password_hash, source.role_code, 1);

MERGE INTO app_users target
USING (
    SELECT employee_id, 'sopheak' AS username,
           RAWTOHEX(STANDARD_HASH('1234', 'SHA256')) AS password_hash,
           'STOREKEEPER' AS role_code
    FROM employees
    WHERE employee_code = 'EMP003'
) source
ON (target.username = source.username)
WHEN MATCHED THEN UPDATE SET
    target.employee_id = source.employee_id,
    target.password_hash = source.password_hash,
    target.role_code = source.role_code,
    target.status = 1
WHEN NOT MATCHED THEN INSERT
    (user_id, employee_id, username, password_hash, role_code, status)
VALUES
    (app_user_seq.NEXTVAL, source.employee_id, source.username,
     source.password_hash, source.role_code, 1);

MERGE INTO app_users target
USING (
    SELECT employee_id, 'maly' AS username,
           RAWTOHEX(STANDARD_HASH('1234', 'SHA256')) AS password_hash,
           'ACCOUNTANT' AS role_code
    FROM employees
    WHERE employee_code = 'EMP004'
) source
ON (target.username = source.username)
WHEN MATCHED THEN UPDATE SET
    target.employee_id = source.employee_id,
    target.password_hash = source.password_hash,
    target.role_code = source.role_code,
    target.status = 1
WHEN NOT MATCHED THEN INSERT
    (user_id, employee_id, username, password_hash, role_code, status)
VALUES
    (app_user_seq.NEXTVAL, source.employee_id, source.username,
     source.password_hash, source.role_code, 1);

COMMIT;

PROMPT ===== Application users created =====
SELECT username, role_code, status
FROM app_users
ORDER BY user_id;
COMMENT ON COLUMN app_users.status IS '0=INACTIVE, 1=ACTIVE';
