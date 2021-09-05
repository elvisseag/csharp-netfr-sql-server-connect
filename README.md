# elvisseag-csharp-netfr-sql-server-connect
CSharp Console App with SQL Server Connection

---

* Create **keys.config** file and put it next to the .EXE file
```
<appSettings>
  <add key="SERVER" value="my_server" />
  <add key="DATABASE" value="my_database" />
  <add key="USER" value="my_user" />
  <add key="PASSWORD" value="my_password" />
</appSettings>

```
---

* Create **connections.config** file and put it next to the .EXE file
```
<connectionStrings>
  <add name="MY_CONNECTION"
       connectionString="server=my_server;database=my_database;user id=my_user; password=my_password;"
       providerName="System.Data.SqlClient"/>
</connectionStrings>
```
