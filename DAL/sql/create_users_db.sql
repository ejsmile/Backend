WSELECT 1;
PRAGMA foreign_keys=OFF;
BEGIN TRANSACTION;
CREATE TABLE [AspNetUsers] (
  [Id] nvarchar(128)  NOT NULL
, [Email] nvarchar(256)  NULL
, [EmailConfirmed] bit NOT NULL
, [PasswordHash] nvarchar(256)  NULL
, [SecurityStamp] nvarchar(256)  NULL
, [PhoneNumber] nvarchar(256)  NULL
, [PhoneNumberConfirmed] bit NOT NULL
, [twoFactorEnabled] bit NOT NULL
, [LockoutEndDateUtc] datetime NULL
, [LockoutEnabled] bit NOT NULL
, [AccessFailedCount] bigint  NOT NULL
, [UserName] nvarchar(256)  NOT NULL
, CONSTRAINT [sqlite_autoindex_AspNetUsers_1] PRIMARY KEY ([Id])
);
CREATE TABLE [AspNetUserLogins] (
  [LoginProvider] nvarchar(128)  NOT NULL
, [ProviderKey] nvarchar(128)  NOT NULL
, [UserId] nvarchar(128)  NOT NULL
, CONSTRAINT [sqlite_autoindex_AspNetUserLogins_1] PRIMARY KEY ([LoginProvider],[ProviderKey],[UserId])
, FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE ON UPDATE NO ACTION
);
CREATE TABLE [AspNetUserClaims] (
  [Id] INTEGER  NOT NULL
, [UserId] nvarchar(128)  NOT NULL
, [ClaimType] nvarchar(256)  NULL
, [ClaimValue] nvarchar(256)  NULL
, CONSTRAINT [sqlite_master_PK_AspNetUserClaims] PRIMARY KEY ([Id])
, FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE ON UPDATE NO ACTION
);
CREATE TABLE [AspNetRoles] (
  [Id] nvarchar(128)  NOT NULL
, [Name] nvarchar(256)  NOT NULL
, CONSTRAINT [sqlite_autoindex_AspNetRoles_1] PRIMARY KEY ([Id])
);
CREATE TABLE [AspNetUserRoles] (
  [UserId] nvarchar(128)  NOT NULL
, [RoleId] nvarchar(128)  NOT NULL
, CONSTRAINT [sqlite_autoindex_AspNetUserRoles_1] PRIMARY KEY ([UserId],[RoleId])
, FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE ON UPDATE NO ACTION
, FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE ON UPDATE NO ACTION
);
CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([UserName] ASC);
CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId] ASC);
CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId] ASC);
CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([Name] ASC);
CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId] ASC);
CREATE INDEX [IX_AspNetUserRoles_UserId] ON [AspNetUserRoles] ([UserId] ASC);
COMMIT;

