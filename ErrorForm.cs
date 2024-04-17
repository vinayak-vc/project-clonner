using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectCloner {

    public partial class ErrorForm : Form {

        public ErrorForm() {
            InitializeComponent();
        }

        public void ErrorText(string text) {
            ErrorLable.Text = text;
        }

        private void Close_button_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}