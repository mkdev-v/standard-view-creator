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

namespace StandardViewCreator
{
    public partial class SolutionSelecter : Form
    {
        public SolutionInfo SelectedItem { get; private set; }

        public SolutionSelecter(List<SolutionInfo> solutions)
        {
            InitializeComponent();

            foreach (var solution in solutions)
            {
                var item = new System.Windows.Forms.ListViewItem(solution.DisplayName);
                item.SubItems.Add(solution.Version);
                item.SubItems.Add(solution.Publisher);
                item.SubItems.Add(solution.SolutionId);
                item.Tag = solution; // 選択時に取り出せるようにオブジェクトを保持
                lvwSolutions.Items.Add(item);
            }

            lvwSolutions.SelectedIndexChanged += (s, e) =>
            {
                if (lvwSolutions.SelectedItems.Count > 0)
                {
                    SelectedItem = lvwSolutions.SelectedItems[0].Tag as SolutionInfo;
                }
            };

            buttonOK.Click += (s, e) =>
            {
                if (SelectedItem != null)
                    this.DialogResult = DialogResult.OK;
                else
                    MessageBox.Show("Solution must be selected.");
            };

            buttonCancel.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
            };
        }
    }
}
