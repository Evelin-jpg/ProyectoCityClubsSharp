using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoCityClub
{
    public partial class Login : Form
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:7093/api/Usuario/autenticar";

        public Login()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
        }

        private async void btn_Login_Click(object sender, EventArgs e)
        {
            string nombre = txt_nombreUsuario.Text;
            string contrasena = txt_contraseñaUsuario.Text;

            // Construir la URL con los parámetros de consulta
            string url = $"{BaseUrl}?nombre={nombre}&contrasena={contrasena}";

            // Realizar la solicitud a la API
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            // Verificar si la solicitud fue exitosa
            if (response.IsSuccessStatusCode)
            {
                // Procesar la respuesta de la API (si es necesario)
                // Por ejemplo, puedes leer el contenido de la respuesta
                // string responseBody = await response.Content.ReadAsStringAsync();

                // Si la autenticación es exitosa, muestra el formulario de menú
                // Hide the current form
                Menu menuForm = new Menu();
                menuForm.Show();
                this.Hide();
            }
            else
            {
                // Si la autenticación falla, muestra un mensaje de error
                MessageBox.Show("Nombre de usuario o contraseña incorrectos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            txt_nombreUsuario.Text = string.Empty;
            txt_contraseñaUsuario.Text = string.Empty;
        }
    }
}
