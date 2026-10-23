# API.CLIENTES — API RESTful en C# (.NET 10 + SQL Server)

API RESTful para administrar **clientes**, hecha en ASP.NET Core Web API (.NET 10) con arquitectura en capas y conexión a SQL Server mediante procedimientos almacenados. Está publicada en MonsterASP.NET.

| Dato | Valor |
|---|---|
| **Enlace público de la API** | `http://TU-SITIO.runasp.net` (abre Swagger para probar) |
| **Documentación OpenAPI** | `http://TU-SITIO.runasp.net/openapi/v1.json` |
| **Código fuente** | Repositorio `https://github.com/TU-USUARIO/API.CLIENTES`, rama `main` (o el ZIP adjunto) |
| **Usuario de prueba (login)** | Correo `admin@apiclientes.com` · Contraseña `Admin1234` |

---

## Listado de endpoints

Todas las respuestas tienen el mismo formato (`Respuesta<T>`):

```json
{ "ok": true, "mensaje": "Texto con el resultado", "valorRetorno": { } }
```

| # | Funcionalidad | Método | Ruta | Cuerpo |
|---|---|---|---|---|
| 1 | Agregar un cliente | `POST` | `/api/Clientes` | Sí (ver B) |
| 2 | Actualizar un cliente por su Id | `PUT` | `/api/Clientes/{IdCliente}` | Sí (ver B) |
| 3 | Eliminar un cliente por su Id | `DELETE` | `/api/Clientes/{IdCliente}` | No |
| 4 | Consultar todos los clientes | `GET` | `/api/Clientes` | No |
| 5 | Consultar un cliente por su Id | `GET` | `/api/Clientes/{IdCliente}` | No |
| 6 | Iniciar sesión (valida las credenciales en la BD) | `POST` | `/api/InicioSesion` | Sí (ver A) |

### A. Iniciar sesión: `POST /api/InicioSesion`

```json
{
  "Correo": "admin@apiclientes.com",
  "Contrasena": "Admin1234"
}
```

Respuesta correcta:

```json
{
  "ok": true,
  "mensaje": "Ha iniciado sesión de manera exitosa. ",
  "valorRetorno": { "idUsuario": 1, "correo": "admin@apiclientes.com", "contrasena": null, "nombreCompleto": "Jason Rojas", "nombreRol": "Administrador" }
}
```

Si el correo o la contraseña no coinciden: `"ok": false` y `"mensaje": "El usuario o la contraseña son incorrectos. "`. La contraseña distingue mayúsculas y minúsculas.

Otro usuario de prueba: `vendedor@apiclientes.com` / `Vende1234`.

### B. Agregar / actualizar cliente: `POST /api/Clientes` y `PUT /api/Clientes/{IdCliente}`

```json
{
  "Cedula": "3-0333-0333",
  "Nombre": "Andrés",
  "Apellido1": "Castro",
  "Apellido2": "Mora",
  "Telefono": "+506 8899-0011",
  "Correo": "andres.castro@correo.com",
  "Direccion": "Cartago centro, 100 m sur de la Basílica",
  "IdProvincia": 3
}
```

- Requeridos: `Cedula` (máx. 20, no se puede repetir), `Nombre`, `Apellido1`, `Telefono`, `Correo` (formato válido) e `IdProvincia`.
- Opcionales: `Apellido2` y `Direccion`.
- Provincias: `1` San José · `2` Alajuela · `3` Cartago · `4` Heredia · `5` Guanacaste · `6` Puntarenas · `7` Limón.
- El `POST` devuelve el `IdCliente` generado en `valorRetorno.idCliente`. En el `PUT` el Id va en la ruta.

### C. Consultar y eliminar

- `GET /api/Clientes`: lista todos los clientes con el nombre de su provincia.
- `GET /api/Clientes/1`: devuelve un cliente. Si el Id no existe: `"ok": false`, *"El cliente con el Id X no ha sido encontrado"*.
- `DELETE /api/Clientes/4`: elimina el cliente. Si el Id no existe: `"ok": false`.

### Prueba rápida del ciclo completo

1. `POST /api/InicioSesion` con el usuario de prueba → `"ok": true`.
2. `POST /api/Clientes` → anotar el `idCliente` que devuelve.
3. `GET /api/Clientes/{id}` → aparece el cliente.
4. `PUT /api/Clientes/{id}` → cambiar teléfono o correo y volver a consultar.
5. `DELETE /api/Clientes/{id}` → luego `GET /api/Clientes/{id}` responde que no existe.

---

## Estructura de la solución

| Proyecto | Contenido |
|---|---|
| `API.CLIENTES` | Web API: `Program.cs`, controladores (`ClientesController`, `InicioSesionController`), `appsettings` y archivo `.http` con todas las pruebas |
| `API.LOGICA.NEGOCIO` | Capa BLL: validaciones de negocio (`ClientesBLL`, `InicioSesionBLL`) |
| `API.DAPPER` | Capa DAL: acceso a datos con `Microsoft.Data.SqlClient` y procedimientos almacenados (`ClientesDAL`, `InicioSesionDAL`) |
| `API.ENTIDADES` | DTOs (`ClientesDto`, `InicioSesionDto`) y la clase genérica `Respuesta<T>` |
| `BaseDatos` | Scripts SQL: `00_Crear_Base_Local.sql`, `01_Tablas_y_Datos.sql` y `02_Procedimientos.sql` |

Flujo de una petición: **Controller → BLL → DAL → Procedimiento almacenado → SQL Server**.

## Base de datos

- Esquema `SEC`: `Roles` y `Usuarios` (credenciales del inicio de sesión).
- Esquema `CLI`: `Provincias` y `Clientes`.
- Procedimientos: `InsertarClientes`, `ObtenerClientes`, `ObtenerClientePorId`, `ActualizarClientePorId`, `EliminarClientePorId` y `spIniciarSesion`.

## Ejecutar en local

1. En SQL Server Management Studio ejecutar `BaseDatos/00_Crear_Base_Local.sql`, luego `01_Tablas_y_Datos.sql` y `02_Procedimientos.sql` (los dos últimos con la base `dbClientes` seleccionada).
2. Revisar la cadena `Conexion` en `API.CLIENTES/appsettings.json`.
3. Abrir `API.CLIENTES.slnx` en Visual Studio y ejecutar el proyecto `API.CLIENTES` (perfil `http`, puerto 5010).
4. Probar con `API.CLIENTES/API.CLIENTES.http` o en `http://localhost:5010/swagger`.

> En producción la cadena de conexión se toma de `appsettings.Production.json`, que no se sube al repositorio.
