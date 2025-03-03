## Library_Sqlite
ASP.NET Core Web API Biblioteca

![Library](img/1.png)
![Library](img/2.png)


## Program
``` 
var connectionString = builder.Configuration.GetConnectionString("Connection");

builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlite(connectionString);
);
``` 

## appsetting.Development.json
``` 
{
  "ConnectionStrings": {
    "Connection": "Data Source=library.db"
  }
}
``` 


