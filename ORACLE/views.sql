CREATE OR REPLACE VIEW vw_purchase_report AS
SELECT
    po.po_id,
    po.po_no,
    pr.request_no,
    q.quotation_no,
    s.supplier_code,
    s.supplier_name,
    requester.full_name AS requested_by,
    po.po_date,
    po.expected_delivery_date,
    po.status,
    NVL(items.item_lines, 0) AS item_lines,
    NVL(items.ordered_qty, 0) AS ordered_qty,
    NVL(receipts.accepted_qty, 0) AS accepted_qty,
    po.subtotal_amount,
    po.tax_amount,
    po.total_amount,
    NVL(invoices.invoiced_amount, 0) AS invoiced_amount,
    NVL(invoices.paid_amount, 0) AS paid_amount
FROM purchase_orders po
JOIN purchase_requests pr
  ON pr.request_id = po.request_id
JOIN employees requester
  ON requester.employee_id = pr.requested_by
JOIN suppliers s
  ON s.supplier_id = po.supplier_id
LEFT JOIN quotations q
  ON q.quotation_id = po.quotation_id
LEFT JOIN (
    SELECT po_id,
           COUNT(*) AS item_lines,
           SUM(quantity) AS ordered_qty
      FROM purchase_order_items
     GROUP BY po_id
) items
  ON items.po_id = po.po_id
LEFT JOIN (
    SELECT gr.po_id,
           SUM(gri.received_qty - gri.rejected_qty) AS accepted_qty
      FROM goods_receipts gr
      JOIN goods_receipt_items gri
        ON gri.receipt_id = gr.receipt_id
     WHERE gr.status = 'RECEIVED'
     GROUP BY gr.po_id
) receipts
  ON receipts.po_id = po.po_id
LEFT JOIN (
    SELECT po_id,
           SUM(CASE WHEN status <> 'CANCELLED' THEN total_amount ELSE 0 END)
               AS invoiced_amount,
           SUM(CASE WHEN status <> 'CANCELLED' THEN paid_amount ELSE 0 END)
               AS paid_amount
      FROM supplier_invoices
     GROUP BY po_id
) invoices
  ON invoices.po_id = po.po_id;

CREATE OR REPLACE VIEW vw_pending_approvals AS
SELECT
    a.approval_id,
    pr.request_no,
    pr.request_date,
    pr.needed_date,
    requester.full_name AS requested_by,
    approver.full_name AS approver_name,
    a.approval_level,
    a.decision,
    pr.purpose
FROM purchase_request_approvals a
JOIN purchase_requests pr
  ON pr.request_id = a.request_id
JOIN employees requester
  ON requester.employee_id = pr.requested_by
JOIN employees approver
  ON approver.employee_id = a.approver_id
WHERE a.decision = 'PENDING';

CREATE OR REPLACE VIEW vw_receiving_report AS
SELECT
    po.po_no,
    p.product_code,
    p.product_name,
    poi.quantity AS ordered_qty,
    NVL(r.accepted_qty, 0) AS accepted_qty,
    poi.quantity - NVL(r.accepted_qty, 0) AS remaining_qty,
    CASE
        WHEN NVL(r.accepted_qty, 0) = 0 THEN 'NOT RECEIVED'
        WHEN NVL(r.accepted_qty, 0) < poi.quantity THEN 'PARTIAL'
        ELSE 'COMPLETE'
    END AS receiving_status
FROM purchase_order_items poi
JOIN purchase_orders po
  ON po.po_id = poi.po_id
JOIN products p
  ON p.product_id = poi.product_id
LEFT JOIN (
    SELECT gr.po_id,
           gri.product_id,
           SUM(gri.received_qty - gri.rejected_qty) AS accepted_qty
      FROM goods_receipts gr
      JOIN goods_receipt_items gri
        ON gri.receipt_id = gr.receipt_id
     WHERE gr.status = 'RECEIVED'
     GROUP BY gr.po_id, gri.product_id
) r
  ON r.po_id = poi.po_id
 AND r.product_id = poi.product_id;

CREATE OR REPLACE VIEW vw_invoice_payment_report AS
SELECT
    i.invoice_no,
    po.po_no,
    s.supplier_name,
    i.invoice_date,
    i.due_date,
    i.total_amount,
    i.paid_amount,
    i.total_amount - i.paid_amount AS balance_amount,
    i.status,
    CASE
        WHEN i.status IN ('PAID', 'CANCELLED') THEN 0
        ELSE GREATEST(0, TRUNC(SYSDATE) - TRUNC(i.due_date))
    END AS days_overdue
FROM supplier_invoices i
JOIN purchase_orders po
  ON po.po_id = i.po_id
JOIN suppliers s
  ON s.supplier_id = po.supplier_id;

CREATE OR REPLACE VIEW vw_supplier_performance AS
SELECT
    s.supplier_code,
    s.supplier_name,
    COUNT(DISTINCT po.po_id) AS total_orders,
    NVL(SUM(po.total_amount), 0) AS total_order_amount,
    COUNT(DISTINCT CASE WHEN po.status IN ('RECEIVED', 'CLOSED') THEN po.po_id END)
        AS completed_orders,
    MAX(po.po_date) AS last_order_date
FROM suppliers s
LEFT JOIN purchase_orders po
  ON po.supplier_id = s.supplier_id
 AND po.status <> 'CANCELLED'
GROUP BY s.supplier_code, s.supplier_name;
