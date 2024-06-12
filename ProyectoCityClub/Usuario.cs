using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using ProyectoCityClub;
using System.Net.Http.Formatting;

namespace ProyectoCityClub
{
    public partial class Usuario : Form
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7093/api/Usuario";

        public Usuario()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{ApiUrl}/GetActiveUsuarios");

                if (response.IsSuccessStatusCode)
                {
                    var usuarios = await response.Content.ReadAsAsync<List<Usuarium>>(new[] { new JsonMediaTypeFormatter() });
                    dataGridView1.DataSource = usuarios;
                    //dataGridView1.Columns["IDUsuario"].Visible = false;
                    //dataGridView1.Columns["Status"].Visible = false;
                }
                else
                {
                    MessageBox.Show("Error al cargar los datos de usuarios.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btn_agregarUsuario_Click(object sender, EventArgs e)
        {
            var nuevoUsuario = new Usuarium
            {
                NumEmpleado = txtNumEmpleado.Text,
                Nombre = txtNombre.Text,
                Contrasena = txtContrasena.Text,
                Status = true // El nuevo usuario estará activo por defecto
            };

            var json = JsonConvert.SerializeObject(nuevoUsuario);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(ApiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Usuario agregado con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataAsync(); // Actualiza la tabla después de agregar
            }
            else
            {
                MessageBox.Show("Error al agregar el usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        

        


        private void btn_LimpiarU_Click(object sender, EventArgs e)
        {
            txtBuscarID.Clear();
            txtNumEmpleado.Clear();
            txtContrasena.Clear();
            txtNombre.Clear(); 
        }

        private void Usuario_Load(object sender, EventArgs e)
        {

        }

        private void btn_VolverMenu_Click(object sender, EventArgs e)
        {
            // Hide the current form
            this.Hide();

            // Open the Menu form
            Menu menu2 = new Menu();
            menu2.ShowDialog();

            // Close the current form after the new form is closed
            this.Close();
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            int id;
            if (int.TryParse(txtBuscarID.Text, out id))
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{ApiUrl}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var usuario = await response.Content.ReadAsAsync<Usuarium>(new[] { new JsonMediaTypeFormatter() });
                    txtNumEmpleado.Text = usuario.NumEmpleado;
                    txtNombre.Text = usuario.Nombre;
                    txtContrasena.Text = usuario.Contrasena;
                    chkStatus.Checked = usuario.Status;
                }
                else
                {
                    MessageBox.Show("Usuario no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un ID válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void modificarUsuario_Click(object sender, EventArgs e)
        {
            int id;
            if (int.TryParse(txtBuscarID.Text, out id))
            {
                var usuarioActualizado = new Usuarium
                {
                    IDUsuario = id,
                    NumEmpleado = txtNumEmpleado.Text,
                    Nombre = txtNombre.Text,
                    Contrasena = txtContrasena.Text,
                    Status = chkStatus.Checked
                };

                var json = JsonConvert.SerializeObject(usuarioActualizado);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync($"{ApiUrl}/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Usuario actualizado con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataAsync(); // Recargar los datos del DataGridView
                }
                else
                {
                    MessageBox.Show("Error al actualizar el usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un ID válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btn_eliminarUsuario_Click(object sender, EventArgs e)
        {
            // Verificar si hay una fila seleccionada
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Obtener el IDUsuario de la fila seleccionada
                var id = (int)dataGridView1.SelectedRows[0].Cells["IDUsuario"].Value;

                // Enviar una solicitud PUT a la API para inactivar el usuario
                HttpResponseMessage response = await _httpClient.PutAsync($"{ApiUrl}/inactivar/{id}", null);

                // Verificar si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Usuario eliminado con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataAsync(); // Recargar los datos del DataGridView
                }
                else
                {
                    MessageBox.Show("Error al eliminar el usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Selecciona un usuario para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_actualizarTablaUsuario_Click(object sender, EventArgs e)
        {
            LoadDataAsync(); // Actualiza la tabla de usuarios

        }
    }
}

