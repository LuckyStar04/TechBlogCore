<img src="https://github.com/LuckyStar04/TechBlogCore.Vue/blob/master/src/assets/logo.png" height="60%" width="60%"/>

----

## 简介 | Introduction

采用 ASP.NET Core Rest API + EF Core + MariaDB + JWT 鉴权，最终部署在 CentOS，采用 Nginx 实现 https

需要项目预览，请移步 [这里](https://lhyy2022.xyz/)

Back-end based on ASP.NET Core Rest API + EF Core + MariaDB + JWT Authorization, deployed on CentOS, implemented https based on Nginx

If you want a preview please visit [here](https://lhyy2022.xyz/)

## Project Setup

### Install Database

For CentOS 7:

```sh
sudo yum install mariadb-server
```

### Specify Database Version for EF Core

For other Linux distro/Windows, you can install any version of MariaDB/MySQL, just need to modify `Program.cs`:

```cs
var sqlVersion = new MariaDbServerVersion(new Version(5, 5, 68)); //change this to your database version
```

### Configure Database ConnectionString

Configure your database connectionstring in `appsettings.Development.json` for development, or `appsettings.json` for production:

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;database=techblog;user=root;password=pswd"
}
```

### Update Database

```sh
cd TechBlogCore.RestApi
dotnet ef database update
```

### Compile and Run for Development

```sh
dotnet run
```

### Compile and Release for Production

```sh
dotnet publish
```
