create database OdontoPro;

use OdontoPro;

create table paciente(
	paciente_id int primary key not null identity(1,1),
	nombre varchar(50) not null,
	apellidos varchar(100)not null,
	direccion varchar(80), 
	estado varchar(50),
	ciudad varchar(50),
	fecha_nac datetime,
	telefono varchar(10),
	email varchar(50),
	edad int 
)
go

create table tratamiento(
	tratamiento_id int primary key not null identity(1,1),
	diagnostico varchar(356),
	tratamiento varchar(356),
	observaciones varchar(356),
	fecha_inicio datetime,
	paciente_id int,
	foreign key(paciente_id) references paciente(paciente_id) 
)
go

create table nota_evolucion(
	nota_evolucion_id int primary key not null identity(1,1),
	nota varchar(500) not null,
	fecha_evolucion datetime,
	tratamiento_id int,
	foreign key(tratamiento_id) references tratamiento(tratamiento_id)
)
go

create table reservacion
(
	reservacion_id int primary key not  null identity(1,1),
	fecha_reservacion datetime not null,
	paciente_id int,
	foreign key(paciente_id) references paciente(paciente_id)
)
go


/*creacion de procedimientos almacenados*/
create proc mostrar_tratamientos
@paciente_id int
as
begin
select distinct t.tratamiento,t.diagnostico, t.observaciones, p.nombre, p.apellidos, p.ciudad,(select COUNT(*) from nota_evolucion ne where t.tratamiento_id=ne.tratamiento_id) 'numero de notas de evolucion' from paciente p join tratamiento t on(p.paciente_id=t.paciente_id) where t.paciente_id=@paciente_id
end
go

--create proc mostrar_pacientes
--as
--begin
--select distinct p.*,(select count(tratamiento) from tratamiento t2 where t2.paciente_id=p.paciente_id) 'numero de tratamientos' from paciente p join tratamiento t on (p.paciente_id=t.paciente_id)
--end
--go 

create proc mostrar_pacientes
@textNombre varchar(50)
as
begin

if @textNombre=''
select 
	distinct 
	p.*,
	ISNULL((select count(tratamiento) 
	from tratamiento t2 where t2.paciente_id=p.paciente_id) ,0) as 'numero de tratamientos',
	ISNULL((select count(fecha_reservacion) 
	from reservacion r where r.paciente_id=p.paciente_id) ,0) as 'reservas' 
	from paciente p left join tratamiento t on (p.paciente_id=t.paciente_id)
else
select 
	distinct 
	p.*,
	ISNULL((select count(tratamiento) 
	from tratamiento t2 where t2.paciente_id=p.paciente_id) ,0) as 'numero de tratamientos', 
	ISNULL((select count(fecha_reservacion) 
	from reservacion r where r.paciente_id=p.paciente_id) ,0) as 'reservas'
	from paciente p left join tratamiento t on (p.paciente_id=t.paciente_id)
	where p.nombre like '%' + @textNombre + '%' 
end
go 

exec mostrar_pacientes 'julio'

create proc mostrar_notas_evolucion
@tratamiento_id int
as
begin
select distinct ne.nota, ne.fecha_evolucion, t.tratamiento,t.diagnostico, t.observaciones, p.nombre, p.apellidos, p.ciudad from paciente p join tratamiento t on(p.paciente_id=t.paciente_id) join nota_evolucion ne on (t.tratamiento_id=ne.tratamiento_id) where t.tratamiento_id=@tratamiento_id
end
go

create proc mostrar_reservacion
@paciente_id int
as
begin
select * from reservacion where paciente_id=@paciente_id
end
go

create proc mostrar_reservaciones
as
begin
select distinct p.nombre, p.apellidos, p.ciudad, r.fecha_reservacion from reservacion r join paciente p on (p.paciente_id=r.paciente_id)
end
go

create proc insertar_reservacion
@fecha datetime,
@paciente_id int
as
begin
if exists(select * from reservacion r 
join paciente p on (p.paciente_id=r.paciente_id) 
where p.paciente_id=@paciente_id)
raiserror('El usuario ya tiene una reserva',16,9)
else
insert into reservacion
values(@fecha,@paciente_id)
end


create proc insertar_paciente
@nombre varchar(50),
@apellidos varchar(100),
@direccion varchar(80),
@estado varchar(50),
@ciudad varchar(50),
@fecha_nac datetime,
@telefono varchar(10),
@email varchar(50),
@edad int
as
begin
if exists(select * from paciente p  
where p.nombre=@nombre and p.apellidos=@apellidos and p.ciudad=@ciudad)
raiserror('El paciente ya existe',16,9)
else
insert into paciente
values(@nombre,@apellidos,@direccion,@estado,@ciudad,@fecha_nac,@telefono,@email,@edad)
end


create proc insertar_tratamiento
@diagnostico varchar(356),
@tratamiento varchar(356),
@observaciones varchar(356),
@fecha_inicio datetime,
@paciente_id int
as
begin
insert into tratamiento
values(@diagnostico, @tratamiento, @observaciones, @fecha_inicio, @paciente_id)
end

create proc insertar_nota_evolucion
@nota varchar(500),
@fecha_evolucion datetime,
@tratamiento_id int
as
begin
insert into nota_evolucion
values(@nota, @fecha_evolucion, @tratamiento_id)
end


/*probar eliminacion en cascada*/
delete paciente
select * from paciente
select * from nota_evolucion
select * from tratamiento



/*insertar pacientes*/
insert into paciente(nombre, apellidos, direccion, estado, ciudad, fecha_nac, telefono,email, edad)
values('Julio Cesar','Bautista Monsalvo', 'lagos del country #2', 'Nayarit', 'Tepic','24-09-1992','3111160560','juliocbm500gmail.com',26);
go
insert into paciente(nombre, apellidos, direccion, estado, ciudad, fecha_nac, telefono,email, edad)
values('Raul Alejandro','Bautista Monsalvo', 'morelos #189', 'Nayarit', 'Tepic','24-03-1991','3112398534','rabmj7500gmail.com',27);
go

/*insertar tratamientos*/
insert into tratamiento
values('caries en tres dientes','endodoncia','alergico a la anestesia','20-05-2019',1)
go
insert into tratamiento
values('caries en un diente','puente y coronas','llora mucho','25-05-2019',2)
go
insert into tratamiento
values('dolor en muela','extraccion de muela','doble dosis de anestesia','28-05-2019',2)
go

/*insertar notas de evolucion*/
insert into nota_evolucion
values('parece que el dolor se fue permanentemente','29-05-2019',1)
go
insert into nota_evolucion
values('limpieza completa','29-05-2019',2)
go
insert into nota_evolucion
values('segundo mes despues de puesta de brackets','30-05-2019',2)
go

insert into nota_evolucion
values('primer consulta despues de puesta de braquets','27-05-2019',2)
go

/*insertar reservaciones*/
insert into reservacion
values('10-06-2019',1)
go

exec mostrar_reservaciones