<?php

// $post is the package sent from unity
if (!isset($_POST['newEntry'])) {
    die("no json file found in post");
}
$jsonString = $_POST['newEntry'];
$data = json_decode($jsonString, true);
echo $data['fruit_name'];

// store connection in variable
$conn = connect();

// add new entry to database with SQL injection protection
$fruitName = $data['fruit_name'];
$color = $data['color'];
$quantity = $data['quantity'];
$stmt = $conn->prepare("INSERT INTO fruit (fruit_name, color, quantity) VALUES (:fruit_name, :color, :quantity)");
$stmt->bindParam(":fruit_name", $fruitName);
$stmt->bindParam(":color", $color);
$stmt->bindParam(":quantity", $quantity);
$stmt->execute();

// get all data from fruit database
$stmt = $conn->prepare("SELECT * from fruit"); /* also can do fruit_name, quantity, or if only want one: WHERE fruit_name = '..' */
$stmt->execute();

// fetch all results from database
$results = $stmt->fetchAll(PDO::FETCH_ASSOC);

// get all names and counts
foreach ($results as $row){
    #echo("name: " . $row['fruit_name'] . " count: " . $row['quantity']) . "<br>";
}

// try connection
function connect(){
    $dbHost = "localhost";
    $dbUsername = "root";
    $dbPassword = "";
    $dbName = "example";
    try {
        $pdo = new PDO("mysql:host=$dbHost;dbname=$dbName", $dbUsername, $dbPassword);
        $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
        return $pdo;
    } catch (PDOException $e) {
        // if connection failed
        echo "connection is not working" . $e->getMessage();
    }
}

