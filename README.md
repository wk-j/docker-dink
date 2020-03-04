## Dink

```bash
dotnet new webapi --language c# --output src/Dink
dotnet add src/Dink/Dink.csproj package DinkToPdf

wk-folder-download https://github.com/rdvojmoc/DinkToPdf/tree/master/v0.12.4/64%20bit native

dotnet publish src/Dink -c Release -o .publish -r linux-x64
```

## Test

- http://localhost/report/summary
- http://localhost/report/generate