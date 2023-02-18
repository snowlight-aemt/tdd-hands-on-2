### 데이터베이스 생성
```shell
dotnet ef database update -v

dotnet ef migrations add AddUsers --project Sellers.Sql --startup-project ./Sellers.Api
dotnet ef database update --project Sellers.Sql --startup-project Sellers.Api 
```

```mermaid
classDiagram
    IUserReader <..CreateUserCommandExecutor
    BackwardCompatibleUserReader ..> ShopUserReader
    BackwardCompatibleUserReader ..> SqlUserReader
    IUserReader <|-- BackwardCompatibleUserReader
    IUserReader <|-- ShopUserReader
    IUserReader <|-- SqlUserReader
```