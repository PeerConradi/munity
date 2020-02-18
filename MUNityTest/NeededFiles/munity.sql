/*
Navicat MySQL Data Transfer

Source Server         : localhost mariadb
Source Server Version : 50505
Source Host           : localhost:3306
Source Database       : munity-test

Target Server Type    : MYSQL
Target Server Version : 50505
File Encoding         : 65001

Date: 2020-02-18 16:10:46
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for admin
-- ----------------------------
DROP TABLE IF EXISTS `admin`;
CREATE TABLE `admin` (
  `userid` varchar(255) NOT NULL,
  `rank` int(11) DEFAULT NULL,
  PRIMARY KEY (`userid`),
  CONSTRAINT `useradminlink` FOREIGN KEY (`userid`) REFERENCES `user` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of admin
-- ----------------------------

-- ----------------------------
-- Table structure for auth
-- ----------------------------
DROP TABLE IF EXISTS `auth`;
CREATE TABLE `auth` (
  `authkey` varchar(255) NOT NULL,
  `userid` varchar(255) DEFAULT NULL,
  `createdate` datetime DEFAULT NULL,
  `expiredate` datetime DEFAULT NULL,
  PRIMARY KEY (`authkey`),
  KEY `userkeylink` (`userid`),
  CONSTRAINT `userkeylink` FOREIGN KEY (`userid`) REFERENCES `user` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of auth
-- ----------------------------

-- ----------------------------
-- Table structure for committee
-- ----------------------------
DROP TABLE IF EXISTS `committee`;
CREATE TABLE `committee` (
  `id` varchar(255) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `fullname` varchar(255) DEFAULT NULL,
  `abbreviation` varchar(255) DEFAULT NULL,
  `article` varchar(255) DEFAULT NULL,
  `conferenceid` varchar(255) DEFAULT NULL,
  `language` varchar(255) DEFAULT NULL,
  `resolutlycommittee` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `committee_conference` (`conferenceid`),
  KEY `head` (`resolutlycommittee`),
  CONSTRAINT `committee_conference` FOREIGN KEY (`conferenceid`) REFERENCES `conference` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `head` FOREIGN KEY (`resolutlycommittee`) REFERENCES `committee` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of committee
-- ----------------------------

-- ----------------------------
-- Table structure for committee_lead
-- ----------------------------
DROP TABLE IF EXISTS `committee_lead`;
CREATE TABLE `committee_lead` (
  `committeeid` varchar(255) DEFAULT NULL,
  `userid` varchar(255) DEFAULT NULL,
  `rolename` varchar(255) DEFAULT NULL,
  KEY `committee` (`committeeid`),
  KEY `user` (`userid`),
  CONSTRAINT `committee` FOREIGN KEY (`committeeid`) REFERENCES `committee` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `user` FOREIGN KEY (`userid`) REFERENCES `user` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of committee_lead
-- ----------------------------

-- ----------------------------
-- Table structure for conference
-- ----------------------------
DROP TABLE IF EXISTS `conference`;
CREATE TABLE `conference` (
  `id` varchar(255) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `fullname` varchar(255) DEFAULT NULL,
  `abbreviation` varchar(255) DEFAULT NULL,
  `startdate` datetime DEFAULT NULL,
  `enddate` datetime DEFAULT NULL,
  `secretarygeneraltitle` varchar(255) DEFAULT NULL,
  `secretarygeneralname` varchar(255) DEFAULT NULL,
  `creationdate` datetime DEFAULT NULL,
  `creationuser` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of conference
-- ----------------------------

-- ----------------------------
-- Table structure for conference_delegation
-- ----------------------------
DROP TABLE IF EXISTS `conference_delegation`;
CREATE TABLE `conference_delegation` (
  `linkid` int(11) NOT NULL AUTO_INCREMENT,
  `conference_id` varchar(255) DEFAULT NULL,
  `delegation_id` varchar(255) DEFAULT NULL,
  `mincount` int(11) DEFAULT 1,
  `maxcount` int(11) DEFAULT 1,
  PRIMARY KEY (`linkid`),
  KEY `conference` (`conference_id`),
  KEY `delegation` (`delegation_id`),
  CONSTRAINT `conference` FOREIGN KEY (`conference_id`) REFERENCES `conference` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `delegation` FOREIGN KEY (`delegation_id`) REFERENCES `delegation` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of conference_delegation
-- ----------------------------

-- ----------------------------
-- Table structure for conference_team
-- ----------------------------
DROP TABLE IF EXISTS `conference_team`;
CREATE TABLE `conference_team` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `conferenceid` varchar(255) DEFAULT NULL,
  `userid` varchar(255) DEFAULT NULL,
  `rolename` varchar(255) DEFAULT NULL,
  `parentrole` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `conferenceteamconnection` (`conferenceid`),
  KEY `userteamconnection` (`userid`),
  CONSTRAINT `conferenceteamconnection` FOREIGN KEY (`conferenceid`) REFERENCES `conference` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `userteamconnection` FOREIGN KEY (`userid`) REFERENCES `user` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of conference_team
-- ----------------------------

-- ----------------------------
-- Table structure for conference_user_auth
-- ----------------------------
DROP TABLE IF EXISTS `conference_user_auth`;
CREATE TABLE `conference_user_auth` (
  `conferenceid` varchar(255) NOT NULL,
  `userid` varchar(255) NOT NULL,
  `CanOpen` bit(1) DEFAULT NULL,
  `CanEdit` bit(1) DEFAULT NULL,
  `CanRemove` bit(1) DEFAULT NULL,
  PRIMARY KEY (`conferenceid`,`userid`),
  KEY `conferenceAuthUser` (`userid`),
  CONSTRAINT `conferenceAuthConference` FOREIGN KEY (`conferenceid`) REFERENCES `conference` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `conferenceAuthUser` FOREIGN KEY (`userid`) REFERENCES `user` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of conference_user_auth
-- ----------------------------

-- ----------------------------
-- Table structure for countries_de
-- ----------------------------
DROP TABLE IF EXISTS `countries_de`;
CREATE TABLE `countries_de` (
  `id` int(11) NOT NULL,
  `name` varchar(75) NOT NULL DEFAULT '',
  `alpha_2` char(2) NOT NULL DEFAULT '',
  `alpha_3` char(3) NOT NULL DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of countries_de
-- ----------------------------
INSERT INTO `countries_de` VALUES ('4', 'Afghanistan', 'af', 'afg');
INSERT INTO `countries_de` VALUES ('8', 'Albanien', 'al', 'alb');
INSERT INTO `countries_de` VALUES ('12', 'Algerien', 'dz', 'dza');
INSERT INTO `countries_de` VALUES ('20', 'Andorra', 'ad', 'and');
INSERT INTO `countries_de` VALUES ('24', 'Angola', 'ao', 'ago');
INSERT INTO `countries_de` VALUES ('28', 'Antigua und Barbuda', 'ag', 'atg');
INSERT INTO `countries_de` VALUES ('31', 'Aserbaidschan', 'az', 'aze');
INSERT INTO `countries_de` VALUES ('32', 'Argentinien', 'ar', 'arg');
INSERT INTO `countries_de` VALUES ('36', 'Australien', 'au', 'aus');
INSERT INTO `countries_de` VALUES ('40', 'Österreich', 'at', 'aut');
INSERT INTO `countries_de` VALUES ('44', 'Bahamas', 'bs', 'bhs');
INSERT INTO `countries_de` VALUES ('48', 'Bahrain', 'bh', 'bhr');
INSERT INTO `countries_de` VALUES ('50', 'Bangladesch', 'bd', 'bgd');
INSERT INTO `countries_de` VALUES ('51', 'Armenien', 'am', 'arm');
INSERT INTO `countries_de` VALUES ('52', 'Barbados', 'bb', 'brb');
INSERT INTO `countries_de` VALUES ('56', 'Belgien', 'be', 'bel');
INSERT INTO `countries_de` VALUES ('64', 'Bhutan', 'bt', 'btn');
INSERT INTO `countries_de` VALUES ('68', 'Bolivien', 'bo', 'bol');
INSERT INTO `countries_de` VALUES ('70', 'Bosnien und Herzegowina', 'ba', 'bih');
INSERT INTO `countries_de` VALUES ('72', 'Botswana', 'bw', 'bwa');
INSERT INTO `countries_de` VALUES ('76', 'Brasilien', 'br', 'bra');
INSERT INTO `countries_de` VALUES ('84', 'Belize', 'bz', 'blz');
INSERT INTO `countries_de` VALUES ('90', 'Salomonen', 'sb', 'slb');
INSERT INTO `countries_de` VALUES ('96', 'Brunei', 'bn', 'brn');
INSERT INTO `countries_de` VALUES ('100', 'Bulgarien', 'bg', 'bgr');
INSERT INTO `countries_de` VALUES ('104', 'Myanmar', 'mm', 'mmr');
INSERT INTO `countries_de` VALUES ('108', 'Burundi', 'bi', 'bdi');
INSERT INTO `countries_de` VALUES ('112', 'Weißrussland', 'by', 'blr');
INSERT INTO `countries_de` VALUES ('116', 'Kambodscha', 'kh', 'khm');
INSERT INTO `countries_de` VALUES ('120', 'Kamerun', 'cm', 'cmr');
INSERT INTO `countries_de` VALUES ('124', 'Kanada', 'ca', 'can');
INSERT INTO `countries_de` VALUES ('132', 'Kap Verde', 'cv', 'cpv');
INSERT INTO `countries_de` VALUES ('140', 'Zentral­afrikanische Republik', 'cf', 'caf');
INSERT INTO `countries_de` VALUES ('144', 'Sri Lanka', 'lk', 'lka');
INSERT INTO `countries_de` VALUES ('148', 'Tschad', 'td', 'tcd');
INSERT INTO `countries_de` VALUES ('152', 'Chile', 'cl', 'chl');
INSERT INTO `countries_de` VALUES ('156', 'Volksrepublik China', 'cn', 'chn');
INSERT INTO `countries_de` VALUES ('170', 'Kolumbien', 'co', 'col');
INSERT INTO `countries_de` VALUES ('174', 'Komoren', 'km', 'com');
INSERT INTO `countries_de` VALUES ('178', 'Kongo, Republik', 'cg', 'cog');
INSERT INTO `countries_de` VALUES ('180', 'Kongo, Demokratische Republik', 'cd', 'cod');
INSERT INTO `countries_de` VALUES ('188', 'Costa Rica', 'cr', 'cri');
INSERT INTO `countries_de` VALUES ('191', 'Kroatien', 'hr', 'hrv');
INSERT INTO `countries_de` VALUES ('192', 'Kuba', 'cu', 'cub');
INSERT INTO `countries_de` VALUES ('196', 'Zypern', 'cy', 'cyp');
INSERT INTO `countries_de` VALUES ('203', 'Tschechien', 'cz', 'cze');
INSERT INTO `countries_de` VALUES ('204', 'Benin', 'bj', 'ben');
INSERT INTO `countries_de` VALUES ('208', 'Dänemark', 'dk', 'dnk');
INSERT INTO `countries_de` VALUES ('212', 'Dominica', 'dm', 'dma');
INSERT INTO `countries_de` VALUES ('214', 'Dominikanische Republik', 'do', 'dom');
INSERT INTO `countries_de` VALUES ('218', 'Ecuador', 'ec', 'ecu');
INSERT INTO `countries_de` VALUES ('222', 'El Salvador', 'sv', 'slv');
INSERT INTO `countries_de` VALUES ('226', 'Äquatorialguinea', 'gq', 'gnq');
INSERT INTO `countries_de` VALUES ('231', 'Äthiopien', 'et', 'eth');
INSERT INTO `countries_de` VALUES ('232', 'Eritrea', 'er', 'eri');
INSERT INTO `countries_de` VALUES ('233', 'Estland', 'ee', 'est');
INSERT INTO `countries_de` VALUES ('242', 'Fidschi', 'fj', 'fji');
INSERT INTO `countries_de` VALUES ('246', 'Finnland', 'fi', 'fin');
INSERT INTO `countries_de` VALUES ('250', 'Frankreich', 'fr', 'fra');
INSERT INTO `countries_de` VALUES ('262', 'Dschibuti', 'dj', 'dji');
INSERT INTO `countries_de` VALUES ('266', 'Gabun', 'ga', 'gab');
INSERT INTO `countries_de` VALUES ('268', 'Georgien', 'ge', 'geo');
INSERT INTO `countries_de` VALUES ('270', 'Gambia', 'gm', 'gmb');
INSERT INTO `countries_de` VALUES ('276', 'Deutschland', 'de', 'deu');
INSERT INTO `countries_de` VALUES ('288', 'Ghana', 'gh', 'gha');
INSERT INTO `countries_de` VALUES ('296', 'Kiribati', 'ki', 'kir');
INSERT INTO `countries_de` VALUES ('300', 'Griechenland', 'gr', 'grc');
INSERT INTO `countries_de` VALUES ('308', 'Grenada', 'gd', 'grd');
INSERT INTO `countries_de` VALUES ('320', 'Guatemala', 'gt', 'gtm');
INSERT INTO `countries_de` VALUES ('324', 'Guinea', 'gn', 'gin');
INSERT INTO `countries_de` VALUES ('328', 'Guyana', 'gy', 'guy');
INSERT INTO `countries_de` VALUES ('332', 'Haiti', 'ht', 'hti');
INSERT INTO `countries_de` VALUES ('340', 'Honduras', 'hn', 'hnd');
INSERT INTO `countries_de` VALUES ('348', 'Ungarn', 'hu', 'hun');
INSERT INTO `countries_de` VALUES ('352', 'Island', 'is', 'isl');
INSERT INTO `countries_de` VALUES ('356', 'Indien', 'in', 'ind');
INSERT INTO `countries_de` VALUES ('360', 'Indonesien', 'id', 'idn');
INSERT INTO `countries_de` VALUES ('364', 'Iran', 'ir', 'irn');
INSERT INTO `countries_de` VALUES ('368', 'Irak', 'iq', 'irq');
INSERT INTO `countries_de` VALUES ('372', 'Irland', 'ie', 'irl');
INSERT INTO `countries_de` VALUES ('376', 'Israel', 'il', 'isr');
INSERT INTO `countries_de` VALUES ('380', 'Italien', 'it', 'ita');
INSERT INTO `countries_de` VALUES ('384', 'Elfenbeinküste', 'ci', 'civ');
INSERT INTO `countries_de` VALUES ('388', 'Jamaika', 'jm', 'jam');
INSERT INTO `countries_de` VALUES ('392', 'Japan', 'jp', 'jpn');
INSERT INTO `countries_de` VALUES ('398', 'Kasachstan', 'kz', 'kaz');
INSERT INTO `countries_de` VALUES ('400', 'Jordanien', 'jo', 'jor');
INSERT INTO `countries_de` VALUES ('404', 'Kenia', 'ke', 'ken');
INSERT INTO `countries_de` VALUES ('408', 'Korea, Nord', 'kp', 'prk');
INSERT INTO `countries_de` VALUES ('410', 'Korea, Süd', 'kr', 'kor');
INSERT INTO `countries_de` VALUES ('414', 'Kuwait', 'kw', 'kwt');
INSERT INTO `countries_de` VALUES ('417', 'Kirgisistan', 'kg', 'kgz');
INSERT INTO `countries_de` VALUES ('418', 'Laos', 'la', 'lao');
INSERT INTO `countries_de` VALUES ('422', 'Libanon', 'lb', 'lbn');
INSERT INTO `countries_de` VALUES ('426', 'Lesotho', 'ls', 'lso');
INSERT INTO `countries_de` VALUES ('428', 'Lettland', 'lv', 'lva');
INSERT INTO `countries_de` VALUES ('430', 'Liberia', 'lr', 'lbr');
INSERT INTO `countries_de` VALUES ('434', 'Libyen', 'ly', 'lby');
INSERT INTO `countries_de` VALUES ('438', 'Liechtenstein', 'li', 'lie');
INSERT INTO `countries_de` VALUES ('440', 'Litauen', 'lt', 'ltu');
INSERT INTO `countries_de` VALUES ('442', 'Luxemburg', 'lu', 'lux');
INSERT INTO `countries_de` VALUES ('450', 'Madagaskar', 'mg', 'mdg');
INSERT INTO `countries_de` VALUES ('454', 'Malawi', 'mw', 'mwi');
INSERT INTO `countries_de` VALUES ('458', 'Malaysia', 'my', 'mys');
INSERT INTO `countries_de` VALUES ('462', 'Malediven', 'mv', 'mdv');
INSERT INTO `countries_de` VALUES ('466', 'Mali', 'ml', 'mli');
INSERT INTO `countries_de` VALUES ('470', 'Malta', 'mt', 'mlt');
INSERT INTO `countries_de` VALUES ('478', 'Mauretanien', 'mr', 'mrt');
INSERT INTO `countries_de` VALUES ('480', 'Mauritius', 'mu', 'mus');
INSERT INTO `countries_de` VALUES ('484', 'Mexiko', 'mx', 'mex');
INSERT INTO `countries_de` VALUES ('492', 'Monaco', 'mc', 'mco');
INSERT INTO `countries_de` VALUES ('496', 'Mongolei', 'mn', 'mng');
INSERT INTO `countries_de` VALUES ('498', 'Moldau', 'md', 'mda');
INSERT INTO `countries_de` VALUES ('499', 'Montenegro', 'me', 'mne');
INSERT INTO `countries_de` VALUES ('504', 'Marokko', 'ma', 'mar');
INSERT INTO `countries_de` VALUES ('508', 'Mosambik', 'mz', 'moz');
INSERT INTO `countries_de` VALUES ('512', 'Oman', 'om', 'omn');
INSERT INTO `countries_de` VALUES ('516', 'Namibia', 'na', 'nam');
INSERT INTO `countries_de` VALUES ('520', 'Nauru', 'nr', 'nru');
INSERT INTO `countries_de` VALUES ('524', 'Nepal', 'np', 'npl');
INSERT INTO `countries_de` VALUES ('528', 'Niederlande', 'nl', 'nld');
INSERT INTO `countries_de` VALUES ('548', 'Vanuatu', 'vu', 'vut');
INSERT INTO `countries_de` VALUES ('554', 'Neuseeland', 'nz', 'nzl');
INSERT INTO `countries_de` VALUES ('558', 'Nicaragua', 'ni', 'nic');
INSERT INTO `countries_de` VALUES ('562', 'Niger', 'ne', 'ner');
INSERT INTO `countries_de` VALUES ('566', 'Nigeria', 'ng', 'nga');
INSERT INTO `countries_de` VALUES ('578', 'Norwegen', 'no', 'nor');
INSERT INTO `countries_de` VALUES ('583', 'Mikronesien', 'fm', 'fsm');
INSERT INTO `countries_de` VALUES ('584', 'Marshallinseln', 'mh', 'mhl');
INSERT INTO `countries_de` VALUES ('585', 'Palau', 'pw', 'plw');
INSERT INTO `countries_de` VALUES ('586', 'Pakistan', 'pk', 'pak');
INSERT INTO `countries_de` VALUES ('591', 'Panama', 'pa', 'pan');
INSERT INTO `countries_de` VALUES ('598', 'Papua-Neuguinea', 'pg', 'png');
INSERT INTO `countries_de` VALUES ('600', 'Paraguay', 'py', 'pry');
INSERT INTO `countries_de` VALUES ('604', 'Peru', 'pe', 'per');
INSERT INTO `countries_de` VALUES ('608', 'Philippinen', 'ph', 'phl');
INSERT INTO `countries_de` VALUES ('616', 'Polen', 'pl', 'pol');
INSERT INTO `countries_de` VALUES ('620', 'Portugal', 'pt', 'prt');
INSERT INTO `countries_de` VALUES ('624', 'Guinea-Bissau', 'gw', 'gnb');
INSERT INTO `countries_de` VALUES ('626', 'Osttimor', 'tl', 'tls');
INSERT INTO `countries_de` VALUES ('634', 'Katar', 'qa', 'qat');
INSERT INTO `countries_de` VALUES ('642', 'Rumänien', 'ro', 'rou');
INSERT INTO `countries_de` VALUES ('643', 'Russland', 'ru', 'rus');
INSERT INTO `countries_de` VALUES ('646', 'Ruanda', 'rw', 'rwa');
INSERT INTO `countries_de` VALUES ('659', 'St. Kitts und Nevis', 'kn', 'kna');
INSERT INTO `countries_de` VALUES ('662', 'St. Lucia', 'lc', 'lca');
INSERT INTO `countries_de` VALUES ('670', 'St. Vincent und die Grenadinen', 'vc', 'vct');
INSERT INTO `countries_de` VALUES ('674', 'San Marino', 'sm', 'smr');
INSERT INTO `countries_de` VALUES ('678', 'São Tomé und Príncipe', 'st', 'stp');
INSERT INTO `countries_de` VALUES ('682', 'Saudi-Arabien', 'sa', 'sau');
INSERT INTO `countries_de` VALUES ('686', 'Senegal', 'sn', 'sen');
INSERT INTO `countries_de` VALUES ('688', 'Serbien', 'rs', 'srb');
INSERT INTO `countries_de` VALUES ('690', 'Seychellen', 'sc', 'syc');
INSERT INTO `countries_de` VALUES ('694', 'Sierra Leone', 'sl', 'sle');
INSERT INTO `countries_de` VALUES ('702', 'Singapur', 'sg', 'sgp');
INSERT INTO `countries_de` VALUES ('703', 'Slowakei', 'sk', 'svk');
INSERT INTO `countries_de` VALUES ('704', 'Vietnam', 'vn', 'vnm');
INSERT INTO `countries_de` VALUES ('705', 'Slowenien', 'si', 'svn');
INSERT INTO `countries_de` VALUES ('706', 'Somalia', 'so', 'som');
INSERT INTO `countries_de` VALUES ('710', 'Südafrika', 'za', 'zaf');
INSERT INTO `countries_de` VALUES ('716', 'Simbabwe', 'zw', 'zwe');
INSERT INTO `countries_de` VALUES ('724', 'Spanien', 'es', 'esp');
INSERT INTO `countries_de` VALUES ('728', 'Südsudan', 'ss', 'ssd');
INSERT INTO `countries_de` VALUES ('729', 'Sudan', 'sd', 'sdn');
INSERT INTO `countries_de` VALUES ('740', 'Suriname', 'sr', 'sur');
INSERT INTO `countries_de` VALUES ('748', 'Swasiland', 'sz', 'swz');
INSERT INTO `countries_de` VALUES ('752', 'Schweden', 'se', 'swe');
INSERT INTO `countries_de` VALUES ('756', 'Schweiz', 'ch', 'che');
INSERT INTO `countries_de` VALUES ('760', 'Syrien', 'sy', 'syr');
INSERT INTO `countries_de` VALUES ('762', 'Tadschikistan', 'tj', 'tjk');
INSERT INTO `countries_de` VALUES ('764', 'Thailand', 'th', 'tha');
INSERT INTO `countries_de` VALUES ('768', 'Togo', 'tg', 'tgo');
INSERT INTO `countries_de` VALUES ('776', 'Tonga', 'to', 'ton');
INSERT INTO `countries_de` VALUES ('780', 'Trinidad und Tobago', 'tt', 'tto');
INSERT INTO `countries_de` VALUES ('784', 'Vereinigte Arabische Emirate', 'ae', 'are');
INSERT INTO `countries_de` VALUES ('788', 'Tunesien', 'tn', 'tun');
INSERT INTO `countries_de` VALUES ('792', 'Türkei', 'tr', 'tur');
INSERT INTO `countries_de` VALUES ('795', 'Turkmenistan', 'tm', 'tkm');
INSERT INTO `countries_de` VALUES ('798', 'Tuvalu', 'tv', 'tuv');
INSERT INTO `countries_de` VALUES ('800', 'Uganda', 'ug', 'uga');
INSERT INTO `countries_de` VALUES ('804', 'Ukraine', 'ua', 'ukr');
INSERT INTO `countries_de` VALUES ('807', 'Nordmazedonien', 'mk', 'mkd');
INSERT INTO `countries_de` VALUES ('818', 'Ägypten', 'eg', 'egy');
INSERT INTO `countries_de` VALUES ('826', 'Vereinigtes Königreich', 'gb', 'gbr');
INSERT INTO `countries_de` VALUES ('834', 'Tansania', 'tz', 'tza');
INSERT INTO `countries_de` VALUES ('840', 'Vereinigte Staaten', 'us', 'usa');
INSERT INTO `countries_de` VALUES ('854', 'Burkina Faso', 'bf', 'bfa');
INSERT INTO `countries_de` VALUES ('858', 'Uruguay', 'uy', 'ury');
INSERT INTO `countries_de` VALUES ('860', 'Usbekistan', 'uz', 'uzb');
INSERT INTO `countries_de` VALUES ('862', 'Venezuela', 've', 'ven');
INSERT INTO `countries_de` VALUES ('882', 'Samoa', 'ws', 'wsm');
INSERT INTO `countries_de` VALUES ('887', 'Jemen', 'ye', 'yem');
INSERT INTO `countries_de` VALUES ('894', 'Sambia', 'zm', 'zmb');

-- ----------------------------
-- Table structure for delegation
-- ----------------------------
DROP TABLE IF EXISTS `delegation`;
CREATE TABLE `delegation` (
  `id` varchar(255) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `fullname` varchar(255) DEFAULT NULL,
  `abbreviation` varchar(255) DEFAULT NULL,
  `type` varchar(255) DEFAULT '',
  `countryid` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of delegation
-- ----------------------------
INSERT INTO `delegation` VALUES ('default_aegypten', 'Ägypten', 'Arabische Republik Ägypten', 'Ägypten', 'COUNTRY', '818');
INSERT INTO `delegation` VALUES ('default_aequatorialguinea', 'Äquatorialguinea', 'Republik Äquatorialguinea', 'Äquatorialguinea', 'COUNTRY', '226');
INSERT INTO `delegation` VALUES ('default_aethiopien', 'Äthiopien', 'Demokratische Bundesrepublik Äthiopien', 'Äthiopien', 'COUNTRY', '231');
INSERT INTO `delegation` VALUES ('default_afghanistan', 'Afghanistan', '	Islamische Republik Afghanistan', 'Afghanistan', 'COUNTRY', '4');
INSERT INTO `delegation` VALUES ('default_albanien', 'Albanien', 'Albanien', 'Albanien', 'COUNTRY', '8');
INSERT INTO `delegation` VALUES ('default_algerien', 'Andorra', 'Andorra', 'Andorra', 'COUNTRY', '20');
INSERT INTO `delegation` VALUES ('default_angola', 'Angola', 'Angola', 'Angola', 'COUNTRY', '24');
INSERT INTO `delegation` VALUES ('default_antigua', 'Antigua und Barbuda', 'Antigua und Barbuda', 'Antigua und Barbuda', 'COUNTRY', '28');
INSERT INTO `delegation` VALUES ('default_arabische_republik_syrien', 'Syrien', 'Arabische Republik Syrien', 'Syrien', 'COUNTRY', '760');
INSERT INTO `delegation` VALUES ('default_argentinien', 'Argentinien', 'Argentinische Republik', 'Argentinien', 'COUNTRY', '32');
INSERT INTO `delegation` VALUES ('default_armenien', 'Armenien', 'Republik Armenien', 'Armenien', 'COUNTRY', '51');
INSERT INTO `delegation` VALUES ('default_aserbaidschan', 'Aserbaidschan', '	Republik Aserbaidschan', 'Aserbaidschan', 'COUNTRY', '31');
INSERT INTO `delegation` VALUES ('default_australien', 'Australien', 'Commonwealth von Australien', 'Australien', 'COUNTRY', '36');
INSERT INTO `delegation` VALUES ('default_bahamas', 'Bahamas', 'Commonwealth der Bahamas', 'Bahamas', 'COUNTRY', '44');
INSERT INTO `delegation` VALUES ('default_bahrain', 'Bahrain', 'Königreich Bahrain', 'Bahrain', 'COUNTRY', '48');
INSERT INTO `delegation` VALUES ('default_bangladesch', 'Bangladesch', '	Volksrepublik Bangladesch', 'Bangladesch', 'COUNTRY', '50');
INSERT INTO `delegation` VALUES ('default_barbados', 'Barbados', 'Barbados', 'Barbados', 'COUNTRY', '52');
INSERT INTO `delegation` VALUES ('default_belgien', 'Belgien', 'Königreich Belgien', 'Belgien', 'COUNTRY', '56');
INSERT INTO `delegation` VALUES ('default_belize', 'Belize', 'Belize', 'Belize', 'COUNTRY', '84');
INSERT INTO `delegation` VALUES ('default_benin', 'Benin', 'Republik Benin', 'Benin', 'COUNTRY', '204');
INSERT INTO `delegation` VALUES ('default_bhutan', 'Bhutan', 'Königreich Bhutan', 'Bhutan', 'COUNTRY', '64');
INSERT INTO `delegation` VALUES ('default_bolivien', 'Bolivien', 'Plurinationaler Staat Bolivien', 'Bolivien', 'COUNTRY', '68');
INSERT INTO `delegation` VALUES ('default_bosnien', 'Bosnien und Herzegowina', 'Bosnien und Herzegowina', 'Bosnien und Herzegowina', 'COUNTRY', '70');
INSERT INTO `delegation` VALUES ('default_botswana', 'Botswana', 'Republik Botswana', 'Botswana', 'COUNTRY', '72');
INSERT INTO `delegation` VALUES ('default_brasilien', 'Brasilien', 'Föderative Republik Brasilien', 'Brasilien', 'COUNTRY', '76');
INSERT INTO `delegation` VALUES ('default_brunei', 'Brunei', 'Brunei Darussalam', 'Brunei', 'COUNTRY', '96');
INSERT INTO `delegation` VALUES ('default_bulgarien', 'Bulgarien', 'Republik Bulgarien', 'Bulgarien', 'COUNTRY', '100');
INSERT INTO `delegation` VALUES ('default_bundesrepublik_somalia', 'Somalia', 'Bundesrepublik Somalia', 'Somalia', 'COUNTRY', '706');
INSERT INTO `delegation` VALUES ('default_burkina_faso', 'Burkina Faso', 'Burkina Faso', 'Burkina Faso', 'COUNTRY', '854');
INSERT INTO `delegation` VALUES ('default_burundi', 'Burundi', 'Republik Burundi', 'Burundi', 'COUNTRY', '108');
INSERT INTO `delegation` VALUES ('default_chile', 'Chile', 'Republik Chile', 'Chile', 'COUNTRY', '152');
INSERT INTO `delegation` VALUES ('default_china', 'Volksrepublik China', 'Volksrepublik China', 'China', 'COUNTRY', '156');
INSERT INTO `delegation` VALUES ('default_costa_rica', 'Costa Rica', 'Republik Costa Rica', 'Costa Rica', 'COUNTRY', '188');
INSERT INTO `delegation` VALUES ('default_daenemark', 'Dänemark', '	Königreich Dänemark', 'Dänemark', 'COUNTRY', '208');
INSERT INTO `delegation` VALUES ('default_demokratische_republik_timor-leste', 'Timor-Leste', 'Demokratische Republik Timor-Leste', 'Timor-Leste', 'COUNTRY', '626');
INSERT INTO `delegation` VALUES ('default_deutschland', 'Deutschland', 'Bundesrepublik Deutschland', 'Deutschland', 'COUNTRY', '276');
INSERT INTO `delegation` VALUES ('default_dominica', 'Dominica', 'Commonwealth Dominica', 'Dominica', 'COUNTRY', '212');
INSERT INTO `delegation` VALUES ('default_dominikanische_republik', 'Dominikanische Republik', 'Dominikanische Republik', 'Dominikanische Republik', 'COUNTRY', '214');
INSERT INTO `delegation` VALUES ('default_dr_kongo', 'DR Kongo', 'Demokratische Republik Kongo', 'DR Kongo', 'COUNTRY', '180');
INSERT INTO `delegation` VALUES ('default_dschibuti', 'Dschibuti', 'Republik Dschibuti', 'Dschibuti', 'COUNTRY', '262');
INSERT INTO `delegation` VALUES ('default_dvr_korea', 'DVR Korea', 'Demokratische Volksrepublik Korea', 'Nordkorea', 'COUNTRY', '408');
INSERT INTO `delegation` VALUES ('default_ecuador', 'Ecuador', '	Republik Ecuador', 'Ecuador', 'COUNTRY', '218');
INSERT INTO `delegation` VALUES ('default_elfenbeinkueste', 'Côte d\'Ivoire', 'Republik Côte d\'Ivoire', 'Côte d\'Ivoire', 'COUNTRY', '384');
INSERT INTO `delegation` VALUES ('default_eritrea', 'Eritrea', 'Staat Eritrea', 'Eritrea', 'COUNTRY', '232');
INSERT INTO `delegation` VALUES ('default_estland', 'Estland', 'Republik Estland', 'Estland', 'COUNTRY', '233');
INSERT INTO `delegation` VALUES ('default_fidschi', 'Fidschi', 'Republik Fidschi-Inseln', 'Fidschi', 'COUNTRY', '242');
INSERT INTO `delegation` VALUES ('default_finnland', 'Finnland', 'Republik Finnland', 'Finnland', 'COUNTRY', '246');
INSERT INTO `delegation` VALUES ('default_frankreich', 'Frankreich', 'Französische Republik', 'Frankreich', 'COUNTRY', '250');
INSERT INTO `delegation` VALUES ('default_föderation_st._kitts_und_nevis', ' St. Kitts und Nevis', 'Föderation St. Kitts und Nevis', ' St. Kitts und Nevis', 'COUNTRY', '659');
INSERT INTO `delegation` VALUES ('default_gabun', 'Gabun', 'Gabunische Republik', 'Gabun', 'COUNTRY', '266');
INSERT INTO `delegation` VALUES ('default_gambia', 'Gambia', 'Republik Gambia', 'Gambia', 'COUNTRY', '270');
INSERT INTO `delegation` VALUES ('default_georgien', 'Georgien', 'Georgien', 'Georgien', 'COUNTRY', '268');
INSERT INTO `delegation` VALUES ('default_ghana', 'Ghana', 'Republik Ghana', 'Ghana', 'COUNTRY', '288');
INSERT INTO `delegation` VALUES ('default_grenada', 'Grenada', 'Staat Grenada', 'Grenada', 'COUNTRY', '308');
INSERT INTO `delegation` VALUES ('default_griechenland', 'Griechenland', 'Hellenische Republik', 'Griechenland', 'COUNTRY', '300');
INSERT INTO `delegation` VALUES ('default_großherzogtum_luxemburg', 'Luxemburg', 'Großherzogtum Luxemburg', ' Luxemburg', 'COUNTRY', '442');
INSERT INTO `delegation` VALUES ('default_guatemala', 'Guatemala', 'Republik Guatemala', 'Guatemala', 'COUNTRY', '320');
INSERT INTO `delegation` VALUES ('default_guinea', 'Guinea', 'Republik Guinea', 'Guinea', 'COUNTRY', '324');
INSERT INTO `delegation` VALUES ('default_Guinea-Bissau', 'Guinea-Bissau', 'Republik Guinea-Bissau', 'Guinea-Bissau', 'COUNTRY', '624');
INSERT INTO `delegation` VALUES ('default_guyana', 'Guyana', 'Kooperative Republik Guyana', 'Guyana', 'COUNTRY', '328');
INSERT INTO `delegation` VALUES ('default_haiti', 'Haiti', 'Republik Haiti', 'Haiti', 'COUNTRY', '332');
INSERT INTO `delegation` VALUES ('default_honduras', 'Honduras', 'Republik Honduras', 'Honduras', 'COUNTRY', '340');
INSERT INTO `delegation` VALUES ('default_indien', 'Indien', 'Republik Indien', 'Indien', 'COUNTRY', '356');
INSERT INTO `delegation` VALUES ('default_indonesien', 'Indonesien', 'Republik Indonesien', 'Indonesien', 'COUNTRY', '360');
INSERT INTO `delegation` VALUES ('default_irak', 'Irak', 'Republik Irak', 'Irak', 'COUNTRY', '368');
INSERT INTO `delegation` VALUES ('default_iran', 'Iran', 'Islamische Republik Iran', 'Iran', 'COUNTRY', '364');
INSERT INTO `delegation` VALUES ('default_irland', 'Irland', 'Irland', 'Irland', 'COUNTRY', '372');
INSERT INTO `delegation` VALUES ('default_islamische_republik_mauretanien', 'Mauretanien', 'Islamische Republik Mauretanien', ' Mauretanien', 'COUNTRY', '478');
INSERT INTO `delegation` VALUES ('default_islamische_republik_pakistan', 'Pakistan', 'Islamische Republik Pakistan', ' Pakistan', 'COUNTRY', '586');
INSERT INTO `delegation` VALUES ('default_island', 'Island', 'Republik Island', 'Island', 'COUNTRY', '352');
INSERT INTO `delegation` VALUES ('default_israel', 'Israel', 'Staat Israel', 'Israel', 'COUNTRY', '376');
INSERT INTO `delegation` VALUES ('default_italien', 'Italien', 'Italienische Republik', 'Italien', 'COUNTRY', '380');
INSERT INTO `delegation` VALUES ('default_jamaika', 'Jamaika', 'Jamaika', 'Jamaika', 'COUNTRY', '388');
INSERT INTO `delegation` VALUES ('default_japan', 'Japan', 'Staat Japan', 'Japan', 'COUNTRY', '392');
INSERT INTO `delegation` VALUES ('default_jemen', 'Jemen', 'Republik Jemen', 'Jemen', 'COUNTRY', '887');
INSERT INTO `delegation` VALUES ('default_jordanien', 'Jordanien', 'Haschemitisches Königreich Jordanien', 'Jordanien', 'COUNTRY', '400');
INSERT INTO `delegation` VALUES ('default_kambodscha', 'Kambodscha', 'Königreich Kambodscha', 'Kambodscha', 'COUNTRY', '116');
INSERT INTO `delegation` VALUES ('default_kamerun', 'Kamerun', 'Republik Kamerun', 'Kamerun', 'COUNTRY', '120');
INSERT INTO `delegation` VALUES ('default_kanada', 'Kanada', 'Kanada', 'Kanada', 'COUNTRY', '124');
INSERT INTO `delegation` VALUES ('default_kap_verde', 'Kap Verde', 'Republik Kap Verde', 'Kap Verde', 'COUNTRY', '132');
INSERT INTO `delegation` VALUES ('default_kasachstan', 'Kasachstan', 'Republik Kasachstan', 'Kasachstan', 'COUNTRY', '398');
INSERT INTO `delegation` VALUES ('default_katar', 'Katar', 'Staat Katar', 'Katar', 'COUNTRY', '634');
INSERT INTO `delegation` VALUES ('default_Kenia', 'Kenia', 'Republik Kenia', 'Kenia', 'COUNTRY', '404');
INSERT INTO `delegation` VALUES ('default_kirgisistan', 'Kirgisistan', 'Kirgisische Republik', 'Kirgisistan', 'COUNTRY', '417');
INSERT INTO `delegation` VALUES ('default_kiribati', 'Kiribati', 'Republik Kiribati', 'Kiribati', 'COUNTRY', '296');
INSERT INTO `delegation` VALUES ('default_kolumbien', 'Kolumbien', 'Republik Kolumbien', 'Kolumbien', 'COUNTRY', '170');
INSERT INTO `delegation` VALUES ('default_komoren', 'Komoren', 'Union der Komoren', 'Komoren', 'COUNTRY', '174');
INSERT INTO `delegation` VALUES ('default_kroatien', 'Kroatien', 'Republik Kroatien', 'Kroatien', 'COUNTRY', '191');
INSERT INTO `delegation` VALUES ('default_kuba', 'Kuba', 'Republik Kuba', 'Kuba', 'COUNTRY', '192');
INSERT INTO `delegation` VALUES ('default_kuwait', 'Kuwait', 'Staat Kuwait', 'Kuwait', 'COUNTRY', '414');
INSERT INTO `delegation` VALUES ('default_königreich_der_niederlande', 'Niederlande', 'Königreich der Niederlande', 'Niederlande', 'COUNTRY', '528');
INSERT INTO `delegation` VALUES ('default_königreich_marokko', 'Marokko', 'Königreich Marokko', 'Marokko', 'COUNTRY', '504');
INSERT INTO `delegation` VALUES ('default_königreich_norwegen', 'Norwegen', 'Königreich Norwegen', 'Norwegen', 'COUNTRY', '578');
INSERT INTO `delegation` VALUES ('default_königreich_saudi-arabien', 'Saudi-Arabien', 'Königreich Saudi-Arabien', 'Saudi-Arabien', 'COUNTRY', '682');
INSERT INTO `delegation` VALUES ('default_königreich_schweden', 'Schweden', 'Königreich Schweden', 'Schweden', 'COUNTRY', '752');
INSERT INTO `delegation` VALUES ('default_königreich_spanien', 'Spanien', 'Königreich Spanien', 'Spanien', 'COUNTRY', '724');
INSERT INTO `delegation` VALUES ('default_königreich_swasiland', 'Swasiland', 'Königreich Swasiland', 'Swasiland', 'COUNTRY', '748');
INSERT INTO `delegation` VALUES ('default_königreich_thailand', 'Thailand', 'Königreich Thailand', 'Thailand', 'COUNTRY', '764');
INSERT INTO `delegation` VALUES ('default_königreich_tonga', 'Tonga', 'Königreich Tonga', 'Tonga', 'COUNTRY', '776');
INSERT INTO `delegation` VALUES ('default_laos', 'Laos', 'Demokratische Volksrepublik Laos', 'Laos', 'COUNTRY', '418');
INSERT INTO `delegation` VALUES ('default_lesotho', 'Lesotho', 'Königreich Lesotho', 'Lesotho', 'COUNTRY', '426');
INSERT INTO `delegation` VALUES ('default_lettland', 'Lettland', 'Republik Lettland', 'Lettland', 'COUNTRY', '428');
INSERT INTO `delegation` VALUES ('default_libanon', 'Libanon', 'Libanesische Republik', 'Libanon', 'COUNTRY', '422');
INSERT INTO `delegation` VALUES ('default_liberia', 'Liberia', 'Republik Liberia', 'Liberia', 'COUNTRY', '430');
INSERT INTO `delegation` VALUES ('default_libyen', 'Libyen', 'Libyen', 'Libyen', 'COUNTRY', '434');
INSERT INTO `delegation` VALUES ('default_liechtenstein', 'Liechtenstein', 'Fürstentum Liechtenstein', 'Liechtenstein', 'COUNTRY', '438');
INSERT INTO `delegation` VALUES ('default_litauen', 'Litauen', 'Republik Litauen', 'Litauen', 'COUNTRY', '440');
INSERT INTO `delegation` VALUES ('default_malaysia', 'Malaysia', 'Malaysia', 'Malaysia', 'COUNTRY', '458');
INSERT INTO `delegation` VALUES ('default_mikronesien', 'Mikronesien', 'Föderierte Staaten von Mikronesien', 'Mikronesien', 'COUNTRY', '583');
INSERT INTO `delegation` VALUES ('default_monaco', 'Monaco', 'Fürstentum Monaco', 'Monaco', 'COUNTRY', '492');
INSERT INTO `delegation` VALUES ('default_mongolei', 'Mongolei', 'Mongolei', 'Mongolei', 'COUNTRY', '496');
INSERT INTO `delegation` VALUES ('default_montenegro', 'Montenegro', 'Montenegro', 'Montenegro', 'COUNTRY', '499');
INSERT INTO `delegation` VALUES ('default_nepal', 'Nepal', 'Demokratische Bundesrepublik Nepal', 'Nepal', 'COUNTRY', '524');
INSERT INTO `delegation` VALUES ('default_neuseeland', 'Neuseeland', 'Neuseeland', 'Neuseeland', 'COUNTRY', '554');
INSERT INTO `delegation` VALUES ('default_nigeria', 'Nigeria', 'Bundesrepublik Nigeria', 'Nigeria', 'COUNTRY', '566');
INSERT INTO `delegation` VALUES ('default_portugal', 'Portugal', 'Portugiesische Republik', 'Portugal', 'COUNTRY', '620');
INSERT INTO `delegation` VALUES ('default_republik_belarus', 'Belarus', 'Republik Belarus', ' Belarus', 'COUNTRY', null);
INSERT INTO `delegation` VALUES ('default_republik_der_philippinen', 'Philippinen', 'Republik der Philippinen', 'Philippinen', 'COUNTRY', '608');
INSERT INTO `delegation` VALUES ('default_republik_kongo', 'Republik Kongo', 'Republik Kongo', 'Republik Kongo', 'COUNTRY', null);
INSERT INTO `delegation` VALUES ('default_republik_korea', 'Republik Korea', 'Republik Korea', 'Südkorea', 'COUNTRY', null);
INSERT INTO `delegation` VALUES ('default_republik_madagaskar', 'Madagaskar', 'Republik Madagaskar', ' Madagaskar', 'COUNTRY', '450');
INSERT INTO `delegation` VALUES ('default_republik_malawi', 'Malawi', 'Republik Malawi', ' Malawi', 'COUNTRY', '454');
INSERT INTO `delegation` VALUES ('default_republik_malediven', 'Malediven', 'Republik Malediven', ' Malediven', 'COUNTRY', '462');
INSERT INTO `delegation` VALUES ('default_republik_mali', 'Mali', 'Republik Mali', ' Mali', 'COUNTRY', '466');
INSERT INTO `delegation` VALUES ('default_republik_malta', 'Malta', 'Republik Malta', ' Malta', 'COUNTRY', '470');
INSERT INTO `delegation` VALUES ('default_republik_marshallinseln', 'Marshallinseln', 'Republik Marshallinseln', ' Marshallinseln', 'COUNTRY', '584');
INSERT INTO `delegation` VALUES ('default_republik_mauritius', 'Mauritius', 'Republik Mauritius', ' Mauritius', 'COUNTRY', '480');
INSERT INTO `delegation` VALUES ('default_republik_moldau', 'Moldau', 'Republik Moldau', ' Moldau', 'COUNTRY', '498');
INSERT INTO `delegation` VALUES ('default_republik_mosambik', 'Mosambik', 'Republik Mosambik', ' Mosambik', 'COUNTRY', '508');
INSERT INTO `delegation` VALUES ('default_republik_namibia', 'Namibia', 'Republik Namibia', ' Namibia', 'COUNTRY', '516');
INSERT INTO `delegation` VALUES ('default_republik_nauru', 'Nauru', 'Republik Nauru', ' Nauru', 'COUNTRY', '520');
INSERT INTO `delegation` VALUES ('default_republik_nicaragua', 'Nicaragua', 'Republik Nicaragua', ' Nicaragua', 'COUNTRY', '558');
INSERT INTO `delegation` VALUES ('default_republik_niger', 'Niger', 'Republik Niger', ' Niger', 'COUNTRY', '562');
INSERT INTO `delegation` VALUES ('default_republik_nordmazedonien', 'Nordmazedonien', 'Republik Nordmazedonien', ' Nordmazedonien', 'COUNTRY', '807');
INSERT INTO `delegation` VALUES ('default_republik_palau', 'Palau', 'Republik Palau', ' Palau', 'COUNTRY', '585');
INSERT INTO `delegation` VALUES ('default_republik_panama', 'Panama', 'Republik Panama', ' Panama', 'COUNTRY', '591');
INSERT INTO `delegation` VALUES ('default_republik_paraguay', 'Paraguay', 'Republik Paraguay', ' Paraguay', 'COUNTRY', '600');
INSERT INTO `delegation` VALUES ('default_republik_peru', 'Peru', 'Republik Peru', ' Peru', 'COUNTRY', '604');
INSERT INTO `delegation` VALUES ('default_republik_polen', 'Polen', 'Republik Polen', ' Polen', 'COUNTRY', '616');
INSERT INTO `delegation` VALUES ('default_republik_ruanda', 'Ruanda', 'Republik Ruanda', ' Ruanda', 'COUNTRY', '646');
INSERT INTO `delegation` VALUES ('default_republik_sambia', 'Sambia', 'Republik Sambia', ' Sambia', 'COUNTRY', '894');
INSERT INTO `delegation` VALUES ('default_republik_san_marino', 'San Marino', 'Republik San Marino', ' San Marino', 'COUNTRY', '674');
INSERT INTO `delegation` VALUES ('default_republik_senegal', 'Senegal', 'Republik Senegal', ' Senegal', 'COUNTRY', '686');
INSERT INTO `delegation` VALUES ('default_republik_serbien', 'Serbien', 'Republik Serbien', ' Serbien', 'COUNTRY', '688');
INSERT INTO `delegation` VALUES ('default_republik_seychellen', 'Seychellen', 'Republik Seychellen', ' Seychellen', 'COUNTRY', '690');
INSERT INTO `delegation` VALUES ('default_republik_sierra_leone', 'Sierra Leone', 'Republik Sierra Leone', ' Sierra Leone', 'COUNTRY', '694');
INSERT INTO `delegation` VALUES ('default_republik_simbabwe', 'Simbabwe', 'Republik Simbabwe', ' Simbabwe', 'COUNTRY', '716');
INSERT INTO `delegation` VALUES ('default_republik_singapur', 'Singapur', 'Republik Singapur', ' Singapur', 'COUNTRY', '702');
INSERT INTO `delegation` VALUES ('default_republik_slowenien', 'Slowenien', 'Republik Slowenien', ' Slowenien', 'COUNTRY', '705');
INSERT INTO `delegation` VALUES ('default_republik_sudan', 'Sudan', 'Republik Sudan', 'Sudan', 'COUNTRY', '729');
INSERT INTO `delegation` VALUES ('default_republik_suriname', 'Suriname', 'Republik Suriname', ' Suriname', 'COUNTRY', '740');
INSERT INTO `delegation` VALUES ('default_republik_südafrika', 'Südafrika', 'Republik Südafrika', ' Südafrika', 'COUNTRY', '710');
INSERT INTO `delegation` VALUES ('default_republik_südsudan', 'Südsudan', 'Republik Südsudan', ' Südsudan', 'COUNTRY', '728');
INSERT INTO `delegation` VALUES ('default_republik_tadschikistan', 'Tadschikistan', 'Republik Tadschikistan', ' Tadschikistan', 'COUNTRY', '762');
INSERT INTO `delegation` VALUES ('default_republik_togo', 'Togo', 'Republik Togo', ' Togo', 'COUNTRY', '768');
INSERT INTO `delegation` VALUES ('default_republik_trinidad_und_tobago', 'Trinidad und Tobago', 'Republik Trinidad und Tobago', ' Trinidad und Tobago', 'COUNTRY', '780');
INSERT INTO `delegation` VALUES ('default_republik_tschad', 'Tschad', 'Republik Tschad', ' Tschad', 'COUNTRY', '148');
INSERT INTO `delegation` VALUES ('default_republik_türkei', 'Türkei', 'Republik Türkei', ' Türkei', 'COUNTRY', '792');
INSERT INTO `delegation` VALUES ('default_republik_uganda', 'Uganda', 'Republik Uganda', ' Uganda', 'COUNTRY', '800');
INSERT INTO `delegation` VALUES ('default_republik_ungarn', 'Ungarn', 'Republik Ungarn', ' Ungarn', 'COUNTRY', '348');
INSERT INTO `delegation` VALUES ('default_republik_usbekistan', 'Usbekistan', 'Republik Usbekistan', ' Usbekistan', 'COUNTRY', '860');
INSERT INTO `delegation` VALUES ('default_republik_vanuatu', 'Vanuatu', 'Republik Vanuatu', ' Vanuatu', 'COUNTRY', '548');
INSERT INTO `delegation` VALUES ('default_republik_zypern', 'Zypern', 'Republik Zypern', ' Zypern', 'COUNTRY', '196');
INSERT INTO `delegation` VALUES ('default_republik_österreich', 'Österreich', 'Republik Österreich', 'Österreich', 'COUNTRY', '40');
INSERT INTO `delegation` VALUES ('default_rumänien', 'Rumänien', 'Rumänien', 'Rumänien', 'COUNTRY', '642');
INSERT INTO `delegation` VALUES ('default_russische_föderation', 'Russland', 'Russische Föderation', 'Russland', 'COUNTRY', '643');
INSERT INTO `delegation` VALUES ('default_salomonen', 'Salomonen', 'Salomonen', 'Salomonen', 'COUNTRY', '90');
INSERT INTO `delegation` VALUES ('default_salvador', 'El Salvador', 'Republik El Salvador', 'El Salvador', 'COUNTRY', '222');
INSERT INTO `delegation` VALUES ('default_sao_tome_und_principe', 'São Tomé und Príncipe', 'Demokratische Republik São Tomé und Príncipe', 'São Tomé und Príncipe', 'COUNTRY', '678');
INSERT INTO `delegation` VALUES ('default_schweizerische_eidgenossenschaft', 'Schweiz', 'Schweizerische Eidgenossenschaft', 'Schweiz', 'COUNTRY', '756');
INSERT INTO `delegation` VALUES ('default_slowakische_republik', 'Slowakei', 'Slowakische Republik', 'Slowakei', 'COUNTRY', '703');
INSERT INTO `delegation` VALUES ('default_sozialistische_republik_vietnam', 'Vietnam', 'Sozialistische Republik Vietnam', 'Vietnam', 'COUNTRY', '704');
INSERT INTO `delegation` VALUES ('default_sri_lanka', 'Sri Lanka', 'Demokratische Sozialistische Republik Sri Lanka', 'Sri Lanka', 'COUNTRY', '144');
INSERT INTO `delegation` VALUES ('default_st._lucia', 'St. Lucia', 'St. Lucia', 'St. Lucia', 'COUNTRY', '662');
INSERT INTO `delegation` VALUES ('default_st._vincent_und_die_grenadinen', 'St. Vincent und die Grenadinen', 'St. Vincent und die Grenadinen', 'St. Vincent und die Grenadinen', 'COUNTRY', '670');
INSERT INTO `delegation` VALUES ('default_staat_palästina', 'Palästina', 'Staat Palästina', 'Palästina', 'COUNTRY', null);
INSERT INTO `delegation` VALUES ('default_staat_vatikanstadt', 'Vatikanstadt', 'Staat Vatikanstadt', 'Vatikanstadt', 'COUNTRY', null);
INSERT INTO `delegation` VALUES ('default_sultanat_oman', 'Oman', 'Sultanat Oman', 'Oman', 'COUNTRY', '512');
INSERT INTO `delegation` VALUES ('default_taiwan', 'Taiwan', 'Taiwan', 'Taiwan', 'COUNTRY', null);
INSERT INTO `delegation` VALUES ('default_tschechische_republik', 'Tschechien', 'Tschechische Republik', 'Tschechien ', 'COUNTRY', '203');
INSERT INTO `delegation` VALUES ('default_tunesische_republik', 'Tunesien', 'Tunesische Republik', 'Tunesien', 'COUNTRY', '788');
INSERT INTO `delegation` VALUES ('default_turkmenistan', 'Turkmenistan', 'Turkmenistan', 'Turkmenistan', 'COUNTRY', '795');
INSERT INTO `delegation` VALUES ('default_tuvalu', 'Tuvalu', 'Tuvalu', 'Tuvalu', 'COUNTRY', '798');
INSERT INTO `delegation` VALUES ('default_türkische_republik_nordzypern', 'Nordzypern', 'Türkische Republik Nordzypern', 'Nordzypern', 'COUNTRY', null);
INSERT INTO `delegation` VALUES ('default_ukraine', 'Ukraine', 'Ukraine', 'Ukraine', 'COUNTRY', '804');
INSERT INTO `delegation` VALUES ('default_unabhängiger_staat_papua-neuguinea', 'Papua-Neuguinea', 'Unabhängiger Staat Papua-Neuguinea', 'Papua-Neuguinea', 'COUNTRY', '598');
INSERT INTO `delegation` VALUES ('default_unabhängiger_staat_samoa', 'Samoa', 'Unabhängiger Staat Samoa', 'Samoa', 'COUNTRY', '882');
INSERT INTO `delegation` VALUES ('default_union_myanmar', 'Myanmar', 'Union Myanmar', 'Myanmar', 'COUNTRY', '104');
INSERT INTO `delegation` VALUES ('default_uruguay', 'Uruguay', 'Republik Östlich des Uruguay', 'Uruguay', 'COUNTRY', '858');
INSERT INTO `delegation` VALUES ('default_venezuela', 'Venezuela', 'Bolivarische Republik Venezuela', 'Venezuela', 'COUNTRY', '862');
INSERT INTO `delegation` VALUES ('default_vereinigtes_königreich_großbritannien_und_nordirland', 'Vereinigtes Königreich', 'Vereinigtes Königreich Großbritannien und Nordirland', 'Vereinigtes Königreich', 'COUNTRY', '826');
INSERT INTO `delegation` VALUES ('default_vereinigte_arabische_emirate', 'Vereinigte Arabische Emirate', 'Vereinigte Arabische Emirate', 'Vereinigte Arabische Emirate', 'COUNTRY', '784');
INSERT INTO `delegation` VALUES ('default_vereinigte_mexikanische_staaten', 'Mexiko', 'Vereinigte Mexikanische Staaten', 'Mexiko', 'COUNTRY', '484');
INSERT INTO `delegation` VALUES ('default_vereinigte_republik_tansania', 'Tansania', 'Vereinigte Republik Tansania', 'Tansania', 'COUNTRY', '834');
INSERT INTO `delegation` VALUES ('default_vereinigte_staaten_von_amerika', 'Vereinigte Staaten', 'Vereinigte Staaten von Amerika', 'Vereinigte Staaten', 'COUNTRY', '840');
INSERT INTO `delegation` VALUES ('default_zentralafrikanische_republik', 'Zentral­afrikanische Republik', 'Zentralafrikanische Republik', 'Zentral­afrikanische Republik', 'COUNTRY', '140');

-- ----------------------------
-- Table structure for delegation_in_committee
-- ----------------------------
DROP TABLE IF EXISTS `delegation_in_committee`;
CREATE TABLE `delegation_in_committee` (
  `delincommitteeid` int(11) NOT NULL AUTO_INCREMENT,
  `linkid` int(11) DEFAULT NULL,
  `committeeid` varchar(255) DEFAULT NULL,
  `mincount` int(11) DEFAULT NULL,
  `maxcount` int(11) DEFAULT NULL,
  PRIMARY KEY (`delincommitteeid`),
  KEY `linker` (`linkid`),
  KEY `committeelink` (`committeeid`),
  CONSTRAINT `committeelink` FOREIGN KEY (`committeeid`) REFERENCES `committee` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `linker` FOREIGN KEY (`linkid`) REFERENCES `conference_delegation` (`linkid`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of delegation_in_committee
-- ----------------------------

-- ----------------------------
-- Table structure for ngo
-- ----------------------------
DROP TABLE IF EXISTS `ngo`;
CREATE TABLE `ngo` (
  `id` varchar(255) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `fullname` varchar(255) DEFAULT NULL,
  `abbreviation` varchar(255) DEFAULT NULL,
  `type` varchar(255) DEFAULT NULL,
  `website` varchar(255) DEFAULT NULL,
  `inecosoc` bit(1) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of ngo
-- ----------------------------

-- ----------------------------
-- Table structure for participation_delegation
-- ----------------------------
DROP TABLE IF EXISTS `participation_delegation`;
CREATE TABLE `participation_delegation` (
  `participationid` int(11) NOT NULL AUTO_INCREMENT,
  `linkid` int(11) DEFAULT NULL,
  `userid` varchar(255) DEFAULT NULL,
  `incommitteeid` int(11) DEFAULT NULL,
  PRIMARY KEY (`participationid`),
  KEY `link_del` (`linkid`),
  KEY `user_link` (`userid`),
  KEY `committee_link` (`incommitteeid`),
  CONSTRAINT `committee_link` FOREIGN KEY (`incommitteeid`) REFERENCES `delegation_in_committee` (`delincommitteeid`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `link_del` FOREIGN KEY (`linkid`) REFERENCES `conference_delegation` (`linkid`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `user_link` FOREIGN KEY (`userid`) REFERENCES `user` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of participation_delegation
-- ----------------------------

-- ----------------------------
-- Table structure for resolution
-- ----------------------------
DROP TABLE IF EXISTS `resolution`;
CREATE TABLE `resolution` (
  `id` varchar(255) NOT NULL,
  `name` varchar(255) DEFAULT '',
  `user` varchar(255) DEFAULT NULL,
  `creationdate` datetime DEFAULT NULL,
  `lastchangeddate` datetime DEFAULT NULL,
  `onlinecode` varchar(255) DEFAULT '',
  `ispublicread` bit(1) DEFAULT b'0',
  `ispublicwrite` bit(1) DEFAULT b'0',
  `allowamendments` bit(1) DEFAULT b'0',
  PRIMARY KEY (`id`),
  KEY `id` (`id`),
  KEY `resolutionuser` (`user`),
  CONSTRAINT `resolutionuser` FOREIGN KEY (`user`) REFERENCES `user` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of resolution
-- ----------------------------

-- ----------------------------
-- Table structure for resolution_auth
-- ----------------------------
DROP TABLE IF EXISTS `resolution_auth`;
CREATE TABLE `resolution_auth` (
  `resolutionid` varchar(255) NOT NULL,
  `userid` varchar(255) DEFAULT NULL,
  `read` bit(1) DEFAULT NULL,
  `write` bit(1) DEFAULT NULL,
  PRIMARY KEY (`resolutionid`),
  KEY `resoauthuser` (`userid`),
  CONSTRAINT `resoauthreso` FOREIGN KEY (`resolutionid`) REFERENCES `resolution` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `resoauthuser` FOREIGN KEY (`userid`) REFERENCES `user` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of resolution_auth
-- ----------------------------

-- ----------------------------
-- Table structure for settings
-- ----------------------------
DROP TABLE IF EXISTS `settings`;
CREATE TABLE `settings` (
  `name` varchar(255) NOT NULL,
  `value` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of settings
-- ----------------------------
INSERT INTO `settings` VALUES ('RESOLUTION_PATH', 'F:\\resolutions');

-- ----------------------------
-- Table structure for user
-- ----------------------------
DROP TABLE IF EXISTS `user`;
CREATE TABLE `user` (
  `id` varchar(255) NOT NULL,
  `username` varchar(255) DEFAULT NULL,
  `password` varchar(255) DEFAULT NULL,
  `salt` varchar(255) DEFAULT NULL,
  `mail` varchar(255) DEFAULT NULL,
  `forename` varchar(255) DEFAULT NULL,
  `lastname` varchar(255) DEFAULT NULL,
  `phone` varchar(255) DEFAULT NULL,
  `street` varchar(255) DEFAULT NULL,
  `housenumber` varchar(255) DEFAULT NULL,
  `city` varchar(255) DEFAULT NULL,
  `zipcode` varchar(255) DEFAULT NULL,
  `registerdate` datetime DEFAULT NULL,
  `lastlogin` datetime DEFAULT NULL,
  `status` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of user
-- ----------------------------

-- ----------------------------
-- Table structure for user_clearance
-- ----------------------------
DROP TABLE IF EXISTS `user_clearance`;
CREATE TABLE `user_clearance` (
  `userid` varchar(255) NOT NULL,
  `CreateConference` bit(1) DEFAULT b'0',
  PRIMARY KEY (`userid`),
  CONSTRAINT `userclearancelink` FOREIGN KEY (`userid`) REFERENCES `user` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of user_clearance
-- ----------------------------
