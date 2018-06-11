# NancyServer2
WebSerwis do Hobby Hunter, projekt do VS 2015 w C#, jako główny framework użyto Nancy.

Aktualnie używa SQL Server jako motora bazy danych. Pozwoli to na proste postawienie na Azure, gdybyśmy chcieli.



Tworzenie serwera for dumdums:

1. Zainstalować SQL Express 2017 https://www.microsoft.com/pl-pl/sql-server/sql-server-editions-express (inny SQLServer / SQLExpress również zadziała). W wersji 2017 można przeklikać isntalację.


2. Utworzyć bazę HobbyHunter, poniżej opis z użyciem Microsoft SQL Server Management Studio 2017 (MSSMS):
    
    1. File -> Connect Object Explorer
    2. Powienien być wypełniony domyślnymi danymi do połączenia zaufanego (server type: DatabaseEngine, server name: nazwa nadana podczas instalacji, Aunthentication: Windows Authentication)
    3. W 'Object Explorer' po prawej stronie, prawy na 'Databases' -> 'New Database'
    4. W nazwie wpisać "HobbyHunter" i nacisnąć OK.
    
3. Wpisać dane połączeniowe do bazy w Program.cs serwera
    Zawartość, która jest domyślnie wpisana w polu Instance w MSSMS gdy klika się Connect powinna się znaleźć w polu Server
    Nazwa bazy danych utworzonej w pkt2. powinna znaleźć się w polu Database.
    Reszta powinna pozostać bez zmian, jeśli postępowano według instrukcji/wybierano opcje domyślne 

4. Uruchomić program, wpisać deploy w konsoli, aby zainicjować tabele. Tworzony jest również też domyślny użytkownik testowy o email'u user@user.user z hasłem useruser.
