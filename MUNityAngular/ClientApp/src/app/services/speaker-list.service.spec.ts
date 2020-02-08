import { TestBed } from '@angular/core/testing';

import { SpeakerListService } from './speaker-list.service';

describe('SpeakerListService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SpeakerListService = TestBed.get(SpeakerListService);
    expect(service).toBeTruthy();
  });
});
