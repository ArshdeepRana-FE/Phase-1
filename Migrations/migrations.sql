USE [RestaurauntManagement]
GO

-- CONTACT_DETAILS TABLE
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[contact_details]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[contact_details](
        [id] [int] NOT NULL,
        [location] [nvarchar](200) NULL,
        [email] [nvarchar](100) NULL,
        [phone_number] [nvarchar](100) NULL,
        PRIMARY KEY CLUSTERED ([id] ASC)
    )
END
GO

-- MENU_ITEMS TABLE
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[menu_items]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[menu_items](
        [id] [int] IDENTITY(1,1) NOT NULL,
        [name] [nvarchar](100) NOT NULL,
        [rating] [decimal](2, 1) NULL,
        [price] [int] NOT NULL,
        [type] [varchar](20) NULL,
        [inStock] [bit] NULL,
        PRIMARY KEY CLUSTERED ([id] ASC)
    )
END
GO

-- Add CHECK constraints to menu_items
IF NOT EXISTS (
    SELECT * FROM sys.check_constraints WHERE name = 'CK_menu_items_rating'
)
    ALTER TABLE [dbo].[menu_items]  WITH CHECK ADD CONSTRAINT [CK_menu_items_rating] CHECK  (([rating]>=(0.0) AND [rating]<=(5.0)))
GO

IF NOT EXISTS (
    SELECT * FROM sys.check_constraints WHERE name = 'CK_menu_items_type'
)
    ALTER TABLE [dbo].[menu_items]  WITH CHECK ADD CONSTRAINT [CK_menu_items_type] CHECK  (([type]='vegan' OR [type]='non vegetarian' OR [type]='vegetarian'))
GO

-- ORDER_ITEM TABLE
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[order_item]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[order_item](
        [id] [int] IDENTITY(1,1) NOT NULL,
        [order_id] [int] NULL,
        [item_id] [int] NULL,
        [quantity] [int] NULL,
        PRIMARY KEY CLUSTERED ([id] ASC)
    )
END
GO

-- Add FK and CHECK constraints to order_item
IF NOT EXISTS (
    SELECT * FROM sys.foreign_keys WHERE name = 'FK_order_item_menu'
)
    ALTER TABLE [dbo].[order_item]  WITH CHECK ADD  CONSTRAINT [FK_order_item_menu] FOREIGN KEY([item_id])
    REFERENCES [dbo].[menu_items] ([id])
GO

IF NOT EXISTS (
    SELECT * FROM sys.foreign_keys WHERE name = 'FK_order_item_order'
)
    ALTER TABLE [dbo].[order_item]  WITH CHECK ADD  CONSTRAINT [FK_order_item_order] FOREIGN KEY([order_id])
    REFERENCES [dbo].[orders] ([id])
GO

IF NOT EXISTS (
    SELECT * FROM sys.check_constraints WHERE name = 'CK_order_item_quantity'
)
    ALTER TABLE [dbo].[order_item]  WITH CHECK ADD CONSTRAINT [CK_order_item_quantity] CHECK  (([quantity]>(0)))
GO

-- ORDERS TABLE
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[orders]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[orders](
        [id] [int] IDENTITY(1,1) NOT NULL,
        [cooking_requests] [nvarchar](300) NULL,
        [order_time] [datetime] NULL,
        [mode_of_payment] [varchar](10) NULL,
        [status] [varchar](10) NULL,
        [restaurant_id] [int] NULL,
        [customer_id] [int] NULL,
        PRIMARY KEY CLUSTERED ([id] ASC)
    )
END
GO

-- Add DEFAULT and FK constraints to orders
IF NOT EXISTS (
    SELECT * FROM sys.default_constraints WHERE name = 'DF_orders_order_time'
)
    ALTER TABLE [dbo].[orders] ADD  CONSTRAINT [DF_orders_order_time] DEFAULT (getdate()) FOR [order_time]
GO

IF NOT EXISTS (
    SELECT * FROM sys.foreign_keys WHERE name = 'FK_orders_customers'
)
    ALTER TABLE [dbo].[orders]  WITH CHECK ADD  CONSTRAINT [FK_orders_customers] FOREIGN KEY([customer_id])
    REFERENCES [dbo].[users] ([id])
GO

IF NOT EXISTS (
    SELECT * FROM sys.foreign_keys WHERE name = 'FK_orders_restaurant'
)
    ALTER TABLE [dbo].[orders]  WITH CHECK ADD  CONSTRAINT [FK_orders_restaurant] FOREIGN KEY([restaurant_id])
    REFERENCES [dbo].[restaurants] ([id])
GO

-- Add CHECK constraints to orders
IF NOT EXISTS (
    SELECT * FROM sys.check_constraints WHERE name = 'CK_orders_mode_of_payment'
)
    ALTER TABLE [dbo].[orders]  WITH CHECK ADD CONSTRAINT [CK_orders_mode_of_payment] CHECK  (([mode_of_payment]='card' OR [mode_of_payment]='upi'))
GO

IF NOT EXISTS (
    SELECT * FROM sys.check_constraints WHERE name = 'CK_orders_status'
)
    ALTER TABLE [dbo].[orders]  WITH CHECK ADD CONSTRAINT [CK_orders_status] CHECK  (([status]='delivered' OR [status]='rejected' OR [status]='accepted'))
GO

-- OWNER_RESTAURANT TABLE
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[owner_restaurant]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[owner_restaurant](
        [id] [int] IDENTITY(1,1) NOT NULL,
        [restaurant_id] [int] NULL,
        [owner_id] [int] NULL,
        PRIMARY KEY CLUSTERED ([id] ASC)
    )
END
GO

-- Add FK constraints to owner_restaurant
IF NOT EXISTS (
    SELECT * FROM sys.foreign_keys WHERE name = 'FK_owner_restaurant_owner'
)
    ALTER TABLE [dbo].[owner_restaurant]  WITH CHECK ADD  CONSTRAINT [FK_owner_restaurant_owner] FOREIGN KEY([owner_id])
    REFERENCES [dbo].[users] ([id])
GO

IF NOT EXISTS (
    SELECT * FROM sys.foreign_keys WHERE name = 'FK_owner_restaurant_restaurant'
)
    ALTER TABLE [dbo].[owner_restaurant]  WITH CHECK ADD  CONSTRAINT [FK_owner_restaurant_restaurant] FOREIGN KEY([restaurant_id])
    REFERENCES [dbo].[restaurants] ([id])
GO

-- RESTAURANTS TABLE
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[restaurants]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[restaurants](
        [id] [int] IDENTITY(1,1) NOT NULL,
        [name] [nvarchar](100) NOT NULL,
        [rating] [decimal](2, 1) NULL,
        [status] [varchar](10) NULL,
        [contact_id] [int] NOT NULL,
        PRIMARY KEY CLUSTERED ([id] ASC)
    )
END
GO

-- Add CHECK constraints to restaurants
IF NOT EXISTS (
    SELECT * FROM sys.check_constraints WHERE name = 'CK_restaurants_rating'
)
    ALTER TABLE [dbo].[restaurants]  WITH CHECK ADD CONSTRAINT [CK_restaurants_rating] CHECK  (([rating]>=(0.0) AND [rating]<=(5.0)))
GO

IF NOT EXISTS (
    SELECT * FROM sys.check_constraints WHERE name = 'CK_restaurants_status'
)
    ALTER TABLE [dbo].[restaurants]  WITH CHECK ADD CONSTRAINT [CK_restaurants_status] CHECK  (([status]='close' OR [status]='open'))
GO

-- USERS TABLE
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[users]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[users](
        [id] [int] IDENTITY(1,1) NOT NULL,
        [name] [nvarchar](100) NOT NULL,
        [role] [varchar](20) NULL,
        [contact_id] [int] NOT NULL,
        PRIMARY KEY CLUSTERED ([id] ASC)
    )
END
GO

-- Add FK and CHECK constraints to users
IF NOT EXISTS (
    SELECT * FROM sys.foreign_keys WHERE name = 'FK_restaurant_contact'
)
    ALTER TABLE [dbo].[users]  WITH CHECK ADD  CONSTRAINT [FK_restaurant_contact] FOREIGN KEY([contact_id])
    REFERENCES [dbo].[contact_details] ([id])
GO

IF NOT EXISTS (
    SELECT * FROM sys.foreign_keys WHERE name = 'FK_user_contact'
)
    ALTER TABLE [dbo].[users]  WITH CHECK ADD  CONSTRAINT [FK_user_contact] FOREIGN KEY([contact_id])
    REFERENCES [dbo].[contact_details] ([id])
GO

IF NOT EXISTS (
    SELECT * FROM sys.check_constraints WHERE name = 'CK_users_role'
)
    ALTER TABLE [dbo].[users]  WITH CHECK ADD CONSTRAINT [CK_users_role] CHECK  (([role]='super admin' OR [role]='owner' OR [role]='customer'))
GO





