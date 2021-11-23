# Pokemon Topic Crup API

DotNet aplication on WebApi for handling CRUD on pokemon

## Using 
- .Net 6.0.0
https://dotnet.microsoft.com/download/dotnet/6.0
- Visual Studio 2022
https://visualstudio.microsoft.com/es/launch/
- Mongo DB
https://cloud.mongodb.com/

## Step by Step

- Clone the repository 
`$ https://github.com/alejsherion/PokemonTopicCrupAPI.git`

if open with VisualStudio
- Right click in solution and select Restore Nugget packages

if open with VS Code
- Open terminal and write "dotnet restore"

Used for save secrets
`> dotnet user-secrets init`
`> dotnet user-secrets set MongoDbSetting:Password alejsherion > Password Database for configuration`



# CREATE BY Alejsherion


Aplicación para construcción API con NetCore 5

- NetCore 5
- MongoDB
- Docker

For Save passwords in user-secrets

API Healchecks
dotnet add package AspNetCore.HealthChecks.MongoDB

Docker
- Docker ps => lista los contenedores en ejecución
- Docker stop => detiene los contenedores en ejecución
- Docker images => lista las imagenes instaladas

- Crear contenedor para Base de datos
  * Docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=Pass#word1 mongo 
    => inicializa el contenedor de mongo con imagen

- Para crear el contenenedor de la APP 
  * Ctrl + P > Docker: Add docker files to workspaces => select tecnology => select OS => select Port => selectec docker compose
  * docker build -t catalog:v1 => publica el repositorio ApiRest
  * docker network create net5tutorial => crea la red entre los contenedores
  * docker network ls => lista las redes habilitadas

- volver a ejecutar el comando docker para ejecutar el contenedor de mongo adicionandole la red
  * Docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=Pass#word1 --network=net5tutorial mongo

- Instalar la imagen e inicializar la aplicación
  * docker build -t catalog:v1
  * docker run -it --rm -p 8080:80 -e MongoDbSettings:Host=mong -e MongoDbSettings:Password=Pass#word1 --network=net5tutorial catalog:v1

- Montar las imágenes en DockerHub
  * docker login
  * docker images => lista las imagenes
  * docker tag catalog:V1 alejsherion/catalog:v1 => agrega otra imagen para subir
  * docker push alejsherion/catalog:v1
  * docker rmi alejsherion/catalog:v1 => elimina la imagen de manera local
  * docker rmi catalog:v1 => elimina la imagen de manera local
  * docker pull para extraer los repositorios de imágenes


- Si no funciona la clave de registro setx KEY "abxd-efgh-ijkl-exyz" /M