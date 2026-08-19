# Trabajo Final Integrador - Backend (Optativa II)

**Alumno:** Ignacio Perez
**Universidad:** Universidad Católica de Cuyo (Sede San Juan)

## Descripción del Proyecto

Este repositorio contiene la resolución del examen final práctico. Consiste en un sistema distribuido compuesto por dos microservicios comunicados de forma síncrona mediante HTTP (`HttpClient`). Ambos proyectos están construidos aplicando **Clean Architecture**, el patrón **CQRS** con MediatR, y utilizando **Entity Framework Core (v8.0.11)**.

### Opción Elegida: Payment Service (Opción 1)
Se desarrolló un servicio de pagos independiente para simular la aprobación o rechazo de transacciones al confirmar una orden en el E-Commerce.

**Regla de Negocio:**
El `PaymentService` evalúa el monto total de la orden recibida. 
* Si el monto es **menor a $100.000**, el servicio aprueba el pago devolviendo el estado `Approved`.
* Si el monto es **igual o mayor a $100.000**, el pago es rechazado devolviendo `Rejected`. 
* El E-Commerce actúa en consecuencia actualizando el estado de la orden en su propia base de datos a `Paid` o `PaymentRejected`.

---

## Estructura de Servicios y Puertos

*   **E-Commerce API:** Se ejecuta en `http://localhost:5263`
*   **Payment Service:** Se ejecuta en `http://localhost:5132`

---

## Cómo ejecutar el proyecto (End-to-End)

Para reproducir el flujo completo de comunicación entre ambos servicios, es necesario ejecutarlos en simultáneo.

1. Abrir una terminal en la carpeta del servicio de pagos (`PaymentService.Api`) y ejecutar el comando:
   `dotnet run`
2. Abrir una segunda terminal en la carpeta principal del sistema (`ECommerce.Api`) y ejecutar:
   `dotnet run`
3. Navegar a la interfaz de Swagger del E-Commerce: `http://localhost:5263/swagger`

---

## Credenciales de Prueba (Usuario Admin)

El sistema cuenta con un usuario Administrador generado en tiempo de ejecución (Seed) para probar los endpoints protegidos.

*   **Email:** admin@ecommerce.com
*   **Contraseña:** Admin123456

---

## Prueba del Flujo Distribuido

1. En el Swagger del E-Commerce, utilizar el endpoint de **Login** con las credenciales del Admin.
2. Copiar el token JWT devuelto y pegarlo en el botón **Authorize** (el candado).
3. Dirigirse al endpoint `POST /api/orders` y enviar una petición con un monto de prueba (ej. `5000`).
4. El E-Commerce se comunicará internamente por HTTP al puerto `5132` del Payment Service.
5. Se devolverá una respuesta HTTP 200 confirmando el proceso, y la orden quedará asentada.