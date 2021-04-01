CREATE TABLE sites(
  id_sites int NOT NULL PRIMARY KEY AUTO_INCREMENT,
  site varchar(255)
);

CREATE TABLE dates(
  id_dates int NOT NULL PRIMARY KEY AUTO_INCREMENT,
  dateAdd date
);

CREATE TABLE articles(
  id_articles int NOT NULL PRIMARY KEY AUTO_INCREMENT,
  title varchar(255) NOT NULL,
  content text NOT NULL,
  sites_id int NOT NULL,
  articleTime time NOT NULL,
  databaseTimeAdd time NOT NULL,
  FOREIGN KEY (sites_id) REFERENCES sites(id_sites)
);

CREATE TABLE articleDates(
  articles_id int NOT NULL,
  articleDates_id int NOT NULL,
  databaseDates_id int NOT NULL,
  FOREIGN KEY (articleDates_id) REFERENCES dates(id_dates),
  FOREIGN KEY (databaseDates_id) REFERENCES dates(id_dates),
  FOREIGN KEY (articles_id) REFERENCES articles(id_articles)
);

INSERT INTO sites (site)
  VALUES
    ("xmoon.pl"),
    ("komputerswiat.pl"),
    ("rmf24.pl");


/*
$1 = title,
$2 = content,
$3 = site,
$4 = article time
$5 = article date
*/
CREATE OR REPLACE PROCEDURE addArticle(TEXT, TEXT, TEXT, TIME, DATE)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO articles (title, content, sites_id, articleTime, databaseTimeAdd)
    VALUES (
      $1,
      $2,
      (SELECT id_sites FROM sites WHERE site = $3),
      $4,
      (SELECT CONVERT (time, SYSDATETIME()))
    );
    COMMIT;
END;
$$;
