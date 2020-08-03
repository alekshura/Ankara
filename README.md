# Ankara
This is Web API of example Ankara application. 
The overall idea is to show how we have used MS SQL Server Docker Image in development process. 
It is also more convenient to use contenirised database, create databases, users, schemas, load data using one command. 
This job can be done using one Docker container `db-init`.

## Start
The main docker compose file is in root directory of a repository. 
Firs one has to create local network:

```
docker network create retager
```
To start Backend using `docker-compose` (see more: https://docs.docker.com/compose/) command:

```
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --build
``` 
Read more: https://docs.microsoft.com/pl-pl/dotnet/architecture/microservices/multi-container-microservice-net-applications/multi-container-applications-docker-compose

To stop container, use
```
docker-compose -f docker-compose.yml -f docker-compose.override.yml down
```

In this way you are starting 3 containers:
 - ankara-api
 - ankara-db-init
 - ankara-db
 
`ankara-api` is Web API container, that serves Web API;
`ankara-db-init` - container for Database initialization. 
It depends on `ankara-db` - we've used MS SQL Server 2017 Image (mcr.microsoft.com/mssql/server:2017-latest). 

All work for creating databases done in `docker-entrypoint.sh`, which starts MS SQL server and executes script from `ankara-db-init.sql` using
`/db-init.sh & /opt/mssql/bin/sqlservr`. 

In such a way `ankara-db-init` starts MS SQL Server, creates the Datababase and users. The last of the job (Db schema, data, etc.) FluentMigator does.
About FM please read here: https://fluentmigrator.github.io/articles/intro.html.

After all migration have been done, ankara-db-init stops his work. 

Job's done. You have you database, you can create more databses in `ankara-db-init.sql`.
