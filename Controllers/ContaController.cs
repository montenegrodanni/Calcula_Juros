using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RecolimentoAtraso.Database.Context;
using RecolimentoAtraso.Model.Entity;
using RecolimentoAtraso.Model.Metodos;

namespace RecolimentoAtraso.Controllers;

[ApiController]
[Route("[controller]")]
public class ContaController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly Metodos _metodos;

    public ContaController(ApplicationDbContext context, Metodos metodos)
    {
        _context = context;
        _metodos = metodos;

    }

    [HttpGet("calcular")]
    public IActionResult CalcularJuros(DateTime dataVencimento, DateTime dataPagamento, float valorOriginalICMS)
    {
        float valorJuros = _metodos.CalculaValorJuros(valorOriginalICMS, dataVencimento);
        float valorMulta = _metodos.CalculaValorMulta(valorOriginalICMS, dataVencimento, dataPagamento);
        float valorDivida = _metodos.CalculaValorTotalMultaJuros(valorOriginalICMS, dataVencimento, dataPagamento);
        int dias = _metodos.CalculaDiasTotaisMulta(dataVencimento, dataPagamento);
        float selicAcumulada = _metodos.CalculaSelicAcumulada(dataVencimento, dataPagamento);
        float selicAcumuladaValor = _metodos.CalculaSelicAcumuladaValor(valorOriginalICMS, dataVencimento, dataPagamento);

        var resultado = new Conta
        {
            ValorOriginal = valorOriginalICMS,
            DataVencimento = dataVencimento,
            DataPagamento = dataPagamento,
            DataSimulacao = DateTime.Now,
            ValorDivida = valorDivida,
            Juros = valorJuros,
            DiasAtraso = dias,
            SelicAcumulado = selicAcumulada,
            SelicAcumuladoValor = selicAcumuladaValor,
            Multa = valorMulta,
        };

        return Ok(resultado);
    }

    [HttpGet("testeTeste")]
    public IActionResult Teste(DateTime dataVencimento, DateTime dataPagamento)
    {
        float valorJuros = _metodos.CalculaSelicAcumulada(dataVencimento, dataPagamento);

        return Ok(valorJuros);
    }

    [HttpPost("TabelaJurosCompleto")]
    public IActionResult CadastraJuros([FromBody] List<TabelaJuros> tabelaJuros)
    {

        foreach (var tj in tabelaJuros)
        {
            var registro = _context.tabelaJuros.FirstOrDefault(j => j.Ano == tj.Ano && j.Mes == tj.Mes);

            if (registro != null)
            {
                return BadRequest($"Registro já existe para o ano {tj.Ano} e mês {tj.Mes}.");
            }

            _context.Add(tj);
        }

        _context.SaveChanges();

        return Ok(tabelaJuros);
    }

    [HttpGet("TabelaJurosCompleto")]
    public IActionResult RetornaTabelaJuros()
    {
        var tabelaJuros = _context.tabelaJuros.ToList();

        return Ok(tabelaJuros);
    }


    [HttpPost("CadastraJurosFromFile")]
    public async Task<IActionResult> CadastraJurosFromFile()
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Database/Others", "postInit.txt");

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("Arquivo não encontrado.");
        }

        string jsonContent = await System.IO.File.ReadAllTextAsync(filePath);
        List<TabelaJuros> tabelaJuros;

        try
        {
            tabelaJuros = JsonConvert.DeserializeObject<List<TabelaJuros>>(jsonContent);
        }
        catch (JsonException)
        {
            return BadRequest("O conteúdo do arquivo não é um JSON válido.");
        }

        // Deleta todos os registros existentes na tabela TabelaJuros
        var allRecords = _context.tabelaJuros.ToList();
        _context.tabelaJuros.RemoveRange(allRecords);
        await _context.SaveChangesAsync();

        return CadastraJuros(tabelaJuros);
    }
}
