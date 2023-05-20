# Pokerino

Simple unpolished Scrum poker web app written with dotnet and Blazor WASM utilizing SignalR.

![Logo!](/assets/pokerino.png "Logo")

## To run the app

You need a [Docker](https://docs.docker.com/desktop/) or your own PostgreSQL (if that's the case update connection string in `/Server/appsettings.json`)

```
docker-compose up
```

To migrate the database (schema - needed for a first time) run:

```
cd ./Server && dotnet ef database update
```

## Screens
![Login screen!](/assets/screen1.png "Login screen")
![Room screen!](/assets/screen2.png "Room screen")
