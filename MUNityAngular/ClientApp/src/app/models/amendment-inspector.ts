import { AbstractAmendment } from "./abstract-amendment.model";
import { Resolution } from "./resolution.model";
import { OperativeSection } from "./operative-section.model";

/**
 * Ein Austausch von Änderungsanträgen bewirkt, dass die Observables nicht mehr funktionieren
 * Deshalb werden alle Änderungsanträge in einem Inspector gesammelt.
 * Dieser Inspector könnte in Zukunft auch die Get_ID etc. funktionalitäten als statische funktionen halten
 * welche ein objekt übergeben bekommen und dann schauen ob diese ID oder iD oder id enthalten...
 * */
export class AmendmentInspector {
  public allAmendments: AbstractAmendment[] = [];


  public static getSectionForAmendment(resolution: Resolution, amendment: AbstractAmendment): OperativeSection {
    if (amendment.Type === 'delete') {
      return resolution.OperativeSections.find(n => n.ID === amendment.TargetSectionID);
    } else if (amendment.Type === 'change') {
      return resolution.OperativeSections.find(n => n.ID === amendment.TargetSectionID);
    } else if (amendment.Type === 'move') {
      return resolution.OperativeSections.find(n => n.ID === amendment.TargetSectionID);
    }
      
    return null;
  }
}
