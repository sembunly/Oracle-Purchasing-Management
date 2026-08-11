-- Run as PURCHASING_USER from the ORACLE directory with SQL*Plus, SQLcl,
-- or SQL Developer's Run Script command (F5).

WHENEVER SQLERROR EXIT SQL.SQLCODE ROLLBACK;
SET DEFINE OFF;
SET SERVEROUTPUT ON;

PROMPT ===== 1. Creating tables, sequences, constraints, and indexes =====
@@purchasing_management.sql

PROMPT ===== 2. Creating total and payment triggers =====
@@business_triggers.sql

PROMPT ===== 3. Creating stock triggers =====
@@trigger_update_stock.sql

PROMPT ===== 4. Creating stored procedures =====
@@sp.sql

PROMPT ===== 5. Creating report views =====
@@views.sql
@@stock_report.sql

PROMPT ===== 6. Loading end-to-end sample data =====
@@sample_data.sql

PROMPT ===== Installation completed successfully =====
