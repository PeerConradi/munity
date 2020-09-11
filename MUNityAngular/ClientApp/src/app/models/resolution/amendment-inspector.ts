import { AbstractAmendment } from "./abstract-amendment.model";
import { Resolution } from "./resolution.model";
import { OperativeSection } from "./operative-section.model";
import { OperativeParagraph } from "./operative-paragraph.model";

/**
 * Ein Austausch von Änderungsanträgen bewirkt, dass die Observables nicht mehr funktionieren
 * Deshalb werden alle Änderungsanträge in einem Inspector gesammelt.
 * Dieser Inspector könnte in Zukunft auch die Get_ID etc. funktionalitäten als statische funktionen halten
 * welche ein objekt übergeben bekommen und dann schauen ob diese ID oder iD oder id enthalten...
 * */
export class AmendmentInspector {
  public allAmendments: AbstractAmendment[] = [];


  public static getSectionForAmendment(resolution: Resolution, amendment: AbstractAmendment): OperativeParagraph {
    if (amendment.type === 'delete') {
      return resolution.operativeSection.paragraphs.find(n => n.operativeParagraphId === amendment.targetSectionId);
    } else if (amendment.type === 'change') {
      return resolution.operativeSection.paragraphs.find(n => n.operativeParagraphId === amendment.targetSectionId);
    } else if (amendment.type === 'move') {
      return resolution.operativeSection.paragraphs.find(n => n.operativeParagraphId === amendment.targetSectionId);
    }

    return null;
  }
}
