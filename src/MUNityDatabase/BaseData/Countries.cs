using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Base;
using MUNity.Database.Extensions;
using MUNity.Database.General;

namespace MUNity.Database.BaseData;

public static class Countries
{
    public static IEnumerable<Country> BaseCountries
    {
        get
        {
            // Africa 
            yield return new Country(5, EContinent.Africa, "Algerien", "Demokratische Volksrepublik Algerien", "DZ", true).AddTranslation("en-EN", "Algeria");
            yield return new Country(7, EContinent.Africa, "Angola", "Republik Angola", "AO", true).AddTranslation("en-EN", "Angola");
            yield return new Country(22, EContinent.Africa, "Benin", "Republik Benin", "BJ", true).AddTranslation("en-EN", "Benin");
            yield return new Country(26, EContinent.Africa, "Botswana", "Republik Botsuana", "BW", true).AddTranslation("en-EN", "Botswana");
            yield return new Country(30, EContinent.Africa, "Burkina Faso", "Burkina Faso", "BF", true).AddTranslation("en-EN", "Burkina Faso");
            yield return new Country(31, EContinent.Africa, "Burundi", "Republik Burundi", "BI", true).AddTranslation("en-EN", "Burundi");
            yield return new Country(78, EContinent.Africa, "Kap Verde", "Republik Cabo Verde", "CV", true).AddTranslation("en-EN", "Cape Verde");
            yield return new Country(76, EContinent.Africa, "Kamerun", "Republik Kamerun", "CM", true).AddTranslation("en-EN", "Cameroon");
            yield return new Country(212, EContinent.Africa, "Zentralafrikanische Republik", "Zentralafrikanische Republik", "CF", true).AddTranslation("en-EN", "Central African Republic");
            yield return new Country(192, EContinent.Africa, "Tschad", "Republik Tschad", "TD", true).AddTranslation("en-EN", "Chad");
            yield return new Country(85, EContinent.Africa, "Komoren", "Union der Komoren", "KM", true).AddTranslation("en-EN", "Comoros");
            yield return new Country(86, EContinent.Africa, "Demokratische Republik Kongo", "Demokratische Republik Kongo", "CD", true).AddTranslation("en-EN", "Congo, Democratic Republic of the (Kinshasa)");
            yield return new Country(87, EContinent.Africa, "Kongo, Republik", "Republik Kongo", "CG", true).AddTranslation("en-EN", "Congo, Republic of (Brazzaville)");
            yield return new Country(41, EContinent.Africa, "Dschibuti", "Republik Dschibuti", "DJ", true).AddTranslation("en-EN", "Djibouti");
            yield return new Country(3, EContinent.Africa, "Ägypten", "Arabische Republik Ägypten", "EG", true).AddTranslation("en-EN", "Egypt");
            yield return new Country(9, EContinent.Africa, "Äquatorialguinea", "Republik Äquatorial­guinea", "GQ", true).AddTranslation("en-EN", "Equatorial Guinea");
            yield return new Country(14, EContinent.Africa, "Äthiopien", "Demokratische Bundesrepublik Äthiopien", "ET", true).AddTranslation("en-EN", "Ethiopia");
            yield return new Country(51, EContinent.Africa, "Gabun", "Gabunische Republik", "GA", true).AddTranslation("en-EN", "Gabon");
            yield return new Country(52, EContinent.Africa, "Gambia", "Republik Gambia", "GM", true).AddTranslation("en-EN", "	Gambia");
            yield return new Country(54, EContinent.Africa, "Ghana", "Republik Ghana", "GH", true).AddTranslation("en-EN", "Ghana");
            yield return new Country(58, EContinent.Africa, "Guinea", "Republik Guinea", "GN", true).AddTranslation("en-EN", "Guinea");
            yield return new Country(59, EContinent.Africa, "Guinea-Bissau", "Republik Guinea-Bissau", "GW", true).AddTranslation("en-EN", "Guinea-Bissau");
            yield return new Country(44, EContinent.Africa, "Elfenbeinküste", "Republik Côte d’Ivoire", "CI", true).AddTranslation("en-EN", "Ivory Coast");
            yield return new Country(81, EContinent.Africa, "Kenia", "Republik Kenia", "KE", true).AddTranslation("en-EN", "Kenya");
            yield return new Country(95, EContinent.Africa, "Lesotho", "Königreich Lesotho", "LS", true).AddTranslation("en-EN", "Lesotho");
            yield return new Country(98, EContinent.Africa, "Liberia", "Republik Liberia", "LR", true).AddTranslation("en-EN", "Liberia");
            yield return new Country(99, EContinent.Africa, "Libyen", "Staat Libyen", "LY", true).AddTranslation("en-EN", "Libya");
            yield return new Country(103, EContinent.Africa, "Madagaskar", "Republik Madagaskar", "MG", true).AddTranslation("en-EN", "Madagascar");
            yield return new Country(104, EContinent.Africa, "Malawi", "Republik Malawi", "MW", true).AddTranslation("en-EN", "Malawi");
            yield return new Country(107, EContinent.Africa, "Mali", "Republik Mali", "ML", true).AddTranslation("en-EN", "Mali");
            yield return new Country(111, EContinent.Africa, "Mauretanien", "Islamische Republik Mauretanien", "MR", true).AddTranslation("en-EN", "Mauritania");
            yield return new Country(112, EContinent.Africa, "Mauritius", "Republik Mauritius", "MU", true).AddTranslation("en-EN", "Mauritius");
            yield return new Country(109, EContinent.Africa, "Marokko", "Königreich Marokko", "MA", true).AddTranslation("en-EN", "Morocco");
            yield return new Country(119, EContinent.Africa, "Mosambik", "Republik Mosambik", "MZ", true).AddTranslation("en-EN", "Mozambique");
            yield return new Country(121, EContinent.Africa, "Namibia", "Republik Namibia", "", true).AddTranslation("en-EN", "Namibia");
            yield return new Country(127, EContinent.Africa, "Niger", "Republik Niger", "NE", true).AddTranslation("en-EN", "Niger");
            yield return new Country(128, EContinent.Africa, "Nigeria", "Bundesrepublik Nigeria", "NG", true).AddTranslation("en-EN", "Nigeria");
            yield return new Country(146, EContinent.Africa, "Ruanda", "Republik Ruanda", "RW", true).AddTranslation("en-EN", "Rwanda");
            yield return new Country(153, EContinent.Africa, "São Tomé und Príncipe", "Demokratische Republik São Tomé und Príncipe", "ST", true).AddTranslation("en-EN", "São Tomé and Príncipe");
            yield return new Country(157, EContinent.Africa, "Senegal", "Republik Senegal", "SN", true).AddTranslation("en-EN", "Senegal");
            yield return new Country(159, EContinent.Africa, "Seychellen", "Republik Seychellen", "SC", true).AddTranslation("en-EN", "Seychelles");
            yield return new Country(160, EContinent.Africa, "Sierra Leone", "Republik Sierra Leone", "SL", true).AddTranslation("en-EN", "Sierra Leone");
            yield return new Country(165, EContinent.Africa, "Somalia", "Bundesrepublik Somalia", "SO", true).AddTranslation("en-EN", "Somalia");
            yield return new Country(179, EContinent.Africa, "Südafrika", "Republik Südafrika", "ZA", true).AddTranslation("en-EN", "South Africa");
            yield return new Country(182, EContinent.Africa, "Südsudan", "Republik Südsudan", "SS", true).AddTranslation("en-EN", "South Sudan");
            yield return new Country(180, EContinent.Africa, "Sudan", "Republik Sudan", "SD", true).AddTranslation("en-EN", "Sudan");
            yield return new Country(186, EContinent.Africa, "Tansania", "Vereinigte Republik Tansania", "TZ", true).AddTranslation("en-EN", "Tanzania");
            yield return new Country(188, EContinent.Africa, "Togo", "Republik Togo", "TG", true).AddTranslation("en-EN", "Togo");
            yield return new Country(194, EContinent.Africa, "Tunesien", "Tunesische Republik", "TN", true).AddTranslation("en-EN", "Tunisia");
            yield return new Country(198, EContinent.Africa, "Uganda", "Republik Uganda", "UG", true).AddTranslation("en-EN", "Uganda");
            yield return new Country(150, EContinent.Africa, "Sambia", "Republik Sambia", "ZM", true).AddTranslation("en-EN", "Zambia");
            yield return new Country(161, EContinent.Africa, "Simbabwe", "Republik Simbabwe", "ZW", true).AddTranslation("en-EN", "Zimbabwe");

            // Asia
            yield return new Country(2, EContinent.Asia, "Afghanistan", "Islamische Republik Afghanistan", "AF", true).AddTranslation("en-EN", "Afghanistan");
            yield return new Country(17, EContinent.Asia, "Bahrain", "Königreich Bahrain", "BH", true).AddTranslation("en-EN", "Bahrain");
            yield return new Country(18, EContinent.Asia, "Bangladesch", "Volksrepublik Bangladesch", "BD", true).AddTranslation("en-EN", "Bangladesh");
            yield return new Country(23, EContinent.Asia, "Bhutan", "Königreich Bhutan", "BT", true).AddTranslation("en-EN", "Bhutan");
            yield return new Country(28, EContinent.Asia, "Brunei", "Brunei Darussalam", "BN", true).AddTranslation("en-EN", "Brunei Darussalam");
            yield return new Country(75, EContinent.Asia, "Kambodscha", "Königreich Kambodscha", "KH", true).AddTranslation("en-EN", "Cambodia");
            yield return new Country(34, EContinent.Asia, "Volksrepublik China", "Volksrepublik China", "CN", true).AddTranslation("en-EN", "China");
            yield return new Country(63, EContinent.Asia, "Indien", "Republik Indien", "IN", true).AddTranslation("en-EN", "India");
            yield return new Country(64, EContinent.Asia, "Indonesien", "Republik Indonesien", "ID", true).AddTranslation("en-EN", "Indonesia");
            yield return new Country(66, EContinent.Asia, "Iran", "Islamische Republik Iran", "IR", true).AddTranslation("en-EN", "Iran");
            yield return new Country(65, EContinent.Asia, "Irak", "Republik Irak", "IQ", true).AddTranslation("en-EN", "Iraq");
            yield return new Country(69, EContinent.Asia, "Israel", "Staat Israel", "IL", true).AddTranslation("en-EN", "Israel");
            yield return new Country(72, EContinent.Asia, "Japan", "Japan", "JP", true).AddTranslation("en-EN", "Japan");
            yield return new Country(74, EContinent.Asia, "Jordanien", "Haschemitisches Königreich Jordanien", "JO", true).AddTranslation("en-EN", "Jordan");
            yield return new Country(93, EContinent.Asia, "Kuwait", "Staat Kuwait", "KW", true).AddTranslation("en-EN", "Kuwait");
            yield return new Country(82, EContinent.Asia, "Kirgisistan", "Kirgisische Republik", "KG", true).AddTranslation("en-EN", "Kyrgyzstan");
            yield return new Country(94, EContinent.Asia, "Laos", "	Demokratische Volksrepublik Laos", "LA", true).AddTranslation("en-EN", "Lao, People’s Democratic Republic of");
            yield return new Country(97, EContinent.Asia, "Libanon", "Libanesische Republik", "LB", true).AddTranslation("en-EN", "Lebanon");
            yield return new Country(105, EContinent.Asia, "Malaysia", "Malaysia", "MY", true).AddTranslation("en-EN", "Malaysia");
            yield return new Country(106, EContinent.Asia, "Malediven", "Republik Malediven", "MV", true).AddTranslation("en-EN", "Maldives");
            yield return new Country(117, EContinent.Asia, "Mongolei", "Mongolei", "MN", true).AddTranslation("en-EN", "Mongolia");
            yield return new Country(120, EContinent.Asia, "Myanmar", "Republik der Union Myanmar", "", true).AddTranslation("en-EN", "MM");
            yield return new Country(123, EContinent.Asia, "Nepal", "Demokratische Bundesrepublik Nepal", "NP", true).AddTranslation("en-EN", "Nepal");
            yield return new Country(88, EContinent.Asia, "Nordkorea", "Demokratische Volksrepublik Korea", "KP", true).AddTranslation("en-EN", "Korea, Democratic People's Republic of (North Korea)");
            yield return new Country(133, EContinent.Asia, "Oman", "Sultanat Oman", "OM", true).AddTranslation("en-EN", "Oman");
            yield return new Country(136, EContinent.Asia, "Pakistan", "Islamische Republik Pakistan", "PK", true).AddTranslation("en-EN", "Pakistan");
            yield return new Country(137, EContinent.Asia, "Palästina", "Staat Palästina", "PS", false).AddTranslation("en-EN", "Palestine");
            yield return new Country(143, EContinent.Asia, "Philippinen", "Republik der Philippinen", "PH", true).AddTranslation("en-EN", "Philippines");
            yield return new Country(80, EContinent.Asia, "Katar", "Staat Katar", "QA", true).AddTranslation("en-EN", "Qatar");
            yield return new Country(154, EContinent.Asia, "Saudi-Arabien", "Königreich Saudi-Arabien", "SA", true).AddTranslation("en-EN", "Saudi Arabia");
            yield return new Country(162, EContinent.Asia, "Singapur", "Republik Singapur", "SG", true).AddTranslation("en-EN", "Singapore");
            yield return new Country(89, EContinent.Asia, "Südkorea", "Republik Korea", "KR", true).AddTranslation("en-EN", "Korea, Republic of (South Korea)");
            yield return new Country(168, EContinent.Asia, "Sri Lanka", "Demokratische Sozialistische Republik Sri Lanka", "LK", true).AddTranslation("en-EN", "Sri Lanka");
            yield return new Country(184, EContinent.Asia, "Syrien", "Arabische Republik Syrien", "SY", true).AddTranslation("en-EN", "Syria");
            yield return new Country(33, EContinent.Asia, "Taiwan ", "Republik China (Taiwan)", "TW", false).AddTranslation("en-EN", "Taiwan");
            yield return new Country(185, EContinent.Asia, "Tadschikistan", "Republik Tadschikistan", "TJ", true).AddTranslation("en-EN", "Tajikistan");
            yield return new Country(187, EContinent.Asia, "Thailand", "Königreich Thailand", "TH", true).AddTranslation("en-EN", "Thailand");
            yield return new Country(135, EContinent.Asia, "Osttimor", "Demokratische Republik Timor-Leste", "TL", true).AddTranslation("en-EN", "East Timor");
            yield return new Country(196, EContinent.Asia, "Turkmenistan", "Turkmenistan", "TM", true).AddTranslation("en-EN", "Turkmenistan");
            yield return new Country(206, EContinent.Asia, "Vereinigte Arabische Emirate", "Vereinigte Arabische Emirate", "AE", true).AddTranslation("en-EN", "United Arab Emirates");
            yield return new Country(202, EContinent.Asia, "Usbekistan", "Republik Usbekistan", "UZ", true).AddTranslation("en-EN", "Uzbekistan");
            yield return new Country(209, EContinent.Asia, "Vietnam", "Sozialistische Republik Vietnam", "VN", true).AddTranslation("en-EN", "Vietnam");
            yield return new Country(73, EContinent.Asia, "Jemen", "Republik Jemen", "YE", true).AddTranslation("en-EN", "Yemen");


            //Transition State
            yield return new Country(11, EContinent.TransitionState, "Armenien", "Republik Armenien", "AM", true).AddTranslation("en-EN", "Armenia");
            yield return new Country(13, EContinent.TransitionState, "Aserbaidschan", "Republik Aserbaidschan", "AZ", true).AddTranslation("en-EN", "Azerbaijan");
            yield return new Country(213, EContinent.TransitionState, "Zypern", "Republik Zypern", "CY", true).AddTranslation("en-EN", "Cyprus");
            yield return new Country(53, EContinent.TransitionState, "Georgien", "Georgien", "GE", true).AddTranslation("en-EN", "Georgia");
            yield return new Country(79, EContinent.TransitionState, "Kasachstan", "Republik Kasachstan", "KZ", true).AddTranslation("en-EN", "Kazakhstan");
            yield return new Country(148, EContinent.TransitionState, "Russland", "Russische Föderation", "RU", true).AddTranslation("en-EN", "Russian Federation");
            yield return new Country(195, EContinent.TransitionState, "Türkei", "Republik Türkei", "TR", true).AddTranslation("en-EN", "Turkey");


            // Europe
            yield return new Country(4, EContinent.Europe, "Albanien", "Republik Albanien", "AL", true).AddTranslation("en-EN", "Albania");
            yield return new Country(6, EContinent.Europe, "Andorra", "Fürstentum Andorra", "AD", true).AddTranslation("en-EN", "Andorra");
            yield return new Country(134, EContinent.Europe, "Österreich", "Republik Österreich", "AT", true).AddTranslation("en-EN", "Austria");
            yield return new Country(210, EContinent.Europe, "Belarus", "Republik Belarus", "BY", true).AddTranslation("en-EN", "Belarus");
            yield return new Country(20, EContinent.Europe, "Belgien", "Königreich Belgien", "BE", true).AddTranslation("en-EN", "Belgium");
            yield return new Country(25, EContinent.Europe, "Bosnien und Herzegowina", "Bosnien und Herzegowina", "BA", true).AddTranslation("en-EN", "Bosnia and Herzegovina");
            yield return new Country(29, EContinent.Europe, "Bulgarien", "Republik Bulgarien", "BG", true).AddTranslation("en-EN", "Bulgaria");
            yield return new Country(91, EContinent.Europe, "Kroatien", "Republik Kroatien", "HR", true).AddTranslation("en-EN", "Croatia");
            yield return new Country(193, EContinent.Europe, "Tschechien", "Tschechische Republik", "CZ", true).AddTranslation("en-EN", "Czechia");
            yield return new Country(37, EContinent.Europe, "Dänemark", "Königreich Dänemark", "DK", true).AddTranslation("en-EN", "Denmark");
            yield return new Country(46, EContinent.Europe, "Estland", "Republik Estland", "EE", true).AddTranslation("en-EN", "Estonia");
            yield return new Country(49, EContinent.Europe, "Finnland", "Republik Finnland", "FI", true).AddTranslation("en-EN", "Finland");
            yield return new Country(50, EContinent.Europe, "Frankreich", "Französische Republik", "FR", true).AddTranslation("en-EN", "France");
            yield return new Country(38, EContinent.Europe, "Deutschland", "Bundesrepublik Deutschland", "DE", true).AddTranslation("en-EN", "Germany");
            yield return new Country(56, EContinent.Europe, "Griechenland", "Hellenische Republik", "GR", true).AddTranslation("en-EN", "Greece");
            yield return new Country(200, EContinent.Europe, "Ungarn", "Ungarn", "HU", true).AddTranslation("en-EN", "Hungary");
            yield return new Country(68, EContinent.Europe, "Island", "Republik Island", "IS", true).AddTranslation("en-EN", "Iceland");
            yield return new Country(67, EContinent.Europe, "Irland", "Irland", "IE", true).AddTranslation("en-EN", "Ireland");
            yield return new Country(70, EContinent.Europe, "Italien", "	Italienische Republik", "IT", true).AddTranslation("en-EN", "Italy");
            yield return new Country(90, EContinent.Europe, "Kosovo", "Republik Kosovo", "XK", false).AddTranslation("en-EN", "Kosovo");
            yield return new Country(96, EContinent.Europe, "Lettland", "Republik Lettland", "LV", true).AddTranslation("en-EN", "Latvia");
            yield return new Country(100, EContinent.Europe, "Liechtenstein", "Fürstentum Liechtenstein", "LI", true).AddTranslation("en-EN", "Liechtenstein");
            yield return new Country(101, EContinent.Europe, "Litauen", "Republik Litauen", "LT", true).AddTranslation("en-EN", "Lithuania");
            yield return new Country(102, EContinent.Europe, "Luxemburg", "Großherzogtum Luxemburg", "LU", true).AddTranslation("en-EN", "Luxembourg");
            yield return new Country(108, EContinent.Europe, "Malta", "Republik Malta", "	MT", true).AddTranslation("en-EN", "Malta");
            yield return new Country(115, EContinent.Europe, "Moldau", "Republik Moldau", "MD", true).AddTranslation("en-EN", "Moldova");
            yield return new Country(116, EContinent.Europe, "Monaco", "Fürstentum Monaco", "MC", true).AddTranslation("en-EN", "Monaco");
            yield return new Country(118, EContinent.Europe, "Montenegro", "Montenegro", "ME", true).AddTranslation("en-EN", "Montenegro");
            yield return new Country(126, EContinent.Europe, "Niederlande", "Königreich der Niederlande", "NL", true).AddTranslation("en-EN", "Netherlands");
            yield return new Country(130, EContinent.Europe, "Nordmazedonien", "Republik Nordmazedonien", "MK", true).AddTranslation("en-EN", "North Macedonia");
            yield return new Country(132, EContinent.Europe, "Norwegen", "Königreich Norwegen", "NO", true).AddTranslation("en-EN", "Norway");
            yield return new Country(144, EContinent.Europe, "Polen", "Republik Polen", "PL", true).AddTranslation("en-EN", "Poland");
            yield return new Country(145, EContinent.Europe, "Portugal", "Portugiesische Republik", "PT", true).AddTranslation("en-EN", "Portugal");
            yield return new Country(147, EContinent.Europe, "Rumänien", "Rumänien", "RO", true).AddTranslation("en-EN", "Romania");
            yield return new Country(152, EContinent.Europe, "San Marino", "Republik San Marino", "SM", true).AddTranslation("en-EN", "San Marino");
            yield return new Country(158, EContinent.Europe, "Serbien", "Republik Serbien", "RS", true).AddTranslation("en-EN", "Serbia");
            yield return new Country(163, EContinent.Europe, "Slowakei", "	Slowakische Republik", "SK", true).AddTranslation("en-EN", "Slovakia");
            yield return new Country(164, EContinent.Europe, "Slowenien", "Republik Slowenien", "SI", true).AddTranslation("en-EN", "Slovenia");
            yield return new Country(167, EContinent.Europe, "Spanien", "Königreich Spanien", "ES", true).AddTranslation("en-EN", "Spain");
            yield return new Country(155, EContinent.Europe, "Schweden", "Königreich Schweden", "SE", true).AddTranslation("en-EN", "Sweden");
            yield return new Country(156, EContinent.Europe, "Schweiz", "Schweizerische Eid­genossen­schaft", "CH", true).AddTranslation("en-EN", "Switzerland");
            yield return new Country(199, EContinent.Europe, "Ukraine", "Ukraine", "UA", true).AddTranslation("en-EN", "Ukraine");
            yield return new Country(208, EContinent.Europe, "Vereinigtes Königreich", "Vereinigtes Königreich Großbritannien und Nordirland", "GB", true).AddTranslation("en-EN", "United Kingdom");
            yield return new Country(204, EContinent.Europe, "Vatikanstadt", "Staat Vatikanstadt", "VA", true).AddTranslation("en-EN", "Vatican City");


            yield return new Country(1, EContinent.Europe, "Abchasien", "Republik Abchasient", "", false).AddTranslation("en-EN", "Abkhazia");
            
            // North America
            yield return new Country(8, EContinent.NorthAmerica, "Antigua und Barbuda", "Antigua und Barbuda", "AG", true).AddTranslation("en-EN", "Antigua and Barbuda");
            yield return new Country(16, EContinent.NorthAmerica, "Bahamas", "Commonwealth der Bahamas", "BS", true).AddTranslation("en-EN", "Bahamas");
            yield return new Country(19, EContinent.NorthAmerica, "Barbados", "Barbados", "BB", true).AddTranslation("en-EN", "Barbados");
            yield return new Country(21, EContinent.NorthAmerica, "Belize", "Belize", "BZ", true).AddTranslation("en-EN", "Belize");
            yield return new Country(77, EContinent.NorthAmerica, "Kanada", "Kanada", "CA", true).AddTranslation("en-EN", "Canada");
            yield return new Country(36, EContinent.NorthAmerica, "Costa Rica", "Republik Costa Rica", "	CR", true).AddTranslation("en-EN", "Costa Rica");
            yield return new Country(92, EContinent.NorthAmerica, "Kuba", "Republik Kuba", "CU", true).AddTranslation("en-EN", "Cuba");
            yield return new Country(39, EContinent.NorthAmerica, "Dominica", "Commonwealth Dominica", "DM", true).AddTranslation("en-EN", "Dominica");
            yield return new Country(40, EContinent.NorthAmerica, "Dominikanische Republik", "Dominikanische Republik", "DO", true).AddTranslation("en-EN", "Dominican Republic");
            yield return new Country(43, EContinent.NorthAmerica, "El Salvador", "Republik El Salvador", "SV", true).AddTranslation("en-EN", "El Salvador");
            yield return new Country(55, EContinent.NorthAmerica, "Grenada", "Grenada", "GD", true).AddTranslation("en-EN", "Grenada");
            yield return new Country(57, EContinent.NorthAmerica, "Guatemala", "Republik Guatemala", "GT", true).AddTranslation("en-EN", "Guatemala");
            yield return new Country(61, EContinent.NorthAmerica, "Haiti", "Republik Haiti", "HT", true).AddTranslation("en-EN", "Haiti");
            yield return new Country(62, EContinent.NorthAmerica, "Honduras", "Republik Honduras", "HN", true).AddTranslation("en-EN", "Honduras");
            yield return new Country(71, EContinent.NorthAmerica, "Jamaika", "Jamaika", "JM", true).AddTranslation("en-EN", "Jamaica");
            yield return new Country(113, EContinent.NorthAmerica, "Mexiko", "Vereinigte Mexikanische Staaten", "MX", true).AddTranslation("en-EN", "Mexico");
            yield return new Country(125, EContinent.NorthAmerica, "Nicaragua", "Republik Nicaragua", "NI", true).AddTranslation("en-EN", "Nicaragua");
            yield return new Country(139, EContinent.NorthAmerica, "Panama", "Republik Panama", "PA", true).AddTranslation("en-EN", "Panama");
            yield return new Country(169, EContinent.NorthAmerica, "St. Kitts und Nevis", "Föderation St. Kitts und Nevis", "KN", true).AddTranslation("en-EN", "Saint Kitts and Nevis");
            yield return new Country(170, EContinent.NorthAmerica, "St. Lucia", "St. Lucia", "LC", true).AddTranslation("en-EN", "Saint Lucia");
            yield return new Country(178, EContinent.NorthAmerica, "St. Vincent und die Grenadinen", "St. Vincent und die Grenadinen", "VC", true).AddTranslation("en-EN", "Saint Vincent and the Grenadines");
            yield return new Country(191, EContinent.NorthAmerica, "Trinidad und Tobago", "Republik Trinidad und Tobago", "TT", true).AddTranslation("en-EN", "Trinidad and Tobago");
            yield return new Country(207, EContinent.NorthAmerica, "Vereinigte Staaten", "Vereinigte Staaten von Amerika", "US", true).AddTranslation("en-EN", "United States");


            // South America
            yield return new Country(10, EContinent.SouthAmerica, "Argentinien", "Argentinische Republik", "AR", true).AddTranslation("en-EN", "Argentina");
            yield return new Country(24, EContinent.SouthAmerica, "Bolivien", "Plurinationaler Staat Bolivien", "BO", true).AddTranslation("en-EN", "Bolivia");
            yield return new Country(27, EContinent.SouthAmerica, "Brasilien", "Föderative Republik Brasilien", "BR", true).AddTranslation("en-EN", "Brazil");
            yield return new Country(32, EContinent.SouthAmerica, "Chile", "Republik Chile", "CL", true).AddTranslation("en-EN", "Chile");
            yield return new Country(84, EContinent.SouthAmerica, "Kolumbien", "Republik Kolumbien", "CO", true).AddTranslation("en-EN", "Colombia");
            yield return new Country(42, EContinent.SouthAmerica, "Ecuador", "Republik Ecuador", "EC", true).AddTranslation("en-EN", "Ecuador");
            yield return new Country(60, EContinent.SouthAmerica, "Guyana", "Kooperative Republik Guyana", "GY", true).AddTranslation("en-EN", "Guyana");
            yield return new Country(141, EContinent.SouthAmerica, "Paraguay", "Republik Paraguay", "PY", true).AddTranslation("en-EN", "Paraguay");
            yield return new Country(142, EContinent.SouthAmerica, "Peru", "Republik Peru", "PE", true).AddTranslation("en-EN", "Peru");
            yield return new Country(183, EContinent.SouthAmerica, "Suriname", "Republik Suriname", "SR", true).AddTranslation("en-EN", "Suriname");
            yield return new Country(201, EContinent.SouthAmerica, "Uruguay", "Republik Östlich des Uruguay", "UY", true).AddTranslation("en-EN", "Uruguay");
            yield return new Country(205, EContinent.SouthAmerica, "Venezuela", "Bolivarische Republik Venezuela", "VE", true).AddTranslation("en-EN", "Venezuela");


            // Oceania
            yield return new Country(15, EContinent.Oceania, "Australien", "Australien", "AU", true).AddTranslation("en-EN", "Australia");
            yield return new Country(48, EContinent.Oceania, "Fidschi", "Republik Fidschi", "FJ", true).AddTranslation("en-EN", "Fiji");
            yield return new Country(83, EContinent.Oceania, "Kiribati", "Republik Kiribati", "KI", true).AddTranslation("en-EN", "Kiribati");
            yield return new Country(110, EContinent.Oceania, "Marshallinseln", "Republik Marshallinseln", "MH", true).AddTranslation("en-EN", "Marshall Islands");
            yield return new Country(114, EContinent.Oceania, "Mikronesien", "Föderierte Staaten von Mikronesien", "FM", true).AddTranslation("en-EN", "Micronesia, Federal States of");
            yield return new Country(122, EContinent.Oceania, "Nauru", "Republik Nauru", "NR", true).AddTranslation("en-EN", "Nauru");
            yield return new Country(124, EContinent.Oceania, "Neuseeland", "Neuseeland", "NZ", true).AddTranslation("en-EN", "New Zealand");
            yield return new Country(138, EContinent.Oceania, "Palau", "Republik Palau", "PW", true).AddTranslation("en-EN", "Palau");
            yield return new Country(140, EContinent.Oceania, "Papua-Neuguinea", "Unabhängiger Staat Papua-Neuguinea", "PG", true).AddTranslation("en-EN", "Papua New Guinea");
            yield return new Country(151, EContinent.Oceania, "Samoa", "Unabhängiger Staat Samoa", "WS", true).AddTranslation("en-EN", "Samoa");
            yield return new Country(149, EContinent.Oceania, "Salomonen", "Salomonen", "SB", true).AddTranslation("en-EN", "Solomon Islands");
            yield return new Country(189, EContinent.Oceania, "Tonga", "Königreich Tonga", "TO", true).AddTranslation("en-EN", "Tonga");
            yield return new Country(197, EContinent.Oceania, "Tuvalu", "Tuvalu", "TV", true).AddTranslation("en-EN", "Tuvalu");
            yield return new Country(203, EContinent.Oceania, "Vanuatu", "Republik Vanuatu", "VU", true).AddTranslation("en-EN", "Vanuatu");


            yield return new Country(12, EContinent.NotSet, "Arzach", "Republik Arzach", "", false).AddTranslation("en-EN", "Republic of Artsakh");
            yield return new Country(35, EContinent.NotSet, "Cookinseln", "Cookinseln", "CK", false).AddTranslation("en-EN", "Cook Islands");
            yield return new Country(45, EContinent.NotSet, "Eritrea", "Staat Eritrea", "ER", true).AddTranslation("en-EN", "Eritrea");
            yield return new Country(47, EContinent.NotSet, "Eswatini", "Königreich Eswatini", "SZ", true).AddTranslation("en-EN", "Eswatini");
            yield return new Country(129, EContinent.NotSet, "Niue", "Niue", "NU", false).AddTranslation("en-EN", "Niue");
            yield return new Country(131, EContinent.NotSet, "Nordzypern", "Türkische Republik Nordzypern", "", false).AddTranslation("en-EN", "Turkish Republic of Northern Cyprus");
            yield return new Country(166, EContinent.NotSet, "Somaliland", "Republik Somaliland", "", false).AddTranslation("en-EN", "Somaliland");
            yield return new Country(181, EContinent.NotSet, "Südossetien", "Südossetien", "", false).AddTranslation("en-EN", "South Ossetia");
            yield return new Country(190, EContinent.NotSet, "Transnistrien", "Transnistrische Moldauische Republik", "", false).AddTranslation("en-EN", "Transnistria");
            yield return new Country(211, EContinent.NotSet, "Westsahara", "Demokratische Arabische Republik Sahara", "EH", false).AddTranslation("en-EN", "Western Sahara", "Sahrawi Arab Democratic Republic");
        }
    }
}
