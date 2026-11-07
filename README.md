# Campeonato Fut7

## Descrição

API para gerenciar um campeonato de futebol 7 (fut7). O organizador cadastra
times e seus jogadores, registra o resultado das partidas e consulta a tabela de
classificação, que é calculada automaticamente a partir dos resultados.

O projeto começou como um sistema de console (etapa A2, ainda disponível no
histórico do Git) e nesta etapa foi expandido para:

1. **Interação por APIs**: endpoints REST (ASP.NET Core) para todas as operações;
2. **Persistência com Entity Framework**: dados gravados em banco SQLite via EF Core com migrations;
3. **Apresentação com casos de uso**: documentados em [`docs/CASOS_DE_USO.md`](docs/CASOS_DE_USO.md).

## Integrantes

- Gabriel Vilar
- Victor Hugo

## Funcionalidades

- Cadastrar, listar, buscar, renomear e remover times.
- Cadastrar, listar e remover jogadores de um time.
- Registrar o resultado de partidas, listar e anular partidas.
- Consultar a classificação do campeonato.

**Regras de negócio** (detalhadas no documento de casos de uso):

- nome de time único; nome e número de camisa únicos dentro do time;
- máximo de 12 jogadores por time;
- mínimo de 7 jogadores para o time disputar uma partida;
- pontuação 3 / 1 / 0 (vitória / empate / derrota) com critérios de desempate;
- time que já jogou não pode ser removido; ao anular uma partida a classificação é recalculada.

## Endpoints

| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | `/api/times` | Cadastrar time |
| `GET` | `/api/times` | Listar times |
| `GET` | `/api/times/buscar?nome=` | Buscar times pelo nome |
| `GET` | `/api/times/{id}` | Detalhar time com seus jogadores |
| `PUT` | `/api/times/{id}` | Renomear time |
| `DELETE` | `/api/times/{id}` | Remover time |
| `POST` | `/api/times/{timeId}/jogadores` | Cadastrar jogador |
| `GET` | `/api/times/{timeId}/jogadores` | Listar jogadores do time |
| `DELETE` | `/api/jogadores/{id}` | Remover jogador |
| `POST` | `/api/partidas` | Registrar resultado de partida |
| `GET` | `/api/partidas` | Listar partidas |
| `GET` | `/api/partidas/{id}` | Detalhar partida |
| `DELETE` | `/api/partidas/{id}` | Anular partida |
| `GET` | `/api/classificacao` | Consultar classificação |

Respostas de erro seguem o padrão: `400` (dados inválidos), `404` (não encontrado)
e `409` (conflito com regra de negócio).

## Tecnologias

- C#
- .NET 8 / ASP.NET Core Web API
- Entity Framework Core 8 (SQLite) com migrations
- Swagger (Swashbuckle) para testar os endpoints
- Git/GitHub

## Estrutura do projeto

```
Controllers/   endpoints da API (Times, Jogadores, Partidas, Classificacao)
Models/        classes de domínio (Time, Jogador, Partida) e RegrasCampeonato
Dtos/          objetos de entrada e saída da API
Data/          CampeonatoContext (Entity Framework)
Migrations/    migrations do banco de dados
docs/          casos de uso e roteiro de demonstração
```

## Como executar

Pré-requisito: [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).

1. Clonar o repositório.
2. Abrir a pasta no Visual Studio ou VS Code.
3. Executar o projeto:

   ```bash
   dotnet run
   ```

4. Abrir o Swagger em <http://localhost:5080/swagger> para testar os endpoints.

O banco de dados (`campeonato.db`) é criado automaticamente na primeira execução
pelas migrations. Para começar do zero, basta apagar esse arquivo.

Para demonstrar o sistema, use o roteiro em [`docs/CASOS_DE_USO.md`](docs/CASOS_DE_USO.md)
com o arquivo [`campeonato-fut7.http`](campeonato-fut7.http) (Visual Studio 2022 ou
extensão REST Client do VS Code).

### Alterando o modelo de dados

Após mudar as classes de `Models/`, gerar uma nova migration (requer a ferramenta
`dotnet-ef`: `dotnet tool install --global dotnet-ef`):

```bash
dotnet ef migrations add NomeDaMigration
```

A migration é aplicada automaticamente ao iniciar a aplicação.
