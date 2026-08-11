CREATE OR REPLACE VIEW vw_stock_report AS
SELECT
    p.product_code,
    p.product_name,
    p.category,
    p.unit,
    p.stock_qty,
    p.reorder_level,
    p.unit_price,
    p.stock_qty * p.unit_price AS inventory_value,
    s.supplier_name AS preferred_supplier,
    CASE
        WHEN p.stock_qty = 0 THEN 'OUT OF STOCK'
        WHEN p.stock_qty <= p.reorder_level THEN 'LOW STOCK'
        ELSE 'IN STOCK'
    END AS stock_status
FROM products p
LEFT JOIN suppliers s
  ON s.supplier_id = p.preferred_supplier_id
WHERE p.status = 1;
