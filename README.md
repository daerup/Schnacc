[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=daerup_Schnacc&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=daerup_Schnacc)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=daerup_Schnacc&metric=bugs)](https://sonarcloud.io/summary/new_code?id=daerup_Schnacc)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=daerup_Schnacc&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=daerup_Schnacc)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=daerup_Schnacc&metric=coverage)](https://sonarcloud.io/summary/new_code?id=daerup_Schnacc)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=daerup_Schnacc&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=daerup_Schnacc)

# Schnacc
A simple snake game written in C# using the .NET framework. 
This project is used for the course SWAT and will refactored and extended during the course. 

## Rules
Eat the food to grow the snake and gain points. Play live agains other players and see their highscores pop up in your side bar. 

Play strategically and do not move unnecessarily, as you will loose points for every move you make.
The further you progress, the faster the snake will move.

To fast? Activate slow motion in exchange for points and take a breath.

Controls:
- <kbd>↑</kbd> Move up
- <kbd>↓</kbd> Move down
- <kbd>←</kbd> Move left
- <kbd>→</kbd> Move right
- <kbd>Space</kbd> Activate slow motion

The controls are buffered, so you can press multiple keys in rapid succession and the snake will move in that direction.

>**Note**: Only logged in players get to have their highscores saved.

# Architecture
The solution follows was originally designed to follow the Onion Architecture. In hindsight, I feel like the ``domain`` project is a bit overloaded and could be furter split up. The UI is implemented using WPF and the MVVM pattern.