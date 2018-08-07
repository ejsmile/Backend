CREATE TABLE IF NOT EXISTS AspNetRoles ( Id varchar(128) NOT NULL, Name varchar(256) NOT NULL, PRIMARY KEY (Id) ); 
CREATE TABLE IF NOT EXISTS AspNetUserRoles ( UserId varchar(128) NOT NULL, RoleId varchar(128) NOT NULL, PRIMARY KEY (UserId, RoleId),
 FOREIGN KEY (RoleId) REFERENCES AspNetRoles (Id) ON DELETE CASCADE, FOREIGN KEY (UserId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE );

 
 CREATE TABLE IF NOT EXISTS AspNetUsers (

 Id varchar(128) NOT NULL, Email varchar(256) NULL, 
 EmailConfirmed boolean NOT NULL, PasswordHash varchar(256) NULL, SecurityStamp varchar(256) NULL, PhoneNumber varchar(256) NULL, PhoneNumberConfirmed boolean NOT NULL, 
 twoFactorEnabled boolean NOT NULL, LockoutEndDateUtc datetime NULL, LockoutEnabled boolean NOT NULL, AccessFailedCount INTEGER NOT NULL, UserName varchar(256) NOT NULL, PRIMARY KEY (Id) 
 
 
 ); 
 CREATE TABLE IF NOT EXISTS AspNetUserClaims ( Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, UserId varchar(128) NOT NULL, ClaimType varchar(256) NULL, ClaimValue varchar(256) NULL, FOREIGN KEY 
 (UserId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE ); CREATE TABLE IF NOT EXISTS AspNetUserLogins ( LoginProvider varchar(128) NOT NULL, ProviderKey varchar(128) NOT NULL, UserId varchar(128) 
 NOT NULL, PRIMARY KEY (LoginProvider, ProviderKey, UserId), FOREIGN KEY (UserId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE ); CREATE UNIQUE INDEX RoleNameIndex ON AspNetRoles (Name); 
 CREATE INDEX IX_AspNetUserRoles_UserId ON AspNetUserRoles (UserId); CREATE INDEX IX_AspNetUserRoles_RoleId ON AspNetUserRoles (RoleId); 
 CREATE UNIQUE INDEX UserNameIndex ON AspNetUsers (UserName); CREATE INDEX IX_AspNetUserClaims_UserId ON AspNetUserClaims (UserId); CREATE INDEX IX_AspNetUserLogins_UserId ON AspNetUserLogins (UserId);
