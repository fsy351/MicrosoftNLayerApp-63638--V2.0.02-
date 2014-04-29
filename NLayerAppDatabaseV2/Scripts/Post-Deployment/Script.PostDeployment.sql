/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

use NLayerAppV2;

DELETE  FROM [BankAccountActivities];
DELETE  FROM [BankAccounts];
DELETE  FROM [Books];
DELETE FROM [Softwares];
DELETE FROM [OrderLines];
DELETE FROM [Orders];
DELETE FROM [Products];
DELETE FROM [Customers];
DELETE FROM [Countries];
GO

/* COUNTRIES */
INSERT INTO dbo.Countries values('32BB805F-40A4-4C37-AA96-B7945C8C385C','Spain','ES');
INSERT INTO dbo.Countries values('C3C82D06-6A07-41FB-B7EA-903EC456BFC5','U.S.','US');
INSERT INTO dbo.Countries values('8B7E39EE-2420-43FD-8CC6-3F0B216D30E2','U.K.','GB');
INSERT INTO dbo.Countries values('C35EE758-3407-408C-951C-1731F8E420AB','Canada','CA');
INSERT INTO dbo.Countries values('0FAD3B34-9137-4084-A425-49D43E3A2069','Italy','IT');
INSERT INTO dbo.Countries values('3980C628-173E-4672-A609-55FBDF677221','Germany','DE');
INSERT INTO dbo.Countries values('8B9D2B4B-2E9D-C2C6-04DE-08CE0304EF51','France','FR');
INSERT INTO dbo.Countries values('5C0C15A7-A29E-4997-B876-A2F5C3E7D864','Argentina','AR');
INSERT INTO dbo.Countries values('864CB0BC-D964-4B78-A17C-A9BDFC6287E4','Russian Federation','CN');
INSERT INTO dbo.Countries values('781F1F62-373B-4C7F-B666-BC3B97378A11','Israel','CN');
INSERT INTO dbo.Countries values('372B95B9-EFAC-4BC2-8C2D-E907B2BC9465','China','CN');

/* CUSTOMERS */
INSERT INTO [dbo].Customers Values('1D814049-F2CD-4E58-8559-188286254502','Cesar', 'De la Torre', 'De la Torre, Cesar', '+34 123456789', 'Microsoft', 9000, 1, '32BB805F-40A4-4C37-AA96-B7945C8C385C','Madrid','28081', 'Parque Empresarial La Finca',null,null);    
INSERT INTO [dbo].Customers Values('703F679A-8597-4705-9B4E-32E4C6A75C17','Unai',  'Zorrilla',    'Zorrilla, Unai', '+34 123456789', 'Plain Concepts', 9000, 1, '32BB805F-40A4-4C37-AA96-B7945C8C385C','Madrid','28081', 'Parque Empresarial La Finca',null,null);
INSERT INTO [dbo].Customers Values('9FA6306C-10CB-4DC3-B4F5-56211046DC68','Miguel Angel', 'Ramos', 'Ramos, Miguel Angel', '+34 123456789', 'Microsoft', 9000, 1, '32BB805F-40A4-4C37-AA96-B7945C8C385C','Madrid','28081', 'Parque Empresarial La Finca',null,null);
INSERT INTO [dbo].Customers Values('B5C0255E-8491-47EC-8251-6EE4993F5C81','Pierre', 'Milet', 'Milet, Pierre', '+34 123456789', 'Microsoft', 9000, 1, '32BB805F-40A4-4C37-AA96-B7945C8C385C','Madrid','28081', 'Parque Empresarial de La Finca',null,null);
INSERT INTO [dbo].Customers Values('0506FCB6-72E7-44C6-81F7-8EEDB52E785D','Ricardo', 'Minguez', 'Minguez, Ricardo', '+34 123456789', 'Microsoft', 9000, 1, '32BB805F-40A4-4C37-AA96-B7945C8C385C','Madrid','28081', 'Parque Empresarial de La Finca',null,null);
INSERT INTO [dbo].Customers Values('DB578C34-4F48-49D0-95D0-CD79E342B9F1','Morteza', 'Manavi', 'Manavi, Morteza', '+34 123456789', 'Canada', 9000, 1, '32BB805F-40A4-4C37-AA96-B7945C8C385C','Malaga','28081', 'Dev. Street',null,null);
INSERT INTO [dbo].Customers Values('B18146F4-8195-4F16-B39A-98DA73968000','Roberto', 'Gonzalez', 'Gonzalez, Roberto', '+34 123456789', 'Renacimiento', 9000, 1, '32BB805F-40A4-4C37-AA96-B7945C8C385C','Madrid','28081', 'Parque Empresarial La Finca',null,null);
INSERT INTO [dbo].Customers Values('E4CE6192-841D-4440-B489-794C4143E52B','Cristian', 'Manteiga', 'Manteiga, Cristian', '+34 123456789', 'Plain Concepts', 9000, 1, '32BB805F-40A4-4C37-AA96-B7945C8C385C','Madrid','28081', 'Other Street',null,null);
INSERT INTO [dbo].Customers Values('553172EE-2399-4FB7-8A45-FBD88D4B1A6B','Megan', 'Fox', 'Fox, Megan', '+1 999999999', 'Hollywood', 9000, 1, 'C3C82D06-6A07-41FB-B7EA-903EC456BFC5','Madrid','28081', 'Fame Street',null,null);

/* BANK ACCOUNTS */
INSERT INTO [dbo].[BankAccounts] values('815823DE-B9BD-C141-AD32-08CE08B761CF','2123','2375','1276549876','01','ES01 2375 2123 011276549876',1000,0,'1D814049-F2CD-4E58-8559-188286254502');
INSERT INTO [dbo].[BankAccounts] values('76A8DF86-C2FB-CB7D-01E1-08CE08B76218','2111','3456','9876549879','02','ES02 3456 2111 029876549879',1000,0,'703F679A-8597-4705-9B4E-32E4C6A75C17');

/* PRODUCTS */
INSERT INTO dbo.Products VALUES('9D49E192-9DAE-C66F-F1B0-08CE03A3B87A', 'Domain-Driven Design: Tackling Complexity in the Heart of Software', 'Domain-Driven Design', 50, 8);
INSERT INTO dbo.Products VALUES('D7E5C537-6A0C-4E19-B41E-3653F4998085', 'Focuses on typical DDD patterns and their implementation using .NET 4.0', 'Domain Oriented N-Layered Architectures', 35, 15);
INSERT INTO dbo.Products VALUES('44668EBF-7B54-4431-8D61-C1298DB50857', 'Book about Entity Framework 4.1 and Code-First', 'Entity Framework 4.1', 30, 5);

INSERT INTO dbo.Products VALUES('A6BC5933-0AEF-4054-B321-8B14DA606ABB', 'Microsoft IDE Development Tool for .NET', 'Visual Studio 2010', 800, 200);
INSERT INTO dbo.Products VALUES('6FEE08D7-FF17-4192-B8B8-6860CE194B1D', 'The best Windows ever', 'Windows 8', 200, 99);
INSERT INTO dbo.Products VALUES('3187625A-9702-4B4A-810F-E46629C9D94F', 'Server Operating System', 'Windows Server 2008 R2', 400, 200);


/* BOOKS: SPECIFIC PRODUCTS */
INSERT INTO dbo.Books VALUES('9D49E192-9DAE-C66F-F1B0-08CE03A3B87A', 'E.E. Press', '1234567809');
INSERT INTO dbo.Books VALUES('D7E5C537-6A0C-4E19-B41E-3653F4998085', 'Krasis Press', '999999999');
INSERT INTO dbo.Books VALUES('44668EBF-7B54-4431-8D61-C1298DB50857', 'Krasis Press', '123123123123');

/* SOFTWARE: SPECIFIC PRODUCTS */
INSERT INTO dbo.Softwares VALUES('A6BC5933-0AEF-4054-B321-8B14DA606ABB', 'P-SW-AAAAA');
INSERT INTO dbo.Softwares VALUES('6FEE08D7-FF17-4192-B8B8-6860CE194B1D', 'P-SW-BBBBB');
INSERT INTO dbo.Softwares VALUES('3187625A-9702-4B4A-810F-E46629C9D94F', 'P-SW-CCCCC');


/*ORDERS*/
INSERT INTO dbo.Orders VALUES('E2D77904-C595-C346-B0E5-08CE03A3B828', '2011-05-28 14:12:01.007','2011-05-15 09:00:01.007',1, '1D814049-F2CD-4E58-8559-188286254502', 'My Home Address', 'My homeStreet', 'Madrid', '28700');
INSERT INTO dbo.Orders VALUES('DE0C5F9F-2009-CA3B-6CD9-08CE03A3B8E0', '2011-05-15 15:12:01.000',NULL,0, '1D814049-F2CD-4E58-8559-188286254502', 'My office', 'My office St.', 'Madrid', '28043');
INSERT INTO dbo.Orders VALUES('3135513C-63FD-43E6-9697-6C6E5D8CE55B', '2011-05-11 17:12:01.000',NULL,0, '703F679A-8597-4705-9B4E-32E4C6A75C17', 'Home Addr', 'My office St.', 'Madrid', '28999');

/*ORDERS LINES*/
INSERT INTO dbo.OrderLines VALUES('3A242385-7AC5-4D2C-81EF-88249C2E546E', 45, 1, 10, 'E2D77904-C595-C346-B0E5-08CE03A3B828', '9D49E192-9DAE-C66F-F1B0-08CE03A3B87A');
INSERT INTO dbo.OrderLines VALUES('A4CE721F-3329-4114-982F-488379890296', 31, 1, 15, 'E2D77904-C595-C346-B0E5-08CE03A3B828', 'D7E5C537-6A0C-4E19-B41E-3653F4998085');
INSERT INTO dbo.OrderLines VALUES('36EFE52E-046C-4F01-8327-F8CCB76EF52E', 180, 1, 20, 'E2D77904-C595-C346-B0E5-08CE03A3B828', 'A6BC5933-0AEF-4054-B321-8B14DA606ABB');

INSERT INTO dbo.OrderLines VALUES('DC1F72F8-AAE6-4BA5-B4DB-BE2BB85AF3CA', 33, 1, 10, 'DE0C5F9F-2009-CA3B-6CD9-08CE03A3B8E0', '6FEE08D7-FF17-4192-B8B8-6860CE194B1D');
INSERT INTO dbo.OrderLines VALUES('CAA1DE89-C38B-4235-9B01-72F9F05DA8F2', 190, 1, 10, 'DE0C5F9F-2009-CA3B-6CD9-08CE03A3B8E0', 'A6BC5933-0AEF-4054-B321-8B14DA606ABB');

INSERT INTO dbo.OrderLines VALUES('F39FA836-1626-4635-B611-87E7BDAE435B', 45, 1, 10, '3135513C-63FD-43E6-9697-6C6E5D8CE55B', '9D49E192-9DAE-C66F-F1B0-08CE03A3B87A');