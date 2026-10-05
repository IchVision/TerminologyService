<div align="center">

# 📚 TerminologyService

**REST API сервис терминологии — справочники с версионированием и элементами**

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-Web_API-512BD4?logo=dotnet&logoColor=white)](https://learn.microsoft.com/aspnet/core)
[![EF Core](https://img.shields.io/badge/EF_Core-10-68217A)](https://learn.microsoft.com/ef/core/)
[![SQLite](https://img.shields.io/badge/SQLite-003B57?logo=sqlite&logoColor=white)](https://www.sqlite.org/)
[![Docker](https://img.shields.io/badge/Docker-ready-2496ED?logo=docker&logoColor=white)](#-запуск-в-docker)
[![OpenAPI](https://img.shields.io/badge/OpenAPI-Swagger_UI-85EA2D?logo=swagger&logoColor=black)](#-документация-api)

</div>

---

## О проекте

**TerminologyService** — сервис для хранения и выдачи справочников (страны, валюты, единицы измерения, классификаторы и т. п.).

Каждый справочник может иметь **несколько версий** с датой начала действия, а каждая версия — свой **набор элементов** (пар «код — значение»). Сервис умеет отдавать элементы как конкретной версии, так и актуальной на сегодняшний день.

### Возможности

- **CRUD справочников** — уникальный код, наименование, описание
- **Версионирование** — у справочника может быть любое число версий с датой начала действия
- **Элементы версий** — коды уникальны в пределах версии
- **Выборка по дате** — список справочников, у которых есть версия, действующая на указанную дату
- **Актуальная версия** — элементы последней вступившей в силу версии без указания номера
- **Единая обработка ошибок** — `409 Conflict` на дубликаты, `404 Not Found`, `ProblemDetails` на непредвиденные ошибки
- **OpenAPI + Swagger UI** из коробки
- **Dockerfile** и автоматическое применение миграций при старте

---

## Архитектура

Проект построен по принципам **Clean Architecture**: зависимости направлены внутрь, к доменной модели.

| Слой | Ответственность |
|---|---|
| **Domain** | Сущности `RefBook`, `VersionRefBook`, `Element` и ограничения длины полей. Без внешних зависимостей. |
| **Application** | Сервисы бизнес-логики, интерфейсы репозиториев, перечисление ошибок `Errors`. Результаты операций возвращаются через `Result<T, Errors>` из [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions) — без исключений для ожидаемых ошибок. |
| **Infrastructure** | EF Core `DataContext`, репозитории, миграции, распознавание нарушений уникальности SQLite. |
| **Api** | Контроллеры, DTO с `DataAnnotations`-валидацией, маппинг `Result` → HTTP-статус, глобальный обработчик исключений, DI-композиция. |

---

## Быстрый старт

### Требования

- [.NET SDK 10.0](https://dotnet.microsoft.com/download)

### Запуск

```bash
git clone https://github.com/IchVision/TerminologyService.git
cd TerminologyService

dotnet run --project src/TerminologyService.Api
```

При первом запуске сервис сам создаст базу SQLite и применит миграции.

| Что | Адрес |
|---|---|
| API | http://localhost:5181 |
| Swagger UI | http://localhost:5181/swagger |
| OpenAPI-спецификация | http://localhost:5181/openapi/v1.json |

> Профиль `https` дополнительно слушает `https://localhost:7041`:
> `dotnet run --project src/TerminologyService.Api --launch-profile https`

---

## Конфигурация

Путь к файлу базы задаётся в `src/TerminologyService.Api/appsettings.json` и считается относительно каталога проекта Api:

```json
{
  "Database": {
    "Path": "../../TerminologyService.sqlite"
  }
}
```

По умолчанию файл `TerminologyService.sqlite` создаётся в корне репозитория (он в `.gitignore`).

Значение можно переопределить переменной окружения:

```bash
Database__Path=/data/terminology.sqlite
```

---

## Запуск в Docker

```bash
docker build -t terminology-service .

docker run --rm -p 8080:8080 \
  -e Database__Path=/tmp/terminology.sqlite \
  terminology-service
```

Swagger UI будет доступен на http://localhost:8080/swagger.

> ⚠️ Контейнер работает от непривилегированного пользователя (`$APP_UID`), поэтому путь к базе должен указывать на каталог, доступный ему на запись. Пример выше хранит базу в `/tmp` — данные пропадут вместе с контейнером. Для постоянного хранения смонтируйте том с подходящими правами и укажите путь внутри него.

---

## Документация API

Базовый путь — `/api`. Методы `PUT` и `DELETE` принимают идентификатор в query-параметре `id`.

### Справочники — `/api/RefBooks`

| Метод | Путь | Описание |
|---|---|---|
| `POST` | `/api/RefBooks` | Создать справочник |
| `GET` | `/api/RefBooks?date={yyyy-MM-dd}` | Список справочников. С `date` — только те, у которых есть версия с датой начала ≤ `date` |
| `PUT` | `/api/RefBooks?id={id}` | Обновить справочник |
| `DELETE` | `/api/RefBooks?id={id}` | Удалить справочник |
| `GET` | `/api/RefBooks/{refBookId}/versions` | Все версии справочника |
| `GET` | `/api/RefBooks/{refBookId}/elements?version={v}` | Элементы версии `v`; без параметра — элементы актуальной версии |

### Версии — `/api/Versions`

| Метод | Путь | Описание |
|---|---|---|
| `POST` | `/api/Versions` | Создать версию справочника |
| `PUT` | `/api/Versions?id={id}` | Обновить номер и дату версии |
| `DELETE` | `/api/Versions?id={id}` | Удалить версию |

### Элементы — `/api/Elements`

| Метод | Путь | Описание |
|---|---|---|
| `POST` | `/api/Elements` | Добавить элемент в версию |
| `PUT` | `/api/Elements?id={id}` | Обновить код и значение элемента |
| `DELETE` | `/api/Elements?id={id}` | Удалить элемент |

### Как выбирается актуальная версия

Если в `GET /api/RefBooks/{refBookId}/elements` не передан `version`, берётся версия справочника с **самой поздней датой начала действия, которая раньше текущей даты**. Версии без даты в этот выбор не попадают — их элементы доступны только по явному `?version=`.

### Коды ответов

| Код | Когда |
|---|---|
| `200 OK` | Операция выполнена |
| `400 Bad Request` | Не прошла валидация тела запроса (обязательные поля, максимальная длина) |
| `404 Not Found` | Запись с указанным `id` не найдена |
| `409 Conflict` | Нарушение уникальности (код справочника, номер/дата версии, код элемента) |
| `500 Internal Server Error` | Непредвиденная ошибка — тело в формате [`ProblemDetails`](https://datatracker.ietf.org/doc/html/rfc9457) |

---

## Структура проекта

```
TerminologyService/
├── src/
│   ├── TerminologyService.Domain/          # Сущности предметной области
│   │   ├── RefBook.cs
│   │   ├── VersionRefBook.cs
│   │   └── Element.cs
│   ├── TerminologyService.Application/     # Бизнес-логика
│   │   ├── Interfaces/                     #   контракты репозиториев
│   │   ├── Services/                       #   RefBook / VersionRefBook / Element сервисы
│   │   └── Errors.cs                       #   коды ошибок
│   ├── TerminologyService.Infrastructure/  # Доступ к данным
│   │   ├── Data/DataContext.cs             #   EF Core контекст и индексы
│   │   ├── Repositories/                   #   реализации репозиториев
│   │   ├── Persistence/                    #   design-time фабрика для миграций
│   │   └── Migrations/
│   └── TerminologyService.Api/             # HTTP-слой
│       ├── Controllers/
│       ├── DTOs/
│       ├── Extensions/ResultExtensions.cs  #   Result → HTTP-статус
│       ├── Exceptions/                     #   глобальный обработчик ошибок
│       ├── Program.cs
│       └── TerminologyService.Api.http     #   примеры запросов
├── Dockerfile
└── TerminologyService.slnx
```

---

## Работа с миграциями

```bash
# установить инструмент (один раз)
dotnet tool install --global dotnet-ef

# добавить миграцию
dotnet ef migrations add <Name> \
  --project src/TerminologyService.Infrastructure
```

Применять миграции вручную не нужно — `Program.cs` вызывает `Database.Migrate()` при старте.

---

## Технологии

| | |
|---|---|
| Платформа | .NET 10, C# (primary constructors, nullable reference types) |
| Web | ASP.NET Core Web API, `Microsoft.AspNetCore.OpenApi`, Swashbuckle Swagger UI |
| Данные | Entity Framework Core 10, SQLite |
| Обработка ошибок | CSharpFunctionalExtensions (`Result` / `UnitResult`), `IExceptionHandler` + `ProblemDetails` |
| Контейнеризация | Docker (multi-stage build) |

---
