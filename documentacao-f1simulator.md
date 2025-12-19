# 📄 Diretrizes Consolidadas de Código C# / .NET (Equipe)

Este documento estabelece as **convenções obrigatórias** de nomenclatura, estilo, organização e tratamento de erros para o desenvolvimento de aplicações .NET na equipe.

> *Baseado em conteúdos de aula e nas convenções oficiais da Microsoft (coding conventions).*  

---

## I. Padrões de Nomenclatura

| Elemento | Padrão | Exemplo |
|--------|--------|--------|
| Classes / Enums | PascalCase | `public class Customer` |
| Interfaces | PascalCase com prefixo `I` | `public interface IService` |
| Métodos Assíncronos | PascalCase + sufixo `Async` | `LoadDataAsync()` |
| Propriedades | PascalCase | `public string FullName { get; private set; }` |
| Namespaces | PascalCase | `MyProject.Utilities` |
| Variáveis Locais | camelCase | `int itemQuantity = 0;` |
| Parâmetros de Método | camelCase | `Calculate(decimal totalValue)` |
| Campos Privados | `_camelCase` + `readonly` | `private readonly string _customerName;` |
| Constantes | PascalCase | `private const int MaximumSize = 100;` |

---

## II. Estilo de Codificação C#

| Área | Diretriz | Exemplo |
|----|---------|--------|
| Linguagem | Inglês obrigatório | `public class OrderService` |
| Declaração de Variáveis | Usar `var` quando o tipo for óbvio | `var customer = new Customer();` |
| Strings | Usar interpolação | `$"User ID: {userId}"` |
| Verificação de Nulo | Usar `is null` / `is not null` | `if (user is null)` |
| Gerenciamento de Recursos | Utilizar `using` com `IDisposable` | `using var connection = new SqlConnection();` |
| DTOs / Models | Propriedades imutáveis com `init` | `public Guid Id { get; init; }` |
| Commit | Commits pequenos e frequentes | — |

---

## III. Diretrizes de Código e Boas Práticas

| Área | Diretriz |
|----|---------|
| Logs | Utilizar `ILogger` via injeção de dependência |
| Tratamento de Erros | `try/catch` apenas para erros tratáveis ou para adicionar contexto |
| Organização | Estrutura de pastas baseada no contexto e arquitetura |
| Banco Relacional | SQL Server + Dapper + chave primária GUID (obrigatório) |
| Banco Não Relacional | MongoDB com driver oficial ou ODM |
| Mensageria | RabbitMQ para comunicação assíncrona |
| Testes de API | Utilizar Insomnia |

---

## IV. Tratamento de Erros por Verbo HTTP

| Verbo | Sucesso | Falha de Negócio | Falha Inesperada |
|-----|--------|-----------------|-----------------|
| GET | 200 OK | 404 Not Found | 500 Internal Server Error |
| POST | 201 Created | 400 Bad Request | 500 Internal Server Error |
| PUT | 200 OK / 204 No Content | 400 Bad Request / 404 Not Found | 500 Internal Server Error |
| PATCH | 200 OK / 204 No Content | 400 Bad Request / 404 Not Found | 500 Internal Server Error |

---

## V. Mapeamento de Exceptions

| Camada | Exceptions Aceitas | Mapeamento |
|------|------------------|-----------|
| Repository | `SqlException`, `Exception` | Erros de banco ou infraestrutura |
| Service | `KeyNotFoundException` | Recurso não encontrado (404) |
| Service | `ArgumentException` | Dados inválidos (400) |
| Service | `InvalidOperationException` | Conflito de estado (409) |
| Controller | `KeyNotFoundException`, `ArgumentException`, `Exception` | Mapeamento para HTTP 400, 404 e 500 |

---

## VI. Gerenciamento de Dependências (Lifetime)

| Lifetime | Definição | Uso Recomendado |
|--------|----------|----------------|
| Scoped | Uma instância por requisição HTTP | Repositórios e serviços |
| Singleton | Uma instância por aplicação | Serviços stateless, HTTP Clients, Loggers |
| Transient | Nova instância a cada uso | Classes leves, factories |

---

## VII. Documentação Obrigatória da API

| Documento | Finalidade |
|---------|------------|
| README.md | Guia principal da API |
| Insomnia Collection | Testes de rotas e ambientes |

---

## VIII. Integração e Fluxo de Mensageria (RabbitMQ)

| Item | Descrição |
|----|-----------|
| Evento de Referência | `RaceFinishedEvent` |
| Serviço Produtor | RaceControlService |
| Serviço Consumidor | CompetitionService |
| Exchange | Default Exchange |
| Fluxo | Finaliza corrida, altera status do circuito, processa pontuação e continua temporada |
| Justificativa | Comunicação ponto-a-ponto simples, sem fanout ou tópicos |

