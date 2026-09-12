using System;
using System.Collections.Generic;

class Jogador
{
    public string Nome;
    public int Numero;

    public Jogador(string nome, int numero)
    {
        Nome = nome;
        Numero = numero;
    }
}

class Time
{
    public string Nome;
    public int Pontos;
    public List<Jogador> Jogadores;

    public Time(string nome)
    {
        Nome = nome;
        Pontos = 0;
        Jogadores = new List<Jogador>();
    }
}

class Program
{
    static void Main()
    {
        List<Time> times = new List<Time>();

        int opcao = 1;

        while (opcao != 0)
        {
            Console.WriteLine();
            Console.WriteLine("CAMPEONATO FUT 7");
            Console.WriteLine("1 - Cadastrar time");
            Console.WriteLine("2 - Cadastrar jogador");
            Console.WriteLine("0 - Sair");
            Console.Write("Escolha uma opção: ");

            opcao = int.Parse(Console.ReadLine());

            if (opcao == 1)
            {
                Console.Write("Nome do time: ");
                string nome = Console.ReadLine();

                bool existe = false;

                foreach (Time time in times)
                {
                    if (time.Nome == nome)
                    {
                        existe = true;
                    }
                }

                if (existe == true)
                {
                    Console.WriteLine("Esse time já está cadastrado.");
                }
                else
                {
                    Time time = new Time(nome);
                    times.Add(time);

                    Console.WriteLine("Time cadastrado.");
                }
            }

            else if (opcao == 2)
            {
                Console.Write("Nome do time: ");
                string nomeTime = Console.ReadLine();

                bool encontrouTime = false;

                foreach (Time time in times)
                {
                    if (time.Nome == nomeTime)
                    {
                        encontrouTime = true;

                        Console.Write("Nome do jogador: ");
                        string nomeJogador = Console.ReadLine();

                        Console.Write("Número da camisa: ");
                        int numero = int.Parse(Console.ReadLine());

                        bool jogadorExiste = false;
                        bool numeroExiste = false;

                        foreach (Jogador jogador in time.Jogadores)
                        {
                            if (jogador.Nome == nomeJogador)
                            {
                                jogadorExiste = true;
                            }

                            if (jogador.Numero == numero)
                            {
                                numeroExiste = true;
                            }
                        }

                        if (jogadorExiste == true)
                        {
                            Console.WriteLine("Esse jogador já está cadastrado.");
                        }
                        else if (numeroExiste == true)
                        {
                            Console.WriteLine("Já existe um jogador com esse número.");
                        }
                        else
                        {
                            Jogador jogador =
                                new Jogador(nomeJogador, numero);

                            time.Jogadores.Add(jogador);

                            Console.WriteLine("Jogador cadastrado.");
                        }
                    }
                }

                if (encontrouTime == false)
                {
                    Console.WriteLine("Time não encontrado.");
                }
            }

            else if (opcao == 0)
            {
                Console.WriteLine("Programa encerrado.");
            }
        }
    }
}