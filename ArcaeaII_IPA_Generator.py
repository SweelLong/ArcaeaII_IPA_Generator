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
print("✅ 加载CoreFile.ipa文件")
# 初始化临时目录
temp_base = Path("Arc2_temp")
temp_base.mkdir(exist_ok = True)
apk_dir = temp_base / "apk_res"
ipa_temp = temp_base / "ipa_res"
print("✅ 初始化临时目录")
# 阶段0：预解压IPA基础资源到APK目标路径
print("\n【0/7】预解压IPA基础资源...")
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
print("\n【1/7】解压与合并APK资源...")
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
print("\n【2/7】重置unlocks文件...")
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
print("\n【3/7】读取songlist文件...")
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
                "bg_inverse": s.get("bg_inverse", s.get("bg", "arcahv")),
                "world_unlock": True,
                "date": s.get("date", 0),
                "version": s.get("version", "?"),
                "difficulties": s.get("difficulties", [])
                }
##            output_dir = apk_dir / "assets/songs" / song["id"]
##            output_dir.mkdir(exist_ok = True)
##            with open(output_dir / "songlist", "w", encoding = "utf-8", newline = "\n") as f:
##                json.dump({"songs": [song]}, f, indent = 4, ensure_ascii = False)
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
print("\n【4/7】重构packlist...")
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
# 阶段5：优化资源占用
print("\n【5/7】优化资源占用...")
def process_parent_folder(parent_folder):
    print(f"开始处理文件夹: {parent_folder}")
    # 检查父文件夹是否存在
    if not os.path.isdir(parent_folder):
        print(f"错误: 父文件夹 '{parent_folder}' 不存在")
        return
    
    # 遍历父文件夹中的所有子文件夹
    for subfolder in os.listdir(parent_folder):
        subfolder_path = os.path.join(parent_folder, subfolder)
        
        # 确保是文件夹而不是文件
        if os.path.isdir(subfolder_path):
            print(f"\n处理子文件夹: {subfolder}")
            if os.path.basename(subfolder) == "pack":
                continue
            # 获取文件夹中的所有文件
            files = os.listdir(subfolder_path)
            
            # 初始化文件集合
            base_ogg = None
            aff_files = []
            base_jpg = None
            base_256_jpg = None
            base_1080_jpg = None
            base_1080_256_jpg = None
            
            # 分类文件
            for file in files:
                file_path = os.path.join(subfolder_path, file)
                if os.path.isfile(file_path):  # 确保是文件而不是子文件夹
                    if file == "base.ogg":
                        base_ogg = file_path
                    elif file.endswith(".aff"):
                        aff_files.append(file_path)
                    elif file == "base.jpg":
                        base_jpg = file_path
                    elif file == "base_256.jpg":
                        base_256_jpg = file_path
                    elif file == "1080_base.jpg":
                        base_1080_jpg = file_path
                    elif file == "1080_base_256.jpg":
                        base_1080_256_jpg = file_path
            
            # 创建要保留的文件列表
            files_to_keep = []
            
            # 添加base.ogg和.aff文件
            if base_ogg:
                files_to_keep.append(base_ogg)
            files_to_keep.extend(aff_files)
            
            # 处理图片组合
            has_standard = base_jpg is not None or base_256_jpg is not None
            has_1080 = base_1080_jpg is not None or base_1080_256_jpg is not None
            
            # 如果两种组合都存在，优先选择1080组合
            if has_1080:
                print("检测到1080组合，处理1080组合")
                # 确保1080组合完整
                if base_1080_jpg and not base_1080_256_jpg:
                    new_file = os.path.join(subfolder_path, "1080_base_256.jpg")
                    print(f"复制 {os.path.basename(base_1080_jpg)} 为 1080_base_256.jpg")
                    shutil.copy2(base_1080_jpg, new_file)
                    files_to_keep.append(new_file)
                elif base_1080_256_jpg and not base_1080_jpg:
                    new_file = os.path.join(subfolder_path, "1080_base.jpg")
                    print(f"复制 {os.path.basename(base_1080_256_jpg)} 为 1080_base.jpg")
                    shutil.copy2(base_1080_256_jpg, new_file)
                    files_to_keep.append(new_file)
                
                # 添加1080组合文件
                if base_1080_jpg:
                    files_to_keep.append(base_1080_jpg)
                if base_1080_256_jpg:
                    files_to_keep.append(base_1080_256_jpg)
            elif has_standard:
                print("检测到标准组合，处理标准组合")
                # 确保标准组合完整
                if base_jpg and not base_256_jpg:
                    new_file = os.path.join(subfolder_path, "base_256.jpg")
                    print(f"复制 {os.path.basename(base_jpg)} 为 base_256.jpg")
                    shutil.copy2(base_jpg, new_file)
                    files_to_keep.append(new_file)
                elif base_256_jpg and not base_jpg:
                    new_file = os.path.join(subfolder_path, "base.jpg")
                    print(f"复制 {os.path.basename(base_256_jpg)} 为 base.jpg")
                    shutil.copy2(base_256_jpg, new_file)
                    files_to_keep.append(new_file)
                
                # 添加标准组合文件
                if base_jpg:
                    files_to_keep.append(base_jpg)
                if base_256_jpg:
                    files_to_keep.append(base_256_jpg)
            else:
                print("未检测到有效图片组合，跳过图片处理")
            
            # 删除不需要的文件
            for file in os.listdir(subfolder_path):
                file_path = os.path.join(subfolder_path, file)
                # 跳过子文件夹和需要保留的文件
                if os.path.isdir(file_path) or file_path in files_to_keep:
                    continue
                try:
                    print(f"删除文件: {file}")
                    os.remove(file_path)
                except Exception as e:
                    print(f"无法删除文件 {file}: {e}")
    
    print("\n处理完成!")
process_parent_folder(apk_dir / "assets/songs")
# 阶段6：注入IPA
print("\n【6/7】注入IPA...")
# 1. 部署IPA基本框架
with zipfile.ZipFile(ipa_path, "r") as zip_ref:
    zip_ref.extractall(ipa_temp)
print("✅构建IPA基本框架")
# 2. 修改版本号
plist_path = ipa_temp / "Payload/Arc-mobile.app/Info.plist"
with open(plist_path, "r", encoding="utf-8") as f:
    content = f.read()
match = re.search(r'<key>CFBundleShortVersionString</key>\s*<string>([^<]+)</string>', content)
old_version = match.group(1).strip() if match else None
print("✅正则匹配版本号")
content = re.sub(r'(<key>CFBundleShortVersionString</key>\s*<string>)([^<]+)(</string>)', fr'\g<1>{new_version}\g<3>', content, count = 1)
with open(plist_path, "w", encoding="utf-8") as f:
    f.write(content)
print(f"✅修改版本号：{old_version} => {new_version}")
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
new_zip_path = str(ipa_path.parent) + f"/Arcaea II {new_version} patched by A2Generator.ipa"
with zipfile.ZipFile(new_zip_path, "w", zipfile.ZIP_DEFLATED) as zip_out:
    for root, dirs, files in os.walk(ipa_temp):
        for file in files:
            file_path = os.path.join(root, file)
            relative_path = os.path.relpath(file_path, ipa_temp)
            zip_out.write(file_path, relative_path)
print("✅压缩并打包IPA文件")
shutil.rmtree(temp_base)
# 阶段7：操作完成
print("【7/7】操作完成！成功输出为：", new_zip_path)
input("键入任意键退出...")
