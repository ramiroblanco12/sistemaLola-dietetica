IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Categorias] (
    [Id] int NOT NULL IDENTITY,
    [Descripcion] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Categorias] PRIMARY KEY ([Id])
);

CREATE TABLE [Ventas] (
    [Id] int NOT NULL IDENTITY,
    [Fecha] datetime2 NOT NULL,
    [Total] decimal(18,2) NOT NULL,
    [MedioPago] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Ventas] PRIMARY KEY ([Id])
);

CREATE TABLE [Productos] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(max) NOT NULL,
    [Descripcion] nvarchar(max) NOT NULL,
    [CodigoBarras] nvarchar(max) NOT NULL,
    [CategoriaId] int NOT NULL,
    [Costo] decimal(18,2) NOT NULL,
    [PrecioFinal] decimal(18,2) NOT NULL,
    [StockActual] decimal(18,2) NOT NULL,
    [StockMinimo] decimal(18,2) NOT NULL,
    [UnidadMedida] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Productos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Productos_Categorias_CategoriaId] FOREIGN KEY ([CategoriaId]) REFERENCES [Categorias] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [DetalleVenta] (
    [Id] int NOT NULL IDENTITY,
    [VentaId] int NOT NULL,
    [ProductoId] int NOT NULL,
    [Cantidad] int NOT NULL,
    [PrecioUnitario] decimal(18,2) NOT NULL,
    [Subtotal] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_DetalleVenta] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DetalleVenta_Productos_ProductoId] FOREIGN KEY ([ProductoId]) REFERENCES [Productos] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_DetalleVenta_Ventas_VentaId] FOREIGN KEY ([VentaId]) REFERENCES [Ventas] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Stock] (
    [Id] int NOT NULL IDENTITY,
    [ProductoId] int NOT NULL,
    [Fecha] datetime2 NOT NULL,
    [cantidad] decimal(18,2) NOT NULL,
    [Tipo] nvarchar(max) NOT NULL,
    [Motivo] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Stock] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Stock_Productos_ProductoId] FOREIGN KEY ([ProductoId]) REFERENCES [Productos] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_DetalleVenta_ProductoId] ON [DetalleVenta] ([ProductoId]);

CREATE INDEX [IX_DetalleVenta_VentaId] ON [DetalleVenta] ([VentaId]);

CREATE INDEX [IX_Productos_CategoriaId] ON [Productos] ([CategoriaId]);

CREATE INDEX [IX_Stock_ProductoId] ON [Stock] ([ProductoId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251118203032_InitialMigration', N'9.0.10');

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Productos]') AND [c].[name] = N'UnidadMedida');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Productos] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [Productos] ALTER COLUMN [UnidadMedida] nvarchar(max) NULL;

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Productos]') AND [c].[name] = N'StockMinimo');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Productos] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Productos] ALTER COLUMN [StockMinimo] decimal(18,2) NULL;

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Productos]') AND [c].[name] = N'StockActual');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Productos] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Productos] ALTER COLUMN [StockActual] decimal(18,2) NULL;

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Productos]') AND [c].[name] = N'Nombre');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Productos] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Productos] ALTER COLUMN [Nombre] nvarchar(max) NULL;

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Productos]') AND [c].[name] = N'Descripcion');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Productos] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Productos] ALTER COLUMN [Descripcion] nvarchar(max) NULL;

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Productos]') AND [c].[name] = N'CodigoBarras');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Productos] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Productos] ALTER COLUMN [CodigoBarras] nvarchar(max) NULL;

ALTER TABLE [Productos] ADD [CodigoBalanza] nvarchar(max) NOT NULL DEFAULT N'';

ALTER TABLE [Productos] ADD [EsDeBalanza] bit NOT NULL DEFAULT CAST(0 AS bit);

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Categorias]') AND [c].[name] = N'Descripcion');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Categorias] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [Categorias] ALTER COLUMN [Descripcion] nvarchar(max) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251204200405_NewMigration', N'9.0.10');

ALTER TABLE [Productos] ADD [PorcentajeIndividual] int NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251204231332_NewMigration2', N'9.0.10');

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Productos]') AND [c].[name] = N'EsDeBalanza');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Productos] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [Productos] ALTER COLUMN [EsDeBalanza] bit NULL;

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Productos]') AND [c].[name] = N'CodigoBalanza');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Productos] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [Productos] ALTER COLUMN [CodigoBalanza] nvarchar(max) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251204232739_NewMigration3', N'9.0.10');

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Productos]') AND [c].[name] = N'StockMinimo');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [Productos] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [Productos] ALTER COLUMN [StockMinimo] int NULL;

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Productos]') AND [c].[name] = N'StockActual');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [Productos] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [Productos] ALTER COLUMN [StockActual] int NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251205015149_NewMigration4', N'9.0.10');

ALTER TABLE [DetalleVenta] DROP CONSTRAINT [FK_DetalleVenta_Productos_ProductoId];

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Productos]') AND [c].[name] = N'PorcentajeIndividual');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [Productos] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [Productos] ALTER COLUMN [PorcentajeIndividual] decimal(18,2) NULL;

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[DetalleVenta]') AND [c].[name] = N'ProductoId');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [DetalleVenta] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [DetalleVenta] ALTER COLUMN [ProductoId] int NULL;

ALTER TABLE [DetalleVenta] ADD CONSTRAINT [FK_DetalleVenta_Productos_ProductoId] FOREIGN KEY ([ProductoId]) REFERENCES [Productos] ([Id]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251220144339_NewMigration5', N'9.0.10');

ALTER TABLE [Productos] ADD [Proveedor] nvarchar(max) NOT NULL DEFAULT N'';

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251224182744_NewMigration60', N'9.0.10');

DECLARE @var13 sysname;
SELECT @var13 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Productos]') AND [c].[name] = N'Proveedor');
IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [Productos] DROP CONSTRAINT [' + @var13 + '];');
ALTER TABLE [Productos] DROP COLUMN [Proveedor];

ALTER TABLE [Productos] ADD [ProveedorId] int NOT NULL DEFAULT 0;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251225185606_NewMigration6', N'9.0.10');

CREATE TABLE [Proveedores] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(max) NOT NULL,
    [Mail] nvarchar(max) NOT NULL,
    [Activo] bit NOT NULL,
    CONSTRAINT [PK_Proveedores] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251225190104_NewMigration303', N'9.0.10');

CREATE TABLE [Sucursal] (
    [SucursalID] int NOT NULL IDENTITY,
    [Nombre] nvarchar(100) NOT NULL,
    [Codigo] nvarchar(20) NOT NULL,
    [Activo] bit NOT NULL,
    CONSTRAINT [PK_Sucursal] PRIMARY KEY ([SucursalID])
);

CREATE INDEX [IX_Productos_ProveedorId] ON [Productos] ([ProveedorId]);

CREATE UNIQUE INDEX [IX_Sucursal_Codigo] ON [Sucursal] ([Codigo]);

ALTER TABLE [Productos] ADD CONSTRAINT [FK_Productos_Proveedores_ProveedorId] FOREIGN KEY ([ProveedorId]) REFERENCES [Proveedores] ([Id]) ON DELETE CASCADE;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260108002114_AddSucursal', N'9.0.10');

ALTER TABLE [Productos] DROP CONSTRAINT [FK_Productos_Proveedores_ProveedorId];

DECLARE @var14 sysname;
SELECT @var14 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Productos]') AND [c].[name] = N'ProveedorId');
IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [Productos] DROP CONSTRAINT [' + @var14 + '];');
ALTER TABLE [Productos] ALTER COLUMN [ProveedorId] int NULL;

ALTER TABLE [Productos] ADD CONSTRAINT [FK_Productos_Proveedores_ProveedorId] FOREIGN KEY ([ProveedorId]) REFERENCES [Proveedores] ([Id]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260108010134_AddSucursal1', N'9.0.10');

DROP INDEX [IX_Productos_ProveedorId] ON [Productos];

CREATE UNIQUE INDEX [IX_Productos_ProveedorId] ON [Productos] ([ProveedorId]) WHERE [ProveedorId] IS NOT NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260108011047_AddSucursal2', N'9.0.10');

DROP INDEX [IX_Productos_ProveedorId] ON [Productos];

CREATE INDEX [IX_Productos_ProveedorId] ON [Productos] ([ProveedorId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260108014111_AddSucursal3', N'9.0.10');

DECLARE @var15 sysname;
SELECT @var15 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Proveedores]') AND [c].[name] = N'Mail');
IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [Proveedores] DROP CONSTRAINT [' + @var15 + '];');
ALTER TABLE [Proveedores] ALTER COLUMN [Mail] nvarchar(max) NULL;

ALTER TABLE [Proveedores] ADD [NumeroTelefono] nvarchar(max) NULL;

ALTER TABLE [Proveedores] ADD [NumeroTelefono2] nvarchar(max) NULL;

ALTER TABLE [Proveedores] ADD [NumeroTelefono3] nvarchar(max) NULL;

CREATE TABLE [ProductosSucursal] (
    [ProductoSucursalId] int NOT NULL IDENTITY,
    [ProductoId] int NOT NULL,
    [SucursalId] int NOT NULL,
    [Activo] bit NOT NULL,
    [precioVenta] decimal(18,2) NOT NULL,
    [FechaActivacion] datetime2 NOT NULL,
    [FechaModificacion] datetime2 NOT NULL,
    CONSTRAINT [PK_ProductosSucursal] PRIMARY KEY ([ProductoSucursalId]),
    CONSTRAINT [FK_ProductosSucursal_Productos_ProductoId] FOREIGN KEY ([ProductoId]) REFERENCES [Productos] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ProductosSucursal_Sucursal_SucursalId] FOREIGN KEY ([SucursalId]) REFERENCES [Sucursal] ([SucursalID]) ON DELETE NO ACTION
);

CREATE INDEX [IX_ProductosSucursal_ProductoId] ON [ProductosSucursal] ([ProductoId]);

CREATE UNIQUE INDEX [IX_ProductosSucursal_SucursalId_ProductoId] ON [ProductosSucursal] ([SucursalId], [ProductoId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260203164729_ModificacionProveedores', N'9.0.10');

ALTER TABLE [Proveedores] ADD [Anotacion] nvarchar(max) NOT NULL DEFAULT N'';

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260203170008_ModificacionProveedores2', N'9.0.10');

DECLARE @var16 sysname;
SELECT @var16 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Proveedores]') AND [c].[name] = N'Anotacion');
IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [Proveedores] DROP CONSTRAINT [' + @var16 + '];');
ALTER TABLE [Proveedores] ALTER COLUMN [Anotacion] nvarchar(250) NOT NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260203182927_ModificacionProveedores3', N'9.0.10');

COMMIT;
GO

