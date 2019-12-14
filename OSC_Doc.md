# API documentation that handles OSC message between Unity and MAX

Each of the follwing functions works similar to POST request in HTTP protocal.

## Unity Sending / Max Recieving

/[Function Name] [param1] [param2] ...

- ```AddSound(int colum, int chanel, int pitch, int vel, int dur)```
This function is called when a sound sample cube is inserted to a track in unity.
   * colum: the location of the sound sample cube inserted to the track(from the track)
   * chanel: the corresponding channel to the sound sample in Ableton
   * pitch: the pitch of the sound sample
   * vel: the velocity of the sound sample
   * dur: the duration of the sound sample

- ```RemoveSound(int colum, int chanel, int pitch)```
This function is called when a sound sample cube is removed from a track in unity.
   * colum: the location of the sound sample cube inserted to the track(from the track)
   * chanel: the corresponding channel to the sound sample in Ableton
   * pitch: the pitch of the sound sample

- ```UpdateBPM(int BPM)```
update BPM using the slider
  * BPM: a new int value modified by user

- ```PreviewSound(int chanel, int pitch, int velocity)```
This function allows user to preview sound when they reach controller to the soundcube
   * chanel: the corresponding channel to the sound sample in Ableton
   * pitch: the pitch of the sound sample
   * vel: the velocity of the sound sample


- ```UpdateState(bool newState)```
  This function turns on and off the timer in Max
  - newState: indicates if the timer should be on or off

- ```resetMax()```
  This funtion is called at every starts and end of the environment, it clears the all the states and sound matrix in Max


### Max Sending /  Unity Recieving
- ```/metro int i```
  This function sends clock ticks to Unitmmm, n$,m,,mm $y, I.E. 1,2,3,4, or 1,2,3....16 (I think this could be done in a differentway)
    - i: the location of clock tick

- ```/file (int channel) (string sampleName) (int min) (int max)```
This function is called at every start of the environment, it parses the sound sample folder and send it to unity so it can populate sound samples dynamically.
    - channel: the integer value of the channel
    - sampleName: the name of sound sample from folder.
    - min: the minimum range of MIDI notes
    - max: the Maximum range of MIDI notes
