# Pokemon Topic Crup API

DotNet aplication on WebApi for handling CRUD on pokemon

## Using 
- .Net 6.0.0
https://dotnet.microsoft.com/download/dotnet/6.0
- Visual Studio  or VS Code
https://visualstudio.microsoft.com/es/launch/
https://code.visualstudio.com/download
- Mongo DB
https://cloud.mongodb.com/

## Step by Step

- Clone the repository 
`https://github.com/alejsherion/PokemonTopicCrupAPI.git`

- if open with VisualStudio
    * Right click in solution and select Restore Nugget packages

- if open with VS Code
    * Open terminal and write "dotnet restore"


## Unit Test

## Deploy

## Debug

### Topics

- Used for save secrets
```bash
dotnet user-secrets init
```
```bash
dotnet user-secrets set MongoDbSetting:Password alejsherion
``` 
API Healchecks
dotnet add package AspNetCore.HealthChecks.MongoDB


## License 
[Alejsherion](https://www.linkedin.com/in/alejvarelectronicing/)