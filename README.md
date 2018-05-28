# NancyServer2
WebSerwis do Hobby Hunter, projekt do VS 2015 w C#, jako główny framework użyto Nancy.

Aktualnie używa SQL Server jako motora bazy danych. Pozwoli to na proste postawienie na Azure, gdybyśmy chcieli.



Tworzenie serwera:

1. Zainstalować SQL Express 2017 https://www.microsoft.com/pl-pl/sql-server/sql-server-editions-express


2. Utworzyć bazę HobbyHunter, poniżej opis z użyciem Microsoft SQL Server Management Studio 2017 (MSSMS):
    File -> Connect Object Explorer
    Powienien być wypełniony domyślnymi danymi do połączenia zaufanego (server type: DatabaseEngine, server name: nazwa nadana podczas instalacji, Aunthentication: Windows Authentication)
    W 'Object Explorer' po prawej stronie, prawy na 'Databases' -> 'New Database'
    W nazwie wpisać "HobbyHunter" i nacisnąć OK
    

3. Włączyć filestream na bazie:
	Uruchomić SQLServerCOnfigurationManager - może nie być w starcie pod tą nazwą - możliwe nazwy exeka:
		SQLServerManager14.msc for [SQL Server 2017] or
		SQLServerManager13.msc for [SQL Server 2016] or
		SQLServerManager12.msc for [SQL Server 2014] or
		SQLServerManager11.msc for [SQL Server 2012] or
		SQLServerManager10.msc for [SQL Server 2008], 
	Przejść do SQL Server Services
	Prawy na SQLServer -> Properties
	Zakładka Filestream
	Zaznaczyć wszystko i zapisać
	Uruchomić ponownie serwer

4. Wpisać dane połączeniowe do bazy w Program.cs
    Zawartość, która jest domyślnie wpisana w MSSMS gdy klikniesz Connect powinna się znaleźć w polu Server
    Reszta powinna pozostać bez zmian, jeśli postępowano według instrukcji/wybierano opcje domyślne 

