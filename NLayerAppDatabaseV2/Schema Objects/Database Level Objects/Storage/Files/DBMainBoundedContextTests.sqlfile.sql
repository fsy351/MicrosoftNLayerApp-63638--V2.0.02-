﻿ALTER DATABASE [$(DatabaseName)]
    ADD FILE (NAME = [NLayerAppV2], FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\NLayerAppV2.mdf', SIZE = 2304 KB, FILEGROWTH = 1024 KB) TO FILEGROUP [PRIMARY];



