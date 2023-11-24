create database pruebaUSER1

USE pruebaUSER1

Create table usuarios(
IdUsuario int primary key identity,
NombreUsuario varchar(50) not null,
Contraseña varchar(50) not null
)

sp_addlogin 'alex2','Alejandro2023*','pruebaUSER1'
sp_addsrvrolemember 'alex2','sysadmin'
use pruebaUSER1
sp_adduser 'alex2','Propietario'
sp_addrolemember 'db_ddladmin','Propietario'

select *from usuarios

use master
go
select name from sys.databases where name = 'pruebaUSER1';

USE pruebaUSER1;
GO
CREATE USER reyna23 FOR LOGIN reyna23;
ALTER ROLE db_datareader ADD MEMBER reyna23;
ALTER ROLE db_datawriter ADD MEMBER reyna23;

use master
sp_addlogin 'alexMaster','Alejandro2023*','master'
sp_addsrvrolemember 'alexMaster','sysadmin'
use master
sp_adduser 'alexMaster','Propietario'
sp_addrolemember 'db_ddladmin','Propietario'

USE master;
GO
SELECT name, is_disabled FROM sys.server_principals WHERE name = 'reyna23';