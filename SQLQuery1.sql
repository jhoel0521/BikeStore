use  Bike_Store ;

CREATE TABLE Customers (
    customer_id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    first_name VARCHAR(255) NOT NULL,
    last_name VARCHAR(255) NOT NULL,
    phone VARCHAR(25) NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    street VARCHAR(255) NULL,
    city VARCHAR(25) NULL,
    state VARCHAR(25) NULL,
    zip_code VARCHAR(5) NULL
);

CREATE TABLE Orders (
    order_id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    customer_id INT NOT NULL,
    order_date DATE NOT NULL,
    FOREIGN KEY (customer_id) REFERENCES Customers(customer_id)
);

CREATE TABLE Products (
    product_id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    product_name VARCHAR(200) NOT NULL,
    model_year INT NOT NULL,
    price DECIMAL(10,2)
);

CREATE TABLE Order_items (
    order_item_id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    order_id INT NOT NULL,
    product_id INT NOT NULL,
    quantity INT NOT NULL,
    price DECIMAL(10,2) NOT NULL,
    discount DECIMAL(4,2) NOT NULL,
    FOREIGN KEY (order_id) REFERENCES Orders(order_id),
    FOREIGN KEY (product_id) REFERENCES Products(product_id)
);
