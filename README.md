# Aws-fun

A playground for getting familiar with some AWS stuff which is used quite heavily at work

- CDKs
- Lambdas
- SQS

Looking to build something simple and get it deployed to AWS to get familiar with the different AWS services and how to interact with them.

# How to run

Set your environment variable
```
    export MONGO_CONNECTION_STRING="mongodb://localhost:27017"
```

Spin up the db
```
    docker compose up -d
```

Spin up the API
```
    cd src/Ufpls.Api
    dotnet run
```

For running the lambda too, see the Readme.md in the lambda project