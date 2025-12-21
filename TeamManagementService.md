# Team Management Service

# F1 Simulator – Team Management Service

Esta documentação descreve **todos os endpoints** do **Team Management Service**, organizados por controller, com explicação **passo a passo**, regras de negócio e fluxos completos.

O objetivo é que qualquer desenvolvedor consiga **entender, testar e manter** o microserviço apenas lendo este documento.

---

## Visão Geral

O **Team Management Service** é responsável por toda a gestão estrutural do grid da Fórmula 1 no simulador:

- Teams (Equipes)
- Cars (Carros)
- Drivers (Pilotos)
- Engineers (Engenheiros)
- Bosses (Chefes de equipe)

Ele garante **limites globais**, **vínculos corretos** entre entidades e **regras de temporada ativa**.

---

## Regra Global – Temporada Ativa

Antes de qualquer **criação, atualização ou exclusão**, o serviço consulta o **Competition Service**.

### Fluxo:

1. O controller chama o service
2. O service consulta `ICompetitionClient.GetActiveSeasonAsync()`
3. Se existir temporada ativa:
    - A operação é bloqueada
    - Uma exceção é lançada
4. Caso contrário, a execução continua

Essa regra se aplica a **todos os controllers**.

---

# TeamController

### Equipes

| Verbo | Rota | Descrição |
| --- | --- | --- |
| POST | `/api/team` | Cria uma nova equipe |
| GET | `/api/team` | Lista todas as equipes |
| GET | `/api/team/{teamId}` | Busca equipe por ID |
| PATCH | `/api/team/update/{teamId}` | Atualiza o país da equipe |

---

# CarController

**Base URL:** `/api/car`

Responsável pela gestão dos carros.

## POST /api/car – Criar carro

### Passo a passo:

1. Verifica temporada ativa
2. Valida body
3. Valida TeamId
4. Verifica limite de 2 carros por equipe
5. Verifica limite global de 22 carros
6. Valida peso e velocidade
7. Gera coeficientes
8. Persiste o carro

---

## GET /api/car – Listar carros

### Passo a passo:

1. Busca todos os carros
2. Retorna 204 se vazio

---

## GET /api/car/{carId} – Buscar carro por ID

### Passo a passo:

1. Valida GUID
2. Busca carro

---

## PATCH /api/car/{carId}/model – Atualizar modelo do carro

### # CarController

### Carros

| Verbo | Rota | Descrição |
| --- | --- | --- |
| POS# DriverController |  |  |

### Pilotos

| Verbo | Rota | Descrição |
| --- | --- | --- |
| POST | `/api/driver` | Cria um novo piloto |
| GET | `/api/driver` | Lista todos os pilotos |
| GET | `/api/driver/{id}` | Busca piloto por ID |
| PATCH | `/api/driver/{id}` | Atualiza dados do piloto |

---

# EngineerController

pa# EngineerController

### Engenheiros

| Verbo | Rota | Descrição |
| --- | --- | --- |
| POST | `/api/engineer` | Cria um novo engenheiro |
| GET | `/api/engineer` | Lis# BossController |

### Chefes de Equipe

| Verbo | Rota | Descrição |
| --- | --- | --- |
| POST | `/api/boss` | Cria um novo chefe de equipe |
| GET | `/api/boss` | Lista todos os chefes |
| GET | `/api/boss/{id}` | Busca chefe por ID |

---

## Conclusão

Este documento cobre **100% dos endpoints** do Team Management Service, detalhando:

- Fluxo de execução
- Regras de negócio
- Limites globais
- Responsabilidades por controller

Ele serve como **documentação oficial**, base para **Swagger/OpenAPI**, README e manutenção futura.