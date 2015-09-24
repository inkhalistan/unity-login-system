<?php
$link = mysql_connect("host", "username", "password");

$username = $_REQUEST['username']; 
$password = $_REQUEST['password']; 

mysql_connect($link) or die(mysql_error()); 
$db_selected = mysql_select_db("databas", $link);

$check = mysql_query("SELECT * FROM `users` WHERE `username`='".$username."'" ) or die (mysql_error());
$numrows = mysql_num_rows($check);
if ($numrows == 0)
	die("Username doesn't exist \n");
else {
	$password = password_hash($password, PASSWORD_BCRYPT);
	while ($row = mysql_fetch_assoc($check)) {//finds the rows that have our username
		if($password == $row['password'])
			die("login-SUCCESS".$row['id']);
		else
			die("Password doesn't match \n");
	}
}
?>
