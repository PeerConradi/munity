import { AbstractAmendment } from "./abstract-amendment.model";

/**
 * Ein Austausch von Änderungsanträgen bewirkt, dass die Observables nicht mehr funktionieren
 * Deshalb werden alle Änderungsanträge in einem Inspector gesammelt.
 * Dieser Inspector könnte in Zukunft auch die Get_ID etc. funktionalitäten als statische funktionen halten
 * welche ein objekt übergeben bekommen und dann schauen ob diese ID oder iD oder id enthalten...
 * */
export class AmendmentInspector {
  public allAmendments: AbstractAmendment[] = [];


}
