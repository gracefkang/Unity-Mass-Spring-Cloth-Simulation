# Unity Mass-Spring Cloth Simulation

### Option 1. Spring-Mass Cloth simulation
Uploaded on github here: https://github.com/gracefkang/Unity-Mass-Spring-Cloth-Simulation

Desc:
Basic mass-spring cloth simulation. You can anchor the cloth to something by giving it the Anchor component. Also added basic collision for spheres.

How to run:
Timing parameters are located in Cloth.cs
Spring parameters are located in Spring.cs
Mass parameters are located in Mass.cs

- Script is located on gameobjects default and default2 in the Cloth component
- Integrator can be chosen in editor
- press play button to see simulation

Notes:
- Did not implement bending springs because I couldn't figure out how to access the i+1th vertex of each direction in Unity's Mesh data :(

Observations/thoughts:
- increasing the stiffness decreased the stretchiness of the cloth.
- increasing damping lowered the amount of time it took for the sim to come to rest. could be described as the "bounciness" of the cloth.
- the cloth tends to fall in on itself. could be due to lack of bendings springs or lack of handling self-collisions.

### Option 3. Experiment with different integrators

See:
- IntegrateEulerExplicit
- IntegrateVerletExplicit
- IntegrateSymplectic

Observations/thoughts:
- Euler tended to "spin out" of control with higher stiffness settings
- Euler also had some jittering around the corners of pinned cloth.
- Verlet had weirder, less realistic, "bouncier" results but was more stable than Euler.
- Symplectic was both stable and more realistic, probably because it was the semi-implicit method out of the 3 options.