/*
Navicat MySQL Data Transfer

Source Server         : localhost mariadb
Source Server Version : 50505
Source Host           : localhost:3306
Source Database       : munity

Target Server Type    : MYSQL
Target Server Version : 50505
File Encoding         : 65001

Date: 2020-02-04 17:16:12
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
INSERT INTO `admin` VALUES ('8c19c2eb-39cf-44dd-942f-1ae2930af524', '5');

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
INSERT INTO `auth` VALUES ('YPkdMoiNuWUu4mFMw0nzBG5I6wT7fC7DcIPCB3/khyQNRboXBiAq3x88WVLW16kpaA4JZWyYuoAJHFRm1dnDUQ==', '8c19c2eb-39cf-44dd-942f-1ae2930af524', '2020-02-04 12:44:35', '2020-02-05 12:44:35');

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
INSERT INTO `committee` VALUES ('4f1c1d1a-477e-494e-bf95-d9b4ea74365c', 'Hallo Welt', 'Hallo Welt', 'asd', 'södkf', '7cafe067-a9be-4595-9479-b067df507d50', null, null);
INSERT INTO `committee` VALUES ('default_de_gv', 'Generalversammlung', 'Generalversammlung', 'GV', 'die', 'default', 'DE', null);
INSERT INTO `committee` VALUES ('default_de_igh', 'Internationaler Gerichtshof', 'Internationaler Gerichtshof', 'IGH', 'der', 'default', 'DE', null);
INSERT INTO `committee` VALUES ('default_de_mrr', 'Menschenrechtsrat', 'Menschenrechtsrat', 'MRR', 'der', 'default', 'DE', null);
INSERT INTO `committee` VALUES ('default_de_sek', 'UN-Sekretariat', 'UN-Sekretariat', 'SEK', 'das', 'default', 'DE', null);
INSERT INTO `committee` VALUES ('default_de_sr', 'Sicherheitsrat', 'Sicherheitsrat', 'SR', 'der', 'default', 'DE', null);
INSERT INTO `committee` VALUES ('default_de_thr', 'UN-Treuhandrat', 'UN-Treuhandrat', 'THR', 'der', 'default', 'DE', null);
INSERT INTO `committee` VALUES ('default_de_undp', 'Entwicklungsprogramm', 'Entwicklungsprogramm', 'UNDP', 'der', 'default', 'DE', 'default_de_gv');
INSERT INTO `committee` VALUES ('default_de_Wiso', 'Wirtschafts- und Sozialrat', 'Wirtschafts- und Sozialrat', 'WiSo', 'der', 'default', 'DE', null);
INSERT INTO `committee` VALUES ('efc5fdab-d571-4b4f-8c36-f9f0918235ce', 'Neues Gremium', 'Gremium das neu ist', 'NG', 'das', '7cafe067-a9be-4595-9479-b067df507d50', null, null);

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
INSERT INTO `conference` VALUES ('7cafe067-a9be-4595-9479-b067df507d50', 'Roflcopter', 'Roflcopter swuuup', 'Rofl', '2020-02-09 00:00:00', '2020-02-12 00:00:00', null, null, '2020-01-09 13:38:39', null);
INSERT INTO `conference` VALUES ('default', 'Default', 'Default Conference', 'DEFAULT', '2019-12-02 00:00:00', '2022-12-31 22:28:10', 'Seine Exzelenz der Generalsekretär', 'António Guterres', '2019-12-02 22:29:00', 'johndoe');
INSERT INTO `conference` VALUES ('test', 'test', 'test', 'test', '2020-02-04 10:55:34', '2020-02-14 10:55:37', 'test', 'test', '2020-02-04 10:55:47', 'test');

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
  CONSTRAINT `conference` FOREIGN KEY (`conference_id`) REFERENCES `conference` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `delegation` FOREIGN KEY (`delegation_id`) REFERENCES `delegation` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of conference_delegation
-- ----------------------------
INSERT INTO `conference_delegation` VALUES ('1', 'default', 'default_aegypten', '1', '1');
INSERT INTO `conference_delegation` VALUES ('2', 'default', 'default_afghanistan', '1', '1');

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
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

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
  CONSTRAINT `conferenceAuthConference` FOREIGN KEY (`conferenceid`) REFERENCES `conference` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `conferenceAuthUser` FOREIGN KEY (`userid`) REFERENCES `user` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of conference_user_auth
-- ----------------------------
INSERT INTO `conference_user_auth` VALUES ('7cafe067-a9be-4595-9479-b067df507d50', '8c19c2eb-39cf-44dd-942f-1ae2930af524', '', '', '');

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
INSERT INTO `delegation` VALUES ('default_aegypten', 'Ägypten', 'Ägypten', 'Ägypten', 'COUNTRY', '818');
INSERT INTO `delegation` VALUES ('default_afghanistan', 'Afghanistan', 'Afghanistan', 'Afghanistan', 'COUNTRY', '4');

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
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of delegation_in_committee
-- ----------------------------
INSERT INTO `delegation_in_committee` VALUES ('1', '1', 'default_de_gv', '1', '1');

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
INSERT INTO `participation_delegation` VALUES ('1', '1', 'johndoe', '1');
INSERT INTO `participation_delegation` VALUES ('2', '1', 'anon', null);

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
INSERT INTO `resolution` VALUES ('04fee515-d0b9-4318-b3a7-11e13530ce11', 'No Name', 'anon', '2020-02-04 10:31:51', '2020-02-04 10:31:51', '77700489', '', '', '\0');

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
INSERT INTO `user` VALUES ('8c19c2eb-39cf-44dd-942f-1ae2930af524', 'MikeLitoris', 'zq4T5wcwgDQ87dAlCJZ/Nd3zsnDs1APUEBPrOwduxS8=', 'FFlppxx6KlFbWEQDAfZR6w==', 'mike@litoris.de', null, null, null, null, null, null, null, '2020-01-07 16:13:47', null, 'OK');
INSERT INTO `user` VALUES ('anon', 'anon', 'anon', 'anon', 'anon', 'An', 'On', '1234323131', 'streetname', '13', 'nowhere', '123456', '2019-12-05 11:03:11', '2019-12-05 11:03:14', 'OK');
INSERT INTO `user` VALUES ('johndoe', 'johndoe', 'password', 'salt', 'john', 'John', 'Doe', '0123456789', 'plygroundstreet', '123', 'nowhere', '123456', '2019-12-02 22:26:44', '2019-12-02 22:26:47', 'OK');

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
INSERT INTO `user_clearance` VALUES ('8c19c2eb-39cf-44dd-942f-1ae2930af524', '');
