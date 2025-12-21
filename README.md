# 🏎️ F1 Simulator

Projeto de **simulação de campeonatos de Fórmula 1**, desenvolvido com arquitetura de **microsserviços**, focado em regras de negócio, integração entre serviços e processamento assíncrono de eventos.

O objetivo do projeto é simular uma temporada completa de F1, desde a gestão do grid até a execução das corridas e atualização das classificações.

---

## 🧩 Arquitetura

O sistema é composto por microsserviços independentes, cada um com responsabilidades bem definidas:

- **Team Management Service**  
  Responsável pela gestão estrutural do grid: equipes, carros, pilotos, engenheiros e chefes de equipe.

- **Engineering Service**  
  Aplica ajustes técnicos nos carros (coeficientes aerodinâmico e de potência) com base em engenheiros especializados.

- **Race Control Service**  
  Simula os eventos de corrida (treinos, qualificação e corrida), calcula desempenho, pontuação e publica os resultados.

- **Competition Service**  
  Orquestra a temporada: calendários, circuitos, estados das corridas e tabelas de classificação.

---

## 🔄 Comunicação entre Serviços

- Comunicação síncrona via **HTTP/REST**
- Comunicação assíncrona via **RabbitMQ**
  - Evento principal: `RaceFinishedEvent`, publicado ao final de uma corrida

---

## ⚙️ Principais Conceitos

- Temporadas sequenciais (a partir de 2025)
- Calendário fixo de 24 corridas
- Máquina de estados para corridas (Pending → InProgress → Finished)
- Pontuação oficial da Fórmula 1
- Regras rígidas de integridade e imutabilidade

---

## 🛠️ Tecnologias Utilizadas

- **.NET / C#**
- **ASP.NET Web API**
- **RabbitMQ** (mensageria)
- **MongoDB** (Race Control Service)
- **SQL Server** (serviços relacionais)

---

## 📌 Observações

- Cada microsserviço possui sua própria documentação detalhada
- O projeto prioriza regras de negócio claras e separação de responsabilidades
- Ideal para estudos de arquitetura distribuída, mensageria e domínio complexo

---

📚 Documentação detalhada disponível nas pastas de cada serviço.
