import { TestBed } from '@angular/core/testing';

import { SimulatorService } from './simulator.service';

describe('SimulatorService', () => {
  let service: SimulatorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SimulatorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
