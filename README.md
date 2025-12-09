# TalentoPlus S.A.S. - Sistema de Gestión de Empleados

Sistema de gestión de empleados para TalentoPlus S.A.S. Permite centralizar la información de empleados, subir datos desde Excel, generar hojas de vida en PDF y exponer una API REST para autoregistro y login.

## Tecnologías

- ASP.NET Core 7
- Entity Framework Core
- PostgreSQL
- QuestPDF (generación de PDFs)
- xUnit + Moq (pruebas unitarias)
- Docker & Docker Compose

## Requisitos

- .NET 7 SDK
- Docker
- Docker Compose

## Variables de entorno

Configura las siguientes variables (ejemplo en `.env`):

```
POSTGRES_USER=postgres
POSTGRES_PASSWORD=postgres
POSTGRES_DB=talentoplus
JWT_SECRET=mi_clave_secreta
SMTP_HOST=smtp.example.com
SMTP_PORT=587
SMTP_USER=user@example.com
SMTP_PASSWORD=password
```

## Cómo correr la solución

### Con Docker Compose

```
docker-compose up --build
```

- La API estará disponible en `http://localhost:5000/api`
- La Web estará disponible en `http://localhost:5000`

### Sin Docker

1. Crear la base de datos PostgreSQL y configurar connection string.
2. Restaurar paquetes:

```
dotnet restore
```

3. Aplicar migraciones:

```
dotnet ef database update --project 2.Infrastructure --startup-project 1.Web
```

4. Ejecutar Web y API:

```
dotnet run --project 1.Web
```


## Ejecutar pruebas unitarias

```
dotnet test
```

## Funcionalidades principales implementadas

- Registro de empleados desde la API
- Login de empleados con JWT
- Gestión de empleados desde la web (CRUD)
- Importación de empleados desde Excel
- Generación de hoja de vida en PDF

**Nota:** La integración con IA y el dashboard avanzado están pendientes.

## Docker Compose básico

```
version: '3.8'
services:
db:
image: postgres:15
restart: always
environment:
POSTGRES_USER: ${POSTGRES_USER}
POSTGRES_PASSWORD: ${POSTGRES_PASSWORD}
POSTGRES_DB: ${POSTGRES_DB}
ports:
- "5432:5432"
volumes:
- pgdata:/var/lib/postgresql/data

web:
build:
context: .
dockerfile: 1.Web/Dockerfile
environment:
- ConnectionStrings__DefaultConnection=Host=db;Database=${POSTGRES_DB};Username=${POSTGRES_USER};Password=${POSTGRES_PASSWORD}
- JWT_SECRET=${JWT_SECRET}
- SMTP_HOST=${SMTP_HOST}
- SMTP_PORT=${SMTP_PORT}
- SMTP_USER=${SMTP_USER}
- SMTP_PASSWORD=${SMTP_PASSWORD}
depends_on:
- db
ports:
- "5000:5000"

volumes:
pgdata:
```
