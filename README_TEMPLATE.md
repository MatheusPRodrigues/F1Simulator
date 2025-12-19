# 📬 Insomnia – Exportação, Importação e Diretrizes

> **Template oficial** para documentação de uso do Insomnia.
>
> Substitua os campos indicados e ajuste conforme a necessidade da API.

---

## 📄 Visão geral

Este documento define o **padrão oficial** para exportação, versionamento e importação de **collections do Insomnia** neste repositório.

### Objetivos

- Padronização do uso do Insomnia
- Segurança no versionamento
- Facilidade de uso e onboarding

---

## 📁 Estrutura no repositório

As collections do Insomnia **devem ser salvas fora do código-fonte**, seguindo obrigatoriamente a estrutura abaixo:

```
docs/
└─ Insomnia/
   └─ Nome_da_API/
      ├─ collection.yaml
```

### Exemplo

```
docs/Insomnia/EngineeringService/
├─ engineering-service.collection.yaml
```

❌ Não é permitido armazenar arquivos do Insomnia dentro de pastas de código da aplicação.

---

## 📤 Como exportar a collection no Insomnia

Existem **duas formas suportadas** para exportação da collection no Insomnia.

---

### 🔹 Opção 1 — Exportação via menu de preferências

1. Abrir o **Insomnia**
2. Acessar:

```
Application → Preferences → Data → Export Data
```

3. Selecionar a **Collection / Workspace** desejada
4. Em **Select Export Type**, selecionar obrigatoriamente:

```
Insomnia v5
```

5. Escolher o formato do arquivo:
   - `YAML` (obrigatório)
6. Salvar o arquivo com o nome:

```
nome-da-api.collection.yaml
```

---

### 🔹 Opção 2 — Exportação direta pela Collection

1. Abrir o **Insomnia**
2. Localizar a **Collection / Workspace** desejada
3. Clicar nos **três pontos (⋮)** ao lado do nome da collection
4. Selecionar a opção **Export**
5. Em **Select Export Type**, selecionar obrigatoriamente:

```
Insomnia v5
```

6. Escolher o formato do arquivo:
   - `YAML` (obrigatório)
7. Salvar o arquivo com o nome:

```
nome-da-api.collection.yaml
```

---

## 📥 Como importar a collection no Insomnia

1. Abrir o **Insomnia**
2. Clicar em **Import/Export**
3. Selecionar **Import Data → From File**
4. Escolher o arquivo:

```
docs/Insomnia/Nome_da_API/nome-da-api.collection.yaml
```

5. Confirmar a importação

---

## 🌱 Uso de variáveis de ambiente

O uso de **variáveis de ambiente é obrigatório** em todas as collections.

### Diretrizes

- ❌ Não utilizar URLs fixas nos requests
- ✅ Utilizar variáveis no formato:

```
{{ base_url }}
{{ resource_id }}
```

---

## ✅ Checklist antes do commit

- [ ] Collection exportada no formato **Insomnia v5**
- [ ] Arquivo salvo em `docs/Insomnia/Nome_da_API/`
- [ ] URLs utilizando `{{ base_url }}`
- [ ] Nenhuma informação sensível versionada

---

## 📌 Considerações finais

- A collection do Insomnia faz parte da documentação técnica do projeto
- Deve refletir sempre o estado atual da API
- Alterações nos endpoints exigem atualização da collection

