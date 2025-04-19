import os
import re
import gc
import json
import shutil
import zipfile
from pathlib import Path

print("ArcaeaII_IPA_Generator.py by SweelLong")
ipa_path = Path("CoreFile.ipa")
apk_path = Path(input("请输入APK文件路径: ").strip())
new_version = input("请输入新版本号（如2.4.5）: ").strip()
if not apk_path.exists():
    print("❌ 错误: APK文件不存在")
    input("键入任意键退出...")
    exit(1)
if not ipa_path.exists():
    print("❌ 错误: CoreFile.ipa文件不存在")
    input("键入任意键退出...")
    exit(1)
print("✅ 加载核心文件")
# 初始化临时目录
temp_base = Path("Arc2_temp")
temp_base.mkdir(exist_ok = True)
apk_dir = temp_base / "apk_res"
ipa_temp = temp_base / "ipa_res"
print("✅ 初始化临时目录")
# 阶段0：预解压IPA基础资源到APK目标路径
print("\n【0/6】预解压IPA基础资源...")
ipa_img_target = apk_dir / "assets/img"
ipa_song_target = apk_dir / "assets/songs"
ipa_layout_target = apk_dir / "assets/layouts"
try:
    # 解压IPA获取基础资源（仅提取img、songs和layouts目录）
    with zipfile.ZipFile(ipa_path, "r") as ipa_zip:
        # 提取img目录（保留目录结构）
        img_members = [m for m in ipa_zip.namelist() if m.startswith("Payload/Arc-mobile.app/img/")]
        for m in img_members:
            ipa_zip.extract(m, temp_base)
        # 提取songs目录（保留目录结构）
        song_members = [m for m in ipa_zip.namelist() if m.startswith("Payload/Arc-mobile.app/songs/")]
        for m in song_members:
            ipa_zip.extract(m, temp_base)
        # 提取layouts目录（保留目录结构）
        layout_members = [m for m in ipa_zip.namelist() if m.startswith("Payload/Arc-mobile.app/layouts/")]
        for m in layout_members:
            ipa_zip.extract(m, temp_base)
    # 复制到APK目标路径（保留IPA独有文件，允许APK覆盖同名）
    ipa_img_source = temp_base / "Payload/Arc-mobile.app/img"
    ipa_song_source = temp_base / "Payload/Arc-mobile.app/songs"
    ipa_layout_source = temp_base / "Payload/Arc-mobile.app/layouts"
    def merge_copy(src, dst):
        dst.mkdir(parents = True, exist_ok = True)
        for item in src.iterdir():
            target_path = dst / item.name
            if item.is_dir():
                merge_copy(item, target_path)
            else:
                # 仅当APK文件不存在时保留IPA文件
                if not target_path.exists():
                    shutil.copy2(item, target_path)
    merge_copy(ipa_img_source, ipa_img_target)
    merge_copy(ipa_song_source, ipa_song_target)
    merge_copy(ipa_layout_source, ipa_layout_target)
    print(f"✅ 解压基础资源完成 - IMG: {len(list(ipa_img_source.glob("**/*")))}文件, SONGS: {len(list(ipa_song_source.glob("**/*")))}文件, LAYOUTS: {len(list(ipa_layout_source.glob("**/*")))}文件")
    shutil.rmtree(temp_base / "Payload", ignore_errors = True)
except Exception as e:
    print(f"❌ IPA预解压失败: {str(e)}")
    shutil.rmtree(temp_base, ignore_errors = True)
    input("键入任意键退出...")
    exit(1)
# 阶段1：解压与合并APK资源
print("\n【1/6】解压与合并APK资源...")
try:
    with zipfile.ZipFile(apk_path, "r") as apk_zip:
        img_count = 0
        song_count = 0
        layout_count = 0
        for file in apk_zip.namelist():
            if file.startswith("assets/img/"):
                apk_zip.extract(file, apk_dir)
                img_count += 1
            if file.startswith("assets/songs/"):
                apk_zip.extract(file, apk_dir)
                song_count += 1
            if file.startswith("assets/layouts/"):
                apk_zip.extract(file, apk_dir)
                layout_count += 1
    # 统计合并后的文件数量
    final_img = len(list(ipa_img_target.glob("**/*")))
    final_song = len(list(ipa_song_target.glob("**/*")))
    final_layout = len(list(ipa_layout_target.glob("**/*")))
    print(f"✅ 新旧资源合并完成 - IMG: {final_img}文件（+{img_count}新增）, SONGS: {final_song}文件（+{song_count}新增）, LAYOUTS: {final_layout}文件（+{layout_count}新增）")
except Exception as e:
    print(f"❌ 解压失败: {str(e)}")
    shutil.rmtree(temp_base, ignore_errors = True)
    input("键入任意键退出...")
    exit(1)
# 阶段2：重置unlocks文件
print("\n【2/6】重置unlocks文件...")
unlocks_path = apk_dir / "assets/songs/unlocks"
if unlocks_path.exists():
    try:
        with open(unlocks_path, "w", encoding="utf-8", newline="\n") as f:
            json.dump({"unlocks": []}, f, indent = 4, ensure_ascii = False)
        print(f"✅ 重置unlocks: {unlocks_path}")
    except Exception as e:
        print(f"❌ 处理unlocks失败: {str(e)}")
        shutil.rmtree(temp_base, ignore_errors = True)
        input("键入任意键退出...")
        exit(1)
else:
    print("⚠️ 警告: unlocks文件不存在，跳过处理")
# 阶段3：读取songlist文件
print("\n【3/6】读取songlist文件...")
songlist_path = apk_dir / "assets/songs/songlist"
if songlist_path.exists():
    song_data = None
    try:
        with open(songlist_path, "r", encoding="utf-8") as f:
            song_data = json.load(f)
        print(f"✅ 读取songlist文件: {songlist_path}")
        filtered_songs = [] 
        for s in song_data.get("songs", []):
            if s["id"] in ("random", "tutorial"):
                continue
            for ss in s["difficulties"]:
                ss.pop("hidden_until", None)
            song = {
                "id": s["id"],
                "title_localized": {"en": s["title_localized"].get("en")},
                "artist": s.get("artist", ""),
                "bpm": s.get("bpm", "100"),
                "bpm_base": s.get("bpm_base", 100),
                "set": s.get("set", "single"),
                "purchase": s.get("purchase", ""),
                "category": s.get("category", "") if s.get("set") != "single" else "poprec",
                "audioPreview": s.get("audioPreview", 0),
                "audioPreviewEnd": s.get("audioPreviewEnd", 0),
                "side": s.get("side", 1),
                "bg": s.get("bg", "arcahv"),
                "bg_inverse": s.get("bg_inverse", "arcahv"),
                "world_unlock": True,
                "date": s.get("date", 0),
                "version": s.get("version", "?"),
                "difficulties": s.get("difficulties", [])
                }
            output_dir = apk_dir / "assets/songs" / song["id"]
            output_dir.mkdir(exist_ok = True)
            with open(output_dir / "songlist", "w", encoding = "utf-8", newline = "\n") as f:
                json.dump({"songs": [song]}, f, indent = 4, ensure_ascii = False)
            filtered_songs.append(song)
        print("✅ 重构歌曲文件")
        with open(songlist_path, "w", encoding = "utf-8", newline = "\n") as f:
            json.dump({"songs": filtered_songs}, f, indent = 4, ensure_ascii = False)
        print(f"✅ 读取完成：共{len(filtered_songs)}首（不含random、tutorial），生成{len(filtered_songs)}个songlist文件")
    except Exception as e:
        print(f"❌ 处理songlist失败: {str(e)}")
        shutil.rmtree(temp_base, ignore_errors = True)
        input("键入任意键退出...")
        exit(1)
    finally:
        del song_data, filtered_songs
        gc.collect()
else:
    print("⚠️ 警告: songlist文件不存在，跳过处理")
# 阶段4：重构packlist文件
print("\n【4/6】重构packlist...")
packlist_path = apk_dir / "assets/songs/packlist"
new_packs = [{
    "id": "base",
    "plus_character": -1,
    "custom_banner": False,
    "is_extend_pack": True,
    "cutout_pack_image": False,
    "name_localized": {"en": "Arcaea"},
    "pack_parent": "DPGM ANDROID",
    "description_localized": {"en": ""}
}]
print(f"✅ 重构packlist文件: {packlist_path}")
if packlist_path.exists():
    pack_data = None
    try:
        with open(packlist_path, "r", encoding = "utf-8") as f:
            pack_data = json.load(f)
        for pack in pack_data.get("packs", []):
            if pack["id"] == "base":
                continue
            new_packs.append({
                "id": pack["id"],
                "plus_character": -1,
                "section": "free" if pack["section"] == "free" else "collab",
                "is_extend_pack": True,
                "custom_banner": pack.get("custom_banner", False),
                "name_localized": {"en": pack["name_localized"]["en"]},
                "description_localized": {"en": ""}
                })
        with open(packlist_path, "w", encoding = "utf-8", newline = "\n") as f:
            json.dump({"packs": new_packs}, f, indent = 4, ensure_ascii = False)
        print(f"✅ 重构完成：处理{len(pack_data.get("packs", [])) - 1}个包")
    except Exception as e:
        print(f"❌ 处理packlist失败: {str(e)}")
        shutil.rmtree(temp_base, ignore_errors = True)
        input("键入任意键退出...")
        exit(1)
    finally:
        del pack_data, new_packs
        gc.collect()
else:
    print("⚠️ 警告: packlist文件不存在，跳过处理")
    with open(packlist_path, "w", encoding="utf-8", newline="\n") as f:
        json.dump({"packs": [fixed_base]}, f, indent=4, ensure_ascii=False)
    del fixed_base
# 阶段5：注入IPA
print("\n【5/6】注入IPA...")
# 1. 部署IPA基本框架
with zipfile.ZipFile(ipa_path, "r") as zip_ref:
    zip_ref.extractall(ipa_temp)
print("✅构建IPA基本框架")
# 2. 修改版本号
plist_path = ipa_temp / "Payload/Arc-mobile.app/Info.plist"
with open(plist_path, "r", encoding = "utf-8") as f:
    content = f.read()
print("✅正则匹配版本号")
content = re.sub(r"(<key>CFBundleShortVersionString</key>\s*<string>)(\d+\.\d+)(</string>)", fr"\g<1>{new_version}\g<3>", content, count = 1)
with open(plist_path, "w", encoding = "utf-8") as f:
    f.write(content)
print(f"✅修改版本号为：{new_version}")
# 3. 注入APK资源
def copy_and_overwrite(source_folder, target_folder):
    if os.path.exists(target_folder):
        shutil.rmtree(target_folder)
    shutil.copytree(source_folder, target_folder)
print("✅注入APK资源")
# 4. 处理img、songs和layouts文件夹
def copy_and_overwrite(source_folder, target_folder):
    if os.path.exists(target_folder):
        shutil.rmtree(target_folder)
    shutil.copytree(source_folder, target_folder)
copy_and_overwrite(
    os.path.join(apk_dir / "assets/img"),
    os.path.join(ipa_temp, "Payload/Arc-mobile.app/img/")
)
copy_and_overwrite(
    os.path.join(apk_dir / "assets/songs"),
    os.path.join(ipa_temp, "Payload/Arc-mobile.app/songs/")
)
copy_and_overwrite(
    os.path.join(apk_dir / "assets/layouts"),
    os.path.join(ipa_temp, "Payload/Arc-mobile.app/layouts/")
)
print("✅处理img、songs和layouts文件夹")
# 5. 压缩并打包IPA文件
new_zip_path = str(ipa_path.parent) + "/Arcaea II patched by SweelLong.ipa"
with zipfile.ZipFile(new_zip_path, "w", zipfile.ZIP_DEFLATED) as zip_out:
    for root, dirs, files in os.walk(ipa_temp):
        for file in files:
            file_path = os.path.join(root, file)
            relative_path = os.path.relpath(file_path, ipa_temp)
            zip_out.write(file_path, relative_path)
print("✅压缩并打包IPA文件")
shutil.rmtree(temp_base)
# 阶段6：操作完成
print("【6/6】操作完成！成功输出为：", new_zip_path)
input("键入任意键退出...")
