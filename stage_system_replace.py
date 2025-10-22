import re

path = "Assets/RSDK/StageSystem.cs"

with open(path, "r", encoding="utf-8") as f:
    content = f.read()

new_content = content
for old, new in [('.X', '.x'), ('.Y', '.y'), ('.Z', '.z'),
                 ('.R', '.r'), ('.G', '.g'), ('.B', '.b'), ('.A', '.a')]:
    new_content = re.sub(re.escape(old), new, new_content)

if new_content != content:
    with open(path, "w", encoding="utf-8") as f:
        f.write(new_content)
    print(f"Updated {path}")
else:
    print("No changes made — file already uses lowercase.")
