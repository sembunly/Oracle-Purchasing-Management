SET SERVEROUTPUT ON;

PROMPT ===== Invalid schema objects (expected: no rows) =====
SELECT object_name, object_type, status
  FROM user_objects
 WHERE status = 'INVALID'
 ORDER BY object_type, object_name;

PROMPT ===== PL/SQL compilation errors (expected: no rows) =====
SELECT name, type, line, position, text
  FROM user_errors
 ORDER BY name, sequence;

PROMPT ===== Row counts for every module =====
SELECT 'PRODUCTS' AS module_name, COUNT(*) AS row_count FROM products
UNION ALL SELECT 'SUPPLIERS', COUNT(*) FROM suppliers
UNION ALL SELECT 'PURCHASE REQUESTS', COUNT(*) FROM purchase_requests
UNION ALL SELECT 'APPROVALS', COUNT(*) FROM purchase_request_approvals
UNION ALL SELECT 'QUOTATIONS', COUNT(*) FROM quotations
UNION ALL SELECT 'PURCHASE ORDERS', COUNT(*) FROM purchase_orders
UNION ALL SELECT 'GOODS RECEIPTS', COUNT(*) FROM goods_receipts
UNION ALL SELECT 'SUPPLIER INVOICES', COUNT(*) FROM supplier_invoices
UNION ALL SELECT 'PAYMENTS', COUNT(*) FROM payments;

PROMPT ===== Full workflow reports =====
SELECT * FROM vw_purchase_report ORDER BY po_date, po_no;
SELECT * FROM vw_pending_approvals ORDER BY request_date, request_no;
SELECT * FROM vw_receiving_report ORDER BY po_no, product_code;
SELECT * FROM vw_invoice_payment_report ORDER BY invoice_date, invoice_no;
SELECT * FROM vw_supplier_performance ORDER BY supplier_code;
SELECT * FROM vw_stock_report ORDER BY product_code;

PROMPT ===== Trigger checks =====
SELECT quotation_no, total_amount
  FROM quotations
 ORDER BY quotation_no;

SELECT po_no, subtotal_amount, tax_amount, total_amount
  FROM purchase_orders
 ORDER BY po_no;

SELECT invoice_no, total_amount, paid_amount, status
  FROM supplier_invoices
 ORDER BY invoice_no;

PROMPT ===== Transaction test: procedure does not commit =====
DECLARE
    v_po_id        purchase_orders.po_id%TYPE;
    v_request_id   purchase_requests.request_id%TYPE;
    v_quotation_id quotations.quotation_id%TYPE;
    v_created_by   employees.employee_id%TYPE;
BEGIN
    SAVEPOINT before_po_test;

    SELECT request_id
      INTO v_request_id
      FROM purchase_requests
     WHERE request_no = 'PR-2026-001';

    SELECT quotation_id
      INTO v_quotation_id
      FROM quotations
     WHERE quotation_no = 'QT-2026-001';

    SELECT employee_id
      INTO v_created_by
      FROM employees
     WHERE employee_code = 'EMP001';

    sp_create_po(
        'PO-ROLLBACK-TEST',
        v_request_id,
        v_quotation_id,
        TRUNC(SYSDATE) + 20,
        v_created_by,
        0,
        v_po_id
    );

    DBMS_OUTPUT.PUT_LINE('Temporary PO created with ID ' || v_po_id);
    ROLLBACK TO before_po_test;
    DBMS_OUTPUT.PUT_LINE('Temporary PO rolled back successfully.');
END;
/

SELECT COUNT(*) AS rollback_test_rows
  FROM purchase_orders
 WHERE po_no = 'PO-ROLLBACK-TEST';
