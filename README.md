# Ankara
This is Web API of example Ankara application. The idea is to show Docker MS Sql Server sturtup for developer machine.
Is is also shows an exmple of configuration Web API .NET Core 3.1 Controlles Examples, configurated for Active Direcotry authorizations.
(Not Azure AD, but traditional AD)

## Start
The main docker compose file is in root directory of a repository. 
Firs one has to create local network:

```
docker network create retager
```

For developer purposes of a `Frontend` (https://compentio.visualstudio.com/Retager/_git/gui) application 
one can start Backend using `docker-compose` (see more: https://docs.docker.com/compose/) command:

```
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --build
``` 
Read more: https://docs.microsoft.com/pl-pl/dotnet/architecture/microservices/multi-container-microservice-net-applications/multi-container-applications-docker-compose

To stop container, use
```
docker-compose -f docker-compose.yml -f docker-compose.override.yml down
```

## DataBase - Fluent Migrator (https://fluentmigrator.github.io/articles/intro.html)
For this purpose used Application `Compentio.Ankara.Database`, that creates database schema and data. Container name for this is `retager-db-init` it runs once and stops.

To update database, use:

```
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --build retager-db-init
``` 

## Frontend Application
Fronend is written in `Angular9` framework with material components: http://material.angular.io.
For developer purposes Backed exposes https://localhost:1443 URL.

##Trobleshouting
For problems with connection between containers, please, check `retager` network settings for that containers, e.g. here we have:

```
"Containers": {
        "1f887a1c36ad53196cff357a3fa56a2a5d431be423e984b9ab7970bd781fe0d0": {
            "Name": "retager-db",
            "EndpointID": "48484b2d8dd103285c12cfd62d9465b7999d7f992dc1907059b42ff546aec110",
            "MacAddress": "02:42:c0:a8:e0:02",
            "IPv4Address": "192.168.224.2/20",
            "IPv6Address": ""
        },

```

for `retager-db` container. Thus, connection strins is `Data Source=192.168.224.2;Initial Catalog=retager; ....`

## Visual Studio development
While developer wants to debug in Docker container API application, in ConnectionString there should be used Docker IP 
Adress from hosts file `c:\Windows\System32\Drivers\etc\hosts`:

```
Added by Docker Desktop
192.168.8.105 host.docker.internal

```

##IIS Express Windows Authentications

In a path `$(solutionDir)\.vs\{projectName}\config\applicationhost.config` change section `windowsAuthentication` and `anonymousAuthentication` for `overrideModeDefault="Allow"`:

```
<sectionGroup name="authentication">
    <section name="anonymousAuthentication" overrideModeDefault="Allow" />
    <section name="basicAuthentication" overrideModeDefault="Deny" />
    <section name="clientCertificateMappingAuthentication" overrideModeDefault="Deny" />
    <section name="digestAuthentication" overrideModeDefault="Deny" />
    <section name="iisClientCertificateMappingAuthentication" overrideModeDefault="Deny" />
    <section name="windowsAuthentication" overrideModeDefault="Allow" />
</sectionGroup>
```

## MalHog
Container used for testing mailing service. More about MailHog here: https://github.com/mailhog/MailHog.
MailHog SMTP server starts on port 1025 and the HTTP server starts on port 8025.
MailHog in-memory stores messages.