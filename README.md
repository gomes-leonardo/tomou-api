# Tomou - Sistema de Gerenciamento de Medicamentos

<div align="center">

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![Entity Framework](https://img.shields.io/badge/Entity_Framework-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=JSON%20web%20tokens&logoColor=white)

**Tomou** - Simplificando o gerenciamento de medicamentos 🏥

[📋 Sobre](#-sobre) • [🏗️ Arquitetura](#️-arquitetura) • [🚀 Funcionalidades](#-funcionalidades) • [📚 API](#-api) • [🛠️ Tecnologias](#️-tecnologias) • [⚙️ Configuração](#️-configuração) • [🧪 Testes](#-testes) • [🤝 Contribuição](#-contribuição)

</div>

---

## 📋 Sobre

O **Tomou** é uma API REST desenvolvida em .NET 8 para gerenciamento completo de medicamentos. O sistema permite que cuidadores e dependentes registrem, consultem, atualizem e excluam informações sobre medicamentos e seus horários de administração.

### 🎯 Objetivos

- **Facilitar o controle de medicamentos** para cuidadores e dependentes
- **Garantir a segurança** através de autenticação JWT e controle de acesso
- **Fornecer uma API robusta** seguindo as melhores práticas de desenvolvimento
- **Manter alta qualidade** através de testes automatizados e Clean Architecture

### 👥 Público-Alvo

- **Cuidadores**: Pessoas responsáveis por gerenciar medicamentos de dependentes
- **Dependentes**: Pessoas que precisam de controle de seus próprios medicamentos
- **Desenvolvedores**: Que desejam integrar com a API do Tomou

---

## 🏗️ Arquitetura

### Clean Architecture (Arquitetura Limpa)

O projeto segue rigorosamente os princípios da Clean Architecture, organizando o código em camadas bem definidas:

```
Tomou/
├── 📁 Tomou.Api/                    # 🎯 Camada de Apresentação
│   ├── Controllers/                  # Controllers REST
│   ├── Filter/                      # Filtros de exceção
│   └── Program.cs                   # Configuração da aplicação
├── 📁 Tomou.Application/            # 🔧 Camada de Aplicação
│   ├── UseCases/                    # Casos de uso da aplicação
│   ├── Services/                    # Serviços de domínio
│   ├── Validators/                  # Validação de entrada
│   └── AutoMapper/                  # Configuração de mapeamentos
├── 📁 Tomou.Communication/          # 📡 DTOs de Comunicação
│   ├── Requests/                    # DTOs de entrada
│   └── Responses/                   # DTOs de saída
├── 📁 Tomou.Domain/                 # 🏛️ Camada de Domínio
│   ├── Entities/                    # Entidades de domínio
│   ├── Repositories/                # Interfaces de repositórios
│   ├── Enums/                       # Enumerações do domínio
│   └── Security/                    # Interfaces de segurança
├── 📁 Tomou.Infrastructure/         # 🏗️ Camada de Infraestrutura
│   ├── DataAccess/                  # Contexto do Entity Framework
│   ├── Repositories/                # Implementações dos repositórios
│   ├── Security/                    # Implementações de criptografia e JWT
│   └── Services/                    # Implementações de serviços
├── 📁 Tomou.Exception/              # ⚠️ Exceções Customizadas
├── 📁 Tomou.TestUtils/              # 🧪 Utilitários para Testes
└── 📁 Tomou.UnitTests/              # 🧪 Testes Unitários
```

### Padrões Arquiteturais

| Padrão | Descrição | Implementação |
|--------|-----------|---------------|
| **CQRS** | Separação entre operações de leitura e escrita | Use Cases separados para Commands e Queries |
| **Repository Pattern** | Abstração do acesso a dados | Interfaces e implementações de repositórios |
| **Unit of Work** | Gerenciamento de transações | `IUnitOfWork` e `UnitOfWork` |
| **Dependency Injection** | Inversão de controle | Configuração no `Program.cs` |
| **DTO Pattern** | Transferência de dados entre camadas | Requests e Responses no `Tomou.Communication` |

---

## 🚀 Funcionalidades

### 👤 Gestão de Usuários

#### Autenticação e Autorização
- ✅ **Registro de usuários** com validação de email único
- ✅ **Login/Autenticação** com JWT tokens
- ✅ **Recuperação de senha** via email
- ✅ **Reset de senha** com tokens seguros
- ✅ **Controle de acesso** baseado em roles (Cuidador/Dependente)

#### Tipos de Usuário
- **Cuidador**: Pode gerenciar dependentes e seus medicamentos
- **Dependente**: Pode visualizar apenas seus próprios medicamentos

### 👥 Gestão de Dependentes

#### Operações Disponíveis
- ✅ **Cadastro de dependentes** (apenas cuidadores)
- ✅ **Atualização de informações** com validação
- ✅ **Exclusão de dependentes** com verificação de propriedade
- ✅ **Consulta de dependentes** com filtros
- ✅ **Limite de 5 dependentes** por cuidador

#### Validações
- Nome obrigatório e válido
- Verificação de propriedade do dependente
- Limite de dependentes por cuidador

### 💊 Gestão de Medicamentos

#### Funcionalidades Principais
- ✅ **Cadastro de medicamentos** com horários específicos
- ✅ **Consulta de medicamentos** com filtros e ordenação
- ✅ **Atualização de informações** com validação
- ✅ **Exclusão de medicamentos** com verificação de propriedade
- ✅ **Controle de horários** de administração
- ✅ **Controle de dias da semana** para administração

#### Características dos Medicamentos
- **Nome e dosagem** obrigatórios
- **Data de início e fim** do tratamento
- **Horários específicos** de administração
- **Dias da semana** para administração
- **Associação** a dependente ou usuário

### 🔐 Controle de Acesso

#### Segurança Implementada
- ✅ **Validação de permissões** em todas as operações
- ✅ **Verificação de propriedade** dos recursos
- ✅ **Tratamento de exceções** de acesso
- ✅ **Tokens JWT** com expiração
- ✅ **Criptografia** de senhas

---

## 📚 API

### 🔐 Autenticação

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| `POST` | `/api/User/register` | Registro de usuário | ❌ |
| `POST` | `/api/User/login` | Login de usuário | ❌ |
| `POST` | `/api/User/forgot-password` | Esqueci minha senha | ❌ |
| `POST` | `/api/User/reset-password` | Reset de senha | ❌ |

### 👥 Dependentes

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| `POST` | `/api/Dependent` | Cadastrar dependente | ✅ |
| `GET` | `/api/Dependent` | Listar dependentes | ✅ |
| `GET` | `/api/Dependent/{id}` | Buscar dependente por ID | ✅ |
| `PUT` | `/api/Dependent/{id}` | Atualizar dependente | ✅ |
| `DELETE` | `/api/Dependent/{id}` | Excluir dependente | ✅ |

### 💊 Medicamentos

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| `POST` | `/api/Medications` | Cadastrar medicamento | ✅ |
| `GET` | `/api/Medications` | Listar medicamentos | ✅ |
| `GET` | `/api/Medications/{medicamentId}` | Buscar medicamento por ID | ✅ |
| `PUT` | `/api/Medications/{medicamentId}` | Atualizar medicamento | ✅ |
| `DELETE` | `/api/Medications/{medicamentId}` | Excluir medicamento | ✅ |

### 📊 Códigos de Status HTTP

| Código | Descrição | Uso |
|--------|-----------|-----|
| `200` | OK | Operação realizada com sucesso |
| `201` | Created | Recurso criado com sucesso |
| `204` | No Content | Operação realizada sem retorno |
| `400` | Bad Request | Dados inválidos |
| `401` | Unauthorized | Não autenticado |
| `403` | Forbidden | Não autorizado |
| `404` | Not Found | Recurso não encontrado |
| `500` | Internal Server Error | Erro interno do servidor |

---

## 🛠️ Tecnologias

### Backend

| Tecnologia | Versão | Propósito |
|------------|--------|-----------|
| **.NET 8** | 8.0 | Framework principal |
| **ASP.NET Core** | 8.0 | Framework web |
| **Entity Framework Core** | 8.0 | ORM para acesso a dados |
| **SQL Server** | 2022 | Banco de dados relacional |
| **AutoMapper** | 12.0 | Mapeamento entre objetos |
| **FluentValidation** | 11.0 | Validação de dados |
| **JWT** | - | Autenticação e autorização |

### Testes

| Tecnologia | Versão | Propósito |
|------------|--------|-----------|
| **xUnit** | 2.6 | Framework de testes unitários |
| **Moq** | 4.20 | Framework de mocking |
| **Shouldly** | 4.2 | Assertions mais expressivas |
| **Bogus** | 35.0 | Geração de dados fake |

### Ferramentas de Desenvolvimento

| Ferramenta | Propósito |
|------------|-----------|
| **Visual Studio 2022** | IDE principal |
| **Git** | Controle de versão |
| **Swagger/OpenAPI** | Documentação da API |

---

## ⚙️ Configuração

### Pré-requisitos

- ✅ **.NET 8 SDK** instalado
- ✅ **SQL Server** configurado
- ✅ **Visual Studio 2022** (recomendado) ou VS Code

### 🚀 Configuração Rápida

1. **Clone o repositório**
   ```bash
   git clone https://github.com/seu-usuario/Tomou.git
   cd Tomou
   ```

2. **Configure a string de conexão**
   ```json
   // appsettings.json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=TomouDB;Trusted_Connection=true;"
     }
   }
   ```

3. **Execute as migrations**
   ```bash
   dotnet ef database update --project Tomou.Infrastructure --startup-project Tomou.Api
   ```

4. **Execute o projeto**
   ```bash
   dotnet run --project Tomou.Api
   ```

### 🔧 Configuração Detalhada

#### Variáveis de Ambiente

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TomouDB;Trusted_Connection=true;"
  },
  "JwtSettings": {
    "SecretKey": "sua-chave-secreta-aqui-com-pelo-menos-32-caracteres",
    "Issuer": "Tomou",
    "Audience": "TomouUsers",
    "ExpirationHours": 24
  },
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "Username": "seu-email@gmail.com",
    "Password": "sua-senha-de-app"
  }
}
```

#### Configuração do JWT

```csharp
// Program.cs
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });
```

---

## 🧪 Testes

### Estratégia de Testes

O projeto implementa uma estratégia abrangente de testes:

- ✅ **Testes Unitários**: Cobertura de Use Cases e Validators
- ✅ **Testes de Integração**: Testes de repositórios
- ✅ **Testes de API**: Testes end-to-end

### Padrões de Teste

| Padrão | Descrição | Implementação |
|--------|-----------|---------------|
| **AAA (Arrange-Act-Assert)** | Estrutura dos testes | Organização clara dos testes |
| **Mocking** | Isolamento de dependências | Uso do Moq framework |
| **Test Data Builders** | Geração de dados de teste | Builders no `Tomou.TestUtils` |

### 🚀 Executando Testes

```bash
# Executar todos os testes
dotnet test

# Executar testes específicos
dotnet test --filter "FullyQualifiedName~Medications"

# Executar com cobertura
dotnet test --collect:"XPlat Code Coverage"

# Executar testes de um projeto específico
dotnet test Tomou.UnitTests/
```

### 📊 Cobertura de Testes

O projeto mantém alta cobertura de testes nos seguintes componentes:

- ✅ **Use Cases**: 100% dos casos de uso testados
- ✅ **Validators**: 100% das validações testadas
- ✅ **Controllers**: Testes de integração
- ✅ **Repositories**: Testes de acesso a dados

---

## 📊 Banco de Dados

### Entity Framework Core

O projeto utiliza Entity Framework Core com abordagem Code First:

- ✅ **Code First approach** para definição do modelo
- ✅ **Migrations** para controle de versão do schema
- ✅ **Configuração de relacionamentos** entre entidades
- ✅ **Seed data** para dados iniciais

### 📋 Entidades Principais

#### User
```csharp
public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsCaregiver { get; set; }
    public ICollection<Dependent> Dependents { get; set; }
    public ICollection<Medication> Medications { get; set; }
}
```

#### Dependent
```csharp
public class Dependent
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public ICollection<Medication> Medications { get; set; }
}
```

#### Medication
```csharp
public class Medication
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Dosage { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public List<TimeOnly> TimesToTake { get; set; }
    public List<DayOfWeek> DaysOfWeek { get; set; }
    public Guid? DependentId { get; set; }
    public Guid? UserId { get; set; }
}
```

---

## 📚 Documentação da API

### Swagger/OpenAPI

A documentação interativa da API está disponível via Swagger:

- **URL**: `https://localhost:7001/swagger`
- **Documentação**: Completa com exemplos
- **Testes**: Diretos dos endpoints
- **Autenticação**: Configurada para JWT

### 📖 Exemplos de Uso

#### Registro de Usuário
```bash
curl -X POST "https://localhost:7001/api/User/register" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "João Silva",
    "email": "joao@email.com",
    "password": "Senha123!",
    "isCaregiver": true
  }'
```

#### Login
```bash
curl -X POST "https://localhost:7001/api/User/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "joao@email.com",
    "password": "Senha123!"
  }'
```

#### Cadastro de Medicamento
```bash
curl -X POST "https://localhost:7001/api/Medications" \
  -H "Authorization: Bearer {seu-token}" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Paracetamol",
    "dosage": "500mg",
    "startDate": "2024-01-01",
    "endDate": "2024-01-31",
    "timesToTake": ["08:00", "20:00"],
    "daysOfWeek": [1, 2, 3, 4, 5, 6, 7],
    "dependentId": "{id-do-dependente}"
  }'
```

---

## 🤝 Contribuição

### Padrões de Código

- ✅ **C# Coding Conventions**: Seguir convenções da Microsoft
- ✅ **SOLID Principles**: Aplicar princípios SOLID
- ✅ **Clean Code**: Código limpo e legível
- ✅ **Naming Conventions**: Nomenclatura consistente

### Processo de Desenvolvimento

1. **Criar branch** a partir da main
   ```bash
   git checkout -b feature/nova-funcionalidade
   ```

2. **Implementar funcionalidade** seguindo os padrões

3. **Adicionar testes unitários** para a nova funcionalidade

4. **Executar testes** para garantir qualidade
   ```bash
   dotnet test
   ```

5. **Criar Pull Request** com descrição detalhada

6. **Code Review** pelos membros da equipe

7. **Merge na main** após aprovação

### 📋 Checklist para Contribuição

- [ ] Código segue os padrões estabelecidos
- [ ] Testes unitários foram adicionados
- [ ] Documentação foi atualizada
- [ ] Build passa sem erros
- [ ] Todos os testes passam

---

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

## 👥 Equipe

- **Desenvolvido com Clean Architecture**
- **Foco em qualidade e manutenibilidade**
- **Testes automatizados**
- **Documentação completa**

---

## 🔄 Versionamento

- **v1.0.0**: Versão inicial com funcionalidades básicas
- **Controle de versão semântico**
- **Changelog mantido atualizado**

---

<div align="center">

**Tomou** - Simplificando o gerenciamento de medicamentos 🏥

[⬆️ Voltar ao topo](#tomou---sistema-de-gerenciamento-de-medicamentos)

</div> 