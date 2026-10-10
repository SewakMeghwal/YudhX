import subprocess

cwd = r"C:\Users\HP 640 G2\.gemini\antigravity\scratch"

r1 = subprocess.run(["git", "add", "BattleRoyaleClient/Assets/Scripts"], cwd=cwd, capture_output=True, text=True)
print("ADD OUT:", r1.stdout, r1.stderr)

r2 = subprocess.run(["git", "commit", "-m", "Implement Procedural Asset Synthesizer, PBR textures, audio PCM, VFX particles, Minimap Radar, 360 Compass Bar, and 3D soldier/weapons/vehicles/watchtowers"], cwd=cwd, capture_output=True, text=True)
print("COMMIT OUT:", r2.stdout, r2.stderr)

r3 = subprocess.run(["git", "push", "origin", "master"], cwd=cwd, capture_output=True, text=True)
print("PUSH OUT:", r3.stdout, r3.stderr)
