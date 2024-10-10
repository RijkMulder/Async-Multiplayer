<?php
$response = new stdClass();
$request = json_decode($_POST['newEntry']);
// check if incoming packet is valid
if (!isset($_POST['newEntry'])) {
    $response->status = "invalidPostData";
    $response->customMessage = "no valid data given in post array.";
    die(json_encode($response));
}

// if json has errors
if (json_last_error() != JSON_ERROR_NONE) {
    $response->status = "invalidJson";
    $response->customMessage = "provided json data could not be decoded: " . json_last_error_msg();
    die (json_encode($response));
}

$connectionResult = getDatabaseConnection();

// connection error
if (is_array($connectionResult) && isset($connectionResult['error']) && $connectionResult['error']) {
    $response->status = "dataBaseConnectionError";
    $response->customMessage = $connectionResult['message'];
    die(json_encode($response));
}

// invalid action
if (!isset($request->action) || empty($request->action)) {
    $response->status = "invalidAction";
    $response->customMessage = "The field 'action' is missing or empty in the request.";
    die(json_encode($response));
}

// check action type
switch ($request->action) {
    case "createAccount":
        include 'CreateAccount.php';  
        break;
    case "loginAccount":
        $token = generateToken();
        include 'Login.php';
        break;
    case "checkToken":
        include 'CheckLoginToken.php';
        break;
    case "logout":
        include 'Logout.php';
        break;
    case "resetPassword":
        break;
    case "getPlot":
        include 'GetPlot.php';
        break;
    case "savePlot":
        include 'SavePlot.php';
        break;
    case "checkTile":
        include 'CheckTile.php';
        break;
    default:
    $response->status = "noValidAction";
    $response->customMessage = "no valid action was given.";
    die (json_encode($response));
}

// generate login token
function generateToken($length = 64) {
    return bin2hex(random_bytes($length / 2));
}

// get connection with database
function getDatabaseConnection() {
    $dbHost = '127.0.0.1';
    $dbName = 'example';
    $dbUser = 'root';
    $dbPass = '';

    try {
        // try to make connection with database
        $pdo = new PDO("mysql:host=$dbHost;dbname=$dbName", $dbUser, $dbPass);
        $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
        return $pdo;
    } catch (PDOException $e) {
        // return errors if connection failed
        return [
            'error' => true,
            'message' => $e->getMessage()
        ];
    }
}
