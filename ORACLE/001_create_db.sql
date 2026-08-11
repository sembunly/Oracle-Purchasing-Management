-- Run this file as SYSTEM (or another DBA user) while connected to a PDB,
-- for example XEPDB1 or FREEPDB1. Do not run it in CDB$ROOT.

ALTER SESSION SET "_ORACLE_SCRIPT" = true;

CREATE USER purchasing_user
IDENTIFIED BY "0000"
DEFAULT TABLESPACE users
TEMPORARY TABLESPACE temp
QUOTA 100M ON users;

GRANT CREATE SESSION,
      CREATE TABLE,
      CREATE SEQUENCE,
      CREATE TRIGGER,
      CREATE VIEW,
      CREATE PROCEDURE
TO purchasing_user;

-- Development credentials used by this classroom project:
-- Username: purchasing_user
-- Password: 0000
-- Use a strong password outside a classroom/development environment.
