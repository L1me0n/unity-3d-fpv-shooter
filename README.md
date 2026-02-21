# Arena Survival FPS (Unity Greybox)

A PC-only, single-player **3D first-person wave survival shooter** built in Unity as a learning + portfolio project.

You are trapped inside a **circular fenced arena**. Enemies (simple greybox shapes: circles/cubes) approach from outside via a **ramp ring** and try to overwhelm you. Your goal is to **survive wave after wave** by shooting them down.

This project is intentionally **minimal visuals/audio** at first, focusing on strong fundamentals: FPS controls, combat loop, enemies, waves, UI, and clean architecture.

## Play
- Itch.io (Windows): https://l1me0n.itch.io/shapeattack

---

## Core Gameplay Loop

1. Wave starts
2. Enemies enter the arena and chase/attack the player
3. Player shoots enemies to survive
4. Short break between waves
5. Next wave gets harder (more enemies / faster / tougher)

Survive as long as possible.

---

## Features (Current)

### Player
- First-person movement (WASD) + mouse look (`CharacterController`)
- **Projectile-based pistol**
  - Fire rate limit
  - **12-round magazine**
  - **Reload**
- Health + damage + death/reset
- Weapon view model spawned in FPS camera slot

### Enemies (Greybox)
- Simple shapes (Capsule/Circle + Cube/Square)
- "Dumb movement" AI (no NavMesh): direct chase toward player
- Enemy types:
  - **Melee chaser (circle)**: close-range hit + cooldown
  - **Ranged shooter (square)**: fires visible projectiles within range
- Enemy death reporting integrates with wave system

### Arena
- Circular zone enclosed by a fence
- Outer ring path + procedural ramps that let enemies enter the arena
- Spawn gates (`SpawnPoints`) placed around the ring

### Waves
- Wave manager spawns enemies with increasing difficulty
- Scaling: enemy count per wave + spawn pacing
- UI feedback:
  - Break countdown
  - Wave start banner
  - Wave cleared banner

---

## Controls

- **WASD**: Move  
- **Mouse**: Look  
- **Left Click**: Shoot  
- **R**: Reload  
- **R (after death)**: Restart  
- **Esc**: Pause / Menu (later)

---

## Tech Goals (Learning Focus)

This project is designed to practice:
- Unity FPS fundamentals (input, camera, `CharacterController`)
- Combat systems (magazines, reload, fire rate, projectile hit logic)
- Enemy AI basics (direct movement, ranged vs melee behavior)
- Wave pacing + spawning systems
- UI (health, wave indicator, game over)
- Clean modular structure and component-based design
- Later optimization (object pooling)

---

## Architecture Notes

### Core Gameplay Components
- `PlayerController` (movement + look)
- `WeaponController` (fire rate, ammo, reload, firing logic)
- `WeaponDefinition` (ScriptableObject stats: damage, fireRate, mag size, projectile prefab, etc.)
- `WeaponView` (spawns weapon model prefab into FPS weapon slot)
- `Health` (player/enemy HP, damage, death)
- `PlayerHurtbox` (reliable damage receiver for projectiles + melee)
- `EnemySquareShooterBrain` (ranged movement + shooting behavior)
- `EnemyMeleeAttack` (melee damage with cooldown)
- `Spawner` + `SpawnPoint` (gate-based spawning)
- `WaveManager` (wave loop + pacing + alive tracking)
- UI: countdown/banners/game-over

**Key idea:** keep systems modular (damage, weapons, AI, waves, UI) so features can be added without rewriting everything.

---

## Phase-by-Phase Production Plan

### Phase 0 - Repo + Unity Setup (Done)
**Goal:** clean foundation.

### Phase 1 - Greybox Arena + Player Controller (Done)
**Goal:** first-person movement inside the arena + procedural fence/ramp ring.

### Phase 2 - Combat Prototype (Done)
**Goal:** basic shooting + damage + enemy dummy.

> Note: the original `Gun` prototype has been replaced by the unified weapon system.

### Phase 3 - Enemy AI + Player Death Loop (Done)
**Goal:** melee chaser + player health + game over + restart.

### Phase 4 - Waves + Spawning (Done)
**Goal:** full wave survival loop with spawn points and UI feedback.

### Phase 5 - Combat Expansion (Weapon System + Ranged Enemy) (Done)
**Goal:** real FPS weapon rules and a second enemy type.
- Replaced old `Gun` with `WeaponController` + `WeaponDefinition`
- Added magazine + reload + fire rate
- Projectile-based firing (visible bullets)
- Added square ranged enemy that shoots projectiles
- Added `PlayerHurtbox` to make projectile/melee damage reliable

### Phase 6 - Economy + Shop + Exploding Covers + Sound (Done)
**Goal:** add progression and "gamey" depth.
- Money rewards on kill (circle +1, square +2)
- Shop UI (CS-style, open with E)
- Weapon upgrades (faster fire / bigger magazine, new weapon views)
- Triangle covers that spawn after X waves, have HP, and explode on destruction
- Sound pass (shooting, reload, footsteps) + small combat polish

---

## Credits
Made by: Turar Turakbay  
Engine: Unity
