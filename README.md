# ControleDeAcesso

Projeto ASP.NET Core para autenticação e controle de acesso de usuários.

## Funcionalidades

- Cadastro de usuários
- Login com JWT
- Validação de senha
- Retorno com padrão de sucesso ou erro

## Tecnologias

- ASP.NET Core
- MySQL
- JWT
- AutoMapper
- Identity (PasswordHasher)

## Como usar

1. Configure o `appsettings.json` com sua conexão e chave JWT.
2. Rode o projeto com `dotnet run` ou via Visual Studio.
3. Acesse a documentação via Swagger: `http://localhost:5113/swagger`.

## Endpoints principais

- `POST /api/Auth/register` – Registrar usuário
- `POST /api/Auth/login` – Fazer login e obter token
