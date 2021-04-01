CREATE TABLE genres(
  id_genres serial PRIMARY KEY,
  genre text
);

CREATE TABLE styles(
  id_styles serial PRIMARY KEY,
  style text
);

CREATE TABLE script_type(
  id_script_type serial PRIMARY KEY,
  type text
);

CREATE TABLE tuning(
  id_tuning serial PRIMARY KEY,
  tuning text,
  strings smallint
);

CREATE TABLE artists(
  id_artists serial PRIMARY KEY,
  artist text
);

CREATE TABLE guitartype(
  id_guitartype serial PRIMARY KEY,
  type text,
  kind text
);

CREATE TABLE song(
  id_song serial PRIMARY KEY,
  title text,
  genres_id INTEGER REFERENCES genres(id_genres),
  guitartype_id INTEGER REFERENCES guitartype(id_guitartype),
  tuning_id INTEGER REFERENCES tuning(id_tuning)
);

CREATE TABLE songscript(
  song_id INTEGER REFERENCES song(id_song),
  script_type_id INTEGER REFERENCES script_type(id_script_type),
  PRIMARY KEY (song_id, script_type_id)
);

CREATE TABLE songstyle(
  song_id INTEGER REFERENCES song(id_song),
  styles_id INTEGER REFERENCES styles(id_styles),
  PRIMARY KEY (song_id, styles_id)
);

CREATE TABLE authors(
  song_id INTEGER REFERENCES song(id_song),
  artists_id INTEGER REFERENCES artists(id_artists),
  PRIMARY KEY (song_id, artists_id)
);

CREATE TABLE time_measure(
  id_time_measure serial PRIMARY KEY,
  measure text
);

CREATE TABLE time_change(
  song_id INTEGER REFERENCES song(id_song),
  time_measure_id INTEGER REFERENCES time_measure(id_time_measure),
  PRIMARY KEY (song_id, time_measure_id)
);
