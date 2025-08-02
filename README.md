# FIAP Cloud Games

## Grupo - 48

- Ricardo Zitelli de Oliveira - RM 360730 - ricardo_zitelli@hotmail.com
- Roberto Lima de Moura - RM364393 - roblm_@hotmail.com
- Victor Prado Chaves - RM362764 - vicpradochaves@gmail.com

## Requisitos Funcionais

- **Cadastro de usu�rios**: Identifica��o do cliente por nome, e-mail e senha, e Validar formato de e-mail e senha segura (m�nimo de 8 caracteres com n�meros, letras e caracteres especiais).
- **Autentica��o e Autoriza��o**: Autentica��o via token JWT. Ter dois n�veis de acesso: Usu�rio e Administrador
- **Escalabilidade e Resili�ncia da aplica��o**: Escolher uma infraestrutura que suporte o alto n�mero de alunos e alunas.
- **Dockerizar a aplica��o**: Criar uma imagem Docker simples e pequena para facilitar novos deploys.
- **Monitorar a aplica��o**: Garantir m�tricas para entender poss�veis problemas com falta de recurso(s) e compreender o comportamento da aplica��o.
- **Arquitetura**: Para essa fase, seguiremos com um monolito para facilitar o desenvolvimento �gil e focar na implementa��o na cloud

## Requisitos T�cnicos

- Modulo 1
	- Desenvolvimento de API com .NET 8.
	- Entity Framework Core para gerenciar os modelos de usu�rios e jogos.
	- Aplicar migrations para a cria��o do banco de dados.
	- Utiliza��o do MSSQL como banco de dados.
	- API seguindo o padr�o Controllers MVC.
	- Implementar Middleware para tratamento de erros e logs estruturados.
	- Documenta��o com Swagger para expor os endpoints da API.
	- Tratamento de erros e sistema de logs no banco de dados
	- Testes unit�rios com xUnit
	
- Modulo 2
	- CI quando for dado um Commit/PR e CD ao dar merge na branch master.
		- Criamos duas pipelines no Azure DevOps para automatizar o processo de build e deploy para Homologa��o e Produ��o.
	- Criar uma Dockerfile para a elabora��o de imagem do FCG relacionada � publica��o na cloud.
		- [Dockerfile](https://dev.azure.com/FiapPosNET/_git/FIAPCloudGames?path=%2FDockerfile&version=GBmaster) encontra-se na raiz do projeto.
	- Enviar e armazenar uma imagem Docker em algum reposit�rio.
		- Utilizamos o ACR (Azure Container Registry).
	- Publicar aplica��o na cloud e atualiza-la utilizando o pipeline.
		- Utilizamos o Azure App Service para hospedar a aplica��o.
	- Utilizar alguma Stack de monitoramento para coletar m�tricas da aplica��o a fim de garantir que a infraestrutura n�o esteja sofrendo com alto tr�fego.
		- Utilizamos o Azure Application Insights para monitoramento e coleta de m�tricas da aplica��o.
	
## DDD - Event Storming

[Diagrama de Event Storming de cadastro de Usu�rio e Cadastro de Jogos no Miro](https://miro.com/welcomeonboard/YlVLSnhYWFFCWFh5bm5MZWYrQTRhbHEwZkM5K2lpZHNuKzUzSmtSN2JlamFDa1daNUdzVTF5ZCtKRHlhSUtSTlkzbHZNbE1ZSmNkQzc5NHJGZGx6Nk14MWsvOWh1U3c5RDhLYmVpRk5VN2t3akRxejNlQkk2eUZlajRNYU9RRE5yVmtkMG5hNDA3dVlncnBvRVB2ZXBnPT0hdjE=?share_link_id=337316909728)

## Setup do Banco de Dados e Migra��es

- Quando o projeto for executado, ele far� as migra��es automaticamente para o banco de dados
- Nesta migra��o inicial tamb�m ser� criado um usu�rio administrador

## String de Conex�o e Appsettings

- Algumas das vari�veis de ambiente est�o configuradas no arquivo "appsettings.json"
- Utilizamos um arquivo chamado ".ENV" na raiz do projeto para configurar outras
- Durante o Deploy essas variaveis s�o preenchidas automaticamente pelas variaveis de ambiente da Azure
- Certifique-se de que o arquivo ".ENV" esteja na raiz do projeto e tenha as seguintes vari�veis preenchidas:
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
- Depois de executar o comando com sucesso, a aplica��o estar� executando em: [Swagger Localhost](http://localhost:8080/swagger/index.html)

## Link do Youtube

- [FIAP Cloud Games - Modulo 1 - REST API com .NET 8](https://youtu.be/oKnti9S-sew)
- [FIAP Cloud Games - Modulo 2 - Azure e Docker](https://youtu.be/41-tkdHv-1g)