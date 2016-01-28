using System;
using System.Collections.Generic;
using System.IO;
using M6.Form;
using Newtonsoft.Json;
using ProtoBuf;

namespace M6.Classes
{
    public struct TuneSeed
    {
        public string Filename { get; set; }
        public double BitRate { get; set; }
        public int StartTick { get; set; }
    }

    public class Project : IMixProperties
    {
        public static Project OpenProject(string projectFolder)
        {
            if (projectFolder == null) throw new ArgumentNullException("projectFolder");

            var project = new Project
            {
                WorkingFolder = projectFolder,
                Tunes = new List<Tune>()
            };

            var metaDataPath = Path.Combine(project.WorkingFolder, "MetaData");
            Directory.CreateDirectory(metaDataPath);

            var projectFileContent = File.ReadAllText(Path.Combine(projectFolder, "project.json"));
            var seedList = JsonConvert.DeserializeObject<TuneSeed[]>(projectFileContent);

            var fileConverterFactory = new FileConverterFactory(new FileSystemHelper());

            foreach (var tuneSeed in seedList)
            {
                var tunePath = Path.Combine(project.WorkingFolder, tuneSeed.Filename);

                var rawTunePath = Path.ChangeExtension(Path.Combine(metaDataPath, tuneSeed.Filename), "m6raw");
                var summaryPath = Path.ChangeExtension(Path.Combine(metaDataPath, tuneSeed.Filename), "summary");

                IFileConverter converter;
                if ((converter = fileConverterFactory.ParseFile(rawTunePath)) == null)
                {
                    converter = fileConverterFactory.ParseFile(tunePath);
                }
                if (converter == null) continue;

                var waveData = converter.ProcessFile();
                if (waveData == null) continue;

                if (!File.Exists(rawTunePath))
                {
                    try
                    {
                        using (var rawFile = File.Create(rawTunePath))
                        {
                            Serializer.Serialize(rawFile, waveData);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }

                var tune = new Tune(project, waveData);

                SummaryCollection summaryData = null;
                try
                {
                    using (var summaryFile = File.OpenRead(summaryPath))
                    {
                        summaryData = Serializer.Deserialize<SummaryCollection>(summaryFile);
                    }

                    tune.SummaryCollection = summaryData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                if (summaryData == null)
                {
                    tune.BuildSummaries();
                    try
                    {
                        using (var summaryFile = File.Create(summaryPath))
                        {
                            Serializer.Serialize(summaryFile, tune.SummaryCollection);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }

                tune.StartTick = tuneSeed.StartTick;
                tune.BitRate = tuneSeed.BitRate;

                tune.Track = project.Tunes.Count;

                project.Tunes.Add(tune);
            }

            return project;
        }

        public string WorkingFolder { get; set; }
        public List<Tune> Tunes { get; private set; }

        public int PlaybackRateInSamplesPerSecond { get { return 44100; } }
    }
}
