use TodoManager
IF EXISTS (SELECT * FROM sys.objects WHERE name = ('AtualizaHora') AND type IN (N'P'))
BEGIN
    DROP PROCEDURE AtualizaHora;
END
GO
create procedure AtualizaHora
(
    -- Add the parameters for the stored procedure here
    @HoraUtilizada TIME,
    @HoraAntiga TIME,
	@idTarefa int
)
AS
BEGIN
	declare @TempoTotal int;

	if (@HoraAntiga IS NULL)
		set @TempoTotal = DATEDIFF(SECOND, '00:00:00', @HoraUtilizada) + DATEDIFF(SECOND, '00:00:00', '00:00:00');
	else
		set @TempoTotal = DATEDIFF(SECOND, '00:00:00', @HoraUtilizada) + DATEDIFF(SECOND, '00:00:00', @HoraAntiga);
		
update Tarefas SET tempoGasto = DATEADD(SECOND, @TempoTotal, '00:00:00') where idTarefa= @idTarefa
end
GO
update Tarefas set tempoGasto = null
exec AtualizaHora @HoraUtilizada ='10:01', @HoraAntiga=null, @idTarefa=1
select * from Tarefas



create Database TodoManager

/* DROPS
drop table Prioridade
drop table Status
drop table Tarefas
*/

/* ALTERS

ALTER TABLE Tarefas
ADD CONSTRAINT prioridade_FK
FOREIGN KEY (prioridade)
REFERENCES Prioridade (prioridadeID);

---------
ALTER TABLE Tarefas
ADD CONSTRAINT status_FK
FOREIGN KEY (status)
REFERENCES Status (statusID);

*/


create table  Tarefas (
	IdTarefa int IDENTITY(1,1) primary key,
	tarefa varchar(50) not null,
	prioridade int,
	tempo time,
	agendamento DateTime,
	status int,
	tempoGasto TIME NULL
)
select * from Tarefas

INSERT INTO Tarefas (tarefa, prioridade, tempo, agendamento, status) VALUES (
'TESTE',
1,
'10:00',
SYSDATETIME(),
1
)


create view tarefas_view AS
(select IdTarefa,TAREFA,TEMPO,tempoGasto, AGENDAMENTO,descricaoPrioridade AS PRIORIDADE, descricaoStatus AS STATUS from Tarefas T
join Prioridade P on P.prioridadeID = T.prioridade 
join Status S on S.statusID = T.status)

drop view tarefas_view

select * from Tarefas





SELECT * FROM tarefas_view order by agendamento 

create table Prioridade (
prioridadeID int IDENTITY(1,1) primary key,
descricaoPrioridade varchar(50) not null
)

insert into Prioridade values(
'Baixa'
)
insert into Prioridade values(
'Media'
)
insert into Prioridade values(
'Alta'
)

ALTER TABLE Tarefas
ADD CONSTRAINT prioridade_FK
FOREIGN KEY (prioridade)
REFERENCES Prioridade (prioridadeID);

select * from Prioridade


CREATE TABLE Status (
statusID int IDENTITY(1,1) primary key,
descricaoStatus varchar(50) not null
)
insert into Status values(
'Agendada'
)
insert into Status values(
'Em andamento'
)
insert into Status values(
'Em Atraso'
)
insert into Status values(
'Finalizada'
)
 
select * from Status

ALTER TABLE Tarefas
ADD CONSTRAINT status_FK
FOREIGN KEY (status)
REFERENCES Status (statusID);

select * from Status

select * from TodoManager..Tarefas
insert into Tarefas values(
'teste2',
1,
'10:00',
SYSDATETIME(),
1
)

CREATE TABLE timerTarefas (
    idTimer INT PRIMARY KEY IDENTITY(1,1),    
    idTarefa INT,                             
    startTime TIME,                           
    endTime TIME, 
	dtHrResgis datetime,
    CONSTRAINT FK_TimerTarefas_Tarefas FOREIGN KEY (idTarefa) REFERENCES Tarefas(idTarefa)
);
select * from timerTarefas



