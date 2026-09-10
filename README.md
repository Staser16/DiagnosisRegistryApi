# ASP.NET WEB API do zarządzania rozpoznaniami medycznymi pacjentów.

Aplikacja umożliwia rejestrowanie oraz odczytywanie rozpoznań pacjentów. Rozpoznania zawierają między innymi kod choroby, opis, datę rozpoznania, status kliniczny oraz stopień potwierdzenia. Aplikacja umożliwia również zmianę statusu klinicznego oraz raportowanie rozpoznań do zewnętrznego systemu. Zewnętrzny system jest zaimplementowany jako osobny endpoint, z którym aplikacja komunikuje się i oczekuje na odpowiedź.

## 1 Technologie użyte w projekcie:

- .NET 10.0

- Entity Framework Core

- SQLite

- Dapper

- GraphQL (Hot Chocolate)

- Scalar (API documentation tool)

- Dependency Injection

- CQRS

- SOLID

- Repository Pattern

- Decorator pattern

- Handler Pattern

- Specification Pattern

- Result Pattern

- Retry Pattern

- Middleware

- LINQ

- Logging

- System.Text.Json

- ValueSet

- Linux

- VS Code

- Migrations

## 2 Wymagania:

- .NET SDK 10.0

- Entity Framework CLI

- Windows/Linux

## 3 Instalacja:

### Klonowanie:

```bash
git clone https://github.com/Staser16/DiagnosisRepositoryApi.git

cd DiagnosisRepositoryApi/
```

### Kompilacja projektu:

```bash
dotnet restore

dotnet build
```

### Ustawienie Bazy z Entity Framework Core:

```bash
dotnet ef database update
```

### Uruchamianie projektu:

```bash
dotnet run
```

## 4 Informacje dodatkowe:

- API będzie dostępne pod endpointem:

```http
http://localhost:5274
```

- Rejestracja rozpoznania:

POST /api/record

- Odczytywanie rozpoznań:

GET /api/QueryRecord

- Zmiana statusu klinicznego:

PATCH /api/record

- Endpoint raportowania:

POST /api/record/Rejestr

- GraphQL:

POST /graphql

- Dokumentacja będzie dostępna pod adresem:

```http
http://localhost:5274/scalar
```

## 5 GraphQL:

API udostępnia możliwość wykonywania zapytań oraz mutacji za pomocą GraphQL przy użyciu biblioteki Hot Chocolate.

GraphQL dostępny jest pod endpointem:

```http
http://localhost:5274/graphql
```

## 6 Walidacja danych:

API wykorzystuje walidację danych podczas rejestrowania rozpoznań.

Sprawdzane są między innymi:

- poprawność kodu względem ValueSet

- poprawność systemu kodowania

- poprawność daty rozpoznania

- istnienie pacjenta

- poprawność danych dotyczących początku rozpoznania

- duplikaty rozpoznań

- poprawność statusu klinicznego

W przypadku wystąpienia wielu błędów są one zwracane razem w odpowiedzi HTTP.

## 7 ValueSet:

Aplikacja wykorzystuje lokalny plik JSON zawierający dozwolone kody rozpoznań.

ValueSet jest ładowany podczas uruchamiania aplikacji i wykorzystywany podczas walidacji rejestrowanych rozpoznań.

Dzięki przechowywaniu danych lokalnie aplikacja nie wymaga połączenia z zewnętrznym serwerem w celu przeprowadzenia walidacji kodów.

## 8 Baza danych:

Aplikacja wykorzystuje SQLite jako bazę danych.

Do obsługi danych wykorzystywane są:

- Entity Framework Core — operacje na encjach i migracje

- Dapper — wykonywanie wybranych zapytań SQL

Baza danych jest tworzona oraz aktualizowana za pomocą migracji Entity Framework Core.

## 9 Raportowanie:

Aplikacja posiada mechanizm raportowania rozpoznań do zewnętrznego systemu poprzez HTTP.

W przypadku wystąpienia tymczasowego błędu aplikacja ponawia próbę wysłania raportu.

Stan raportowania rozpoznania jest przechowywany w bazie danych i dostępny poprzez API.

## 10 Dane testowe:

Baza danych zawiera przykładowego pacjenta wraz z jego danymi, wykorzystywanego do testowania aplikacji.

```
Id:    b82e4c17-9a36-4f51-a2d8-731c5e90b441
Pesel: 12345678901
```

Przykładowe użycie:

GET /api/QueryRecord/Patient?PatientId=b82e4c17-9a36-4f51-a2d8-731c5e90b441