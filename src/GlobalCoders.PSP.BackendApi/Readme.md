## Entities data models

### Migrations

1. Go to project dir

```bash
cd ./src/GlobalCoders.PSP.BackendApi
```

2. Run migration command

```
dotnet ef migrations add Init -o Data/Migrations -c <ContextDbName>  --msbuildprojectextensionspath ../../obj/<ProjectName>
```

Example:
```bash
dotnet ef migrations add Init -o Data/Migrations -c BackendContext  --msbuildprojectextensionspath ../../obj/GlobalCoders.PSP.BackendApi
```