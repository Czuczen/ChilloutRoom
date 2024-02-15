# ChilloutRoom

"**ChilloutRoom**" to aplikacja webowa oparta na frameworku [ASP.NET Boilerplate](https://aspnetboilerplate.com/Templates) w wersji 6.0.0.
Boilerplate ten zawiera gotowe rozwiązania dla aplikacji webowych, takie jak autoryzacja, uwierzytelnianie, zarządzanie sesjami oraz wiele innych przydatnych funkcjonalności.
W ramach "**ChilloutRoom**" użyto narzędzi takich jak **ASP.NET MVC 5.x.** Dodatkowo, do zarządzania bazą danych wykorzystano **Entity Framework**, a do tworzenia interaktywnych elementów w interfejsie użytkownika **jQuery**.  

Celem stworzenia aplikacji było zgłębienie technologii .NET oraz zastosowanie jej w praktyce.  

## Funkcjonalności

Główną funkcjonalnością aplikacji jest gra "**Plantacja**", która polega na hodowaniu roślin. Został dla niej stworzony panel konfiguracyjny (system CMS).
Gra jest podzielona na dzielnice, a każda z nich ma swojego opiekuna, który w panelu konfiguracyjnym w postaci wypełniania formularzy definiuje, co ma się na niej znajdować.
Można definiować takie rzeczy jak produkty typu nawóz, gleba, lampa, woda itp. oraz zadania ich wymagania i nagrody.  

[Poradnik gracza plantacji](https://docs.google.com/document/d/1h-qN-2J9vUjZNBh68RoF-0TmmEXqJihHZg6hzVhQ2-E/edit?usp=share_link)  
[Gameplay](https://youtu.be/UkhslP_ob7s?list=TLGGXh18XzzmEbIyNDAyMjAyMw)  

Drugą grą jest kółko i krzyżyk. Można grać przeciwko komputerowi na różnych poziomach trudności, a także przeciwko innym graczom.  

W aplikacji zastosowano trzy rodzaje testów.  
[Testy Selenium IDE](https://docs.google.com/document/d/11A6NV4iZUJ2crnvJl6ZRyxe2msLfy7Xm8oOMw6gF56M/edit?usp=share_link)  
Testy struktury - sprawdzają one czy definicje opiekuna dzielnicy są poprawne.  
Testy jednostkowe  
Testy Selenium IDE i testy jednostkowe zawierają tylko przykładowe testy. Nie testują całej aplikacji.  

## Technologie

C#  
JavaScript  
HTML  
CSS  

ASP.NET MVC 5.x.  
Entity Framework  
jQuery  
Bootstrap  
SignalR  

Selenium IDE  
xUnit  
Shouldly  

<details>
  <summary><h2>Jak uruchomić aplikację</h2></summary>
  
### Visual Studio

1. W projekcie "**CzuczenLand.Web**" dodaj plik "**settings.config**" i uzupełnij go według szablonu:  
```  
  <appSettings>
    <add key="webpages:Version" value="3.0.0.0" />
    <add key="webpages:Enabled" value="false" />
    <add key="ClientValidationEnabled" value="true" />
    <add key="UnobtrusiveJavaScriptEnabled" value="true" />
    <add key="AdminPassword" value=""/>
  </appSettings>
```
2. W wartości klucza "**AdminPassword**" wprowadź swoje hasło dla host admina.  
3. W projekcie "**CzuczenLand.Web**" dodaj plik "**connection.config**" i uzupełnij go według szablonu:  
```
  <connectionStrings>
    <add name="Default" connectionString="" providerName="System.Data.SqlClient" />
  </connectionStrings>
```
4. W wartości "**connectionString=**" wprowadź informacje dotyczące połączenia z bazą danych MSSQL.  
5. Kliknij prawym przyciskiem myszy na rozwiązanie w eksploratorze rozwiązań i z rozwijanej listy wybierz "**Przywróć pakiety NuGet**".  
6. Kliknij prawym przyciskiem myszy na projekt "**CzuczenLand.WebApi**" i z rozwijanej listy wybierz "**Zwolnij projekt**".  
   Powtórz działanie dla projektów:  
		"**CzuczenLand.Web**"  
		"**CzuczenLand.EntityFramework**"  
		"**CzuczenLand.Core**"  
		"**CzuczenLand.Application**"  
		"**CzuczenLand.Migrator**"  
		"**CzuczenLand.Tests**"  
7. Kliknij prawym przyciskiem myszy na rozwiązanie w eksploratorze rozwiązań i z rozwijanej listy wybierz "**Ładuj wszystkie projekty**".  
8. Kliknij prawym przyciskiem myszy na projekt "**CzuczenLand.Web**" i z rozwijanej listy wybierz "**Ustaw jako projekt startowy**".  
9. Na górnym pasku menu wybierz "**Narzędzia**", a następnie z rozwijanej listy najedź na "**Menedżer pakietów NuGet**" i z kolejnej listy wybierz "**Konsola menedżera pakietów**".  
10. W konsoli menedżera pakietów dla pola "**Projekt domyślny**" z rozwijanej listy wybierz "**CzuczenLand.EntityFramework**".  
11. W konsoli menedżera pakietów wpisz komende "**Update-Database**" i wciśnij Enter.  
12. Uruchom aplikację (Ctrl+F5).  


### JetBrains Rider

1. W projekcie "**CzuczenLand.Web**" dodaj plik "**settings.config**" i uzupełnij go według szablonu:  
```  
  <appSettings>
    <add key="webpages:Version" value="3.0.0.0" />
    <add key="webpages:Enabled" value="false" />
    <add key="ClientValidationEnabled" value="true" />
    <add key="UnobtrusiveJavaScriptEnabled" value="true" />
    <add key="AdminPassword" value=""/>
  </appSettings>
```
2. W wartości klucza "**AdminPassword**" wprowadź swoje hasło dla host admina.  
3. W projekcie "**CzuczenLand.Web**" dodaj plik "**connection.config**" i uzupełnij go według szablonu:  
```
  <connectionStrings>
    <add name="Default" connectionString="" providerName="System.Data.SqlClient" />
  </connectionStrings>
```
4. W wartości "**connectionString=**" wprowadź informacje dotyczące połączenia z bazą danych MSSQL.  
5. W eksploratorze kliknij prawym przyciskiem myszy na projekt "**CzuczenLand.EntityFramework**", z rozwijanej listy najedź na "**EntityFramework**" i z kolejnej listy wybierz "**Update Database**".  
6. W oknie które się wyświetliło, w dolnej sekcji zaznacz "**Use connection string**".  
7. W polu "**Connection string**" wprowadź informacje dotyczące połączenia z bazą danych.  
8. W polu "**Connection provider**" wprowadź wartość "**s**" i z listy wybierz "**System.Data.SqlClient**".  
9. Zatwierdź przyciskiem **Ok**.  
10. W górnym prawym rogu w wyborze konfiguracji zmień wartość "**CzuczenLand.Migrator**" na "**CzuczenLand.Web - IIS Express**".  
11. Uruchom aplikację (Ctrl+F5).  


### Tworzenie dzielnicy (opcjonalnie)
**Uwaga! Zaleca się wykonanie tej czynności na serwerze gdyż lokalnie może to zająć sporo czasu.**  

1. Zaloguj się do aplikacji na konto host admina wprowadzając w pole "**Nick**" wartość "**admin**", a w pole "**Hasło**" wartość, która została wcześniej ustawiona w pliku "**settings.config**" w projekcie "**CzuczenLand.Web**" dla klucza "**AdminPassword**".  
2. Przejdź do menu "**Panel konfiguracyjny**".  
3. Kliknij przycisk "**Kloner**".  
4. W oknie które się wyświetliło, kliknij przycisk "**Klonuj**" w sekcji "**Klonuj z folderu aplikacji**".  
5. Po ukończeniu tworzenia dzielnicy w nowej zakładce wyświetlą się informacje o stworzonej dzielnicy i jej opiekunie.  
6. Zapisz dane logowania opiekuna dzielnicy, na przykład w notatniku.  
7. Wróć do zakładki z interfejsem klonowania dzielnic i zamknij go.  
8. Pod nagłówkiem "**Wyszukiwanie**" z rozwijanej listy wybierz "**Dzielnica**".  
9. Kliknij "**Edytuj**" dla rekordu dzielnicy o nazwie "**Chillout**".  
10. Dla pola "**Czy jest zdefiniowana**" zmień wartość z "**Nie**" na "**Tak**" i zapisz zmianę.  
11. Teraz możesz rozpocząć grę, logując się na konto opiekuna stworzonej dzielnicy lub rejestrując się jako nowy użytkownik. Administrator nie może grać w grę "**Plantacja**".  

</details>

## Dodatkowe informacje

W aplikacji wykorzystano bazę danych MSSQL, której struktura została przedstawiona [tutaj](https://drive.google.com/file/d/1x4gN9onQD901x2pOWjNzMSTfUaJJHAln/view?usp=sharing).  
W ramach projektu zostało wykorzystane narzędzie do generowania dokumentacji kodu, Doxygen - [dokumentacja](https://czuczen.github.io/ChilloutRoomDokumentacja).  
