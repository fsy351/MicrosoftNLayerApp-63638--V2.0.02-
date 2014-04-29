ALTER DATABASE [$(DatabaseName)]
    ADD LOG FILE (NAME = [NLayerAppV2_log], FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\NLayerAppV2_log.LDF', SIZE = 576 KB, MAXSIZE = 2097152 MB, FILEGROWTH = 10 %);



