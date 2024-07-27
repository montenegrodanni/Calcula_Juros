namespace RecolimentoAtraso.Model.Entity;

public class TabelaJuros
{
    public int Id { get; set; }
    public int Ano { get; set; }
    public int Mes { get; set; }
    public float ValorJuros { get; set; }

    public TabelaJuros() { }
}
