/***********************************************************************************
   SCRIPT 01 - TABLAS Y DATOS DE PRUEBA - API.CLIENTES

   Ejecutar conectado a la base correcta:
     - Local:      dbClientes (después del script 00)
     - En línea:   la base MSSQL que crea MonsterASP

   Se puede ejecutar varias veces: solo crea lo que no existe.

   Orden:  00 (solo local)  ->  01_Tablas_y_Datos.sql  ->  02_Procedimientos.sql
************************************************************************************/

/*==================================================================================
                                    SCHEMAS
==================================================================================*/

IF SCHEMA_ID(N'SEC') IS NULL
    EXEC(N'CREATE SCHEMA SEC');
GO

IF SCHEMA_ID(N'CLI') IS NULL
    EXEC(N'CREATE SCHEMA CLI');
GO


/*==================================================================================
                              SEGURIDAD (SEC)
==================================================================================*/

IF OBJECT_ID(N'SEC.Roles', N'U') IS NULL
BEGIN
    CREATE TABLE SEC.Roles (
        IdRol           INT PRIMARY KEY IDENTITY(1,1),
        NombreRol       NVARCHAR(50)  NOT NULL,
        Descripcion     NVARCHAR(200)
    );
END
GO

IF OBJECT_ID(N'SEC.Usuarios', N'U') IS NULL
BEGIN
    CREATE TABLE SEC.Usuarios (
        IdUsuario       INT PRIMARY KEY IDENTITY(1,1),
        Nombre          NVARCHAR(50)  NOT NULL,
        Apellido1       NVARCHAR(50)  NOT NULL,
        Apellido2       NVARCHAR(50),
        Correo          NVARCHAR(100) NOT NULL,
        Contrasena      NVARCHAR(50)  NOT NULL,
        FechaCreacion   DATETIME      NOT NULL DEFAULT GETDATE(),
        Activo          BIT           NOT NULL DEFAULT 1,
        IdRol           INT           NOT NULL FOREIGN KEY REFERENCES SEC.Roles (IdRol)
    );
END
GO


/*==================================================================================
                                CLIENTES (CLI)
==================================================================================*/

IF OBJECT_ID(N'CLI.Provincias', N'U') IS NULL
BEGIN
    CREATE TABLE CLI.Provincias (
        IdProvincia         INT PRIMARY KEY IDENTITY(1,1),
        NombreProvincia     NVARCHAR(50) NOT NULL
    );
END
GO

IF OBJECT_ID(N'CLI.Clientes', N'U') IS NULL
BEGIN
    CREATE TABLE CLI.Clientes (
        IdCliente       INT PRIMARY KEY IDENTITY(1,1),
        Cedula          NVARCHAR(20)  NOT NULL UNIQUE,
        Nombre          NVARCHAR(50)  NOT NULL,
        Apellido1       NVARCHAR(50)  NOT NULL,
        Apellido2       NVARCHAR(50),
        Telefono        NVARCHAR(20)  NOT NULL,
        Correo          NVARCHAR(100) NOT NULL,
        Direccion       NVARCHAR(500),
        FechaRegistro   DATETIME      NOT NULL DEFAULT GETDATE(),
        IdProvincia     INT           NOT NULL FOREIGN KEY REFERENCES CLI.Provincias (IdProvincia)
    );
END
GO


/*==================================================================================
                                DATOS DE PRUEBA
==================================================================================*/

-- Roles
IF NOT EXISTS (SELECT 1 FROM SEC.Roles)
BEGIN
    INSERT INTO SEC.Roles (NombreRol, Descripcion) VALUES
        (N'Administrador', N'Usuario con acceso total al sistema'),
        (N'Vendedor',      N'Usuario que administra la cartera de clientes');
END
GO

-- Usuarios (credenciales para probar el inicio de sesión)
IF NOT EXISTS (SELECT 1 FROM SEC.Usuarios WHERE Correo = N'admin@apiclientes.com')
BEGIN
    INSERT INTO SEC.Usuarios (Nombre, Apellido1, Apellido2, Correo, Contrasena, IdRol)
    VALUES (N'Jason', N'Rojas', NULL, N'admin@apiclientes.com', N'Admin1234', 1);
END

IF NOT EXISTS (SELECT 1 FROM SEC.Usuarios WHERE Correo = N'vendedor@apiclientes.com')
BEGIN
    INSERT INTO SEC.Usuarios (Nombre, Apellido1, Apellido2, Correo, Contrasena, IdRol)
    VALUES (N'Laura', N'Méndez', N'Solís', N'vendedor@apiclientes.com', N'Vende1234', 2);
END
GO

-- Provincias de Costa Rica
IF NOT EXISTS (SELECT 1 FROM CLI.Provincias)
BEGIN
    INSERT INTO CLI.Provincias (NombreProvincia) VALUES
        (N'San José'),
        (N'Alajuela'),
        (N'Cartago'),
        (N'Heredia'),
        (N'Guanacaste'),
        (N'Puntarenas'),
        (N'Limón');
END
GO

-- Clientes de ejemplo (datos ficticios)
IF NOT EXISTS (SELECT 1 FROM CLI.Clientes)
BEGIN
    INSERT INTO CLI.Clientes (Cedula, Nombre, Apellido1, Apellido2, Telefono, Correo, Direccion, IdProvincia) VALUES
        (N'1-1111-1111', N'María',  N'Vargas',  N'Quesada', N'+506 8811-2233', N'maria.vargas@correo.com',  N'Barrio Escalante, 200 m norte del parque', 1),
        (N'2-2222-2222', N'Carlos', N'Jiménez', N'Rojas',   N'+506 8722-3344', N'carlos.jimenez@correo.com', N'Centro de Alajuela, frente al mercado',    2),
        (N'4-4444-4444', N'Sofía',  N'Araya',   NULL,       N'+506 8633-4455', N'sofia.araya@correo.com',    N'San Rafael de Heredia',                    4);
END
GO


/*==================================================================================
                                 COMPROBACIÓN
==================================================================================*/

SELECT * FROM SEC.Roles;
SELECT * FROM SEC.Usuarios;
SELECT * FROM CLI.Provincias;
SELECT * FROM CLI.Clientes;
GO
