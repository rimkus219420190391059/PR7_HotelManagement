# АИС «Гостиничный комплекс» — ПР7

Вариант 12. Демонстрационная реализация архитектуры .NET по принципам Clean Architecture.

## Состав решения

- `HotelManagement.Core` — доменные сущности и контракты репозиториев.
- `HotelManagement.Application` — прикладные сценарии бронирования и уборки.
- `HotelManagement.Infrastructure` — реализации хранилищ и начальные данные.
- `HotelManagement.WebApi` — REST API и точка композиции зависимостей.
- `HotelManagement.UnitTests` — модульные тесты прикладной логики.
- `HotelManagement.IntegrationTests` — интеграционные тесты HTTP API.

Архитектура сохраняет состав слоёв, описанный в отчёте ПР7. Для компактной и запускаемой учебной реализации вместо PostgreSQL используется in-memory хранилище. Это позволяет запустить приложение без отдельной установки СУБД. Заменить хранилище на PostgreSQL можно в Infrastructure, не меняя Core и Application.

## Направление зависимостей

```text
HotelManagement.Core
        ↑
HotelManagement.Application
        ↑                 ↑
HotelManagement.Infrastructure
        ↑
HotelManagement.WebApi

UnitTests -> Core + Application
IntegrationTests -> WebApi
```

`Core` не ссылается на другие проекты. `Application` зависит от `Core`. `Infrastructure` реализует контракты ядра и зависит от `Core` (в данной реализации не требует прямой ссылки на Application). `WebApi` подключает реализации Infrastructure и сценарии Application в точке композиции.

## Требования

- .NET SDK 8.0.100 или новее.
- Для тестов требуется доступ к NuGet при первом восстановлении пакетов.

## Запуск

Из корневой папки решения:

```bash
dotnet restore
dotnet build
dotnet run --project src/HotelManagement.WebApi
```

Swagger UI: `http://localhost:5080/swagger`

## Примеры запросов

Получить список номеров:

```http
GET /api/rooms
```

Создать бронирование:

```http
POST /api/bookings
Content-Type: application/json

{
  "guestName": "Иван Петров",
  "roomId": 101,
  "checkIn": "2026-11-10T14:00:00Z",
  "checkOut": "2026-11-12T12:00:00Z"
}
```

Зафиксировать уборку:

```http
POST /api/rooms/101/cleaning
Content-Type: application/json

{
  "employeeName": "Анна"
}
```

Запуск тестов:

```bash
dotnet test
```

## Примечание

Это рабочий учебный каркас с реализованными сценариями и тестами, а не законченная промышленная АИС. Данные in-memory сбрасываются при перезапуске. JWT, PostgreSQL, RabbitMQ, внешняя СКУД и Telegram Bot API из архитектурного описания отчёта не подключены в этой демонстрационной версии.
