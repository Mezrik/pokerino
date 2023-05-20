# Pokerino

Simple unpolished Scrum poker app written with dotnet utilizing SignalR for realtime state distribution.

## To run the app

You need a [Docker](https://docs.docker.com/desktop/) or your own PostgreSQL (if that's the case update connection string in `/Server/appsettings.json`)

```
docker-compose up
```

```
cd ./Server && dotnet ef database update
```
