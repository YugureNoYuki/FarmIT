Use [master]
GO

If Exists (Select * from sys.databases where name = 'FarmDb')
DROP DATABASE [FarmDb]
GO
Create Database [FarmDb]
GO

SET QUOTED_IDENTIFIER OFF;
GO
USE [FarmDb];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 0 'Unit'
CREATE TABLE [dbo].[Unit] (
    [UnitID] int primary key identity(1,1),
	[UnitDescr] varchar (5) not null,
);
GO
-- Creating table 1 'Country'
CREATE TABLE [dbo].[Country] (
    [CountryID] int primary key identity(1,1),
	[CountryDescr] varchar (25) not null
);
GO
-- Creating table 2 'Province'
CREATE TABLE [dbo].[Province] (
    [ProvinceID] int primary key identity(1,1),
	[ProvinceDescr] varchar (36) not null,
    [CountryID] int not null foreign key references [Country] ([CountryID])
);
GO
---- Creating table 3 'Farm'
CREATE TABLE [dbo].[Farm] (
    [FarmID] int primary key identity(1,1),
	[FarmName] varchar (35) not null,
    [ProvinceID] int not null foreign key references [Province](ProvinceID)
);
GO
-- Creating table 4 'Land'
CREATE TABLE [dbo].[Land] (
    [LandID] int primary key identity(1,1),
	[LandName] varchar (35) not null,
    [FarmID] int not null foreign key references [Farm]([FarmID])
);
GO
-- Creating table 5 'RainfallRecord'
CREATE TABLE [dbo].[RainfallRecord] (
    [RainFallRecID] int primary key identity(1,1),
	[RecordDate] date not null,
	[RecordMeasurement] int not null,
	[UnitID] int not null foreign key references [Unit](UnitID), --++
    [LandID] int not null foreign key references [Land](LandID)
);
GO
-- Creating table 6 'FieldStatus'
CREATE TABLE [dbo].[FieldStatus] (
    [FieldStatID] int primary key identity(1,1),
	[FieldStatDescr] varchar(25) not null,
	[StatPreConditions] varchar (255) null,
);
GO
-- Creating table 7 'Field'
CREATE TABLE [dbo].[Field] (
    [FieldID] int primary key identity(1,1),
	[FieldName] varchar (35) not null,
	[FieldHectares] decimal(18,2) not null,
    [FieldStatusID] int not null foreign key references [FieldStatus]([FieldStatID]),
	[LandID] int not null foreign key references [Land](LandID)
);
GO
-- Creating table 8 'NaturalDisaster'
CREATE TABLE [dbo].[NaturalDisaster] (
    [NatDisasterID] int primary key identity(1,1),
	[NatDisasterDescr] varchar (35) not null,
	[NatDisasterPrecautions] varchar (100),
	[NatDisasterPotentialEffects] varchar (100),
	[NatDisasterSigns] varchar (100)
);
GO
-- Creating assoc table 9 'FieldNaturalDisaster'
CREATE TABLE [dbo].[FieldNaturalDisaster] (
    [FieldID] int not null foreign key references [Field](FieldID),
	[NatDisasterID] int not null foreign key references [NaturalDisaster](NatDisasterID),
	[Date] date not null,
	primary key (FieldID,NatDisasterID)
);
GO
-- Creating table 10 'BiologicalDisaster'
CREATE TABLE [dbo].[BiologicalDisaster] (
    [BioDisasterID] int primary key identity(1,1),
	[BioDisasterDescr] varchar (35) not null,
	[BioDisasterNotes] varchar (50)
);
GO
-- Creating assoc table 11 'FieldBiologicalDisaster'
CREATE TABLE [dbo].[FieldBiologicalDisaster] (
    [FieldID] int not null foreign key references [Field](FieldID),
	[BioDisasterID] int not null foreign key references [BiologicalDisaster](BioDisasterID),
	[Date] date not null,
	primary key (FieldID,BioDisasterID)
);
GO
-- Creating table 12 'FarmWorkerType'
CREATE TABLE [dbo].[FarmWorkerType] (
    [FarmWorkerTypeID] int primary key identity(1,1),
	[FarmWorkerTypeDescr] varchar (30) not null,
	[FarmWorkerTypeNotes] varchar(50),
	[FarmWorkerTypeActiveStatus] varchar(10) not null
);
GO
-- Creating table 13 'Gender'
CREATE TABLE [dbo].[Gender] (
    [GenderID] int primary key identity(1,1),
	[GenderDescr] varchar (8) not null
);
GO
-- Creating table 14 'Title'
CREATE TABLE [dbo].[Title] (
    [TitleID] int primary key identity(1,1),
	[TitleDescr] varchar (8) not null
);
GO
-- Creating table 15 'FarmWorker'
CREATE TABLE [dbo].[FarmWorker] (
    [FarmWorkerNum] int primary key identity(1,1),
	[FarmWorkerFName] varchar (15) not null,
	[FarmWorkerLName] varchar (15) not null,
	[FarmWorkerContactNum] char (10),
	[FarmWorkerImg] varchar(255),
	[Address] varchar (35),
	[Surburb] varchar (35),
	[City] varchar (35),
	[CountryID] int not null foreign key references [Country](CountryID),
	[ProvinceID] int not null foreign key references [Province](ProvinceID),
	[ContractStartDate] date not null,
	[ContractEndDate] date not null,
    [FarmWorkerIDNum] char (13) not null,
    [TitleID] int not null foreign key references [Title] (TitleID),
    [GenderID] int not null foreign key references [Gender] (GenderID),
    [FarmWorkerTypeID] int not null foreign key references [FarmWorkerType] (FarmWorkerTypeID),
	[FarmID] int not null foreign key references [Farm] (FarmID)
);
GO
-- Creating table 16 'UserAccessLevel'
CREATE TABLE [dbo].[UserAccessLevel] (
    [UserAccessLevelID] int primary key identity(1,1),
	[UserAccessLevelDescr] varchar (25) not null,
	[Notes] varchar(50) not null
);
GO
-- Creating table 17 'User'
CREATE TABLE [dbo].[User] (
    [UserID] int primary key identity(1,1),
	--[UserName] varchar (35) null,
	[UserEmailAddress] varchar (35) not null,
	[UserPassword] nvarchar (max) not null,
	[UserContractNum] char (10),
	[UserFName] varchar (35) not null,
    [UserLName] varchar (35) not null,
	[UserIDNum] char (13) null,
	[IsEmailVerified] bit, --[IsEmailVerified] bit
	[ActivationCode] uniqueidentifier,
	[ResetPasswordCode] nvarchar(100) null,
    [UserAccessLevelID] int not null foreign key references [UserAccessLevel] (UserAccessLevelID)
);
GO
--Creating table 17.1 'UserRole'
CREATE TABLE [dbo].[UserRole](
	[UserRoleID] int primary key identity(1,1),
	[UserID] int not null foreign key references [User](UserID),
	[UserAccesssLevelID] int not null foreign key references [UserAccessLevel](UserAccessLevelID)
)
-- Creating table 18 'AuditType'
CREATE TABLE [dbo].[AuditType](
	[AuditTypeID] int identity(1,1) primary key,
	[AuditTypeDescr] varchar (50) not null
);
GO
-- Creating table 19 'Audit'
CREATE TABLE [dbo].[Audit](
	[AuditID] int identity(1,1) primary key,
	[AuditRefTable] varchar (35) not null,
	[TrxDate] date not null,
	[TrxTime] datetime not null,
	[TrxNewRecord] varchar (255),
	[TrxOldRecord] varchar (255),
	[AuditRefAtrribute] varchar (35),
	[UserID] int not null foreign key references [User](UserID),
	[AuditTypeID] int not null foreign key references [AuditType] (AuditTypeID)
);
GO
-- Creating table 20 'Log'
CREATE TABLE [dbo].[Log] (
    [LogID] int primary key identity(1,1),
	[LogInTimeStamp] datetime not null,
	[LogOutTimeStamp] datetime not null,
	[UserID] int not null foreign key references [User] (UserID)
);
GO
-- Creating table 21 'AttendenceSheet'
CREATE TABLE [dbo].[AttendenceSheet] (
    [AttendenceSheetID] int primary key identity(1,1),
	[ClockInTime] smalldatetime not null,
	[ClockOutTime] smalldatetime  null,
	[FarmWorkerNum] int not null foreign key references [FarmWorker](FarmWorkerNum),
	[UserID] int not null foreign key references [User] (UserID)
);
GO
-- Creating table 22 'VehicleType'
CREATE TABLE [dbo].[VehicleType] (
    [VehTypeID] int primary key identity(1,1),
	[VehTypeDescr] varchar (30) not null,
	[VehTypeImg] varchar(255)
);
GO
-- Creating table 23 'VehicleMake'
CREATE TABLE [dbo].[VehicleMake] (
    [VehMakeID] int primary key identity(1,1),
	[VehMakeDescr] varchar (30) not null
);
GO
-- Creating table 24 'Vehicle'
CREATE TABLE [dbo].[Vehicle] (
    [VehicleID] int primary key identity(1,1),
	[VehName] varchar (30) not null,
	[VehYear] varchar (10),
	[VehModel] varchar (30) not null,
	[VehEngineNum] varchar (30),
	[VehVinNum] varchar (17) not null,
	[VehRegNum] varchar (12) not null,
	[VehLicenseNum] varchar (15) not null,
	[VehExpDate] date not null,
	[VehCurrMileage] int not null,
	[VehServiceInterval] int not null,
	[UnitID] int not null foreign key references [Unit](UnitID),
	[VehNextService] int not null,
	[Farm] int not null foreign key references [Farm](FarmID),
    [VehTypeID] int not null foreign key references [VehicleType](VehTypeID),
    [VehMakeID] int not null foreign key references [VehicleMake](VehMakeID)
);
GO
-- Creating table 25 'VehicleService'
CREATE TABLE [dbo].[VehicleService] (
    [VehicleServiceID] int primary key identity(1,1),
	[VehicleService_Date] date not null,
	[VehicleService_Cost] decimal(18,2),
	[VehicleServiceRecord] int not null,
	[UnitID] int not null foreign key references [Unit](UnitID),
    [VehicleID] int not null foreign key references [Vehicle](VehicleID)
);
GO

-- Creating table 26 'OrderStatus' chkd
CREATE TABLE [dbo].[OrderStatus] (
    [OrderStatusID] int primary key identity(1,1),
	[OrderStatusDescr] varchar (20) not null
);
GO
-- Creating table 27 'Supplier'
CREATE TABLE [dbo].[Supplier] (
    [SupplierID] int identity(1,1) primary key,
	[SupplierName] varchar (50) not null,
	[Address] varchar (35),
	[Surburb] varchar (35),
	[City] varchar (35),
	[CountryID] int not null foreign key references [Country](CountryID),
	[ProvinceID] int not null foreign key references [Province](ProvinceID),
	[SupplierEmailAddress] varchar (35) not null,
	[SupplierPhoneNum] char (10),
	[SupplierCellNum] char (10)
);
GO
-- Creating table 28 'Order'
CREATE TABLE [dbo].[Order] (
    [OrderNum] int primary key identity(1,1),
	[OrderDate] date not null,
	[OrderItemPrice] decimal(18,2) not null,
	[SupplierID] int not null foreign key references [Supplier](SupplierID),
	[UserID] int not null foreign key references [User](UserID),
	[FarmID] int not null foreign key references [Farm](FarmID),
	[OrderStatusID] int not null foreign key references [OrderStatus](OrderStatusID),
	[OrderItem] varchar (35) not null,
	[OrderQty] int not null,
	[UnitID] int not null foreign key references [Unit](UnitID)
);
GO
-- Creating table 29 'InventoryType'
CREATE TABLE [dbo].[InventoryType] (
    [InvTypeID] int primary key identity(1,1),
	[InvTypeDescr] varchar (35) not null
);
GO
-- Creating table 30 'Inventory'
CREATE TABLE [dbo].[Inventory] (
    [InventoryID] int primary key identity(1,1),
	[InvDescr] varchar (35) not null,
	[InvQty] int not null,
	[InvDatePurchased] date not null,
	[InvCode] varchar (10) not null,
	[Unit] int not null foreign key references [Unit](UnitID),
	[InvTypeID] int not null foreign key references [InventoryType](InvTypeID)
);
GO
-- Creating table 31 'Treatment'
CREATE TABLE [dbo].[Treatment] (
    [TreatmentID] int primary key identity(1,1),
	[TreatmentDescr] varchar (50) not null
);
GO
-- Creating assoc table 32 'InventoryTreatment'
CREATE TABLE [dbo].[InventoryTreatment](
	[TreatmentID] int not null foreign key references [Treatment](TreatmentID),
	[InventoryID] int not null foreign key references [Inventory](InventoryID),
	primary key (TreatmentID,InventoryID),
	TreatmentQnty int not null,
	[Unit] int not null foreign key references [Unit](UnitID)
);
GO
-- Creating assoc table 33 'FieldTreatment'
CREATE TABLE [dbo].[FieldTreatment](
	[FieldID] int not null foreign key references [Field](FieldID),
	[TreatmentID] int not null foreign key references [Treatment](TreatmentID),
	primary key (FieldID,TreatmentID),
	TreatmentDate date not null,
);
GO
-- Creating assoc table 34 'OrderLine'
CREATE TABLE [dbo].[OrderLine](
	[OrderNum] int not null foreign key references [Order](OrderNum),
	[InventoryID] int not null foreign key references [Inventory](InventoryID),
	primary key (OrderNum,InventoryID),
	OrderLineTotalAmount decimal(18,2) not null,
	OrderLineTotalQuantity int not null
);
GO
-- Creating table 35 'CropCycle'
CREATE TABLE [dbo].[CropCycle] (
    [CropCycleID] int primary key identity(1,1),
	[CropCycleStartDate] date not null,
	[CropCycleEndDate] date not null,
	[CropCycleDescr] varchar (35) not null
);
GO
-- Creating table 36 'CropType'
CREATE TABLE [dbo].[CropType] (
    [CropTypeID] int identity(1,1) primary key,
	[CropTypeDescr] varchar (20),
	[MaturityDays] int
);
GO
-- Creating table 37 'FieldStage'
CREATE TABLE [dbo].[FieldStage] (
    [FieldStageID] int primary key identity(1,1),
	[FieldStageDescr] varchar (20) not null,
);
GO
-- Creating table 38 'Plantation' add candidate key
CREATE TABLE [dbo].[Plantation] (
	[PlantationID] int primary key identity(1,1),
    [FieldID] int not null foreign key references [Field] (FieldID),
	[CropTypeID] int not null foreign key references [CropType](CropTypeID),
	[CropCycleID] int not null foreign key references [CropCycle](CropCycleID),
	[FieldStageID] int not null foreign key references [FieldStage](FieldStageID),
	[DatePlanted] date,
	[RefugeSeedAmntUsed] decimal(18,2),
	[RefugeUnit] int not null foreign key references [Unit](UnitID),
	[RefugeAreaHectares] decimal(18,2),
	[ExpectedYieldQnty] decimal(18,2) not null,
	[YieldUnit] int not null foreign key references [Unit](UnitID),
	[DateHarvested] date,
	[PlantationStatus] varchar (20) not null
);
GO
-- Creating table 39 'Silo'
CREATE TABLE [dbo].[Silo] (
    [SiloID] int primary key identity(1,1),
	[SiloDescr] varchar (20) not null,
	[SiloCapacity] decimal(18,2) not null,
	[UnitID] int not null foreign key references [Unit](UnitID),
	[SiloRentalFeePA] decimal(18,2),
	[SiloStatus] varchar(20)
);
GO
-- Creating table 40 'SiloHarvest'
CREATE TABLE [dbo].[SiloHarvest] (
    [SiloHarvestID] int primary key identity(1,1),
	[SiloHarvestStoreStartDate] date not null,
	[SiloHarvestStoreEndDate] date not null,
	[SiloHarvestTonnesStored] decimal(18,2) not null,
	[SiloID] int not null foreign key references [Silo] (SiloID),
	[PlantationID] int not null foreign key references [Plantation](PlantationID),
);
GO
-- Creating table 41 'Customer'
CREATE TABLE [dbo].[Customer] (
    [CustomerID] int identity(1,1) primary key,
	[CustomerFName] varchar (50) not null,
	[CustomerLName] varchar (50) not null,
	[CustomerContactNum] char (10) not null,
	[ContactPersonalEmailAddress] varchar (35),
	[CompanyName] varchar (35),
	[Address] varchar (35),
	[Surburb] varchar (35),
	[City] varchar (35),
	[CountryID] int not null foreign key references [Country](CountryID),
	[ProvinceID] int not null foreign key references [Province](ProvinceID),
	[CompanyTelNum] char (10),
);
GO
-- Creating table 42 'Sale'
CREATE TABLE [dbo].[Sale] (
    [SaleID] int primary key identity(1,1),
	[SaleDate] date not null,
	[SaleQty] decimal(18,2) not null,
	[UnitID] int not null foreign key references [Unit](UnitID),
	[SaleAmnt] decimal(18,2) not null,
	[CustomerID] int not null foreign key references [Customer](CustomerID),
	[PurchaseAgreement] varchar(255)
);
GO
-- Creating assoc table 43 'SiloHarvestSale'
CREATE TABLE [dbo].[SiloHarvestSale] (
    [SiloHarvestID] int not null foreign key references [SiloHarvest](SiloHarvestID),
	[SaleID] int not null foreign key references [Sale](SaleID)
	primary key (SiloHarvestID,SaleID),
	SiloHarvestSaleTotalAmnt decimal(18,2) not null,
	SiloHarvestSaleTotalQty decimal(18,2) not null
);
GO