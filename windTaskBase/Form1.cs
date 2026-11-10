using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace windTaskBase
{
    public partial class Form1 : Form
    {
        private readonly string _connectionString = "server=localhost;port=3306;user=root;password=root;database=messenger";
        private readonly UserRepository _usersRepository;
        public Form1()
        {
            InitializeComponent();
            _usersRepository = new UserRepository(_connectionString);

            UpdateListBox();
        }

        private void UpdateListBox()
        {
            List<windTaskBase.User> list = _usersRepository.GetAll();

            listBox1.Items.Clear();

            foreach (var user in list)
            {
                listBox1.Items.Add($"{user.Id}. {user.Name}, {user.Email}, Created at: ({user.GetFormattedDate(user.CreatedAt)}), Birthday: ({user.GetFormattedDate(user.Birthday)})");
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userName = textBox1.Text.Trim();
            string email = textBox2.Text.Trim();
            string password = textBox3.Text.Trim();
            DateTime Birthday = dateTimePicker1.Value;

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter username, email and password.");
                return;
            }

            if (DateTime.Now.Year - Birthday.Year < 18)
            {
                MessageBox.Show("Too young to register.");
                return;
            }

            if (_usersRepository.Create(userName, email, password, Birthday) > 0)
            {
                MessageBox.Show("Registered successfully.");
            }
            else
            {
                MessageBox.Show("Failed to register.");
            }

            UpdateListBox();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UpdateListBox();
            MessageBox.Show("Updated successfully.");
        }
    }
}
