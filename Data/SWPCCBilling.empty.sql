DROP TABLE IF EXISTS [Family];
CREATE TABLE [Family] 
(
	[Id] INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,
	[StreetAddress] TEXT  NOT NULL,
	[City] TEXT  NOT NULL,
	[State] TEXT  NOT NULL,
	[ZIP] TEXT  NOT NULL,
	[Joined] DATE  NOT NULL,
	[Departed] DATE  NULL,
	[DueDay] INTEGER DEFAULT '1' NOT NULL,
	[Notes] TEXT  NULL
);


DROP TABLE IF EXISTS [Parent];
CREATE TABLE [Parent] 
(
	[Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	[FirstName] TEXT NOT NULL,
	[LastName] TEXT NOT NULL,
	[Email] TEXT NULL
);

DROP TABLE IF EXISTS [Child];
CREATE TABLE [Child] 
(
	[Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	[FirstName] TEXT NOT NULL,
	[LastName] TEXT NOT NULL,
	[Room] TEXT NOT NULL,
	[Joined] DATE NOT NULL,
	[Departed] DATE NULL
);

DROP TABLE IF EXISTS [Fee];
CREATE TABLE [Fee]
(
	[Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	[Name] TEXT NOT NULL,
	[Type] TEXT NOT NULL,
	[Amount] NUMERIC NULL
);

DROP TABLE IF EXISTS [FamilyParent];
CREATE TABLE [FamilyParent] 
(
	[FamilyId] INTEGER NOT NULL,
	[ParentId] INTEGER NOT NULL,
	FOREIGN KEY(FamilyId) REFERENCES Family(Id),
	FOREIGN KEY(ParentId) REFERENCES Parent(Id)
);

DROP TABLE IF EXISTS [FamilyChild];
CREATE TABLE [FamilyChild] 
(
	[FamilyId] INTEGER NOT NULL,
	[ChildId] INTEGER NOT NULL,
	FOREIGN KEY(FamilyId) REFERENCES Family(Id),
	FOREIGN KEY(ChildId) REFERENCES Child(Id)
);

DROP TABLE IF EXISTS [FamilyDiscount];
CREATE TABLE [FamilyDiscount] 
(
	[FamilyId] INTEGER NOT NULL,
	[FeeId] INTEGER NOT NULL,
	[Percent] NUMERIC NOT NULL,
	[EffectiveDate] DATE NOT NULL,
	FOREIGN KEY(FamilyId) REFERENCES Family(Id),
	FOREIGN KEY(FeeId) REFERENCES Fee(Id)
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
	[EffectiveDate] DATE NOT NULL,
	FOREIGN KEY(ChildId) REFERENCES Child(Id)
);

DROP TABLE IF EXISTS [Payment];
CREATE TABLE [Payment]
(
	[FamilyId] INTEGER NOT NULL,
	[InvoiceId] INTEGER NULL,
	[CheckNum] TEXT NULL,
	[Amount] NUMERIC NOT NULL,
	[Received] DATE NOT NULL,
	[Deposited] DATE NULL,
	FOREIGN KEY(FamilyId) REFERENCES Family(Id)
);

DROP TABLE IF EXISTS [Ledger];
CREATE TABLE [Ledger]
(
	[Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	[FamilyId] INTEGER NOT NULL,
	[Date] DATE NOT NULL,
	[FeeId] INTEGER NULL,
	[PaymentId] INTEGER NULL,
	[Amount] NUMERIC NOT NULL,
	[Notes] TEXT NULL,
	FOREIGN KEY(FamilyId) REFERENCES Family(Id),
	FOREIGN KEY(FeeId) REFERENCES Fee(Id),
	FOREIGN KEY(PaymentId) REFERENCES Payment(Id)
);

DROP TABLE IF EXISTS [Invoice];
CREATE TABLE [Invoice]
(
	[Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	[FamilyId] INTEGER NOT NULL,
	[Date] DATE NOT NULL,
	[PastDue] NUMERIC NOT NULL,
	[NowDue] NUMERIC NOT NULL,
	FOREIGN KEY(FamilyId) REFERENCES Family(Id)
);

DROP TABLE IF EXISTS [InvoiceLine];
CREATE TABLE [InvoiceLine]
(
	[InvoiceId] NOT NULL,
	[LedgerId] NOT NULL,
	FOREIGN KEY(InvoiceId) REFERENCES Invoice(Id),
	FOREIGN KEY(LedgerId) REFERENCES Ledger(Id)
);