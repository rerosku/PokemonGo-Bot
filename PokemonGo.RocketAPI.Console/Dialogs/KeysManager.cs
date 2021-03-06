﻿/*
 * Created by SharpDevelop.
 * User: Xelwon
 * Date: 02/03/2017
 * Time: 23:58
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using PokemonGo.RocketAPI.Hash;

namespace PokeMaster.Dialogs
{
    /// <summary>
    /// Description of KeysManager.
    /// </summary>
    public partial class KeysManager : Form
    {
        private static string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs");
        private static string filename = Path.Combine(path, "keys.json");
        public KeysManager()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            if (File.Exists(filename)){
                var strJSON = File.ReadAllText(filename);
                var keys1 = JsonConvert.DeserializeObject<List<string>>(strJSON);
                listView.Items.Clear();
                foreach ( var element in keys1) {
                   AddKey(element);
                }
            }
        }
        void buttonAcept_Click(object sender, EventArgs e)
        {
            var strings = new List<string>();
            foreach (ListViewItem element in listView.Items) {
                strings.Add(element.Text);
            }
          string strJSON = JsonConvert.SerializeObject(strings,Formatting.Indented);
          File.WriteAllText(filename,strJSON);
          Close();
        }
        void buttonDelete_Click(object sender, EventArgs e)
        {
            for (var i = listView.SelectedItems.Count -1;i>=0;i--)
                listView.Items.Remove(listView.SelectedItems[i]);
        }
        void AddKey(string key){
            var listItem = listView.Items.Add(key);
            var hashInfo = PokeHashHasher.GetInformation(key);
            listItem.SubItems.Add( hashInfo[0]);
            listItem.SubItems.Add( hashInfo[1]);
        }
        void buttonAdd_Click(object sender, EventArgs e)
        {
            AddKey(textBox1.Text);
            textBox1.Text ="";
        }
        void listView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            var order = (sender as ListView).Sorting;
            listView.ListViewItemSorter = new Components.ListViewItemComparer(e.Column, order);
            (sender as ListView).Sorting = order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
        }
    }
}
