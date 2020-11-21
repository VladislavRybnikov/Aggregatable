# :gift: Aggregatable
Main idea of the project is to simplify implementation of entity updates in event based systems. There are situations when multiple microservices work with one entity (or aggregate) 
and update parts of this entity in different storages, so there should be written non-trivial logic to synchronize data state between this storages.
> :exclamation: Unlike other similar projets this does not use same model for aggregation and enetity represantations. Also, data updates performs automaticaly on events in choosen storage (no need in separaate repositories...)

## :zap: Example
Firstly, you should create a class wich will implement AggregateRoot<T>, where T is your existing entity

```csharp
public class UserAggregateRoot : AggregateRoot<User>
{
...
}
```

## :trophy: What was done
