CREATE SEQUENCE vending_machine_id_seq;

CREATE TABLE vending_machines (
    id        BIGINT NOT NULL DEFAULT nextval('vending_machine_id_seq'),
    name      TEXT NOT NULL,
    latitude  NUMERIC(8, 6) NOT NULL,
    longitude NUMERIC(9, 6) NOT NULL,
    city      TEXT,
    category  TEXT NOT NULL,
    source    TEXT NOT NULL,

    CHECK (latitude BETWEEN -90 AND 90),
    CHECK (longitude BETWEEN -180 AND 180),
    CHECK (category IN (
        'drinks',
        'snacks',
        'other'
    ))
);