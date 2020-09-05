import { TestBed } from '@angular/core/testing';

import { SpeakerListService } from './speaker-list.service';

describe('SpeakerListService', () => {
  let httpClientSpy: { get: jasmine.Spy };
  let service: SpeakerListService;

  beforeEach(() => {
    httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);
    service = new SpeakerListService(httpClientSpy as any, '', null);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
