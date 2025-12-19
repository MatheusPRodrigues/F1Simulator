# Engineering Service API

Este serviço faz parte do projeto **F1Simulator** e é responsável por aplicar **ajustes nos coeficientes técnicos dos carros (Ca e Cp)** com base na atuação de engenheiros especializados.

>

## 🎯 Responsabilidade do Serviço

O **Engineering Service** atua como uma camada de orquestração que:

* Consulta dados de **Carros** em outro serviço
* Consulta dados de **Engenheiros** em outro serviço
* Aplica regras de negócio e fatores aleatórios
* Atualiza os coeficientes aerodinâmico (**Ca**) e de potência (**Cp**) do carro

---

## 🧩 Regras de Negócio

* Um engenheiro **só pode alterar** coeficientes do carro ao qual está associado
* O ajuste do coeficiente é calculado usando:

  * `ExperienceFactor` do engenheiro
  * Um fator aleatório entre `-1` e `1`
* Os valores finais de **Ca** e **Cp** são limitados entre `0` e `10`
* Se nenhum coeficiente for alterado, a operação falha

---

## 🌐 Endpoints

### Atualizar coeficientes do carro

**PUT** `/api/engineering/car/{carId}`

#### Path Params

| Nome  | Tipo          | Descrição                                        |
| ----- | ------------- | ------------------------------------------------ |
| carId | string (GUID) | ID do carro que terá os coeficientes atualizados |

#### Body

```json
{
  "engineerCaId": "string",
  "engineerCpId": "string"
}
```

> Ambos os campos são opcionais, porém **ao menos um deve ser informado**.

---

## 📤 Resposta da requisição (200 OK)

```json
{
  "carId": "9f3a1d2e-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "teamId": "bbbbbbbb-cccc-dddd-eeee-ffffffffffff",
  "model": "F1-2025",
  "weightKg": 798,
  "speed": 350,
  "ca": 6.42,
  "cp": 7.10,
  "isActive": true
}
```

---

## ⚠️ Possíveis Erros

| Status Code               | Motivo                                                              |
| ------------------------- | ------------------------------------------------------------------- |
| 400 Bad Request           | Nenhum coeficiente foi alterado ou engenheiro não pertence ao carro |
| 404 Not Found             | Carro não encontrado                                                |
| 500 Internal Server Error | Erro inesperado                                                     |

---

## 📌 Observações Finais

* Este serviço não persiste dados
* Atua apenas como processador de regras e integrador
