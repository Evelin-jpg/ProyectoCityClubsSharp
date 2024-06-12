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
    public partial class Membresia : Form
    {

        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7093/api/Membresia";

        public Membresia()
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
                HttpResponseMessage response = await _httpClient.GetAsync($"{ApiUrl}/GetActiveMembresias");

                if (response.IsSuccessStatusCode)
                {
                    var usuarios = await response.Content.ReadAsAsync<List<Membresium>>(new[] { new JsonMediaTypeFormatter() });
                    dataGridView1.DataSource = usuarios;
                    //dataGridView1.Columns["IDMembresia"].Visible = false;
                    //dataGridView1.Columns["Status"].Visible = false;
                }
                else
                {
                    MessageBox.Show("Error al cargar los datos de membresia.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btn_agregarMembresia_Click(object sender, EventArgs e)
        {
            var nuevaMembresia = new Membresium
            {
                Numero = txtNumero.Text,
                Tipo = txtTipo.Text,
                Premia = Convert.ToInt32(txtPremia.Text),
                Status = 1 // La nueva membresía estará activa por defecto
            };

            var json = JsonConvert.SerializeObject(nuevaMembresia);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(ApiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Membresía agregada con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataAsync(); // Actualiza la tabla después de agregar
            }
            else
            {
                MessageBox.Show("Error al agregar la membresía.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btn_modificarMembresia_Click(object sender, EventArgs e)
        {
            int id;
            if (int.TryParse(txtBuscarID.Text, out id))
            {
                var membresiaActualizada = new Membresium
                {
                    IDMembresia = id, // Asegúrate de asignar el ID de la membresía que se está modificando
                    Numero = txtNumero.Text,
                    Tipo = txtTipo.Text,
                    Premia = Convert.ToInt32(txtPremia.Text),
                    Status = chkStatus.Checked ? 1 : 0 // Suponiendo que Status es un entero
                };

                var json = JsonConvert.SerializeObject(membresiaActualizada);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync($"{ApiUrl}/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Membresía modificada con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataAsync(); // Recargar los datos del DataGridView
                }
                else
                {
                    MessageBox.Show("Error al modificar la membresía.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un ID válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btn_eliminarMembresia_Click(object sender, EventArgs e)
        {
            int id;
            if (int.TryParse(txtBuscarID.Text, out id))
            {
                HttpResponseMessage response = await _httpClient.PutAsync($"{ApiUrl}/inactivar/{id}", null);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Membresía eliminada con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataAsync(); // Recargar los datos del DataGridView
                }
                else
                {
                    MessageBox.Show("Error al eliminar la membresía.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un ID válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_actualizarTablaMembresia_Click(object sender, EventArgs e)
        {
            LoadDataAsync(); // Actualiza la tabla de membresías

        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            int id;
            if (int.TryParse(txtBuscarID.Text, out id))
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{ApiUrl}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var membresia = await response.Content.ReadAsAsync<Membresium>(new[] { new JsonMediaTypeFormatter() });
                    txtNumero.Text = membresia.Numero;
                    txtPremia.Text = membresia.Premia.ToString(); // Convertir int a string
                    txtTipo.Text = membresia.Tipo;
                    chkStatus.Checked = membresia.Status == 1; // Suponiendo que Status es un entero
                }
                else
                {
                    MessageBox.Show("Membresía no encontrada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un ID válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_LimpiarM_Click(object sender, EventArgs e)
        {
            txtBuscarID.Clear();
            txtNumero.Clear();
            txtPremia.Clear();
            txtTipo.Clear();
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
