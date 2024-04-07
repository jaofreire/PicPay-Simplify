
# PicPay Simplify

Application maked for a code exam by PicPay, [PicPay Challenge Repository](https://github.com/PicPay/picpay-desafio-backend?tab=readme-ov-file)



## Run the API

First you must pull the API-IMAGE in the Docker and expose an Port, follow the steps:
### Containers

```cmd
  docker pull jaofreire/picpaysimplify:latest
```

After that, clone the project repository, [PicPaySimplify Repository](https://github.com/jaofreire/PicPay-Simplify-Repository)

Save in anywhere what you prefer and init CMD with the past where the Docker Compose its saved 

In CMD just run the compose

```cmd
  docker compose up --build
```

The necessaries containers and images will be create and the Ports will be expose,

in this case the ports expose in the Docker Compose are configured this way: 

```cmd
  3091:8081
  3090:8080
```

### SQl SERVER

The SqlServer will be initializate with the default  User: sa, Password: @Sa1234567,

but can be changed in Docker Compose.

The Db_PicPaySimplify for default is not initializate in SqlServer, so, you must access the Swagger of API 

```url
  https://localhost:3091/swagger
```
 and run the MigrationUpdate Method,

 This method will do a migration and Add Db_PicPaySimplify in SqlServer

 Right now, the API is ready to work










## EndPoints



Base Url: https://localhost:3091

## User

#### Return all users
```http
  GET /user/getAll
```

User Model:
```model
   {
    "id": 0,
    "name": "string",
    "document": "string",
    "documentNumber": "string",
    "email": "string",
    "password": "string",
    "userType": "string",
    "balance": 0
  }
```

#### Register new user

```http
  POST user/register
```

| Parâmetro   | Tipo       | 
| :---------- | :--------- | 
| `userModel`      | `UserModel` |


```http
  DELETE user/remove/{id}
```

| Parâmetro   | Tipo       |
| :---------- | :--------- | 
| `id`      | `int` |


## Transaction

#### Return all transactions
```http
  GET /transaction/getAll
```

Transaction Model:
```model

  {
    "id": 0,
    "payerId": 0,
    "payer": {
      "id": 0,
      "name": "string",
      "email": "string",
      "userType": "string",
      "balance": 0
    },
    "receiverId": 0,
    "receiver": {
      "id": 0,
      "name": "string",
      "email": "string",
      "userType": "string",
      "balance": 0
    },
    "value": 0
  }

```
TransactionPayLoudDTO Model:
```model

{
  "payerId": 0,
  "receiverId": 0,
  "value": 0
}

```
#### Create new transaction
```http
  POST /transaction/create
```

| Parâmetro   | Tipo       |
| :---------- | :--------- | 
| `payLoud`      | `TransactionPayLoudDTO` |
 
