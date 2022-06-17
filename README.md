# Transactional Outbox Sample

This sample shows how to configure and use MassTransit's new transactional outbox, including:

- An API controller that uses a domain registration service to register a member for an event. The controller has no knowledge of MassTransit, and the registration service uses `IPublishEndpoint` to publish a domain event which is written to the transactional outbox.
- Adds the transactional outbox delivery service as a hosted service, which delivers outbox message to the transport.
- A separate service that consumes the domain event by itself, and also includes a saga state machine which uses the transactional outbox (and inbox, for idempotent message delivery).

## Video

This sample is explained in [this video](https://youtu.be/3TjGnmLno_A).

# Run in Docker

To run the sample using Docker, just type `docker compose up --build` and connect to [localhost](http://localhost:5000/swagger) to test the API.



