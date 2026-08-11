-- End-to-end demonstration data for the purchasing workflow.
-- Requires schema, triggers, and stored procedures to be installed first.

INSERT INTO employees (
    employee_id, employee_code, full_name, email,
    department, job_title, status
) VALUES (
    employee_seq.NEXTVAL, 'EMP001', 'Sok Dara', 'dara@example.com',
    'Administration', 'Requester', 1
);

INSERT INTO employees (
    employee_id, employee_code, full_name, email,
    department, job_title, status
) VALUES (
    employee_seq.NEXTVAL, 'EMP002', 'Chan Vanna', 'vanna@example.com',
    'Management', 'Purchasing Manager', 1
);

INSERT INTO employees (
    employee_id, employee_code, full_name, email,
    department, job_title, status
) VALUES (
    employee_seq.NEXTVAL, 'EMP003', 'Lim Sopheak', 'sopheak@example.com',
    'Warehouse', 'Storekeeper', 1
);

INSERT INTO employees (
    employee_id, employee_code, full_name, email,
    department, job_title, status
) VALUES (
    employee_seq.NEXTVAL, 'EMP004', 'Kim Maly', 'maly@example.com',
    'Finance', 'Accountant', 1
);

INSERT INTO suppliers (
    supplier_id, supplier_code, supplier_name, contact_person,
    phone, email, address, tax_number, status
) VALUES (
    supplier_seq.NEXTVAL, 'SUP001', 'ABC Computer', 'Mr. Visal',
    '012345678', 'sales@abccomputer.com', 'Phnom Penh', 'K001-100000001', 1
);

INSERT INTO suppliers (
    supplier_id, supplier_code, supplier_name, contact_person,
    phone, email, address, tax_number, status
) VALUES (
    supplier_seq.NEXTVAL, 'SUP002', 'Global Office Supply', 'Ms. Lina',
    '098765432', 'sales@globaloffice.com', 'Kandal', 'K001-100000002', 1
);

INSERT INTO products (
    product_id, product_code, product_name, category, unit,
    unit_price, stock_qty, reorder_level, preferred_supplier_id, status
) VALUES (
    product_seq.NEXTVAL, 'P001', 'Dell Laptop', 'Computer', 'UNIT',
    700, 5, 3,
    (SELECT supplier_id FROM suppliers WHERE supplier_code = 'SUP001'),
    1
);

INSERT INTO products (
    product_id, product_code, product_name, category, unit,
    unit_price, stock_qty, reorder_level, preferred_supplier_id, status
) VALUES (
    product_seq.NEXTVAL, 'P002', 'HP Printer', 'Office', 'UNIT',
    250, 8, 5,
    (SELECT supplier_id FROM suppliers WHERE supplier_code = 'SUP002'),
    1
);

INSERT INTO products (
    product_id, product_code, product_name, category, unit,
    unit_price, stock_qty, reorder_level, preferred_supplier_id, status
) VALUES (
    product_seq.NEXTVAL, 'P003', 'Wireless Mouse', 'Computer', 'UNIT',
    20, 2, 10,
    (SELECT supplier_id FROM suppliers WHERE supplier_code = 'SUP001'),
    1
);

-- Request 1 follows the complete workflow.
INSERT INTO purchase_requests (
    request_id, request_no, request_date, requested_by,
    needed_date, purpose, status
) VALUES (
    request_seq.NEXTVAL, 'PR-2026-001', TRUNC(SYSDATE),
    (SELECT employee_id FROM employees WHERE employee_code = 'EMP001'),
    TRUNC(SYSDATE) + 14, 'New equipment for administration team', 1
);

INSERT INTO purchase_request_items (
    request_item_id, request_id, product_id,
    quantity, estimated_unit_price, notes
) VALUES (
    request_item_seq.NEXTVAL,
    (SELECT request_id FROM purchase_requests WHERE request_no = 'PR-2026-001'),
    (SELECT product_id FROM products WHERE product_code = 'P001'),
    2, 700, 'Two laptops for new staff'
);

INSERT INTO purchase_request_items (
    request_item_id, request_id, product_id,
    quantity, estimated_unit_price, notes
) VALUES (
    request_item_seq.NEXTVAL,
    (SELECT request_id FROM purchase_requests WHERE request_no = 'PR-2026-001'),
    (SELECT product_id FROM products WHERE product_code = 'P002'),
    1, 250, 'Shared office printer'
);

INSERT INTO purchase_request_approvals (
    approval_id, request_id, approval_level, approver_id, decision
) VALUES (
    approval_seq.NEXTVAL,
    (SELECT request_id FROM purchase_requests WHERE request_no = 'PR-2026-001'),
    1,
    (SELECT employee_id FROM employees WHERE employee_code = 'EMP002'),
    0
);

DECLARE
    v_request_id  purchase_requests.request_id%TYPE;
    v_approver_id employees.employee_id%TYPE;
BEGIN
    SELECT request_id
      INTO v_request_id
      FROM purchase_requests
     WHERE request_no = 'PR-2026-001';

    SELECT employee_id
      INTO v_approver_id
      FROM employees
     WHERE employee_code = 'EMP002';

    sp_approve_request(
        v_request_id,
        v_approver_id,
        1,
        'Budget and business need approved.'
    );
END;
/

-- Request 2 remains pending so VW_PENDING_APPROVALS has demonstration data.
INSERT INTO purchase_requests (
    request_id, request_no, request_date, requested_by,
    needed_date, purpose, status
) VALUES (
    request_seq.NEXTVAL, 'PR-2026-002', TRUNC(SYSDATE),
    (SELECT employee_id FROM employees WHERE employee_code = 'EMP001'),
    TRUNC(SYSDATE) + 21, 'Replace old computer accessories', 1
);

INSERT INTO purchase_request_items (
    request_item_id, request_id, product_id,
    quantity, estimated_unit_price
) VALUES (
    request_item_seq.NEXTVAL,
    (SELECT request_id FROM purchase_requests WHERE request_no = 'PR-2026-002'),
    (SELECT product_id FROM products WHERE product_code = 'P003'),
    10, 20
);

INSERT INTO purchase_request_approvals (
    approval_id, request_id, approval_level, approver_id, decision
) VALUES (
    approval_seq.NEXTVAL,
    (SELECT request_id FROM purchase_requests WHERE request_no = 'PR-2026-002'),
    1,
    (SELECT employee_id FROM employees WHERE employee_code = 'EMP002'),
    0
);

-- Two suppliers quote against the approved request.
INSERT INTO quotations (
    quotation_id, quotation_no, request_id, supplier_id,
    quotation_date, valid_until, status, notes
) VALUES (
    quotation_seq.NEXTVAL, 'QT-2026-001',
    (SELECT request_id FROM purchase_requests WHERE request_no = 'PR-2026-001'),
    (SELECT supplier_id FROM suppliers WHERE supplier_code = 'SUP001'),
    TRUNC(SYSDATE), TRUNC(SYSDATE) + 30, 0,
    'Best price and delivery time'
);

INSERT INTO quotation_items (
    quotation_item_id, quotation_id, product_id, quantity, unit_price
) VALUES (
    quotation_item_seq.NEXTVAL,
    (SELECT quotation_id FROM quotations WHERE quotation_no = 'QT-2026-001'),
    (SELECT product_id FROM products WHERE product_code = 'P001'),
    2, 680
);

INSERT INTO quotation_items (
    quotation_item_id, quotation_id, product_id, quantity, unit_price
) VALUES (
    quotation_item_seq.NEXTVAL,
    (SELECT quotation_id FROM quotations WHERE quotation_no = 'QT-2026-001'),
    (SELECT product_id FROM products WHERE product_code = 'P002'),
    1, 240
);

INSERT INTO quotations (
    quotation_id, quotation_no, request_id, supplier_id,
    quotation_date, valid_until, status
) VALUES (
    quotation_seq.NEXTVAL, 'QT-2026-002',
    (SELECT request_id FROM purchase_requests WHERE request_no = 'PR-2026-001'),
    (SELECT supplier_id FROM suppliers WHERE supplier_code = 'SUP002'),
    TRUNC(SYSDATE), TRUNC(SYSDATE) + 30, 0
);

INSERT INTO quotation_items (
    quotation_item_id, quotation_id, product_id, quantity, unit_price
) VALUES (
    quotation_item_seq.NEXTVAL,
    (SELECT quotation_id FROM quotations WHERE quotation_no = 'QT-2026-002'),
    (SELECT product_id FROM products WHERE product_code = 'P001'),
    2, 695
);

INSERT INTO quotation_items (
    quotation_item_id, quotation_id, product_id, quantity, unit_price
) VALUES (
    quotation_item_seq.NEXTVAL,
    (SELECT quotation_id FROM quotations WHERE quotation_no = 'QT-2026-002'),
    (SELECT product_id FROM products WHERE product_code = 'P002'),
    1, 245
);

UPDATE quotations
   SET status = CASE quotation_no
       WHEN 'QT-2026-001' THEN 1
       ELSE 2
   END
 WHERE quotation_no IN ('QT-2026-001', 'QT-2026-002');

-- Create a PO through PL/SQL, then add its detail lines.
DECLARE
    v_po_id        purchase_orders.po_id%TYPE;
    v_request_id   purchase_requests.request_id%TYPE;
    v_quotation_id quotations.quotation_id%TYPE;
    v_created_by   employees.employee_id%TYPE;
BEGIN
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
        'PO-2026-001',
        v_request_id,
        v_quotation_id,
        TRUNC(SYSDATE) + 10,
        v_created_by,
        70,
        v_po_id
    );

    UPDATE purchase_orders
       SET status = 1,
           approved_by = (
               SELECT employee_id
                 FROM employees
                WHERE employee_code = 'EMP002'
           )
     WHERE po_id = v_po_id;
END;
/

-- Receive part of the PO. Stock changes only after status becomes RECEIVED.
INSERT INTO goods_receipts (
    receipt_id, receipt_no, po_id, receipt_date,
    received_by, status, notes
) VALUES (
    receipt_seq.NEXTVAL, 'GR-2026-001',
    (SELECT po_id FROM purchase_orders WHERE po_no = 'PO-2026-001'),
    TRUNC(SYSDATE),
    (SELECT employee_id FROM employees WHERE employee_code = 'EMP003'),
    0, 'First partial delivery'
);

INSERT INTO goods_receipt_items (
    receipt_item_id, receipt_id, product_id,
    received_qty, rejected_qty
) VALUES (
    receipt_item_seq.NEXTVAL,
    (SELECT receipt_id FROM goods_receipts WHERE receipt_no = 'GR-2026-001'),
    (SELECT product_id FROM products WHERE product_code = 'P001'),
    1, 0
);

INSERT INTO goods_receipt_items (
    receipt_item_id, receipt_id, product_id,
    received_qty, rejected_qty
) VALUES (
    receipt_item_seq.NEXTVAL,
    (SELECT receipt_id FROM goods_receipts WHERE receipt_no = 'GR-2026-001'),
    (SELECT product_id FROM products WHERE product_code = 'P002'),
    1, 0
);

UPDATE goods_receipts
   SET status = 1
 WHERE receipt_no = 'GR-2026-001';

UPDATE purchase_orders
   SET status = 2
 WHERE po_no = 'PO-2026-001';

-- Supplier invoice and a partial payment.
INSERT INTO supplier_invoices (
    invoice_id, invoice_no, po_id, invoice_date, due_date,
    subtotal_amount, tax_amount, total_amount, paid_amount, status
) VALUES (
    invoice_seq.NEXTVAL, 'INV-ABC-001',
    (SELECT po_id FROM purchase_orders WHERE po_no = 'PO-2026-001'),
    TRUNC(SYSDATE), TRUNC(SYSDATE) + 30,
    1600, 70, 1670, 0, 0
);

DECLARE
    v_payment_id payments.payment_id%TYPE;
    v_invoice_id supplier_invoices.invoice_id%TYPE;
BEGIN
    SELECT invoice_id
      INTO v_invoice_id
      FROM supplier_invoices
     WHERE invoice_no = 'INV-ABC-001';

    sp_record_payment(
        'PAY-2026-001',
        v_invoice_id,
        1000,
        'BANK_TRANSFER',
        'BANK-REF-0001',
        v_payment_id
    );
END;
/

COMMIT;
