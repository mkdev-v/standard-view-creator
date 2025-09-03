using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Organization;
using StandardViewCreator.Models;

namespace StandardViewCreator
{
    public partial class ViewSelector : Form
    {
        public ViewModel SelectedItem { get; private set; }

        public ViewSelector(List<ViewModel> views)
        {
            InitializeComponent();

            foreach (var view in views)
            {
                var item = new System.Windows.Forms.ListViewItem(view.Guid.ToString());
                item.SubItems.Add(view.Name);
                item.SubItems.Add(view.Entity);
                item.SubItems.Add(view.Type);
                item.Tag = view; // 選択時に取り出せるようにオブジェクトを保持
                tlpMain_lvwViews.Items.Add(item);
            }

            tlpMain_lvwViews.SelectedIndexChanged += (s, e) =>
            {
                if (tlpMain_lvwViews.SelectedItems.Count > 0)
                {
                    SelectedItem = tlpMain_lvwViews.SelectedItems[0].Tag as ViewModel;
                }
            };

            tlpMain_btnOK.Click += (s, e) =>
            {
                if (SelectedItem != null)
                    this.DialogResult = DialogResult.OK;
                else
                    MessageBox.Show("Solution must be selected.");
            };

            tlpMain_btnCancel.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
            };
        }
    }
}
