insert into [FCStore.Models.FCStoreDbContext].[dbo].[Brands](NameStr,Name2,Tag,CountryCode) 
select NameStr,Name2,Tag,0 FROM FCStore.dbo.tb_brand 
where FCStore.dbo.tb_brand.BID > 
(SELECT COUNT(*) FROM [FCStore.Models.FCStoreDbContext].[dbo].[Brands])

insert into [FCStore.Models.FCStoreDbContext].[dbo].[Categories](ParCID,NameStr,Tag) 
select ParCID,NameStr,Tag FROM FCStore.dbo.tb_category 
where FCStore.dbo.tb_category.CID > 
(SELECT COUNT(*) FROM [FCStore.Models.FCStoreDbContext].[dbo].[Categories])

insert into [FCStore.Models.FCStoreDbContext].[dbo].[Products]([CID],[BID],[Title],Chose,Price,MarketPrice,Discount,Stock,Sale,ImgPath,Descript,Tag,[Date],PVCount) 
select CID,BrandID,Title,Chose,Price,MarketPrice,1,Stock,Sale,ImgPath,Descript,Tag,[Date],0 FROM FCStore.dbo.tb_product
where FCStore.dbo.tb_product.PID > 
(SELECT COUNT(*) FROM [FCStore.Models.FCStoreDbContext].[dbo].[Products]) 
AND FCStore.dbo.tb_product.BrandID <> -1 AND FCStore.dbo.tb_product.CID <> -1
 Order by FCStore.dbo.tb_product.Tag

insert [FCStore.Models.FCStoreDbContext].[dbo].[Columns] Values('推荐');
insert [FCStore.Models.FCStoreDbContext].[dbo].[Columns] Values('新品');
insert [FCStore.Models.FCStoreDbContext].[dbo].[Columns] Values('特卖');
insert [FCStore.Models.FCStoreDbContext].[dbo].[Columns] Values('热卖');

insert [FCStore.Models.FCStoreDbContext].[dbo].[ColumnProducts] Values(1,63);
insert [FCStore.Models.FCStoreDbContext].[dbo].[ColumnProducts] Values(1,64);
insert [FCStore.Models.FCStoreDbContext].[dbo].[ColumnProducts] Values(1,65);
insert [FCStore.Models.FCStoreDbContext].[dbo].[ColumnProducts] Values(1,66);
insert [FCStore.Models.FCStoreDbContext].[dbo].[ColumnProducts] Values(1,67);
insert [FCStore.Models.FCStoreDbContext].[dbo].[ColumnProducts] Values(1,68);
insert [FCStore.Models.FCStoreDbContext].[dbo].[ColumnProducts] Values(1,69);

insert [FCStore.Models.FCStoreDbContext].[dbo].[Roles] Values('admin','10000','admin','ALL');
insert [FCStore.Models.FCStoreDbContext].[dbo].[Roles] Values('saler','500','saler','ALL');
insert [FCStore.Models.FCStoreDbContext].[dbo].[Roles] Values('client','100','client','ALL');

insert [FCStore.Models.FCStoreDbContext].[dbo].[Users] Values('fion','1','1','test@qq.com',NULL,'');
insert [FCStore.Models.FCStoreDbContext].[dbo].[Users] Values('test','2','2','test@qq.com',NULL,'');

insert [FCStore.Models.FCStoreDbContext].[dbo].[RoleUsers] Values(1,1);
insert [FCStore.Models.FCStoreDbContext].[dbo].[RoleUsers] Values(1,2);

insert [FCStore.Models.FCStoreDbContext].[dbo].[Areas] Values(502,'中国','广东省','佛山市','禅城区',528000)

insert [FCStore.Models.FCStoreDbContext].[dbo].[Addresses] Values(1,'城门头西路2号之2 803','18923230566',1)

insert [FCStore.Models.FCStoreDbContext].[dbo].[Addresses] Values(1,'城门头西路2号之2 803','18923230566',2)

insert [FCStore.Models.FCStoreDbContext].[dbo].[Addresses] Values(1,'城门头西路2号之2 803','18923230566',2)

insert [FCStore.Models.FCStoreDbContext].[dbo].[Addresses] Values(1,'城门头西路2号之2 803','18923230566',2)
