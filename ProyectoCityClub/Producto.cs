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
    public partial class Producto : Form
    {

        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7093/api/Producto";

        public Producto()
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
                HttpResponseMessage response = await _httpClient.GetAsync($"{ApiUrl}/GetActiveProductos");

                if (response.IsSuccessStatusCode)
                {
                    var productos = await response.Content.ReadAsAsync<List<Productium>>(new[] { new JsonMediaTypeFormatter() });
                    dataGridView1.DataSource = productos;
                }
                else
                {
                    MessageBox.Show("Error al cargar los datos de productos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            txtCantidad.Clear();
            txtClave.Clear();
            txtNombre.Clear();
            txtPrecio.Clear();

        }

        private async void btn_agregarProducto_Click(object sender, EventArgs e)
        {
            var nuevoProducto = new Productium
            {
                Clave = txtClave.Text,
                Nombre = txtNombre.Text,
                Precio = txtPrecio.Text,
                Cantidad = txtCantidad.Text,
                Status = 1 // El nuevo producto estará activo por defecto
            };

            var json = JsonConvert.SerializeObject(nuevoProducto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(ApiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Producto agregado con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataAsync(); // Actualiza la tabla después de agregar
            }
            else
            {
                MessageBox.Show("Error al agregar el producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btn_modificarProducto_Click(object sender, EventArgs e)
        {
            int id;
            if (int.TryParse(txtBuscarID.Text, out id))
            {
                var productoActualizado = new Productium
                {
                    IDProducto = id,
                    Clave = txtClave.Text,
                    Nombre = txtNombre.Text,
                    Precio = txtPrecio.Text,
                    Cantidad = txtCantidad.Text,
                    Status = chkStatus.Checked ? 1 : 0
                };

                var json = JsonConvert.SerializeObject(productoActualizado);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync($"{ApiUrl}/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Producto modificado con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataAsync(); // Recargar los datos del DataGridView
                }
                else
                {
                    MessageBox.Show("Error al modificar el producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un ID válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btn_eliminarProducto_Click(object sender, EventArgs e)
        {
            int id;
            if (int.TryParse(txtBuscarID.Text, out id))
            {
                HttpResponseMessage response = await _httpClient.PutAsync($"{ApiUrl}/inactivar/{id}", null);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Producto eliminado con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataAsync(); // Recargar los datos del DataGridView
                }
                else
                {
                    MessageBox.Show("Error al eliminar el producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un ID válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_actualizarTablaProducto_Click(object sender, EventArgs e)
        {
            LoadDataAsync(); // Actualiza la tabla de productos

        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            int id;
            if (int.TryParse(txtBuscarID.Text, out id))
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{ApiUrl}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var producto = await response.Content.ReadAsAsync<Productium>(new[] { new JsonMediaTypeFormatter() });
                    txtClave.Text = producto.Clave;
                    txtNombre.Text = producto.Nombre;
                    txtPrecio.Text = producto.Precio;
                    txtCantidad.Text = producto.Cantidad;
                    chkStatus.Checked = producto.Status == 1;
                }
                else
                {
                    MessageBox.Show("Producto no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
