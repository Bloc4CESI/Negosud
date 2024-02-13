BEGIN TRY

BEGIN TRAN;

-- CreateTable
CREATE TABLE [dbo].[Address] (
    [Id] INT NOT NULL IDENTITY(1,1),
    [Name] NVARCHAR(50) NOT NULL,
    [Number] INT NOT NULL,
    [Street] NVARCHAR(max) NOT NULL,
    [City] NVARCHAR(50) NOT NULL,
    [Country] NVARCHAR(50) NOT NULL,
    CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED ([Id])
);

-- CreateTable
CREATE TABLE [dbo].[Client] (
    [Id] INT NOT NULL IDENTITY(1,1),
    [FirstName] NVARCHAR(50) NOT NULL,
    [LastName] NVARCHAR(50) NOT NULL,
    [Email] NVARCHAR(255) NOT NULL,
    [PhoneNumber] NVARCHAR(50) NOT NULL,
    [Password] TEXT NOT NULL,
    [AddressId] INT,
    CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED ([Id])
);

-- CreateTable
CREATE TABLE [dbo].[sessions] (
    [id] INT NOT NULL IDENTITY(1,1),
    [Client_id] INT NOT NULL,
    [expires] DATETIME2 NOT NULL,
    [session_token] NVARCHAR(1000) NOT NULL,
    [access_token] NVARCHAR(1000) NOT NULL,
    [created_at] DATETIME2 NOT NULL CONSTRAINT [sessions_created_at_df] DEFAULT CURRENT_TIMESTAMP,
    [updated_at] DATETIME2 NOT NULL CONSTRAINT [sessions_updated_at_df] DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT [sessions_pkey] PRIMARY KEY CLUSTERED ([id]),
    CONSTRAINT [sessions_session_token_key] UNIQUE NONCLUSTERED ([session_token]),
    CONSTRAINT [sessions_access_token_key] UNIQUE NONCLUSTERED ([access_token])
);

-- CreateTable
CREATE TABLE [dbo].[ClientOrder] (
    [Id] INT NOT NULL IDENTITY(1,1),
    [Date] DATETIME NOT NULL,
    [Price] DECIMAL(10,2) NOT NULL,
    [ClientId] INT NOT NULL,
    [OrderStatus] NVARCHAR(50) NOT NULL,
    CONSTRAINT [PK_ClientOrder] PRIMARY KEY CLUSTERED ([Id])
);

-- CreateTable
CREATE TABLE [dbo].[ClientOrderLine] (
    [Id] INT NOT NULL IDENTITY(1,1),
    [Quantity] INT NOT NULL,
    [Price] DECIMAL(10,2) NOT NULL,
    [ClientOrderId] INT NOT NULL,
    [ProductId] INT NOT NULL,
    CONSTRAINT [PK_ClientOrderLine] PRIMARY KEY CLUSTERED ([Id])
);

-- CreateTable
CREATE TABLE [dbo].[Employee] (
    [Id] INT NOT NULL IDENTITY(1,1),
    [LastName] NVARCHAR(50),
    [FirstName] NVARCHAR(50),
    [Password] TEXT NOT NULL,
    [Email] NVARCHAR(255) NOT NULL,
    CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED ([Id])
);

-- CreateTable
CREATE TABLE [dbo].[Family] (
    [Id] INT NOT NULL IDENTITY(1,1),
    [Name] NVARCHAR(50) NOT NULL,
    CONSTRAINT [PK_Family] PRIMARY KEY CLUSTERED ([Id])
);

-- CreateTable
CREATE TABLE [dbo].[Inventory] (
    [Id] INT NOT NULL IDENTITY(1,1),
    [Date] DATE NOT NULL,
    [StatusInventory] VARCHAR(20) NOT NULL,
    CONSTRAINT [PK_Inventaire] PRIMARY KEY CLUSTERED ([Id])
);

-- CreateTable
CREATE TABLE [dbo].[InventoryLigne] (
    [Id] INT NOT NULL IDENTITY(1,1),
    [StockId] INT NOT NULL,
    [InventoryId] INT NOT NULL,
    [QuantityInventory] INT NOT NULL,
    CONSTRAINT [PK_InventoryLigne] PRIMARY KEY NONCLUSTERED ([Id])
);

-- CreateTable
CREATE TABLE [dbo].[Product] (
    [Id] INT NOT NULL IDENTITY(1,1),
    [Name] NVARCHAR(50) NOT NULL,
    [Price] DECIMAL(10,2) NOT NULL,
    [Image] NVARCHAR(max) NOT NULL,
    [Description] TEXT,
    [DateProduction] DATE NOT NULL,
    [NbProductBox] INT,
    [FamilyId] INT NOT NULL,
    [ProviderId] INT NOT NULL,
    [Home] NVARCHAR(max) NOT NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([Id])
);

-- CreateTable
CREATE TABLE [dbo].[Provider] (
    [Id] INT NOT NULL IDENTITY(1,1),
    [Name] NVARCHAR(50) NOT NULL,
    [PhoneNumber] NVARCHAR(50) NOT NULL,
    [Email] NVARCHAR(255) NOT NULL,
    [AddressId] INT NOT NULL,
    CONSTRAINT [PK_Provider] PRIMARY KEY CLUSTERED ([Id])
);

-- CreateTable
CREATE TABLE [dbo].[ProviderOrder] (
    [Id] INT NOT NULL IDENTITY(1,1),
    [Date] DATETIME NOT NULL,
    [Price] DECIMAL(10,2) NOT NULL,
    [ProviderId] INT NOT NULL,
    [ProviderOrderStatus] NVARCHAR(50) NOT NULL,
    CONSTRAINT [PK_ProviderOrder] PRIMARY KEY CLUSTERED ([Id])
);

-- CreateTable
CREATE TABLE [dbo].[ProviderOrderLine] (
    [Id] INT NOT NULL IDENTITY(1,1),
    [Quantity] INT NOT NULL,
    [Price] DECIMAL(10,2) NOT NULL,
    [ProviderOrderId] INT NOT NULL,
    [ProductId] INT NOT NULL,
    CONSTRAINT [PK_ProviderOrderLine] PRIMARY KEY CLUSTERED ([Id])
);

-- CreateTable
CREATE TABLE [dbo].[Stock] (
    [Id] INT NOT NULL IDENTITY(1,1),
    [Quantity] INT NOT NULL,
    [Minimum] INT,
    [Maximum] INT,
    [AutoOrder] BIT NOT NULL,
    [ProductId] INT NOT NULL,
    CONSTRAINT [PK_Stock] PRIMARY KEY CLUSTERED ([Id])
);

-- AddForeignKey
ALTER TABLE [dbo].[Client] ADD CONSTRAINT [FK_Client_Address] FOREIGN KEY ([AddressId]) REFERENCES [dbo].[Address]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

-- AddForeignKey
ALTER TABLE [dbo].[ClientOrder] ADD CONSTRAINT [FK_ClientOrder_Client] FOREIGN KEY ([ClientId]) REFERENCES [dbo].[Client]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

-- AddForeignKey
ALTER TABLE [dbo].[ClientOrderLine] ADD CONSTRAINT [FK_ClientOrderLine_ClientOrder] FOREIGN KEY ([ClientOrderId]) REFERENCES [dbo].[ClientOrder]([Id]) ON DELETE CASCADE ON UPDATE NO ACTION;

-- AddForeignKey
ALTER TABLE [dbo].[ClientOrderLine] ADD CONSTRAINT [FK_ClientOrderLine_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

-- AddForeignKey
ALTER TABLE [dbo].[InventoryLigne] ADD CONSTRAINT [FK_InventoryLigne_Inventory] FOREIGN KEY ([InventoryId]) REFERENCES [dbo].[Inventory]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

-- AddForeignKey
ALTER TABLE [dbo].[InventoryLigne] ADD CONSTRAINT [FK_InventoryLigne_Stock] FOREIGN KEY ([StockId]) REFERENCES [dbo].[Stock]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

-- AddForeignKey
ALTER TABLE [dbo].[Product] ADD CONSTRAINT [FK_Product_Family] FOREIGN KEY ([FamilyId]) REFERENCES [dbo].[Family]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

-- AddForeignKey
ALTER TABLE [dbo].[Product] ADD CONSTRAINT [FK_Product_Provider] FOREIGN KEY ([ProviderId]) REFERENCES [dbo].[Provider]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

-- AddForeignKey
ALTER TABLE [dbo].[Provider] ADD CONSTRAINT [FK_Provider_Address] FOREIGN KEY ([AddressId]) REFERENCES [dbo].[Address]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

-- AddForeignKey
ALTER TABLE [dbo].[ProviderOrder] ADD CONSTRAINT [FK_ProviderOrder_Provider] FOREIGN KEY ([ProviderId]) REFERENCES [dbo].[Provider]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

-- AddForeignKey
ALTER TABLE [dbo].[ProviderOrderLine] ADD CONSTRAINT [FK_ProviderOrderLine_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

-- AddForeignKey
ALTER TABLE [dbo].[ProviderOrderLine] ADD CONSTRAINT [FK_ProviderOrderLine_ProviderOrder] FOREIGN KEY ([ProviderOrderId]) REFERENCES [dbo].[ProviderOrder]([Id]) ON DELETE CASCADE ON UPDATE NO ACTION;

-- AddForeignKey
ALTER TABLE [dbo].[Stock] ADD CONSTRAINT [FK_Stock_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

COMMIT TRAN;

END TRY
BEGIN CATCH

IF @@TRANCOUNT > 0
BEGIN
    ROLLBACK TRAN;
END;
THROW

END CATCH

