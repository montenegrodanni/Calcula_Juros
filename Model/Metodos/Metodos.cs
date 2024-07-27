using RecolimentoAtraso.Database.Context;
using RecolimentoAtraso.Model.Entity;

namespace RecolimentoAtraso.Model.Metodos;

public class Metodos
{
    private readonly ApplicationDbContext _context;

    public Metodos(ApplicationDbContext context)
    {
        _context = context;
    }

    public int CalculaDiasTotaisMulta(DateTime dataVencimento, DateTime dataPagamento)
    {
        Conta conta = new Conta();

        conta.DataVencimento = dataVencimento;
        conta.DataPagamento = dataPagamento;

        TimeSpan diferenca = conta.DataPagamento - conta.DataVencimento;

        int dias = diferenca.Days;

        return dias;
    }

    public float CalculaValorMulta(float valor, DateTime dataVencimento, DateTime dataPagamento)
    {
        Conta conta = new Conta();

        float multaAcumulada = 0f;
        float multaDiaria = 0.0033f;
        float contaMultaDiaria = 0f;
        int diasAtraso = CalculaDiasTotaisMulta(dataVencimento, dataPagamento);

        for (int dias = 1; dias <= diasAtraso; dias++)
        {
            if (contaMultaDiaria >= 0.20f)
            {
                multaAcumulada = valor * 0.20f;
                break;
            }
            else
            {
                // Retorna valor da multa
                multaAcumulada += valor * multaDiaria;

                // Soma 0.33
                contaMultaDiaria += multaDiaria;
            }
        }

        // Porcentagem de multa
        conta.Multa = multaAcumulada;

        return (float)Math.Round(multaAcumulada, 2);
    }


    public float CalculaValorJuros(float valor, DateTime dataVencimento)
    {
        HashSet<Tuple<int, int>> anoMes = [Tuple.Create(dataVencimento.Year, dataVencimento.Month)];

        float valorJuros = 0.0f;

        var tabelaJuros = _context.tabelaJuros.FirstOrDefault(j => j.Ano == dataVencimento.Year && j.Mes == dataVencimento.Month);

        if (tabelaJuros == null)
        {
            throw new Exception($"Ano {anoMes} e mês {anoMes} não foram cadastrados na tabela de juros.");
        }

        valorJuros += valor * (tabelaJuros.ValorJuros / 100);

        Conta conta = new Conta { Juros = valorJuros };

        return valorJuros;
    }

    public float CalculaValorTotalMultaJuros(float valor, DateTime dataVencimento, DateTime dataPagamento)
    {
        Conta conta = new Conta();

        float valorJuros = CalculaValorMulta(valor, dataVencimento, dataPagamento);
        float valorMulta = CalculaSelicAcumuladaValor(valor, dataVencimento, dataPagamento);

        float valorDivida = valorJuros + valorMulta + valor;

        conta.ValorDivida = valorDivida;

        return (float)Math.Round(valorDivida, 2);
    }

    public float CalculaSelicAcumulada(DateTime dataVencimento, DateTime dataPagamento)
    {
        Conta conta = new Conta();

        var totalSelicAcumulado = _context.tabelaJuros
            .Where(c =>
                (c.Ano == dataVencimento.Year && c.Mes >= dataVencimento.Month) &&
                (c.Ano == dataPagamento.Year && c.Mes <= dataPagamento.Month))
            .Sum(c => c.ValorJuros);

        totalSelicAcumulado = (float)Math.Round(totalSelicAcumulado, 2);

        conta.SelicAcumulado = totalSelicAcumulado;

        return totalSelicAcumulado;
    }

    public float CalculaSelicAcumuladaValor(float valor, DateTime dataVencimento, DateTime dataPagamento)
    {
        Conta conta = new Conta();

        var calculaSelicAcumulada = CalculaSelicAcumulada(dataVencimento, dataPagamento);

        var valorSelicAcumulada = (valor * calculaSelicAcumulada) / 100;

        valorSelicAcumulada = (float)Math.Round(valorSelicAcumulada, 2);

        conta.SelicAcumuladoValor = valorSelicAcumulada;

        return valorSelicAcumulada;
    }
}
