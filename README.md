# Campeonato Fut7

## Descrição

Sistema em console para gerenciar um campeonato de futebol 7 (fut7). O sistema
permite cadastrar times e seus jogadores, consultar as informações cadastradas
e registrar o resultado das partidas, atualizando a pontuação de cada time na
tabela do campeonato.

## Integrantes

- Gabriel Vilar
- Victor Hugo

## Funcionalidades

- **Cadastrar time**: cria um novo time, impedindo o cadastro de times com
  nomes duplicados.
- **Cadastrar jogador**: vincula um jogador a um time existente, impedindo
  nomes de jogador ou números de camisa duplicados dentro do mesmo time.
- **Listar times**: exibe todos os times cadastrados com sua pontuação atual.
- **Listar jogadores**: exibe os jogadores de um time específico.
- **Buscar time**: mostra o nome e a pontuação de um time pesquisado pelo nome.
- **Registrar vitória** (regra de negócio): soma 3 pontos ao time vencedor
  informado, atualizando sua posição na tabela do campeonato.

## Tecnologias

- C#
- .NET
- Git/GitHub

## Como executar

1. Clonar o repositório.
2. Abrir o projeto no Visual Studio ou VS Code.
3. Executar o projeto (`dotnet run`).
