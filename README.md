# Employees – TalentoPlus API

Sistema backend desarrollado en **C# (.NET 9)** para la **gestión de empleados y candidatos**, orientado a procesos de **contratación futura**.  
La aplicación expone una **API RESTful** que permite administrar información de usuarios, importar datos desde archivos Excel, generar hojas de vida en PDF y notificar eventos relevantes mediante correo electrónico.

---

## 🚀 Funcionalidades principales

- Gestión de empleados y candidatos
- Importación masiva de información desde archivos Excel
- Generación automática de hojas de vida en PDF
- Notificaciones por correo electrónico ante eventos del sistema
- API RESTful segura y escalable
- Autenticación y autorización con JWT
- Persistencia de datos mediante Entity Framework Core y MySQL
- Pruebas unitarias para asegurar calidad del código

---

## 📊 Importación de datos desde Excel

El sistema permite cargar información de empleados a partir de archivos Excel:

- Mapeo de hojas de cálculo usando EPPlus
- Validación de datos antes de persistirlos
- Conversión de filas de Excel a DTOs
- Inserción segura en base de datos

Este proceso facilita la carga masiva de candidatos provenientes de fuentes externas como procesos de reclutamiento o migraciones de datos.

---

## 📄 Generación de hojas de vida (PDF)

A partir de la información almacenada del empleado, el sistema:

- Construye documentos PDF dinámicos usando QPdf
- Genera hojas de vida estructuradas y exportables
- Permite estandarizar formatos para procesos de selección

---

## 📧 Notificaciones por correo electrónico

El sistema integra un servicio SMTP que permite:

- Enviar correos automáticos cuando se crea un nuevo usuario
- Notificar eventos importantes del ciclo de vida del empleado
- Facilitar la comunicación en procesos de contratación

---

## 🔐 Autenticación y seguridad

La API utiliza JWT (JSON Web Tokens) para proteger los endpoints.

Flujo de seguridad:
1. Autenticación del usuario y emisión de token JWT
2. Envío del token en cada request mediante:

```
Authorization: Bearer {token}
```

3. Validación de firma, expiración y claims

---

## 🧱 Arquitectura

El proyecto está estructurado siguiendo principios de diseño orientados a mantenibilidad:

- Controllers: Exposición de endpoints REST
- Services: Lógica de negocio
- DTOs: Contratos de entrada y salida
- Persistence: Acceso a datos con EF Core
- Security: Autenticación JWT
- Infrastructure: Integraciones externas (SMTP, PDF, Excel)
- Tests: Pruebas unitarias

Este enfoque facilita la escalabilidad y futuras integraciones empresariales.

---

## 🧪 Pruebas unitarias

Incluye pruebas unitarias para validar:
- Lógica de negocio
- Procesos de importación
- Comportamiento de servicios críticos

Esto permite detectar errores tempranamente y asegurar estabilidad.

---

## 🛠️ Tecnologías utilizadas

- Lenguaje: C#
- Framework: .NET 9
- API: ASP.NET Web API
- ORM: Entity Framework Core
- Base de datos: MySQL
- Autenticación: JWT
- Excel: EPPlus
- PDF: QPdf
- Correo: SMTP
- Testing: Pruebas unitarias
- Control de versiones: Git

---

## ▶️ Ejecución del proyecto

1. Clonar el repositorio:

```
git clone https://github.com/TEQUIE835/employees-talentoplus.git
```

2. Configurar en appsettings.json:
- Cadena de conexión MySQL
- Credenciales SMTP

3. Ejecutar migraciones:

```
dotnet ef database update
```

4. Ejecutar la aplicación:

```
dotnet run
```

---

## 📌 Notas finales

Este proyecto fue desarrollado como parte de un proceso de formación avanzada en backend, simulando escenarios empresariales reales como importación masiva de datos, generación de documentos y notificaciones automáticas, aplicando buenas prácticas de arquitectura y seguridad.
