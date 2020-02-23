/*
Navicat MySQL Data Transfer

Source Server         : localhost mariadb
Source Server Version : 50505
Source Host           : localhost:3306
Source Database       : munity-test

Target Server Type    : MYSQL
Target Server Version : 50505
File Encoding         : 65001

Date: 2020-02-19 13:56:33
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
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=latin1;

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
  `role` int(255) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `conferenceteamconnection` (`conferenceid`),
  KEY `userteamconnection` (`userid`),
  KEY `roleLink` (`role`),
  CONSTRAINT `conferenceteamconnection` FOREIGN KEY (`conferenceid`) REFERENCES `conference` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `roleLink` FOREIGN KEY (`role`) REFERENCES `conference_team_roles` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `userteamconnection` FOREIGN KEY (`userid`) REFERENCES `user` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;


-- ----------------------------
-- Table structure for conference_team_roles
-- ----------------------------
DROP TABLE IF EXISTS `conference_team_roles`;
CREATE TABLE `conference_team_roles` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `parentrole` int(11) DEFAULT NULL,
  `mincount` int(11) DEFAULT NULL,
  `maxcount` int(11) DEFAULT NULL,
  `conferenceid` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `selfReference` (`parentrole`),
  KEY `conferenceReference` (`conferenceid`),
  CONSTRAINT `conferenceReference` FOREIGN KEY (`conferenceid`) REFERENCES `conference` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `selfReference` FOREIGN KEY (`parentrole`) REFERENCES `conference_team_roles` (`id`) ON DELETE SET NULL ON UPDATE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;


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
INSERT INTO `delegation` VALUES ('default_afghanistan', 'Afghanistan', '	Islamische Republik Afghanistan', 'Afghanistan', 'COUNTRY', '4');
INSERT INTO `delegation` VALUES ('default_albanien', 'Albanien', 'Albanien', 'Albanien', 'COUNTRY', '8');
INSERT INTO `delegation` VALUES ('default_algerien', 'Andorra', 'Andorra', 'Andorra', 'COUNTRY', '20');

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
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=latin1;

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
  `canread` bit(1) DEFAULT NULL,
  `canwrite` bit(1) DEFAULT NULL,
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
