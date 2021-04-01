<?php
if(isset($_GET['feed'])){
  $feed = $_GET['feed'];

  include("getFeed.php");

  getFeed($feed);
}
?>
