-- Purchasing Management System - Core Schema
-- Run as PURCHASING_USER in a fresh schema.

CREATE TABLE employees (
    employee_id      NUMBER PRIMARY KEY,
    employee_code    VARCHAR2(20)  NOT NULL,
    full_name        VARCHAR2(100) NOT NULL,
    email            VARCHAR2(100),
    department       VARCHAR2(100),
    job_title        VARCHAR2(100),
    status           NUMBER(1) DEFAULT 1 NOT NULL, -- 0: INACTIVE, 1: ACTIVE
    created_at       DATE DEFAULT SYSDATE NOT NULL,
    CONSTRAINT uk_employee_code UNIQUE (employee_code),
    CONSTRAINT uk_employee_email UNIQUE (email),
    CONSTRAINT ck_employee_status CHECK (status IN (0, 1))
);

CREATE TABLE suppliers (
    supplier_id      NUMBER PRIMARY KEY,
    supplier_code    VARCHAR2(20)  NOT NULL,
    supplier_name    VARCHAR2(100) NOT NULL,
    contact_person   VARCHAR2(100),
    phone            VARCHAR2(20),
    email            VARCHAR2(100),
    address          VARCHAR2(255),
    tax_number       VARCHAR2(50),
    status           NUMBER(1) DEFAULT 1 NOT NULL, -- 0: INACTIVE, 1: ACTIVE
    created_at       DATE DEFAULT SYSDATE NOT NULL,
    CONSTRAINT uk_supplier_code UNIQUE (supplier_code),
    CONSTRAINT ck_supplier_status CHECK (status IN (0, 1))
);

CREATE TABLE products (
    product_id             NUMBER PRIMARY KEY,
    product_code           VARCHAR2(20)  NOT NULL,
    product_name           VARCHAR2(100) NOT NULL,
    category               VARCHAR2(50),
    unit                   VARCHAR2(20) DEFAULT 'UNIT' NOT NULL,
    unit_price             NUMBER(12,2) DEFAULT 0 NOT NULL,
    stock_qty              NUMBER(12,2) DEFAULT 0 NOT NULL,
    reorder_level          NUMBER(12,2) DEFAULT 0 NOT NULL,
    preferred_supplier_id  NUMBER,
    status                 NUMBER(1) DEFAULT 1 NOT NULL, -- 0: INACTIVE, 1: ACTIVE
    created_at             DATE DEFAULT SYSDATE NOT NULL,
    CONSTRAINT uk_product_code UNIQUE (product_code),
    CONSTRAINT fk_product_supplier FOREIGN KEY (preferred_supplier_id)
        REFERENCES suppliers (supplier_id),
    CONSTRAINT ck_product_price CHECK (unit_price >= 0),
    CONSTRAINT ck_product_stock CHECK (stock_qty >= 0),
    CONSTRAINT ck_product_reorder CHECK (reorder_level >= 0),
    CONSTRAINT ck_product_status CHECK (status IN (0, 1))
);

CREATE TABLE purchase_requests (
    request_id       NUMBER PRIMARY KEY,
    request_no       VARCHAR2(30)  NOT NULL,
    request_date     DATE DEFAULT SYSDATE NOT NULL,
    requested_by     NUMBER NOT NULL,
    needed_date      DATE,
    purpose          VARCHAR2(500),
    status           NUMBER(1) DEFAULT 0 NOT NULL, -- 0: DRAFT, 1: PENDING, 2: APPROVED, 3: REJECTED, 4: CANCELLED
    created_at       DATE DEFAULT SYSDATE NOT NULL,
    CONSTRAINT uk_request_no UNIQUE (request_no),
    CONSTRAINT fk_request_employee FOREIGN KEY (requested_by)
        REFERENCES employees (employee_id),
    CONSTRAINT ck_request_dates CHECK (needed_date IS NULL OR needed_date >= request_date),
    CONSTRAINT ck_request_status CHECK (
        status IN (0, 1, 2, 3, 4)
    )
);

CREATE TABLE purchase_request_items (
    request_item_id       NUMBER PRIMARY KEY,
    request_id            NUMBER NOT NULL,
    product_id            NUMBER NOT NULL,
    quantity              NUMBER(12,2) NOT NULL,
    estimated_unit_price  NUMBER(12,2) DEFAULT 0 NOT NULL,
    status                NUMBER(1) DEFAULT 1 NOT NULL, -- 0: INACTIVE, 1: ACTIVE
    notes                 VARCHAR2(255),
    CONSTRAINT fk_pr_item_request FOREIGN KEY (request_id)
        REFERENCES purchase_requests (request_id),
    CONSTRAINT fk_pr_item_product FOREIGN KEY (product_id)
        REFERENCES products (product_id),
    CONSTRAINT uk_pr_item_product UNIQUE (request_id, product_id),
    CONSTRAINT ck_pr_item_qty CHECK (quantity > 0),
    CONSTRAINT ck_pr_item_price CHECK (estimated_unit_price >= 0),
    CONSTRAINT ck_pr_item_status CHECK (status IN (0, 1))
);

CREATE TABLE purchase_request_approvals (
    approval_id      NUMBER PRIMARY KEY,
    request_id       NUMBER NOT NULL,
    approval_level   NUMBER(3) DEFAULT 1 NOT NULL,
    approver_id      NUMBER NOT NULL,
    decision         NUMBER(1) DEFAULT 0 NOT NULL, -- 0: PENDING, 1: APPROVED, 2: REJECTED
    status           NUMBER(1) DEFAULT 1 NOT NULL, -- 0: INACTIVE, 1: ACTIVE
    comments         VARCHAR2(500),
    decision_date    DATE,
    created_at       DATE DEFAULT SYSDATE NOT NULL,
    CONSTRAINT fk_approval_request FOREIGN KEY (request_id)
        REFERENCES purchase_requests (request_id),
    CONSTRAINT fk_approval_employee FOREIGN KEY (approver_id)
        REFERENCES employees (employee_id),
    CONSTRAINT uk_request_approval_level UNIQUE (request_id, approval_level),
    CONSTRAINT ck_approval_level CHECK (approval_level > 0),
    CONSTRAINT ck_approval_decision CHECK (
        decision IN (0, 1, 2)
    ),
    CONSTRAINT ck_approval_status CHECK (status IN (0, 1)),
    CONSTRAINT ck_approval_date CHECK (
        (decision = 0 AND decision_date IS NULL)
        OR (decision IN (1, 2) AND decision_date IS NOT NULL)
    )
);

CREATE TABLE quotations (
    quotation_id      NUMBER PRIMARY KEY,
    quotation_no      VARCHAR2(30) NOT NULL,
    request_id        NUMBER NOT NULL,
    supplier_id       NUMBER NOT NULL,
    quotation_date    DATE DEFAULT SYSDATE NOT NULL,
    valid_until       DATE,
    status            NUMBER(1) DEFAULT 0 NOT NULL, -- 0: RECEIVED, 1: SELECTED, 2: REJECTED, 3: EXPIRED
    total_amount      NUMBER(14,2) DEFAULT 0 NOT NULL,
    notes             VARCHAR2(500),
    created_at        DATE DEFAULT SYSDATE NOT NULL,
    CONSTRAINT uk_quotation_no UNIQUE (quotation_no),
    CONSTRAINT uk_request_supplier_quote UNIQUE (request_id, supplier_id),
    CONSTRAINT fk_quotation_request FOREIGN KEY (request_id)
        REFERENCES purchase_requests (request_id),
    CONSTRAINT fk_quotation_supplier FOREIGN KEY (supplier_id)
        REFERENCES suppliers (supplier_id),
    CONSTRAINT ck_quotation_dates CHECK (
        valid_until IS NULL OR valid_until >= quotation_date
    ),
    CONSTRAINT ck_quotation_total CHECK (total_amount >= 0),
    CONSTRAINT ck_quotation_status CHECK (
        status IN (0, 1, 2, 3)
    )
);

CREATE TABLE quotation_items (
    quotation_item_id  NUMBER PRIMARY KEY,
    quotation_id       NUMBER NOT NULL,
    product_id         NUMBER NOT NULL,
    quantity           NUMBER(12,2) NOT NULL,
    unit_price         NUMBER(12,2) NOT NULL,
    status             NUMBER(1) DEFAULT 1 NOT NULL, -- 0: INACTIVE, 1: ACTIVE
    subtotal           NUMBER(14,2)
        GENERATED ALWAYS AS (quantity * unit_price) VIRTUAL,
    CONSTRAINT fk_quote_item_quote FOREIGN KEY (quotation_id)
        REFERENCES quotations (quotation_id),
    CONSTRAINT fk_quote_item_product FOREIGN KEY (product_id)
        REFERENCES products (product_id),
    CONSTRAINT uk_quote_item_product UNIQUE (quotation_id, product_id),
    CONSTRAINT ck_quote_item_qty CHECK (quantity > 0),
    CONSTRAINT ck_quote_item_price CHECK (unit_price >= 0),
    CONSTRAINT ck_quote_item_status CHECK (status IN (0, 1))
);

CREATE TABLE purchase_orders (
    po_id                   NUMBER PRIMARY KEY,
    po_no                   VARCHAR2(30) NOT NULL,
    request_id              NUMBER NOT NULL,
    quotation_id            NUMBER,
    supplier_id             NUMBER NOT NULL,
    po_date                 DATE DEFAULT SYSDATE NOT NULL,
    expected_delivery_date  DATE,
    created_by              NUMBER NOT NULL,
    approved_by             NUMBER,
    status                  NUMBER(1) DEFAULT 0 NOT NULL, -- 0: DRAFT, 1: APPROVED, 2: PARTIALLY_RECEIVED, 3: RECEIVED, 4: CANCELLED, 5: CLOSED
    subtotal_amount         NUMBER(14,2) DEFAULT 0 NOT NULL,
    tax_amount              NUMBER(14,2) DEFAULT 0 NOT NULL,
    total_amount            NUMBER(14,2) DEFAULT 0 NOT NULL,
    notes                   VARCHAR2(500),
    created_at              DATE DEFAULT SYSDATE NOT NULL,
    CONSTRAINT uk_po_no UNIQUE (po_no),
    CONSTRAINT uk_po_quotation UNIQUE (quotation_id),
    CONSTRAINT fk_po_request FOREIGN KEY (request_id)
        REFERENCES purchase_requests (request_id),
    CONSTRAINT fk_po_quotation FOREIGN KEY (quotation_id)
        REFERENCES quotations (quotation_id),
    CONSTRAINT fk_po_supplier FOREIGN KEY (supplier_id)
        REFERENCES suppliers (supplier_id),
    CONSTRAINT fk_po_created_by FOREIGN KEY (created_by)
        REFERENCES employees (employee_id),
    CONSTRAINT fk_po_approved_by FOREIGN KEY (approved_by)
        REFERENCES employees (employee_id),
    CONSTRAINT ck_po_delivery_date CHECK (
        expected_delivery_date IS NULL OR expected_delivery_date >= po_date
    ),
    CONSTRAINT ck_po_amounts CHECK (
        subtotal_amount >= 0 AND tax_amount >= 0 AND total_amount >= 0
    ),
    CONSTRAINT ck_po_status CHECK (
        status IN (
            0, 1, 2, 3, 4, 5
        )
    )
);

CREATE TABLE purchase_order_items (
    po_item_id       NUMBER PRIMARY KEY,
    po_id            NUMBER NOT NULL,
    product_id       NUMBER NOT NULL,
    quantity         NUMBER(12,2) NOT NULL,
    unit_price       NUMBER(12,2) NOT NULL,
    status           NUMBER(1) DEFAULT 1 NOT NULL, -- 0: INACTIVE, 1: ACTIVE
    subtotal         NUMBER(14,2)
        GENERATED ALWAYS AS (quantity * unit_price) VIRTUAL,
    CONSTRAINT fk_po_item_po FOREIGN KEY (po_id)
        REFERENCES purchase_orders (po_id),
    CONSTRAINT fk_po_item_product FOREIGN KEY (product_id)
        REFERENCES products (product_id),
    CONSTRAINT uk_po_item_product UNIQUE (po_id, product_id),
    CONSTRAINT ck_po_item_qty CHECK (quantity > 0),
    CONSTRAINT ck_po_item_price CHECK (unit_price >= 0),
    CONSTRAINT ck_po_item_status CHECK (status IN (0, 1))
);

CREATE TABLE goods_receipts (
    receipt_id       NUMBER PRIMARY KEY,
    receipt_no       VARCHAR2(30) NOT NULL,
    po_id            NUMBER NOT NULL,
    receipt_date     DATE DEFAULT SYSDATE NOT NULL,
    received_by      NUMBER NOT NULL,
    status           NUMBER(1) DEFAULT 0 NOT NULL, -- 0: DRAFT, 1: RECEIVED, 2: CANCELLED
    notes            VARCHAR2(500),
    created_at       DATE DEFAULT SYSDATE NOT NULL,
    CONSTRAINT uk_receipt_no UNIQUE (receipt_no),
    CONSTRAINT fk_receipt_po FOREIGN KEY (po_id)
        REFERENCES purchase_orders (po_id),
    CONSTRAINT fk_receipt_employee FOREIGN KEY (received_by)
        REFERENCES employees (employee_id),
    CONSTRAINT ck_receipt_status CHECK (
        status IN (0, 1, 2)
    )
);

CREATE TABLE goods_receipt_items (
    receipt_item_id  NUMBER PRIMARY KEY,
    receipt_id       NUMBER NOT NULL,
    product_id       NUMBER NOT NULL,
    received_qty     NUMBER(12,2) NOT NULL,
    rejected_qty     NUMBER(12,2) DEFAULT 0 NOT NULL,
    status           NUMBER(1) DEFAULT 1 NOT NULL, -- 0: INACTIVE, 1: ACTIVE
    notes            VARCHAR2(255),
    CONSTRAINT fk_receipt_item_receipt FOREIGN KEY (receipt_id)
        REFERENCES goods_receipts (receipt_id),
    CONSTRAINT fk_receipt_item_product FOREIGN KEY (product_id)
        REFERENCES products (product_id),
    CONSTRAINT uk_receipt_item_product UNIQUE (receipt_id, product_id),
    CONSTRAINT ck_receipt_item_qty CHECK (received_qty > 0),
    CONSTRAINT ck_receipt_item_status CHECK (status IN (0, 1)),
    CONSTRAINT ck_receipt_rejected_qty CHECK (
        rejected_qty >= 0 AND rejected_qty <= received_qty
    )
);

CREATE TABLE supplier_invoices (
    invoice_id       NUMBER PRIMARY KEY,
    invoice_no       VARCHAR2(50) NOT NULL,
    po_id            NUMBER NOT NULL,
    invoice_date     DATE DEFAULT SYSDATE NOT NULL,
    due_date         DATE NOT NULL,
    subtotal_amount  NUMBER(14,2) NOT NULL,
    tax_amount       NUMBER(14,2) DEFAULT 0 NOT NULL,
    total_amount     NUMBER(14,2) NOT NULL,
    paid_amount      NUMBER(14,2) DEFAULT 0 NOT NULL,
    status           NUMBER(1) DEFAULT 0 NOT NULL, -- 0: UNPAID, 1: PARTIAL, 2: PAID, 3: CANCELLED
    notes            VARCHAR2(500),
    created_at       DATE DEFAULT SYSDATE NOT NULL,
    CONSTRAINT uk_invoice_no UNIQUE (invoice_no),
    CONSTRAINT fk_invoice_po FOREIGN KEY (po_id)
        REFERENCES purchase_orders (po_id),
    CONSTRAINT ck_invoice_dates CHECK (due_date >= invoice_date),
    CONSTRAINT ck_invoice_amounts CHECK (
        subtotal_amount >= 0
        AND tax_amount >= 0
        AND total_amount = subtotal_amount + tax_amount
        AND paid_amount >= 0
        AND paid_amount <= total_amount
    ),
    CONSTRAINT ck_invoice_status CHECK (
        status IN (0, 1, 2, 3)
    )
);

CREATE TABLE payments (
    payment_id       NUMBER PRIMARY KEY,
    payment_no       VARCHAR2(30) NOT NULL,
    invoice_id       NUMBER NOT NULL,
    payment_date     DATE DEFAULT SYSDATE NOT NULL,
    amount           NUMBER(14,2) NOT NULL,
    payment_method   VARCHAR2(30) NOT NULL,
    reference_no     VARCHAR2(100),
    status           NUMBER(1) DEFAULT 1 NOT NULL, -- 0: PENDING, 1: POSTED, 2: VOID
    notes            VARCHAR2(500),
    created_at       DATE DEFAULT SYSDATE NOT NULL,
    CONSTRAINT uk_payment_no UNIQUE (payment_no),
    CONSTRAINT fk_payment_invoice FOREIGN KEY (invoice_id)
        REFERENCES supplier_invoices (invoice_id),
    CONSTRAINT ck_payment_amount CHECK (amount > 0),
    CONSTRAINT ck_payment_method CHECK (
        payment_method IN ('CASH', 'BANK_TRANSFER', 'CHEQUE', 'CARD')
    ),
    CONSTRAINT ck_payment_status CHECK (
        status IN (0, 1, 2)
    )
);

-- Sequences demonstrate Oracle's sequence-based key generation.
CREATE SEQUENCE employee_seq       START WITH 1 INCREMENT BY 1 NOCYCLE;
CREATE SEQUENCE supplier_seq       START WITH 1 INCREMENT BY 1 NOCYCLE;
CREATE SEQUENCE product_seq        START WITH 1 INCREMENT BY 1 NOCYCLE;
CREATE SEQUENCE request_seq        START WITH 1 INCREMENT BY 1 NOCYCLE;
CREATE SEQUENCE request_item_seq   START WITH 1 INCREMENT BY 1 NOCYCLE;
CREATE SEQUENCE approval_seq       START WITH 1 INCREMENT BY 1 NOCYCLE;
CREATE SEQUENCE quotation_seq      START WITH 1 INCREMENT BY 1 NOCYCLE;
CREATE SEQUENCE quotation_item_seq START WITH 1 INCREMENT BY 1 NOCYCLE;
CREATE SEQUENCE po_seq             START WITH 1 INCREMENT BY 1 NOCYCLE;
CREATE SEQUENCE po_item_seq        START WITH 1 INCREMENT BY 1 NOCYCLE;
CREATE SEQUENCE receipt_seq        START WITH 1 INCREMENT BY 1 NOCYCLE;
CREATE SEQUENCE receipt_item_seq   START WITH 1 INCREMENT BY 1 NOCYCLE;
CREATE SEQUENCE invoice_seq        START WITH 1 INCREMENT BY 1 NOCYCLE;
CREATE SEQUENCE payment_seq        START WITH 1 INCREMENT BY 1 NOCYCLE;

-- Oracle does not automatically index foreign keys. These indexes improve joins
-- and reduce locking problems when a parent row is updated or deleted.
CREATE INDEX ix_product_supplier       ON products (preferred_supplier_id);
CREATE INDEX ix_request_employee       ON purchase_requests (requested_by);
CREATE INDEX ix_pr_item_product        ON purchase_request_items (product_id);
CREATE INDEX ix_pr_item_status         ON purchase_request_items (status);
CREATE INDEX ix_approval_approver      ON purchase_request_approvals (approver_id);
CREATE INDEX ix_approval_status        ON purchase_request_approvals (status);
CREATE INDEX ix_quotation_supplier     ON quotations (supplier_id);
CREATE UNIQUE INDEX uk_selected_quote_per_request
    ON quotations (
        CASE WHEN status = 1 THEN request_id ELSE NULL END
    );
CREATE INDEX ix_quote_item_product     ON quotation_items (product_id);
CREATE INDEX ix_quote_item_status      ON quotation_items (status);
CREATE INDEX ix_po_request             ON purchase_orders (request_id);
CREATE INDEX ix_po_supplier            ON purchase_orders (supplier_id);
CREATE INDEX ix_po_item_product        ON purchase_order_items (product_id);
CREATE INDEX ix_po_item_status         ON purchase_order_items (status);
CREATE INDEX ix_receipt_po             ON goods_receipts (po_id);
CREATE INDEX ix_receipt_item_product   ON goods_receipt_items (product_id);
CREATE INDEX ix_receipt_item_status    ON goods_receipt_items (status);
CREATE INDEX ix_invoice_po             ON supplier_invoices (po_id);
CREATE INDEX ix_payment_invoice        ON payments (invoice_id);

-- Numeric status code dictionary (stored as NUMBER for compact, indexed values).
COMMENT ON COLUMN employees.status IS '0: INACTIVE, 1: ACTIVE';
COMMENT ON COLUMN suppliers.status IS '0: INACTIVE, 1: ACTIVE';
COMMENT ON COLUMN products.status IS '0: INACTIVE, 1: ACTIVE';
COMMENT ON COLUMN purchase_requests.status IS '0: DRAFT, 1: PENDING, 2: APPROVED, 3: REJECTED, 4: CANCELLED';
COMMENT ON COLUMN purchase_request_items.status IS '0: INACTIVE/SOFT DELETED, 1: ACTIVE';
COMMENT ON COLUMN purchase_request_approvals.decision IS '0: PENDING, 1: APPROVED, 2: REJECTED';
COMMENT ON COLUMN purchase_request_approvals.status IS '0: INACTIVE/SOFT DELETED, 1: ACTIVE';
COMMENT ON COLUMN quotations.status IS '0: RECEIVED, 1: SELECTED, 2: REJECTED, 3: EXPIRED';
COMMENT ON COLUMN quotation_items.status IS '0: INACTIVE/SOFT DELETED, 1: ACTIVE';
COMMENT ON COLUMN purchase_orders.status IS '0: DRAFT, 1: APPROVED, 2: PARTIALLY_RECEIVED, 3: RECEIVED, 4: CANCELLED, 5: CLOSED';
COMMENT ON COLUMN purchase_order_items.status IS '0: INACTIVE/SOFT DELETED, 1: ACTIVE';
COMMENT ON COLUMN goods_receipts.status IS '0: DRAFT, 1: RECEIVED, 2: CANCELLED';
COMMENT ON COLUMN goods_receipt_items.status IS '0: INACTIVE/SOFT DELETED, 1: ACTIVE';
COMMENT ON COLUMN supplier_invoices.status IS '0: UNPAID, 1: PARTIAL, 2: PAID, 3: CANCELLED';
COMMENT ON COLUMN payments.status IS '0: PENDING, 1: POSTED, 2: VOID';
