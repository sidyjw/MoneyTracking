# Plano de Implementação: Acumulação de Erros nas Entidades

## Visão Geral

Objetivo: Alterar as Entidades para que acumulem múltiplos erros de validação em vez de retornar no primeiro erro encontrado, seguindo o padrão já implementado em `User.Create`.

## Design Decisivo

1. **Lista de erros na classe `Entity`**: Centralizada e acessível por todas as entidades
2. **Métodos helper protected**: `AddError()`, `AddErrors()`, `HasValidationErrors()`, `GetValidationErrors()`, `ClearValidationErrors()`
3. **Gerenciamento por Features**: Métodos de entidade NÃO chamam `ClearValidationErrors()`; Features são responsáveis por limpar erros antes de operações
4. **Validação de ValueObjects**: Manter padrão híbrido atual - ValueObjects continuam retornando `ResultT`, entidades adicionam erros da lista deles

---

## 1. Modificar `Domain/Common/Entity.cs`

### Adicionar Infraestrutura de Erros

```csharp
protected List<Error> ValidationErrors { get; private set; } = [];

protected void AddError(Error error)
{
    ValidationErrors.Add(error);
}

protected void AddErrors(List<Error> errors)
{
    ValidationErrors.AddRange(errors);
}

protected bool HasValidationErrors()
{
    return ValidationErrors.Count > 0;
}

protected List<Error> GetValidationErrors()
{
    return ValidationErrors;
}

protected void ClearValidationErrors()
{
    ValidationErrors.Clear();
}
```

---

## 2. Padrão de Validação a Ser Aplicado

### Para Métodos com Múltiplas Validações

```csharp
public ResultT<Entity> MethodName(params)
{
    // Acumular erros em vez de retornar antecipadamente
    if (condition1)
        AddError(Error.Validation(ErrorCode1, Description1));
    
    if (condition2)
        AddError(Error.Validation(ErrorCode2, Description2));
    
    var result = ValueObject.Create(...);
    if (result.IsFailure)
        AddErrors(result.Errors!);
    
    // Retornar todos erros acumulados ou sucesso
    if (HasValidationErrors())
        return GetValidationErrors();
    
    // Lógica de sucesso...
    UpdateTimestamp();
    return this;
}
```

### Para Métodos com Única Validação

```csharp
public ResultT<Entity> MethodName(params)
{
    var result = ValueObject.Create(...);
    if (result.IsFailure)
        return result.Errors!;
    
    // Lógica de sucesso...
    UpdateTimestamp();
    return this;
}
```

---

## 3. Arquivos e Métodos a Modificar

### `Domain/Common/Entity.cs` (NOVO)
- Adicionar propriedades e métodos de validação

---

### `Domain/User/User.cs` (4 métodos)

#### 1. `UpdateName` (linhas 44-57)
**Validações a acumular:**
- `if (newName == _name.Full)` → Error.Validation
- `UserFullName.Create(newName)` → Adicionar errors do ResultT

#### 2. `UpdateEmail` (linhas 59-72)
**Validações a acumular:**
- `if (newEmail == _email.Value)` → Error.Validation
- `Email.Create(newEmail)` → Adicionar errors do ResultT

#### 3. `AddCategory` (linhas 74-86)
**Validações a acumular:**
- `if (newCategory is null)` → Error.Validation
- `if (_categories.Any(...))` → Error.Conflict

#### 4. `RemoveCategory` (linhas 88-100)
**Validações a acumular:**
- `if (categoryToRemove is null)` → Error.Validation
- `if (!_categories.Contains(...))` → Error.NotFound

---

### `Domain/Account/Account.cs` (4 métodos)

#### 5. `Credit` (linhas 65-80)
**Validações a acumular:**
- `if (newTransaction is null)` → Error.Validation
- `if (newTransaction.Type != TransactionType.Income)` → Error.Validation
- `_balance.Credit(...)` → Adicionar errors do ResultT

#### 6. `Debit` (linhas 82-97)
**Validações a acumular:**
- `if (newTransaction is null)` → Error.Validation
- `if (newTransaction.Type != TransactionType.Expense)` → Error.Validation
- `_balance.Debit(...)` → Adicionar errors do ResultT

#### 7. `UpdateName` (linhas 43-52)
**Validação única:**
- `if (newName.Value == _name.Value)` → Usar padrão de validação única

#### 8. `UpdateType` (linhas 54-63)
**Validação única:**
- `if (newType == _type)` → Usar padrão de validação única

---

### `Domain/Category/Category.cs` (3 métodos)

#### 9. `UpdateName` (linhas 36-45)
**Validação única:**
- `if (newName == _name)` → Usar padrão de validação única

#### 10. `UpdateType` (linhas 47-56)
**Validação única:**
- `if (newType == _type)` → Usar padrão de validação única

#### 11. `SetMonthlyBudget` (linhas 58-67)
**Validação única:**
- `if (monthlyBudget is null)` → Usar padrão de validação única

---

### `Domain/Transaction/Transaction.cs` (1 método)

#### 12. `Create` (linhas 38-55)
**Validações a acumular:**
- `if (type == TransactionType.Income && amount.Value <= 0)` → Error.Validation
- `if (type == TransactionType.Expense && amount.Value >= 0)` → Error.Validation

---

## 4. Compatibilidade com Código Existente

A mudança **NÃO quebra código existente** porque:

- `GetValidationErrors()` retorna `List<Error>`
- `ResultT<T>` tem conversão implícita de `List<Error>` (verificado em `ResultT.cs:50-51`)
- Features já usam `result.Errors!` que funciona com lista:
  - `Features/User/Create/Create.cs:10-11` - usa `emailResult.Errors!`
  - `Features/User/Create/Create.cs:18-19` - usa `userResult.Errors!`
  - `Features/Account/Add/Add.cs:17-18` - usa `accountResult.Errors!`

---

## 5. Exemplo Antes vs Depois

### Antes (Retorno Antecipado)

```csharp
public ResultT<User> UpdateName(string newName)
{
    if (newName == _name.Full)
        return Error.Validation(UserErrors.NameUnchanged, "...");
    
    var nameResult = UserFullName.Create(newName);
    if (nameResult.IsFailure)
        return nameResult.Errors!;
    
    _name = nameResult.Value;
    UpdateTimestamp();
    return this;
}
```

### Depois (Acumulação de Erros)

```csharp
public ResultT<User> UpdateName(string newName)
{
    if (newName == _name.Full)
        AddError(Error.Validation(UserErrors.NameUnchanged, "..."));
    
    var nameResult = UserFullName.Create(newName);
    if (nameResult.IsFailure)
        AddErrors(nameResult.Errors!);
    
    if (HasValidationErrors())
        return GetValidationErrors();
    
    _name = nameResult.Value;
    UpdateTimestamp();
    return this;
}
```

---

## 6. Benefícios da Abordagem

1. **Acumulação de múltiplos erros**: Usuário recebe todos os erros de uma vez
2. **Centralização**: Lista de erros gerenciada na classe `Entity`
3. **Consistência**: Todas as entidades seguem o mesmo padrão
4. **Manutenibilidade**: Métodos helper facilitam adicionar/remover validações
5. **Backward compatibility**: Não quebra código existente

---

## 7. Testes a Considerar

Como não há testes unitários implementados (verificado em `Tests/Features/UserFeatures/UserFeaturesTests.cs:1-8`), os testes deverão ser criados após a implementação para validar:
- Acumulação de múltiplos erros em um único método
- Limpeza adequada da lista entre chamadas
- Retorno correto de `List<Error>` em caso de falha
- Retorno correto da entidade em caso de sucesso
