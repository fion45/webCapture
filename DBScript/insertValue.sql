insert into [FCStore.Models.FCStoreDbContext].[dbo].[Brands](NameStr,Name2,Tag,CountryCode) 
select NameStr,Name2,Tag,0 FROM FCStore.dbo.tb_brand 
where FCStore.dbo.tb_brand.BID > 
(SELECT COUNT(*) FROM [FCStore.Models.FCStoreDbContext].[dbo].[Brands])

insert into [FCStore.Models.FCStoreDbContext].[dbo].[Categories](ParCID,NameStr,Tag) 
select ParCID,NameStr,Tag FROM FCStore.dbo.tb_category 
where FCStore.dbo.tb_category.CID > 
(SELECT COUNT(*) FROM [FCStore.Models.FCStoreDbContext].[dbo].[Categories])

insert into [FCStore.Models.FCStoreDbContext].[dbo].[Products]([CID],[BID],[Title],Chose,Price,MarketPrice,Discount,Stock,Sale,ImgPath,Descript,Tag,[Date]) 
select CID,BrandID,Title,Chose,Price,MarketPrice,Discount,Stock,Sale,ImgPath,Descript,Tag,[Date] FROM FCStore.dbo.tb_product
where FCStore.dbo.tb_product.PID > 
(SELECT COUNT(*) FROM [FCStore.Models.FCStoreDbContext].[dbo].[Products]) 
AND FCStore.dbo.tb_product.BrandID <> -1 AND FCStore.dbo.tb_product.CID <> -1
 Order by FCStore.dbo.tb_product.Tag

insert [FCStore.Models.FCStoreDbContext].[dbo].[Columns] Values('推荐');
insert [FCStore.Models.FCStoreDbContext].[dbo].[Columns] Values('新品');
insert [FCStore.Models.FCStoreDbContext].[dbo].[Columns] Values('特卖');
insert [FCStore.Models.FCStoreDbContext].[dbo].[Columns] Values('热卖');

insert [FCStore.Models.FCStoreDbContext].[dbo].[ColumnProducts] Values(1,1284);
insert [FCStore.Models.FCStoreDbContext].[dbo].[ColumnProducts] Values(1,1285);
insert [FCStore.Models.FCStoreDbContext].[dbo].[ColumnProducts] Values(1,1286);
insert [FCStore.Models.FCStoreDbContext].[dbo].[ColumnProducts] Values(1,1287);
insert [FCStore.Models.FCStoreDbContext].[dbo].[ColumnProducts] Values(1,1288);
insert [FCStore.Models.FCStoreDbContext].[dbo].[ColumnProducts] Values(1,1289);
insert [FCStore.Models.FCStoreDbContext].[dbo].[ColumnProducts] Values(1,1290);