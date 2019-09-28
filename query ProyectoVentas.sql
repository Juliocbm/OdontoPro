create table usuario(
id_usuario int not null identity(1,1) primary key,
nombre varchar(50) not null,
usuario varchar(50) not null,
contraseña varchar(50) not null
)
go

insert into usuario
values('Raul alejandro','rabm500','12345678')

create procedure insertarUsuario
@nombre varchar(50),
@usuario varchar(50),
@contraseña varchar(50)
as
begin
if EXISTS(select usuario from usuario where usuario=@usuario)
raiserror('El usuario ya existe',16,1)
else
insert into usuario(nombre,usuario,contraseña)
values(@nombre,@usuario,@contraseña)
end

exec insertarUsuario 'mariana','admin','123'

create table permisos(
id_permisos int identity(1,1) primary key not null,
vender int,
aplicar_descuentos int,
devoluciones int,
cobros int,
pagar int,
clientes_proveedores int,
configuracion int,
usuarios int,
reportes int,
id_usuario int,
FOREIGN KEY (id_usuario) REFERENCES usuario(id_usuario)
)
go

insert into permisos(vender,aplicar_descuentos,devoluciones,cobros,pagar,clientes_proveedores,configuracion,usuarios,reportes,id_usuario)
values(1,0,1,1,0,1,1,0,1,3)

delete from usuario 

create proc insertarPermisos
@vender int,
@aplicar_descuentos int,
@devoluciones int,
@cobros int,
@pagar int,
@clientes_proveedores int,
@configuracion int,
@usuarios int,
@reportes int,
@id_usuario int
as
begin
insert into permisos
values(
@vender,
@aplicar_descuentos,
@devoluciones,
@cobros,
@pagar,
@clientes_proveedores,
@configuracion,
@usuarios,
@reportes,
@id_usuario
)
end


create proc mostrarUsuarios
as
begin
select u.id_usuario, u.nombre, u.usuario, p.*
from usuario as u 
inner join permisos as p on(u.id_usuario = p.id_usuario)
end

exec mostrarUsuarios

create procedure editarUsuario
@id_usuario int,
@nombre varchar(50),
@usuario varchar(50),
@contraseña varchar(50)
as
begin
UPDATE usuario
SET nombre = @nombre, usuario=@usuario, contraseña=@contraseña
WHERE id_usuario =@id_usuario;
end


create procedure editarPermisos
@vender int,
@aplicar_descuentos int,
@devoluciones int,
@cobros int,
@pagar int,
@clientes_proveedores int,
@configuracion int,
@usuarios int,
@reportes int,
@id_usuario int
as
begin
UPDATE permisos
SET vender=@vender,
aplicar_descuentos=@aplicar_descuentos,
devoluciones=@devoluciones,
cobros=@cobros,
pagar=@pagar,
clientes_proveedores=@clientes_proveedores,
configuracion=@configuracion,
usuarios=@usuarios,
reportes=@reportes
WHERE id_usuario =@id_usuario;
end


create proc buscarPorUsuarioPorNombre
@letra varchar(50)
as
begin
select u.id_usuario, u.nombre, u.usuario, p.*
from usuario as u 
inner join permisos as p on(u.id_usuario = p.id_usuario)
where u.nombre+u.usuario like '%'+@letra+'%'
end

create proc eliminarUsuario
@id_usuario int
as
begin
if (select u.usuario from usuario as u where id_usuario=@id_usuario)='admin'
raiserror('No puedes eliminar al administrador',16,1)
else
delete from usuario where id_usuario=@id_usuario and usuario<>'admin'
end

exec eliminarUsuario 4
