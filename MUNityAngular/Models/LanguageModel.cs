using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNityAngular.Models
{
    public class LanguageModel
    {
        #region Spekaerlist
        public string SPEAKERLIST { get; set; } = "Redeliste";

        public string SPEAKERLIST_QUESTIONS { get; set; } = "Fragen und Kurzbemerkungen";

        public string SPEAKERLIST_CLOSED { get; set; } = "Liste geschlossen";

        public string SPEAKERLIST_OPENED { get; set; } = "Liste offen";

        public string SPEAKER_TIME { get; set; } = "Redezeit";

        public string QUESTION_TIME { get; set; } = "Fragen und Kurzbermerkungen";

        public string CURRENT { get; set; } = "Aktuell";

        public string CURRENT_SPEAKER { get; set; } = "Aktueller Redner";

        public string CURRENT_QUESTIONS { get; set; } = "Aktuelle Frage";

        public string TIME { get; set; } = "Zeit";

        public string NEXT { get; set; } = "Nächster";

        public string START_STOP { get; set; } = "Go/Pause";

        public string UP { get; set; } = "UP";

        public string DOWN { get; set; } = "DOWN";

        public string ANSWER { get; set; } = "Antworten";

        public string GUEST_SPEAKER { get; set; } = "Gastredner";

        public string SECRETARY_GENERAL { get; set; } = "Generalsekretär";

        public string CLOSE_SPEAKERLIST { get; set; } = "Liste schließen";

        public string CLOSE_QUESTIONS { get; set; } = "Fragen schließen";

        public string REMOVE_BANNER { get; set; } = "Banner entfernen";

        public string TIME_BANNER { get; set; } = "Zeitbanner";

        public string CLEAR_SPEAKERLIST_HEAD { get; set; } = "R. Kopf leeren";

        public string CLEAR_QUESTION_HEAD { get; set; } = "FuK Kopf leeren";

        public string SPEAKERLIST_ADD { get; set; } = "Hinzufügen";
        #endregion

        #region ResolutionEditor
        public string TEXT_OF_NEW_PARAGRAPH { get; set; } = "Text des neuen Absatzes";

        public string POSITION { get; set; } = "Position";

        public string AS_SUBPOINT { get; set; } = "Als Unterpunkt";

        public string NEW_TEXT { get; set; } = "Neuer Text";

        public string NEW_POSITION { get; set; } = "Neue Position";

        public string AMENDMENTS { get; set; } = "Änderungsanträge";

        public string ADD_AMENDMENT { get; set; } = "Hinzufügen";
        #endregion

        #region Committee
        public string OVERVIEW { get; set; } = "Übersicht";

        public string DETECT_PRESENCE { get; set; } = "Anwesenheit feststellen";

        public string CREATE_VOTING { get; set; } = "Abstimmung durchführen";

        public string MANAGE_SPEAKERLIST { get; set; } = "Redeliste führen";

        public string OPEN_PRESENTATION { get; set; } = "Präsentation öffnen";
        #endregion

        #region Menu
        public string DASHBOARD { get; set; } = "Start";

        public string RESOLUTIONS { get; set; } = "Resolutionen";

        public string COMMITTEES { get; set; } = "Gremien";

        public string DELEGATIONS { get; set; } = "Delegationen";

        public string OPTIONS { get; set; } = "Einstellungen";
        #endregion

        #region Presence
        public string RECHECK_PRESENCE { get; set; } = "Neu Feststellen";

        public string NOTED { get; set; } = "Festgestellt";

        public string OUT_OF { get; set; } = "von";

        public string PRESENT { get; set; } = "Anwesend";

        public string DETERMINE { get; set; } = "Festzustellen";

        public string ABSENT { get; set; } = "Abwesend";

        public string MAJORITY { get; set; } = "Mehrheit";

        public string VOTES { get; set; } = "Stimmen";

        public string TOTAL_PRESENT { get; set; } = "Insgesamt Anwesend";
        #endregion

        #region Voting
        public string VOTING { get; set; } = "Abstimmung";

        public string ACCEPTENCE { get; set; } = "Dafür";

        public string AGAINST { get; set; } = "Dagegen";

        public string ABSTENTION { get; set; } = "Enthaltungen";

        public string DIRECT_VOTE { get; set; } = "Mündliche Abstimmung";

        public string NOT_VOTED_YET { get; set; } = "Noch nicht abgestimmt";

        public string PRESS_KEY { get; set; } = "Drücke";

        public string VOTING_RESULT { get; set; } = "Abstimmungsergebnis";
        #endregion
    }
}
