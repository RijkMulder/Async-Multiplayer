<?php

// isset checks if variable key json 
// post is from the unity function
if (!isset($_POST['newHighscore'])) {
    die("Key json does not exist in post array");
}

$names = ["Jan", "Dik", "Henk", "kanker", "nijntje", "kurwatje", "bober"];

# new standard class
$villager = new stdClass();

/* variables of new classes can be declared like this
 if converting to json variable names must be exactly same as in unity */
$villager->name = $names[(rand(0, 6))];
$villager->age = Rand(18, 60);
$villager->craftSkill = Rand(0, 100);
$villager->fightSkill = Rand(0, 100);

echo (json_encode($villager));