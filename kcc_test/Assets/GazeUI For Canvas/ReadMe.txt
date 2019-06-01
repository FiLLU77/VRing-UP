Introduction :
This is a simple asset using raycasting to provide a Gaze UI working with Unity's UI controls.
There is a simple demonstration accessible on youtube : https://www.youtube.com/watch?v=8U4ChekAgnQ
Feel free to contact me is you have any question : youe.graillot@gmail.com

How to integrate ?

Just drag and drop the "Gaze" prefab in your scene and select the desired Camera in the Canvas.
You will see a square sight at the center of your screen which permit you to select any UI controls.
As the "EyeRaycaster.cs" script disable the prefab if no VR headset is found, you can enable [Force Active] option if you want to use the asset without VR.

How does it work ?

This GazeUI uses a raycast (from the sight at the center of the screen) which permit to select Unity's UI controls such as buttons or sliders.
A growing image (a cyan circle by default) indicates the moment when the selected UI componenent will be activated.
A Slider's value is incremented and is set to 0 when the maximum value is exceeded.

Customization

You will find five accessible variables in the editor :
	Float loadingTime - Define the amount of time the user must stare his target before activating
	Float loadingTime - Define the slider's increment value
	Color activeColor - Set the color of the growing circle
	Curve curve - Define the evolution of growing circle's expansion
	Bool forceActive - Enable the Gaze object even if no headset is detected
	
You can modify the IndicatorFill's RawImage to customize the growing image and the Center's RawImage to customize the sight.