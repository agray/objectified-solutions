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
namespace objectified_solutions {
    public class Constants {
        public const string OUTPUT_TYPE_DLL = "Library";
        public const string OUTPUT_TYPE_EXE = "EXE";
        public const string BACKSLASH = @"\";
        public const string TAB = @"\t";
        public const string FOURSPACES = "    ";
        public const string DOT = ".";
        public const string COMMA = ",";
        public const char SPACE_CHAR = ' ';

        public const string PROJECT = "Project";
        public const string CSPROJ = "csproj";
        public const string WIXPROJ = "wixproj";
        public const string DTPROJ = "dtproj";
        public const string MSBUILD = "msbuild";
        public const string NESTED_PROJECTS = "NestedProjects";

        public const string ITEM_GROUP_REFERENCE = "PackageReference";
        public const string ITEM_GROUP_CONTENT = "Content";
        public const string ITEM_GROUP_COMPILE = "Compile";
        public const string ITEM_GROUP_NONE = "None";
        public const string ITEM_GROUP_PROJECTREFERENCE = "ProjectReference";
        public const string ITEM_GROUP_BOOTSTRAPPERPACKAGE = "BootstrapperPackage";

        public const string PROPERTY_PROJECT = "Project";
        public const string PROPERTY_OUTPUTTYPE = "OutputType";
        public const string PROPERTY_CONFIGURATION = "Configuration";
        public const string PROPERTY_PLATFORM = "Platform";
        public const string PROPERTY_PRODUCTVERSION = "ProductVersion";
        public const string PROPERTY_ROOTNAMESPACE = "RootNamespace";
        public const string PROPERTY_TARGETFRAMEWORK = "TargetFramework";
        public const string PROPERTY_SCHEMAVERSION = "SchemaVersion";
        public const string PROPERTY_PROJECTGUID = "ProjectGuid";
        public const string PROPERTY_SPECIFICVERSION = "SpecificVersion";
        public const string PROPERTY_HINTPATH = "HintPath";
        public const string PROPERTY_DEPENDENTUPON = "DependentUpon";
        public const string PROPERTY_SUBTYPE = "SubType";
        public const string PROPERTY_COPYTOOUTPUTDIRECTORY = "CopyToOutputDirectory";
        public const string PROPERTY_NAME = "Name";
    }
}