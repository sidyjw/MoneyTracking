# Plano de Implementação: Remoção do ClearValidationErrors dos Métodos de Entidades

## Resumo da Mudança

**Comportamento Atual:**
- Cada método de entidade chama `ClearValidationErrors()` no início
- Os erros são limpados antes de cada validação
- Responsabilidade da entidade

**Novo Comportamento:**
- Métodos de entidade NÃO chamam `ClearValidationErrors()`
- Erros são acumulados progressivamente em múltiplas chamadas
- Responsabilidade da **Feature** de limpar erros antes de validar/persistir

---

## 1. Arquivos da Camada de Domínio (Entidades)

### 1.1 `Domain/User/User.cs` (5 métodos)

| Método | Linha | Ação |
|--------|-------|------|
| `UpdateName` | 46 | Remover `ClearValidationErrors();` |
| `UpdateEmail` | 66 | Remover `ClearValidationErrors();` |
| `AddCategory` | 86 | Remover `ClearValidationErrors();` |
| `RemoveCategory` | 105 | Remover `ClearValidationErrors();` |
| `AddAccount` | 124 | Remover `ClearValidationErrors();` |

### 1.2 `Domain/Account/Account.cs` (4 métodos)

| Método | Linha | Ação |
|--------|-------|------|
| `UpdateName` | 45 | Remover `ClearValidationErrors();` |
| `UpdateType` | 58 | Remover `ClearValidationErrors();` |
| `Credit` | 71 | Remover `ClearValidationErrors();` |
| `Debit` | 93 | Remover `ClearValidationErrors();` |

### 1.3 `Domain/Category/Category.cs` (4 métodos)

| Método | Linha | Ação |
|--------|-------|------|
| `UpdateName` | 38 | Remover `ClearValidationErrors();` |
| `UpdateType` | 51 | Remover `ClearValidationErrors();` |
| `SetMonthlyBudget` | 64 | Remover `ClearValidationErrors();` |
| `RemoveMonthlyBudget` | 77 | Remover `ClearValidationErrors();` |

### 1.4 `Domain/Transaction/Transaction.cs` (1 método)

| Método | Linha | Ação |
|--------|-------|------|
| `Create` | 50 | Remover `ClearValidationErrors();` |

---

## 2. Atualização da Documentação

### 2.1 `AGENTS.md`

**Seção: Error Handling (linhas 85-90)**

Alterar de:
```
- Use `ClearValidationErrors()`, `AddError()`, `HasValidationErrors()`, `GetValidationErrors()` helpers
- Return early on validation failure
- Collect multiple validation errors before returning
```

Para:
```
- Use `AddError()`, `AddErrors()`, `HasValidationErrors()`, `GetValidationErrors()`, `ClearValidationErrors()` helpers
- **Features** must call `entity.ClearValidationErrors()` before calling entity methods to ensure clean state
- Entities accumulate errors across method calls
- Return all accumulated errors when `HasValidationErrors()` is true
```

**Seção: Entity Update Method (linhas 157-177)**

Alterar de:
```csharp
public ResultT<User> UpdateName(string newName)
{
    ClearValidationErrors();
    
    if (newName == _name.Full)
        AddError(Error.Validation(UserErrors.NameUnchanged, "Name must be different"));
    
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

Para:
```csharp
public ResultT<User> UpdateName(string newName)
{
    if (newName == _name.Full)
        AddError(Error.Validation(UserErrors.NameUnchanged, "Name must be different"));
    
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

**Adicionar nova seção após "Feature Layer Operation":**

```csharp
#### Feature Layer with Error Clearing
public async Task<ResultT<UpdateUserResult>> UpdateUserAsync(UpdateUserRequest request)
{
    var user = await _userRepository.GetByIdAsync(request.UserId);
    if (user is null)
        return Error.NotFound(UserErrors.UserNotFound, "User not found");
    
    // Clear errors before entity operations
    user.ClearValidationErrors();
    
    var updateResult = user.UpdateName(request.Name);
    if (updateResult.IsFailure)
        return updateResult.Errors!;
    
    await _userRepository.UpdateAsync(user);
    return new UpdateUserResult(user.Id);
}
```

### 2.2 `PLAN_VALIDATION_ERRORS.md`

Alterar o "Design Decisivo" (linhas 7-12) de:
```
1. **Lista de erros na classe `Entity`**: Centralizada e acessível por todas as entidades
2. **Métodos helper protected**: `AddError()`, `AddErrors()`, `HasValidationErrors()`, `GetValidationErrors()`, `ClearValidationErrors()`
3. **Gerenciamento manual**: Cada método chama `ClearValidationErrors()` no início
4. **Validação de ValueObjects**: Manter padrão híbrido atual - ValueObjects continuam retornando `ResultT`, entidades adicionam erros da lista deles
```

Para:
```
1. **Lista de erros na classe `Entity`**: Centralizada e acessível por todas as entidades
2. **Métodos helper protected**: `AddError()`, `AddErrors()`, `HasValidationErrors()`, `GetValidationErrors()`, `ClearValidationErrors()`
3. **Gerenciamento por Features**: Métodos de entidade NÃO chamam `ClearValidationErrors()`; Features são responsáveis por limpar erros antes de operações
4. **Validação de ValueObjects**: Manter padrão híbrido atual - ValueObjects continuam retornando `ResultT`, entidades adicionam erros da lista deles
```

Alterar o "Padrão de Validação a Ser Aplicado" (linhas 56-95) removendo todas as chamadas de `ClearValidationErrors();`.

### 2.3 `README.md`

**Seção: Convenções de Código (linhas 109-118)**

Adicionar após a linha 118:
```
- Entidades acumulam erros de validação progressivamente
- Features devem limpar erros (`entity.ClearValidationErrors()`) antes de operações de entidade
```

**Seção: Próximos Passos (linhas 96-107)**

Adicionar após a linha 107:
```
- Atualizar documentação para refletir padrão de acumulação de erros
```

---

## 3. Impacto no Código Existente

### Análise de Impacto

**Features atuais:**
- `Features/User/Create/Create.cs` - Usa apenas `User.Create()` (sem chamadas de ClearValidationErrors necessárias)
- `Features/Account/Add/Add.cs` - Usa apenas `Account.Create()` (sem chamadas de ClearValidationErrors necessárias)

**Conclusão:**
- As Features existentes NÃO precisam de alterações imediatas
- Somente Features futuras que chamam métodos de update precisarão adicionar `ClearValidationErrors()`

---

## 4. Exemplo de Uso Pós-Mudança

### Cenário: Feature Atualiza Múltiplos Atributos

```csharp
public async Task<ResultT<UpdateUserResult>> UpdateUserAsync(UpdateUserRequest request)
{
    var user = await _userRepository.GetByIdAsync(request.UserId);
    if (user is null)
        return Error.NotFound(UserErrors.UserNotFound, "User not found");
    
    // Feature limpa erros antes de operar na entidade
    user.ClearValidationErrors();
    
    // Acumula erros de ambas as operações
    user.UpdateName(request.Name);
    user.UpdateEmail(request.Email);
    
    // Verifica se há erros acumulados
    if (user.HasValidationErrors())
        return user.GetValidationErrors();
    
    await _userRepository.UpdateAsync(user);
    return new UpdateUserResult(user.Id);
}
```

---

## 5. Testes a Considerar

Após a implementação, criar testes para validar:
1. **Acumulação de erros:** Múltiplas chamadas de métodos de entidade acumulam erros
2. **Limpeza pela Feature:** Feature limpa erros antes de operações
3. **Comportamento sem limpeza:** Erros persistem entre chamadas se Feature não limpar
4. **Validação final:** Feature verifica `HasValidationErrors()` antes de persistir

---

## 6. Benefícios da Nova Abordagem

1. **Flexibilidade:** Features podem decidir quando limpar erros
2. **Validação em lote:** Acumula erros de múltiplas operações
3. **Responsabilidade clara:** Features controlam o fluxo de validação
4. **DDD mais correto:** Entidades mantêm estado, Features orquestram operações
