using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace StrayResourceFinder
{
    public enum ResourceType
    {
        Object,
        Sprite,
        Sound,
        Room,
        Script,
        Background
    }


    public class Resource
    {
        private char[] seperators = { ';', '+', '-', '*', '/', '\r', '&', '%', '|', '<', '>', '!', '=', '\n', '"', ' ', ':', '?', '#', '[', ']', '(', ')', '{', '}', '[', ']', '\t', ',', '.', '\''};

        public ResourceType type { get { return _type; } }
        private ResourceType _type;
        public string name;

        public Resource (string name, ResourceType type)
        {
            this.name = name;
            _type = type;
        }

        public bool HasContent()
        {
            switch (_type)
            {
                case ResourceType.Object:
                case ResourceType.Script:
                case ResourceType.Room:
                    return true;
            }
            return false;
        }

        public string GetPath()
        {
            switch (_type)
            {
                case ResourceType.Object:
                    return "\\objects\\" + name + ".object.gmx";
                case ResourceType.Script:
                    return "\\scripts\\" + name + ".gml";
                case ResourceType.Room:
                    return "\\rooms\\" + name + ".room.gmx";
            }
            return "";
        }

        public string[] GetContent(string projectPath)
        {
            switch (_type)
            {
                case ResourceType.Object:
                    return GetContentObject(projectPath + GetPath());
                case ResourceType.Script:
                    return GetContentScript(projectPath + GetPath());
                case ResourceType.Room:
                    return GetContentRoom(projectPath + GetPath());

                default:
                    return new string[0];
            }
        }
        
        private string[] GetContentScript(string path)
        {
            string fileContent = File.ReadAllText(path);
            return fileContent.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
        }

        private string[] GetContentRoom(string path)
        {
            string fileContent = File.ReadAllText(path);

            List<string[]> extractedContent = new List<string[]>();

            extractedContent.Add(GetContentRoomInstances(fileContent));

            extractedContent.Add(GetContentRoomTiles(fileContent));

            extractedContent.Add(GetContentRoomBackgrounds(fileContent));

            return extractedContent.SelectMany(x => x).ToArray();
        }

        private string[] GetContentRoomBackgrounds(string fileContent)
        {
            List<string> objects = new List<string>();

            int tilesStartIndex = fileContent.IndexOf("<backgrounds>");
            int tilesEndIndex = fileContent.IndexOf("</backgrounds>");

            if (tilesStartIndex >= 0 && tilesEndIndex > tilesStartIndex)
            {
                string[] tileLines = fileContent.Substring(tilesStartIndex + 13, tilesEndIndex - tilesStartIndex - 15).Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in tileLines)
                {
                    if (s.Length > 21)
                    {
                        string unTabbed = s.Trim(new[] { '\t', ' ' });
                        int nameIndex = unTabbed.IndexOf("name=\"") + 6;
                        if (nameIndex >= 0)
                        {
                            string name = unTabbed.Substring(nameIndex);
                            name = name.Substring(0, name.IndexOf('\"'));
                            if (name != "")
                            {
                                objects.Add(name);
                            }
                        }
                    }
                }
            }

            return objects.ToArray();
        }

        private string[] GetContentRoomTiles(string fileContent)
        {
            List<string> objects = new List<string>();

            int tilesStartIndex = fileContent.IndexOf("<tiles>");
            int tilesEndIndex = fileContent.IndexOf("</tiles>");

            if (tilesStartIndex >= 0 && tilesEndIndex > tilesStartIndex)
            {
                string[] tileLines = fileContent.Substring(tilesStartIndex + 7, tilesEndIndex - tilesStartIndex - 9).Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in tileLines)
                {
                    if (s.Length > 15)
                    {
                        string unTabbed = s.Trim(new[] { '\t', ' ' });
                        string name = unTabbed.Substring(14);
                        name = name.Substring(0, name.IndexOf('\"'));
                        objects.Add(name);
                    }
                }
            }

            return objects.ToArray();
        }

        private string[] GetContentRoomInstances(string fileContent)
        {
            List<string> objects = new List<string>();

            //Loop through all instances getting their name and their creation code
            int instancesStartIndex = fileContent.IndexOf("<instances>");
            int instancesEndIndex = fileContent.IndexOf("</instances>");

            if (instancesStartIndex >= 0 && instancesEndIndex > instancesStartIndex)
            {
                string[] instanceLines = fileContent.Substring(instancesStartIndex + 11, instancesEndIndex - instancesStartIndex - 13).Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in instanceLines)
                {
                    if (s.Length > 20)
                    {
                        string unTabbed = s.Trim(new[] { '\t', ' ' });
                        string name = unTabbed.Substring(19);
                        name = name.Substring(0, name.IndexOf('\"'));

                        int codeIndex = unTabbed.IndexOf("code=\"")+6;
                        string code = unTabbed.Substring(codeIndex);
                        if (code[0] != '\"')
                        {
                            int codeEndIndex = code.IndexOf("&#xA;\"");
                            if (codeEndIndex >= 0)
                            {
                                code = code.Substring(0, codeEndIndex);
                                objects.AddRange(code.Split(seperators, StringSplitOptions.RemoveEmptyEntries));
                            }
                        }

                        objects.Add(name);
                    }
                }
            }
            return objects.ToArray();
        }

        private string[] GetContentObject(string path)
        {
            string fileContent = File.ReadAllText(path);

            List<string[]> extractedContent = new List<string[]>();

            string shortData = getShortXmlData(fileContent, "<spriteName>", "</spriteName>");
            if (shortData != "" && shortData != "&lt;undefined&gt;")
            { extractedContent.Add(new[] { shortData }); }

            shortData = getShortXmlData(fileContent, "<maskName>", "</maskName>");
            if (shortData != "" && shortData != "&lt;undefined&gt;")
            { extractedContent.Add(new[] { shortData }); }

            shortData = getShortXmlData(fileContent, "<parentName>", "</parentName>");
            if (shortData != "" && shortData != "&lt;undefined&gt;")
            { extractedContent.Add(new[] { shortData }); }

            int stringStartIndex, stringEndIndex;

            while (true)
            {
                stringStartIndex = fileContent.IndexOf("<string>");
                if (stringStartIndex >= 0) {
                    stringEndIndex = fileContent.IndexOf("</string>");
                    if (stringEndIndex > stringStartIndex+7)
                    {
                        extractedContent.Add(
                            fileContent.Substring(
                                stringStartIndex+8,
                                stringEndIndex - stringStartIndex-8
                            ).Split(seperators, StringSplitOptions.RemoveEmptyEntries)
                            
                        );
                        fileContent = fileContent.Remove(0, stringEndIndex+8);
                    } else
                    {
                        break;
                    }
                } else
                {
                    break;
                }
            }

            return extractedContent.SelectMany(x => x).ToArray();
        }

        private string getShortXmlData (string data, string start, string end)
        {
            int index = data.IndexOf(start);
            int indexEnd = data.IndexOf(end);
            if (index >= 0 && indexEnd > index)
            {
                return data.Substring(index + start.Length, indexEnd - index - start.Length);
            }
            else return "";
        }

        public override string ToString()
        {
            return name;
        }
    }
}
