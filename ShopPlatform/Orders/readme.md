## 3-04 비동기 프로세스

## 트랜잭션 ID 위한 컬럼 추가

`dotnet ef migrations add AddPaymentTransactionIdToOrder`
`dotnet ef database update --project ./Orders/Orders.Api`

## Azure.Storage.Queues 설치
* Azure client 설정
* 메시지 처리를 위해서 (accept/payment-approved)
* `appsettings.json` 설정 파일 에서 Azure 세팅

## polly 설치
* retry pattern : 테스트 retry 를 위해서