import { Component, OnInit } from '@angular/core';
import { ContaService } from 'src/services/conta.service';

@Component({
  selector: 'app-calcula-multa-juros',
  templateUrl: './calcula-multa-juros.component.html',
  styleUrls: ['./calcula-multa-juros.component.scss']
})
export class CalculaMultaJurosComponent implements OnInit {
  dataVencimento: string = '';
  dataPagamento: string = '';
  valorOriginalICMS: number = 0;
  resultado: any;
  errorMessage: string = '';

  showCalculo: boolean = false;

  constructor(private contaService: ContaService) { }

  ngOnInit(): void {
    this.showCalculo = false;
  }

  show()
  {
    this.showCalculo = true;
  }
  
  hide()
  {
    this.showCalculo = false;
  }

  limpar()
  {
    this.dataVencimento = '';
    this.dataPagamento = '';
    this.valorOriginalICMS = 0;
    this.hide();
  }

  calcular() {
    if (!this.dataVencimento || !this.dataPagamento || !this.valorOriginalICMS) {
      alert('Data de Vencimento, Pagamento e Valor são obrigatórios.');
      this.hide();
      return;
    }

    this.contaService.calcularJuros(this.dataVencimento, this.dataPagamento, this.valorOriginalICMS)
    .subscribe(
      data => {
        this.resultado = data;
      },
      error => {
        this.errorMessage = 'Ocorreu um erro ao calcular os juros, verifique se o ano e o mês estão cadastrados.';
      }
    );
    this.show();
  }

}
