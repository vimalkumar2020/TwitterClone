create database EFMVC

use EFMVC

CREATE TABLE Person
(
user_id varchar(25) PRIMARY KEY NOT NULL,
password varchar(50) NOT NULL,
fullName varchar(30) NOT NULL,
email varchar(50) NOT NULL,	
joined datetime2 NOT NULL,
active bit NOT NULL,
)

CREATE TABLE Following
(
user_id varchar(25) NOT NULL,
following_id varchar(25) NOT NULL,
PRIMARY KEY (user_id, following_id)
)

CREATE TABLE Tweet
(
tweet_id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
user_id varchar(25) NOT NULL,
message varchar(140) NOT NULL,
created datetime2 NOT NULL
)