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
using System.Xml;
using objectified_solutions.views.fileview.project;
using objectified_solutions.views.fileview.source;

namespace objectified_solutions.parsers {
    public class ProjFileParser {
        public static void Parse(string csprojFile, ProjectObject project) {
            var doc = new XmlDocument();
            var nsmgr = GetNsMgr(csprojFile);
            doc.Load(csprojFile);

            var propertyGroupList = doc.SelectNodes("//Project/PropertyGroup", nsmgr);
            if(propertyGroupList != null) {
                var properties = propertyGroupList[0];
                //project.OutputType = GetProperty(properties, Constants.PROPERTY_OUTPUTTYPE);
                //project.Configuration = GetProperty(properties, Constants.PROPERTY_CONFIGURATION);
                //project.Platform = GetProperty(properties, Constants.PROPERTY_PLATFORM);
                //project.ProductVersion = GetProperty(properties, Constants.PROPERTY_PRODUCTVERSION);
                //project.RootNamespace = GetProperty(properties, Constants.PROPERTY_ROOTNAMESPACE);
                project.TargetFramework = GetProperty(properties, Constants.PROPERTY_TARGETFRAMEWORK);
                //project.SchemaVersion = GetProperty(properties, Constants.PROPERTY_SCHEMAVERSION);
            }

            var itemGroups = doc.SelectNodes("//Project/ItemGroup", nsmgr);
            ProcessItemGroups(itemGroups, project);
        }

        private static void ProcessItemGroups(XmlNodeList itemGroups, ProjectObject project) {
            project.References = new List<Reference>();
            project.SourceFiles = new List<SourceCodeFile>();
            project.ProjectReferences = new List<ProjectReference>();
            foreach(XmlNode itemGroup in itemGroups) {
                var childNodes = itemGroup.ChildNodes;
                if(childNodes.Count > 0) {
                    switch(childNodes[0].Name) {
                        case Constants.ITEM_GROUP_REFERENCE:
                            ProcessReferenceItems(childNodes, project);
                            break;
                        case Constants.ITEM_GROUP_COMPILE:
                            ProcessSourceItems(childNodes, project, Constants.ITEM_GROUP_COMPILE);
                            break;
                        case Constants.ITEM_GROUP_CONTENT:
                            ProcessSourceItems(childNodes, project, Constants.ITEM_GROUP_CONTENT);
                            break;
                        case Constants.ITEM_GROUP_PROJECTREFERENCE:
                            ProcessProjectReferenceItems(childNodes, project);
                            break;
                        case Constants.ITEM_GROUP_BOOTSTRAPPERPACKAGE:
                            break;
                    }
                }
            }
        }

        private static void ProcessReferenceItems(XmlNodeList nodes, ProjectObject project) {
            foreach (XmlNode node in nodes) {
                if(node.Attributes != null) {
                    var name = GetName(node.Attributes[0]);
                    var specificVersionString = GetProperty(node, Constants.PROPERTY_SPECIFICVERSION);
                    var specificVersion = specificVersionString != null && bool.Parse(specificVersionString);
                    var hintPath = GetProperty(node, Constants.PROPERTY_HINTPATH);
                    var reference = new Reference { Name = name, SpecificVersion = specificVersion, HintPath = hintPath };
                    project.References.Add(reference);
                }
            }
        }

        private static void ProcessSourceItems(XmlNodeList nodes, ProjectObject project, string itemType) {
            foreach (XmlNode node in nodes) {
                SourceCodeFile sourceFile = null;
                if(node.Attributes != null) {
                    var filename = GetName(node.Attributes[0]);
                    var relativePath = GetRelativePath(node.Attributes[0]);
                    var subType = GetProperty(node, Constants.PROPERTY_SUBTYPE);
                    if(itemType.Equals(Constants.ITEM_GROUP_COMPILE)) {
                        var dependentUpon = GetProperty(node, Constants.PROPERTY_DEPENDENTUPON);
                        sourceFile = new SourceCodeFile { FileName = filename, RelativePath = relativePath, IsCompiled = true, SubType = subType, DependentUpon = dependentUpon };
                    } else {
                        var copyToOutputDirectory = GetProperty(node, Constants.PROPERTY_COPYTOOUTPUTDIRECTORY);
                        sourceFile = new SourceCodeFile { FileName = filename, RelativePath = relativePath, IsCompiled = false, SubType = subType, CopyToOutputDirectory = copyToOutputDirectory };
                    }
                }
                project.SourceFiles.Add(sourceFile);
            }
        } 

        private static void ProcessProjectReferenceItems(XmlNodeList nodes, ProjectObject project) {
            foreach (XmlNode node in nodes) {
                var name = GetProperty(node, Constants.PROPERTY_NAME);
                var projectGuid = GetProperty(node, Constants.PROPERTY_PROJECT);
                if(node.Attributes != null) {
                    var relativePath = GetRelativePath(node.Attributes[0]);
                    var projectReference = new ProjectReference { Name = name, ProjectGuid = projectGuid, RelativePath = relativePath };
                    project.ProjectReferences.Add(projectReference);
                }
            }
        }

        private static string GetName(XmlAttribute attribute) {
            var name = attribute.Value;
            if(!name.Contains(Constants.COMMA)) {
                return name;
            }
            var firstComma = name.IndexOf(Constants.COMMA, StringComparison.Ordinal);
            return name.Remove(firstComma);
        }

        private static string GetRelativePath(XmlAttribute attribute) {
            var path = attribute.Value;
            if(!path.Contains(Constants.BACKSLASH)) {
                return null;
            }
            var lastSlash = path.LastIndexOf(Constants.BACKSLASH, StringComparison.Ordinal);
            return path.Substring(0, lastSlash + 1);
        }

        private static XmlNamespaceManager GetNsMgr(string csprojFile) {
            var nsmgr = new XmlNamespaceManager(new XmlTextReader(csprojFile).NameTable);
            nsmgr.AddNamespace(Constants.MSBUILD, "http://schemas.microsoft.com/developer/msbuild/2003");
            nsmgr.PushScope();
            return nsmgr;
        }

        private static string GetProperty(XmlNode properties, string property){
            string value = null;
            foreach(XmlNode childNode in properties.ChildNodes){
                if(childNode.Name.Equals(property)) {
                    value = childNode.InnerText;
                    break;
                }
            }
            return value;
        }
    }
}