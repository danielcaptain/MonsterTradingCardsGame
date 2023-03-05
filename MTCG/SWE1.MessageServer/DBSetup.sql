drop table if exists cardpackage;
drop table if exists stack;
drop table if exists player;
drop table if exists card;

create extension if not exists "pgcrypto";
create extension if not exists "uuid-ossp";

create table if not exists card(
	--id integer primary key generated always as identity,
	id uuid primary key DEFAULT uuid_generate_v4 (),
	type integer NULL,
	name text NOT NULL,
	damage real NOT NULL,
	isMonster bool NOT NULL
);

create table if not exists player(
	id integer generated always as identity,
	name text unique primary key not null,
	coins integer,
	token text ,
	password text not null,
	alias text,
	bio text,
	image text
);

create table if not exists stack(
	id integer primary key generated always as identity,
	player name references player,
	card uuid references card,
	partOfDeck boolean
);  

create table if not exists cardpackage(
	id integer primary key generated always as identity,
	c0id uuid references card,
	c1id uuid references card,
	c2id uuid references card,
	c3id uuid references card,
	c4id uuid references card
);
