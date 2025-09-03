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
    public partial class SolutionSelector : Form
    {
        public SolutionModel SelectedItem { get; private set; }

        public SolutionSelector(List<SolutionModel> solutions)
        {
            InitializeComponent();

            foreach (var solution in solutions)
            {
                var item = new System.Windows.Forms.ListViewItem(solution.DisplayName);
                item.SubItems.Add(solution.Version);
                item.SubItems.Add(solution.Publisher);
                item.SubItems.Add(solution.SolutionId);
                item.Tag = solution; // 選択時に取り出せるようにオブジェクトを保持
                tlpMain_lvwSolutions.Items.Add(item);
            }

            tlpMain_lvwSolutions.SelectedIndexChanged += (s, e) =>
            {
                if (tlpMain_lvwSolutions.SelectedItems.Count > 0)
                {
                    SelectedItem = tlpMain_lvwSolutions.SelectedItems[0].Tag as SolutionModel;
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
