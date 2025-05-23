## Library_Sqlite
ASP.NET Core Web API Biblioteca

![Library](img/1.png)
![Library](img/2.png)


## Program
```cs 
var connectionString = builder.Configuration.GetConnectionString("Connection");

builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlite(connectionString);
);
``` 

## appsetting.Development.json
```cs 
{
  "ConnectionStrings": {
    "Connection": "Data Source=library.db"
  }
}
``` 

[DeepWiki moraisLuismNet/Library_Sqlite](https://deepwiki.com/moraisLuismNet/Libray_Sqlite)



