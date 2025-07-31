# 🚀 Guia de Instalação e Configuração - Tomou

Este guia fornece instruções passo a passo para configurar e executar o projeto Tomou em seu ambiente de desenvolvimento.

---

## 📋 Pré-requisitos

### Software Necessário

| Software | Versão | Download |
|----------|--------|----------|
| **.NET 8 SDK** | 8.0 ou superior | [Download .NET 8](https://dotnet.microsoft.com/download/dotnet/8.0) |
| **SQL Server** | 2019 ou superior | [Download SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) |
| **Visual Studio 2022** | 17.0 ou superior | [Download VS 2022](https://visualstudio.microsoft.com/downloads/) |
| **Git** | 2.0 ou superior | [Download Git](https://git-scm.com/downloads) |

### Verificação de Instalação

```bash
# Verificar versão do .NET
dotnet --version

# Verificar versão do Git
git --version

# Verificar se o SQL Server está rodando
sqlcmd -S localhost -E -Q "SELECT @@VERSION"
```

---

## 🔧 Configuração Inicial

### 1. Clone do Repositório

```bash
# Clone o repositório
git clone https://github.com/seu-usuario/Tomou.git

# Navegue para o diretório do projeto
cd Tomou
```

### 2. Configuração do Banco de Dados

#### Opção A: SQL Server Local

1. **Instale o SQL Server** (se ainda não tiver)
2. **Configure a string de conexão** no arquivo `Tomou.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TomouDB;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

#### Opção B: SQL Server Express

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=TomouDB;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

#### Opção C: SQL Server com Autenticação SQL

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TomouDB;User Id=sa;Password=sua-senha;TrustServerCertificate=true;"
  }
}
```

### 3. Configuração do JWT

Edite o arquivo `Tomou.Api/appsettings.json` e adicione:

```json
{
  "JwtSettings": {
    "SecretKey": "sua-chave-secreta-aqui-com-pelo-menos-32-caracteres-para-seguranca",
    "Issuer": "Tomou",
    "Audience": "TomouUsers",
    "ExpirationHours": 24
  }
}
```

### 4. Configuração de Email (Opcional)

Para funcionalidades de reset de senha, configure o email:

```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "Username": "seu-email@gmail.com",
    "Password": "sua-senha-de-app",
    "EnableSsl": true
  }
}
```

---

## 🗄️ Configuração do Banco de Dados

### 1. Executar Migrations

```bash
# Navegue para o diretório da API
cd Tomou.Api

# Execute as migrations
dotnet ef database update --project ../Tomou.Infrastructure --startup-project .

# Ou execute diretamente
dotnet ef database update
```

### 2. Verificar Criação do Banco

```bash
# Conecte ao SQL Server
sqlcmd -S localhost -E

# Verifique se o banco foi criado
SELECT name FROM sys.databases WHERE name = 'TomouDB'
GO

# Use o banco
USE TomouDB
GO

# Verifique as tabelas
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'
GO
```

### 3. Estrutura do Banco

O banco de dados será criado com as seguintes tabelas:

- **Users**: Usuários do sistema
- **Dependents**: Dependentes dos cuidadores
- **Medications**: Medicamentos
- **MedicationLogs**: Logs de administração
- **PasswordResetTokens**: Tokens para reset de senha

---

## 🚀 Executando o Projeto

### 1. Restaurar Dependências

```bash
# Restaurar pacotes NuGet
dotnet restore

# Ou restore específico
dotnet restore Tomou.Api
dotnet restore Tomou.Application
dotnet restore Tomou.Infrastructure
```

### 2. Build do Projeto

```bash
# Build completo
dotnet build

# Build específico
dotnet build Tomou.Api
```

### 3. Executar a Aplicação

```bash
# Execute a API
dotnet run --project Tomou.Api

# Ou navegue para o diretório e execute
cd Tomou.Api
dotnet run
```

### 4. Verificar Execução

A aplicação estará disponível em:
- **API**: `https://localhost:7001`
- **Swagger**: `https://localhost:7001/swagger`
- **HTTP**: `http://localhost:5001`

---

## 🧪 Executando Testes

### 1. Testes Unitários

```bash
# Executar todos os testes
dotnet test

# Executar testes específicos
dotnet test Tomou.UnitTests/

# Executar com detalhes
dotnet test --verbosity normal

# Executar com cobertura
dotnet test --collect:"XPlat Code Coverage"
```

### 2. Testes Específicos

```bash
# Testes de medicamentos
dotnet test --filter "FullyQualifiedName~Medications"

# Testes de usuários
dotnet test --filter "FullyQualifiedName~Users"

# Testes de dependentes
dotnet test --filter "FullyQualifiedName~Dependents"
```

---

## 🔧 Configuração de Desenvolvimento

### 1. Visual Studio 2022

1. **Abra o projeto** no Visual Studio 2022
2. **Configure o projeto de inicialização** como `Tomou.Api`
3. **Configure a string de conexão** no `appsettings.Development.json`
4. **Execute as migrations** via Package Manager Console:

```powershell
Update-Database -ProjectName Tomou.Infrastructure -StartupProjectName Tomou.Api
```

### 2. Visual Studio Code

1. **Instale as extensões**:
   - C# Dev Kit
   - C# Extensions
   - REST Client

2. **Configure o launch.json**:

```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": "Launch Tomou API",
      "type": "coreclr",
      "request": "launch",
      "preLaunchTask": "build",
      "program": "${workspaceFolder}/Tomou.Api/bin/Debug/net8.0/Tomou.Api.dll",
      "args": [],
      "cwd": "${workspaceFolder}/Tomou.Api",
      "console": "internalConsole",
      "stopAtEntry": false
    }
  ]
}
```

### 3. Configuração de Debug

Adicione ao `launchSettings.json`:

```json
{
  "profiles": {
    "Tomou.Api": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "https://localhost:7001;http://localhost:5001",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

---

## 🔍 Troubleshooting

### Problemas Comuns

#### 1. Erro de Conexão com Banco

**Sintoma**: `A network-related or instance-specific error occurred`

**Solução**:
```bash
# Verifique se o SQL Server está rodando
net start MSSQLSERVER

# Teste a conexão
sqlcmd -S localhost -E -Q "SELECT 1"
```

#### 2. Erro de Migration

**Sintoma**: `There is already an object named 'TableName' in the database`

**Solução**:
```bash
# Remova o banco e recrie
dotnet ef database drop --force
dotnet ef database update
```

#### 3. Erro de JWT

**Sintoma**: `Invalid token` ou `Token validation failed`

**Solução**:
- Verifique se a `SecretKey` tem pelo menos 32 caracteres
- Certifique-se de que a configuração está correta no `appsettings.json`

#### 4. Erro de Build

**Sintoma**: `The type or namespace name could not be found`

**Solução**:
```bash
# Limpe e restaure
dotnet clean
dotnet restore
dotnet build
```

#### 5. Erro de Porta

**Sintoma**: `Only one usage of each socket address is normally permitted`

**Solução**:
- Verifique se a porta não está em uso
- Altere a porta no `launchSettings.json`
- Mate processos que estejam usando a porta

```bash
# Windows
netstat -ano | findstr :7001
taskkill /PID <PID> /F

# Linux/Mac
lsof -i :7001
kill -9 <PID>
```

---

## 📊 Monitoramento

### 1. Logs da Aplicação

Os logs são configurados automaticamente. Verifique:
- **Console**: Durante o desenvolvimento
- **Arquivos**: Em produção
- **Swagger**: Para testes da API

### 2. Métricas de Performance

```bash
# Verificar uso de memória
dotnet-counters monitor --process-id <PID>

# Verificar performance
dotnet-trace collect --name Tomou --process-id <PID>
```

### 3. Health Checks

A API inclui health checks em:
- `/health` - Status geral
- `/health/ready` - Pronto para receber requests
- `/health/live` - Aplicação viva

---

## 🔒 Configuração de Segurança

### 1. Produção

```json
{
  "JwtSettings": {
    "SecretKey": "chave-super-secreta-com-pelo-menos-64-caracteres-em-producao",
    "ExpirationHours": 1
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=prod-server;Database=TomouDB;User Id=app-user;Password=senha-forte;"
  }
}
```

### 2. HTTPS

```bash
# Gerar certificado de desenvolvimento
dotnet dev-certs https --trust

# Verificar certificados
dotnet dev-certs https --check
```

### 3. CORS

Configure no `Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowProduction", policy =>
    {
        policy.WithOrigins("https://seu-dominio.com")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
```

---

## 📚 Próximos Passos

### 1. Teste a API

1. **Acesse o Swagger**: `https://localhost:7001/swagger`
2. **Teste o registro**: POST `/api/User/register`
3. **Teste o login**: POST `/api/User/login`
4. **Teste os endpoints protegidos**

### 2. Desenvolvimento

1. **Crie uma branch**: `git checkout -b feature/nova-funcionalidade`
2. **Implemente a funcionalidade**
3. **Adicione testes**
4. **Crie um Pull Request**

### 3. Deploy

1. **Configure o ambiente de produção**
2. **Configure o banco de dados**
3. **Configure as variáveis de ambiente**
4. **Deploy da aplicação**

---

## 🤝 Suporte

### Recursos Úteis

- **Documentação da API**: `API_DOCUMENTATION.md`
- **README Principal**: `README.md`
- **Issues**: Repositório do GitHub
- **Swagger**: Interface interativa

### Contato

Para dúvidas ou problemas:
1. Verifique esta documentação
2. Consulte o README principal
3. Abra uma issue no repositório
4. Verifique os logs da aplicação

---

**Tomou** - Guia de instalação completo 🚀 