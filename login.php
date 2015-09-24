<?php
$username = $_REQUEST['username']; 
$password = $_REQUEST['password']; 
$host = 'localhost'; 
$user = 'secondhand_tcg';  
$dbpassword = '111111';
$db = 'secondhand_tcg'; 

mysql_connect($host, $user, $dbpassword) or die(mysql_error()); 
mysql_select_db($db); 

$check = mysql_query("SELECT * FROM `users` WHERE `username`='".$username."'" ) or die (mysql_error());
$numrows = mysql_num_rows($check);
if ($numrows == 0)
	die("Username doesn't exist \n");
else {
	$password = md5($password);
	while ($row = mysql_fetch_assoc($check)) {//finds the rows that have our username
		if($password == $row['password'])
			die("login-SUCCESS".$row['id']);
		else
			die("Password doesn't match \n");
	}
}
?>
