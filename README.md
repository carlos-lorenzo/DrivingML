# DrivingML

DrivingML is a Unity project that trains autonomous cars to drive around a circuit using neural networks and evolutionary (genetic) algorithms implemented entirely from scratch — no external machine learning libraries are used.

Te aim of this proyect was first of all learning hands on how evolutionary algorithms are implemented in combination with neural networks.

---

![Demo GIF](docs/start.gif)
##### First generations: Moving randomly

![Demo GIF](docs/end.gif)

##### After 100 generations: Driving correctly

## Overview

DrivingML simulates multiple agents (cars) on a closed circuit inside Unity. Each agent is controlled by a neural network implemented from scratch using the project's own numeric and matrix code. A genetic algorithm (selection, crossover, mutation, and elitism) evolves network parameters across generations so agents progressively improve lap time and collision avoidance.

Key design points:

- Fully self-contained: all math, neural-network classes and training logic are handwritten in C#.
- Evolutionary training: populations of agents are evaluated in the Unity simulation; top performers are selected to produce the next generation.
- Real-time visualization in the Unity Editor for observing training and evaluation runs.

## What it does

- Simulates a population of cars driving around a circuit.
- Evaluates each car with a fitness function (for example: lap time, distance traveled without collision, checkpoints reached).
- Uses genetic operations to produce new generations of neural networks controlling the cars.
- Produces models that can be loaded into the simulation for evaluation or demonstration.

## How it works (technical summary)

1. Simulation and agents
    - Each car is an agent with sensors and actuators implemented in Unity scripts. Sensors provide observations (for example: raycasts or positional inputs) which are fed into a neural network controller.

2. Neural networks (from scratch)
    - All neural network code, matrix operations, activations and layer handling are implemented directly in the project's C# scripts (no TensorFlow, PyTorch, ML-Agents, or other ML libraries).
    - The code organizes networks into layers and neurons and computes forward passes deterministically in C#.

3. Evolutionary (genetic) training
    - A population of candidate controllers (instances of the project's neural network objects) is evaluated each generation.
    - Fitness ranking selects the best-performing controllers to reproduce.
    - Crossover and mutation create new offspring networks; elitism can keep top performers unchanged between generations.
    - The system repeats the evaluate-select-reproduce cycle until a stopping criterion (number of generations or target fitness) is met.

4. Project components
    - The repository contains several C# scripts implementing the above pieces. Notable files include (under `Assets/Scripts`):
    - `GeneticManagement.cs`, `DeepGeneticManagement.cs` — training orchestration and population control.
    - `NeuralNetworkObject.cs`, `GeneticNeuralNetworkk.cs`, `NetworkClases.cs`, `LayerObject.cs` — neural network and numeric primitives implemented from scratch.
    - `AgentMoves.cs`, `AgentView.cs`, `Drive.cs`, `ManualDrive.cs` — agent control and sensor handling.
    - `DeepDrive.cs`, `NetworkTest.cs` — experiment harnesses and test runners.

## Implementation notes and scope

- All math and neural-network primitives are written from scratch in C# and integrated with Unity's game loop.
- The training loop is evolutionary: agents are evaluated in the physics simulation and a genetic algorithm builds subsequent generations.
- This project demonstrates how to build ML-like systems without external ML libraries and how to couple evolutionary strategies with a physics-based simulator.

## Getting started (open and run)

1. Install Unity (recommended: a 2019.4+ LTS or later editor that matches your local Unity installation).
2. Open the `DrivingML` folder in Unity Hub.
3. In the Editor, open the primary scene in `Assets/Scenes` that contains the track and agent prefabs.
4. Play the scene to observe training or run evaluation modes exposed by the `GeneticManagement` / `DeepGeneticManagement` components on the scene GameObject.

Notes:

- If the project uses package manager dependencies, Unity will restore them when you open the project.
- Use the Editor inspector to tune population size, mutation rate, and fitness parameters exposed by the training manager scripts.

## Development notes and tips

- The numeric code and neural network primitives are intentionally simple and instructional. They are useful for experimenting with topology, activation functions, and evolutionary operators.
- When experimenting, try:
  - Increasing population size for more diverse exploration.
  - Reducing mutation rate if converging prematurely.
  - Adding checkpoints to the track for more granular fitness.

## Project structure (high-level)

- `Assets/Scripts/` — core implementation: agents, networks, management and experiment scripts.
- `Assets/Scenes/` — scenes used for training/evaluation.
- `docs/` — (not present by default) recommended place for gifs, videos and plots.

## Contact / Contributing

If you want to contribute, open an issue describing the change or improvement. For small fixes, open a pull request against `main` with a short description of intent.
