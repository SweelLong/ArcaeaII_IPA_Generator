using Newtonsoft.Json;
using System.ComponentModel;
using System.IO.Compression;
using System.Linq;

namespace ArcaeaPatcher
{
    public partial class MainForm : Form
    {
        public MainForm() => InitializeComponent();

        private BindingList<Song> songBindingList;

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (File.Exists(Program.CoreFile))
            {
                CoreFileLoadedInfo.Text = "核心文件：CoreFile.ipa 已加载";
            }
        }

        private void AboutItemButton_Click(object sender, EventArgs e)
        {
            Program.MsgInfo("欢迎使用Arcaea 补丁程序！\n该程序旨在轻松编辑songlist，一键制作Arcaea 苹果端\n核心文件请群友在群内自取\n如有更多功能的需求欢迎咨询\n有问题疑问欢迎咨询SweelLong（sweellong@qq.com）\n前往https://github.com/SweelLong了解更多");
        }

        private void FileItemButton_IPA_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new()
            {
                Title = "请选择核心文件",
                Filter = "苹果应用程序安装包（*.ipa）|*.ipa|所有文件（*.*）|*.*"
            };
            file.ShowDialog();
            if (file.FileName != "")
            {
                if (File.Exists(Program.CoreFile))
                {
                    Program.MsgError($"核心文件：{Program.CoreFile} 已加载。\n请自行删除该文件方可加载新的哦！");
                }
                File.Copy(file.FileName, Program.CoreFile);
                //MainProgressBar.Text = "核心文件资源加载中...";
                //MainProgressBar.Maximum = 0;
                //MainProgressBar.Value = 0;
                //MainProgressBar.Step = 0;
                string[] targetFolders = ["Payload/Arc-mobile.app/songs", "Payload/Arc-mobile.app/img/bg"];
                using ZipArchive archive = ZipFile.OpenRead(file.FileName);
                foreach (string targetFolder in targetFolders)
                {
                    //MainProgressBar.Maximum += archive.Entries.Count;
                    //MainProgressBar.Value += archive.Entries.Count;
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.FullName.StartsWith(targetFolder, StringComparison.OrdinalIgnoreCase))
                        {
                            string relativePath = entry.FullName[targetFolder.Length..];
                            if (relativePath.StartsWith('/'))
                            {
                                relativePath = relativePath[1..];
                            }
                            string destinationPath = Path.Combine(Program.AssetsPath, targetFolder.Replace("/", "\\"), relativePath).Replace("Payload\\Arc-mobile.app", "");
                            //MainProgressBar.Step++;
                            if (entry.Name == "")
                            {
                                // 如果是文件夹，创建对应的文件夹
                                Directory.CreateDirectory(Path.GetDirectoryName(destinationPath) ?? "");
                            }
                            else
                            {
                                // 如果是文件，创建文件夹并提取文件
                                Directory.CreateDirectory(Path.GetDirectoryName(destinationPath) ?? "");
                                entry.ExtractToFile(destinationPath, true);
                            }
                        }
                    }
                }
                CoreFileLoadedInfo.Text = $"核心文件：{Path.GetFileName(file.FileName)} 已加载\n内置资源已从中导出！";
                ProgressBarText.Text = "核心文件加载完毕！";
            }
        }

        private void ReadResItemButton_Click(object sender, EventArgs e)
        {
            if (CoreFileLoadedInfo.Text == "核心文件：未加载")
            {
                Program.MsgError("请先加载核心文件后使用");
            }
            else
            {
                var data = JsonConvert.DeserializeObject<SongsData>(File.ReadAllText(Program.AssetsPath + "\\songs\\songlist"));
                // 将Difficulty和TitleLocalized对象转换为文本
                if (data != null)
                {
                    foreach (var song in data.Songs)
                    {
                        var titleLocalized = song.TitleLocalized;
                        var languageTexts = new List<string>();
                        if (titleLocalized != null)
                        {
                            if (!string.IsNullOrEmpty(titleLocalized.En))
                            {
                                languageTexts.Add($"英语: {titleLocalized.En}");
                            }
                            if (!string.IsNullOrEmpty(titleLocalized.Ja))
                            {
                                languageTexts.Add($"日语: {titleLocalized.Ja}");
                            }
                            if (!string.IsNullOrEmpty(titleLocalized.Ko))
                            {
                                languageTexts.Add($"韩语: {titleLocalized.Ko}");
                            }
                            if (!string.IsNullOrEmpty(titleLocalized.Zh_Hans))
                            {
                                languageTexts.Add($"简体中文: {titleLocalized.Zh_Hans}");
                            }
                            if (!string.IsNullOrEmpty(titleLocalized.Zh_Hant))
                            {
                                languageTexts.Add($"繁体中文: {titleLocalized.Zh_Hant}");
                            }
                        }
                        song.TitleLocalizedText = string.Join(Environment.NewLine, languageTexts);
                        song.DifficultiesText = string.Join(Environment.NewLine, song.Difficulties?.Select(d => $"难度等级: {d.RatingClass}, 谱面设计师: {d.ChartDesigner}, 封面设计师: {d.JacketDesigner}, 评级: {d.Rating}, 评级附加: {d.RatingPlus}"));
                    }
                    //foreach(var song in data.Songs)
                    //{
                    //    if(song.Set == "single")
                    //    {
                    //        song.Category = "poprec";
                    //        // "category": "poprec",
                    //    }
                    //    if(song.BgInverse == null || song.BgInverse == "")
                    //    {
                    //        song.BgInverse = song.Bg;
                    //    }
                    //}
                    songBindingList = [.. data.Songs];
                    // 将数据绑定到dataGridView1
                    MainDataGridView.DataSource = songBindingList;

                    // 设置列标题为中文名
                    MainDataGridView.Columns["Id"].HeaderText = "歌曲ID\n(点击此列任意一项查看详情)";
                    // MainDataGridView.Columns["TitleLocalizedText"].HeaderText = "歌名本地化信息";
                    MainDataGridView.Columns["Artist"].HeaderText = "曲师";
                    MainDataGridView.Columns["Bpm"].HeaderText = "BPM";
                    MainDataGridView.Columns["BpmBase"].HeaderText = "基础BPM(整数)";
                    MainDataGridView.Columns["Set"].HeaderText = "曲包";
                    MainDataGridView.Columns["Purchase"].HeaderText = "购买信息";
                    MainDataGridView.Columns["Category"].HeaderText = "类别";
                    MainDataGridView.Columns["AudioPreview"].HeaderText = "音频预览起始";
                    MainDataGridView.Columns["AudioPreviewEnd"].HeaderText = "音频预览结束";
                    MainDataGridView.Columns["Side"].HeaderText = "侧边";
                    MainDataGridView.Columns["Bg"].HeaderText = "背景";
                    MainDataGridView.Columns["BgInverse"].HeaderText = "别侧背景";
                    MainDataGridView.Columns["WorldUnlock"].HeaderText = "全局解锁";
                    MainDataGridView.Columns["Date"].HeaderText = "日期";
                    MainDataGridView.Columns["Version"].HeaderText = "版本信息";
                    MainDataGridView.Columns["SourceCopyright"].HeaderText = "版权信息";
                    // MainDataGridView.Columns["DifficultiesText"].HeaderText = "难度信息";
                    MainDataGridView.Columns["TitleLocalizedText"].HeaderText = "歌名本地化信息 - 文本 (右键编辑)";
                    MainDataGridView.Columns["DifficultiesText"].HeaderText = "难度信息  - 文本  (右键编辑)";

                    // 隐藏原始的对象列
                    MainDataGridView.Columns["TitleLocalized"].Visible = false;
                    // dataGridView1.Columns["Difficulties"].Visible = false;

                    foreach (var song in songBindingList)
                    {
                        if (song.Set == "single")
                        {
                            song.Category = "poprec";
                            // "category": "poprec",
                        }
                        if (song.BgInverse == null || song.BgInverse == "")
                        {
                            song.BgInverse = song.Bg;
                        }
                    }

                    ShowRowsCountLabel.Text = $"读取到{MainDataGridView.RowCount}项";
                }
            }
        }

        private void MainDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowRowsCountLabel.Text = $"读取到{MainDataGridView.RowCount}项";
            if (e.RowIndex == -1 || e.ColumnIndex != 0 || e.RowIndex >= MainDataGridView.RowCount - 1)
            {
                return;
            }
            if (MainDataGridView[0, e.RowIndex].Value != null)
            {
                SongIDText.Text = "歌曲ID：" + MainDataGridView[0, e.RowIndex].Value.ToString();
                string basefile = Program.AssetsPath + "\\songs\\" + MainDataGridView[0, e.RowIndex].Value.ToString() + "\\base.jpg";
                if (!File.Exists(basefile))
                {
                    basefile = Program.AssetsPath + "\\songs\\" + MainDataGridView[0, e.RowIndex].Value.ToString() + "\\1080_base.jpg";
                    if (!File.Exists(basefile))
                    {
                        Program.MsgError("找不到base.jpg！");
                        return;
                    }
                }
                BasepictureBox.Image = Image.FromFile(basefile);

                string base256file = Program.AssetsPath + "\\songs\\" + MainDataGridView[0, e.RowIndex].Value.ToString() + "\\base_256.jpg";
                if (!File.Exists(base256file))
                {
                    base256file = Program.AssetsPath + "\\songs\\" + MainDataGridView[0, e.RowIndex].Value.ToString() + "\\1080_base_256.jpg";
                    if (!File.Exists(base256file))
                    {
                        Program.MsgError("找不到base_256.jpg！\n已自动读取base.jpg");
                        File.Copy(basefile, base256file);
                        return;
                    }
                }
                Base256pictureBox.Image = Image.FromFile(base256file);
            }
            if (MainDataGridView[17, e.RowIndex].Value != null)
            {
                SongNameText.Text = "歌曲名称：\n" + MainDataGridView[17, e.RowIndex].Value.ToString();
            }
            else
            {
                Program.MsgError("请先设定歌曲名称(歌曲本地化信息)！");
            }
            if (MainDataGridView[11, e.RowIndex].Value != null)
            {
                string bgfile = Program.AssetsPath + "\\img\\bg\\1080\\" + MainDataGridView[11, e.RowIndex].Value.ToString() + ".jpg";
                if (!File.Exists(bgfile))
                {
                    bgfile = Program.AssetsPath + "\\img\\bg\\" + MainDataGridView[11, e.RowIndex].Value.ToString() + ".jpg";
                    if (!File.Exists(bgfile))
                    {
                        Program.MsgError("找不到bg文件！");
                        return;
                    }
                }
                BGpictureBox.Image = Image.FromFile(bgfile);
            }
            else
            {
                Program.MsgError("请先设定bg文件！");
            }
            if (MainDataGridView[12, e.RowIndex].Value != null)
            {
                string bginversefile = Program.AssetsPath + "\\img\\bg\\1080\\" + MainDataGridView[12, e.RowIndex].Value.ToString() + ".jpg";
                if (!File.Exists(bginversefile))
                {
                    bginversefile = Program.AssetsPath + "\\img\\bg\\" + MainDataGridView[12, e.RowIndex].Value.ToString() + ".jpg";
                    if (!File.Exists(bginversefile))
                    {
                        Program.MsgError("找不到bg_inverse文件！");
                        return;
                    }
                }
                BGinversepictureBox.Image = Image.FromFile(bginversefile);
            }
            else
            {
                Program.MsgError("请先设定bg_inverse文件！");
            }
        }

        private void SonglistItemButton_imSonglist_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new()
            {
                Title = "请选择单个songlist文件"
            };
            file.ShowDialog();
            if (file.FileName != "")
            {
                var data = JsonConvert.DeserializeObject<SongsData>(File.ReadAllText(file.FileName));
                // 将Difficulty和TitleLocalized对象转换为文本
                if (data != null)
                {
                    foreach (var song in data.Songs)
                    {
                        var titleLocalized = song.TitleLocalized;
                        var languageTexts = new List<string>();
                        if (titleLocalized != null)
                        {
                            if (!string.IsNullOrEmpty(titleLocalized.En))
                            {
                                languageTexts.Add($"英语: {titleLocalized.En}");
                            }
                            if (!string.IsNullOrEmpty(titleLocalized.Ja))
                            {
                                languageTexts.Add($"日语: {titleLocalized.Ja}");
                            }
                            if (!string.IsNullOrEmpty(titleLocalized.Ko))
                            {
                                languageTexts.Add($"韩语: {titleLocalized.Ko}");
                            }
                            if (!string.IsNullOrEmpty(titleLocalized.Zh_Hans))
                            {
                                languageTexts.Add($"简体中文: {titleLocalized.Zh_Hans}");
                            }
                            if (!string.IsNullOrEmpty(titleLocalized.Zh_Hant))
                            {
                                languageTexts.Add($"繁体中文: {titleLocalized.Zh_Hant}");
                            }
                        }
                        song.TitleLocalizedText = string.Join(Environment.NewLine, languageTexts);
                        song.DifficultiesText = string.Join(Environment.NewLine, song.Difficulties?.Select(d => $"难度等级: {d.RatingClass}, 谱面设计师: {d.ChartDesigner}, 封面设计师: {d.JacketDesigner}, 评级: {d.Rating}, 评级附加: {d.RatingPlus}"));
                    }
                    foreach (var song in data.Songs)
                    {
                        bool skip = false;
                        foreach(var s in songBindingList)
                        {
                            if(song.Id == "random" || song.Id == "tutorial")
                            {
                                skip = true;
                                break;
                            }
                            if(s.Id == song.Id)
                            {
                                Program.MsgError($"出现了相同的歌曲ID：{song.Id}，已跳过该项，请自行检查songlist文件");
                                skip = true;
                                break;
                            }
                        }
                        if(skip == false)
                        {
                            songBindingList.Add(song);
                            MoveFolderCompact(Path.Combine(Directory.GetParent(file.FileName).FullName, song.Id), Path.Combine(Program.AssetsPath, "songs", song.Id));
                            Program.MsgInfo(song.Id + "：成功添加！");
                        }
                    }
                    foreach(var f in Directory.GetFiles(Path.Combine(Directory.GetParent(Directory.GetParent(file.FileName).FullName).FullName, "img\\bg\\1080")))
                    {
                        File.Copy(f, Path.Combine(Program.AssetsPath, "img\\bg\\1080\\", Path.GetFileName(f)));
                    }
                    foreach (var f in Directory.GetFiles(Path.Combine(Directory.GetParent(Directory.GetParent(file.FileName).FullName).FullName, "img\\bg")))
                    {
                        File.Copy(f, Path.Combine(Program.AssetsPath, "img\\bg\\1080", Path.GetFileName(f)));
                    }
                    ShowRowsCountLabel.Text = $"读取到{MainDataGridView.RowCount}项";
                    Program.MsgInfo("导入完毕，建议点击保存资源按钮，以删除冗余文件！");
                }
            }
        }

        void MoveFolderCompact(string source, string destination)
        {
            if (!Directory.Exists(source))
            {
                throw new DirectoryNotFoundException($"源文件夹 {source} 不存在。");
            }
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            foreach (string dirPath in Directory.GetDirectories(source, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(source, destination));
            }

            foreach (string filePath in Directory.GetFiles(source, "*", SearchOption.AllDirectories))
            {
                File.Copy(filePath, filePath.Replace(source, destination), true);
            }

            Directory.Delete(source, true);
        }

        private static string QuoteValueIfNeeded(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }
            // 如果值包含逗号、双引号或换行符，则用双引号括起来
            if (value.Contains(",") || value.Contains("\"") || value.Contains("\r") || value.Contains("\n"))
            {
                // 把双引号替换为两个双引号
                value = value.Replace("\"", "\"\"");
                return $"\"{value}\"";
            }
            return value;
        }

        private void SonglistItemButton_exExcel_Click(object sender, EventArgs e)
        {
            Program.MsgInfo("为了减少不必要的内存开支，现仅支持生成Excel表格的csv格式！\n如需xlsx等格式请用Excel打开后另存为所需格式！");
            SaveFileDialog sfd = new()
            {
                Filter = "CSV 文件 (*.csv) | *.csv",
                Title = "请选择保存路径"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new(sfd.FileName, false, System.Text.Encoding.UTF8))
                {
                    // 写入标题行
                    for (int i = 0; i < MainDataGridView.Columns.Count; i++)
                    {
                        if (i > 0)
                        {
                            writer.Write(",");
                        }
                        writer.Write(QuoteValueIfNeeded(MainDataGridView.Columns[i].HeaderText));
                    }
                    writer.WriteLine();

                    // 写入数据行
                    for (int i = 0; i < MainDataGridView.Rows.Count; i++)
                    {
                        for (int j = 0; j < MainDataGridView.Columns.Count; j++)
                        {
                            if (j > 0)
                            {
                                writer.Write(",");
                            }
                            var cellValue = MainDataGridView.Rows[i].Cells[j].Value?.ToString();
                            writer.Write(QuoteValueIfNeeded(cellValue));
                        }
                        writer.WriteLine();
                    }
                }
            }
        }

        private void MainDataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // 跳过标题行和空行
            if (e.RowIndex < 0 || MainDataGridView.Rows[e.RowIndex].IsNewRow)
            {
                return;
            }

            // 这里简单示例验证基础BPM字段（假设它应该是整数）
            if (MainDataGridView.Columns[e.ColumnIndex].Name == "BpmBase")
            {
                if (!int.TryParse(e.FormattedValue.ToString(), out int result))
                {
                    MainDataGridView.Rows[e.RowIndex].ErrorText = "基础BPM必须是整数";
                    Program.MsgError("基础BPM必须是整数");
                    MainDataGridView[e.ColumnIndex, e.RowIndex].Value = result;
                    e.Cancel = true;
                }
                else
                {
                    MainDataGridView.Rows[e.RowIndex].ErrorText = "";
                }
            }

            //if(MainDataGridView.Columns[e.ColumnIndex].Name == "Bg")
            //{
            //    string bg = Program.AssetsPath + "\\img\\bg\\1080\\" + MainDataGridView[e.ColumnIndex, e.RowIndex].Value.ToString() + ".jpg";
            //    if (!File.Exists(bg))
            //    {
            //        bg = Program.AssetsPath + "\\img\\bg\\" + MainDataGridView[e.ColumnIndex, e.RowIndex].Value.ToString() + ".jpg";
            //        if (!File.Exists(bg))
            //        {
            //            MainDataGridView.Rows[e.RowIndex].ErrorText = "找不到bg文件！";
            //            Program.MsgError("找不到bg文件！");
            //        }
            //    }
            //}

            //if (MainDataGridView.Columns[e.ColumnIndex].Name == "BgInverse")
            //{
            //    string bg = Program.AssetsPath + "\\img\\bg\\1080\\" + MainDataGridView[e.ColumnIndex, e.RowIndex].Value.ToString() + ".jpg";
            //    if (!File.Exists(bg))
            //    {
            //        bg = Program.AssetsPath + "\\img\\bg\\" + MainDataGridView[e.ColumnIndex, e.RowIndex].Value.ToString() + ".jpg";
            //        if (!File.Exists(bg))
            //        {
            //            MainDataGridView.Rows[e.RowIndex].ErrorText = "找不到bg_inverse文件！";
            //            Program.MsgError("找不到bg_inverse文件！");
            //        }
            //    }
            //}
        }

        private void SaveResItemButton_Click(object sender, EventArgs e)
        {
            if (songBindingList == null)
            {
                Program.MsgError("请先加载资源！");
                return;
            }
            #region 将TitleLocalizedText和DifficultiesText还原到对象中
            foreach (var song in songBindingList)
            {
                #region 还原TitleLocalized
                song.TitleLocalized = new TitleLocalized();
                if (!string.IsNullOrEmpty(song.TitleLocalizedText))
                {
                    var lines = song.TitleLocalizedText.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines)
                    {
                        if (line.StartsWith("英语: "))
                        {
                            song.TitleLocalized.En = line.Substring(3).Trim();
                        }
                        else if (line.StartsWith("日语: "))
                        {
                            song.TitleLocalized.Ja = line.Substring(3).Trim();
                        }
                        else if (line.StartsWith("韩语: "))
                        {
                            song.TitleLocalized.Ko = line.Substring(3).Trim();
                        }
                        else if (line.StartsWith("简体中文: "))
                        {
                            song.TitleLocalized.Zh_Hans = line.Substring(5).Trim();
                        }
                        else if (line.StartsWith("繁体中文: "))
                        {
                            song.TitleLocalized.Zh_Hant = line.Substring(5).Trim();
                        }
                    }
                }
                #endregion
                #region 还原Difficulties
                song.Difficulties = new List<Difficulty>();
                if (!string.IsNullOrEmpty(song.DifficultiesText))
                {
                    var lines = song.DifficultiesText.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines)
                    {
                        var parts = line.Split([", "], StringSplitOptions.RemoveEmptyEntries);
                        var difficulty = new Difficulty();
                        foreach (var part in parts)
                        {
                            if (part.StartsWith("难度等级: "))
                            {
                                int ratingClass;
                                if (int.TryParse(part.Substring(6).Trim(), out ratingClass))
                                {
                                    difficulty.RatingClass = ratingClass;
                                }
                            }
                            else if (part.StartsWith("谱面设计师: "))
                            {
                                difficulty.ChartDesigner = part.Substring(7).Trim();
                            }
                            else if (part.StartsWith("封面设计师: "))
                            {
                                difficulty.JacketDesigner = part.Substring(7).Trim();
                            }
                            else if (part.StartsWith("评级: "))
                            {
                                int rating;
                                if (int.TryParse(part.Substring(4).Trim(), out rating))
                                {
                                    difficulty.Rating = rating;
                                }
                            }
                            else if (part.StartsWith("评级附加: "))
                            {
                                bool ratingPlus;
                                if (bool.TryParse(part.Substring(6).Trim(), out ratingPlus))
                                {
                                    difficulty.RatingPlus = ratingPlus;
                                }
                            }
                        }
                        song.Difficulties.Add(difficulty);
                    }
                }
                #endregion
            }
            #endregion
            #region 更新总的songlist文件
            SongsData root = new()
            {
                Songs = [.. songBindingList]
            };
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };
            string path = Program.AssetsPath + "\\songs\\songlist";
            // File.Move(path, Program.AssetsPath + "\\songlist_bak", true);
            string json = JsonConvert.SerializeObject(root, settings);
            // json = json.Replace("null", "\"\"");
            // 将换行符替换为 Unix (LF) 换行符
            json = json.Replace("\r\n", "\n");
            File.WriteAllText(path, json);
            Program.MsgInfo("songlist文件保存成功！");
            #endregion
            #region 删除不包含在歌曲列表中的曲目文件
            foreach (var dir in Directory.GetDirectories(Program.AssetsPath + "\\songs"))
            {
                if (Path.GetFileName(dir) == "pack" || Path.GetFileName(dir) == "random" || Path.GetFileName(dir) == "tutorial")
                {
                    continue;
                }
                bool existId = false;
                foreach (var s in songBindingList)
                {
                    if (Path.GetFileName(dir) == s.Id)
                    {
                        existId = true;
                        break;
                    }
                }
                if (existId == false)
                {
                    Directory.Delete(dir, true);
                    Program.MsgInfo($"文件夹：{dir} 已清除！");
                }
            }
            #endregion
            #region 删除不必要的背景文件
            foreach (var bgfile in Directory.GetFiles(Program.AssetsPath + "\\img\\bg\\1080"))
            {
                bool existId = false;
                foreach (var s in songBindingList)
                {

                    if (Path.GetFileNameWithoutExtension(bgfile) == s.Bg || Path.GetFileNameWithoutExtension(bgfile) == s.BgInverse)
                    {
                        existId = true;
                        break;
                    }
                }
                if (existId == false)
                {
                    File.Delete(bgfile);
                    Program.MsgInfo($"背景：{bgfile} 已清除！");
                }
            }
            #endregion
            ArcSonglistMigrator.Migrator.Start(path);
            Program.MsgInfo("曲目songlist文件生成成功！");
        }

        private static void AddDirectoryToZip(ZipArchive zipArchive, string sourceDirectory, string destinationDirectory)
        {
            foreach (string filePath in Directory.GetFiles(sourceDirectory, "*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(sourceDirectory, filePath);
                string entryName = Path.Combine(destinationDirectory, relativePath);
                zipArchive.CreateEntryFromFile(filePath, entryName);
            }
        }

        private void FileItemButton_Patch_Click(object sender, EventArgs e)
        {
            if (songBindingList == null)
            {
                Program.MsgError("请先加载资源！");
                return;
            }

            // 生成处理后文件的保存路径
            string ipaFileName = Path.GetFileNameWithoutExtension(Program.CoreFile);
            string patchedIpaPath = Path.Combine(Program.AssetsPath, $"{ipaFileName}_patched.ipa");

            try
            {
                using (ZipArchive originalIpa = ZipFile.OpenRead(Program.CoreFile))
                using (ZipArchive patchedIpa = ZipFile.Open(patchedIpaPath, ZipArchiveMode.Create))
                {
                    // 复制除了要删除的文件夹之外的所有条目到新的 ZIP 文件
                    foreach (ZipArchiveEntry entry in originalIpa.Entries)
                    {
                        if (!entry.FullName.StartsWith("Payload/Arc-mobile.app/songs/") &&
                            !entry.FullName.StartsWith("Payload/Arc-mobile.app/img/bg/"))
                        {
                            // 创建临时文件来存储条目内容
                            string tempFilePath = Path.Combine(Program.AssetsPath, Path.GetRandomFileName());
                            try
                            {
                                entry.ExtractToFile(tempFilePath);
                                patchedIpa.CreateEntryFromFile(tempFilePath, entry.FullName);
                            }
                            finally
                            {
                                // 删除临时文件
                                if (File.Exists(tempFilePath))
                                {
                                    File.Delete(tempFilePath);
                                }
                            }
                        }
                    }

                    // 添加 Assets 中的 songs 文件夹
                    string songsPath = Path.Combine(Program.AssetsPath, "songs");
                    if (Directory.Exists(songsPath))
                    {
                        AddDirectoryToZip(patchedIpa, songsPath, "Payload/Arc-mobile.app/songs");
                    }

                    // 添加 Assets 中的 img/bg 文件夹
                    string bgPath = Path.Combine(Program.AssetsPath, "img", "bg");
                    if (Directory.Exists(bgPath))
                    {
                        AddDirectoryToZip(patchedIpa, bgPath, "Payload/Arc-mobile.app/img/bg");
                    }
                }

                Program.MsgInfo($"处理后的 IPA 文件已保存到: {patchedIpaPath}");
            }
            catch (Exception ex)
            {
                Program.MsgError($"处理 IPA 文件时发生错误: {ex.Message}");
            }
        }

        private void DelRowItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell row in MainDataGridView.SelectedCells)
            {
                if (row.RowIndex < MainDataGridView.RowCount)
                {
                    MainDataGridView.Rows.Remove(MainDataGridView.Rows[row.RowIndex]);
                    Program.MsgInfo("成功删除所选行，保存前可重新读取以撤回修改！");
                }
            }
        }

        private void SpecifyPackItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell row in MainDataGridView.SelectedCells)
            {
                DataGridViewRow selectedRow = MainDataGridView.Rows[row.RowIndex];
                string val = selectedRow.Cells["Category"].Value.ToString();

                // 创建编辑窗口并传递数据
                TitleLocalizedTextEditor editForm = new TitleLocalizedTextEditor(val);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    // 获取编辑后的数据
                    string editedName = editForm.TitleLocalizedText;

                    // 更新 DataGridView 中的数据
                    selectedRow.Cells["Category"].Value = editedName;
                }
            }
        }

        private void PackManagement_Click(object sender, EventArgs e)
        {
            //TODO 表中的曲包检查功能的实现
            Program.MsgInfo("在写了，在写了");
        }

        private void EditTitleLocalizedText_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell row in MainDataGridView.SelectedCells)
            {
                DataGridViewRow selectedRow = MainDataGridView.Rows[row.RowIndex];
                TitleLocalizedTextEditor editForm = new(selectedRow.Cells["TitleLocalizedText"].Value.ToString());
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    Program.MsgInfo("成功修改为：\n" + editForm.TitleLocalizedText);
                    selectedRow.Cells["TitleLocalizedText"].Value = editForm.TitleLocalizedText;
                }
            }
        }

        private void EditDifficultiesText_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell row in MainDataGridView.SelectedCells)
            {
                DataGridViewRow selectedRow = MainDataGridView.Rows[row.RowIndex];
                DifficultiesTextEditor editForm = new(selectedRow.Cells["DifficultiesText"].Value.ToString());
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    Program.MsgInfo("成功修改为：\n" + editForm.DifficultiesText);
                    selectedRow.Cells["DifficultiesText"].Value = editForm.DifficultiesText;
                }
            }
        }
    }
}