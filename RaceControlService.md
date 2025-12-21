# 🏁 Race Control Service

Microserviço responsável por **simular eventos de corrida**, processando dados, aplicando algoritmos de desempenho e gerando os resultados oficiais da temporada.

---

## 📌 Responsabilidade

Este microserviço cuida da **simulação completa da corrida**, sendo responsável por:

- Receber os dados necessários do evento  
- Executar o algoritmo de corrida  
- Gerar classificações e pontuações  
- Persistir os resultados  
- Notificar outros microsserviços ao final da corrida  

---

## 🏎️ Contexto da Corrida

As corridas são os eventos centrais do sistema e seguem um fluxo bem definido de etapas:

### 🔹 Treinos Livres (TL1, TL2 e TL3)

Momento em que os pilotos testam:

- Configurações dos carros  
- Tipos de pneus  
- Ajustes gerais  

⚠️ **Essas etapas não geram pontos e não influenciam o grid de largada.**

---

### 🔹 Qualificação (*Qualifier*)

Etapa competitiva onde os pilotos disputam posições para definir o **grid de largada** da corrida principal.

---

### 🔹 Corrida (*Race*)

Etapa final do evento.

- Os pilotos disputam a vitória no circuito  
- Os resultados impactam diretamente a classificação da temporada  
- 🏆 **Apenas os 10 primeiros colocados pontuam**

---

## ⚙️ Lógicas de Incremento e Pontuação

Durante o evento da corrida, alguns atributos são alterados dinamicamente:

### 🔧 Coeficientes do Veículo

- **Coeficiente Aerodinâmico (Ca)**
- **Coeficiente de Potência (Cp)**  

📌 Esses coeficientes podem sofrer alterações **em qualquer etapa** da corrida.

---

### 🧑‍✈️ Handicap do Piloto

O handicap do piloto também pode ser alterado ao longo do evento.

#### Fórmula de cálculo

```
H[novo] = H[atual] - (Fator[Experiência] × 0.5)
```

**Onde:**

- `H[novo]` → Novo handicap  
- `H[atual]` → Handicap atual do piloto  
- `Fator[Experiência]` → Fator de experiência do piloto  

---

### 📊 Pontuação de Desempenho (PD)

A classificação da **Qualificação** e da **Corrida Final** é definida com base na **Pontuação de Desempenho (PD)**.

A ordem é sempre do **maior para o menor PD**.

#### Fórmula

```
PD = (Ca × 0.4) + (Cp × 0.4) - H + FatorSorte
```

**Componentes:**

- `Ca` → Coeficiente Aerodinâmico  
- `Cp` → Coeficiente de Potência  
- `H` → Handicap atual do piloto  
- `FatorSorte` → Valor aleatório inteiro entre **1 e 10**, permitindo resultados inesperados (*zebras*)

---

## 🔌 Endpoints do Microserviço

| Verbo | Rota | Descrição |
|------|------|-----------|
| POST | `api/race/simulate/tl1` | Inicia o primeiro treino livre |
| POST | `api/race/simulate/tl2` | Inicia o segundo treino livre |
| POST | `api/race/simulate/tl3` | Inicia o terceiro treino livre |
| POST | `api/race/simulate/qualifier` | Inicia a corrida de qualificação |
| POST | `api/race/simulate/race` | Inicia a corrida final (apenas os 10 primeiros pontuam) |
| GET | `api/race/{id}` | Retorna os dados de uma corrida pelo Id |
| GET | `api/race/season/{seasonYear}` | Retorna as corridas de uma temporada específica |

---

## 🗄️ Modelagem do Documento no MongoDB

Para persistir os dados das corridas escolhemos o MongoDB por ser um banco performático e de alta flexibilidade para manipulação do documento (dado salvo no banco).<br>
Afim de melhorar possíveis consultas e para ter um histórico de registros robusto das corridas simuladas, será persistido no MongoDB:
- **RaceOrder**
- **RaceSeason**
- **Circuit**
- **QualifierGrid**
- **RaceGrid**

**Exemplo de estrutura do documento:**
```json
{
	Id: ObjectId('6940ad8e6f832a3ae902c930'),
	RaceId: 'c6f16253-75d0-492a-afcd-2d13fd3ff3a9',
	Round: 1,
	Season: 2025,
	{
		CircuitId: '5092dfbd-ad63-4afb-9338-1881feb7d3c7',
		CircuitName: 'Pista teste',
	    Country: 'Teste',
	    LapsNumber: 3
	},
	[
		{
	      DriverId: '732a83f6-e7a6-48e8-abd6-c5d47a2cc5d9',
	      DriverName: 'Bortoleto',
	      Position: 1
	    },
	    {
	      DriverId: '6a7440c6-d6d1-4a8f-b6f2-8f62a826e11c',
	      Name: 'Leclerc',
	      Position: 2
	    },
	     ...
	],
	[
	    {
	      driverId: 'c742c5d8-34ea-48a1-97d0-3882c156922a',
	      driverName: 'Verstapen',
	      teamId: 'c631ee12-dc06-475a-aa44-3ea9d3d7e71f',
	      teamName: 'RedBull',
	      position: 1,
	      pontuation: 25
	    },
	    {
	      driverId: 'eaf3ceae-37c7-46c0-ade4-a44e515ec8b3',
	      driverName: 'Felipe Massa',
	      teamId: 'f2ca1695-f791-4653-9501-6d94cc4d0884',
	      teamName: 'Ferrari',
	      position: 2,
	      pontuation: 18
	    }, 
	    ...
	]
}
```
---

## 📡 Mensageria com RabbitMQ

Para sinalizar que uma corrida foi devidamente completada, passando por todas as etapas (TL1, TL2, TL3, Qualifier e Race) é publicada uma mensagem na fila do RabbitMQ denominada **"RaceFinishedEvent"** após a execução da última etapa da corrida (race).

Fila utilizada: **RaceFinishedEvent**

---

## ⚙️ Configuração do AppSettings

Para executar o microsserviço de RaceControl é necessário configurar a sua conexão com o Banco de Dados MongoDB. Para configurar sua conexão com o MongoDB você deve acessar o appsettings.json e alterar as configurações para o seu ambiente.
**appsettings.json**
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "MongoDB": {
    "ConnectionURI": "mongodb://localhost:27017/", //sua URL aqui
    "DatabaseName": "F1SimulatorDBRaceControlService"
  }
}
```
No cenário acima a URL na propriedade "ConnectionURI" é a de um MongoDB rodando localmente na máquina.

---

Documented by **[Matheus Rodrigues](https://github.com/MatheusPRodrigues)** 📙