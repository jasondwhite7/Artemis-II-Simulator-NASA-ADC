# Artemis II Mission Simulator
### NASA App Development Challenge 2024–2025
**Award: Best User Interface** — 1 of 4 teams selected internationally to present at NASA Johnson Space Center

---

## What It Is
An interactive application simulating the Artemis II mission using real NASA mission data.
Built for the NASA App Development Challenge by a team of 5.

## Features
- **3D Flight Path Visualization** — Orion's trajectory rendered as a color-coded path with multiple settings denoting various information
- **Ground Antenna Link Optimizer** — custom algorithm computing signal strength from Orion to ground stations, with three prioritization modes:
  - Maximize link budget
  - Minimize antenna switches
  - Balanced combination
- **Corrected Dataset** — identified and corrected inaccuracies in the provided NASA dataset, achieving a model within 0.02% of verified values
- **Modular Architecture** - separate modules for data parsing, calculations, storing, as well as a timer that controls all of the UI elements 
- **Simulation Speed** - slider to change simulation speed from anywhere between real-time to 20,000x speed
- **3d Orion Model with Camera Angles and Animations** - custom Orion Model made in Blender, multiple camera angles using Cinemachine,
  and animations made in Unity for each mission stage
- **Bonus Antenna Visibility Algorithm** - converted antenna's spherical coordinates to cartesian, calculated angle between antenna and Orion,
  and determined if antenna was visible to Orion
- **Velocity Widget** - displayed Orion's resultant velocity as a visual vector, as well the 3 vectors of Orion's fixed-body reference frame
- **Timers** - displayed mission elapsed time, time until next stage, and time in GMT, updating with simulation speed
- **Total Distance and Lunar Time** - calculated total distance traveled by Orion as well as displayed elapsed time as a moon phase widget

## Tech Stack
Unity, C#

## Video Presentation
[![Artemis II Mission Simulator Demo](https://img.youtube.com/vi/kYOU_ufs9sI/maxresdefault.jpg)](https://www.youtube.com/watch?v=kYOU_ufs9sl)

## Team
| Name | Role | GitHub |
|------|------|--------|
| Jason | Lead Programmer | @jasondwhite7 |
| Madeline | Front-End Engineer | @mdbruns3 |

## Presented At
- NASA Johnson Space Center, Spring 2025 — to NASA workforce, leadership, and the public.
- Columbus, Ohio, Spring 2025 - to Ohio Senate
- Delaware, Ohio, Spring 2025 - to Delaware City Council
- Delaware, Ohio, Spring 2025 - to Delaware City Schools School Board
