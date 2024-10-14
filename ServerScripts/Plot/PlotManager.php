<?php
// include connection check
include '../ConnectionCheck.php';

// check action type
switch ($request->action) {
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
