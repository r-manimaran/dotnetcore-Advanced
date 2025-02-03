# EFCore Migrations Tools update in .Net 9
- Contains different Entity Framework Migration options using EFCore Tools.

- Create a new Model Customer
- Setup DbContext
- Before applying the migrations check for model changes.

![alt text](Images/image.png)

- Create new initial Migration

![alt text](Images/image-1.png)

Add new Column to the existing Model
![alt text](Images/image-2.png)

- Check for model changes

![alt text](Images/image-3.png)

- Revert back the changes in the model.
```csharp
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }   
}
```
- Now check for the model change

![alt text](Images/image-4.png)

## Update to the Database

```bash
# update the migrations to the database
> dotnet ef database update
```
Alternatively we can get the script in the context to a sql file using below

```bash
# get Sql statement for the migration
> dotnet ef dbcontext script -o update.sql
```
![alt text](Images/image-5.png)

## List Migrations

```bash
> dotnet ef migrations list
# this will generate script for the db objects along with MigrationHistory table
> dotnet ef migrations script -o update.sql
```
![alt text](Images/image-6.png)

![alt text](Images/image-7.png)

- use -i flag for Idempotent script

## Apply the database update

```bash
> dotnet ef database update
```
- After database update

![alt text](Images/image-8.png)

- Now update the model to have a new property and create a new migration

![alt text](Images/image-9.png)

![alt text](Images/image-10.png)

```bash
# generate the migration script after the initial migration
> dotnet ef migrations script 20250203202458_initial -o update.sql 
```
![alt text](Images/image-11.png)

- With idempotent flag -i the above command will create the script with If exists check.

```bash
# generate script with idempotent flag
> dotnet ef migrations script 20250203202458_initial -o update.sql -i
```
![alt text](Images/image-12.png)

## Create Bundle migrations
Using this option you can create a bundle (an .exe file) which contains a migration.
```bash
> dotnet ef migrations bundle
```

![alt text](Images/image-13.png)

- Copy and share the efbundle.exe to run the migration.
- Note: Ensure the appSettings.json is also present in the same folder when running the exe.
- Or use Environment variables or ConnectionString from Azure KeyVault instead of appSettings.json also.

![alt text](Images/image-14.png)

![alt text](Images/image-15.png)

## Create Compiled model for optimized quering

```bash
> dotnet ef dbcontext optimize -o ./Data/Optimized/
```
- The above command will create a Optimized folder will compiled models.

![alt text](Images/image-16.png)

![alt text](Images/image-17.png)

- So, when everytime the model changes run the above command to recreate the compiled models for better performance.
