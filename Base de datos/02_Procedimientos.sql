/***********************************************************************************
   SCRIPT 02 - PROCEDIMIENTOS ALMACENADOS - API.CLIENTES

   Ejecutar conectado a la base correcta (local dbClientes o la base en línea),
   después de 01_Tablas_y_Datos.sql.

   Se usa CREATE OR ALTER para que se pueda ejecutar varias veces sin error.
************************************************************************************/

/*==================================================================================
                              MÓDULO DE CLIENTES (CLI)
==================================================================================*/

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/***********************************************************************************
   Nombre:      InsertarClientes
   Proposito:   Insertar un nuevo cliente en la tabla CLI.Clientes

   REVISIONES:
   Ver            Fecha            Autor                  Descripcion
   -----------    -----------     --------------         -------------------
   1.0            23/10/2026      Jason Rojas            1.0 Creación de procedimiento almacenado

   Notas:       Valida que la provincia exista y que la cédula no esté repetida.
                Devuelve el IdCliente generado.
************************************************************************************/
CREATE OR ALTER PROCEDURE [dbo].[InsertarClientes]
@pCedula            NVARCHAR(20),
@pNombre            NVARCHAR(50),
@pApellido1         NVARCHAR(50),
@pApellido2         NVARCHAR(50),
@pTelefono          NVARCHAR(20),
@pCorreo            NVARCHAR(100),
@pDireccion         NVARCHAR(500),
@pIdProvincia       INT

AS
BEGIN

    BEGIN TRY

        IF NOT EXISTS (SELECT 1 FROM CLI.Provincias WHERE IdProvincia = @pIdProvincia)
        BEGIN
            RAISERROR(N'La provincia indicada no existe.', 16, 1);
        END

        IF EXISTS (SELECT 1 FROM CLI.Clientes WHERE Cedula = @pCedula)
        BEGIN
            RAISERROR(N'Ya existe un cliente registrado con esa cédula.', 16, 1);
        END

        DECLARE @IdClienteGenerado INT;

        INSERT INTO CLI.Clientes (Cedula, Nombre, Apellido1, Apellido2, Telefono, Correo, Direccion, FechaRegistro, IdProvincia)
        VALUES (@pCedula, @pNombre, @pApellido1, @pApellido2, @pTelefono, @pCorreo, @pDireccion, GETDATE(), @pIdProvincia)

        SET @IdClienteGenerado = CAST(SCOPE_IDENTITY() AS INT)

        SELECT @IdClienteGenerado AS IdCliente

    END TRY

    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH

END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/***********************************************************************************
   Nombre:      ObtenerClientes
   Proposito:   Obtener la información de todos los clientes con su provincia

   REVISIONES:
   Ver            Fecha            Autor                  Descripcion
   -----------    -----------     --------------         -------------------
   1.0            23/10/2026      Jason Rojas            1.0 Creación de procedimiento almacenado

   Notas:
************************************************************************************/
CREATE OR ALTER PROCEDURE [dbo].[ObtenerClientes]

AS
BEGIN

    BEGIN TRY

       SELECT CLT.IdCliente, CLT.Cedula, CLT.Nombre, CLT.Apellido1, CLT.Apellido2,
              CLT.Telefono, CLT.Correo, CLT.Direccion, CLT.FechaRegistro,
              CLT.IdProvincia, PRV.NombreProvincia
       FROM CLI.Clientes CLT
       INNER JOIN CLI.Provincias PRV
            ON CLT.IdProvincia = PRV.IdProvincia
       ORDER BY CLT.IdCliente

    END TRY

    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH

END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/***********************************************************************************
   Nombre:      ObtenerClientePorId
   Proposito:   Obtener la información de un cliente en específico

   REVISIONES:
   Ver            Fecha            Autor                  Descripcion
   -----------    -----------     --------------         -------------------
   1.0            23/10/2026      Jason Rojas            1.0 Creación de procedimiento almacenado

   Notas:
************************************************************************************/
CREATE OR ALTER PROCEDURE [dbo].[ObtenerClientePorId]
@pIdCliente         INT
AS
BEGIN

    BEGIN TRY

       SELECT CLT.IdCliente, CLT.Cedula, CLT.Nombre, CLT.Apellido1, CLT.Apellido2,
              CLT.Telefono, CLT.Correo, CLT.Direccion, CLT.FechaRegistro,
              CLT.IdProvincia, PRV.NombreProvincia
       FROM CLI.Clientes CLT
       INNER JOIN CLI.Provincias PRV
            ON CLT.IdProvincia = PRV.IdProvincia
       WHERE CLT.IdCliente = @pIdCliente

    END TRY

    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH

END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/***********************************************************************************
   Nombre:      ActualizarClientePorId
   Proposito:   Actualizar la información de un cliente

   REVISIONES:
   Ver            Fecha            Autor                  Descripcion
   -----------    -----------     --------------         -------------------
   1.0            23/10/2026      Jason Rojas            1.0 Creación de procedimiento almacenado

   Notas:       Valida la provincia y que la cédula no la tenga otro cliente.
                Si el Id no existe no se actualiza ninguna fila.
************************************************************************************/
CREATE OR ALTER PROCEDURE [dbo].[ActualizarClientePorId]
@pIdCliente         INT,
@pCedula            NVARCHAR(20),
@pNombre            NVARCHAR(50),
@pApellido1         NVARCHAR(50),
@pApellido2         NVARCHAR(50),
@pTelefono          NVARCHAR(20),
@pCorreo            NVARCHAR(100),
@pDireccion         NVARCHAR(500),
@pIdProvincia       INT

AS
BEGIN

    BEGIN TRY

        IF NOT EXISTS (SELECT 1 FROM CLI.Provincias WHERE IdProvincia = @pIdProvincia)
        BEGIN
            RAISERROR(N'La provincia indicada no existe.', 16, 1);
        END

        IF EXISTS (SELECT 1 FROM CLI.Clientes WHERE Cedula = @pCedula AND IdCliente <> @pIdCliente)
        BEGIN
            RAISERROR(N'Ya existe otro cliente registrado con esa cédula.', 16, 1);
        END

        UPDATE CLI.Clientes SET Cedula = @pCedula,
                                Nombre = @pNombre,
                                Apellido1 = @pApellido1,
                                Apellido2 = @pApellido2,
                                Telefono = @pTelefono,
                                Correo = @pCorreo,
                                Direccion = @pDireccion,
                                IdProvincia = @pIdProvincia
        WHERE IdCliente = @pIdCliente

    END TRY

    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH

END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/***********************************************************************************
   Nombre:      EliminarClientePorId
   Proposito:   Eliminar un cliente en específico

   REVISIONES:
   Ver            Fecha            Autor                  Descripcion
   -----------    -----------     --------------         -------------------
   1.0            23/10/2026      Jason Rojas            1.0 Creación de procedimiento almacenado

   Notas:       Si el Id no existe no se elimina ninguna fila.
************************************************************************************/
CREATE OR ALTER PROCEDURE [dbo].[EliminarClientePorId]
@pIdCliente         INT
AS
BEGIN

    BEGIN TRY

        DELETE FROM CLI.Clientes WHERE IdCliente = @pIdCliente

    END TRY

    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH

END
GO


/*==================================================================================
                              INICIO DE SESIÓN (SEC)
==================================================================================*/

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/***********************************************************************************
   Nombre:      spIniciarSesion
   Proposito:   Verificar si existe la persona consultada

   REVISIONES:
   Ver            Fecha            Autor                  Descripcion
   -----------    -----------     --------------         -------------------
   1.0            23/10/2026      Jason Rojas            1.0 Creación de procedimiento almacenado

   Notas:       SP de validación. La contraseña distingue mayúsculas y minúsculas.
                Solo pueden iniciar sesión los usuarios activos.
************************************************************************************/
CREATE OR ALTER PROCEDURE [dbo].[spIniciarSesion]
@pCorreo            NVARCHAR(100),
@pContrasena        NVARCHAR(50)
AS
BEGIN

    BEGIN TRY

       DECLARE @Existe BIT;
       DECLARE @IdUsuario INT;
       DECLARE @NombreCompleto NVARCHAR(160);
       DECLARE @NombreRol NVARCHAR(50);

       SELECT TOP 1 @IdUsuario = USR.IdUsuario,
                    @NombreCompleto = CONCAT_WS(' ', USR.Nombre, USR.Apellido1, USR.Apellido2),
                    @NombreRol = ROL.NombreRol
       FROM SEC.Usuarios USR
       INNER JOIN SEC.Roles ROL
            ON USR.IdRol = ROL.IdRol
       WHERE USR.Correo = @pCorreo AND
             USR.Contrasena = @pContrasena COLLATE Latin1_General_CS_AS AND
             USR.Activo = 1
       ORDER BY USR.IdUsuario

       IF (@IdUsuario IS NOT NULL)
       BEGIN
            SET @Existe = 1;
            SELECT @Existe AS Existe, N'Ha iniciado sesión de manera exitosa. ' AS Mensaje,
                   @IdUsuario AS IdUsuario, ISNULL(@NombreCompleto, N'Nombre desconocido') AS NombreCompleto,
                   ISNULL(@NombreRol, N'Sin rol') AS NombreRol;
       END
       ELSE
       BEGIN
            SET @Existe = 0;
            SELECT @Existe AS Existe, N'El usuario o la contraseña son incorrectos. ' AS Mensaje,
                   0 AS IdUsuario, N'Nombre desconocido' AS NombreCompleto, N'Sin rol' AS NombreRol;
       END

    END TRY

    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH

END
GO


/*==================================================================================
                                 COMPROBACIÓN
==================================================================================*/

EXEC ObtenerClientes;
EXEC ObtenerClientePorId @pIdCliente = 1;
EXEC spIniciarSesion @pCorreo = N'admin@apiclientes.com', @pContrasena = N'Admin1234';
EXEC spIniciarSesion @pCorreo = N'admin@apiclientes.com', @pContrasena = N'admin1234';
GO
