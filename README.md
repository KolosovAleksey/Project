# 🗓️ Портал учёта мероприятий (Events Portal)

Веб-приложение для управления мероприятиями, регистрацией участников и площадками. Разработано на ASP.NET Core MVC с использованием Entity Framework Core и Identity.

## 📋 Требования

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- SQLite (встроенная, не требует отдельной установки)

## 🚀 Установка и запуск

1. **Клонировать репозиторий**  
   ```bash
   git clone https://github.com/practice-june-2026/team3-events-.git
   cd team3-events-
   ```

2. **Восстановить зависимости**  
   ```bash
   dotnet restore
   ```

3. **Применить миграции и создать базу данных**  
   ```bash
   dotnet tool restore
   dotnet tool run dotnet-ef database update
   ```

4. **Запустить приложение**  
   ```bash
   dotnet watch run
   ```

5. **Открыть в браузере**  
   ```
   http://localhost:5174
   ```

## 🧪 Тестовые данные

При первом запуске автоматически создаются:

- **Роли**: `admin`, `participant`
- **Пользователи**:  
  - Администратор: `admin@example.com` / `Admin123!`  
  - Участник 1: `user1@example.com` / `User123!`  
  - Участник 2: `user2@example.com` / `User123!`
- **Категории**: `IT`, `Business`, `Art`, `Education`, `Health`
- **Площадки**: 5 площадок в Москве
- **Мероприятия**: 8 событий с разными статусами (`announced`, `ongoing`, `completed`)
- **Регистрации**: 15 записей с различными статусами (`confirmed`, `cancelled`, `attended`)

## 📁 Структура проекта (ключевые директории)

```
├── Controllers/      # Контроллеры MVC
├── Views/            # Razor-представления
├── Models/           # ViewModel-классы
├── Entities/         # Модели базы данных (EF Core)
├── Data/             # DbContext и инициализатор БД
├── Services/         # Бизнес-логика (регистрация)
├── Migrations/       # Миграции EF Core
├── wwwroot/          # Статические файлы (CSS, JS)
└── Program.cs        # Точка входа
```

## 🧩 Модель данных

- **User** – пользователь (расширяет `IdentityUser`)
- **Category** – категория мероприятия
- **Venue** – площадка проведения
- **Event** – мероприятие (название, дата, вместимость, статус, описание)
- **Registration** – запись участника на мероприятие (статус: `confirmed`, `cancelled`, `attended`)

## 🛠 Технологии

- ASP.NET Core MVC
- Entity Framework Core (Code First)
- SQLite
- ASP.NET Core Identity
- Bootstrap (интерфейс)


