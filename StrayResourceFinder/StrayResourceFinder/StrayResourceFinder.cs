using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrayResourceFinder
{
    public static class StrayResourceFinder
    {
        public static ProjectResources FindUnusedResources(string projectPath, bool checkAllRooms)
        {
            Resource firstRoom;

            Dictionary<string, Resource> uncheckedResources = GetProjectResources(projectPath, out firstRoom);
            Stack<Resource> aboutToCheckResources = new Stack<Resource>();

            if (checkAllRooms)
            {
                var copyKeys = uncheckedResources.ToDictionary(entry => entry.Key,
                                               entry => entry.Value).Keys;
                foreach (var k in copyKeys)
                {
                    if (uncheckedResources[k].type == ResourceType.Room)
                    {
                        aboutToCheckResources.Push(uncheckedResources[k]);
                        uncheckedResources.Remove(k);
                    }
                }
            } else
            {
                aboutToCheckResources.Push(firstRoom);
                uncheckedResources.Remove(firstRoom.name);
            }

            string projectRoot = Path.GetDirectoryName(projectPath);

            while (aboutToCheckResources.Count > 0)
            {
                Resource r = aboutToCheckResources.Pop();
                string[] content = r.GetContent(projectRoot);
                foreach (string s in content)
                {
                    if (uncheckedResources.ContainsKey(s))
                    {
                        if (uncheckedResources[s].HasContent())
                        {
                            aboutToCheckResources.Push(uncheckedResources[s]);
                        }
                        uncheckedResources.Remove(s);
                    }
                }
            }

            return new ProjectResources(
                uncheckedResources.Values.ToList().Where(x => x.type == ResourceType.Object).ToList(),
                uncheckedResources.Values.ToList().Where(x => x.type == ResourceType.Sprite).ToList(),
                uncheckedResources.Values.ToList().Where(x => x.type == ResourceType.Background).ToList(),
                uncheckedResources.Values.ToList().Where(x => x.type == ResourceType.Script).ToList(),
                uncheckedResources.Values.ToList().Where(x => x.type == ResourceType.Sound).ToList(),
                uncheckedResources.Values.ToList().Where(x => x.type == ResourceType.Room).ToList()
            );
        }


        private static Dictionary<string, Resource> GetProjectResources(string projectPath, out Resource firstRoom)
        {
            Dictionary<string, Resource> resources = new Dictionary<string, Resource>();
            string[] lines = File.ReadAllLines(projectPath);

            resources = MergeDictionaries(GetProjectResource("objects"   , "objects"    , "object"    , "objects"   , ResourceType.Object    , lines), resources);
            resources = MergeDictionaries(GetProjectResource("sound"     , "sounds"     , "sound"     , "sound"     , ResourceType.Sound     , lines), resources);
            resources = MergeDictionaries(GetProjectResource("sprites"   , "sprites"    , "sprite"    , "sprites"   , ResourceType.Sprite    , lines), resources);
            resources = MergeDictionaries(GetProjectResource("scripts"   , "scripts"    , "script"    , "scripts"   , ResourceType.Script    , lines, ".gml"), resources);
            resources = MergeDictionaries(GetProjectResource("background", "backgrounds", "background", "background", ResourceType.Background, lines), resources);

            Dictionary<string, Resource> rooms = GetProjectResource("rooms", "rooms", "room", "rooms", ResourceType.Room, lines);
            firstRoom = rooms[rooms.Keys.First()];
            resources = MergeDictionaries(rooms, resources);

            return resources;
        }

        private static Dictionary<string, Resource> GetProjectResource(string resourceSingle, string resourceMany, string resourceList, string resourceFolderName, ResourceType type ,string[] projectContent, string ending = "")
        {
            Dictionary<string, Resource> resources = new Dictionary<string, Resource>();
            int startIndex = Array.IndexOf(projectContent, ("  <" + resourceMany + " name=\"" + resourceSingle + "\">"));
            int endIndex = Array.IndexOf(projectContent, ("  </" + resourceMany + ">"));

            string folderName = "<" + resourceList + ">" + resourceFolderName + "\\";
            int folderLength = folderName.Length;
            int folderEndLength = ("</" + resourceList +">").Length;
            int extensionLength = ending.Length;
            if (startIndex > 0 && endIndex > startIndex)
            {
                for (int i=startIndex+1; i<endIndex;i++)
                {
                    string unTabbed = projectContent[i].Trim(new char[]  { '\t', ' ' });
                    if (unTabbed.Length > folderLength && unTabbed.Substring(0, folderLength) == folderName)
                    {
                        string resourceName = unTabbed.Substring(folderLength, unTabbed.Length - folderLength - folderEndLength-extensionLength);
                        resources.Add(resourceName, new Resource(resourceName, type));
                    }
                }

            }

            return resources;

        }

        private static Dictionary<TKey, TValue> MergeDictionaries<TKey, TValue> (Dictionary<TKey, TValue> dictionary1, Dictionary<TKey, TValue> dictionary2)
        {
            dictionary1.ToList().ForEach(x => dictionary2.Add(x.Key, x.Value));
            return dictionary2;
        }
    }


    public class ProjectResources
    {
        public List<Resource> objects;
        public List<Resource> sprites;
        public List<Resource> backgrounds;
        public List<Resource> scripts;
        public List<Resource> sounds;
        public List<Resource> rooms;

        public ProjectResources(
            List<Resource> objects, List<Resource> sprites, List<Resource> backgrounds,
            List<Resource> scripts, List<Resource> sounds , List<Resource> rooms)
        {
            this.objects = objects;
            this.sprites = sprites;
            this.backgrounds = backgrounds;
            this.scripts = scripts;
            this.sounds = sounds;
            this.rooms = rooms;
        }
    }
}
