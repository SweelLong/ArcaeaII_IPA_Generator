using System.Text;
using System.Text.Json;

namespace ArcSonglistMigrator
{
    /// <summary>
    /// The core of this application.
    /// </summary>
    public class Migrator
    {
        public static void Start(string jsonFilePath)
        {
            //try
            //{
                //Console.Title = "ArcSonglistMigrator";
                //Console.WriteLine("Welcome to ArcSonglistMigrator, a tool designed to migrate and allocate songlist from apk to ipa.\nPlease type the path of songlist(\\assets\\songs\\songlist): ");
                string jsonContent = File.ReadAllText(jsonFilePath);
                RootObject? root = JsonSerializer.Deserialize<RootObject>(jsonContent);
                // Store song information by id key.
                Dictionary<string, Song> songDictionary = [];
                if (root is not null) foreach (var song in root.songs)
                    {
                        /* song.SetDefaultValues();*/
                        if (song.id == "random" || song.id == "tutorial")
                        {
                            continue;
                        }
                        if (song.set == "single" && song.category == null)
                        {
                            song.category = "poprec";
                        }
                        else if(song.category == null)
                        {
                            song.category = "";
                        }
                        if(song.bg_inverse == null)
                        {
                            song.bg_inverse = song.bg;
                        }
                        if(song.source_copyright == null)
                        {
                            song.source_copyright = "";
                        }
                        foreach (var s in song.difficulties)
                        {
                            if(s.ratingPlus == null)
                            {
                                s.ratingPlus = false;
                            }
                            if (s.rating == 0 || s.rating == -1)
                            {
                                s.rating = -1;
                                s.ratingPlus = false;
                                s.chartDesigner = "";
                                s.jacketDesigner = "";
                            }
                        }
                        songDictionary[song.id] = song;
                    }
                // Output song information.
                //foreach (KeyValuePair<string, Song> entry in songDictionary)
                //{
                //    Console.WriteLine($"Song ID: {entry.Key}, Title: {entry.Value.title_localized["en"]}");
                //}
                //Console.WriteLine("Well Done! All information has been successfully stored!\nPlease wait for a moment...");
                //Thread.Sleep(2000);
                string outputFolder = Path.GetDirectoryName(jsonFilePath) ?? "";
            //// Read each song and output it as a songlist file
            JsonSerializerOptions options = new()
            {
                // Set not to escape non-ASCII characters.
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
            //List<Song> songList = [.. songDictionary.Values];
            //RootObject outputRoot = new()
            //{
            //    songs = songList
            //};
            //File.WriteAllText(jsonFilePath, JsonSerializer.Serialize(outputRoot, options), Encoding.UTF8);
            foreach (var entry in songDictionary)
                {
                    Song song = entry.Value;
                    if (song.set == "single")
                    {
                        song.set = "base";
                        song.category = "";
                    }
                    DirectoryInfo directoryInfo = new DirectoryInfo(outputFolder);
                    DirectoryInfo parentDirectory = directoryInfo.Parent;
                    //string bg = parentDirectory.FullName + "\\img\\bg\\1080\\" + song.bg + ".jpg";
                    //if(!File.Exists(bg))
                    //{
                    //    Console.ForegroundColor = ConsoleColor.Red;
                    //    Console.WriteLine("找不到资源文件：", bg);
                    //}
                    RootObject singleSongRoot = new()
                    {
                        songs = [song]
                    };
                    string outputJson = JsonSerializer.Serialize(singleSongRoot, options);
                    outputJson = outputJson.Replace("\r\n", "\n");
                    string outputFilePath = Path.Combine(outputFolder, song.id, "songlist");
                    File.WriteAllText(outputFilePath, outputJson);
                    //Console.WriteLine($"Song : \"{song.id}\" has been written to {outputFilePath}.");
                }
                //Console.Write("Everything goes well...");
                //Console.ReadKey();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"An error occurred: {ex.Message}");
            //}
        }
    }
}