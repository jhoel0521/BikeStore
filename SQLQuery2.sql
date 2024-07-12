use  Bike_Store ;

-- Clientes
INSERT INTO Customers (first_name, last_name, phone, email, street, city, state, zip_code) VALUES
('Juan', 'Pérez', '555-1234', 'juan.perez@example.com', 'Calle Falsa 123', 'Ciudad', 'Estado', '12345'),
('María', 'González', '555-5678', 'maria.gonzalez@example.com', 'Avenida Siempre Viva 456', 'Ciudad', 'Estado', '67890'),
('Carlos', 'Rodríguez', '555-8765', 'carlos.rodriguez@example.com', 'Boulevard del Sol 789', 'Ciudad', 'Estado', '54321'),
('Ana', 'López', '555-4321', 'ana.lopez@example.com', 'Camino Verde 101', 'Ciudad', 'Estado', '98765'),
('Luis', 'Fernández', '555-3456', 'luis.fernandez@example.com', 'Plaza Mayor 202', 'Ciudad', 'Estado', '65432');

-- Productos
INSERT INTO Products (product_name, model_year, price) VALUES
('Bicicleta de Montaña', 2023, 500.00),
('Bicicleta de Carretera', 2023, 750.00),
('Bicicleta Híbrida', 2023, 600.00),
('Bicicleta BMX', 2023, 300.00),
('Bicicleta Plegable', 2023, 400.00);

-- Pedidos/Ordenes

INSERT INTO Orders (customer_id, order_date) VALUES
(1, '2023-01-01'),
(1, '2023-02-01'),
(2, '2023-01-15'),
(2, '2023-02-15'),
(3, '2023-01-20'),
(3, '2023-02-20'),
(4, '2023-01-25'),
(4, '2023-02-25'),
(5, '2023-01-30'),
(5, '2023-02-28');

-- Detalles de los pedidos/ordenes

INSERT INTO Order_items (order_id, product_id, quantity, price, discount) VALUES
-- Ordenes del cliente 1
(1, 1, 1, 500.00, 0.00),
(1, 2, 1, 750.00, 0.00),
(2, 3, 1, 600.00, 0.00),
(2, 4, 2, 300.00, 0.00),
-- Ordenes del cliente 2
(3, 2, 1, 750.00, 0.00),
(3, 5, 1, 400.00, 0.00),
(4, 1, 1, 500.00, 0.00),
(4, 3, 1, 600.00, 0.00),
-- Ordenes del cliente 3
(5, 4, 1, 300.00, 0.00),
(5, 5, 1, 400.00, 0.00),
(6, 1, 2, 500.00, 0.00),
-- Ordenes del cliente 4
(7, 2, 1, 750.00, 0.00),
(7, 3, 2, 600.00, 0.00),
(8, 4, 1, 300.00, 0.00),
(8, 5, 1, 400.00, 0.00),
-- Ordenes del cliente 5
(9, 1, 1, 500.00, 0.00),
(9, 3, 1, 600.00, 0.00),
(10, 2, 1, 750.00, 0.00),
(10, 4, 1, 300.00, 0.00);
