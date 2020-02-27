import { Component, OnInit } from '@angular/core';
import { Resolution } from 'src/app/models/resolution.model';
import { Preamble } from 'src/app/models/preamble.model';
import { OperativeSection } from 'src/app/models/operative-section.model';
import { PreambleParagraph } from 'src/app/models/preamble-paragraph.model';

@Component({
  selector: 'app-all-components',
  templateUrl: './all-components.component.html',
  styleUrls: ['./all-components.component.css']
})
export class AllComponentsComponent implements OnInit {

  reso: Resolution;

  constructor() {
    const res = new Resolution()
    res.ID = 'test';
    res.Name = 'test';
    res.CommitteeName = 'Gremium';
    res.SubmitterName = 'Delegationsname';
    res.SupporterNames.push('Unterstützer 1');
    res.SupporterNames.push('Unterstützer 2');

    res.OperativeSections = [];
    res.Preamble = new Preamble();
    res.Preamble.Paragraphs = [];
    res.Topic = 'Thema';
    const pparagraphOne = new PreambleParagraph();
    pparagraphOne.ID = 'pOne';
    pparagraphOne.Text = 'Hallo Welt';
    res.Preamble.Paragraphs.push(pparagraphOne);

    const oaOne = new OperativeSection();
    oaOne.Path = '1';
    oaOne.ID = 'oaOne';
    oaOne.Text = 'Paragraph 1';
    oaOne.ChangeAmendmentCount = 2;
    oaOne.DeleteAmendmentCount = 1;
    res.OperativeSections.push(oaOne);

    const oaTwo = new OperativeSection();
    oaTwo.Path = '2';
    oaTwo.ID = 'oaTwo';
    oaTwo.Text = 'Dieser Absatz hat deutlich mehr Text um die Auswahl für Änderungsanträge einmal zu testen.'
    res.OperativeSections.push(oaTwo);

    this.reso = res;
  }

  ngOnInit() {

  }

}
