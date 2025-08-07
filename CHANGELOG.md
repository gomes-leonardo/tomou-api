# 📋 Changelog - Tomou

Todas as mudanças notáveis neste projeto serão documentadas neste arquivo.

O formato é baseado em [Keep a Changelog](https://keepachangelog.com/pt-BR/1.0.0/),
e este projeto adere ao [Versionamento Semântico](https://semver.org/lang/pt-BR/).

---

## [1.0.2] - 2024-01-17

### 🔧 Melhorado

#### 🏗️ Arquitetura e Padrões
- **DTOs de Query Estruturados**
  - Criado `DependentQuery` para filtros de dependentes
  - Criado `MedicationsQuery` para filtros de medicamentos
  - Melhorado `MedicationLogQuery` com parâmetro `id` para cuidadores
  - Padronização de filtros em todos os controllers GET

#### 🎯 Controllers Atualizados
- **MedicationLogController**
  - Implementado método GET completo com filtros avançados
  - Suporte para cuidadores vs usuários normais
  - Filtros por status, datas, medicamentos, etc.
  - Paginação e ordenação

- **DependentController**
  - Atualizado para usar `DependentQuery` DTO
  - Filtros por nome e ordenação padronizados

- **MedicationsController**
  - Atualizado para usar `MedicationsQuery` DTO
  - Filtros por ID (dependente), nome e ordenação

#### 📋 Estrutura dos DTOs
```csharp
// DependentQuery
- Name (string?) - filtro por nome
- Order (string) - ordenação (asc/desc)

// MedicationsQuery
- Id (Guid?) - ID do dependente (para cuidadores)
- Name (string?) - filtro por nome
- Order (string) - ordenação (asc/desc)

// MedicationLogQuery
- Id (Guid?) - ID do dependente (para cuidadores)
- MedicationId (Guid?) - filtro por medicamento
- Status (MedicationLogStatus?) - filtro por status
- ScheduledFrom/To (DateTime?) - filtro por data agendada
- TakenFrom/To (DateTime?) - filtro por data tomada
- OnlyOverdue (bool?) - apenas atrasados
- IsSnoozed (bool?) - apenas adiados
- NameContains (string?) - filtro por nome
- Order (string) - ordenação
- Page/PageSize (int?) - paginação
```

#### ✅ Benefícios
- **Consistência**: Todos os controllers seguem o mesmo padrão
- **Manutenibilidade**: Filtros centralizados em DTOs
- **Extensibilidade**: Fácil adição de novos filtros
- **Documentação**: Estrutura clara dos parâmetros de query

---

## [1.0.1] - 2024-01-16

### 🐛 Correções de Testes

#### 🐛 Corrigido
- **Testes Unitários**
  - Corrigidos construtores incorretos nos testes de DeleteMedicationUseCase
  - Corrigidos construtores incorretos nos testes de UpdateMedicationUseCase
  - Corrigidos construtores incorretos nos testes de DeleteDependentUseCase
  - Corrigida ordem de parâmetros no método Execute do UpdateDependentUseCase
  - Corrigido uso de builders estáticos nos testes de UpdateDependentUseCase
  - Corrigidos tipos de parâmetros nos métodos de teste
  - Corrigidos warnings de conversão de tipos nullable
  - Corrigida lógica de testes para cenários de cuidadores vs usuários comuns

#### 🔧 Melhorado
- **Qualidade dos Testes**
  - Melhorada cobertura de testes para cenários de usuários não-cuidadores
  - Adicionados testes adicionais para validação de permissões
  - Melhorada organização e legibilidade dos testes
  - Corrigidos imports desnecessários

#### ✅ Resultado
- **109 testes passando** (100% de sucesso)
- **0 testes falhando**
- **0 warnings de compilação**
- **Todos os erros de compilação corrigidos**

---

## [1.0.0] - 2024-01-15

### 🎉 Lançamento Inicial

#### ✅ Adicionado
- **Sistema de Autenticação**
  - Registro de usuários com validação de email único
  - Login com JWT tokens
  - Recuperação de senha via email
  - Reset de senha com tokens seguros
  - Controle de acesso baseado em roles (Cuidador/Dependente)

- **Gestão de Dependentes**
  - Cadastro de dependentes (apenas cuidadores)
  - Atualização de informações com validação
  - Exclusão de dependentes com verificação de propriedade
  - Consulta de dependentes com filtros
  - Limite de 5 dependentes por cuidador

- **Gestão de Medicamentos**
  - Cadastro de medicamentos com horários específicos
  - Consulta de medicamentos com filtros e ordenação
  - Atualização de informações com validação
  - Exclusão de medicamentos com verificação de propriedade
  - Controle de horários de administração
  - Controle de dias da semana para administração

- **Arquitetura e Infraestrutura**
  - Clean Architecture implementada
  - Entity Framework Core com Code First
  - SQL Server como banco de dados
  - JWT para autenticação
  - AutoMapper para mapeamento de objetos
  - FluentValidation para validação de dados

- **Testes**
  - Testes unitários para Use Cases
  - Testes de validação
  - Testes de repositórios
  - Cobertura de testes abrangente

- **Documentação**
  - README completo com instruções
  - Documentação técnica da API
  - Guia de instalação e configuração
  - Swagger/OpenAPI integrado

#### 🏗️ Estrutura do Projeto
```
Tomou/
├── Tomou.Api/                    # Camada de Apresentação
├── Tomou.Application/            # Camada de Aplicação
├── Tomou.Communication/          # DTOs de Comunicação
├── Tomou.Domain/                # Camada de Domínio
├── Tomou.Exception/             # Exceções Customizadas
├── Tomou.Infrastructure/        # Camada de Infraestrutura
├── Tomou.TestUtils/             # Utilitários para Testes
└── Tomou.UnitTests/             # Testes Unitários
```

#### 🔧 Tecnologias Utilizadas
- **.NET 8**: Framework principal
- **ASP.NET Core**: Framework web
- **Entity Framework Core**: ORM
- **SQL Server**: Banco de dados
- **JWT**: Autenticação
- **AutoMapper**: Mapeamento
- **FluentValidation**: Validação
- **xUnit**: Testes unitários
- **Moq**: Mocking
- **Swagger**: Documentação da API

#### 📊 Funcionalidades Principais
- ✅ Autenticação e autorização JWT
- ✅ CRUD completo de usuários
- ✅ CRUD completo de dependentes
- ✅ CRUD completo de medicamentos
- ✅ Validação de dados robusta
- ✅ Tratamento de exceções centralizado
- ✅ Testes automatizados
- ✅ Documentação completa

#### 🔒 Segurança
- Senhas criptografadas com hash seguro
- Tokens JWT com expiração configurável
- Controle de acesso baseado em propriedade
- Validação de entrada em todos os endpoints
- Proteção contra ataques comuns

#### 🧪 Qualidade
- Cobertura de testes abrangente
- Padrões de código consistentes
- Documentação técnica completa
- Arquitetura limpa e manutenível
- Performance otimizada

---

## [0.9.0] - 2024-01-10

### 🚧 Versão Beta

#### ✅ Adicionado
- Estrutura inicial do projeto
- Configuração básica do Entity Framework
- Controllers básicos
- Autenticação JWT inicial
- Testes unitários básicos

#### 🔧 Melhorado
- Estrutura de pastas organizada
- Configuração de dependências
- Documentação inicial

#### 🐛 Corrigido
- Problemas de build
- Configurações de conexão
- Dependências do NuGet

---

## [0.8.0] - 2024-01-05

### 🚧 Versão Alpha

#### ✅ Adicionado
- Projeto base com Clean Architecture
- Configuração inicial do .NET 8
- Estrutura de camadas definida
- Repositórios básicos
- Entidades de domínio

#### 🔧 Melhorado
- Organização do código
- Separação de responsabilidades
- Configuração de dependências

---

## [0.7.0] - 2024-01-01

### 🚧 Versão Pré-Alpha

#### ✅ Adicionado
- Criação inicial do projeto
- Configuração básica do .NET 8
- Estrutura de pastas
- Arquivos de configuração

---

## 🔄 Próximas Versões

### [1.1.0] - Planejado

#### 🎯 Funcionalidades Planejadas
- **Notificações Push**
  - Lembretes de medicamentos
  - Notificações em tempo real
  - Configuração de horários

- **Relatórios e Analytics**
  - Relatórios de adesão
  - Estatísticas de medicamentos
  - Dashboards interativos

- **Melhorias na API**
  - Paginação em listagens
  - Filtros avançados
  - Cache de consultas
  - Rate limiting

#### 🔧 Melhorias Técnicas
- Performance otimizada
- Cache distribuído
- Logs estruturados
- Métricas de monitoramento

### [1.2.0] - Planejado

#### 🎯 Funcionalidades Planejadas
- **Integração com Farmácias**
  - Busca de medicamentos
  - Comparação de preços
  - Pedidos online

- **Histórico Médico**
  - Registro de consultas
  - Histórico de medicamentos
  - Relatórios médicos

- **API Mobile**
  - Endpoints otimizados para mobile
  - Push notifications
  - Sincronização offline

### [2.0.0] - Planejado

#### 🎯 Funcionalidades Planejadas
- **Inteligência Artificial**
  - Recomendações de medicamentos
  - Detecção de interações
  - Alertas inteligentes

- **Integração com Dispositivos**
  - Smartwatches
  - Dispositivos IoT
  - Wearables médicos

- **Telemedicina**
  - Consultas online
  - Prescrições digitais
  - Integração com médicos

---

## 📝 Convenções de Versionamento

### Versionamento Semântico
- **MAJOR.MINOR.PATCH**
- **MAJOR**: Mudanças incompatíveis na API
- **MINOR**: Novas funcionalidades compatíveis
- **PATCH**: Correções de bugs compatíveis

### Tipos de Mudanças
- ✅ **Adicionado**: Novas funcionalidades
- 🔧 **Alterado**: Mudanças em funcionalidades existentes
- 🐛 **Corrigido**: Correções de bugs
- ⚠️ **Depreciado**: Funcionalidades que serão removidas
- 🗑️ **Removido**: Funcionalidades removidas
- 🔒 **Segurança**: Correções de segurança

---

## 🤝 Contribuição

Para contribuir com o projeto:

1. **Crie uma branch** a partir da main
2. **Implemente suas mudanças** 3. **Adicione testes** para novas funcionalidades
4. **Atualize a documentação**
5. **Crie um Pull Request**

### Checklist para Pull Request
- [ ] Código segue os padrões estabelecidos
- [ ] Testes foram adicionados/atualizados
- [ ] Documentação foi atualizada
- [ ] Build passa sem erros
- [ ] Todos os testes passam
- [ ] Changelog foi atualizado

---

## 📞 Suporte

Para dúvidas sobre versões ou mudanças:

- **Issues**: Abra uma issue no repositório
- **Documentação**: Consulte a documentação técnica
- **Swagger**: Use a interface interativa da API

---

**Tomou** - Histórico de versões completo 📋 