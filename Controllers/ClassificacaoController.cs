using CampeonatoFut7.Data;
using CampeonatoFut7.Dtos;
using CampeonatoFut7.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampeonatoFut7.Controllers;

[ApiController]
[Route("api/classificacao")]
public class ClassificacaoController : ControllerBase
{
    private readonly CampeonatoContext _context;

    public ClassificacaoController(CampeonatoContext context)
    {
        _context = context;
    }

    // UC08 - Consultar a tabela de classificação do campeonato
    // Critérios de desempate: pontos, vitórias, saldo de gols, gols pró e nome.
    [HttpGet]
    public async Task<ActionResult<List<ClassificacaoItem>>> Obter()
    {
        List<Time> times = await _context.Times.AsNoTracking().ToListAsync();
        List<Partida> partidas = await _context.Partidas.AsNoTracking().ToListAsync();

        List<ClassificacaoItem> tabela = new List<ClassificacaoItem>();

        foreach (Time time in times)
        {
            ClassificacaoItem item = new ClassificacaoItem
            {
                TimeId = time.Id,
                Time = time.Nome
            };

            foreach (Partida partida in partidas)
            {
                if (partida.TimeCasaId == time.Id)
                {
                    RegistrarJogo(item, partida.GolsCasa, partida.GolsVisitante);
                }
                else if (partida.TimeVisitanteId == time.Id)
                {
                    RegistrarJogo(item, partida.GolsVisitante, partida.GolsCasa);
                }
            }

            tabela.Add(item);
        }

        List<ClassificacaoItem> ordenada = tabela
            .OrderByDescending(i => i.Pontos)
            .ThenByDescending(i => i.Vitorias)
            .ThenByDescending(i => i.SaldoGols)
            .ThenByDescending(i => i.GolsPro)
            .ThenBy(i => i.Time)
            .ToList();

        for (int i = 0; i < ordenada.Count; i++)
        {
            ordenada[i].Posicao = i + 1;
        }

        return ordenada;
    }

    private static void RegistrarJogo(ClassificacaoItem item, int golsPro, int golsContra)
    {
        item.Jogos++;
        item.GolsPro += golsPro;
        item.GolsContra += golsContra;

        if (golsPro > golsContra)
        {
            item.Vitorias++;
            item.Pontos += RegrasCampeonato.PontosVitoria;
        }
        else if (golsPro == golsContra)
        {
            item.Empates++;
            item.Pontos += RegrasCampeonato.PontosEmpate;
        }
        else
        {
            item.Derrotas++;
            item.Pontos += RegrasCampeonato.PontosDerrota;
        }
    }
}
