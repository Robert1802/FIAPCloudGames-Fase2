# FIAP Cloud Games

## Grupo - 48

- Ricardo Zitelli de Oliveira - RM 360730 - ricardo_zitelli@hotmail.com
- Roberto Lima de Moura - RM364393 - roblm_@hotmail.com
- Victor Prado Chaves - RM362764 - vicpradochaves@gmail.com

## Requisitos Funcionais

- **Cadastro de usuários**: Identificação do cliente por nome, e-mail e senha, e Validar formato de e-mail e senha segura (mínimo de 8 caracteres com números, letras e caracteres especiais).
- **Autenticação e Autorização**: Autenticação via token JWT. Ter dois níveis de acesso: Usuário e Administrador
- **Escalabilidade e Resiliência da aplicação**: Escolher uma infraestrutura que suporte o alto número de alunos e alunas.
- **Dockerizar a aplicação**: Criar uma imagem Docker simples e pequena para facilitar novos deploys.
- **Monitorar a aplicação**: Garantir métricas para entender possíveis problemas com falta de recurso(s) e compreender o comportamento da aplicação.
- **Arquitetura**: Para essa fase, seguiremos com um monolito para facilitar o desenvolvimento ágil e focar na implementação na cloud

## Requisitos Técnicos

- Modulo 1
	- Desenvolvimento de API com .NET 8.
	- Entity Framework Core para gerenciar os modelos de usuários e jogos.
	- Aplicar migrations para a criação do banco de dados.
	- Utilização do MSSQL como banco de dados.
	- API seguindo o padrão Controllers MVC.
	- Implementar Middleware para tratamento de erros e logs estruturados.
	- Documentação com Swagger para expor os endpoints da API.
	- Tratamento de erros e sistema de logs no banco de dados
	- Testes unitários com xUnit
	
- Modulo 2
	- CI quando for dado um Commit/PR e CD ao dar merge na branch master.
		- Criamos duas pipelines no Azure DevOps para automatizar o processo de build e deploy para Homologação e Produção.
	- Criar uma Dockerfile para a elaboração de imagem do FCG relacionada à publicação na cloud.
		- [Dockerfile](https://dev.azure.com/FiapPosNET/_git/FIAPCloudGames?path=%2FDockerfile&version=GBmaster) encontra-se na raiz do projeto.
	- Enviar e armazenar uma imagem Docker em algum repositório.
		- Utilizamos o ACR (Azure Container Registry).
	- Publicar aplicação na cloud e atualiza-la utilizando o pipeline.
		- Utilizamos o Azure App Service para hospedar a aplicação.
	- Utilizar alguma Stack de monitoramento para coletar métricas da aplicação a fim de garantir que a infraestrutura não esteja sofrendo com alto tráfego.
		- Utilizamos o Azure Application Insights para monitoramento e coleta de métricas da aplicação.
	
## DDD - Event Storming

[Diagrama de Event Storming de cadastro de Usuário e Cadastro de Jogos no Miro](https://miro.com/welcomeonboard/YlVLSnhYWFFCWFh5bm5MZWYrQTRhbHEwZkM5K2lpZHNuKzUzSmtSN2JlamFDa1daNUdzVTF5ZCtKRHlhSUtSTlkzbHZNbE1ZSmNkQzc5NHJGZGx6Nk14MWsvOWh1U3c5RDhLYmVpRk5VN2t3akRxejNlQkk2eUZlajRNYU9RRE5yVmtkMG5hNDA3dVlncnBvRVB2ZXBnPT0hdjE=?share_link_id=337316909728)

## Setup do Banco de Dados e Migrações

- Quando o projeto for executado, ele fará as migrações automaticamente para o banco de dados
- Nesta migração inicial também será criado um usuário administrador

## String de Conexão e Appsettings

- Algumas das variáveis de ambiente estão configuradas no arquivo "appsettings.json"
- Utilizamos um arquivo chamado ".ENV" na raiz do projeto para configurar outras
- Durante o Deploy essas variaveis são preenchidas automaticamente pelas variaveis de ambiente da Azure
- Certifique-se de que o arquivo ".ENV" esteja na raiz do projeto e tenha as seguintes variáveis preenchidas:
```
ConnectionStrings__DefaultConnection=
ApplicationInsights__ConnectionString=
SeedAdmin__Email=
SeedAdmin__Senha=
SeedAdmin__Nome=
Jwt__ChaveSecreta=  
Jwt__Issuer=
Jwt__Audience=
SwaggerEnvironment=Localhost
```

## Executar o Projeto

- Abra uma instancia do Docker Desktop no seu computador
- Abra uma instancia do PowerShell na pasta "\FIAPCloudGames.WebApi"
- Execute o comando: docker-compose up --build
- Depois de executar o comando com sucesso, a aplicação estará executando em: [Swagger Localhost](http://localhost:8080/swagger/index.html)

## Link do Youtube

- [FIAP Cloud Games - Modulo 1 - REST API com .NET 8](https://youtu.be/oKnti9S-sew)
- [FIAP Cloud Games - Modulo 2 - Azure e Docker](https://youtu.be/41-tkdHv-1g)