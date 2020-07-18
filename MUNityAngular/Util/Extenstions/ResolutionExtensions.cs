using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Resolution;

namespace MUNityAngular.Util.Extenstions
{
    public static class ResolutionExtensions
    {
        public static string ToJson(this ResolutionModel resolution)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(resolution, Newtonsoft.Json.Formatting.Indented);
            return json;
        }

        public static string AsNewtonsoftJson(this object o)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(o);
            return json;
        }

        //Theoretisch müsste das auch über ein Template gehen, aber das ganze wäre dann nicht
        //Typesafe weil ich generisch <> angeben müsste in was das ganze konvertiert werden muss
        //und ich will ja gar nicht das aus einer Liste von ChangeAmendments eine Liste von DeleteAmendments wird!
        public static List<Hubs.HubObjects.HUBChangeAmendment> ToHubAmendments(this IEnumerable<ChangeAmendmentModel> a)
        {
            var list = new List<Hubs.HubObjects.HUBChangeAmendment>();
            foreach(var element in a)
            {
                list.Add(new Hubs.HubObjects.HUBChangeAmendment(element));
            }
            return list;
        }

        public static List<Hubs.HubObjects.HUBDeleteAmendment> ToHubAmendments(this IEnumerable<DeleteAmendmentModel> a)
        {
            var list = new List<Hubs.HubObjects.HUBDeleteAmendment>();
            foreach(var element in a)
            {
                list.Add(new Hubs.HubObjects.HUBDeleteAmendment(element));
            }
            return list;
        }

        public static List<Hubs.HubObjects.HUBMoveAmendment> ToHubAmendments(this IEnumerable<MoveAmendmentModel> a)
        {
            var list = new List<Hubs.HubObjects.HUBMoveAmendment>();
            foreach(var element in a)
            {
                list.Add(new Hubs.HubObjects.HUBMoveAmendment(element));
            }
            return list;
        }

        public static List<Hubs.HubObjects.HUBAddAmendment> ToHubAmendments(this IEnumerable<AddAmendmentModel> a)
        {
            var list = new List<Hubs.HubObjects.HUBAddAmendment>();
            foreach(var element in a)
            {
                list.Add(new Hubs.HubObjects.HUBAddAmendment(element));
            }
            return list;
        }

        public static List<Hubs.HubObjects.HUBPreambleParagraph> ToHubParagraphs(this IEnumerable<PreambleParagraphModel> a)
        {
            var list = new List<Hubs.HubObjects.HUBPreambleParagraph>();
            foreach(var element in a)
            {
                list.Add(new Hubs.HubObjects.HUBPreambleParagraph(element));
            }
            return list;
        }

        public static List<Hubs.HubObjects.HUBOperativeParagraph> ToHubParagraphs(this IEnumerable<OperativeParagraphModel> a)
        {
            var list = new List<Hubs.HubObjects.HUBOperativeParagraph>();
            foreach(var element in a)
            {
                list.Add(new Hubs.HubObjects.HUBOperativeParagraph(element));
            }
            return list;
        }

    }
}
