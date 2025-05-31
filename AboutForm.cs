// AboutForm.cs
using System;
using System.Windows.Forms;

namespace JutiapaCommunityApp
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            LoadDocumentation();
        }

        private void LoadDocumentation()
        {
            documentationTextBox.Text = $@"=== DOCUMENTACIÓN TÉCNICA ===

API de Mapas:
- Leaflet v1.9.4
- OpenStreetMap
- Leaflet.draw v1.0.4

Endpoint Groq:
- URL: https://api.groq.com/openai/v1/chat/completions
- Modelo: llama3-70b-8192

Prompt Ejemplo:
{{
  ""zone"": {{ ""type"": ""Polygon"", ""coordinates"": [[[-89.92,14.29],[-89.90,14.30],...]] }},
  ""context"": ""Área rural, población aproximada 5000 habitantes"",
  ""analysis"": [""infraestructura"",""salud"",""educación"",""emprendimiento""]
}}

Esquema de Respuesta Esperada:
{{
  ""problemas_comunes"": [""Falta de acceso a agua potable"", ...],
  ""sugerencias_inversion"": [""Sistemas de captación de agua pluvial"", ...],
  ""ideas_proyectos"": [""Talleres de agricultura sostenible"", ...]
}}

Instrucciones:
1. Obtener API key gratuita en groq.com
2. Guardar clave en appsettings.json
3. La aplicación es de código abierto (MIT License)";
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}