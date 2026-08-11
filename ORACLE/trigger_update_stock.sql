-- Stock changes use accepted quantity: received_qty - rejected_qty.
-- Draft receipt items do not affect stock.

CREATE OR REPLACE TRIGGER trg_receipt_status_stock
AFTER UPDATE OF status ON goods_receipts
FOR EACH ROW
DECLARE
    v_multiplier NUMBER;
BEGIN
    IF :OLD.status <> 'RECEIVED' AND :NEW.status = 'RECEIVED' THEN
        v_multiplier := 1;
    ELSIF :OLD.status = 'RECEIVED' AND :NEW.status <> 'RECEIVED' THEN
        v_multiplier := -1;
    ELSE
        RETURN;
    END IF;

    FOR item_row IN (
        SELECT product_id,
               SUM(received_qty - rejected_qty) AS accepted_qty
          FROM goods_receipt_items
         WHERE receipt_id = :NEW.receipt_id
         GROUP BY product_id
    ) LOOP
        UPDATE products
           SET stock_qty = stock_qty + (item_row.accepted_qty * v_multiplier)
         WHERE product_id = item_row.product_id;
    END LOOP;
END;
/

CREATE OR REPLACE TRIGGER trg_receipt_item_stock
AFTER INSERT OR UPDATE OR DELETE ON goods_receipt_items
FOR EACH ROW
DECLARE
    v_status goods_receipts.status%TYPE;

    PROCEDURE adjust_stock (
        p_product_id products.product_id%TYPE,
        p_delta      NUMBER
    ) IS
    BEGIN
        UPDATE products
           SET stock_qty = stock_qty + p_delta
         WHERE product_id = p_product_id;
    END adjust_stock;
BEGIN
    IF INSERTING THEN
        SELECT status
          INTO v_status
          FROM goods_receipts
         WHERE receipt_id = :NEW.receipt_id;

        IF v_status = 'RECEIVED' THEN
            adjust_stock(
                :NEW.product_id,
                :NEW.received_qty - :NEW.rejected_qty
            );
        END IF;
    ELSIF DELETING THEN
        SELECT status
          INTO v_status
          FROM goods_receipts
         WHERE receipt_id = :OLD.receipt_id;

        IF v_status = 'RECEIVED' THEN
            adjust_stock(
                :OLD.product_id,
                -(:OLD.received_qty - :OLD.rejected_qty)
            );
        END IF;
    ELSE
        SELECT status
          INTO v_status
          FROM goods_receipts
         WHERE receipt_id = :OLD.receipt_id;

        IF v_status = 'RECEIVED' THEN
            adjust_stock(
                :OLD.product_id,
                -(:OLD.received_qty - :OLD.rejected_qty)
            );
        END IF;

        SELECT status
          INTO v_status
          FROM goods_receipts
         WHERE receipt_id = :NEW.receipt_id;

        IF v_status = 'RECEIVED' THEN
            adjust_stock(
                :NEW.product_id,
                :NEW.received_qty - :NEW.rejected_qty
            );
        END IF;
    END IF;
END;
/
