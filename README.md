# FIAP Cloud Games

## Grupo - 48

- Roberto Lima de Moura - RM364393 - roblm_@hotmail.com
- Ricardo Zitelli de Oliveira - RM 360730 - ricardo_zitelli@hotmail.com
- Victor Prado Chaves - RM362764 - vicpradochaves@gmail.com

## Requisitos Funcionais

- **Cadastro de usuários**: Identificação do cliente por nome, e-mail e senha, e Validar formato de e-mail e senha segura (mínimo de 8 caracteres com números, letras e caracteres especiais).
- **Autenticação e Autorização**: Autenticação via token JWT. Ter dois níveis de acesso: Usuário e Administrador

## Requisitos Técnicos

- Desenvolvimento de API com .NET 8.
- Entity Framework Core para gerenciar os modelos de usuários e jogos.
- Aplicar migrations para a criação do banco de dados.
- Utilização do MSSQL como banco de dados.
- API seguindo o padrão Controllers MVC.
- Implementar Middleware para tratamento de erros e logs estruturados.
- Documentação com Swagger para expor os endpoints da API.
- Tratamento de erros e sistema de logs no banco de dados
- Testes unitários com xUnit

## DDD - Event Storming

[Diagrama de Envent Storm de cadastro de Usuário e Cadastro de Jogos no Miro](https://miro.com/app/board/uXjVIxoFegg=/)

## Setup do Banco de Dados e Migrações

- Crie um banco de dados chamado "fiapcloudgames" no seu banco de dados local
- Quando o projeto for executado, ele fará as migrações automaticamente para o banco de dados
- Nesta migração inicial também será criado um usuário administrador

## String de Conexão

- Alterar a string de conexão no arquivo "appsettings.json" para o seu servidor local
