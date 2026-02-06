# Arena Survival FPS (Unity Greybox)

A PC-only, single-player **3D first-person wave survival shooter** built in Unity as a learning + portfolio project.

You are trapped inside a **circular fenced arena**. Enemies (simple greybox shapes like circles/cubes) approach from outside via a **ramp ring** and try to overwhelm you. Your goal is to **survive wave after wave** by shooting them down.

This project is intentionally **minimal visuals/audio** at first, focusing on strong fundamentals: FPS controls, combat loop, enemies, waves, UI, and clean architecture.

---

## Core Gameplay Loop

1. Wave starts
2. Enemies enter the arena and chase/attack the player
3. Player shoots enemies to survive
4. Short break between waves
5. Next wave gets harder (more enemies / faster / tougher)

Survive as long as possible.

---

## Features (Target)

### Player
- First-person movement (WASD) + mouse look
- Shooting (hitscan raycast for crisp feel)
- Health + damage + death/reset
- Optional: sprint, reload/ammo later

### Enemies (Greybox)
- Simple shapes (Capsule / Cube / Sphere)
- "Dumb movement" AI (no NavMesh): enemies move directly toward the player
- Attack behaviors:
  - Melee chaser (close-range hit + cooldown)
  - Optional later: ranged shooter with slow projectiles

### Arena
- Circular zone enclosed by a fence
- Outer ring path + ramp that leads enemies toward one or more entry gates
- Simple cover objects (optional) to create micro-decisions

### Waves
- Wave manager spawns enemies in increasing difficulty
- Basic scaling: enemy count + spawn rate + health
- UI shows current wave and remaining enemies

---

## Controls (Planned)

- **WASD**: Move  
- **Mouse**: Look  
- **Left Click**: Shoot  
- **R**: Restart (after death)  
- **Esc**: Pause / Menu (later)

---

## Tech Goals (Learning Focus)

This project is designed to practice:

- Unity FPS fundamentals (input, camera, CharacterController)
- Shooting systems (raycast damage, hit effects)
- Enemy AI basics (state + direct movement)
- Spawning and wave pacing
- UI (health, wave indicator, game over)
- Clean code structure and component-based design
- Optimization basics (object pooling) as a later upgrade

---

## Architecture Notes (Planned)

**Core components**
- `PlayerController` (movement + look)
- `Gun` (raycast shooting, fire rate, hit detection)
- `Health` (player/enemy HP, damage, death)
- `EnemyBrain` (direct chase, attack cooldown)
- `WaveManager` (wave pacing, spawn rules)
- `SpawnPoint` / `Spawner` (spawn logic)
- `UIHud` (HP, wave, timer)

**Key idea:** keep systems modular (damage, AI, spawning, UI) so the project is easy to expand.

---

## Phase-by-Phase Production Plan

### Phase 0 - Repo + Unity Setup
**Goal:** clean foundation.
- Create Unity project
- Basic folder structure:
  - `Assets/_Project/Scenes`
  - `Assets/_Project/Scripts`
  - `Assets/_Project/Prefabs`
  - `Assets/_Project/Materials`
  - `Assets/_Project/UI`
- Add `.gitignore` + README + initial commit

**Done when:**
- Project opens + runs an empty scene
- Repo pushed and clean

---

### Phase 1 - Greybox Arena + Player Controller
**Goal:** move around in the arena in first person.
- Build circular arena (Plane/Cylinder) + fence ring
- Outer ring + ramp layout (greybox geometry)
- Implement FPS movement + mouse look using `CharacterController`

**Done when:**
- Player can walk around smoothly
- Arena layout matches the intended "fenced circle + outer ramp ring"

---

### Phase 2 - Shooting + Enemy Dummy
**Goal:** core combat interaction.
- Add `Gun` script with raycast shooting
- Create an enemy dummy (capsule) with `Health`
- On hit: reduce HP, destroy on death
- Basic crosshair + hit feedback (optional: debug lines)

**Done when:**
- Clicking shoots and kills an enemy dummy reliably
- No jank aiming / camera issues

---

### Phase 3 - Enemy AI (Dumb Movement) + Player Health
**Goal:** first real threat.
- Enemy chaser:
  - Move directly toward player
  - If close enough: damage player on cooldown
- Player health + death state + restart

**Done when:**
- Enemy can chase and hurt the player
- Player can die and restart cleanly

---

### Phase 4 - Waves + Spawning
**Goal:** the full survival loop.
- Add spawners around the arena (gates / ramp entry points)
- Wave system:
  - Wave number increments
  - Spawn `N` enemies per wave
  - Short break between waves
  - Difficulty ramps (count, spawn rate, enemy HP)

**Done when:**
- Game plays like a real wave survival loop
- UI shows wave + remaining enemies (or enemy count)

---

### Phase 5 - Polish + Performance
**Goal:** make it feel "game-y" and portfolio-ready.
- Object pooling for enemies/bullets (if needed)
- Better feedback:
  - hit marker
  - screen shake (light)
  - simple VFX / SFX
- Basic pause menu + settings (optional)
- Build and upload (itch.io optional)

**Done when:**
- Runs smoothly
- Feels responsive and readable
- A friend can play without explanation

---

## Stretch Goals (Optional)

- Ranged enemy type (slow projectile shooter)
- Simple upgrades between waves (damage/fire rate/HP)
- Headshots / weak points
- Different arena layouts
- Boss wave every 5 waves
- Covers that blow up

---

## Credits

Made by: Turar Turakbay  
Engine: Unity

