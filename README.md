# O que é

=====================

Esse projeto é uma PoC com uma API servindo como producer em um tópico do Kafka e um Worker servindo como consumer do tópico.

## Tecnologias

- .NET Core 3
- Kafka (confluentinc)

## Requisitos de dev

- VS ou VS Code
- .NET Core SDK
- Docker (com docker compose)

## Como executar

- Na pasta raiz, executar:

```bash
docker-compose up
```

- [Acessar o swagger](http://localhost:5001/index.html) e fazer o post com o payload
- Acompanhar os logs do consumer.

## Para desenvolver

- Subir um compose apenas com as imagens do kafka e zookeeper. No launchSettings já contém as variáveis de ambiente necessárias.
- Na pasta raiz, executar:

```bash
docker-compose -f docker-compose.development.yml up
```

## Úteis

- [Configuration properties](https://github.com/edenhill/librdkafka/blob/master/CONFIGURATION.md)
