import { TestBed } from '@angular/core/testing';
import { OperativeParagraph } from '../models/resolution/operative-paragraph.model';

import { ResolutionService } from './resolution.service';
import { UserService } from "./user.service";

describe('ResolutionService', () => {
  let httpClientSpy: { get: jasmine.Spy };
  let service: ResolutionService;
  let userService: UserService;

  beforeEach(() => {
    httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);
    userService = new UserService(httpClientSpy as any, '');
    service = new ResolutionService(httpClientSpy as any, userService, null, '');
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('Should get the first level path of operative sections', () => {
    let resolution = service.getTestResolution();
    let paragraph = resolution.operativeSection.paragraphs[0];
    let result = service.getPathOfOperativeParagraph(paragraph, resolution);
    expect(result).toBe('1');
  });

  it('Should be able to resolve a operative paragraph path inside a child', () => {
    let resolution = service.getTestResolution();
    let parent = resolution.operativeSection.paragraphs[0];
    let paragraph = parent.children[0];
    let path: OperativeParagraph[] = [];
    let result = service.resolveOperativeParagraphPath(paragraph, parent, path);
    expect(path.length).toBe(2);
  });

  it('should not find the path of a operative paragraph that is not inside the resolution', () => {
    let resolution = service.getTestResolution();
    let parent = resolution.operativeSection.paragraphs[0];
    let paragraph = parent.children[0];
    let path: OperativeParagraph[] = [];
    let fakeParagraph = new OperativeParagraph();
    fakeParagraph.operativeParagraphId = 'fake';

    let result = service.resolveOperativeParagraphPath(fakeParagraph, parent, path);
    expect(path.length).toBe(0);
  });

  it('should find a paragraph if there are multiple sub paragraphs', () => {
    let resolution = service.getTestResolution();
    let parent = resolution.operativeSection.paragraphs[0];
    let paragraph = parent.children[0];
    let path: OperativeParagraph[] = [];
    let newParagraph = new OperativeParagraph();
    newParagraph.operativeParagraphId = 'fake';
    parent.children.push(newParagraph);
    let result = service.resolveOperativeParagraphPath(newParagraph, parent, path);
    expect(path.length).toBe(2);
  });

  it('should create a path for a sub operative paragraph', () => {
    let resolution = service.getTestResolution();
    let parent = resolution.operativeSection.paragraphs[0];
    let paragraph = parent.children[0];
    let result = service.getPathOfOperativeParagraph(paragraph, resolution);
    expect(result).toBe('1.1');
  });

  it('shoudl be able to find path for sub 1.2 paragraph', () => {
    let resolution = service.getTestResolution();
    let parent = resolution.operativeSection.paragraphs[0];
    let paragraph = parent.children[0];
    let path: OperativeParagraph[] = [];
    let newParagraph = new OperativeParagraph();
    newParagraph.operativeParagraphId = 'fake';
    parent.children.push(newParagraph);
    let result = service.getPathOfOperativeParagraph(newParagraph, resolution);
    expect(result).toBe('1.2');
  });
});
