-- Run as PURCHASING_USER from the ORACLE directory with SQL*Plus, SQLcl,
-- or SQL Developer's Run Script command (F5).

WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK;
SET DEFINE OFF;
SET SERVEROUTPUT ON;

PROMPT ===== 1. Creating tables, sequences, constraints, and indexes =====
@@002_purchasing_management.sql

PROMPT ===== 2. Creating total and payment triggers =====
@@003_business_triggers.sql

PROMPT ===== 3. Creating stock triggers =====
@@004_trigger_update_stock.sql

PROMPT ===== 4. Creating stored procedures =====
@@005_sp.sql

PROMPT ===== 5. Creating report views =====
@@006_views.sql
@@007_stock_report.sql

PROMPT ===== 6. Loading end-to-end sample data =====
@@008_sample_data.sql

PROMPT ===== 7. Creating application users =====
@@009_app_users.sql

PROMPT ===== Installation completed successfully =====
