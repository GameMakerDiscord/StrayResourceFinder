using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace StrayResourceFinder
{
    public partial class Window : Form
    {
        public Window()
        {
            InitializeComponent();
        }

        string path = "";

        private void buttonGo_Click(object sender, EventArgs e)
        {
            DialogResult result = openFile.ShowDialog();
            if (result == DialogResult.OK)
            {
                path = openFile.FileName;
                PopulateTree(path);
            }
        }

        private void PopulateTree(string path)
        {
            if (File.Exists(path) && Path.GetExtension(path) == ".gmx")
            {
                treeView.Nodes.Clear();

                ProjectResources resources = StrayResourceFinder.FindUnusedResources(path, checkBoxAllRooms.Checked);


                AddResourceToMainTree(resources.sprites, "sprites");
                AddResourceToMainTree(resources.sounds, "sounds");
                AddResourceToMainTree(resources.backgrounds, "backgrounds");
                AddResourceToMainTree(resources.scripts, "scripts");
                AddResourceToMainTree(resources.objects, "objects");
                AddResourceToMainTree(resources.rooms, "rooms");
            }
        }

        private void AddResourceToMainTree(List<Resource> resources, string name)
        {
            if (resources.Count > 0)
            { treeView.Nodes.Add(ResourcesToNode(resources, name + "(" + resources.Count + ")")); }
        }

        private TreeNode ResourcesToNode(List<Resource> resources, string name)
        {
            TreeNode node = new TreeNode(name);
            foreach (Resource r in resources)
            {
                node.Nodes.Add(r.name);
            }
            return node;
        }

        private void button_refresh_Click(object sender, EventArgs e)
        {
            PopulateTree(path);
        }
    }
}
