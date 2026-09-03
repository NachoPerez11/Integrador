# E-Commerce y Payment Service — Arquitectura Limpia

Este repositorio contiene la solución completa de un sistema distribuido compuesto por la API principal de **E-Commerce** y su microservicio integrado de pagos (**Payment Service**). Todo el ecosistema está construido bajo los estrictos lineamientos de la **Arquitectura Limpia**, aplicando patrones como **CQRS**, **Unit of Work**, **Repository Pattern** y **Value Objects**. Utiliza **Entity Framework Core** con **SQLite** para la persistencia de datos en cada servicio, **JWT** para la seguridad basada en roles, y comunicación HTTP síncrona mediante `HttpClient`.

Desarrollado para la asignatura **Backend** de la Tecnicatura Universitaria en Desarrollo de Software de la Universidad Católica de Cuyo.

---

## Estructura de la Solución

La solución agrupa las capas independientes de ambos sistemas para aislar la lógica de negocio de la infraestructura y respetar la regla de Inversión de Dependencias:

* **`Ecosistema E-Commerce`**:
  * `ECommerce.Domain`: Capa central sin dependencias. Entidades, Value Objects, excepciones personalizadas y reglas de negocio encapsuladas mediante constructores privados.
  * `ECommerce.Application`: Orquestador del sistema. Implementa CQRS mediante MediatR (Commands y Queries), interfaces/contratos, DTOs y validación centralizada de entradas con FluentValidation.
  * `ECommerce.Infrastructure`: Capa de datos y servicios externos. Contexto de base de datos, Fluent API, repositorios concretos, inyección de `HttpClient` para conectar con el módulo de pagos, hashing con BCrypt y generación de tokens JWT.
  * `ECommerce.Api`: Punto de entrada web de la tienda. Controladores, configuración de Middlewares y seguridad por roles.

* **Ecosistema de Pagos**:
  * `PaymentService.Domain`: Modelo de negocio aislado para la gestión de transacciones y validaciones internas de cobro.
  * `PaymentService.Application`: Casos de uso y DTOs específicos para el procesamiento de transacciones.
  * `PaymentService.Infrastructure`.
  * `PaymentService.Api`: Endpoints HTTP expuestos para que la tienda principal solicite y evalúe las aprobaciones o rechazos de cobro.

---

## Tecnologías y Herramientas Utilizadas

* **Framework:** .NET 8.0 (SDK)
* **Persistencia:** Entity Framework Core 8.0 (SQLite)
* **Patrones Arquitectónicos:** Clean Architecture, CQRS, Repository Pattern, Unit of Work, Value Objects.
* **Librerías Clave:** MediatR, FluentValidation, BCrypt.Net-Next, Jwt.
* **Documentación:** Swagger UI.

---

## Requisitos e Instalación

Al tratarse de un sistema distribuido, se deben configurar y ejecutar ambos proyectos de manera coordinada.

### 1. Clonar el repositorio
```bash
git clone <https://github.com/NachoPerez11/Integrador.git>
```

### 2. Configuración de Variables de Entorno (appsettings.json)

* **Para el E-Commerce (`ECommerce.Api/appsettings.json`):**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=ecommerce.db"
  },
  "JwtSettings": {
    "Secret": "AcaVaUnaClaveSuperSecretaYLargaParaQueFuncioneElJWT123!",
    "Issuer": "ECommerceApi",
    "Audience": "ECommerceUsers"
  },
  "Services": {
    "Payment": "http://localhost:5132" 
  }
}
```

* **Para el Payment Service (`PaymentService.Api/appsettings.json`):**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=payments.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### 3. Creación y Migración de Bases de Datos
Ejecute los comandos de migraciones para generar el archivo físico de SQLite con sus respectivas tablas:

* **Base de datos de E-Commerce:**
  ```dotnetcli
  dotnet ef migrations add Initial -p ECommerce.Infrastructure -s ECommerce.Api
  dotnet ef database update -p ECommerce.Infrastructure -s ECommerce.Api
  ```

### 4. Ejecución del Sistema Distribuido
Para que la integración funcione, mantenga abiertas dos terminales simultáneas:

* **Terminal 1 (Microservicio de Pagos):**
  ```bash
  cd PaymentService.Api
  dotnet run
  ```

* **Terminal 2 (API de E-Commerce):**
  ```bash
  cd ECommerce.Api
  dotnet run
  ```

---

## Flujo de Integración, Pruebas y Endpoints

Una vez levantados ambos servicios, puede interactuar mediante sus respectivas interfaces de **Swagger UI** en el navegador.

### Operaciones en E-Commerce API:
1. **Autenticación y Roles:** * Registre un usuario en `POST /api/Auth/register`.
   * Inicie sesión en `POST /api/Auth/login` para obtener el Token JWT y autorizar las peticiones en Swagger.
2. **Gestión de Productos:** * Utilice `POST /api/Products` (exclusivo para rol `Admin`) para dar de alta artículos bajo validaciones de dominio.
   * Utilice `GET /api/Products` para consultar el catálogo público.
3. **Creación de Órdenes e Integración:**
   * Envíe un `POST /api/Orders` con el `UserId` y el `TotalAmount`.

### 💳 Operaciones en Payment Service API:
1. **Procesamiento de Cobros:** * Expone el endpoint `POST /api/payments/process`, el cual recibe el identificador de la orden y el monto.