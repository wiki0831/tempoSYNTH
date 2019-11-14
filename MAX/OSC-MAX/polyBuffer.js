var pb = new PolyBuffer(jsarguments[1]);       // PolyBuffer instantiates a polybuffer~ object named by second argument to js  

var dump = pb.getshortname();

outlets = 2;


// "open" is a js message, so instead we use "openwindow" to open polybuffer~'s window.
function openwindow()
{
    pb.open();
}

function IndexToName(index){
	outlet(1,dump[index*2 -1]);
}

function NameToIndex(nam){
	var dump = pb.getshortname();
	for (var i = 0; i < (pb.count*2); i++) {
		if(dump[i] == nam){
		outlet(0,(i+1)/2);
		}
	}
}

// wclose is a js message, so instead we use "closewindow" to close polybuffer~'s window.
function closewindow()
{
    pb.wclose();
}

function getcount()
{
    post("Number of items in the polybuffer:", pb.count);    // read-only attribute which reports the number of buffer~s in polybuffer~.
    post();
}

function getsize()
{
    post("Memory used in the polybuffer:", pb.size, " bytes\n");    // read-only attribute which reports the memory usage (Bytes) of polybuffer~.
}

function anything()
{
    // dynamically send any message to the polybuffer~ object
    if (arguments.length > 0) {
        eval("pb." + messagename + "(arrayfromargs(arguments))");
    } else
        eval("pb." + messagename + "()");
}

function notifydeleted()
{    
    pb.freepeer();    // delete the polybuffer
}

function printany(name)
{
    var tmp = new PolyBuffer(name); // instantiate new polybuffer~ with name "name". if named polybuffer~ exists in patcher, then link to it.
    outlet(1,tmp.getshortname());
	// post the polybuffer~'s contents to Max window
    tmp.freepeer();    // delete the polybuffer~
}