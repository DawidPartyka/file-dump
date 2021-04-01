insert into genres(genre)
  values ('pop'),
         ('progressive rock'),
         ('progressive metal'),
         ('bgm'),
         ('pop rock'),
         ('classical'),
         ('emotive');

insert into tuning(strings, tuning)
  values (6, 'EADGBE'),
         (6, 'CGCFAD'),
         (7, 'BEADGBE'),
         (6, 'DADGBE'),
         (6, 'GDA#FCC'),
         (4, 'D#G#C#F#'),
         (6, 'A#FA#D#GC'),
         (7, 'BDADGBE');

insert into guitartype(type, kind)
  values ('gitara','akustyczna'),
         ('gitara','elektryczna'),
         ('gitara','klasyczna'),
         ('bas', 'elektryczny');

insert into styles(style)
  values ('fingerstyle'),
         ('sweep picking'),
         ('tapping'),
         ('Alternate picking'),
         ('percussion'),
         ('tremolo'),
         ('slap');

insert into script_type(type)
  values ('Nuty'),
         ('Tabulatura');

insert into song(title, genres_id, guitartype_id, tuning_id)
  values
    ('Everybody Wants To Rule The World', (SELECT id_genres from genres WHERE genre='pop'),
    (SELECT id_guitartype from guitartype WHERE kind='akustyczna'),
    (SELECT id_tuning from tuning WHERE tuning='EADGBE')),

    ('Marigold', (SELECT id_genres from genres WHERE genre='progressive rock'),
    (SELECT id_guitartype from guitartype WHERE kind='elektryczna'),
    (SELECT id_tuning from tuning WHERE tuning='CGCFAD')),

    ('For the Love of God', (SELECT id_genres from genres WHERE genre='progressive metal'),
    (SELECT id_guitartype from guitartype WHERE kind='elektryczna'),
    (SELECT id_tuning from tuning WHERE tuning='BEADGBE')),

    ('Schism', (SELECT id_genres from genres WHERE genre='progressive rock'),
    (SELECT id_guitartype from guitartype WHERE kind='elektryczna'),
    (SELECT id_tuning from tuning WHERE tuning='DADGBE')),

    ('Lost Woods', (SELECT id_genres from genres WHERE genre='bgm'),
    (SELECT id_guitartype from guitartype WHERE kind='klasyczna'),
    (SELECT id_tuning from tuning WHERE tuning='EADGBE')),

    ('Shout', (SELECT id_genres from genres WHERE genre='pop rock'),
    (SELECT id_guitartype from guitartype WHERE kind='klasyczna'),
    (SELECT id_tuning from tuning WHERE tuning='EADGBE')),

    ('Une lumière envoûtante', (SELECT id_genres from genres WHERE genre='classical'),
    (SELECT id_guitartype from guitartype WHERE kind='akustyczna'),
    (SELECT id_tuning from tuning WHERE tuning='GDA#FCC')),

    ('Soothsayer', (SELECT id_genres from genres WHERE genre='emotive'),
    (SELECT id_guitartype from guitartype WHERE kind='elektryczny'),
    (SELECT id_tuning from tuning WHERE tuning='D#G#C#F#')),

    ('Lotus Island', (SELECT id_genres from genres WHERE genre='progressive metal'),
    (SELECT id_guitartype from guitartype WHERE kind='elektryczna'),
    (SELECT id_tuning from tuning WHERE tuning='A#FA#D#GC')),

    ('Cafo', (SELECT id_genres from genres WHERE genre='progressive rock'),
    (SELECT id_guitartype from guitartype WHERE kind='elektryczna'),
    (SELECT id_tuning from tuning WHERE tuning='BDADGBE'));

insert into songscript(song_id, script_type_id)
  values ((SELECT id_song from song WHERE title='Everybody Wants To Rule The World'),
		(SELECT id_script_type from script_type WHERE type='Nuty')),

        ((SELECT id_song from song WHERE title='Everybody Wants To Rule The World'),
		(SELECT id_script_type from script_type WHERE type='Tabulatura')),

        ((SELECT id_song from song WHERE title='Marigold'),
		(SELECT id_script_type from script_type WHERE type='Tabulatura')),

        ((SELECT id_song from song WHERE title='For the Love of God'),
		(SELECT id_script_type from script_type WHERE type='Tabulatura')),

        ((SELECT id_song from song WHERE title='Schism'),
		(SELECT id_script_type from script_type WHERE type='Tabulatura')),

        ((SELECT id_song from song WHERE title='Lost Woods'),
		(SELECT id_script_type from script_type WHERE type='Nuty')),

        ((SELECT id_song from song WHERE title='Shout'),
		(SELECT id_script_type from script_type WHERE type='Nuty')),

        ((SELECT id_song from song WHERE title='Une lumière envoûtante'),
		(SELECT id_script_type from script_type WHERE type='Tabulatura')),

        ((SELECT id_song from song WHERE title='Soothsayer'),
		(SELECT id_script_type from script_type WHERE type='Tabulatura')),

        ((SELECT id_song from song WHERE title='Lotus Island'),
		(SELECT id_script_type from script_type WHERE type='Tabulatura')),

        ((SELECT id_song from song WHERE title='Cafo'),
		(SELECT id_script_type from script_type WHERE type='Nuty')),

        ((SELECT id_song from song WHERE title='Cafo'),
		(SELECT id_script_type from script_type WHERE type='Tabulatura'));

insert into time_measure(measure)
  values ('4/4'),
          ('7/8'),
          ('12/8'),
          ('8/8'),
          ('5/8'),
          ('6/8'),
          ('2/4'),
          ('3/8'),
          ('7/6'),
          ('8/5'),
          ('6/5'),
          ('9/4'),
          ('7/4'),
          ('6/4'),
          ('5/4');

insert into songstyle(song_id, styles_id)
  values ((SELECT id_song from song WHERE title='Everybody Wants To Rule The World'),
		(SELECT id_styles from styles WHERE style='fingerstyle')),

        ((SELECT id_song from song WHERE title='Marigold'),
		(SELECT id_styles from styles WHERE style='sweep picking')),

        ((SELECT id_song from song WHERE title='For the Love of God'),
		(SELECT id_styles from styles WHERE style='sweep picking')),

        ((SELECT id_song from song WHERE title='For the Love of God'),
		(SELECT id_styles from styles WHERE style='tapping')),

        ((SELECT id_song from song WHERE title='Schism'),
		(SELECT id_styles from styles WHERE style='Alternate picking')),

        ((SELECT id_song from song WHERE title='Lost Woods'),
		(SELECT id_styles from styles WHERE style='fingerstyle')),

        ((SELECT id_song from song WHERE title='Lost Woods'),
		(SELECT id_styles from styles WHERE style='percussion')),

        ((SELECT id_song from song WHERE title='Shout'),
		(SELECT id_styles from styles WHERE style='fingerstyle')),

        ((SELECT id_song from song WHERE title='Shout'),
		(SELECT id_styles from styles WHERE style='percussion')),

        ((SELECT id_song from song WHERE title='Une lumière envoûtante'),
		(SELECT id_styles from styles WHERE style='fingerstyle')),

        ((SELECT id_song from song WHERE title='Une lumière envoûtante'),
		(SELECT id_styles from styles WHERE style='tremolo')),

        ((SELECT id_song from song WHERE title='Soothsayer'),
		(SELECT id_styles from styles WHERE style='slap')),

        ((SELECT id_song from song WHERE title='Lotus Island'),
		(SELECT id_styles from styles WHERE style='tapping')),

        ((SELECT id_song from song WHERE title='Cafo'),
		(SELECT id_styles from styles WHERE style='sweep picking')),

        ((SELECT id_song from song WHERE title='Cafo'),
		(SELECT id_styles from styles WHERE style='Alternate picking'));

insert into artists(artist)
  values ('Tears for Fears'),
        ('Periphery'),
        ('Steve Vai'),
        ('Tool'),
        ('Koji Kondo'),
        ('Legend of Zelda'),
        ('Michiru Oshima'),
        ('Buckethead'),
        ('Animals as Leaders');

insert into authors(song_id, artists_id)
  values ((SELECT id_song from song WHERE title='Everybody Wants To Rule The World'),
		(SELECT id_artists from artists WHERE artist='Tears for Fears')),

        ((SELECT id_song from song WHERE title='Marigold'),
		(SELECT id_artists from artists WHERE artist='Periphery')),

        ((SELECT id_song from song WHERE title='For the Love of God'),
		(SELECT id_artists from artists WHERE artist='Steve Vai')),

        ((SELECT id_song from song WHERE title='Schism'),
		(SELECT id_artists from artists WHERE artist='Tool')),

        ((SELECT id_song from song WHERE title='Lost Woods'),
		(SELECT id_artists from artists WHERE artist='Koji Kondo')),

        ((SELECT id_song from song WHERE title='Lost Woods'),
		(SELECT id_artists from artists WHERE artist='Legend of Zelda')),

        ((SELECT id_song from song WHERE title='Shout'),
		(SELECT id_artists from artists WHERE artist='Tears for Fears')),

        ((SELECT id_song from song WHERE title='Une lumière envoûtante'),
		(SELECT id_artists from artists WHERE artist='Michiru Oshima')),

        ((SELECT id_song from song WHERE title='Soothsayer'),
		(SELECT id_artists from artists WHERE artist='Buckethead')),

        ((SELECT id_song from song WHERE title='Lotus Island'),
		(SELECT id_artists from artists WHERE artist='Buckethead')),

        ((SELECT id_song from song WHERE title='Cafo'),
		(SELECT id_artists from artists WHERE artist='Animals as Leaders'));

insert into time_change(song_id, time_measure_id)
  values ((SELECT id_song from song WHERE title='Everybody Wants To Rule The World'),
		(SELECT id_time_measure from time_measure WHERE measure='4/4')),
        ((SELECT id_song from song WHERE title='Marigold'),
		(SELECT id_time_measure from time_measure WHERE measure='7/8')),
        ((SELECT id_song from song WHERE title='For the Love of God'),
		(SELECT id_time_measure from time_measure WHERE measure='4/4')),
        ((SELECT id_song from song WHERE title='Schism'),
		(SELECT id_time_measure from time_measure WHERE measure='12/8')),
        ((SELECT id_song from song WHERE title='Schism'),
		(SELECT id_time_measure from time_measure WHERE measure='5/8')),
        ((SELECT id_song from song WHERE title='Schism'),
		(SELECT id_time_measure from time_measure WHERE measure='6/8')),
        ((SELECT id_song from song WHERE title='Schism'),
		(SELECT id_time_measure from time_measure WHERE measure='2/4')),
        ((SELECT id_song from song WHERE title='Schism'),
		(SELECT id_time_measure from time_measure WHERE measure='4/4')),
        ((SELECT id_song from song WHERE title='Schism'),
		(SELECT id_time_measure from time_measure WHERE measure='3/8')),
        ((SELECT id_song from song WHERE title='Schism'),
		(SELECT id_time_measure from time_measure WHERE measure='8/8')),
        ((SELECT id_song from song WHERE title='Schism'),
		(SELECT id_time_measure from time_measure WHERE measure='7/8')),
        ((SELECT id_song from song WHERE title='Lost Woods'),
		(SELECT id_time_measure from time_measure WHERE measure='4/4')),
        ((SELECT id_song from song WHERE title='Shout'),
		(SELECT id_time_measure from time_measure WHERE measure='4/4')),
        ((SELECT id_song from song WHERE title='Une lumière envoûtante'),
		(SELECT id_time_measure from time_measure WHERE measure='2/4')),
        ((SELECT id_song from song WHERE title='Une lumière envoûtante'),
		(SELECT id_time_measure from time_measure WHERE measure='7/6')),
        ((SELECT id_song from song WHERE title='Une lumière envoûtante'),
		(SELECT id_time_measure from time_measure WHERE measure='8/5')),
        ((SELECT id_song from song WHERE title='Une lumière envoûtante'),
		(SELECT id_time_measure from time_measure WHERE measure='6/5')),
        ((SELECT id_song from song WHERE title='Soothsayer'),
		(SELECT id_time_measure from time_measure WHERE measure='4/4')),
        ((SELECT id_song from song WHERE title='Lotus Island'),
		(SELECT id_time_measure from time_measure WHERE measure='4/4')),
        ((SELECT id_song from song WHERE title='Lotus Island'),
		(SELECT id_time_measure from time_measure WHERE measure='2/4')),
        ((SELECT id_song from song WHERE title='Cafo'),
		(SELECT id_time_measure from time_measure WHERE measure='9/4')),
        ((SELECT id_song from song WHERE title='Cafo'),
		(SELECT id_time_measure from time_measure WHERE measure='7/4')),
        ((SELECT id_song from song WHERE title='Cafo'),
		(SELECT id_time_measure from time_measure WHERE measure='6/4')),
        ((SELECT id_song from song WHERE title='Cafo'),
		(SELECT id_time_measure from time_measure WHERE measure='5/4'));
