create procedure InsertIntoProduct
(
	@Name nvarchar(100),
	@Sku varchar(100)
)
as
Begin
	insert into Product([Name],Sku) 
	values (@Name,@Sku)
End
Go

--select * from Product


create procedure GetProductById
	@Id int
as
Begin
	select * from Product 
	where Id = @Id
End
Go


create procedure UpdateProduct
(
	@Id int,
	@Name nvarchar(100),
	@Sku varchar(50)
)
as
Begin
	update Product set [Name] = @Name, Sku = @Sku
	where Id = @Id
End
Go


Create Procedure RemoveProduct
	@Id int
as
begin
	Delete from Product
	where id = @Id
end
Go