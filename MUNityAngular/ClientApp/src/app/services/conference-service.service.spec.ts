import { TestBed } from '@angular/core/testing';

import { ConferenceServiceService } from './conference-service.service';

describe('ConferenceServiceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ConferenceServiceService = TestBed.get(ConferenceServiceService);
    expect(service).toBeTruthy();
  });
});
