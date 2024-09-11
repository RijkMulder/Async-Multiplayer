
<?php  
class Pyramid {
        Function MakePyramid($height){
            $pyrWidth = $height + 1;
            $canvasWidth = $pyrWidth + 1;
            for ($y = $height; $y > 0; $y--) {
                for ($x = $canvasWidth; $x > 0; $x--) {
                    $pixelID = ($x > $canvasWidth - $y) ? "🟦" : "🧱";
                    echo ($pixelID);
                }
                echo "<br>";
            }
        }
}
$p = new Pyramid();
$p-> MakePyramid(10);
?>