CREATE EXTENSION IF NOT EXISTS "pgcrypto";

-- ============================================================
-- Users Table
-- ============================================================

CREATE TABLE users (
    id UUID PRIMARY KEY,
    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    role VARCHAR(50) NOT NULL DEFAULT 'Customer'
);
-- ============================================================
-- Categories Table
-- ============================================================
create table catgeories(
id UUID primary key default gen_random_uuid(),
name varchar(255) not null,
description VARCHAR(500),
parent_id UUID NULL,
is_deleted BOOLEAN NOT NULL DEFAULT FALSE,
created_at timestamp default now(),
updated_at timestamp default now(),
constraint FK_Parent_Id foreign key (parent_id) references catgeories(id) ON DELETE SET NULL
)

-- ============================================================
-- PRODUCTS
-- ============================================================

Create Table products(
id UUID primary key default gen_random_uuid(),
name varchar(255) not null,
description VARCHAR(500),
price decimal(10,2) not null,
stock_quantity INTEGER NOT NULL DEFAULT 0,
category_id UUID not null,
is_deleted BOOLEAN NOT NULL DEFAULT FALSE,
created_at timestamp default now(),
updated_at timestamp default now(),
--constraint check Check_Price(price >= 0)
--constraint check Check_Stock_Quantity(stock_quantity >= 0)
constraint FK_Category_Id foreign key (category_id) references catgeories(id) ON DELETE RESTRICT
);

-- ============================================================
-- ORDERS
-- ============================================================

CREATE TABLE orders(
id UUID primary key default gen_random_uuid(),
user_id UUID not null,
shipping_address VARCHAR(500) NOT NULL,
status varchar(50) not null default 'pending',
created_at timestamp default now()
)

-- ============================================================
-- orderitems
-- ============================================================
create table orderitems(
id UUID primary key default gen_random_uuid(),
order_id UUID not null,
product_id UUID not null,
quantity INTEGER not null default 1,
--check constraint Check_Quantity(quantity >= 1),
unit_price DECIMAL(10, 2) NOT NULL CHECK (unit_price >= 0)
)


select * from information_schema.tables 