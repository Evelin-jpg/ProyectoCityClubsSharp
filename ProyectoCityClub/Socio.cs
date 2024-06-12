using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net.Http.Formatting;

namespace ProyectoCityClub
{
    public partial class Socio : Form
    {

        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7093/api/Socio";

        public Socio()
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
                HttpResponseMessage response = await _httpClient.GetAsync($"{ApiUrl}/GetActiveSocios");

                if (response.IsSuccessStatusCode)
                {
                    var socios = await response.Content.ReadAsAsync<List<Socium>>(new[] { new JsonMediaTypeFormatter() });
                    dataGridView1.DataSource = socios;
                }
                else
                {
                    MessageBox.Show("Error al cargar los datos de socios.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_LimpiarS_Click(object sender, EventArgs e)
        {
            txtBuscarID.Clear();
            txtFechaIngreso.Clear();
            txtNombre.Clear();
            txtTelefono.Clear();
        }

        private async void btn_agregarSocio_Click(object sender, EventArgs e)
        {
            var nuevoSocio = new Socium
            {
                Nombre = txtNombre.Text,
                Telefono = txtTelefono.Text,
                FechaIngreso = txtFechaIngreso.Text,
                Status = 1 // El nuevo socio estará activo por defecto
            };

            var json = JsonConvert.SerializeObject(nuevoSocio);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(ApiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Socio agregado con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataAsync(); // Actualiza la tabla después de agregar
            }
            else
            {
                MessageBox.Show("Error al agregar el socio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btn_modificarSocio_Click(object sender, EventArgs e)
        {
            int id;
            if (int.TryParse(txtBuscarID.Text, out id))
            {
                var socioActualizado = new Socium
                {
                    IDSocio = id,
                    Nombre = txtNombre.Text,
                    Telefono = txtTelefono.Text,
                    FechaIngreso = txtFechaIngreso.Text,
                    Status = chkStatus.Checked ? 1 : 0
                };

                var json = JsonConvert.SerializeObject(socioActualizado);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync($"{ApiUrl}/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Socio modificado con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataAsync(); // Recargar los datos del DataGridView
                }
                else
                {
                    MessageBox.Show("Error al modificar el socio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un ID válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btn_eliminarSocio_Click(object sender, EventArgs e)
        {
            int id;
            if (int.TryParse(txtBuscarID.Text, out id))
            {
                HttpResponseMessage response = await _httpClient.PutAsync($"{ApiUrl}/inactivar/{id}", null);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Socio eliminado con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataAsync(); // Recargar los datos del DataGridView
                }
                else
                {
                    MessageBox.Show("Error al eliminar el socio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un ID válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_actualizarTablaSocio_Click(object sender, EventArgs e)
        {
            LoadDataAsync(); // Actualiza la tabla de socios

        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            int id;
            if (int.TryParse(txtBuscarID.Text, out id))
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{ApiUrl}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var socio = await response.Content.ReadAsAsync<Socium>(new[] { new JsonMediaTypeFormatter() });
                    txtNombre.Text = socio.Nombre;
                    txtTelefono.Text = socio.Telefono;
                    txtFechaIngreso.Text = socio.FechaIngreso;
                    chkStatus.Checked = socio.Status == 1;
                }
                else
                {
                    MessageBox.Show("Socio no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un ID válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
    }
}
