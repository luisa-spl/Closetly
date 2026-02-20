# Closetly API 👗✨

![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/Entity%20Framework-b31c2d?style=for-the-badge&logo=dotnet&logoColor=white)
![NUnit](https://img.shields.io/badge/NUnit-25A162?style=for-the-badge&logo=nunit&logoColor=white)

O **Closetly** é uma API RESTful desenvolvida para gerenciar uma plataforma de aluguel de roupas, focada no consumo sustentável e na moda circular. O sistema automatiza todo o ciclo de vida da locação, desde a escolha das peças e processamento do pagamento até a devolução e avaliação do serviço.

## 🚀 Funcionalidades

- **Gestão de Catálogo e Estoque:** Cadastro de produtos (roupas) com controle de status (`AVAILABLE`, `UNAVAILABLE`, `DELETED`).
- **Processamento de Pedidos:** - Criação de locações com períodos predefinidos (3, 7 ou 14 dias).
  - Cancelamento de pedidos (com atualização de status das peças).
  - Devolução de pedidos.
- **Pagamentos:** Registro e validação de pagamentos atrelados aos pedidos.
- **Sistema de Avaliações (Rating):** Permite aos usuários avaliarem seus pedidos com notas de 1 a 5 estrelas.
- **Geração de Relatórios:** Exportação do histórico de locações de um usuário em formato CSV estruturado.

## 🛠️ Tecnologias Utilizadas

- **Linguagem:** C#
- **Framework:** .NET (ASP.NET Core Web API)
- **ORM:** Entity Framework Core
- **Padrão de Arquitetura:** Controller - Service - Repository (Injeção de Dependências)
- **Testes Unitários:** NUnit e Moq
- **Cobertura de Código:** Coverlet & ReportGenerator

## 🏗️ Arquitetura e Boas Práticas

Este projeto foi construído focando em código limpo, manutenção e resiliência:
- Separação clara de responsabilidades entre regras de negócio (Services) e acesso a dados (Repositories).
- Uso extensivo de **DTOs (Data Transfer Objects)** com *Data Annotations* para blindar a API contra dados inválidos.
- Implementação de `CancellationToken` para otimizar requisições e evitar processamento desnecessário no banco de dados.
- Tratamento global e específico de exceções, retornando mensagens claras via `ProblemDetails` e Status Codes HTTP adequados (400, 404, 409, 500).

## 📊 Modelagem de Dados

O banco de dados relacional foi estruturado para garantir a integridade das locações. 
![Diagrama do Banco de Dados do Closetly](https://i.ibb.co/ds058KDj/Captura-de-ecr-2026-02-19-212412.png)

## ⚙️ Como executar o projeto

1. Clone o repositório:
   ```bash
   git clone [https://github.com/luisa-spl/Closetly.git](https://github.com/luisa-spl/Closetly.git)

2. Navegue até a pasta do projeto:
   ```bash
   cd Closetly

3. Restaure as dependências::
   ```bash
   dotnet restore

4. Atualize o banco de dados:
   ```bash
   dotnet ef database update

5. Execute a API:
   ```bash
   dotnet run

# Closetly API — Documentação de Endpoints

A documentação interativa completa está disponível via **Swagger UI** em:

```
http://localhost:<porta>/swagger
```

## Enums de Referência

Os campos do tipo `string` que representam enums aceitam os seguintes valores (verificar a nomenclatura exata no Swagger):

**ProductSize** — tamanhos disponíveis para produto: ‘S’ | ‘M’ | ‘L’ | ‘XL’ | ‘XXL’ | ‘XXXL’.

**ProductType** — tipos de peça (ex: vestido, blusa, calça): ‘TSHIRT’ |‘SHIRT’ | ‘DRESS’ | ‘PANTS’ | ‘SKIRT’ | ‘JACKET’ | ‘SUIT’ | ‘COAT’ | ‘SHOES’ | ‘ACCESSORY’.

**ProductOccasion** — ocasião de uso (ex: casual, festa, trabalho):  ‘CASUAL’ | ‘FORMAL’ | ‘WEDDING’ | ‘PARTY’ .

**ProductStatus** — situação do produto (ex: disponível, alugado, em manutenção): ‘AVAILABLE’ | ‘UNAVAILABLE’ | ‘DELETED’.

**OrderStatus** — situação do pedido (ex: ativo, cancelado, devolvido): ‘PENDING’ | ‘CANCELLED’ | ‘LEASED’ | ‘CONCLUDED’.

**PaymentType** — forma de pagamento (ex: Pix, Cartão, Boleto): 'PIX' | 'CREDIT' | 'DEBIT'.

**PaymentStatus** — situação do pagamento (ex: pendente, pago): 'APPROVED' | 'PENDING'.


Desenvolvido com 💜 por Géssica Medeiros, Marina Bortolucci e Luisa Spinola
