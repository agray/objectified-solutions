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
using objectified_solutions.views.fileview.source;

namespace objectified_solutions.views.fileview.project {
    public class ProjectObject {
        public string Name { get; set; }
        public string ProjectGuid { get; set; } //unique id
        public string RelativePath { get; set; }
        public string OutputType { get; set; }
        public string Configuration { get; set; }
        public string Platform { get; set; }
        public string ProductVersion { get; set; }
        public string RootNamespace { get; set; }
        public string TargetFramework { get; set; }
        public string SchemaVersion { get; set; }
                
        public List<SourceCodeFile> SourceFiles { get; set; }
        public List<Reference> References { get; set; }
        public List<ProjectReference> ProjectReferences { get; set; }
    }
}