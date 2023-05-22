# Projeto mqtt-client-server

Este projeto é uma aplicação mqtt-client-server que pode ser executada usando Docker Compose.

## Pré-requisitos

Certifique-se de ter os seguintes requisitos instalados em seu sistema:

- Docker
- Docker Compose
- Make

## Configuração

Antes de executar a aplicação, é necessário realizar algumas configurações. 

1. Clone o repositório do projeto:
```bash
git clone https://github.com/hnovais95/mqtt-client-server
```

2. Acesse o diretório do projeto:
```bash
cd mqtt-client-server
```

3. Crie um arquivo `.env` e defina as variáveis de ambiente necessárias. Por exemplo:
```bash
POSTGRES_USER=postgres
POSTGRES_PASSWORD=admin
POSTGRES_DB=northwind
```

## Executando a aplicação

O projeto mqtt-client-server pode ser executado facilmente usando o Docker Compose e o Makefile fornecido.

### Comandos disponíveis no Makefile

- `make build`: Compila as imagens Docker.
- `make up`: Inicia os contêineres.
- `make down`: Interrompe e remove os contêineres.
- `make restart`: Reinicia os contêineres.
- `make logs`: Exibe os logs dos contêineres em tempo real.

### Executando a aplicação

1. Compile as imagens Docker:
```bash
make build
```

2. Inicie os contêineres:
```bash
make up
```
Isso iniciará os contêineres do projeto, incluindo o servidor MQTT e o banco de dados PostgreSQL.


3. Acesse a aplicação

Após iniciar os contêineres, você poderá acompanhar a aplicação em execução através dos logs.

### Parando a aplicação

Para parar a execução da aplicação e remover os contêineres, execute o seguinte comando:
```bash
make down
```

Isso interromperá os contêineres e removerá todos os recursos relacionados à aplicação.

## Logs

Você pode verificar os logs dos contêineres executando o seguinte comando:
```bash
make logs
```

Isso exibirá os logs dos contêineres em tempo real.