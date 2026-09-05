# TaskProcessor

API .NET para processamento assíncrono de tarefas: a requisição publica a tarefa numa fila do RabbitMQ e um worker processa em segundo plano, gravando o status no MongoDB.

[![CI](https://github.com/jvitorsi-dev/TaskProcessor/actions/workflows/ci.yml/badge.svg)](https://github.com/jvitorsi-dev/TaskProcessor/actions/workflows/ci.yml)

## Sobre

Projeto pessoal para estudar mensageria e separação de responsabilidades entre serviços. O código está dividido em camadas (Domain, Application, Infrastructure, API e Worker), com testes de unidade no `TaskProcessor.Tests`.

## Tecnologias

- .NET 8 (ASP.NET Core)
- RabbitMQ
- MongoDB
- Docker / Docker Compose
- xUnit + GitHub Actions (CI)

## Funcionalidades

- `POST /api/jobs/send` — cria um job (tipo + dados) e publica na fila
- `GET /api/jobs/{id}` — consulta o status de um job
- `GET /api/jobs/seed` — cria um job de exemplo (EmailJob)
- `EmailJobWorker` — background service que consome a fila e atualiza o status no MongoDB

## Como executar

Requisito: Docker.

```bash
cp .env.example .env   # usuário e senha do RabbitMQ
docker compose up --build
```

API em `http://localhost:5000`, Swagger em `/swagger`.

Para rodar os testes: `dotnet test`.

## Próximos passos

- ampliar a cobertura de testes
- política de retry com dead-letter queue
