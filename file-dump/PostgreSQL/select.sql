/* Select wyświetlający id, tytuł, strojenie i gatunek transkrypcji
utworów zapisanych za pomocą tabulatur posortowane według tytułu*/
SELECT song.id_song, song.title, genres.genre, tuning.tuning from song
RIGHT JOIN songscript ON songscript.song_id = song.id_song
RIGHT JOIN genres ON song.genres_id = genres.id_genres
RIGHT JOIN tuning ON tuning.id_tuning = song.tuning_id
RIGHT JOIN script_type ON script_type.id_script_type = songscript.script_type_id
WHERE script_type.type = 'Tabulatura' ORDER BY song.title;

/* Select wyświetlający id i nazwę gatunków które pojawiają się
w więcej niż jednym utworze*/
SELECT genres.id_genres, genres.genre, COUNT(song.genres_id) FROM song
RIGHT JOIN genres ON song.genres_id = genres.id_genres
GROUP BY genres.genre, genres.id_genres
HAVING COUNT(song.genres_id) > 1;

/* Select wyświetlający id i tytuły utworów które mają
przynajmniej 4 różne rytmy w transkrypcji*/
SELECT song.id_song, song.title, count(time_change.song_id) FROM song
RIGHT JOIN time_change ON song.id_song = time_change.song_id
RIGHT JOIN time_measure
ON time_measure.id_time_measure = time_change.time_measure_id
GROUP BY song.id_song
HAVING COUNT(time_change.song_id) > 3;

/* Select wyświetlający ilość transkrypcji w nutach oraz tabulaturach*/
SELECT ste.type, COUNT(sst.script_type_id) AS ilość
FROM script_type AS ste
LEFT JOIN songscript AS sst ON ste.id_script_type = sst.script_type_id
GROUP BY type;

/* Select wyświetlający tytuły i typ transkrypcji dla utworów posiadających
tylko jeden typ transkrypcji posortowane według typu zapisu*/
SELECT title, s.id_song as id_utworu, type FROM song s
LEFT JOIN songscript sst ON sst.song_id = s.id_song
LEFT JOIN script_type ste ON ste.id_script_type = sst.script_type_id
GROUP BY type, s.id_song, title
HAVING (SELECT count(*) FROM songscript
		WHERE songscript.song_id = s.id_song) = 1
ORDER BY type;

/* Select wyświetlający wykonawców posiadających dwie lub więcej
transkrypcji wraz z tytułami ich utworów posortowanych według id wykonawcy*/
SELECT title, artist, ar.id_artists as id_wykonawcy FROM song s
LEFT JOIN authors au ON s.id_song = au.song_id
LEFT JOIN artists ar ON au.artists_id = ar.id_artists
GROUP BY ar.id_artists, title, artist
HAVING (SELECT count(*) FROM authors
		WHERE authors.artists_id = ar.id_artists) >= 2
ORDER BY ar.id_artists;

/* Select wyświetlający utwory, wykonawców i gatunek utworów
posiadających dwóch lub więcej wykonawców*/
SELECT title, artist, s.id_song as id_utworu, g.genre FROM song s
LEFT JOIN genres g ON s.genres_id = g.id_genres
LEFT JOIN authors au ON s.id_song = au.song_id
LEFT JOIN artists ar ON au.artists_id = ar.id_artists
GROUP BY s.id_song, title, artist, g.genre
HAVING (SELECT count(*) FROM authors WHERE authors.song_id = s.id_song) >= 2;

/* Select wyświetlający tytuł, id utworu, liczbę strun, strojenie instrumentu, rodzaj instrumentu i instrumentu
dla transkrypcji wyłącznie dla gitary elektrycznej lub basu elektrycznego*/
SELECT title, id_song as id_utworu, t.tuning, strings, type as instrument, kind as rodzaj FROM song s
LEFT JOIN tuning t ON s.tuning_id = t.id_tuning
LEFT JOIN authors au ON s.id_song = au.song_id
LEFT JOIN guitartype gt ON gt.id_guitartype = s.guitartype_id
WHERE gt.type = 'gitara' AND kind = 'elektryczna'
UNION
SELECT title, id_song as id_utworu, t.tuning, strings, type as instrument, kind as rodzaj FROM song s
LEFT JOIN tuning t ON s.tuning_id = t.id_tuning
LEFT JOIN authors au ON s.id_song = au.song_id
LEFT JOIN guitartype gt ON gt.id_guitartype = s.guitartype_id
WHERE gt.type = 'bas' AND kind = 'elektryczny'
ORDER BY strings DESC;

/*select do znajdowania po rytmie*/
SELECT title, id_song as id_utworu, measure FROM song s
LEFT JOIN time_change tc ON tc.song_id = s.id_song
RIGHT JOIN time_measure tm ON tc.time_measure_id = tm.id_time_measure
WHERE measure = '4/4' OR measure = '5/4';

/*select do zwracania po części tytułu lub artysty*/
SELECT title, id_song as id_utworu, artist FROM song s
LEFT JOIN authors au ON au.song_id = s.id_song
LEFT JOIN artists ar ON ar.id_artists = au.artists_id
WHERE title LIKE '%oo%' OR artist LIKE '%oo%'
ORDER BY title;

/* Procedura zmieniająca tytuł */
CREATE OR REPLACE PROCEDURE ChangeTitle(INT, TEXT)
LANGUAGE plpgsql
AS $$
BEGIN
    UPDATE song
    SET title = $2
    WHERE id_song = $1;

    COMMIT;
END;
$$;
--CALL ChangeTitle(7, 'Une lumiere envoutante');

/*TRIGGER*/
CREATE OR REPLACE FUNCTION checkGuitarType() returns trigger as
$$
begin
	if NEW.type != LOWER(NEW.type) OR NEW.kind != LOWER(NEW.kind) then
		RAISE 'Użyj wyłącznie małych liter dla atrybutu type, oraz kind';
		RETURN OLD;
	else
		RETURN NEW;
	end if;

end;
$$
language plpgsql;
create trigger checkGuitarType after insert or update on guitartype
for each row execute procedure checkGuitarType();

--insert into guitartype(type, kind)
 -- values ('GiTara','eLEKTROakUStyczna');

 --insert into guitartype(type, kind)
 -- values ('gitara','elektroakustyczna');


/* Funkcja do zliczania ilości zmian rytmu w utworze */
CREATE OR REPLACE FUNCTION rythmCount(x integer) RETURNS integer AS $$
        BEGIN
                RETURN CAST((SELECT COUNT(*) FROM time_change
						WHERE song_id = x) as integer);
        END;
$$ LANGUAGE plpgsql;

--SELECT * FROM rythmCount(4);


/*Select z wcześniej wykorzystujący funkcje*/
SELECT song.id_song, song.title, count(time_change.song_id) FROM song
RIGHT JOIN time_change ON song.id_song = time_change.song_id
RIGHT JOIN time_measure
ON time_measure.id_time_measure = time_change.time_measure_id
WHERE (SELECT rythmCount(id_song)) > 3
GROUP BY song.id_song;
