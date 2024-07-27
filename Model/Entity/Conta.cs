namespace RecolimentoAtraso.Model.Entity;

public class Conta
{
    public float ValorOriginal { get; set; }
    public DateTime DataVencimento { get; set; }
    public DateTime DataPagamento { get; set; }
    public DateTime DataSimulacao { get; set; } = DateTime.Now;
    public float ValorDivida { get; set; }
    public float Multa { get; set; }
    public float Juros { get; set; }
    public int DiasAtraso { get; set; }
    public float SelicAcumulado { get; set; }
    public float SelicAcumuladoValor { get; set; }
}