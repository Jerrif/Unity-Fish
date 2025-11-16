# Fish Minigame

https://github.com/user-attachments/assets/45dd3cd4-f8c7-4731-a501-938559a5e0e2


This is a simple fishing minigame I made in Unity, inspired by a similar mechanic in [Dungeon Drafters](https://www.dungeondrafters.com/). You cast a hook into the water to catch fish swimming by, trying to time the delayed hook so that it hits as many fish as possible each cast. Try to rack up as many catches as you can before time runs out.

## About the Project

I built this self-contained game to see if I could finish a (slightly more complex) game completely on my own (escaping tutorial hell). Additionally, I knew from the start that I wanted to write some shaders and actually use them. In the end, the core gameplay turned out pretty straightforward, but quite satisfying to play. 

<img width="2560" height="1307" alt="Unity_2025-11-17_01-40-03" src="https://github.com/user-attachments/assets/3f6d14ed-a8ee-4a03-8ff5-f0ad7efe9979" />

## Technical Approach

The project makes heavy use of events and delegates for communication between systems. The hook controller, fish manager, scoring system, and UI all communicate through static events, which keeps the code fairly decoupled while remaining readable. My biggest struggle is probably 'picking a lane' and committing to a single architecture, in the face of literally everything being called an 'antipattern' by _someone_.

Fish movement is handled with a sine wave function, with some randomized variation applied to each fish's amplitude and frequency to make them feel less uniform. The spawning system sends out waves of fish at intervals, and each one fades in and out when spawned and despawned.

## (Some) Features

- **Shaders** for water, fish waterlines, and screen transitions
- **UI system** with fade transitions between main menu, gameplay, and game over screens
- **Audio management** for background music and sound effects
- **Object pooling** for the "+1" point popups
- **Particle effects** for fish being caught
- Various utility scripts for timers, faders, and singletons

## What I'm Proud Of

The shaders are probably the highlight here. I wrote a water shader that creates a gentle flowing effect, giving the water surface some life without being too distracting. More importantly, I created a waterline shader for the fish that creates the illusion of submersion; the bottom half of each fish's texture gets turned a dark blue, with a small lighter band near the 'waterline'. 
This shader uses the fish's world position to apply the correct rotation to the shader's mask, which required some careful handling to avoid the effect applying incorrectly to fish which had different rotations.

<img width="228" height="531" alt="" src="https://github.com/user-attachments/assets/d8d7c7c2-a4fd-43aa-82c4-e63c46f4a34a" />
