import subprocess

cwd = r"C:\Users\HP 640 G2\.gemini\antigravity\scratch"

print("Staging all files...")
r1 = subprocess.run(["git", "add", "-A"], cwd=cwd, capture_output=True, text=True)
print("ADD RETURN:", r1.returncode, r1.stdout, r1.stderr)

print("Committing...")
r2 = subprocess.run(["git", "commit", "-m", "Add Unity meta files and full procedural 3D Battle Royale action engine assets"], cwd=cwd, capture_output=True, text=True)
print("COMMIT RETURN:", r2.returncode, r2.stdout, r2.stderr)

print("Pushing to GitHub origin master...")
r3 = subprocess.run(["git", "push", "origin", "master"], cwd=cwd, capture_output=True, text=True)
print("PUSH RETURN:", r3.returncode, r3.stdout, r3.stderr)
