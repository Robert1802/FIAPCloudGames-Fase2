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
- Testes unitários com xUnit

## DDD - Event Storming

## Setup do Banco de Dados e Migrações

- No Package Manager Console escolha o "Default Project" como "Infrastructure"
- Depois digite: Update-Database -StartupProject Infrastructure

## String de Conexão

- Alterar a string de conexão para o seu servidor local
