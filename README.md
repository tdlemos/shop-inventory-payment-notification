# Compra virtual com Saga Pattern
Este projeto é um modelo de pagamento utilizando Saga Pattern com a lib MassTransit.

Para rodar este projeto é necessário a instalação do RabbitMQ e do PostreSQL local ou em servidor remoto.

O teste se inicia através de um request feito no projeto Shop.Order.Server, através do endpoint /Orders [POST]. A partir desta requisição a orquestração dos eventos se iniciam e os demais services entram em ação.