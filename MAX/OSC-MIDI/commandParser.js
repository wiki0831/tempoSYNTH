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


var numChannels = 5;
var soundLength = 16;
var soundMatrix = new Array();

Init();

function Init(){
	post("Init\n");
	for (var i = 0; i < numChannels; i++) {
		soundMatrix[i] = new Array();
		for (var j = 0; j < soundLength; j++){
			soundMatrix[i][j] = new Array();
		}
	}
	outlet(5,1);
}

function resetMatrix(){
	Init();
}

function State(state) {
  outlet(0, state);
}

function BPM(bpmVal) {
  if (bpmVal > 0) {
    outlet(1, bpmVal);
  }
}

function PreviewSound(chanel, pitch, velocity){
	outlet(4,[chanel, pitch, velocity]);
}

function AddSound(colum, chanel, pitch, vel, dur) {
	cursound = [pitch, vel, dur]

  	if (soundMatrix[chanel][colum].length == 0) {
    soundMatrix[chanel][colum].push(cursound);
	outlet(3,"Channel "+chanel+" is Empty, adding: " + cursound)
	return;
  	}

	if (soundMatrix[chanel][colum].length != 0){
	for (var i = 0; i < soundMatrix[chanel][colum].length; i++) {
		if(soundMatrix[chanel][colum][i][0] == pitch){
			outlet(3,"Channel "+chanel+" exist Duplicate:" + cursound);
			return;
		}
	}	
		soundMatrix[chanel][colum].push(cursound);
		outlet(3,"Channel "+chanel+" polyphony, adding: " + cursound)
		
	}
}

function RemoveSound(colum, chanel, pitch) {
	

	if (soundMatrix[chanel][colum].length == 0) {
	outlet(3,"Channel "+chanel+" is Empty, nothing to remove")
	return;
  	}
	if (soundMatrix[chanel][colum].length != 0){
	for (var i = 0; i < soundMatrix[chanel][colum].length; i++) {
		if(soundMatrix[chanel][colum][i][0] == pitch){
			soundMatrix[chanel][colum].splice(i,1);
		  	outlet(3, "Remove Channel: " +chanel + ", at: " + colum + ", with pitch:" + pitch);
			return;
		}
	}
	}
	outlet(3, "Remove Sound Fail, element not found")
}


function PrintColumn(colum) {
  var output = [];
  for (var i = 0; i < soundMatrix.length; i++) {
	
	for(var j = 0; j < soundMatrix[i][colum].length; j++){
		var midi = [];
		midi.push(i);
		midi.push(soundMatrix[i][colum][j].join(' '));
		output.push(midi.join(' '));
  		}
	}
  if(output.length > 0){
  outlet(2, [output.join(";")]);
  }
}

function PrintMatrix() {
  var output = "";
  for (var i = 0; i < soundMatrix.length; i++) {
    output = output + " Channel :" + i+"\n";

    for (var j = 0; j < soundMatrix[i].length; j++) {
		
		output = output
		
		for (var k = 0; k < soundMatrix[i][j].length; k++) {
      		output = output + " "+ soundMatrix[i][j][k];
		}
		
		output = output + " ; ";
    }
		output = output + "\n";
  }
  outlet(3, output);
}
