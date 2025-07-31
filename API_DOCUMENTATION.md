# 📚 Documentação Técnica da API Tomou

## 🔗 Visão Geral

A API do Tomou é uma API REST desenvolvida em .NET 8 que fornece endpoints para gerenciamento completo de medicamentos. Esta documentação fornece detalhes técnicos sobre todos os endpoints, modelos de dados e exemplos de uso.

---

## 🔐 Autenticação

### JWT (JSON Web Tokens)

A API utiliza JWT para autenticação. Todos os endpoints protegidos requerem um token válido no header `Authorization`.

#### Formato do Token
```
Authorization: Bearer {seu-token-jwt}
```

#### Estrutura do Token
```json
{
  "sub": "user-id",
  "email": "user@email.com",
  "isCaregiver": true,
  "exp": 1640995200,
  "iat": 1640908800
}
```

---

## 📋 Endpoints

### 🔐 Autenticação

#### POST /api/User/register
Registra um novo usuário no sistema.

**Request Body:**
```json
{
  "name": "João Silva",
  "email": "joao@email.com",
  "password": "Senha123!",
  "isCaregiver": true
}
```

**Response (201 Created):**
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "name": "João Silva",
  "email": "joao@email.com",
  "isCaregiver": true,
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Validações:**
- Nome obrigatório (mínimo 3 caracteres)
- Email obrigatório e único
- Senha obrigatória (mínimo 6 caracteres)
- isCaregiver obrigatório

---

#### POST /api/User/login
Realiza login do usuário.

**Request Body:**
```json
{
  "email": "joao@email.com",
  "password": "Senha123!"
}
```

**Response (200 OK):**
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "name": "João Silva",
  "email": "joao@email.com",
  "isCaregiver": true,
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Validações:**
- Email obrigatório
- Senha obrigatória
- Credenciais válidas

---

#### POST /api/User/forgot-password
Solicita reset de senha.

**Request Body:**
```json
{
  "email": "joao@email.com"
}
```

**Response (200 OK):**
```json
{
  "message": "Email de recuperação enviado com sucesso"
}
```

**Validações:**
- Email obrigatório
- Email deve existir no sistema

---

#### POST /api/User/reset-password
Reseta a senha usando token.

**Request Body:**
```json
{
  "token": "reset-token-here",
  "newPassword": "NovaSenha123!"
}
```

**Response (200 OK):**
```json
{
  "message": "Senha alterada com sucesso"
}
```

**Validações:**
- Token obrigatório e válido
- Nova senha obrigatória (mínimo 6 caracteres)

---

### 👥 Dependentes

#### POST /api/Dependent
Cadastra um novo dependente (apenas cuidadores).

**Headers:**
```
Authorization: Bearer {token}
```

**Request Body:**
```json
{
  "name": "Maria Silva"
}
```

**Response (201 Created):**
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440001",
  "name": "Maria Silva",
  "userId": "550e8400-e29b-41d4-a716-446655440000"
}
```

**Validações:**
- Nome obrigatório (mínimo 3 caracteres)
- Usuário deve ser cuidador
- Limite de 5 dependentes por cuidador

---

#### GET /api/Dependent
Lista dependentes do usuário autenticado.

**Headers:**
```
Authorization: Bearer {token}
```

**Query Parameters:**
- `id` (opcional): Filtrar por ID do dependente
- `name` (opcional): Filtrar por nome
- `order` (opcional): Ordenação (asc/desc)

**Response (200 OK):**
```json
[
  {
    "id": "550e8400-e29b-41d4-a716-446655440001",
    "name": "Maria Silva",
    "userId": "550e8400-e29b-41d4-a716-446655440000"
  }
]
```

---

#### GET /api/Dependent/{id}
Busca dependente específico por ID.

**Headers:**
```
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440001",
  "name": "Maria Silva",
  "userId": "550e8400-e29b-41d4-a716-446655440000"
}
```

**Validações:**
- Dependente deve existir
- Usuário deve ter acesso ao dependente

---

#### PUT /api/Dependent/{id}
Atualiza informações do dependente.

**Headers:**
```
Authorization: Bearer {token}
```

**Request Body:**
```json
{
  "name": "Maria Silva Santos"
}
```

**Response (200 OK):**
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440001",
  "name": "Maria Silva Santos",
  "userId": "550e8400-e29b-41d4-a716-446655440000"
}
```

**Validações:**
- Nome obrigatório (mínimo 3 caracteres)
- Dependente deve existir
- Usuário deve ter acesso ao dependente

---

#### DELETE /api/Dependent/{id}
Exclui dependente.

**Headers:**
```
Authorization: Bearer {token}
```

**Response (204 No Content)**

**Validações:**
- Dependente deve existir
- Usuário deve ter acesso ao dependente

---

### 💊 Medicamentos

#### POST /api/Medications
Cadastra um novo medicamento.

**Headers:**
```
Authorization: Bearer {token}
```

**Request Body:**
```json
{
  "name": "Paracetamol",
  "dosage": "500mg",
  "startDate": "2024-01-01",
  "endDate": "2024-01-31",
  "timesToTake": ["08:00", "20:00"],
  "daysOfWeek": [1, 2, 3, 4, 5, 6, 7],
  "dependentId": "550e8400-e29b-41d4-a716-446655440001"
}
```

**Response (201 Created):**
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440002",
  "name": "Paracetamol",
  "dosage": "500mg",
  "startDate": "2024-01-01",
  "endDate": "2024-01-31",
  "timesToTake": ["08:00", "20:00"],
  "daysOfWeek": [1, 2, 3, 4, 5, 6, 7],
  "dependentId": "550e8400-e29b-41d4-a716-446655440001",
  "userId": "550e8400-e29b-41d4-a716-446655440000"
}
```

**Validações:**
- Nome obrigatório
- Dosagem obrigatória
- Data de início obrigatória
- Data de fim obrigatória
- Data de fim deve ser posterior à data de início
- Horários obrigatórios (pelo menos um)
- Dias da semana obrigatórios (pelo menos um)
- Dependente deve existir (se fornecido)
- Usuário deve ter acesso ao dependente

---

#### GET /api/Medications
Lista medicamentos do usuário autenticado.

**Headers:**
```
Authorization: Bearer {token}
```

**Query Parameters:**
- `id` (opcional): Filtrar por ID do medicamento
- `name` (opcional): Filtrar por nome
- `order` (opcional): Ordenação (asc/desc)

**Response (200 OK):**
```json
[
  {
    "id": "550e8400-e29b-41d4-a716-446655440002",
    "name": "Paracetamol",
    "dosage": "500mg",
    "startDate": "2024-01-01",
    "endDate": "2024-01-31",
    "timesToTake": ["08:00", "20:00"],
    "daysOfWeek": [1, 2, 3, 4, 5, 6, 7],
    "dependentId": "550e8400-e29b-41d4-a716-446655440001",
    "userId": "550e8400-e29b-41d4-a716-446655440000"
  }
]
```

---

#### GET /api/Medications/{medicamentId}
Busca medicamento específico por ID.

**Headers:**
```
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440002",
  "name": "Paracetamol",
  "dosage": "500mg",
  "startDate": "2024-01-01",
  "endDate": "2024-01-31",
  "timesToTake": ["08:00", "20:00"],
  "daysOfWeek": [1, 2, 3, 4, 5, 6, 7],
  "dependentId": "550e8400-e29b-41d4-a716-446655440001",
  "userId": "550e8400-e29b-41d4-a716-446655440000"
}
```

**Validações:**
- Medicamento deve existir
- Usuário deve ter acesso ao medicamento

---

#### PUT /api/Medications/{medicamentId}
Atualiza medicamento.

**Headers:**
```
Authorization: Bearer {token}
```

**Request Body:**
```json
{
  "name": "Paracetamol 750mg",
  "dosage": "750mg",
  "startDate": "2024-01-01",
  "endDate": "2024-02-01",
  "timesToTake": ["08:00", "14:00", "20:00"],
  "daysOfWeek": [1, 2, 3, 4, 5, 6, 7],
  "dependentId": "550e8400-e29b-41d4-a716-446655440001"
}
```

**Response (200 OK):**
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440002",
  "name": "Paracetamol 750mg",
  "dosage": "750mg",
  "startDate": "2024-01-01",
  "endDate": "2024-02-01",
  "timesToTake": ["08:00", "14:00", "20:00"],
  "daysOfWeek": [1, 2, 3, 4, 5, 6, 7],
  "dependentId": "550e8400-e29b-41d4-a716-446655440001",
  "userId": "550e8400-e29b-41d4-a716-446655440000"
}
```

**Validações:**
- Nome obrigatório
- Dosagem obrigatória
- Data de início obrigatória
- Data de fim obrigatória
- Data de fim deve ser posterior à data de início
- Horários obrigatórios (pelo menos um)
- Dias da semana obrigatórios (pelo menos um)
- Dependente deve existir (se fornecido)
- Usuário deve ter acesso ao medicamento
- Usuário deve ter acesso ao dependente

---

#### DELETE /api/Medications/{medicamentId}
Exclui medicamento.

**Headers:**
```
Authorization: Bearer {token}
```

**Response (204 No Content)**

**Validações:**
- Medicamento deve existir
- Usuário deve ter acesso ao medicamento

---

## 📊 Códigos de Status HTTP

| Código | Descrição | Uso |
|--------|-----------|-----|
| `200` | OK | Operação realizada com sucesso |
| `201` | Created | Recurso criado com sucesso |
| `204` | No Content | Operação realizada sem retorno |
| `400` | Bad Request | Dados inválidos ou validação falhou |
| `401` | Unauthorized | Não autenticado ou token inválido |
| `403` | Forbidden | Não autorizado para acessar o recurso |
| `404` | Not Found | Recurso não encontrado |
| `409` | Conflict | Conflito (ex: email já existe) |
| `500` | Internal Server Error | Erro interno do servidor |

---

## ⚠️ Tratamento de Erros

### Formato de Erro Padrão

```json
{
  "message": "Descrição do erro",
  "errors": [
    {
      "field": "campo",
      "message": "mensagem de erro específica"
    }
  ]
}
```

### Exemplos de Erros

#### 400 Bad Request - Validação
```json
{
  "message": "Dados inválidos",
  "errors": [
    {
      "field": "name",
      "message": "Nome é obrigatório"
    },
    {
      "field": "email",
      "message": "Email deve ser válido"
    }
  ]
}
```

#### 401 Unauthorized
```json
{
  "message": "Credenciais inválidas"
}
```

#### 403 Forbidden
```json
{
  "message": "Acesso negado ao recurso"
}
```

#### 404 Not Found
```json
{
  "message": "Medicamento não encontrado"
}
```

#### 409 Conflict
```json
{
  "message": "Email já cadastrado"
}
```

---

## 🔧 Configuração de Desenvolvimento

### Variáveis de Ambiente

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

### Swagger/OpenAPI

A documentação interativa está disponível em:
- **URL**: `https://localhost:7001/swagger`
- **Autenticação**: Configurada para JWT
- **Testes**: Diretos dos endpoints

---

## 📝 Exemplos de Uso

### Fluxo Completo de Registro e Login

```bash
# 1. Registrar usuário
curl -X POST "https://localhost:7001/api/User/register" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "João Silva",
    "email": "joao@email.com",
    "password": "Senha123!",
    "isCaregiver": true
  }'

# 2. Login
curl -X POST "https://localhost:7001/api/User/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "joao@email.com",
    "password": "Senha123!"
  }'

# 3. Cadastrar dependente
curl -X POST "https://localhost:7001/api/Dependent" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Maria Silva"
  }'

# 4. Cadastrar medicamento
curl -X POST "https://localhost:7001/api/Medications" \
  -H "Authorization: Bearer {token}" \
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

### Consultas com Filtros

```bash
# Listar medicamentos ordenados por nome (ascendente)
curl -X GET "https://localhost:7001/api/Medications?order=asc" \
  -H "Authorization: Bearer {token}"

# Buscar medicamentos por nome
curl -X GET "https://localhost:7001/api/Medications?name=paracetamol" \
  -H "Authorization: Bearer {token}"

# Listar dependentes ordenados por nome (descendente)
curl -X GET "https://localhost:7001/api/Dependent?order=desc" \
  -H "Authorization: Bearer {token}"
```

---

## 🔒 Segurança

### Autenticação
- JWT tokens com expiração configurável
- Senhas criptografadas com hash seguro
- Tokens de reset de senha com expiração

### Autorização
- Controle de acesso baseado em propriedade dos recursos
- Verificação de permissões em todas as operações
- Separação entre cuidadores e dependentes

### Validação
- Validação de entrada em todos os endpoints
- Sanitização de dados
- Proteção contra ataques comuns

---

## 📈 Performance

### Otimizações Implementadas
- Entity Framework com lazy loading desabilitado
- Queries otimizadas com includes
- Paginação em listagens grandes
- Cache de configurações

### Monitoramento
- Logs estruturados
- Métricas de performance
- Tratamento de exceções centralizado

---

## 🤝 Suporte

Para dúvidas técnicas ou problemas com a API:

1. **Documentação**: Consulte esta documentação
2. **Swagger**: Use a interface interativa
3. **Issues**: Abra uma issue no repositório
4. **Logs**: Verifique os logs da aplicação

---

**Tomou API** - Documentação técnica completa 📚 