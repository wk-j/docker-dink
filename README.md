## Dink to PDF

```bash
wk-folder-download https://github.com/rdvojmoc/DinkToPdf/tree/master/v0.12.4/64%20bit native

dotnet new webapi --language c# --output src/Dink
dotnet add src/Dink/Dink.csproj package DinkToPdf
docker-compose up --build
```

## Test

- http://localhost/report/summary
- http://localhost/report/generate