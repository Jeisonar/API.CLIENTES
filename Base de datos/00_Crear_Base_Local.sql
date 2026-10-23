/***********************************************************************************
   SCRIPT 00 - CREAR LA BASE DE DATOS LOCAL - API.CLIENTES

   Ejecutar SOLO en tu computadora (SQL Server local / LocalDB).
   En MonsterASP NO se ejecuta: la base ya la crea el panel.

   Orden:  00 (solo local)  ->  01_Tablas_y_Datos.sql  ->  02_Procedimientos.sql
************************************************************************************/

IF DB_ID(N'dbClientes') IS NULL
BEGIN
    CREATE DATABASE dbClientes;
END
GO

USE dbClientes;
GO

SELECT DB_NAME() AS BaseDeDatosActual;
GO
