-- public.humidity definition

-- Drop table

-- DROP TABLE public.humidity;

CREATE TABLE public.humidity (
	id serial4 NOT NULL,
	"name" varchar NOT NULL,
	"timestamp" timestamp NOT NULL,
	region numeric NOT NULL,
	humidity numeric NOT NULL,
	CONSTRAINT humidity_pkey PRIMARY KEY (id)
);