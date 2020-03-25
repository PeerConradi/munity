using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.DataHandlers.Database;
using MUNityAngular.DataHandlers.EntityFramework;
using MySql.Data.MySqlClient;
using MUNityAngular.DataHandlers.EntityFramework.Models;

namespace MUNityAngular.Services
{
    public class InstallationService
    {
        private MunityContext _context;

        public List<string> InstallationLog { get; private set; }

        public InstallationService(MunityContext context)
        {
            _context = context;
            InstallationLog = new List<string>();
        }

        public bool IsInstalled { get; private set; }

        public Version InstalledVersion
        {
            get => throw new NotImplementedException();
        }

        public void Install()
        {
            InstallationLog.Add("Start Installation");
            CreateDefaultDelegations();
            InstallationLog.Add("Added Default Delegations!");
        }

        public void CreateDefaultDelegations()
        {
            _context.Delegations.Add(new Delegation() { DelegationId = "default_aegypten", Name = "Ägypten", FullName = "ArabischeRepublikÄgypten", Abbreviation = "Ägypten", Type = "COUNTRY", IconName = "eg" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_aequatorialguinea", Name = "Äquatorialguinea", FullName = "RepublikÄquatorialguinea", Abbreviation = "Äquatorialguinea", Type = "COUNTRY", IconName = "gq" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_aethiopien", Name = "Äthiopien", FullName = "DemokratischeBundesrepublikÄthiopien", Abbreviation = "Äthiopien", Type = "COUNTRY", IconName = "et" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_afghanistan", Name = "Afghanistan", FullName = "IslamischeRepublikAfghanistan", Abbreviation = "Afghanistan", Type = "COUNTRY", IconName = "af" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_albanien", Name = "Albanien", FullName = "Albanien", Abbreviation = "Albanien", Type = "COUNTRY", IconName = "al" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_algerien", Name = "Algerien", FullName = "Algerien", Abbreviation = "Algerien", Type = "COUNTRY", IconName = "dz" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_andorra", Name = "Andorra", FullName = "Andorra", Abbreviation = "Andorra", Type = "COUNTRY", IconName = "ad" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_angola", Name = "Angola", FullName = "Angola", Abbreviation = "Angola", Type = "COUNTRY", IconName = "ao" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_antigua", Name = "AntiguaundBarbuda", FullName = "AntiguaundBarbuda", Abbreviation = "AntiguaundBarbuda", Type = "COUNTRY", IconName = "ag" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_arabische_republik_syrien", Name = "Syrien", FullName = "ArabischeRepublikSyrien", Abbreviation = "Syrien", Type = "COUNTRY", IconName = "sy" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_argentinien", Name = "Argentinien", FullName = "ArgentinischeRepublik", Abbreviation = "Argentinien", Type = "COUNTRY", IconName = "ar" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_armenien", Name = "Armenien", FullName = "RepublikArmenien", Abbreviation = "Armenien", Type = "COUNTRY", IconName = "am" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_aserbaidschan", Name = "Aserbaidschan", FullName = "RepublikAserbaidschan", Abbreviation = "Aserbaidschan", Type = "COUNTRY", IconName = "az" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_australien", Name = "Australien", FullName = "CommonwealthvonAustralien", Abbreviation = "Australien", Type = "COUNTRY", IconName = "au" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_bahamas", Name = "Bahamas", FullName = "CommonwealthderBahamas", Abbreviation = "Bahamas", Type = "COUNTRY", IconName = "bs" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_bahrain", Name = "Bahrain", FullName = "KönigreichBahrain", Abbreviation = "Bahrain", Type = "COUNTRY", IconName = "bh" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_bangladesch", Name = "Bangladesch", FullName = "VolksrepublikBangladesch", Abbreviation = "Bangladesch", Type = "COUNTRY", IconName = "bd" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_barbados", Name = "Barbados", FullName = "Barbados", Abbreviation = "Barbados", Type = "COUNTRY", IconName = "bb" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_belgien", Name = "Belgien", FullName = "KönigreichBelgien", Abbreviation = "Belgien", Type = "COUNTRY", IconName = "be" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_belize", Name = "Belize", FullName = "Belize", Abbreviation = "Belize", Type = "COUNTRY", IconName = "bz" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_benin", Name = "Benin", FullName = "RepublikBenin", Abbreviation = "Benin", Type = "COUNTRY", IconName = "bj" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_bhutan", Name = "Bhutan", FullName = "KönigreichBhutan", Abbreviation = "Bhutan", Type = "COUNTRY", IconName = "bt" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_bolivien", Name = "Bolivien", FullName = "PlurinationalerStaatBolivien", Abbreviation = "Bolivien", Type = "COUNTRY", IconName = "bo" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_bosnien", Name = "BosnienundHerzegowina", FullName = "BosnienundHerzegowina", Abbreviation = "BosnienundHerzegowina", Type = "COUNTRY", IconName = "ba" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_botswana", Name = "Botswana", FullName = "RepublikBotswana", Abbreviation = "Botswana", Type = "COUNTRY", IconName = "bw" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_brasilien", Name = "Brasilien", FullName = "FöderativeRepublikBrasilien", Abbreviation = "Brasilien", Type = "COUNTRY", IconName = "br" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_brunei", Name = "Brunei", FullName = "BruneiDarussalam", Abbreviation = "Brunei", Type = "COUNTRY", IconName = "bn" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_bulgarien", Name = "Bulgarien", FullName = "RepublikBulgarien", Abbreviation = "Bulgarien", Type = "COUNTRY", IconName = "bg" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_bundesrepublik_somalia", Name = "Somalia", FullName = "BundesrepublikSomalia", Abbreviation = "Somalia", Type = "COUNTRY", IconName = "so" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_burkina_faso", Name = "BurkinaFaso", FullName = "BurkinaFaso", Abbreviation = "BurkinaFaso", Type = "COUNTRY", IconName = "bf" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_burundi", Name = "Burundi", FullName = "RepublikBurundi", Abbreviation = "Burundi", Type = "COUNTRY", IconName = "bi" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_chile", Name = "Chile", FullName = "RepublikChile", Abbreviation = "Chile", Type = "COUNTRY", IconName = "cl" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_china", Name = "VolksrepublikChina", FullName = "VolksrepublikChina", Abbreviation = "China", Type = "COUNTRY", IconName = "cn" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_costa_rica", Name = "CostaRica", FullName = "RepublikCostaRica", Abbreviation = "CostaRica", Type = "COUNTRY", IconName = "cr" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_daenemark", Name = "Dänemark", FullName = "KönigreichDänemark", Abbreviation = "Dänemark", Type = "COUNTRY", IconName = "dk" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_demokratische_republik_timor-leste", Name = "Timor-Leste", FullName = "DemokratischeRepublikTimor-Leste", Abbreviation = "Timor-Leste", Type = "COUNTRY", IconName = "tl" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_deutschland", Name = "Deutschland", FullName = "BundesrepublikDeutschland", Abbreviation = "Deutschland", Type = "COUNTRY", IconName = "de" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_dominica", Name = "Dominica", FullName = "CommonwealthDominica", Abbreviation = "Dominica", Type = "COUNTRY", IconName = "dm" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_dominikanische_republik", Name = "DominikanischeRepublik", FullName = "DominikanischeRepublik", Abbreviation = "DominikanischeRepublik", Type = "COUNTRY", IconName = "do" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_dr_kongo", Name = "DRKongo", FullName = "DemokratischeRepublikKongo", Abbreviation = "DRKongo", Type = "COUNTRY", IconName = "cd" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_dschibuti", Name = "Dschibuti", FullName = "RepublikDschibuti", Abbreviation = "Dschibuti", Type = "COUNTRY", IconName = "dj" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_dvr_korea", Name = "DVRKorea", FullName = "DemokratischeVolksrepublikKorea", Abbreviation = "Nordkorea", Type = "COUNTRY", IconName = "kp" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_ecuador", Name = "Ecuador", FullName = "RepublikEcuador", Abbreviation = "Ecuador", Type = "COUNTRY", IconName = "ec" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_elfenbeinkueste", Name = "Côted\"Ivoire", FullName = "RepublikCôted\"Ivoire", Abbreviation = "Côted\"Ivoire", Type = "COUNTRY", IconName = "ci" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_eritrea", Name = "Eritrea", FullName = "StaatEritrea", Abbreviation = "Eritrea", Type = "COUNTRY", IconName = "er" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_estland", Name = "Estland", FullName = "RepublikEstland", Abbreviation = "Estland", Type = "COUNTRY", IconName = "ee" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_fidschi", Name = "Fidschi", FullName = "RepublikFidschi-Inseln", Abbreviation = "Fidschi", Type = "COUNTRY", IconName = "fj" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_finnland", Name = "Finnland", FullName = "RepublikFinnland", Abbreviation = "Finnland", Type = "COUNTRY", IconName = "fi" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_frankreich", Name = "Frankreich", FullName = "FranzösischeRepublik", Abbreviation = "Frankreich", Type = "COUNTRY", IconName = "fr" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_gabun", Name = "Gabun", FullName = "GabunischeRepublik", Abbreviation = "Gabun", Type = "COUNTRY", IconName = "ga" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_gambia", Name = "Gambia", FullName = "RepublikGambia", Abbreviation = "Gambia", Type = "COUNTRY", IconName = "gm" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_georgien", Name = "Georgien", FullName = "Georgien", Abbreviation = "Georgien", Type = "COUNTRY", IconName = "ge" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_ghana", Name = "Ghana", FullName = "RepublikGhana", Abbreviation = "Ghana", Type = "COUNTRY", IconName = "gh" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_grenada", Name = "Grenada", FullName = "StaatGrenada", Abbreviation = "Grenada", Type = "COUNTRY", IconName = "gd" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_griechenland", Name = "Griechenland", FullName = "HellenischeRepublik", Abbreviation = "Griechenland", Type = "COUNTRY", IconName = "gr" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_guatemala", Name = "Guatemala", FullName = "RepublikGuatemala", Abbreviation = "Guatemala", Type = "COUNTRY", IconName = "gt" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_guinea", Name = "Guinea", FullName = "RepublikGuinea", Abbreviation = "Guinea", Type = "COUNTRY", IconName = "gn" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_Guinea-Bissau", Name = "Guinea-Bissau", FullName = "RepublikGuinea-Bissau", Abbreviation = "Guinea-Bissau", Type = "COUNTRY", IconName = "gw" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_guyana", Name = "Guyana", FullName = "KooperativeRepublikGuyana", Abbreviation = "Guyana", Type = "COUNTRY", IconName = "gy" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_haiti", Name = "Haiti", FullName = "RepublikHaiti", Abbreviation = "Haiti", Type = "COUNTRY", IconName = "ht" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_honduras", Name = "Honduras", FullName = "RepublikHonduras", Abbreviation = "Honduras", Type = "COUNTRY", IconName = "hn" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_indien", Name = "Indien", FullName = "RepublikIndien", Abbreviation = "Indien", Type = "COUNTRY", IconName = "in" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_indonesien", Name = "Indonesien", FullName = "RepublikIndonesien", Abbreviation = "Indonesien", Type = "COUNTRY", IconName = "id" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_irak", Name = "Irak", FullName = "RepublikIrak", Abbreviation = "Irak", Type = "COUNTRY", IconName = "iq" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_iran", Name = "Iran", FullName = "IslamischeRepublikIran", Abbreviation = "Iran", Type = "COUNTRY", IconName = "ir" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_irland", Name = "Irland", FullName = "Irland", Abbreviation = "Irland", Type = "COUNTRY", IconName = "ie" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_islamische_republik_mauretanien", Name = "Mauretanien", FullName = "IslamischeRepublikMauretanien", Abbreviation = "Mauretanien", Type = "COUNTRY", IconName = "mr" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_islamische_republik_pakistan", Name = "Pakistan", FullName = "IslamischeRepublikPakistan", Abbreviation = "Pakistan", Type = "COUNTRY", IconName = "pk" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_island", Name = "Island", FullName = "RepublikIsland", Abbreviation = "Island", Type = "COUNTRY", IconName = "is" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_israel", Name = "Israel", FullName = "StaatIsrael", Abbreviation = "Israel", Type = "COUNTRY", IconName = "il" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_italien", Name = "Italien", FullName = "ItalienischeRepublik", Abbreviation = "Italien", Type = "COUNTRY", IconName = "it" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_jamaika", Name = "Jamaika", FullName = "Jamaika", Abbreviation = "Jamaika", Type = "COUNTRY", IconName = "jm" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_japan", Name = "Japan", FullName = "StaatJapan", Abbreviation = "Japan", Type = "COUNTRY", IconName = "jp" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_jemen", Name = "Jemen", FullName = "RepublikJemen", Abbreviation = "Jemen", Type = "COUNTRY", IconName = "ye" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_jordanien", Name = "Jordanien", FullName = "HaschemitischesKönigreichJordanien", Abbreviation = "Jordanien", Type = "COUNTRY", IconName = "jo" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_kambodscha", Name = "Kambodscha", FullName = "KönigreichKambodscha", Abbreviation = "Kambodscha", Type = "COUNTRY", IconName = "kh" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_kamerun", Name = "Kamerun", FullName = "RepublikKamerun", Abbreviation = "Kamerun", Type = "COUNTRY", IconName = "cm" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_kanada", Name = "Kanada", FullName = "Kanada", Abbreviation = "Kanada", Type = "COUNTRY", IconName = "ca" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_kap_verde", Name = "KapVerde", FullName = "RepublikKapVerde", Abbreviation = "KapVerde", Type = "COUNTRY", IconName = "cv" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_kasachstan", Name = "Kasachstan", FullName = "RepublikKasachstan", Abbreviation = "Kasachstan", Type = "COUNTRY", IconName = "kz" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_katar", Name = "Katar", FullName = "StaatKatar", Abbreviation = "Katar", Type = "COUNTRY", IconName = "qa" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_Kenia", Name = "Kenia", FullName = "RepublikKenia", Abbreviation = "Kenia", Type = "COUNTRY", IconName = "ke" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_kirgisistan", Name = "Kirgisistan", FullName = "KirgisischeRepublik", Abbreviation = "Kirgisistan", Type = "COUNTRY", IconName = "kg" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_kiribati", Name = "Kiribati", FullName = "RepublikKiribati", Abbreviation = "Kiribati", Type = "COUNTRY", IconName = "ki" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_kolumbien", Name = "Kolumbien", FullName = "RepublikKolumbien", Abbreviation = "Kolumbien", Type = "COUNTRY", IconName = "co" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_komoren", Name = "Komoren", FullName = "UnionderKomoren", Abbreviation = "Komoren", Type = "COUNTRY", IconName = "km" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_kroatien", Name = "Kroatien", FullName = "RepublikKroatien", Abbreviation = "Kroatien", Type = "COUNTRY", IconName = "hr" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_kuba", Name = "Kuba", FullName = "RepublikKuba", Abbreviation = "Kuba", Type = "COUNTRY", IconName = "cu" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_kuwait", Name = "Kuwait", FullName = "StaatKuwait", Abbreviation = "Kuwait", Type = "COUNTRY", IconName = "kw" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_laos", Name = "Laos", FullName = "DemokratischeVolksrepublikLaos", Abbreviation = "Laos", Type = "COUNTRY", IconName = "la" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_lesotho", Name = "Lesotho", FullName = "KönigreichLesotho", Abbreviation = "Lesotho", Type = "COUNTRY", IconName = "ls" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_lettland", Name = "Lettland", FullName = "RepublikLettland", Abbreviation = "Lettland", Type = "COUNTRY", IconName = "lv" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_libanon", Name = "Libanon", FullName = "LibanesischeRepublik", Abbreviation = "Libanon", Type = "COUNTRY", IconName = "lb" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_liberia", Name = "Liberia", FullName = "RepublikLiberia", Abbreviation = "Liberia", Type = "COUNTRY", IconName = "lr" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_libyen", Name = "Libyen", FullName = "Libyen", Abbreviation = "Libyen", Type = "COUNTRY", IconName = "ly" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_liechtenstein", Name = "Liechtenstein", FullName = "FürstentumLiechtenstein", Abbreviation = "Liechtenstein", Type = "COUNTRY", IconName = "li" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_litauen", Name = "Litauen", FullName = "RepublikLitauen", Abbreviation = "Litauen", Type = "COUNTRY", IconName = "lt" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_luxemburg", Name = "Luxemburg", FullName = "GroßherzogtumLuxemburg", Abbreviation = "Luxemburg", Type = "COUNTRY", IconName = "lu" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_malaysia", Name = "Malaysia", FullName = "Malaysia", Abbreviation = "Malaysia", Type = "COUNTRY", IconName = "my" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_marokko", Name = "Marokko", FullName = "KönigreichMarokko", Abbreviation = "Marokko", Type = "COUNTRY", IconName = "ma" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_mikronesien", Name = "Mikronesien", FullName = "FöderierteStaatenvonMikronesien", Abbreviation = "Mikronesien", Type = "COUNTRY", IconName = "fm" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_monaco", Name = "Monaco", FullName = "FürstentumMonaco", Abbreviation = "Monaco", Type = "COUNTRY", IconName = "mc" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_mongolei", Name = "Mongolei", FullName = "Mongolei", Abbreviation = "Mongolei", Type = "COUNTRY", IconName = "mn" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_montenegro", Name = "Montenegro", FullName = "Montenegro", Abbreviation = "Montenegro", Type = "COUNTRY", IconName = "me" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_nepal", Name = "Nepal", FullName = "DemokratischeBundesrepublikNepal", Abbreviation = "Nepal", Type = "COUNTRY", IconName = "np" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_neuseeland", Name = "Neuseeland", FullName = "Neuseeland", Abbreviation = "Neuseeland", Type = "COUNTRY", IconName = "nz" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_niederlande", Name = "Niederlande", FullName = "KönigreichderNiederlande", Abbreviation = "Niederlande", Type = "COUNTRY", IconName = "nl" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_nigeria", Name = "Nigeria", FullName = "BundesrepublikNigeria", Abbreviation = "Nigeria", Type = "COUNTRY", IconName = "ng" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_norwegen", Name = "Norwegen", FullName = "KönigreichNorwegen", Abbreviation = "Norwegen", Type = "COUNTRY", IconName = "no" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_portugal", Name = "Portugal", FullName = "PortugiesischeRepublik", Abbreviation = "Portugal", Type = "COUNTRY", IconName = "pt" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_belarus", Name = "Belarus", FullName = "RepublikBelarus", Abbreviation = "Belarus", Type = "COUNTRY", IconName = null });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_der_philippinen", Name = "Philippinen", FullName = "RepublikderPhilippinen", Abbreviation = "Philippinen", Type = "COUNTRY", IconName = "ph" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_kongo", Name = "RepublikKongo", FullName = "RepublikKongo", Abbreviation = "RepublikKongo", Type = "COUNTRY", IconName = null });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_korea", Name = "RepublikKorea", FullName = "RepublikKorea", Abbreviation = "Südkorea", Type = "COUNTRY", IconName = null });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_madagaskar", Name = "Madagaskar", FullName = "RepublikMadagaskar", Abbreviation = "Madagaskar", Type = "COUNTRY", IconName = "mg" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_malawi", Name = "Malawi", FullName = "RepublikMalawi", Abbreviation = "Malawi", Type = "COUNTRY", IconName = "mw" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_malediven", Name = "Malediven", FullName = "RepublikMalediven", Abbreviation = "Malediven", Type = "COUNTRY", IconName = "mv" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_mali", Name = "Mali", FullName = "RepublikMali", Abbreviation = "Mali", Type = "COUNTRY", IconName = "ml" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_malta", Name = "Malta", FullName = "RepublikMalta", Abbreviation = "Malta", Type = "COUNTRY", IconName = "mt" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_marshallinseln", Name = "Marshallinseln", FullName = "RepublikMarshallinseln", Abbreviation = "Marshallinseln", Type = "COUNTRY", IconName = "mh" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_mauritius", Name = "Mauritius", FullName = "RepublikMauritius", Abbreviation = "Mauritius", Type = "COUNTRY", IconName = "mu" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_moldau", Name = "Moldau", FullName = "RepublikMoldau", Abbreviation = "Moldau", Type = "COUNTRY", IconName = "md" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_mosambik", Name = "Mosambik", FullName = "RepublikMosambik", Abbreviation = "Mosambik", Type = "COUNTRY", IconName = "mz" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_namibia", Name = "Namibia", FullName = "RepublikNamibia", Abbreviation = "Namibia", Type = "COUNTRY", IconName = "na" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_nauru", Name = "Nauru", FullName = "RepublikNauru", Abbreviation = "Nauru", Type = "COUNTRY", IconName = "nr" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_nicaragua", Name = "Nicaragua", FullName = "RepublikNicaragua", Abbreviation = "Nicaragua", Type = "COUNTRY", IconName = "ni" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_niger", Name = "Niger", FullName = "RepublikNiger", Abbreviation = "Niger", Type = "COUNTRY", IconName = "ne" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_nordmazedonien", Name = "Nordmazedonien", FullName = "RepublikNordmazedonien", Abbreviation = "Nordmazedonien", Type = "COUNTRY", IconName = "mk" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_oesterreich", Name = "Österreich", FullName = "RepublikÖsterreich", Abbreviation = "Österreich", Type = "COUNTRY", IconName = "at" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_palau", Name = "Palau", FullName = "RepublikPalau", Abbreviation = "Palau", Type = "COUNTRY", IconName = "pw" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_panama", Name = "Panama", FullName = "RepublikPanama", Abbreviation = "Panama", Type = "COUNTRY", IconName = "pa" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_paraguay", Name = "Paraguay", FullName = "RepublikParaguay", Abbreviation = "Paraguay", Type = "COUNTRY", IconName = "py" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_peru", Name = "Peru", FullName = "RepublikPeru", Abbreviation = "Peru", Type = "COUNTRY", IconName = "pe" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_polen", Name = "Polen", FullName = "RepublikPolen", Abbreviation = "Polen", Type = "COUNTRY", IconName = "pl" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_ruanda", Name = "Ruanda", FullName = "RepublikRuanda", Abbreviation = "Ruanda", Type = "COUNTRY", IconName = "rw" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_sambia", Name = "Sambia", FullName = "RepublikSambia", Abbreviation = "Sambia", Type = "COUNTRY", IconName = "zm" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_san_marino", Name = "SanMarino", FullName = "RepublikSanMarino", Abbreviation = "SanMarino", Type = "COUNTRY", IconName = "sm" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_senegal", Name = "Senegal", FullName = "RepublikSenegal", Abbreviation = "Senegal", Type = "COUNTRY", IconName = "sn" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_serbien", Name = "Serbien", FullName = "RepublikSerbien", Abbreviation = "Serbien", Type = "COUNTRY", IconName = "rs" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_seychellen", Name = "Seychellen", FullName = "RepublikSeychellen", Abbreviation = "Seychellen", Type = "COUNTRY", IconName = "sc" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_sierra_leone", Name = "SierraLeone", FullName = "RepublikSierraLeone", Abbreviation = "SierraLeone", Type = "COUNTRY", IconName = "sl" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_simbabwe", Name = "Simbabwe", FullName = "RepublikSimbabwe", Abbreviation = "Simbabwe", Type = "COUNTRY", IconName = "zw" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_singapur", Name = "Singapur", FullName = "RepublikSingapur", Abbreviation = "Singapur", Type = "COUNTRY", IconName = "sg" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_slowenien", Name = "Slowenien", FullName = "RepublikSlowenien", Abbreviation = "Slowenien", Type = "COUNTRY", IconName = "si" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_sudan", Name = "Sudan", FullName = "RepublikSudan", Abbreviation = "Sudan", Type = "COUNTRY", IconName = "sd" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_suedafrika", Name = "Südafrika", FullName = "RepublikSüdafrika", Abbreviation = "Südafrika", Type = "COUNTRY", IconName = "za" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_suedsudan", Name = "Südsudan", FullName = "RepublikSüdsudan", Abbreviation = "Südsudan", Type = "COUNTRY", IconName = "ss" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_suriname", Name = "Suriname", FullName = "RepublikSuriname", Abbreviation = "Suriname", Type = "COUNTRY", IconName = "sr" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_tadschikistan", Name = "Tadschikistan", FullName = "RepublikTadschikistan", Abbreviation = "Tadschikistan", Type = "COUNTRY", IconName = "tj" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_togo", Name = "Togo", FullName = "RepublikTogo", Abbreviation = "Togo", Type = "COUNTRY", IconName = "tg" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_trinidad_und_tobago", Name = "TrinidadundTobago", FullName = "RepublikTrinidadundTobago", Abbreviation = "TrinidadundTobago", Type = "COUNTRY", IconName = "tt" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_tschad", Name = "Tschad", FullName = "RepublikTschad", Abbreviation = "Tschad", Type = "COUNTRY", IconName = "td" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_türkei", Name = "Türkei", FullName = "RepublikTürkei", Abbreviation = "Türkei", Type = "COUNTRY", IconName = "tr" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_uganda", Name = "Uganda", FullName = "RepublikUganda", Abbreviation = "Uganda", Type = "COUNTRY", IconName = "ug" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_ungarn", Name = "Ungarn", FullName = "RepublikUngarn", Abbreviation = "Ungarn", Type = "COUNTRY", IconName = "hu" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_usbekistan", Name = "Usbekistan", FullName = "RepublikUsbekistan", Abbreviation = "Usbekistan", Type = "COUNTRY", IconName = "uz" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_vanuatu", Name = "Vanuatu", FullName = "RepublikVanuatu", Abbreviation = "Vanuatu", Type = "COUNTRY", IconName = "vu" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_republik_zypern", Name = "Zypern", FullName = "RepublikZypern", Abbreviation = "Zypern", Type = "COUNTRY", IconName = "cy" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_rumaenien", Name = "Rumänien", FullName = "Rumänien", Abbreviation = "Rumänien", Type = "COUNTRY", IconName = "ro" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_russische_foederation", Name = "Russland", FullName = "RussischeFöderation", Abbreviation = "Russland", Type = "COUNTRY", IconName = "ru" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_salomonen", Name = "Salomonen", FullName = "Salomonen", Abbreviation = "Salomonen", Type = "COUNTRY", IconName = "sb" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_salvador", Name = "ElSalvador", FullName = "RepublikElSalvador", Abbreviation = "ElSalvador", Type = "COUNTRY", IconName = "sv" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_sao_tome_und_principe", Name = "SãoToméundPríncipe", FullName = "DemokratischeRepublikSãoToméundPríncipe", Abbreviation = "SãoToméundPríncipe", Type = "COUNTRY", IconName = "st" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_saudi-arabien", Name = "Saudi-Arabien", FullName = "KönigreichSaudi-Arabien", Abbreviation = "Saudi-Arabien", Type = "COUNTRY", IconName = "sa" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_schweden", Name = "Schweden", FullName = "KönigreichSchweden", Abbreviation = "Schweden", Type = "COUNTRY", IconName = "se" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_schweizerische_eidgenossenschaft", Name = "Schweiz", FullName = "SchweizerischeEidgenossenschaft", Abbreviation = "Schweiz", Type = "COUNTRY", IconName = "ch" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_slowakische_republik", Name = "Slowakei", FullName = "SlowakischeRepublik", Abbreviation = "Slowakei", Type = "COUNTRY", IconName = "sk" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_sozialistische_republik_vietnam", Name = "Vietnam", FullName = "SozialistischeRepublikVietnam", Abbreviation = "Vietnam", Type = "COUNTRY", IconName = "vn" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_spanien", Name = "Spanien", FullName = "KönigreichSpanien", Abbreviation = "Spanien", Type = "COUNTRY", IconName = "es" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_sri_lanka", Name = "SriLanka", FullName = "DemokratischeSozialistischeRepublikSriLanka", Abbreviation = "SriLanka", Type = "COUNTRY", IconName = "lk" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_staat_palästina", Name = "Palästina", FullName = "StaatPalästina", Abbreviation = "Palästina", Type = "COUNTRY", IconName = null });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_staat_vatikanstadt", Name = "Vatikanstadt", FullName = "StaatVatikanstadt", Abbreviation = "Vatikanstadt", Type = "COUNTRY", IconName = null });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_st_kitts_und_nevis", Name = "St.KittsundNevis", FullName = "FöderationSt.KittsundNevis", Abbreviation = "St.KittsundNevis", Type = "COUNTRY", IconName = "kn" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_st_lucia", Name = "St.Lucia", FullName = "St.Lucia", Abbreviation = "St.Lucia", Type = "COUNTRY", IconName = "lc" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_st_vincent_und_die_grenadinen", Name = "St.VincentunddieGrenadinen", FullName = "St.VincentunddieGrenadinen", Abbreviation = "St.VincentunddieGrenadinen", Type = "COUNTRY", IconName = "vc" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_sultanat_oman", Name = "Oman", FullName = "SultanatOman", Abbreviation = "Oman", Type = "COUNTRY", IconName = "om" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_swasiland", Name = "Swasiland", FullName = "KönigreichSwasiland", Abbreviation = "Swasiland", Type = "COUNTRY", IconName = "sz" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_taiwan", Name = "Taiwan", FullName = "Taiwan", Abbreviation = "Taiwan", Type = "COUNTRY", IconName = null });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_thailand", Name = "Thailand", FullName = "KönigreichThailand", Abbreviation = "Thailand", Type = "COUNTRY", IconName = "th" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_tonga", Name = "Tonga", FullName = "KönigreichTonga", Abbreviation = "Tonga", Type = "COUNTRY", IconName = "to" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_tschechische_republik", Name = "Tschechien", FullName = "TschechischeRepublik", Abbreviation = "Tschechien", Type = "COUNTRY", IconName = "cz" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_tunesische_republik", Name = "Tunesien", FullName = "TunesischeRepublik", Abbreviation = "Tunesien", Type = "COUNTRY", IconName = "tn" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_turkmenistan", Name = "Turkmenistan", FullName = "Turkmenistan", Abbreviation = "Turkmenistan", Type = "COUNTRY", IconName = "tm" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_tuvalu", Name = "Tuvalu", FullName = "Tuvalu", Abbreviation = "Tuvalu", Type = "COUNTRY", IconName = "tv" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_türkische_republik_nordzypern", Name = "Nordzypern", FullName = "TürkischeRepublikNordzypern", Abbreviation = "Nordzypern", Type = "COUNTRY", IconName = null });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_ukraine", Name = "Ukraine", FullName = "Ukraine", Abbreviation = "Ukraine", Type = "COUNTRY", IconName = "ua" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_unabhängiger_staat_papua-neuguinea", Name = "Papua-Neuguinea", FullName = "UnabhängigerStaatPapua-Neuguinea", Abbreviation = "Papua-Neuguinea", Type = "COUNTRY", IconName = "pg" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_unabhängiger_staat_samoa", Name = "Samoa", FullName = "UnabhängigerStaatSamoa", Abbreviation = "Samoa", Type = "COUNTRY", IconName = "ws" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_union_myanmar", Name = "Myanmar", FullName = "UnionMyanmar", Abbreviation = "Myanmar", Type = "COUNTRY", IconName = "mm" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_uruguay", Name = "Uruguay", FullName = "RepublikÖstlichdesUruguay", Abbreviation = "Uruguay", Type = "COUNTRY", IconName = "uy" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_venezuela", Name = "Venezuela", FullName = "BolivarischeRepublikVenezuela", Abbreviation = "Venezuela", Type = "COUNTRY", IconName = "ve" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_vereinigtes_koenigreich", Name = "VereinigtesKönigreich", FullName = "VereinigtesKönigreichGroßbritannienundNordirland", Abbreviation = "VereinigtesKönigreich", Type = "COUNTRY", IconName = "gb" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_vereinigte_arabische_emirate", Name = "VereinigteArabischeEmirate", FullName = "VereinigteArabischeEmirate", Abbreviation = "VereinigteArabischeEmirate", Type = "COUNTRY", IconName = "ae" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_vereinigte_mexikanische_staaten", Name = "Mexiko", FullName = "VereinigteMexikanischeStaaten", Abbreviation = "Mexiko", Type = "COUNTRY", IconName = "mx" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_vereinigte_republik_tansania", Name = "Tansania", FullName = "VereinigteRepublikTansania", Abbreviation = "Tansania", Type = "COUNTRY", IconName = "tz" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_vereinigte_staaten_von_amerika", Name = "VereinigteStaaten", FullName = "VereinigteStaatenvonAmerika", Abbreviation = "VereinigteStaaten", Type = "COUNTRY", IconName = "us" });
            _context.Delegations.Add(new Delegation() { DelegationId = "default_zentralafrikanische_republik", Name = "Zentral­afrikanischeRepublik", FullName = "ZentralafrikanischeRepublik", Abbreviation = "Zentral­afrikanischeRepublik", Type = "COUNTRY", IconName = "cf" });
            _context.SaveChanges();
        }
    }
}
