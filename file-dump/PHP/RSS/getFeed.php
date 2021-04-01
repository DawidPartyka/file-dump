<?php
function getFeed($feedUrl) {
    $timeStart = date('Y-m-d H:i:s');
    $timeAdd;

    function toUTF8($input){
      return preg_replace_callback("/(&#[0-9]+;)/", function($m) {
        return mb_convert_encoding($m[1], "UTF-8", "HTML-ENTITIES");
      }, $input);
    }

    class sites{
      private $sites = ['rmf24.pl','komputerswiat.pl','xmoon.pl'];
      private $site;
      private $feedUrl;

      public function __construct($s){
        $this->feedUrl = $s;

        for($i = 0; $i < count($this->sites); $i++){
          if(strpos($this->feedUrl, $this->sites[$i]) !== false) $this->site = $i;
        }
      }

      public function returnSite(){
        return $this->sites[$this->site];
      }

      public function returnXML(){
        $rss = file_get_contents($this->feedUrl);
        if(strpos($rss,'<?xml') === false) {
          return false;
        }
        else{
          return new SimpleXmlElement($rss);
        }
      }
    }

    class articles{
      private $title;
      private $articleContent;
      private $date;

      public function __construct($t, $a, $d){
        $this->title = $t;
        $this->articleContent = $a;
        $this->date = substr(date('Y-m-d h:i:s', strtotime($d)), 0, -1);
      }

      public function __toString(){
        return $this->title."</br>".$this->articleContent."</br>".$this->date." ".$this->time."</br>"."</br>";
      }

      public function addToBase($url){
        require("config.php");
        $artTitle = toUTF8($this->title);
        $artContent = toUTF8($this->articleContent);
        $st = $url->returnSite();
        $timeAdd = false;

        $sql = "CALL checkArticle('$artTitle','$artContent');";
    		$result = $conn->query($sql);

        if($result){
          $smth = $result->fetch_assoc();
          $result->close();
          $conn->next_result();
        }

        $sql = "call checkSite('$st');";
    		$result = $conn->query($sql);

        if($result){
          $articlesAmount = $result->fetch_assoc();
          $result->close();
          $conn->next_result();
        }

      	if(!implode($smth) && implode($articlesAmount) < 5){
          $sql = "call addArticle('$artTitle', '$artContent', '$st', '$this->date');";
          $result = $conn->query($sql);

          if($result){
            $conn->next_result();
          }

          if($result === TRUE){
            $sql = "call findArticle('$artTitle', '$artContent');";
            $result = $conn->query($sql);

            if($result){
              $idArticle = $result->fetch_assoc();
              $timeAdd = date('Y-m-d H:i:s')."\nArticle ID: ".implode($idArticle);
              $conn->next_result();
              $result->close();
            }

            echo "Dodano do bazy"."</br>";
          }
        }
        else{
          if(implode($articlesAmount) == 5){
            echo "W bazie znajduje się już 5 artykułów z tej strony";
			$conn->close();
            return "cap";
          }
          else echo "Artykuł znajduje się w bazie. Nastąpi próba znalezienia innego artykułu</br></br>";
        }

        $conn->close();
        return $timeAdd;
      }
    }

    $url = new sites($feedUrl);
    $content = $url->returnXML();

    if($content !== false){

      if(isset($content->channel->item)){
        foreach($content->channel->item as $entry) {
          $date = substr($entry->pubDate,strpos($entry->pubDate,","));
          $date = date('Y-m-d h:i:s', strtotime($date));
          $article = new articles($entry->title,$entry->description,$date);

          $timeAdd = $article->addToBase($url);

          if($timeAdd !== false || $timeAdd == 'cap') break;
          else $article->try = true;
        }
      }
      else if(isset($content->entry)){
        foreach($content->entry as $entry) {
            if(isset($content->entry->summary)){
              $date = str_replace("T"," ", str_replace("Z"," ", $entry->published));
              $article = new articles($entry->title,$entry->summary,$date);
            }
            else if(isset($content->entry->content)){
              $article = new articles($entry->title,$entry->content,$entry->published);
            }

            $timeAdd = $article->addToBase($url);

            if($timeAdd !== false || $timeAdd == 'cap') break;
          }
      }
    }
    else{
      echo "Nierozpoznany url. Jeśli podany link to http://www.komputerswiat.pl/rss-feeds/komputer-swiat-feed.aspx proszę spróbować http://www.komputerswiat.pl/.feed";
    }

    if($timeAdd === false || $timeAdd === 'cap'){
      $timeAdd = 'None';
    }

    $log = "Start: ".$timeStart."\ninsert time: ".$timeAdd."\nEnd: ".date('Y-m-d H:i:s')."\n\n";
    file_put_contents('./log_'.date("Y-m-d").'.log', $log, FILE_APPEND);
}
?>
