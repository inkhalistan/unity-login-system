<?php
$username = $_REQUEST['username']; 
$password = $_REQUEST['password']; 
$email = $_REQUEST['email'];
$host = 'localhost'; 
$user = 'secondhand_tcg'; 
$dbpassword = '111111'; 
$db = 'secondhand_tcg'; 
$table = 'users'; 

mysql_connect($host, $user, $dbpassword) or die(mysql_error()); 
mysql_select_db($db); 

$password = md5($password); 
mysql_query("INSERT INTO `users` VALUES ('NULL', '{$username}', '{$password}', '{$email}')") or die (mysql_error());

$findid = mysql_query("SELECT * FROM `users` WHERE `username`='".$username."'" ) or die (mysql_error());
$numrows = mysql_num_rows($findid);
$message = "";
if ($numrows == 0)
	die(" the new user not found \n");
else {
	while($row = mysql_fetch_assoc($findid))
		$userid = $row['id'] ;  break;

$startingcollectioncards = array(0,0,1,1,2,3,3,4,5,6,0,1,2,3,4,5,6,7,8,9,10,10,11,11,11);
	foreach ($startingcollectioncards as $card)
		mysql_query("INSERT INTO `player_collections` VALUES ('NULL', '{$userid}', '{$card}')") or die (mysql_error()); 

	$startingdeckcards = array(0,1,2,3,4,5,6,7,8,9,10,0,1,2,3,4,5,6,7,8,9,10,11);
	foreach ($startingdeckcards as $card)
		mysql_query("INSERT INTO `player_decks` VALUES ('NULL', '{$userid}', '{$card}')") or die (mysql_error()); 
   
	$query = "INSERT INTO silver VALUES('NULL', '{$userid}', '150')"; 
	$result = mysql_query($query) or die('Query failed: ' . mysql_error()); 
			
echo "done, userid:".$userid."";
}
?>