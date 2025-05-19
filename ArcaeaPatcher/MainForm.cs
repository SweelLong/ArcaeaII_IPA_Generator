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
                CoreFileLoadedInfo.Text = "�����ļ���CoreFile.ipa �Ѽ���";
            }
        }

        private void AboutItemButton_Click(object sender, EventArgs e)
        {
            Program.MsgInfo("��ӭʹ��Arcaea ��������\n�ó���ּ�����ɱ༭songlist��һ������Arcaea ƻ����\n�����ļ���Ⱥ����Ⱥ����ȡ\n���и��๦�ܵ�����ӭ��ѯ\n���������ʻ�ӭ��ѯSweelLong��sweellong@qq.com��\nǰ��https://github.com/SweelLong�˽����");
        }

        private void FileItemButton_IPA_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new()
            {
                Title = "��ѡ������ļ�",
                Filter = "ƻ��Ӧ�ó���װ����*.ipa��|*.ipa|�����ļ���*.*��|*.*"
            };
            file.ShowDialog();
            if (file.FileName != "")
            {
                if (File.Exists(Program.CoreFile))
                {
                    Program.MsgError($"�����ļ���{Program.CoreFile} �Ѽ��ء�\n������ɾ�����ļ����ɼ����µ�Ŷ��");
                }
                File.Copy(file.FileName, Program.CoreFile);
                //MainProgressBar.Text = "�����ļ���Դ������...";
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
                                // ������ļ��У�������Ӧ���ļ���
                                Directory.CreateDirectory(Path.GetDirectoryName(destinationPath) ?? "");
                            }
                            else
                            {
                                // ������ļ��������ļ��в���ȡ�ļ�
                                Directory.CreateDirectory(Path.GetDirectoryName(destinationPath) ?? "");
                                entry.ExtractToFile(destinationPath, true);
                            }
                        }
                    }
                }
                CoreFileLoadedInfo.Text = $"�����ļ���{Path.GetFileName(file.FileName)} �Ѽ���\n������Դ�Ѵ��е�����";
                ProgressBarText.Text = "�����ļ�������ϣ�";
            }
        }

        private void ReadResItemButton_Click(object sender, EventArgs e)
        {
            if (CoreFileLoadedInfo.Text == "�����ļ���δ����")
            {
                Program.MsgError("���ȼ��غ����ļ���ʹ��");
            }
            else
            {
                var data = JsonConvert.DeserializeObject<SongsData>(File.ReadAllText(Program.AssetsPath + "\\songs\\songlist"));
                // ��Difficulty��TitleLocalized����ת��Ϊ�ı�
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
                                languageTexts.Add($"Ӣ��: {titleLocalized.En}");
                            }
                            if (!string.IsNullOrEmpty(titleLocalized.Ja))
                            {
                                languageTexts.Add($"����: {titleLocalized.Ja}");
                            }
                            if (!string.IsNullOrEmpty(titleLocalized.Ko))
                            {
                                languageTexts.Add($"����: {titleLocalized.Ko}");
                            }
                            if (!string.IsNullOrEmpty(titleLocalized.Zh_Hans))
                            {
                                languageTexts.Add($"��������: {titleLocalized.Zh_Hans}");
                            }
                            if (!string.IsNullOrEmpty(titleLocalized.Zh_Hant))
                            {
                                languageTexts.Add($"��������: {titleLocalized.Zh_Hant}");
                            }
                        }
                        song.TitleLocalizedText = string.Join(Environment.NewLine, languageTexts);
                        song.DifficultiesText = string.Join(Environment.NewLine, song.Difficulties?.Select(d => $"�Ѷȵȼ�: {d.RatingClass}, �������ʦ: {d.ChartDesigner}, �������ʦ: {d.JacketDesigner}, ����: {d.Rating}, ��������: {d.RatingPlus}"));
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
                    // �����ݰ󶨵�dataGridView1
                    MainDataGridView.DataSource = songBindingList;

                    // �����б���Ϊ������
                    MainDataGridView.Columns["Id"].HeaderText = "����ID\n(�����������һ��鿴����)";
                    // MainDataGridView.Columns["TitleLocalizedText"].HeaderText = "�������ػ���Ϣ";
                    MainDataGridView.Columns["Artist"].HeaderText = "��ʦ";
                    MainDataGridView.Columns["Bpm"].HeaderText = "BPM";
                    MainDataGridView.Columns["BpmBase"].HeaderText = "����BPM(����)";
                    MainDataGridView.Columns["Set"].HeaderText = "����";
                    MainDataGridView.Columns["Purchase"].HeaderText = "������Ϣ";
                    MainDataGridView.Columns["Category"].HeaderText = "���";
                    MainDataGridView.Columns["AudioPreview"].HeaderText = "��ƵԤ����ʼ";
                    MainDataGridView.Columns["AudioPreviewEnd"].HeaderText = "��ƵԤ������";
                    MainDataGridView.Columns["Side"].HeaderText = "���";
                    MainDataGridView.Columns["Bg"].HeaderText = "����";
                    MainDataGridView.Columns["BgInverse"].HeaderText = "��౳��";
                    MainDataGridView.Columns["WorldUnlock"].HeaderText = "ȫ�ֽ���";
                    MainDataGridView.Columns["Date"].HeaderText = "����";
                    MainDataGridView.Columns["Version"].HeaderText = "�汾��Ϣ";
                    MainDataGridView.Columns["SourceCopyright"].HeaderText = "��Ȩ��Ϣ";
                    // MainDataGridView.Columns["DifficultiesText"].HeaderText = "�Ѷ���Ϣ";
                    MainDataGridView.Columns["TitleLocalizedText"].HeaderText = "�������ػ���Ϣ - �ı� (�Ҽ��༭)";
                    MainDataGridView.Columns["DifficultiesText"].HeaderText = "�Ѷ���Ϣ  - �ı�  (�Ҽ��༭)";

                    // ����ԭʼ�Ķ�����
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

                    ShowRowsCountLabel.Text = $"��ȡ��{MainDataGridView.RowCount}��";
                }
            }
        }

        private void MainDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowRowsCountLabel.Text = $"��ȡ��{MainDataGridView.RowCount}��";
            if (e.RowIndex == -1 || e.ColumnIndex != 0 || e.RowIndex >= MainDataGridView.RowCount - 1)
            {
                return;
            }
            if (MainDataGridView[0, e.RowIndex].Value != null)
            {
                SongIDText.Text = "����ID��" + MainDataGridView[0, e.RowIndex].Value.ToString();
                string basefile = Program.AssetsPath + "\\songs\\" + MainDataGridView[0, e.RowIndex].Value.ToString() + "\\base.jpg";
                if (!File.Exists(basefile))
                {
                    basefile = Program.AssetsPath + "\\songs\\" + MainDataGridView[0, e.RowIndex].Value.ToString() + "\\1080_base.jpg";
                    if (!File.Exists(basefile))
                    {
                        Program.MsgError("�Ҳ���base.jpg��");
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
                        Program.MsgError("�Ҳ���base_256.jpg��\n���Զ���ȡbase.jpg");
                        File.Copy(basefile, base256file);
                        return;
                    }
                }
                Base256pictureBox.Image = Image.FromFile(base256file);
            }
            if (MainDataGridView[17, e.RowIndex].Value != null)
            {
                SongNameText.Text = "�������ƣ�\n" + MainDataGridView[17, e.RowIndex].Value.ToString();
            }
            else
            {
                Program.MsgError("�����趨��������(�������ػ���Ϣ)��");
            }
            if (MainDataGridView[11, e.RowIndex].Value != null)
            {
                string bgfile = Program.AssetsPath + "\\img\\bg\\1080\\" + MainDataGridView[11, e.RowIndex].Value.ToString() + ".jpg";
                if (!File.Exists(bgfile))
                {
                    bgfile = Program.AssetsPath + "\\img\\bg\\" + MainDataGridView[11, e.RowIndex].Value.ToString() + ".jpg";
                    if (!File.Exists(bgfile))
                    {
                        Program.MsgError("�Ҳ���bg�ļ���");
                        return;
                    }
                }
                BGpictureBox.Image = Image.FromFile(bgfile);
            }
            else
            {
                Program.MsgError("�����趨bg�ļ���");
            }
            if (MainDataGridView[12, e.RowIndex].Value != null)
            {
                string bginversefile = Program.AssetsPath + "\\img\\bg\\1080\\" + MainDataGridView[12, e.RowIndex].Value.ToString() + ".jpg";
                if (!File.Exists(bginversefile))
                {
                    bginversefile = Program.AssetsPath + "\\img\\bg\\" + MainDataGridView[12, e.RowIndex].Value.ToString() + ".jpg";
                    if (!File.Exists(bginversefile))
                    {
                        Program.MsgError("�Ҳ���bg_inverse�ļ���");
                        return;
                    }
                }
                BGinversepictureBox.Image = Image.FromFile(bginversefile);
            }
            else
            {
                Program.MsgError("�����趨bg_inverse�ļ���");
            }
        }

        private void SonglistItemButton_imSonglist_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new()
            {
                Title = "��ѡ�񵥸�songlist�ļ�"
            };
            file.ShowDialog();
            if (file.FileName != "")
            {
                var data = JsonConvert.DeserializeObject<SongsData>(File.ReadAllText(file.FileName));
                // ��Difficulty��TitleLocalized����ת��Ϊ�ı�
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
                                languageTexts.Add($"Ӣ��: {titleLocalized.En}");
                            }
                            if (!string.IsNullOrEmpty(titleLocalized.Ja))
                            {
                                languageTexts.Add($"����: {titleLocalized.Ja}");
                            }
                            if (!string.IsNullOrEmpty(titleLocalized.Ko))
                            {
                                languageTexts.Add($"����: {titleLocalized.Ko}");
                            }
                            if (!string.IsNullOrEmpty(titleLocalized.Zh_Hans))
                            {
                                languageTexts.Add($"��������: {titleLocalized.Zh_Hans}");
                            }
                            if (!string.IsNullOrEmpty(titleLocalized.Zh_Hant))
                            {
                                languageTexts.Add($"��������: {titleLocalized.Zh_Hant}");
                            }
                        }
                        song.TitleLocalizedText = string.Join(Environment.NewLine, languageTexts);
                        song.DifficultiesText = string.Join(Environment.NewLine, song.Difficulties?.Select(d => $"�Ѷȵȼ�: {d.RatingClass}, �������ʦ: {d.ChartDesigner}, �������ʦ: {d.JacketDesigner}, ����: {d.Rating}, ��������: {d.RatingPlus}"));
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
                                Program.MsgError($"��������ͬ�ĸ���ID��{song.Id}����������������м��songlist�ļ�");
                                skip = true;
                                break;
                            }
                        }
                        if(skip == false)
                        {
                            songBindingList.Add(song);
                            MoveFolderCompact(Path.Combine(Directory.GetParent(file.FileName).FullName, song.Id), Path.Combine(Program.AssetsPath, "songs", song.Id));
                            Program.MsgInfo(song.Id + "���ɹ���ӣ�");
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
                    ShowRowsCountLabel.Text = $"��ȡ��{MainDataGridView.RowCount}��";
                    Program.MsgInfo("������ϣ�������������Դ��ť����ɾ�������ļ���");
                }
            }
        }

        void MoveFolderCompact(string source, string destination)
        {
            if (!Directory.Exists(source))
            {
                throw new DirectoryNotFoundException($"Դ�ļ��� {source} �����ڡ�");
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
            // ���ֵ�������š�˫���Ż��з�������˫����������
            if (value.Contains(",") || value.Contains("\"") || value.Contains("\r") || value.Contains("\n"))
            {
                // ��˫�����滻Ϊ����˫����
                value = value.Replace("\"", "\"\"");
                return $"\"{value}\"";
            }
            return value;
        }

        private void SonglistItemButton_exExcel_Click(object sender, EventArgs e)
        {
            Program.MsgInfo("Ϊ�˼��ٲ���Ҫ���ڴ濪֧���ֽ�֧������Excel����csv��ʽ��\n����xlsx�ȸ�ʽ����Excel�򿪺����Ϊ�����ʽ��");
            SaveFileDialog sfd = new()
            {
                Filter = "CSV �ļ� (*.csv) | *.csv",
                Title = "��ѡ�񱣴�·��"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new(sfd.FileName, false, System.Text.Encoding.UTF8))
                {
                    // д�������
                    for (int i = 0; i < MainDataGridView.Columns.Count; i++)
                    {
                        if (i > 0)
                        {
                            writer.Write(",");
                        }
                        writer.Write(QuoteValueIfNeeded(MainDataGridView.Columns[i].HeaderText));
                    }
                    writer.WriteLine();

                    // д��������
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
            // ���������кͿ���
            if (e.RowIndex < 0 || MainDataGridView.Rows[e.RowIndex].IsNewRow)
            {
                return;
            }

            // �����ʾ����֤����BPM�ֶΣ�������Ӧ����������
            if (MainDataGridView.Columns[e.ColumnIndex].Name == "BpmBase")
            {
                if (!int.TryParse(e.FormattedValue.ToString(), out int result))
                {
                    MainDataGridView.Rows[e.RowIndex].ErrorText = "����BPM����������";
                    Program.MsgError("����BPM����������");
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
            //            MainDataGridView.Rows[e.RowIndex].ErrorText = "�Ҳ���bg�ļ���";
            //            Program.MsgError("�Ҳ���bg�ļ���");
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
            //            MainDataGridView.Rows[e.RowIndex].ErrorText = "�Ҳ���bg_inverse�ļ���";
            //            Program.MsgError("�Ҳ���bg_inverse�ļ���");
            //        }
            //    }
            //}
        }

        private void SaveResItemButton_Click(object sender, EventArgs e)
        {
            if (songBindingList == null)
            {
                Program.MsgError("���ȼ�����Դ��");
                return;
            }
            #region ��TitleLocalizedText��DifficultiesText��ԭ��������
            foreach (var song in songBindingList)
            {
                #region ��ԭTitleLocalized
                song.TitleLocalized = new TitleLocalized();
                if (!string.IsNullOrEmpty(song.TitleLocalizedText))
                {
                    var lines = song.TitleLocalizedText.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines)
                    {
                        if (line.StartsWith("Ӣ��: "))
                        {
                            song.TitleLocalized.En = line.Substring(3).Trim();
                        }
                        else if (line.StartsWith("����: "))
                        {
                            song.TitleLocalized.Ja = line.Substring(3).Trim();
                        }
                        else if (line.StartsWith("����: "))
                        {
                            song.TitleLocalized.Ko = line.Substring(3).Trim();
                        }
                        else if (line.StartsWith("��������: "))
                        {
                            song.TitleLocalized.Zh_Hans = line.Substring(5).Trim();
                        }
                        else if (line.StartsWith("��������: "))
                        {
                            song.TitleLocalized.Zh_Hant = line.Substring(5).Trim();
                        }
                    }
                }
                #endregion
                #region ��ԭDifficulties
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
                            if (part.StartsWith("�Ѷȵȼ�: "))
                            {
                                int ratingClass;
                                if (int.TryParse(part.Substring(6).Trim(), out ratingClass))
                                {
                                    difficulty.RatingClass = ratingClass;
                                }
                            }
                            else if (part.StartsWith("�������ʦ: "))
                            {
                                difficulty.ChartDesigner = part.Substring(7).Trim();
                            }
                            else if (part.StartsWith("�������ʦ: "))
                            {
                                difficulty.JacketDesigner = part.Substring(7).Trim();
                            }
                            else if (part.StartsWith("����: "))
                            {
                                int rating;
                                if (int.TryParse(part.Substring(4).Trim(), out rating))
                                {
                                    difficulty.Rating = rating;
                                }
                            }
                            else if (part.StartsWith("��������: "))
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
            #region �����ܵ�songlist�ļ�
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
            // �����з��滻Ϊ Unix (LF) ���з�
            json = json.Replace("\r\n", "\n");
            File.WriteAllText(path, json);
            Program.MsgInfo("songlist�ļ�����ɹ���");
            #endregion
            #region ɾ���������ڸ����б��е���Ŀ�ļ�
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
                    Program.MsgInfo($"�ļ��У�{dir} �������");
                }
            }
            #endregion
            #region ɾ������Ҫ�ı����ļ�
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
                    Program.MsgInfo($"������{bgfile} �������");
                }
            }
            #endregion
            ArcSonglistMigrator.Migrator.Start(path);
            Program.MsgInfo("��Ŀsonglist�ļ����ɳɹ���");
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
                Program.MsgError("���ȼ�����Դ��");
                return;
            }

            // ���ɴ�����ļ��ı���·��
            string ipaFileName = Path.GetFileNameWithoutExtension(Program.CoreFile);
            string patchedIpaPath = Path.Combine(Program.AssetsPath, $"{ipaFileName}_patched.ipa");

            try
            {
                using (ZipArchive originalIpa = ZipFile.OpenRead(Program.CoreFile))
                using (ZipArchive patchedIpa = ZipFile.Open(patchedIpaPath, ZipArchiveMode.Create))
                {
                    // ���Ƴ���Ҫɾ�����ļ���֮���������Ŀ���µ� ZIP �ļ�
                    foreach (ZipArchiveEntry entry in originalIpa.Entries)
                    {
                        if (!entry.FullName.StartsWith("Payload/Arc-mobile.app/songs/") &&
                            !entry.FullName.StartsWith("Payload/Arc-mobile.app/img/bg/"))
                        {
                            // ������ʱ�ļ����洢��Ŀ����
                            string tempFilePath = Path.Combine(Program.AssetsPath, Path.GetRandomFileName());
                            try
                            {
                                entry.ExtractToFile(tempFilePath);
                                patchedIpa.CreateEntryFromFile(tempFilePath, entry.FullName);
                            }
                            finally
                            {
                                // ɾ����ʱ�ļ�
                                if (File.Exists(tempFilePath))
                                {
                                    File.Delete(tempFilePath);
                                }
                            }
                        }
                    }

                    // ��� Assets �е� songs �ļ���
                    string songsPath = Path.Combine(Program.AssetsPath, "songs");
                    if (Directory.Exists(songsPath))
                    {
                        AddDirectoryToZip(patchedIpa, songsPath, "Payload/Arc-mobile.app/songs");
                    }

                    // ��� Assets �е� img/bg �ļ���
                    string bgPath = Path.Combine(Program.AssetsPath, "img", "bg");
                    if (Directory.Exists(bgPath))
                    {
                        AddDirectoryToZip(patchedIpa, bgPath, "Payload/Arc-mobile.app/img/bg");
                    }
                }

                Program.MsgInfo($"������ IPA �ļ��ѱ��浽: {patchedIpaPath}");
            }
            catch (Exception ex)
            {
                Program.MsgError($"���� IPA �ļ�ʱ��������: {ex.Message}");
            }
        }

        private void DelRowItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell row in MainDataGridView.SelectedCells)
            {
                if (row.RowIndex < MainDataGridView.RowCount)
                {
                    MainDataGridView.Rows.Remove(MainDataGridView.Rows[row.RowIndex]);
                    Program.MsgInfo("�ɹ�ɾ����ѡ�У�����ǰ�����¶�ȡ�Գ����޸ģ�");
                }
            }
        }

        private void SpecifyPackItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell row in MainDataGridView.SelectedCells)
            {
                DataGridViewRow selectedRow = MainDataGridView.Rows[row.RowIndex];
                string val = selectedRow.Cells["Category"].Value.ToString();

                // �����༭���ڲ���������
                TitleLocalizedTextEditor editForm = new TitleLocalizedTextEditor(val);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    // ��ȡ�༭�������
                    string editedName = editForm.TitleLocalizedText;

                    // ���� DataGridView �е�����
                    selectedRow.Cells["Category"].Value = editedName;
                }
            }
        }

        private void PackManagement_Click(object sender, EventArgs e)
        {
            //TODO ���е�������鹦�ܵ�ʵ��
            Program.MsgInfo("��д�ˣ���д��");
        }

        private void EditTitleLocalizedText_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell row in MainDataGridView.SelectedCells)
            {
                DataGridViewRow selectedRow = MainDataGridView.Rows[row.RowIndex];
                TitleLocalizedTextEditor editForm = new(selectedRow.Cells["TitleLocalizedText"].Value.ToString());
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    Program.MsgInfo("�ɹ��޸�Ϊ��\n" + editForm.TitleLocalizedText);
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
                    Program.MsgInfo("�ɹ��޸�Ϊ��\n" + editForm.DifficultiesText);
                    selectedRow.Cells["DifficultiesText"].Value = editForm.DifficultiesText;
                }
            }
        }
    }
}