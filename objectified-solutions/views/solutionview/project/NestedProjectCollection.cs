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
using System.Collections.Generic;
using System.Linq;

namespace objectified_solutions.views.solutionview.project {
    public class NestedProjectCollection {
        public static List<NestedProject> AllLines { get; set; }
        public static List<string> Children { get; set; }
        public static List<string> Parents { get; set; }
        public static List<string> RootParents { get; set; }
        public static List<string> NestedProjects { get; set; }
        public static List<string> NestedFolders { get; set; }

        public static void PopulateCollection(List<string> nestedProjects) {
            AllLines = new List<NestedProject>();
            Children = new List<string>();
            Parents = new List<string>();

            foreach (var line in nestedProjects) {
                var trimmedLine = line.Trim();
                var tokens = trimmedLine.Split(Constants.SPACE_CHAR);
                var nestedProject = new NestedProject { Parent = Common.TrimToken(tokens[2], 1, 2), Child = Common.TrimToken(tokens[0], 1, 2) };
                AllLines.Add(nestedProject);
                if(!Children.Contains(nestedProject.Child)) {
                    Children.Add(nestedProject.Child);
                }
                if(!Parents.Contains(nestedProject.Parent)) {
                    Parents.Add(nestedProject.Parent);
                }
            }

            RootParents = (from parent in Parents
                           where !Children.Contains(parent)
                           select parent).ToList();
            NestedProjects = new List<string>();
            NestedFolders = new List<string>();

            foreach(var child in Children) {
                if(!IsParent(child)) {
                    NestedProjects.Add(child);
                } else {
                    NestedFolders.Add(child);
                }
            }
        }

        private static bool IsParent(string item) {
            return Parents.Contains(item);
        }

        public static bool IsRootFolder(NestedProject nestedProject) {
            return RootParents.Contains(nestedProject.Parent);
        }

        public static bool IsNestedProject(string item) {
            return NestedProjects.Contains(item);
        }        
    }
}