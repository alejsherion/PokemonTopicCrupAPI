# Pokemon Topic Crup API
> v 1.0

DotNet aplication on WebApi for handling CRUD on pokemon you use and also choosing your favorite ones through likes 

**¡Gotta Catch 'Em All!**

## Using 
- DotNet 6.0.0
https://dotnet.microsoft.com/download/dotnet/6.0
- Visual Studio  or VS Code
https://visualstudio.microsoft.com/es/launch/
https://code.visualstudio.com/download
- Mongo DB
https://cloud.mongodb.com/

## Debug
### Step by Step

- Clone the repository 
```
https://github.com/alejsherion/PokemonTopicCrupAPI.git
```

- if open with **Visual Studio**
    * Right click in solution and select Restore Nugget packages
    * Right click in solution and select Compile
    * Press F5 or In menu select Debug and Start debug
    
- if open with **VS Code**
  * Open *terminal* in root paht and write 
    ```
    dotnet restore
    ```
    For restaurate *Nuget's* from nuget package manager    

  * After write 
    ```
    dotnet build
    ```
    For compile solution and verify if ok

  * For start aplication write
    ```c#
    dotnet run
    ```

When the application is running we can access the application by **Swagger** or implement the consumption of services by Postman or the preferred service consumption client

It depends on the port that the configurator has in your IDE, by default in the project by Visual Studio port 1701 is configured for Https and 1702 for Http

```
https://localhost:1701
```
```
http://localhost:1702
```

de otra manera podemos probar la API con la Url pública de despliegue

[Public Site in Azure](https://webapicrudpokemon20211124163927.azurewebsites.net/)

![Swagger](https://i.ibb.co/dg4tgyX/Swagger.jpg)

But the operation of the API is not fully functional from Swagger because all services except for authentication and integration with the external API require authentication.

In case of using Postman, it is recommended to create two global variables

1. API client url (url to share between services)
```postman
{{%TOKEN%}}
```
2. Token (when logging in, only 1 record is updated)
```postman
{{%URL_WEBAPI_POKEMON%}}
```

![Global Variables](https://i.ibb.co/0yCZvPZ/Variables-globales.jpg)

**API Services List**

![Postman](https://i.ibb.co/M7QdH94/Collection-Postman.jpg)

The API provides the collection used in Postman as an aid.

[Postman Collection]("https://go.postman.co/workspace/My-Workspace~7df5ba93-b8dc-4fe1-874c-74312556f2b2/collection/3107392-194e408b-7039-42cf-8783-09b50ac6ed75")
```
https://go.postman.co/workspace/My-Workspace~7df5ba93-b8dc-4fe1-874c-74312556f2b2/collection/3107392-194e408b-7039-42cf-8783-09b50ac6ed75
```

**Authentication Controller**

Authentication controller is for registering users and logging in for API consumption

* Sign Up

>Method: POST

>Controller: Authentication

>Content-Type: application/json
```url
https://{{%URL_WEBAPI_POKEMON%}}/Athentication/SignIn
```
Body
```yaml
{
    "email":"alejandro@owner.e",
    "password":"Alejandro#2"
}
```
Response **Status 200 OK**
```
User created successfully!
```

![Sign Up](https://i.ibb.co/mJnWTmg/Authentication-Sign-Up.jpg)

* Sign In

>Method: POST

>Controller: Authentication

>Content-Type: application/json
```url
https://{{%URL_WEBAPI_POKEMON%}}/Athentication/SignIn
```
Body
```yaml
{
    "email":"alejandro@owner.e",
    "password":"Alejandro#2"
}
```
Response **Status 200 OK**
```yaml
{
    "email": "alejandro@owner.e",
    "password": "Alejandro#2",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImFsZWphbmRyb0Bvd25lci5lIiwibmJmIjoxNjM3NzAwMjQ2LCJleHAiOjE2Mzc3MDE0NDYsImlhdCI6MTYzNzcwMDI0Nn0.Qg2iOql63ithgYeLvzPZjeE0AwC28Al3XQSs10tnPmo"
}
```

A structure for password validation was defined where it must have:
Between 10 and 20 characters
At least 1 capital letter
At least 1 lowercase letter
At least 1 Special Character like! @ #?

![Sign In](https://i.ibb.co/pP2cZLp/Authentication-Sign-In.jpg)

**ALL METHODS THAT REQUIRE AUTHENTICATION REQUIRE THE PARAMETER TO BE PASSED THROUGH THE HEADBOARD**
![Header Authentication Token]()

In case the request is not authorized, it will deliver as a response.
Response **Status 401 Unauthorized**

**Pokemon Controller**

Pokemon controller where you can record these, you can interact on public records and you can create your own pokemons as if it were your own pokemon library

* Get All
>Method: GET

>Controller: Pokemon

>Content-Type: application/json
```url
https://{{%URL_WEBAPI_POKEMON%}}/Pokemon
```
Header
```c#
Authorization   Bearer {{%TOKEN%}}
Pagination      { "Page":1, "Records": 5 }
```
Response **Status 200 OK**
```yaml
{
    "page": 1,
    "records": 5,
    "pagesCount": 1,
    "result": [
        {
            "id": "bbed6e24-cd67-48df-a4cb-523dab82efc3",
            "pokedexOrder": 4,
            "name": "Charmander",
            "region": "Kanto",
            "type": "Fire",
            "createAt": "2021-11-21T23:37:48.6494273-05:00"
        }
    ]
}
```

This method lists all the pokemon saved by the user and additionally lists the pokemon that are in public state.

A form for paging was defined by implementing properties in the Header so that the consumption of the method was clean.

![Get All](https://i.ibb.co/DwVBj7f/Pokemon-Get-All.jpg)

* Get By Id
>Method: GET

>Controller: Pokemon

>Content-Type: application/json
```c#
https://{{%URL_WEBAPI_POKEMON%}}/Pokemon/{id}
```
Header
```c#
Authorization   Bearer {{%TOKEN%}}
```
Response **Status 200 OK**
```yaml
{
    "id": "bbed6e24-cd67-48df-a4cb-523dab82efc3",
    "pokedexOrder": 4,
    "name": "Charmander",
    "region": "Kanto",
    "type": "Fire",
    "createAt": "2021-11-21T23:37:48.6494273-05:00"
}
```
Response **Status 400 Not Found**
```
User can't get information of the pokemon that are not his own
```

This method consults a pokemon by Id but only that it belongs to the one making the request or it is a public pokemon.

![Get By Id](https://i.ibb.co/86W5WQY/Pokemon-Get-By-Id.jpg)

* Save
>Method: POST

>Controller: Pokemon

>Content-Type: application/json
```c#
https://{{%URL_WEBAPI_POKEMON%}}/Pokemon
```
Header
```c#
Authorization   Bearer {{%TOKEN%}}
```
Body
```yaml
{
    "pokedexOrder": 5,
    "name": "Charmeleon",
    "region": "Kanto",
    "type": "Fire"    
}
```
Response **Status 200 OK**
```yaml
{
    "id": "bbed6e24-cd67-48df-a4cb-523dab82efc3",
    "pokedexOrder": 4,
    "name": "Charmander",
    "region": "Kanto",
    "type": "Fire",
    "createAt": "2021-11-21T23:37:48.6494273-05:00"
}
```

![Save](https://i.ibb.co/80vFCYT/Pokemon-Save.jpg)

* Update
>Method: PUT

>Controller: Pokemon

>Content-Type: application/json
```c#
https://{{%URL_WEBAPI_POKEMON%}}/Pokemon/{id}
```
Header
```c#
Authorization   Bearer {{%TOKEN%}}
```
Body
```yaml
{
    "pokedexOrder": 5,
    "name": "CharmeleonMod",
    "region": "Kanto",
    "type": "Fire"    
}
```
Response **Status 200 OK**
```yaml
{
    "id": "bbed6e24-cd67-48df-a4cb-523dab82efc3",
    "pokedexOrder": 4,
    "name": "Charmander",
    "region": "Kanto",
    "type": "Fire",
    "createAt": "2021-11-21T23:37:48.6494273-05:00"
}
```
Response **Status 204 No Content** => Response Succesfully

Response **Status 400 Not Found**
```
User can't get information of the pokemon that are not his own
```

![Update](https://i.ibb.co/m0ZdK2R/Pokemon-Update.jpg)

* Remove
>Method: DELETE

>Controller: Pokemon

>Content-Type: application/json
```c#
https://{{%URL_WEBAPI_POKEMON%}}/Pokemon/{id}
```
Header
```c#
Authorization   Bearer {{%TOKEN%}}
```

Response **Status 200 OK**

Response **Status 400 Not Found**

```
User can't get information of the pokemon that are not his own
```

![Remove](https://i.ibb.co/SVNLzD9/Pokemon-Remove.jpg)

* Remove All Own
>Method: DELETE

>Controller: Pokemon

>Content-Type: application/json
```c#
https://{{%URL_WEBAPI_POKEMON%}}/Pokemon/RemoveAllOwn
```
Header
```c#
Authorization   Bearer {{%TOKEN%}}
```

Response **Status 200 OK**

This method deletes all the pokemon that the user making the request has saved

![RemoveAllOwn](https://i.ibb.co/bgLKZTK/Pokemon-Remove-All-Own.jpg)

**Like Controller**

This controller is designed to manage user likes with the pokemon that belong to it or public

* Get All
>Method: GET

>Controller: Like

>Content-Type: application/json
```url
https://{{%URL_WEBAPI_POKEMON%}}/Like
```
Header
```c#
Authorization   Bearer {{%TOKEN%}}
Pagination      { "Page":1, "Records": 5 }
```
Response **Status 200 OK**
```yaml
{
    "page": 1,
    "records": 5,
    "pagesCount": 0,
    "result": [
        {
            "id": "ca680ff1-fe4e-4502-9af6-8f3a3b962ae3",
            "pokemonId": "bbed6e24-cd67-48df-a4cb-523dab82efc3",
            "user": "alejandro@owner.e",
            "createAt": "0001-01-01T00:00:00+00:00"
        }
    ]
}
```

This method lists all the likes associated with Pokémon saved by the user and / or public.

A form for paging was defined by implementing properties in the Header so that the consumption of the method was clean.

![Get All](https://i.ibb.co/R6QyT1f/Like-Get-All.jpg)

* Save
>Method: POST

>Controller: Like

>Content-Type: application/json
```url
https://{{%URL_WEBAPI_POKEMON%}}/Like
```
Header
```c#
Authorization   Bearer {{%TOKEN%}}
```
Body
```
{
    "PokemonId":"bbed6e24-cd67-48df-a4cb-523dab82efc3"
}
```
Response **Status 200 OK**
```yaml
{
    "id": "ca680ff1-fe4e-4502-9af6-8f3a3b962ae3",
    "pokemonId": "bbed6e24-cd67-48df-a4cb-523dab82efc3",
    "user": "alejandro@owner.e",
    "createAt": "0001-01-01T00:00:00+00:00"
}
```

Response **Status 400 Not Found**

```
User can't like the pokemon that are not his own or public
```

This method lists all the likes associated with Pokémon saved by the user and / or public.

![Save](https://i.ibb.co/1KTPMmn/Like-Save.jpg)

* Remove
>Method: DELETE

>Controller: Like

>Content-Type: application/json
```url
https://{{%URL_WEBAPI_POKEMON%}}/Like/{id}
```
Header
```c#
Authorization   Bearer {{%TOKEN%}}
```
Response **Status 200 OK**

Response **Status 400 Not Found**

```
User can't remove the Like that are not his own
```

This method removes a previously placed like on a Pokemon, but only on own or public Pokemon.

![Remove](https://i.ibb.co/JB7wjGn/Like-Remove.jpg)

* RemoveAllLike
>Method: DELETE

>Controller: Like

>Content-Type: application/json
```url
https://{{%URL_WEBAPI_POKEMON%}}/Like/RemoveAllLike
```
Header
```c#
Authorization   Bearer {{%TOKEN%}}
```
Response **Status 200 OK**

This method removes all the likes that the user has added

![Remove](https://i.ibb.co/m00y10C/Like-Remove-All.jpg)

## Unit Test

XUnit

## Deploy

Docker

### Topics

- Used for save word secrets *User Secrets*
```bash
dotnet user-secrets init
```
*Example:* For save password database 
```bash
dotnet user-secrets set MongoDbSetting:Password somePassword
``` 

API Healchecks

The application has the endpoints to validate its health status, verify operation and connection with the database

```
dotnet add package AspNetCore.HealthChecks.MongoDB
```

Application status
```
https://{{%URL_WEBAPI_POKEMON%}}/health/live
```
![Live](https://i.ibb.co/wy2wBFL/Healthy-Live.jpg)

Database connection status verification
```
https://{{%URL_WEBAPI_POKEMON%}}/health/ready
```
![Live](https://i.ibb.co/MGGZx7h/Healthy-Ready.jpg)

## External Client Usability

An external Rest API was used that provides detailed information about the Pokemon
As they describe it themselves in their API

>This is a full RESTful API linked to an extensive database detailing everything about the Pokémon main game series.
We've covered everything from Pokémon to Berry Flavors.

```
https://pokeapi.co/
```

And this Documentation
```
https://pokeapi.co/docs/v2
```

In this way it was implemented, but information was filtered by the most relevant properties related to a specific Pokemon

**Pokemon Controller**

* GetInfoPokeAPI
>Method: GET

>Controller: Pokemon

>Content-Type: application/json
```url
https://{{%URL_WEBAPI_POKEMON%}}/Pokemon/GetInfoPokeAPI?pokemonName={namepokemon}
```
Response **Status 200 OK**
```yaml
{
    "id": 132,
    "height": 3,
    "weight": 40,
    "location_area_enconunters": null,
    "name": "ditto",
    "order": 203,
    "sprites": {
        "back_default": "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/back/132.png",
        "back_female": null,
        "back_shiny": "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/back/shiny/132.png",
        "back_shiny_female": null,
        "front_default": "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/132.png",
        "front_female": null,
        "front_shiny": "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/shiny/132.png",
        "front_shiny_female": null
    }
}
```
![Sprite2](https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/132.png)
![Sprite1](https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/back/132.png)
![Sprite2](https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/shiny/132.png)
![Sprite2](https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/back/shiny/132.png)

![GetInfoPokeAPI](https://i.ibb.co/d6WxbG7/Pokemon-Get-Info-Poke-API.jpg)


## License 
[Alejsherion](https://www.linkedin.com/in/alejvarelectronicing/)
