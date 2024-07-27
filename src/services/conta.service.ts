import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

interface Conta {
  valorOriginal: number;
  dataVencimento: string;
  dataPagamento: string;
  dataSimulacao: string;
  valorDivida: number;
  dias: number;
  selicAcumulado: number;
  selicAcumuladoValor: number;
  multa: number;
}

@Injectable({
  providedIn: 'root',
})
export class ContaService {
  private apiUrl = 'https://localhost:7072/conta/calcular';

  constructor(private http: HttpClient) {}

  calcularJuros(dataVencimento: string, dataPagamento: string, valorOriginalICMS: number): Observable<any> {
    let params = new HttpParams()
      .set('valorOriginalICMS', valorOriginalICMS.toString())
      .set('dataVencimento', dataVencimento)
      .set('dataPagamento', dataPagamento);

    return this.http.get<any>(this.apiUrl, { params: params });
  }
}
