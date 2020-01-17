create database VideoDatabase
drop database VideoDatabase

use VideoDatabase

create table UserInfo(
	userID int check (userID > -1), 
	isCreator int not null default 0, --by default 0 (not a creator)
	userName nvarchar(30) not null, --cannot be null
	userEmail nvarchar(320) not null, --cannot be null
	dateOfBirth date,
	joinDate date not null default getdate(),
	loginstatus int not null default 0, --0 = not logged in, 1 = logged in
	[password] nvarchar(16) not null, 
	primary key (userID),
)

create table  Video
( 
	videoID int check (videoID > -1),
	uploaderID int not null,
	likes int not null default 0,
	[views] int not null default 0,
	dislikes int not null default 0,
	primary key(videoID),
	foreign key(uploaderID) references UserInfo(userID) on update cascade on delete cascade,
)

create table VideoDescription
(
	vID int primary key foreign key references Video(videoID) on update cascade on delete cascade,
	vDesc nvarchar(200) default null
)

create table Categories
(
	cID int primary key,
	cDescription nvarchar(20) not null unique
)

insert into Categories values
(0,'No Category'),
(1,'Action'),
(2,'Adventure'),
(3,'Comedy'),
(4,'Drama'),
(5,'Educational')

create table VideoGenre
(
	vID int foreign key references Video(videoID) on update cascade on delete cascade,
	catID int foreign key references Categories(cID) on update cascade on delete set null

)



create table Creators(
	creatorID int,
	totalSubcribers int not null default 0,
	uploadedVideos int not null default 0,
	creationDate date not null default getdate(),
	primary key (creatorID),
	foreign key(creatorID) references UserInfo(userID) on update cascade on delete cascade,
)

create table Subscribers(
	subscriberID int,
	subscribedTo int,
	subDate date not null,
	Primary key(subscriberID,subscribedTo),
	foreign key(subscriberID) references userInfo(UserID) on update no action on delete no action,
	foreign key(subscribedTo) references userInfo(UserID) on update no action on delete no action
)




create trigger UserDeleted on UserInfo
for delete
as
begin
declare @uID int
select @uID = userID from deleted

delete from Subscribers where @uID = subscriberID

end
go

--Sign up procedure
create procedure SignUp --conditions in values checked on front end
@UN nvarchar(30),
@pass nvarchar(16),
@email nvarchar(320),
@DOB date,
@status int output
as
begin
declare @uID int
set @status = 0


if @UN in (select userName from UserInfo)
	begin
	print ('Username already exists')
	end
else
	begin
	if @email in (select userEmail from UserInfo)
		begin
		print('Email already exists')
		end
	else
		begin
		select @uID = max(userID) from UserInfo
		set @uID = @uID + 1
		insert into UserInfo (userID,userName,userEmail,[password],dateOfBirth) values (@uID,@UN,@email,@pass,@DOB)
		set @status = 1
		end
	end

end
go

--login procedure
create procedure [Login]
@Input_UN nvarchar(30),
@Input_Pass nvarchar(16),
@Status int output
as
begin
if @Input_UN in (select userName from UserInfo)
	begin
	if @Input_Pass in (select [password] from UserInfo)
		begin
		update UserInfo set loginstatus = 1 where userName = @Input_UN and @Input_Pass = [password]
		print('Login successful!')
		set @Status = 1
		end
	else
		begin
		print('Invalid Password')
		set @Status = 0
		end
	end
else
	begin
	print('Username does not exist')
	set @Status = 0
	end

end
go

--Log off
create procedure Logoff
@Input_UN int,
@status int output
as
begin
	if 1 in (select loginstatus from UserInfo where @Input_UN = userName)
	begin
		update UserInfo set loginstatus = 0 where userName = @Input_UN
		set @status = 1;
	end
	else
	begin
	set @status = 0
	end
end
go



--Top video
create procedure TopVideo
@searchcriteria int, --1 for likes, 2 for views, 3 for dislikes. make checkboxes on frontend for this
@searchCategory int --look in category table for corresponding IDs					
as
begin
	if @searchcriteria = 1 --likes
	begin
		select *
		from Video
		where likes = (select max(V.likes)
						from Video as V join VideoGenre as VG on V.videoID = VG.vID
						where @searchCategory = VG.catID )
		
	end
	else if @searchcriteria = 2 --views
	begin
		select *
		from Video
		where [views] = (select max(V.[views])
						from Video as V join VideoGenre as VG on V.videoID = VG.vID
						where @searchCategory = VG.catID )
	end
	else if @searchcriteria = 3 --dislikes
	begin
		select *
		from Video
		where dislikes = (select max(V.dislikes)
						from Video as V join VideoGenre as VG on V.videoID = VG.vID
						where @searchCategory = VG.catID )
							
	end

end
go

--Delete Account
create procedure DeleteAccount --by default, a logged in user will be able to see this functionality, but it should ask for their password as well before deleting
@uID int,
@pass nvarchar(16)
as
begin
	if @pass = (select [password] from UserInfo where @uID = userID)
	begin
		delete from UserInfo where userID = @uID
	end
	else
	begin
		print('Invalid password')
	end
end
go

--LOGIN STATUS--
create procedure checklogin
	@UserID_param int,
	@flag int output
as
begin

if @UserID_param in ( select UserInfo.userID from UserInfo)
	begin
	if (select UserInfo.loginstatus from UserInfo where @UserID_param=UserInfo.userID )=1
		begin
		set @flag =0
		end
	else
		begin
		set @flag=-1
		end
	end
else
	begin
	set @flag=-1
	end

end
go

--SUBSCRIPTION UPDATE--
create procedure subscriptionUpdate
	@subsID_param int,
	@substoId_param int
as
begin 

declare @flag1 int
declare @flag2 int

execute checklogin 
@UserID_param=@subsID_param,
@flag=@flag1 output

execute checklogin 
@UserID_param=@substoID_param,
@flag=@flag2 output


declare @isCreatorbool int

set @isCreatorbool = (select isCreator from userInfo where UserID=@substoID_param)

if @flag1 =0 and @flag2 =0 and @isCreatorbool=1
	begin
	insert into Subscribers
		values (@subsID_param,@substoId_param,getdate())

	print N'You have successfully subscribed'
	end
else
	begin
	print N'subscription unsuccessfull'
	end

end
go

--LIKING RATIO--
create procedure videoLikingRatio
	@videoID_param int
as
begin

declare @data int

set @data=(select (likes * 100)/(likes+dislikes) from Video where @videoID_param=videoID)

if @data !=0
	begin
	select @data as percentage
	end
else
	begin
	print N'video does not exist!'
	end

end
go

--OLDEST CREATORS--
create procedure oldviewers
	@date_param date
as
begin

if @date_param < getdate()
	begin
	select * from Creators where creationDate<@date_param
	end
else
	begin
	print N'date is invalid'
	end

end
go

--Most Liked Channel Owners--
create procedure mostlikedchannelowners
	@count int
as
begin
	select top (@count) UI.userName
	from UserInfo as UI join Creators as C on UI.userID = C.creatorID
	group by C.creatorID, UI.userName, c.totalSubcribers
	order by c.totalSubcribers desc
end

go



--Video Removing --
create procedure videoTakedown
@videoIDtoremove int,
@myvar int = 0 -- not to be input, for use in procedure
as
begin
	set @myvar =
	(select Video.uploaderID
	from Video
	where Video.videoID = @videoIDtoremove
	)
	if((select UserInfo.isCreator
		from UserInfo
		where UserInfo.userID = @myvar) = 1
		AND
		(select UserInfo.loginstatus 
		from UserInfo
		where UserInfo.userID = @myvar) = 1
		)
	Begin
		delete from Video
		where Video.videoID = @videoIDtoremove
		print 'Video removed successfully'
	End
	Else
	Begin
		print 'You are either logged out, or you do not have the privelege to delete this video'
	End
end
go


-- Adding description--
create procedure addingdescription
@inputdescription nvarchar(200),
@inputvideoID int,
@myvar int = 0 -- not to be input, for use in procedure
as
Begin
	set @myvar =
	(select Video.uploaderID
	from Video
	where Video.videoID = @inputvideoID
	)
	if((select UserInfo.isCreator
		from UserInfo
		where UserInfo.userID = @myvar) = 1
		AND
		(select UserInfo.loginstatus 
		from UserInfo
		where UserInfo.userID = @myvar) = 1
		)
	Begin
		update VideoDescription
		set vDesc = @inputdescription
		where vID = @inputvideoID
		print 'Your video description has been updated.'
	End
	Else
	Begin
		print 'You are either logged out, or you do not have the privelege to update the description on this video'
	End
End
go



-- Video Upload Notification Generation--
create procedure VideoUploadedNotif
@uploadedvideoID int
as
Begin
	select Subscribers.subscriberID
	from Subscribers
	where Subscribers.subscribedTo in
				(select Video.uploaderID
				from Video
				where Video.videoID = @uploadedvideoID
				)
	--the notification part will be handled on the front end.
	print 'These are the subscribers of the person whose video was just uploaded.'
End
go

