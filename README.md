# Compra virtual com Saga Pattern
Este projeto é um model de pagamento de produtos online utilizando Saga Pattern com a lib MassTransit.

Para rodar este projeto é necessário que a utilização de RabbitMQ e PostreSQL instalado localmwente ou em algum server remoto.

O teste se inicia através de um request feito no projeto Shop.Order.Server, através do endpoint /Orders [POST]. A partir desta requisição a orquestração dos eventos se inicia e os demais services entram em ação.