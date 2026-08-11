-- Drop only application login/role/permission objects.
-- Run as PURCHASING_USER when you want to recreate app users and permissions.
--
-- This script does NOT drop business tables such as EMPLOYEES, PRODUCTS,
-- SUPPLIERS, PURCHASE_ORDERS, GOODS_RECEIPTS or PAYMENTS.

SET DEFINE OFF;
SET SERVEROUTPUT ON;
WHENEVER SQLERROR CONTINUE;

PROMPT ===== Dropping app security views =====

BEGIN
    EXECUTE IMMEDIATE 'DROP VIEW vw_app_user_permissions';
    DBMS_OUTPUT.PUT_LINE('Dropped view VW_APP_USER_PERMISSIONS');
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE = -942 THEN
            DBMS_OUTPUT.PUT_LINE('View VW_APP_USER_PERMISSIONS does not exist');
        ELSE
            RAISE;
        END IF;
END;
/

BEGIN
    EXECUTE IMMEDIATE 'DROP VIEW vw_app_role_permissions';
    DBMS_OUTPUT.PUT_LINE('Dropped view VW_APP_ROLE_PERMISSIONS');
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE = -942 THEN
            DBMS_OUTPUT.PUT_LINE('View VW_APP_ROLE_PERMISSIONS does not exist');
        ELSE
            RAISE;
        END IF;
END;
/

PROMPT ===== Dropping app security tables =====

BEGIN
    EXECUTE IMMEDIATE 'DROP TABLE app_role_permissions CASCADE CONSTRAINTS PURGE';
    DBMS_OUTPUT.PUT_LINE('Dropped table APP_ROLE_PERMISSIONS');
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE = -942 THEN
            DBMS_OUTPUT.PUT_LINE('Table APP_ROLE_PERMISSIONS does not exist');
        ELSE
            RAISE;
        END IF;
END;
/

BEGIN
    EXECUTE IMMEDIATE 'DROP TABLE app_permissions CASCADE CONSTRAINTS PURGE';
    DBMS_OUTPUT.PUT_LINE('Dropped table APP_PERMISSIONS');
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE = -942 THEN
            DBMS_OUTPUT.PUT_LINE('Table APP_PERMISSIONS does not exist');
        ELSE
            RAISE;
        END IF;
END;
/

BEGIN
    EXECUTE IMMEDIATE 'DROP TABLE app_modules CASCADE CONSTRAINTS PURGE';
    DBMS_OUTPUT.PUT_LINE('Dropped table APP_MODULES');
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE = -942 THEN
            DBMS_OUTPUT.PUT_LINE('Table APP_MODULES does not exist');
        ELSE
            RAISE;
        END IF;
END;
/

BEGIN
    EXECUTE IMMEDIATE 'DROP TABLE app_users CASCADE CONSTRAINTS PURGE';
    DBMS_OUTPUT.PUT_LINE('Dropped table APP_USERS');
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE = -942 THEN
            DBMS_OUTPUT.PUT_LINE('Table APP_USERS does not exist');
        ELSE
            RAISE;
        END IF;
END;
/

BEGIN
    EXECUTE IMMEDIATE 'DROP TABLE app_roles CASCADE CONSTRAINTS PURGE';
    DBMS_OUTPUT.PUT_LINE('Dropped table APP_ROLES');
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE = -942 THEN
            DBMS_OUTPUT.PUT_LINE('Table APP_ROLES does not exist');
        ELSE
            RAISE;
        END IF;
END;
/

PROMPT ===== Dropping app security sequence =====

BEGIN
    EXECUTE IMMEDIATE 'DROP SEQUENCE app_user_seq';
    DBMS_OUTPUT.PUT_LINE('Dropped sequence APP_USER_SEQ');
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE = -2289 THEN
            DBMS_OUTPUT.PUT_LINE('Sequence APP_USER_SEQ does not exist');
        ELSE
            RAISE;
        END IF;
END;
/

PROMPT ===== App security objects removed =====

SELECT object_type, object_name
  FROM user_objects
 WHERE object_name IN (
       'APP_USERS',
       'APP_USER_SEQ',
       'APP_ROLES',
       'APP_MODULES',
       'APP_PERMISSIONS',
       'APP_ROLE_PERMISSIONS',
       'VW_APP_ROLE_PERMISSIONS',
       'VW_APP_USER_PERMISSIONS'
 )
 ORDER BY object_type, object_name;
