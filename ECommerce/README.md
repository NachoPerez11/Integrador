# E-Commerce API — Arquitectura Limpia (.NET 8)

Este proyecto consiste en el desarrollo de una API REST para un sistema de E-Commerce, construida bajo los estrictos lineamientos de la **Arquitectura Limpia (Clean Architecture)** y aplicando el patrón **CQRS**. Utiliza **Entity Framework Core** con **SQLite** para la persistencia de datos y **JWT** para la seguridad.

Desarrollado para la asignatura **Backend (Optativa II)** de la Tecnicatura Universitaria en Desarrollo de Software de la Universidad Católica de Cuyo (UCC).

---

## 🏗️ Estructura de la Solución

El proyecto está dividido en capas independientes para aislar la lógica del negocio de los componentes tecnológicos y de infraestructura, respetando la Inversión de Dependencias:

* **`ECommerce.Domain`**: Capa central sin dependencias externas. Contiene las entidades puras (`Product`, `User`), Excepciones de Dominio personalizadas y reglas de negocio encapsuladas mediante *Domain-Driven Design* (uso de constructores privados y *Factory Methods*).
* **`ECommerce.Application`**: Actúa como el orquestador del sistema. Implementa el patrón CQRS mediante **MediatR** (Commands y Queries), contiene los DTOs de salida, los contratos/interfaces (`IProductRepository`, `IJwtTokenGenerator`) y la validación de entrada mediante **FluentValidation** (Pipeline Behaviors).
* **`ECommerce.Infrastructure`**: Implementa el acceso a datos y servicios externos. Contiene el `ECommerceDbContext`, configuraciones explícitas de tablas con **Fluent API**, los repositorios concretos de SQLite, el Unit of Work, y la implementación de seguridad (generación de tokens JWT y hasheo con BCrypt).
* **`ECommerce.Api`**: Punto de entrada de la aplicación. Contiene los Endpoints expuestos a través de Controladores, configuración de Middlewares (Autenticación/Autorización) y el contenedor de Inyección de Dependencias.

---

## 🚀 Tecnologías y Herramientas Utilizadas

* **SDK:** .NET 8.0
* **ORM:** Entity Framework Core 8.0 (Proveedor SQLite)
* **Patrones:** Clean Architecture, CQRS, Unit of Work, Repository Pattern
* **Librerías Clave:** MediatR, FluentValidation, BCrypt.Net-Next, System.IdentityModel.Tokens.Jwt
* **Documentación:** Swagger UI
* **IDE:** Visual Studio Code

---

## ⚙️ Requisitos e Instalación

Siga estos pasos para clonar, configurar y ejecutar el proyecto localmente en su entorno:

### 1. Clonar el repositorio
```bash
git clone <URL_DE_TU_REPOSITORIO_GITHUB>
cd ECommerce
```

### 2. Configurar Variables de Entorno
Antes de ejecutar la base de datos, asegúrese de tener configurado el archivo `appsettings.json` en el proyecto `ECommerce.Api` con su cadena de conexión y las claves secretas para JWT:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=ecommerce.db"
  },
  "JwtSettings": {
    "Secret": "AcaVaUnaClaveSuperSecretaYLargaParaQueFuncioneElJWT123!",
    "Issuer": "ECommerceApi",
    "Audience": "ECommerceUsers"
  }
}
```

### 3. Restaurar dependencias y compilar
```bash
dotnet build
```

### 4. Ejecutar las Migraciones (Creación de la BD)
Ejecute el siguiente comando desde la raíz del proyecto para generar automáticamente el archivo físico de la base de datos `ecommerce.db` con sus respectivas tablas indexadas:

```bash
dotnet ef database update --project ECommerce.Infrastructure --startup-project ECommerce.Api
```

### 5. Arrancar la API
```bash
dotnet run --project ECommerce.Api
```

---

## 💡 Uso e Interacción con la API

Una vez iniciado el servidor local, la interfaz interactiva de **Swagger UI** estará disponible en su navegador en la URL asignada (generalmente `http://localhost:5xxx/swagger`).

### Flujo de Pruebas:

1. **Registrar un Usuario:** Diríjase al endpoint `POST /api/Auth/register` para registrar una nueva cuenta. El sistema validará los datos, hasheará la contraseña con BCrypt y guardará la entidad.
2. **Iniciar Sesión (Login):** Vaya al endpoint `POST /api/Auth/login`, ingrese las credenciales creadas y obtenga el Token JWT generado por la respuesta.
3. **Crear Productos:** Ingrese al endpoint `POST /api/Products` e inserte nuevos artículos enviando el JSON correspondiente. Las reglas del Dominio validarán que no se ingresen precios nulos ni stock negativo.
4. **Listar Productos:** Utilice el endpoint `GET /api/Products` para recuperar el catálogo. La petición viaja mediante un `Query` y devuelve DTOs mapeados directamente desde la capa de Aplicación.