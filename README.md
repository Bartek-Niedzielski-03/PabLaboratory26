# PabLaboratory26 – Projekt zaliczeniowy z przedmiotu Programowanie aplikacji back-endowych 2026 

## Autor
Bartłomiej Niedzielski

## Opis projektu
Aplikacja backendowa CRM zbudowana w ASP.NET Core 9, C#, z wykorzystaniem czystej architektury.

## Zrealizowane funkcje

### Laboratoria 2-7 (podstawa projektu)
- Czysta architektura: AppCore, Infrastructure, WebApi, Tests
- Encje: Person, Company, Organization dziedziczące po Contact
- Repozytoria generyczne i konkretne (Memory i Entity Framework)
- Unit of Work pattern
- DTOs z mapowaniem (FromEntity, ToEntity)
- Walidacja danych wejściowych (FluentValidation)
- Implementacja EF Core z SQLite
- ASP.NET Identity (użytkownicy CrmUser, role CrmRole)
- JWT – logowanie, access token, refresh token, revoke
- Polityki autoryzacji (AdminOnly, SalesAccess, ReadOnlyAccess itd.)
- Seeder danych (role, użytkownicy, przykładowe kontakty)
- Obsługa wyjątków (ProblemDetailsExceptionHandler)
- Notatki i tagi do kontaktów

### Zadanie finalne w moim przypadku zadanie nr.4
- **ValueObject PhoneNumber** – przechowuje numer telefonu, rozpoznaje kraj i kod kraju na podstawie prefiksu międzynarodowego
- **Polimorficzny kontroler** `POST /api/contacts/poly` – jeden endpoint obsługuje tworzenie kontaktów trzech typów (Person, Company, Organization) na podstawie pola `contactType` w JSON
- **Polimorficzna deserializacja** – własny `JsonConverter` (`CreateContactDtoConverter`) dekoduje typ kontaktu z JSON
- **Właściciel kontaktu** – każdy kontakt zapisuje `CreatedByUserId`; edycja i usunięcie możliwe tylko przez właściciela lub administratora
- **Testy integracyjne** – testy end-to-end dla polimorficznego kontrolera

## Uruchomienie testów

```bash
cd PabLaboratory26Tests
dotnet test
```

## Endpointy API

### Autoryzacja
| Metoda | Endpoint | Opis |
|--------|----------|------|
| POST | `/api/auth/login` | Logowanie, zwraca JWT |
| POST | `/api/auth/refresh` | Odświeżenie tokenu |
| POST | `/api/auth/revoke` | Wylogowanie |
| GET | `/api/auth/me` | Dane zalogowanego użytkownika |

### Kontakty polimorficzne (zadanie finalne)
| Metoda | Endpoint | Opis |
|--------|----------|------|
| GET | `/api/contacts/poly` | Lista wszystkich kontaktów (Person + Company + Organization) |
| GET | `/api/contacts/poly/{id}` | Kontakt po ID |
| POST | `/api/contacts/poly` | Dodaj kontakt dowolnego typu |
| PUT | `/api/contacts/poly/{id}` | Edytuj kontakt (tylko właściciel lub admin) |
| DELETE | `/api/contacts/poly/{id}` | Usuń kontakt (tylko właściciel lub admin) |

## Użytkownicy testowi
| Email | Hasło | Rola |
|-------|-------|------|
| sales@crm.local | Sales@123! | Salesperson |

## Technologie
- ASP.NET Core 9
- Entity Framework Core + SQLite
- ASP.NET Identity
- JWT Bearer Authentication
- FluentValidation
- xUnit (testy)

## Link do repozytorium
https://github.com/Bartek-Niedzielski-03/PabLaboratory26