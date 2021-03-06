DROP TABLE IF EXISTS [Family];
CREATE TABLE [Family] 
(
	[Id] INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,
	[StreetAddress] TEXT  NOT NULL,
	[City] TEXT  NOT NULL,
	[State] TEXT  NOT NULL,
	[ZIP] TEXT  NOT NULL,
	[Joined] TEXT  NOT NULL,
	[Departed] TEXT  NULL,
	[DueDay] INTEGER DEFAULT '1' NOT NULL,
	[Notes] TEXT  NULL
);


DROP TABLE IF EXISTS [Parent];
CREATE TABLE [Parent] 
(
	[Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	[FamilyId] INTEGER NOT NULL,
	[FirstName] TEXT NOT NULL,
	[LastName] TEXT NOT NULL,
	[Email] TEXT NULL
);

DROP TABLE IF EXISTS [Child];
CREATE TABLE [Child] 
(
	[Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	[FamilyId] INTEGER NOT NULL,
	[FirstName] TEXT NOT NULL,
	[LastName] TEXT NOT NULL,
	[Room] TEXT NOT NULL,
	[Joined] TEXT NOT NULL,
	[Departed] TEXT NULL
);

DROP TABLE IF EXISTS [Fee];
CREATE TABLE [Fee]
(
	[Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	[Name] TEXT NOT NULL,
	[Type] TEXT NOT NULL,
	[Amount] REAL NULL,
	[Category] TEXT NOT NULL DEFAULT 'None'
);

DROP TABLE IF EXISTS [Discount];
CREATE TABLE [Discount] 
(
	[Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	[FamilyId] INTEGER NOT NULL,
	[FeeId] INTEGER NOT NULL,
	[Percent] REAL NOT NULL,
	[IsFinancialAid] INTEGER NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS [ChildDays];
CREATE TABLE [ChildDays]
(
	[ChildId] INTEGER NOT NULL,
	[Mon] INTEGER DEFAULT '0' NOT NULL,
	[Tue] INTEGER DEFAULT '0' NOT NULL,
	[Wed] INTEGER DEFAULT '0' NOT NULL,
	[Thu] INTEGER DEFAULT '0' NOT NULL,
	[Fri] INTEGER DEFAULT '0' NOT NULL,
	[Effective] TEXT NOT NULL
);

DROP TABLE IF EXISTS [Payment];
CREATE TABLE [Payment]
(
	[Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	[FamilyId] INTEGER NOT NULL,
	[CheckNum] TEXT NULL,
	[Amount] REAL NOT NULL,
	[Received] TEXT NOT NULL, 
	[Deposited] TEXT NULL
);

DROP TABLE IF EXISTS [Invoice];
CREATE TABLE [Invoice]
(
	[FamilyId] INTEGER NOT NULL,
	[Date] TEXT NOT NULL,
	[Amount] REAL NOT NULL
);

DROP TABLE IF EXISTS [Ledger];
CREATE TABLE [Ledger]
(
	[Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	[FamilyId] INTEGER NOT NULL,
	[Date] TEXT NOT NULL,
	[FeeId] INTEGER NULL,
	[PaymentId] INTEGER NULL,
	[UnitPrice] REAL NOT NULL,
	[Quantity] INTEGER NOT NULL,
	[Amount] REAL NOT NULL,
	[Notes] TEXT NULL
);

DROP TABLE IF EXISTS [SelectFeeType];
CREATE TABLE [SelectFeeType]
(
	[Type] TEXT NOT NULL,
	[Description] TEXT NOT NULL
);

INSERT INTO [SelectFeeType] VALUES ('F', 'Fixed');
INSERT INTO [SelectFeeType] VALUES ('V', 'Varying');
INSERT INTO [SelectFeeType] VALUES ('C', 'Per-Child');
INSERT INTO [SelectFeeType] VALUES ('D', 'Per-Billable Day');
INSERT INTO [SelectFeeType] VALUES ('M', 'Per-Minute');

DROP TABLE IF EXISTS [SelectRoom];
CREATE TABLE [SelectRoom]
(
	[Room] TEXT NOT NULL,
	[Description] TEXT NOT NULL
);

INSERT INTO [SelectRoom] VALUES ('YT', 'Young Toddler');
INSERT INTO [SelectRoom] VALUES ('TR', 'Toddler');
INSERT INTO [SelectRoom] VALUES ('PS1', 'Preschool 1');
INSERT INTO [SelectRoom] VALUES ('PS2', 'Preschool 2');

DROP TABLE IF EXISTS [SelectFeeCategory];
CREATE TABLE [SelectFeeCategory]
(
	[Category] TEXT NOT NULL,
	[Description] TEXT NOT NULL
);

INSERT INTO [SelectFeeCategory] VALUES ('Tuition', 'Tuition');
INSERT INTO [SelectFeeCategory] VALUES ('Fees', 'Fees');
INSERT INTO [SelectFeeCategory] VALUES ('Financial Aid', 'Financial Aid');
INSERT INTO [SelectFeeCategory] VALUES ('Insurance', 'Insurance');
INSERT INTO [SelectFeeCategory] VALUES ('Donations', 'Donations');
INSERT INTO [SelectFeeCategory] VALUES ('Credits', 'Credits');



