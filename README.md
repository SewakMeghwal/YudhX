# YudhX — Original 3D Battle Royale Game Engine & Backend

[![GitHub Repository](https://img.shields.io/badge/GitHub-SewakMeghwal%2FYudhX-blue.svg)](https.github.com/SewakMeghwal/YudhX.git)
[![Unity Version](https://img.shields.io/badge/Unity-2022.3%20LTS-informational.svg)](https://unity.com)
[![Backend](https://img.shields.io/badge/Django-REST%20Framework-green.svg)](https://www.djangoproject.com/)

**YudhX** is a production-grade, original 3D Battle Royale multiplayer game built with Unity (C#) and a server-authoritative Django REST Framework backend.

---

## 🎮 Features Overview

* **Server-Authoritative Multiplayer**: Real-time position, shooting, ammo, and damage synchronization.
* **Third-Person Locomotion & Camera**: WASD / Analog controls, smooth rotation, ADS zoom, shoulder toggling, and wall occlusion avoidance.
* **Modular Ballistics & Combat**: 4 weapon archetypes (AR-47, Sub-9, Scatter-12, Pistol-9), RPM cadence limiters, spread cones, and hitboxes (Head: 2.5x, Limbs: 0.75x).
* **Health & DBNO System**: 100 HP, Level 1–3 Armor absorption, Downed DBNO state (100 DBNO HP), and revive mechanics.
* **Inventory & Loot**: Scalable weapon slots (Primary, Secondary, Sidearm, Melee), 20-slot 50kg backpack, weighted loot table generation, and 'F' key pickup interactions.
* **Safe Zone Shrinking**: Multi-phase shrinking circles (Phase 1–4), inner circle bounds math, electric blue wall visuals, and zone damage ticks.
* **Bot AI**: NavMesh pathfinding, line-of-sight perception, combat engagement, and safe zone flee navigation.
* **Airplane & Parachute Drop**: Random flight paths across map perimeter, eject prompt, freefall pitch dive steering, and manual/auto parachute gliding.
* **Vehicles**: 4x4 Offroad Buggy with WheelCollider physics, seat switching, fuel consumption, and vehicle destruction explosions.
* **Django REST Backend**: JWT Bearer token authentication, profiles, match stats history logging, and ranked leaderboards (Top Wins & Top Kills).
* **Performance Optimization**: Zero-allocation `ObjectPoolManager` for combat VFX, PooledParticle recycling, and real-time FPS/GC profiler overlay.
* **Anti-Cheat Foundations**: Server speed hack detection, teleportation bounds checks, fire rate cadence validation, and automated warning/kick system.

---

## 📂 Project Structure

```
scratch/
├── BattleRoyaleClient/              # Unity Game Client
│   └── Assets/
│       └── Scripts/
│           ├── Core/
│           ├── Player/              # Movement, Input, Animation Hooks
│           ├── Camera/              # Third-Person Camera, Collision Occlusion
│           ├── Health/              # HP, Armor, Hitboxes, DBNO State
│           ├── Weapons/             # Modular Ballistics, Recoil, Factory
│           ├── Inventory/           # Weapon Slots, Backpack, Stacking
│           ├── Loot/                # Weighted Loot Tables, World Pickups
│           ├── Map/                 # Map Manager, Spawns, POI Zones
│           ├── Zone/                # Safe Zone State Machine, Wall Visuals
│           ├── AI/                  # NavMesh Bot AI, Sensors, States
│           ├── Multiplayer/         # Server Authority, Combat RPCs, Anti-Cheat
│           ├── Match/               # Match Flow State Machine, Airplane Drop
│           ├── Vehicles/            # 4x4 Vehicle Physics, Seats, Fuel
│           ├── UI/                  # Mobile Virtual Joystick & Touch Controls
│           ├── Backend/             # Unity REST API Client & SimpleJWT Auth
│           └── Utilities/           # ObjectPoolManager & Performance Profiler
└── BattleRoyaleBackend/             # Django REST Framework Backend
    ├── requirements.txt
    ├── Dockerfile
    ├── docker-compose.yml
    ├── config/                      # Django Settings & URLs
    ├── accounts/                    # Auth & JWT Endpoints
    ├── players/                     # Profiles & Progression Stats
    ├── matches/                     # Match History & Results Logging
    └── leaderboards/                # Ranked Wins & Kills Leaderboards
```

---

## 🚀 Quick Start Guide

### 1. Django REST Backend Setup

```bash
cd BattleRoyaleBackend
python -m venv venv
# On Windows:
venv\Scripts\activate
# On Linux/Mac:
source venv/bin/activate

pip install -r requirements.txt
python manage.py migrate
python manage.py runserver 0.0.0.0:8000
```

* **Backend Base URL**: `http://127.0.0.1:8000/api`
* **Docker Setup**: `docker-compose up --build`

### 2. Unity Game Client Setup

1. Open Unity Hub and Add `BattleRoyaleClient` directory (Unity 2022.3 LTS or Unity 6 LTS).
2. Open `Assets/Scenes/MainMap.unity`.
3. Press **Play** in Unity Editor.

---

## 🐙 Git Repository

GitHub Remote: `https://github.com/SewakMeghwal/YudhX.git` (Branch: `master`)
