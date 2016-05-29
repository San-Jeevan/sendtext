CREATE TABLE [dbo].[Member] (
    [Id]                   INT            IDENTITY (1, 1) NOT NULL,
    [Email]                VARCHAR (256) NOT NULL UNIQUE,
    [PasswordHash]         NVARCHAR (MAX) NULL,
    [PasswordSalt]         NVARCHAR (MAX) NULL,
    [SecurityStamp]        NVARCHAR (MAX) NULL,
    [PhoneNumber]          VARCHAR (15)   NULL,
	[CountryCode]          VARCHAR (256)           NOT NULL,
	[FCMId]                VARCHAR (MAX) NULL,
	[APNId]                VARCHAR (MAX) NULL,
    [TwoFactorEnabled]     BIT            NOT NULL,
    [LockoutEndDateUtc]    DATETIME       NULL,
    [LockoutEnabled]       BIT            NOT NULL,
    [AccessFailedCount]    INT            NOT NULL,
    [UserName]             NVARCHAR (256) NOT NULL UNIQUE,
	[LastLoggedin]         DATETIME       NULL,
	[ForgotPwHash]         VARCHAR(128)   NULL,
	[ForgotPwCreated]      DATETIME       NULL,
    CONSTRAINT [PK_dbo.Member] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
CREATE UNIQUE NONCLUSTERED INDEX [MemberNameIndex]
    ON [dbo].[Member]([UserName] ASC);

CREATE UNIQUE NONCLUSTERED INDEX [MemberPhoneIndex]
    ON [dbo].[Member]([PhoneNumber] ASC);


CREATE TABLE [dbo].[Session] (
    [Id]                   INT            IDENTITY (1, 1) NOT NULL,
    [SignalRRoom]          VARCHAR (256) NOT NULL,
    [ShortName]            VARCHAR (256) NULL UNIQUE,
	[LastActive]           DATETIME       NULL,
    CONSTRAINT [PK_dbo.Session] PRIMARY KEY CLUSTERED ([Id] ASC)
);


CREATE UNIQUE NONCLUSTERED INDEX [SessionShortNameIndex]
    ON [dbo].[Session]([ShortName] ASC);


