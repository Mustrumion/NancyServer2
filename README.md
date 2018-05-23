# NancyServer2
WebSerwis do Hobby Hunter, projekt do VS 2015 w C#, jako główny framework użyto Nancy.

Aktualnie używa SQL Server jako motora bazy danych. Pozwoli to na proste postawienie na Azure, gdybyśmy chcieli.



Tworzenie serwera:

1. Zainstalować SQL Express 2017 https://www.microsoft.com/pl-pl/sql-server/sql-server-editions-express


2. Utworzyć jakąś bazę, poniżej opis z użyciem Microsoft SQL Server Management Studio 2017 (MSSMS):
    File -> Connect Object Explorer
    Powienien być wypełniony domyślnymi danymi do połączenia zaufanego (server type: DatabaseEngine, server name: nazwa nadana podczas instalacji, Aunthentication: Windows Authentication)
    W 'Object Explorer' po prawej stronie, prawy na 'Databases' -> 'New Database'
    W nazwie wpisać "HobbyHunter" i nacisnąć OK
    

3. Wykonać na niej skrypt CreateTables.sql
    Otworzyć MSSMS 2017
    W Object Explorer rozwinąć Databases
    Prawym na utworzonej w 2. bazie danych -> 'New Query'
    Wkleić zawartość CreateTables.sql, nacisnąć na F5 lub Execute

4. Wpisać dane połączeniowe do bazy w Program.cs
    Zawartość, która jest domyślnie wpisana w MSSMS gdy klikniesz Connect powinna się znaleźć w polu Server
    Reszta powinna pozostać bez zmian, jeśli postępowano według instrukcji/wybierano opcje domyślne 

