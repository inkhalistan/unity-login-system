<?php
$link = mysql_connect("host", "username", "password");
$db_selected = mysql_select_db("databas", $link);
$table = "myTable";

$username = $_REQUEST['username']; 
$password = $_REQUEST['password']; 
$email = $_REQUEST['email'];
function fetchinfo($rowname,$tablename,$finder,$findervalue) {
	if($finder == "1") $result = mysql_query("SELECT $rowname FROM $tablename");
	else $result = mysql_query("SELECT $rowname FROM $tablename WHERE `$finder`='$findervalue'");
	$row = mysql_fetch_assoc($result);
	return $row[$rowname];
}

/* The \b in the pattern indicates a word boundary, so only the distinct
 * word "web" is matched, and not a word partial like "webbing" or "cobweb" */
if (preg_match("/^[a-zA-Z0-9\s\.,!?]*$/", $password) {
    echo "Password was good.";
} else {
    die("Password is not valid.");
}


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
		
	$startingdeckcards = array(0,1,2,3,4,5,6,7,8,9,10,0,1,2,3,4,5,6,7,8,9,10,11);
	foreach ($startingdeckcards as $card)
		mysql_query("INSERT INTO `player_decks` VALUES ('NULL', '{$userid}', '{$card}')") or die (mysql_error()); 
   
	$query = "INSERT INTO silver VALUES('NULL', '{$userid}', '150')"; 
	$result = mysql_query($query) or die('Query failed: ' . mysql_error()); 
			
echo "done, userid:".$userid."";
}
?>
