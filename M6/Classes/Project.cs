using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace M6.Classes
{
    public class Project
    {
        public static Project OpenProject(string projectFolder)
        {
            if (projectFolder == null) throw new ArgumentNullException("projectFolder");

            var projectFileContent = File.ReadAllText(Path.Combine(projectFolder, "project.json"));

            var project = JsonConvert.DeserializeObject<Project>(projectFileContent);
            project.WorkingFolder = projectFolder;

            return project;
        }

        public string WorkingFolder { get; set; }
        public string[] TuneFilenames { get; set; }
    }
}
