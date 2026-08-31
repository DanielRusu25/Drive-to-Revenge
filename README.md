# Overview
Drive to Revenge is a racing game made in Unity using C#. It has complex features such us:  
* Realistic driving  
* A system to modify cars 
* Ai drivers to race  
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
