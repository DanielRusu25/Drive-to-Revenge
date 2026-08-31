# Overview
Drive to Revenge is a racing game made in Unity using C#. It has complex features such us:  
* Realistic driving  
* A system to purchase and modify cars  
* Multiple types of races  
* Ai drivers to race against  
* Save system  
* Big open world map  

# How everything works

## Driving controller
The driving in this game is mostly realistic rather than arcade, taking in consideration things like the cars weight, horsepower, engine power and torque curves, drivetrain (FWD,RWD or AWD), transmission type (manual or automatic), gear ratios, steering angle, brake pads' torque, suspension's damper and stiffness and the grip of the tires or the terrain.

In order to make the car move, we first need to find the engine's current torque output. This requires a physically grounded approach that calculates mechanical force backward from the wheels to the engine:  
* Wheel Kinematics: We start by finding the average rotational speed (RPM) of the driven wheels, which dynamically adapts based on the car's drivetrain layout (FWD, RWD, or AWD). This wheel speed is then multiplied by the current gear and final drive ratios to simulate the mechanical link through the transmission.  
 Engine Inertia: To simulate the heavy, mechanical feel of a real combustion engine, the engine RPM does not instantly snap to match the wheels. Instead, it smoothly interpolates toward the target speed, ensuring it never drops below the vehicle's base idle state.  
* Dynamic Torque Calculation: Once the true engine RPM is established, it is fed into a mathematical model utilizing real-world automotive formulas ($Torque = \frac{Horsepower \times 5252}{RPM}$). The system evaluates the car's specific horsepower curve, converts that power into torque, and multiplies it by the transmission ratios. Finally, this raw mechanical force is modulated by the player's throttle input and performance upgrades (like nitrous) to deliver realistic, responsive acceleration to the wheels.  

$$ \text{Output Torque} = \left( \frac{\text{Current HP} \times 5252}{\text{Engine RPM}} \right) \times \text{Gear Ratio} \times \text{Final Drive} \times \text{Throttle} \times \text{Nitrous} $$

**Where:**
*   Current HP: Evaluated dynamically from the engine's custom power curve at the current RPM.
*   5252: The real-world constant used to convert horsepower to foot-pounds of torque.
*   Gear Ratio & Final Drive: The mechanical multipliers provided by the transmission.
*   Throttle & Nitrous: Player-driven inputs that modulate the final mechanical force.

## Tuning and purchase system
There are a total of 5 cars to modify, each with visual and performance upgrades that can be purchased using the in game currency.

**Visual upgrades** consists of 3 different wheel types and 16 colors for each vehicle, being able to change and save at runtime.

**Performance upgrades** affect every aspect inside the car controller and they are split in 2 categories: parts and tuning.  
Inside the **parts** submenu there are 8 categories that have multiple levels of performance. These categories are: 
 * engine -> changes the HP
 * brakes -> changes the brake torque
 * transmission -> changes the numbers of gears
 * driveshaft -> changes the time required to shift gears
 * drivetrain -> swaps between FWD, RWD, AWD
 * suspension -> unlocks better tuning capabilities
 * tyers -> changes the grip levels
 * nitrous -> changes the power increase gained from the nitrous

   
Inside the **tune** submenu there are 4 categories that you can insert custom values for:
 * gear ratios and final drive ratio
 * transmission type: manual vs automatic
 * Handling: maximum steer angle and the ride height of the car
 * Suspension: the stiffness and damper
The last 2 are affected in terms of how much customization the player has access depending on what level of suspension the player has bought inside the parts menu.

Everything was tied together in a simple menu using Unity's uGui system, taking in consideration color theory to make everything look beautiful.  
Every value found inside this menu, ranging from the costs of each part, to the value that changes the performance of the car can be done without entering the code, neatly using a custom editor.

 ![First image collage of upgrade menu](Images-for-Github/1.png)
 ![Second image collage of upgrade menu](Images-for-Github/2.png)


 
