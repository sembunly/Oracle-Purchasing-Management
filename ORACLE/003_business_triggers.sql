-- Derived totals and payment status automation.

CREATE OR REPLACE TRIGGER trg_quotation_total
AFTER INSERT OR UPDATE OR DELETE ON quotation_items
BEGIN
    UPDATE quotations q
       SET total_amount = (
           SELECT NVL(SUM(qi.subtotal), 0)
             FROM quotation_items qi
            WHERE qi.quotation_id = q.quotation_id
       );
END;
/

CREATE OR REPLACE TRIGGER trg_po_total
AFTER INSERT OR UPDATE OR DELETE ON purchase_order_items
BEGIN
    UPDATE purchase_orders po
       SET subtotal_amount = (
               SELECT NVL(SUM(poi.subtotal), 0)
                 FROM purchase_order_items poi
                WHERE poi.po_id = po.po_id
           ),
           total_amount = (
               SELECT NVL(SUM(poi.subtotal), 0)
                 FROM purchase_order_items poi
                WHERE poi.po_id = po.po_id
           ) + po.tax_amount;
END;
/

CREATE OR REPLACE TRIGGER trg_invoice_payment_status
AFTER INSERT OR UPDATE OR DELETE ON payments
BEGIN
    UPDATE supplier_invoices i
       SET paid_amount = (
               SELECT NVL(SUM(p.amount), 0)
                 FROM payments p
                WHERE p.invoice_id = i.invoice_id
                  AND p.status = 1
           ),
           status = CASE
               WHEN (
                   SELECT NVL(SUM(p.amount), 0)
                     FROM payments p
                    WHERE p.invoice_id = i.invoice_id
                      AND p.status = 1
               ) = 0 THEN 0
               WHEN (
                   SELECT NVL(SUM(p.amount), 0)
                     FROM payments p
                    WHERE p.invoice_id = i.invoice_id
                      AND p.status = 1
               ) >= i.total_amount THEN 2
               ELSE 1
           END
     WHERE i.status <> 3;
END;
/
