# Casos de uso — Campeonato Fut7

Documento de apoio à apresentação do sistema. Descreve quem usa o sistema, o que
ele consegue fazer (casos de uso), as regras de negócio envolvidas e um roteiro
passo a passo para a demonstração.

## 1. Ator

**Organizador do campeonato**: pessoa que administra o campeonato de futebol 7.
Cadastra times e jogadores, lança o resultado das partidas e consulta a tabela de
classificação. Usa o sistema pelos endpoints da API (via Swagger ou arquivo `.http`).

## 2. Lista de casos de uso

| ID   | Caso de uso                    | Endpoint                                  | Regras |
|------|--------------------------------|-------------------------------------------|--------|
| UC01 | Cadastrar time                 | `POST /api/times`                         | RN01 |
| UC02 | Listar times                   | `GET /api/times`                          | — |
| UC03 | Buscar time pelo nome          | `GET /api/times/buscar?nome=`             | — |
| UC04 | Cadastrar jogador em um time   | `POST /api/times/{timeId}/jogadores`      | RN02, RN03, RN04 |
| UC05 | Listar jogadores de um time    | `GET /api/times/{timeId}/jogadores`       | — |
| UC06 | Registrar resultado de partida | `POST /api/partidas`                      | RN05, RN06 |
| UC07 | Listar partidas                | `GET /api/partidas`                       | — |
| UC08 | Consultar classificação        | `GET /api/classificacao`                  | RN07, RN08 |
| UC09 | Renomear time                  | `PUT /api/times/{id}`                     | RN01 |
| UC10 | Remover time                   | `DELETE /api/times/{id}`                  | RN09 |
| UC11 | Remover jogador                | `DELETE /api/jogadores/{id}`              | — |
| UC12 | Anular partida                 | `DELETE /api/partidas/{id}`               | RN10 |

## 3. Regras de negócio

| ID   | Regra |
|------|-------|
| RN01 | O nome do time é obrigatório, tem até 50 caracteres e é único (não diferencia maiúsculas de minúsculas). |
| RN02 | Dentro de um mesmo time não pode haver dois jogadores com o mesmo nome. |
| RN03 | O número da camisa vai de 1 a 99 e não pode se repetir dentro do time. |
| RN04 | Cada time pode ter no máximo **12** jogadores. |
| RN05 | Para disputar uma partida, cada time precisa ter pelo menos **7** jogadores (formação do futebol 7). |
| RN06 | Um time não joga contra ele mesmo; os gols de cada lado vão de 0 a 99. |
| RN07 | Pontuação: vitória = **3** pontos, empate = **1**, derrota = **0**. |
| RN08 | Critérios de desempate na classificação: pontos, vitórias, saldo de gols, gols pró e, por último, ordem alfabética. |
| RN09 | Um time que já disputou partidas não pode ser removido (preserva o histórico). |
| RN10 | A classificação é **calculada a partir das partidas** (não é guardada); ao anular uma partida a tabela é recalculada automaticamente. |

## 4. Detalhamento dos principais casos de uso

### UC01 — Cadastrar time
- **Pré-condição:** nenhuma.
- **Fluxo principal:** o organizador envia o nome do time; o sistema cria o time e responde `201 Created` com o `id` gerado.
- **Fluxos alternativos:**
  - nome vazio ou só com espaços → `400 Bad Request`;
  - já existe time com esse nome (RN01) → `409 Conflict`.

### UC04 — Cadastrar jogador em um time
- **Pré-condição:** o time existe.
- **Fluxo principal:** o organizador informa nome e número da camisa; o sistema vincula o jogador ao time e responde `201 Created`.
- **Fluxos alternativos:**
  - time inexistente → `404 Not Found`;
  - número fora de 1–99 ou nome vazio → `400 Bad Request`;
  - nome repetido no time (RN02) ou número repetido (RN03) → `409 Conflict`;
  - time já com 12 jogadores (RN04) → `409 Conflict`.

### UC06 — Registrar resultado de partida
- **Pré-condição:** os dois times existem e cada um tem pelo menos 7 jogadores.
- **Fluxo principal:** o organizador informa time da casa, time visitante e os gols de cada lado; o sistema grava a partida e responde `201 Created` indicando o resultado (vitória de um dos times ou empate).
- **Fluxos alternativos:**
  - mesmo time nos dois lados ou gols fora de 0–99 (RN06) → `400 Bad Request`;
  - algum dos times não existe → `404 Not Found`;
  - algum dos times com menos de 7 jogadores (RN05) → `409 Conflict`.

### UC08 — Consultar classificação
- **Pré-condição:** nenhuma (sem partidas, todos os times aparecem zerados).
- **Fluxo principal:** o sistema percorre as partidas, soma jogos, vitórias, empates, derrotas, gols e pontos de cada time (RN07), ordena pelos critérios da RN08 e devolve a tabela com a posição de cada time.

## 5. Roteiro de demonstração

Pode ser executado pelo **Swagger** (`http://localhost:5080/swagger`) ou pelo arquivo
[`campeonato-fut7.http`](../campeonato-fut7.http), que já traz todas as requisições na ordem.
Comece com o banco vazio (apague o arquivo `campeonato.db` para reiniciar).

| Passo | Ação | Resultado esperado |
|-------|------|--------------------|
| 1 | UC01 — cadastrar **Flamengo** e **Vasco** | `201` para cada um |
| 2 | UC01 — cadastrar **Flamengo** de novo | `409` (RN01) |
| 3 | UC06 — registrar partida Flamengo × Vasco sem jogadores | `409` (RN05: mínimo de 7 jogadores) |
| 4 | UC04 — cadastrar 7 jogadores em cada time | `201` para cada jogador |
| 5 | UC04 — repetir um nome e um número de camisa | `409` (RN02 e RN03) |
| 6 | UC05 / UC02 — listar jogadores e times | listas com 7 jogadores por time |
| 7 | UC03 — buscar times com `nome=fla` | retorna apenas o Flamengo |
| 8 | UC06 — Flamengo 3 × 1 Vasco, Vasco 2 × 2 Flamengo, Vasco 1 × 0 Flamengo | `201` para cada, com o resultado |
| 9 | UC08 — consultar classificação | Flamengo 1º (4 pts, saldo +1) e Vasco 2º (4 pts, saldo −1), desempate por saldo (RN08) |
| 10 | UC12 — anular a última partida e consultar a classificação de novo | Flamengo 4 pts × Vasco 1 pt (RN10) |
| 11 | UC10 — tentar remover o Flamengo | `409` (RN09) |
| 12 | Encerrar a aplicação, subir de novo e repetir o UC08 | os dados continuam lá (persistência com Entity Framework) |

## 6. Onde cada conceito aparece no código (apoio à defesa)

| Conceito | Onde ver |
|----------|----------|
| Classes de modelo e construtores | `Models/Time.cs`, `Models/Jogador.cs`, `Models/Partida.cs` |
| Relacionamento entre objetos | `Time` possui `List<Jogador>`; `Partida` referencia dois `Time` (casa e visitante) |
| `List<T>` e `foreach` | `ClassificacaoController` (monta a tabela), `JogadoresController` (verifica duplicidade no elenco) |
| Regras de negócio | `Models/RegrasCampeonato.cs` e os controllers |
| API (endpoints) | pasta `Controllers/` |
| Entity Framework | `Data/CampeonatoContext.cs` e pasta `Migrations/` |
