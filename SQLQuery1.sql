use  Bike_Store ;
create table Customers (
 customer_id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
 fisrt_name varchar(255) NOT NULL,
 last_name varchar(255) NOT NULL,
 phone varchar(25) null,
 email varchar(255) NOT NULL,
 street varchar(255) null,
 city varchar(25) null,
 state varchar(25) null,
 zip_code varchar(5) null
);
create table Orders(  
 order_id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
 customer_id Int NOT NULL,
 order_date date NOT NULL,
 foreign key (customer_id)  references Customers ( customer_id )
);

create table Products( 
	product_id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	product_name varchar(200) NOT NULL,
	model_year Int NOT NULL,
	price decimal(10,2)
);

create table Order_items(
 order_item_id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
 order_id Int NOT NULL,
 product_id Int NOT NULL,
 quantity Int NOT NULL,
 price decimal(10,2) NOT NULL,
 discount decimal(4,2) NOT NULL,
 foreign key (order_id)  references Orders ( order_id ),
 foreign key (product_id)  references Products ( product_id )
);


use  Bike_Store ;

-- Insertar datos falsos en la tabla Customers
INSERT INTO Customers (fisrt_name, last_name, phone, email, street, city, state, zip_code)
VALUES
    ('Juan', 'Pérez', '123-456-7890', 'juan@example.com', 'Calle Principal', 'La Paz', 'LP', '12345'),
    ('María', 'García', '987-654-3210', 'maria@example.com', 'Avenida Central', 'Santa Cruz', 'SC', '54321');

-- Insertar datos falsos en la tabla Orders
INSERT INTO Orders (customer_id, order_date)
VALUES
    (1, '2024-07-11'),
    (2, '2024-07-12');

-- Insertar datos falsos en la tabla Products
INSERT INTO Products (product_name, model_year, price)
VALUES
    ('Bicicleta de montaña', 2022, 599.99),
    ('Bicicleta urbana', 2023, 449.99);

-- Insertar datos falsos en la tabla Order_items
INSERT INTO Order_items (order_id, product_id, quantity, price, discount)
VALUES
    (1, 1, 2, 1199.98, 0.1),
    (2, 2, 1, 449.99, 0);
