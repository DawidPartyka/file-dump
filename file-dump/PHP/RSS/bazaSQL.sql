CREATE TABLE sites(
  id_sites int NOT NULL PRIMARY KEY AUTO_INCREMENT,
  site varchar(255)
);

CREATE TABLE articles(
  id_articles int NOT NULL PRIMARY KEY AUTO_INCREMENT,
  title varchar(255) NOT NULL,
  content text NOT NULL,
  sites_id int NOT NULL,
  articleTimeStamp timestamp NOT NULL,
  databaseTimeStamp timestamp DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  FOREIGN KEY (sites_id) REFERENCES sites(id_sites)
);


INSERT INTO sites (site)
  VALUES
    ("xmoon.pl"),
    ("komputerswiat.pl"),
    ("rmf24.pl");

DELIMITER //
CREATE OR REPLACE PROCEDURE addArticle(IN artTitle TEXT, artContent TEXT, siteName TEXT, artTime TIMESTAMP)
BEGIN
  INSERT INTO articles (title, content, sites_id, articleTimeStamp)
  VALUES (
    artTitle,
    artContent,
    (SELECT id_sites FROM sites WHERE site = siteName),
    artTime
  );
  COMMIT;
END //
DELIMITER ;

DELIMITER //
CREATE OR REPLACE PROCEDURE checkArticle(IN artTitle TEXT, artContent TEXT)
BEGIN
  SELECT COUNT(*) FROM articles
  WHERE title = artTitle AND content = artContent;
  COMMIT;
END //
DELIMITER ;

DELIMITER //
CREATE OR REPLACE PROCEDURE checkSite(IN artSite TEXT)
BEGIN
  SELECT COUNT(*) FROM articles
  WHERE sites_id = (SELECT id_sites FROM sites WHERE site = artSite);
  COMMIT;
END //
DELIMITER ;
