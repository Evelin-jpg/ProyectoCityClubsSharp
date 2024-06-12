using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoCityClub
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Hide the current form
            this.Hide();

            // Open the Membresia form
            Membresia membresia2 = new Membresia();
            membresia2.ShowDialog();

            // Close the current form after the new form is closed
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Hide the current form
            this.Hide();

            // Open the Producto form
            Producto producto2 = new Producto();
            producto2.ShowDialog();

            // Close the current form after the new form is closed
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Hide the current form
            this.Hide();

            // Open the Socio form
            Socio socio2 = new Socio();
            socio2.ShowDialog();

            // Close the current form after the new form is closed
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Hide the current form
            this.Hide();

            // Open the Usuario form
            Usuario usuario2 = new Usuario();
            usuario2.ShowDialog();

            // Close the current form after the new form is closed
            this.Close();
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("¿Deseas cerrar sesión?", "Confirmar cierre de sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                // Ocultar el formulario actual
                this.Hide();

                // Abrir el formulario de Login
                Login loginForm = new Login();
                loginForm.Show();

                // Mostrar el mensaje de éxito al cerrar sesión
                MessageBox.Show("La sesión ha sido cerrada con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
