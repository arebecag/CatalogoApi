# Projeto de Catálogo de Produtos

Este projeto consiste em uma API desenvolvida em .NET 8 e um front-end desenvolvido em React para gerenciamento de produtos.

## O Frontend está em uma pasta separada nomeada como "frontend"

### Backend

- .NET 8 SDK
- SQL Server

## Configuração do SQL Server:
- Certifique-se de que o SQL Server está instalado e rodando em sua máquina.
- O projeto está configurado para utilizar o SQL Server em localhost.
- Atualize a string de conexão:
No arquivo appsettings.Development.json, verifique se a string de conexão está configurada corretamente para apontar para o seu SQL Server local.

- Aplicar migrações e atualizar o banco de dados:
  ### `update-database`

- Rodar a aplicação:
- ### `dotnet run`

### Configuração do Frontend:

- Node.js
- npm (Node Package Manager)

### Caso não tenha instalado:
Instalar dependências:
### `npm install`
Rodar a aplicação:
### `npm start`

O front-end estará disponível em:
### `http://localhost:3000`


