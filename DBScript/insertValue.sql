insert into [FCStoreWeb].[dbo].[Brands](NameStr,Name2,CountryCode,Tag,Important) 
select NameStr,Name2,0,Tag,0 FROM FCStore.dbo.tb_brand 
where FCStore.dbo.tb_brand.BID > 
(SELECT COUNT(*) FROM [FCStoreWeb].[dbo].[Brands])

insert into [FCStoreWeb].[dbo].[Categories](ParCID,NameStr,Tag) 
select ParCID,NameStr,Tag FROM FCStore.dbo.tb_category 
where FCStore.dbo.tb_category.CID > 
(SELECT COUNT(*) FROM [FCStoreWeb].[dbo].[Categories])

insert into [FCStoreWeb].[dbo].[Products]([CID],[BID],[Title],Chose,Price,MarketPrice,Discount,Stock,Sale,ImgPath,PVCount,Descript,[Date],Tag,EvaluationStarCount) 
select CID,BrandID,Title,Chose,Price,MarketPrice,1,Stock,Sale,ImgPath,0,Descript,[Date],Tag,10 FROM FCStore.dbo.tb_product
where FCStore.dbo.tb_product.PID > 
(SELECT COUNT(*) FROM [FCStoreWeb].[dbo].[Products]) 
--AND FCStore.dbo.tb_product.BrandID <> -1 AND FCStore.dbo.tb_product.CID <> -1
AND FCStore.dbo.tb_product.BrandID > 0 AND FCStore.dbo.tb_product.CID > 0
 Order by FCStore.dbo.tb_product.Tag

insert [FCStoreWeb].[dbo].[Columns] Values('限时抢购','正品+ 便宜','极速到货，免运费');
insert [FCStoreWeb].[dbo].[Columns] Values('新品上市','最新+ 最潮','把握脉搏,紧跟潮流,走在时代尖端');
insert [FCStoreWeb].[dbo].[Columns] Values('团购优惠','优质+ 省钱','越多人买，越便宜');
insert [FCStoreWeb].[dbo].[Columns] Values('健康宝宝','安全+ 放心','极速到货，免运费');


insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(1,63,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(1,64,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(1,65,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(1,66,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(1,67,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(1,68,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(1,69,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(1,2000,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(1,510,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(1,1000,1,1,0);

insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(2,63,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(2,64,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(2,65,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(2,66,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(2,67,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(2,68,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(2,69,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(2,2000,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(2,510,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(2,1000,1,1,0);

insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(3,63,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(3,64,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(3,65,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(3,66,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(3,67,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(3,68,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(3,69,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(3,2000,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(3,510,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(3,1000,1,1,0);

insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(4,63,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(4,64,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(4,65,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(4,66,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(4,67,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(4,68,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(4,69,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(4,2000,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(4,510,1,1,0);
insert [FCStoreWeb].[dbo].[ReColumnProducts] Values(4,1000,1,1,0);

insert [FCStoreWeb].[dbo].[Roles] Values('admin','10000','admin','ALL');
insert [FCStoreWeb].[dbo].[Roles] Values('saler','500','saler','ALL');
insert [FCStoreWeb].[dbo].[Roles] Values('client','100','client','ALL');

insert [FCStoreWeb].[dbo].[Users] Values('fion','fion','12348765','86945494@qq.com',0,NULL,'',100,NULL,NULL,NULL,NULL);
insert [FCStoreWeb].[dbo].[Users] Values('fion','1','111111','86945494@qq.com',0,NULL,'',100,NULL,NULL,NULL,NULL);
insert [FCStoreWeb].[dbo].[Users] Values('test','2','222222','test@qq.com',0,NULL,'',100,NULL,NULL,NULL,NULL);

insert [FCStoreWeb].[dbo].[RoleUsers] Values(1,1);
insert [FCStoreWeb].[dbo].[RoleUsers] Values(1,2);
insert [FCStoreWeb].[dbo].[RoleUsers] Values(1,3);

--insert [FCStoreWeb].[dbo].[Addresses] Values('test',1,'城门头西路2号之2 803','18923230566','528000',1)

--insert [FCStoreWeb].[dbo].[Addresses] Values('test',1,'城门头西路2号之2 803','18923230566','528000',2)

--insert [FCStoreWeb].[dbo].[Addresses] Values('test',1,'城门头西路2号之2 803','18923230566','528000',2)

--insert [FCStoreWeb].[dbo].[Addresses] Values('test',1,'城门头西路2号之2 803','18923230566','528000',2)

insert [FCStoreWeb].[dbo].[BannerItems] Values('浪漫七夕','浪漫七夕', '/picture/banner/0.jpg',0,'')

insert [FCStoreWeb].[dbo].[BannerItems] Values('浪漫七夕','浪漫七夕', '/picture/banner/1.jpg',1,'')

insert [FCStoreWeb].[dbo].[BannerItems] Values('浪漫七夕','浪漫七夕', '/picture/banner/2.jpg',2,'')

insert [FCStoreWeb].[dbo].[BannerItems] Values('浪漫七夕','浪漫七夕', '/picture/banner/3.jpg',3,'')

insert [FCStoreWeb].[dbo].[BannerItems] Values('浪漫七夕','浪漫七夕', '/picture/banner/4.jpg',4,'')

insert [FCStoreWeb].[dbo].[BannerItems] Values('浪漫七夕','浪漫七夕', '/picture/banner/5.jpg',5,'')

insert [FCStoreWeb].[dbo].[PushInfoes] Values('2014-07-31',1,50,165,49)
insert [FCStoreWeb].[dbo].[PushInfoes] Values('2014-07-31',1,51,11,49)
insert [FCStoreWeb].[dbo].[PushInfoes] Values('2014-07-31',1,52,20,60)
insert [FCStoreWeb].[dbo].[PushInfoes] Values('2014-07-31',1,53,1,154)
insert [FCStoreWeb].[dbo].[PushInfoes] Values('2014-07-31',1,54,1,154)
insert [FCStoreWeb].[dbo].[PushInfoes] Values('2014-07-31',1,55,1,154)
insert [FCStoreWeb].[dbo].[PushInfoes] Values('2014-07-31',1,50,165,49)
