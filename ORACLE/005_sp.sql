-- Stored procedures contain business rules but intentionally do not COMMIT.
-- The caller controls COMMIT or ROLLBACK for the complete transaction.

CREATE OR REPLACE PROCEDURE sp_approve_request (
    p_request_id  IN purchase_requests.request_id%TYPE,
    p_approver_id IN employees.employee_id%TYPE,
    p_decision    IN purchase_request_approvals.decision%TYPE,
    p_comments    IN purchase_request_approvals.comments%TYPE DEFAULT NULL
) AS
    v_decision      purchase_request_approvals.decision%TYPE;
    v_request_state purchase_requests.status%TYPE;
    v_pending_count NUMBER;
BEGIN
    v_decision := p_decision;

    IF v_decision IS NULL OR v_decision NOT IN (1, 2) THEN
        RAISE_APPLICATION_ERROR(-20001, 'Decision must be 1 (APPROVED) or 2 (REJECTED).');
    END IF;

    SELECT status
      INTO v_request_state
      FROM purchase_requests
     WHERE request_id = p_request_id
       FOR UPDATE;

    IF v_request_state <> 1 THEN
        RAISE_APPLICATION_ERROR(-20002, 'Only a request with status 1 (PENDING) can be approved.');
    END IF;

    UPDATE purchase_request_approvals
       SET decision = v_decision,
           comments = p_comments,
           decision_date = SYSDATE
     WHERE request_id = p_request_id
       AND approver_id = p_approver_id
       AND decision = 0
       AND status = 1;

    IF SQL%ROWCOUNT = 0 THEN
        RAISE_APPLICATION_ERROR(-20003, 'No pending approval was assigned to this approver.');
    END IF;

    IF v_decision = 2 THEN
        UPDATE purchase_requests
           SET status = 3
         WHERE request_id = p_request_id;
    ELSE
        SELECT COUNT(*)
          INTO v_pending_count
          FROM purchase_request_approvals
         WHERE request_id = p_request_id
           AND decision = 0
           AND status = 1;

        IF v_pending_count = 0 THEN
            UPDATE purchase_requests
               SET status = 2
             WHERE request_id = p_request_id;
        END IF;
    END IF;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RAISE_APPLICATION_ERROR(-20004, 'Purchase request was not found.');
END;
/

CREATE OR REPLACE PROCEDURE sp_create_po (
    p_po_no                   IN purchase_orders.po_no%TYPE,
    p_request_id              IN purchase_requests.request_id%TYPE,
    p_quotation_id            IN quotations.quotation_id%TYPE,
    p_expected_delivery_date  IN purchase_orders.expected_delivery_date%TYPE,
    p_created_by              IN employees.employee_id%TYPE,
    p_tax_amount              IN purchase_orders.tax_amount%TYPE,
    p_po_id                   OUT purchase_orders.po_id%TYPE
) AS
    v_request_status purchase_requests.status%TYPE;
    v_supplier_id    quotations.supplier_id%TYPE;
    v_quote_status   quotations.status%TYPE;
BEGIN
    SELECT status
      INTO v_request_status
      FROM purchase_requests
     WHERE request_id = p_request_id;

    IF v_request_status <> 2 THEN
        RAISE_APPLICATION_ERROR(-20011, 'Purchase request must have status 2 (APPROVED) first.');
    END IF;

    SELECT supplier_id, status
      INTO v_supplier_id, v_quote_status
      FROM quotations
     WHERE quotation_id = p_quotation_id
       AND request_id = p_request_id;

    IF v_quote_status <> 1 THEN
        RAISE_APPLICATION_ERROR(-20012, 'Quotation must have status 1 (SELECTED).');
    END IF;

    IF p_tax_amount < 0 THEN
        RAISE_APPLICATION_ERROR(-20013, 'Tax amount cannot be negative.');
    END IF;

    p_po_id := po_seq.NEXTVAL;

    INSERT INTO purchase_orders (
        po_id, po_no, request_id, quotation_id, supplier_id,
        po_date, expected_delivery_date, created_by, status,
        subtotal_amount, tax_amount, total_amount
    ) VALUES (
        p_po_id, p_po_no, p_request_id, p_quotation_id, v_supplier_id,
        SYSDATE, p_expected_delivery_date, p_created_by, 0,
        0, p_tax_amount, p_tax_amount
    );

    INSERT INTO purchase_order_items (
        po_item_id, po_id, product_id, quantity, unit_price
    )
    SELECT po_item_seq.NEXTVAL,
           p_po_id,
           product_id,
           quantity,
           unit_price
      FROM quotation_items
     WHERE quotation_id = p_quotation_id
       AND status = 1;

    IF SQL%ROWCOUNT = 0 THEN
        RAISE_APPLICATION_ERROR(-20015, 'Selected quotation has no items.');
    END IF;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RAISE_APPLICATION_ERROR(-20014, 'Request or matching quotation was not found.');
END;
/

CREATE OR REPLACE PROCEDURE sp_record_payment (
    p_payment_no      IN payments.payment_no%TYPE,
    p_invoice_id      IN supplier_invoices.invoice_id%TYPE,
    p_amount          IN payments.amount%TYPE,
    p_payment_method  IN payments.payment_method%TYPE,
    p_reference_no    IN payments.reference_no%TYPE,
    p_payment_id      OUT payments.payment_id%TYPE
) AS
    v_total_amount supplier_invoices.total_amount%TYPE;
    v_paid_amount  supplier_invoices.paid_amount%TYPE;
    v_status       supplier_invoices.status%TYPE;
BEGIN
    SELECT total_amount, paid_amount, status
      INTO v_total_amount, v_paid_amount, v_status
      FROM supplier_invoices
     WHERE invoice_id = p_invoice_id
       FOR UPDATE;

    IF v_status = 3 THEN
        RAISE_APPLICATION_ERROR(-20021, 'Cannot pay an invoice with status 3 (CANCELLED).');
    END IF;

    IF p_amount <= 0 THEN
        RAISE_APPLICATION_ERROR(-20022, 'Payment amount must be greater than zero.');
    END IF;

    IF p_amount > (v_total_amount - v_paid_amount) THEN
        RAISE_APPLICATION_ERROR(-20023, 'Payment exceeds the outstanding balance.');
    END IF;

    p_payment_id := payment_seq.NEXTVAL;

    INSERT INTO payments (
        payment_id, payment_no, invoice_id, payment_date,
        amount, payment_method, reference_no, status
    ) VALUES (
        p_payment_id, p_payment_no, p_invoice_id, SYSDATE,
        p_amount, UPPER(TRIM(p_payment_method)), p_reference_no, 1
    );
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RAISE_APPLICATION_ERROR(-20024, 'Supplier invoice was not found.');
END;
/

CREATE OR REPLACE PROCEDURE sp_soft_delete (
    p_table_name IN VARCHAR2,
    p_id         IN NUMBER
) AS
    v_table_name VARCHAR2(128);
    v_sql        VARCHAR2(1000);
BEGIN
    v_table_name := UPPER(TRIM(p_table_name));

    CASE v_table_name
        WHEN 'EMPLOYEES' THEN
            v_sql := 'UPDATE employees SET status = 0 WHERE employee_id = :id';
        WHEN 'SUPPLIERS' THEN
            v_sql := 'UPDATE suppliers SET status = 0 WHERE supplier_id = :id';
        WHEN 'PRODUCTS' THEN
            v_sql := 'UPDATE products SET status = 0 WHERE product_id = :id';
        WHEN 'PURCHASE_REQUEST_ITEMS' THEN
            v_sql := 'UPDATE purchase_request_items SET status = 0 WHERE request_item_id = :id';
        WHEN 'PURCHASE_REQUEST_APPROVALS' THEN
            v_sql := 'UPDATE purchase_request_approvals SET status = 0 WHERE approval_id = :id';
        WHEN 'QUOTATION_ITEMS' THEN
            v_sql := 'UPDATE quotation_items SET status = 0 WHERE quotation_item_id = :id';
        WHEN 'PURCHASE_ORDER_ITEMS' THEN
            v_sql := 'UPDATE purchase_order_items SET status = 0 WHERE po_item_id = :id';
        WHEN 'GOODS_RECEIPT_ITEMS' THEN
            v_sql := 'UPDATE goods_receipt_items SET status = 0 WHERE receipt_item_id = :id';
        WHEN 'PAYMENTS' THEN
            v_sql := 'UPDATE payments SET status = 0 WHERE payment_id = :id';
        ELSE
            RAISE_APPLICATION_ERROR(
                -20090,
                'Soft delete is not allowed for table ' || v_table_name ||
                '. Use the workflow cancel/reject status for header documents.'
            );
    END CASE;

    EXECUTE IMMEDIATE v_sql USING p_id;

    IF SQL%ROWCOUNT = 0 THEN
        RAISE_APPLICATION_ERROR(-20091, 'No row was found to soft delete.');
    END IF;
END;
/
