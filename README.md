# Padel Manager - Backend (API)

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?logo=postgresql&logoColor=white)
![Clean Architecture](https://img.shields.io/badge/Architecture-Clean-brightgreen)

Bienvenido al repositorio central de la API para **Padel Manager**. Este sistema está diseñado para optimizar y modernizar la gestión de torneos de pádel en el NEA, reemplazando los procesos manuales y hojas de Excel por una plataforma interactiva y escalable.

---

## Arquitectura del Proyecto

El backend sigue los principios de **Clean Architecture**, dividiendo las responsabilidades en 4 capas principales para facilitar el mantenimiento y el testing:

1.  **Domain (Núcleo):** Contiene las entidades de negocio (`Jugador`, `Torneo`, `Partido`), constantes y contratos base. No tiene dependencias externas.
2.  **Application (Cerebro):** Contiene la lógica de negocio, servicios, DTOs y validaciones. Es donde se orquestan los flujos de la aplicación.
3.  **Infrastructure (Músculos):** Implementa el acceso a la base de datos (**PostgreSQL**) mediante Entity Framework Core, repositorios y servicios externos como seguridad o mensajería.
4.  **API (Puerta de Entrada):** Controladores ASP.NET Core que exponen los endpoints consumidos por el Frontend (React).

---

## Stack Tecnológica

* **Lenguaje:** C# 12 / .NET 8.0+
* **Base de Datos:** PostgreSQL
* **ORM:** Entity Framework Core
* **Documentación:** Swagger / OpenAPI
* **Pruebas:** Postman (Carga de datos y pruebas unitarias de endpoints)
* **IDE:** Visual Studio 2022

---

## Configuración para Desarrolladores

Para poner el proyecto en marcha en tu entorno local, sigue estos pasos:

1.  **Clonar el repositorio:**
    ```bash
    git clone [https://github.com/FabianGQuintana/Padel-Manager-Backend.git](https://github.com/FabianGQuintana/Padel-Manager-Backend.git)
    ```
2.  **Configurar la Base de Datos:**
    Asegúrate de tener PostgreSQL instalado. Configura tu cadena de conexión en el archivo `appsettings.Development.json` dentro de la carpeta `PadelManager.API`.
3.  **Aplicar Migraciones:**
    Abre la "Consola del Administrador de Paquetes" en Visual Studio y ejecuta:
    ```powershell
    Update-Database
    ```
4.  **Ejecutar:**
    Presiona `F5` para iniciar el servidor. Swagger se abrirá automáticamente en tu navegador.

---

## Reglas de Trabajo (Git Workflow)

Para mantener el código limpio y organizado en el **Team Versori 2**, utilizaremos el siguiente flujo:

* **Ramas Principales:**
    * `main`: Contiene el código estable y listo para producción.
    * `develop`: Rama principal de integración para nuevas funciones.
* **Ramas de Tarea:**
    * Para cada nueva funcionalidad o arreglo, crea una rama desde develop: `feature/nombre-de-la-tarea`.
* **Commits:** Usa mensajes descriptivos (ej: `feat: add tournament entities`).

---

## Comunicación y Meetings

Nuestro punto de encuentro oficial para dudas, revisiones de código y sincronización es nuestro servidor de **Discord**:

🔗 [(https://discord.gg/6pPkH9af)]

---
 *Desarrollado  Team Versori 2*
