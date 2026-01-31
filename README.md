# MoneyTracking

**MoneyTracking** é uma aplicação de gerenciamento financeiro pessoal escrita em C#/.NET 10.0, projetada para ajudar usuários a rastrear suas contas financeiras, transações, categorias e orçamentos.

## 🚀 Stack Tecnológica

- **.NET 10.0** - Framework alvo
- **C# 12+** - Recursos modernos da linguagem
- **xUnit 2.9.3** - Framework de testes
- **Clean Architecture / DDD** - Arquitetura de software

## 🏗️ Arquitetura

O projeto segue os princípios de **Clean Architecture** e **Domain-Driven Design (DDD)** com clara separação de responsabilidades:

```
MoneyTracking/
├── Domain/           # Camada de domínio (entidades, value objects, lógica de negócio)
├── Features/         # Camada de aplicação (serviços e casos de uso)
├── Infra/            # Camada de infraestrutura (interfaces de repositórios)
└── Tests/            # Camada de testes (testes unitários e de integração)
```

### Camada de Domínio (Domain/)

Contém as classes principais do domínio, incluindo:

- **Entidades**: `User`, `Account`, `Transaction`, `Category`, `MonthlyBudget`
- **Value Objects**: `UserFullName`, `Email`, `AccountName`, `Balance`, `MonthYear`, etc.
- **Result Pattern**: `Result` e `ResultT<TValue>` para tratamento de erros sem exceções
- **Error Pattern**: `Error` e `ErrorType` para representação de erros

### Camada de Features (Features/)

Contém a lógica de aplicação e casos de uso:

- **UserFeature**: Criação de usuários
- **AccountFeature**: Adição de contas a usuários

### Camada de Infraestrutura (Infra/)

Contém interfaces para acesso a dados:

- `IUserRepository`: Interface para repositório de usuários
- `IAccountRepository`: Interface para repositório de contas

*Nota: Implementação de banco de dados e ORM ainda pendente.*

## 🎯 Padrões Utilizados

1. **Domain-Driven Design (DDD)** - Modelos de domínio ricos, value objects, agregados
2. **Clean Architecture** - Separação de responsabilidades, inversão de dependência
3. **Result Pattern** - Para tratamento de erros sem exceções
4. **Repository Pattern** - Para abstração de acesso a dados
5. **Factory Pattern** - Métodos estáticos `Create()` em entidades
6. **Record Types** - Para value objects imutáveis
7. **Partial Classes** - Features divididas em múltiplos arquivos

## 📦 Como Compilar e Executar

### Compilação
```bash
dotnet build
```

### Executar Testes
```bash
dotnet test
```

### Executar Testes com Cobertura
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## 📊 Status do Projeto

**Em desenvolvimento** - Projeto em fase inicial com modelos de domínio básicos implementados, mas sem camada de apresentação ou persistência de banco de dados.

### ✅ Implementado
- Modelos de domínio completos (User, Account, Transaction, Category, MonthlyBudget)
- Value Objects com validação
- Result Pattern para tratamento de erros
- Features básicas (criar usuário, adicionar conta)
- Interfaces de repositório

### ⏳ Pendente
- Implementação de repositórios com ORM (Entity Framework Core recomendado)
- Camada de apresentação (Web API ou Blazor UI)
- Features completas (Transações, Categorias)
- Testes abrangentes
- Camada de persistência de banco de dados
- Autenticação e autorização
- Documentação (API docs)

## 🛠️ Próximos Passos

1. Implementar camada de repositórios com Entity Framework Core
2. Adicionar camada de apresentação (Web API ou Blazor)
3. Implementar features de Transactions e Categories
4. Escrever testes abrangentes
5. Adicionar documentação completa (README, API docs)
6. Implementar features de ícone e cor para categorias
7. Adicionar validação e melhorias de tratamento de erros
8. Adicionar logging e monitoramento
9. Considerar autenticação/autorização
10. Adicionar gerenciamento de configuração (appsettings, variáveis de ambiente)
11. Atualizar documentação para refletir padrão de acumulação de erros

## 📝 Convenções de Código

- Null reference types habilitados (`<Nullable>enable</Nullable>`)
- Implicit usings para código mais limpo
- Global usings para imports comuns
- Validações em construtores de Value Objects
- Métodos estáticos `Create()` para entidades
- Métodos de update retornam `ResultT<TEntity>` para method chaining
- Tratamento de erros via Result Pattern (não exceções)
- Entidades acumulam erros de validação progressivamente
- Features devem limpar erros (`entity.ClearValidationErrors()`) antes de operações de entidade

## 📄 Licença

Este projeto está em desenvolvimento e não possui licença definida ainda.
