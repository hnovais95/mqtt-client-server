-- public.rain definition

-- Drop table

-- DROP TABLE public.rain;

CREATE TABLE public.rain (
	id serial4 NOT NULL,
	"name" varchar NOT NULL,
	"timestamp" timestamp NOT NULL,
	region numeric NOT NULL,
	rain bool NOT NULL,
	CONSTRAINT rain_pkey PRIMARY KEY (id)
);