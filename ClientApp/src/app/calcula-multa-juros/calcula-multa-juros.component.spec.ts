import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CalculaMultaJurosComponent } from './calcula-multa-juros.component';

describe('CalculaMultaJurosComponent', () => {
  let component: CalculaMultaJurosComponent;
  let fixture: ComponentFixture<CalculaMultaJurosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CalculaMultaJurosComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CalculaMultaJurosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
