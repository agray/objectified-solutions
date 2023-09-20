#region Licence
/*
 * The MIT License
 *
 * Copyright (c) 2008-2013, Andrew Gray
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using objectified_solutions.views.fileview;
using objectified_solutions.views.solutionview;
using objectified_solutions.views.solutionview.solution;

namespace objectified_solutions
{
    public sealed class SolutionObject {
        public string FormatVersion { get; set; }
        public string VSVersion { get; set; }
        public string Name { get; set; }
        public string RootPath { get; set; }
        public FileView FileView { get; set; }
        public SolutionView SolutionView { get; set; }

        public SolutionObject(string slnFile) {
            var lines = new List<string>(File.ReadAllLines(slnFile));
            RootPath = GetRootPath(slnFile);
            Name = GetName(slnFile);
            FormatVersion = GetFormatVersion(lines);
            VSVersion = lines[2].Substring(2);

            FileView = new FileView(lines, RootPath);
            SolutionView = new SolutionView(lines);
        }

        public string SolutionDetails {
            get {
                var sb = new StringBuilder();
                sb.AppendLine("Solution Format Version: " + FormatVersion);
                sb.AppendLine("Solution will open in: " + VSVersion);
                sb.AppendLine("Solution location: " + RootPath);
                return sb.ToString();
            }
        }

        public string FileViewDetails {
            get {
                var sb = new StringBuilder();
                sb.AppendLine("Projects in " + Name + " solution:");
                foreach (var project in FileView.Projects) {
                    sb.AppendLine(Constants.FOURSPACES + "Name: " + project.Name);
                    sb.AppendLine(Constants.FOURSPACES + "RelativePath: " + project.RelativePath);
                    sb.AppendLine(Constants.FOURSPACES + "Number of Source Files: " + project.SourceFiles.Count);
                    sb.AppendLine(Constants.FOURSPACES + "Number of System References: " + project.References.Count);
                    sb.AppendLine(Constants.FOURSPACES + "Number of Project References: " + project.ProjectReferences.Count);
                    sb.AppendLine(Constants.FOURSPACES + "-------------");
                }
                return sb.ToString();
            }
        }

        public string SolutionViewDetails {
            get {
                var sb = new StringBuilder();
                EmitSolutionFolder(sb, SolutionView.SolutionFolders, 0);
                EmitProjectsNotInASolutionFolder(sb);
                return sb.ToString();
            }
        }

        private void EmitSolutionFolder(StringBuilder sb, IEnumerable<SolutionFolderObject> sfos, int numTabs) {
            foreach (SolutionFolderObject sfo in sfos) {
                sb.Append(Common.Tabs(numTabs)).AppendLine(sfo.Name);
                //Print out nested solution folders
                if (sfo.HasNestedFolders())
                {
                    EmitSolutionFolder(sb, sfo.NestedFolders, numTabs + 1);
                }

                //Print out projects
                if (sfo.HasNestedProjects())
                {
                    foreach (var projectName in from projectGuid in sfo.NestedProjects
                                                let projectName = FileView.GetProjectName(projectGuid)
                                                where projectName != null
                                                select projectName)
                    {
                        sb.Append(Common.Tabs(numTabs + 1)).AppendLine(projectName);
                    }
                }   
            }
        }

        private void EmitProjectsNotInASolutionFolder(StringBuilder sb)
        {
            foreach (var projectName in SolutionView.ProjectsNotInASolutionFolder
                         .Select(projectGuid => FileView
                             .GetProjectName(projectGuid))
                         .Where(projectName => projectName != null))
            {
                sb.AppendLine(projectName);
            }
        }

        private string GetRootPath(string slnFile) {
            int lastSlash = slnFile.LastIndexOf(Constants.BACKSLASH, StringComparison.Ordinal);
            return slnFile.Remove(lastSlash + 1);
        }

        private string GetName(string slnFile) {
            int lastSlash = slnFile.LastIndexOf(Constants.BACKSLASH, StringComparison.Ordinal);
            return slnFile.Substring(lastSlash + 1);
        }

        private string GetFormatVersion(List<string> lines)
        {
            string firstNonBlankLine = lines.Find(x => !x.Equals(""));
            string[] tokens = Common.Split(firstNonBlankLine);
            return tokens[tokens.Length - 1];
        }
    }
}