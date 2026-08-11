-- Run as PURCHASING_USER from the ORACLE directory with SQL*Plus, SQLcl,
-- or SQL Developer's Run Script command (F5).

WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK;
SET DEFINE OFF;
SET SERVEROUTPUT ON;

PROMPT ===== 1. Creating tables, sequences, constraints, and indexes =====
@C:\Oracle-Ass\ORACLE\002_purchasing_management.sql

PROMPT ===== 2. Creating total and payment triggers =====
@C:\Oracle-Ass\ORACLE\003_business_triggers.sql

PROMPT ===== 3. Creating stock triggers =====
@C:\Oracle-Ass\ORACLE\004_trigger_update_stock.sql

PROMPT ===== 4. Creating stored procedures =====
@C:\Oracle-Ass\ORACLE\005_sp.sql

PROMPT ===== 5. Creating report views =====
@C:\Oracle-Ass\ORACLE\006_views.sql
@C:\Oracle-Ass\ORACLE\007_stock_report.sql

PROMPT ===== 6. Loading end-to-end sample data =====
@C:\Oracle-Ass\ORACLE\008_sample_data.sql

PROMPT ===== 7. Creating application users =====
@C:\Oracle-Ass\ORACLE\009_app_users.sql

PROMPT ===== Installer finished. Verify that no ORA- or SP2- errors were printed above. =====
