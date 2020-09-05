import { TestBed } from '@angular/core/testing';

import { SimulationService } from './simulator.service';

describe('SimulatorService', () => {
  let httpClientSpy: { get: jasmine.Spy };
  let service: SimulationService;

  beforeEach(() => {
    //TestBed.configureTestingModule({});
    httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);
    service = new SimulationService(httpClientSpy as any, '');
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
