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


function UpdateVolume(nam, state){
	outlet(6, [nam, state]);
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

function AddSound(nam, colum) {
  if (soundMatrix[colum].indexOf(nam) == -1) {
    soundMatrix[colum].push(nam);
  }
  outlet(3, nam + " at " + colum);
}

function RemoveSound(nam, colum) {
	var loc = soundMatrix[colum].indexOf(nam);
  if (loc != -1) {
    soundMatrix[colum].splice(loc,1);
  }
  outlet(3, nam + " at " + colum);
}

function PreviewSound(nam, state){
	outlet(4,[nam, state]);
}

function PrintColumn(colum) {
  var output = [];
  for (var j = 0; j < soundMatrix[colum].length; j++) {
    output.push(soundMatrix[colum][j]);
  }
  outlet(2, output);
}

function PrintMatrix() {
  var output = "";
  for (var i = 0; i < soundMatrix.length; i++) {
    output = output + " column :" + i;

    for (var j = 0; j < soundMatrix[i].length; j++) {
      output = output + " " + soundMatrix[i][j];
    }
		output = output + ";\n";
  }
  outlet(3, output);
}
