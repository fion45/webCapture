insert into [FCStoreWeb].[dbo].[Brands](NameStr,Name2,CountryCode,Tag,Important,Column_ColumnID) 
select NameStr,Name2,0,Tag,0,null FROM FCStore.dbo.tb_brand 
where FCStore.dbo.tb_brand.BID > 
(SELECT COUNT(*) FROM [FCStoreWeb].[dbo].[Brands])

insert into [FCStoreWeb].[dbo].[Categories](ParCID,NameStr,Tag) 
select ParCID,NameStr,Tag FROM FCStore.dbo.tb_category 
where FCStore.dbo.tb_category.CID > 
(SELECT COUNT(*) FROM [FCStoreWeb].[dbo].[Categories])

insert into [FCStoreWeb].[dbo].[Products]([CID],[BID],[Title],Chose,Price,MarketPrice,Discount,Stock,Sale,ImgPath,PVCount,Descript,[Date],Tag) 
select CID,BrandID,Title,Chose,Price,MarketPrice,1,Stock,Sale,ImgPath,0,Descript,[Date],Tag FROM FCStore.dbo.tb_product
where FCStore.dbo.tb_product.PID > 
(SELECT COUNT(*) FROM [FCStoreWeb].[dbo].[Products]) 
AND FCStore.dbo.tb_product.BrandID <> -1 AND FCStore.dbo.tb_product.CID <> -1
 Order by FCStore.dbo.tb_product.Tag

insert [FCStoreWeb].[dbo].[Columns] Values('推荐');
insert [FCStoreWeb].[dbo].[Columns] Values('新品');
insert [FCStoreWeb].[dbo].[Columns] Values('特卖');
insert [FCStoreWeb].[dbo].[Columns] Values('热卖');

insert [FCStoreWeb].[dbo].[ColumnProducts] Values(1,63);
insert [FCStoreWeb].[dbo].[ColumnProducts] Values(1,64);
insert [FCStoreWeb].[dbo].[ColumnProducts] Values(1,65);
insert [FCStoreWeb].[dbo].[ColumnProducts] Values(1,66);
insert [FCStoreWeb].[dbo].[ColumnProducts] Values(1,67);
insert [FCStoreWeb].[dbo].[ColumnProducts] Values(1,68);
insert [FCStoreWeb].[dbo].[ColumnProducts] Values(1,69);
insert [FCStoreWeb].[dbo].[ColumnProducts] Values(1,2000);
insert [FCStoreWeb].[dbo].[ColumnProducts] Values(1,510);
insert [FCStoreWeb].[dbo].[ColumnProducts] Values(1,1000);

insert [FCStoreWeb].[dbo].[Roles] Values('admin','10000','admin','ALL');
insert [FCStoreWeb].[dbo].[Roles] Values('saler','500','saler','ALL');
insert [FCStoreWeb].[dbo].[Roles] Values('client','100','client','ALL');

insert [FCStoreWeb].[dbo].[Users] Values('fion','1','111111','test@qq.com',NULL,'',100);
insert [FCStoreWeb].[dbo].[Users] Values('test','2','222222','test@qq.com',NULL,'',100);

insert [FCStoreWeb].[dbo].[RoleUsers] Values(1,1);
insert [FCStoreWeb].[dbo].[RoleUsers] Values(1,2);

insert [FCStoreWeb].[dbo].[Addresses] Values('test',1,'城门头西路2号之2 803','18923230566','528000',1)

insert [FCStoreWeb].[dbo].[Addresses] Values('test',1,'城门头西路2号之2 803','18923230566','528000',2)

insert [FCStoreWeb].[dbo].[Addresses] Values('test',1,'城门头西路2号之2 803','18923230566','528000',2)

insert [FCStoreWeb].[dbo].[Addresses] Values('test',1,'城门头西路2号之2 803','18923230566','528000',2)
