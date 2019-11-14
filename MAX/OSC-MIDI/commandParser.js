autowatch = 1;
outlets = 7;
intlets = 2;

setoutletassist(0,"Tempo Clock On/Off");
setoutletassist(1,"Tempo Value");
setoutletassist(2,"Sounds at Position");
setoutletassist(3,"Matrix Debug");
setoutletassist(4,"PreViewSound");
setoutletassist(5,"Init");
setoutletassist(6,"Volume Port");



post("Init");

var soundMatrixLength = 16;
var wei = "hello";
var soundMatrix = new Array();
for (var i = 0; i < soundMatrixLength; i++) {
	soundMatrix[i] = new Array();
}


function resetMatrix(){
	soundMatrix = new Array();
	for (var i = 0; i < soundMatrixLength; i++) {
		soundMatrix[i] = new Array();
	}
	outlet(5,1);
}

function bang() {
  if (wei == "hello") {
    wei = "what";
  } else {
    wei = "hello";
  }
  outlet(0, wei);
}

function State(state) {
  outlet(0, state);
}

function BPM(bpmVal) {
  if (bpmVal > 0) {
    outlet(1, bpmVal);
  }
}

function AddSound(colum, chanel, pitch, vel, dur) {
	cursound = [chanel, pitch, vel, dur]
	post(1);
  if (soundMatrix[colum].indexOf(cursound) == -1) {
    soundMatrix[colum].push(cursound);
  }
  outlet(3, "Add Channel " + chanel + " at " + colum);
}

function RemoveSound(nam, colum) {
	var loc = soundMatrix[colum].indexOf(nam);
  if (loc != -1) {
    soundMatrix[colum].splice(loc,1);
  }
  outlet(3, "Remove Channel " +nam + " at " + colum);
}

function PreviewSound(chanel, pitch, velocity){
	outlet(4,[chanel, pitch, velocity]);
}

function PrintColumn(colum) {
  var output = [];
  for (var j = 0; j < soundMatrix[colum].length; j++) {
    output.push(soundMatrix[colum][j].join(' '));
  }
  outlet(2, [output.join(";")]);
}

function PrintMatrix() {
  var output = "";
  for (var i = 0; i < soundMatrix.length; i++) {
    output = output + " column :" + i+" : ";

    for (var j = 0; j < soundMatrix[i].length; j++) {
		
		output = output + "Sound:"
		
		for (var k = 0; k < soundMatrix[i][j].length; k++) {
      		output = output + " "+ soundMatrix[i][j][k];
		}
		
		output = output + " ; ";
    }
		output = output + "\n";
  }
  outlet(3, output);
}
