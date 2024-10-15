# RPG AI STRATEGY Game Project

## Project Overview

This project involves developing a strategy game on the PC platform using the Unity3D game engine. The game incorporates artificial intelligence and war simulation elements, with soldiers trained using reinforcement learning techniques. Our research focuses on optimizing intelligent agent behavior through adjustments to rewards, parameters, and methods.

**Project Number:** 113-CSIE-S020  
**Duration:** Semester 1, 2023 to Semester 1, 2024  
**Advisor:** Professor Chuan-Ming Liu

### Team Members
- 韓正勤 (110590008)
- 呂凱達 (110590038)
- 張高偉 (110590039)

## Key Features

- AI-driven soldiers using reinforcement learning
- War simulation and strategy gameplay
- Multi-agent training using ML-Agents and MA-POCA algorithm
- Research on sparse vs. dense reward environments

## Technologies Used

- Unity3D (version 2021.3)
- ML-Agents
- MA-POCA algorithm
- Visual Studio
- Git
- Tensorboard

## Project Structure

1. **Game Development:**
   - Implemented using Unity3D
   - Gameplay inspired by strategy games like Battlefield and Total War Simulator

2. **AI Training:**
   - Utilizes ML-Agents for agent creation and training
   - Implements MA-POCA algorithm for multi-agent learning
   - Custom reward system considering attack rewards, damage penalties, and more

3. **Research Component:**
   - Comparative study on sparse vs. dense reward environments
   - Analysis of MA-POCA performance in different team sizes (1v1, 3v3, 5v5)

## Research Findings

Our research compared the performance of the MA-POCA algorithm in sparse and dense reward environments, analyzing key metrics such as cumulative rewards, ELO ratings, episode length, and entropy across different team sizes.

## Future Work

This project lays the foundation for more complex AI-driven games and provides insights into AI development for war simulation games.

## References

1. Unity 3D (2021.3) - [Documentation](https://docs.unity3d.com/ScriptReference)
2. [ML-Agents GitHub Repository](https://github.com/Unity-Technologies/ml-agents)
3. Andrew Cohen, Ervin Teng, Vincent-Pierre Berges, Ruo-Ping Dong, Hunter Henry, Marwan Mattar, Alexander Zook, Sujoy Ganguly, "On the Use and Misuse of Absorbing States in Multi-agent Reinforcement Learning", arxiv, 2022-06-07
4. Hanski, Jari, Biçak, Kaan Baris"An Evaluation of the Unity Machine Learning Agents Toolkit in Dense and Sparse Reward Video Game Environments", DiVA portal, 2021-07-05
5. Maram Hasan, Rajdeep Niyogi"Reward specifications in collaborative multi-agent learning: A comparative study", ACM, 2024-05-21
