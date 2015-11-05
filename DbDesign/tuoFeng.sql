if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Comment') and o.name = 'FK_COMMENT_REFERENCE_USER')
alter table Comment
   drop constraint FK_COMMENT_REFERENCE_USER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Comment') and o.name = 'FK_COMMENT_REFERENCE_USER')
alter table Comment
   drop constraint FK_COMMENT_REFERENCE_USER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Comment') and o.name = 'FK_COMMENT_REFERENCE_TRAVELPA')
alter table Comment
   drop constraint FK_COMMENT_REFERENCE_TRAVELPA
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Thumb') and o.name = 'FK_THUMB_REFERENCE_USER')
alter table Thumb
   drop constraint FK_THUMB_REFERENCE_USER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Thumb') and o.name = 'FK_THUMB_REFERENCE_TRAVELPA')
alter table Thumb
   drop constraint FK_THUMB_REFERENCE_TRAVELPA
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TravelParts') and o.name = 'FK_TRAVELPA_REFERENCE_TRAVELS')
alter table TravelParts
   drop constraint FK_TRAVELPA_REFERENCE_TRAVELS
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TravelParts') and o.name = 'FK_TRAVELPA_REFERENCE_USER')
alter table TravelParts
   drop constraint FK_TRAVELPA_REFERENCE_USER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Travels') and o.name = 'FK_TRAVELS_TRAVELS_U_USER')
alter table Travels
   drop constraint FK_TRAVELS_TRAVELS_U_USER
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Comment')
            and   type = 'U')
   drop table Comment
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Thumb')
            and   type = 'U')
   drop table Thumb
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TravelParts')
            and   type = 'U')
   drop table TravelParts
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Travels')
            and   type = 'U')
   drop table Travels
go

if exists (select 1
            from  sysobjects
           where  id = object_id('"User"')
            and   type = 'U')
   drop table "User"
go

if exists (select 1
            from  sysobjects
           where  id = object_id('UserRelate')
            and   type = 'U')
   drop table UserRelate
go

/*==============================================================*/
/* Table: Comment                                               */
/*==============================================================*/
create table Comment (
   Id                   bigint               identity,
   TravelPartId         int                  not null,
   UserId               int                  not null,
   Content              nvarchar(200)        not null,
   ToUserId             int                  null,
   CreateTime           datetime             not null,
   constraint PK_COMMENT primary key (Id)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '回复某人，为空的时候是回复主贴；不为空的时候是回复某个人的评论',
   'user', @CurrentUser, 'table', 'Comment', 'column', 'ToUserId'
go

/*==============================================================*/
/* Table: Thumb                                                 */
/*==============================================================*/
create table Thumb (
   Id                   bigint               identity,
   TravelPartId         int                  not null,
   UserId               int                  not null,
   IsDelete             bit                  not null,
   CreateTime           datetime             not null,
   constraint PK_THUMB primary key (Id)
)
go

/*==============================================================*/
/* Table: TravelParts                                           */
/*==============================================================*/
create table TravelParts (
   Id                   int                  identity,
   TravelId             int                  null,
   UserId               int                  not null,
   PartType             tinyint              not null,
   Description          varchar(max)         null,
   PartUrl              varchar(100)         null,
   Longitude            numeric(10,6)        null,
   Latitude             numeric(10,6)        null,
   Height               numeric(10,6)        null,
   Area                 nvarchar(100)        null,
   CreateTime           datetime             not null,
   IsDelete             bit                  not null,
   constraint PK_TRAVELPARTS primary key (Id)
)
go

/*==============================================================*/
/* Table: Travels                                               */
/*==============================================================*/
create table Travels (
   Id                   int                  identity,
   UserId               int                  not null,
   TravelName           varchar(50)          not null,
   CreateTime           datetime             not null,
   UpdateTime           datetime             null,
   CoverImage           varchar(100)         null,
   IsDelete             bit                  not null,
   constraint PK_TRAVELS primary key (Id)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '封面图',
   'user', @CurrentUser, 'table', 'Travels', 'column', 'CoverImage'
go

/*==============================================================*/
/* Table: "User"                                                */
/*==============================================================*/
create table "User" (
   Id                   int                  identity,
   UserName             varchar(100)         not null,
   ShowName             varchar(100)         null,
   PassWord             varchar(20)          null,
   Sex                  bit                  null,
   PhoneNum             varchar(20)          null,
   Email                varchar(50)          null,
   Age                  int                  null,
   ImgUrl               varchar(Max)         null,
   IsEnable             bit                  not null,
   CreateTime           datetime             not null,
   UpdateTime           datetime             not null,
   constraint PK_USER primary key (Id)
)
go

/*==============================================================*/
/* Table: UserRelate                                            */
/*==============================================================*/
create table UserRelate (
   Id                   int                  identity,
   UserId               int                  null,
   RelateUserId         int                  null,
   IsDelete             bit                  null,
   CreateTime           datetime             null,
   constraint PK_USERRELATE primary key (Id)
)
go

alter table Comment
   add constraint FK_COMMENT_REFERENCE_USER1 foreign key (ToUserId)
      references "User" (Id)
go

alter table Comment
   add constraint FK_COMMENT_REFERENCE_USER foreign key (UserId)
      references "User" (Id)
go

alter table Comment
   add constraint FK_COMMENT_REFERENCE_TRAVELPA foreign key (TravelPartId)
      references TravelParts (Id)
go

alter table Thumb
   add constraint FK_THUMB_REFERENCE_USER foreign key (UserId)
      references "User" (Id)
go

alter table Thumb
   add constraint FK_THUMB_REFERENCE_TRAVELPA foreign key (TravelPartId)
      references TravelParts (Id)
go

alter table TravelParts
   add constraint FK_TRAVELPA_REFERENCE_TRAVELS foreign key (TravelId)
      references Travels (Id)
go

alter table TravelParts
   add constraint FK_TRAVELPA_REFERENCE_USER foreign key (UserId)
      references "User" (Id)
go

alter table Travels
   add constraint FK_TRAVELS_TRAVELS_U_USER foreign key (UserId)
      references "User" (Id)
go
