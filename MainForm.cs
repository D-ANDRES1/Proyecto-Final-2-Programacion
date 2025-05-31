// MainForm.cs
using JutiapaCommunityApp;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;

namespace JutiapaCommunityApp
{
    public partial class MainForm : Form
    {
        private GroqService _groqService;
        private string _currentGeoJson = "";

        public MainForm()
        {
            InitializeComponent();
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            try
            {
                // Inicializar WebView2 primero
                await InitializeWebView2Async();

                // Luego inicializar el mapa
                InitializeMap();

                // Finalmente inicializar el servicio Groq
                _groqService = new GroqService();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error durante la inicialización: {ex.Message}", "Error crítico",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task InitializeWebView2Async()
        {
            // Crear un entorno explícito para WebView2
            var env = await CoreWebView2Environment.CreateAsync();

            // Inicializar el control con el entorno creado
            await mapWebView.EnsureCoreWebView2Async(env);

            // Configuraciones adicionales
            var settings = mapWebView.CoreWebView2.Settings;
            settings.IsWebMessageEnabled = true;
            settings.AreDevToolsEnabled = true;

            // Registrar eventos para diagnóstico
            mapWebView.CoreWebView2.NavigationCompleted += (s, e) => {
                Debug.WriteLine($"Navegación completada: {e.IsSuccess}");
            };

            mapWebView.CoreWebView2.DOMContentLoaded += (s, e) => {
                Debug.WriteLine("DOM completamente cargado");
            };

            // Registrar el manejador de mensajes aquí para asegurar disponibilidad
            mapWebView.CoreWebView2.WebMessageReceived += MapWebView_WebMessageReceived;
        }
        private void InitializeMap()
        {
            // Cargar directamente el HTML en la vista
            string htmlContent = GenerateMapHtml();
            mapWebView.CoreWebView2.NavigateToString(htmlContent);
        }

        private string GenerateMapHtml()
        {
            return @"
            <!DOCTYPE html>
            <html>
            <head>
                <title>Mapa de Jutiapa</title>
                <meta charset=""utf-8"" />
                <link rel=""stylesheet"" href=""https://unpkg.com/leaflet@1.9.4/dist/leaflet.css"" />
                <script src=""https://unpkg.com/leaflet@1.9.4/dist/leaflet.js""></script>
                <link rel=""stylesheet"" href=""https://cdnjs.cloudflare.com/ajax/libs/leaflet.draw/1.0.4/leaflet.draw.css"" />
                <script src=""https://cdnjs.cloudflare.com/ajax/libs/leaflet.draw/1.0.4/leaflet.draw.js""></script>
                <style>
                    body, html {
                        margin: 0;
                        padding: 0;
                        height: 100%;
                        width: 100%;
                        overflow: hidden;
                    }
                    #map {
                        height: 100vh;
                        width: 100%;
                    }
                </style>
            </head>
            <body>
                <div id=""map""></div>
                <script>
                    // Solución definitiva para problemas de carga
                    function initMap() {
                        console.log('Inicializando mapa...');
                        
                        const map = L.map('map').setView([14.2917, -89.8958], 12);
                        
                        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                            attribution: '&copy; <a href=""https://www.openstreetmap.org/copyright"">OpenStreetMap</a>',
                            maxZoom: 19
                        }).addTo(map);
                        
                        const drawnItems = new L.FeatureGroup();
                        map.addLayer(drawnItems);
                        
                        const drawControl = new L.Control.Draw({
                            draw: {
                                polygon: {
                                    allowIntersection: false,
                                    showArea: true
                                },
                                rectangle: {
                                    showArea: true,
                                    metric: true
                                },
                                circle: false,
                                marker: true,
                                polyline: false
                            },
                            edit: {
                                featureGroup: drawnItems
                            }
                        });
                        map.addControl(drawControl);
                        
                        map.on(L.Draw.Event.CREATED, function(e) {
                            const layer = e.layer;
                            drawnItems.addLayer(layer);
                            
                            // Convertir a GeoJSON con estructura simplificada
                            const geojson = {
                                type: 'Feature',
                                geometry: layer.toGeoJSON().geometry,
                                properties: {
                                    layerType: e.layerType,
                                    area: layer.getArea ? layer.getArea() : 0
                                }
                            };
                            
                            console.log('GeoJSON generado:', geojson);
                            
                            if (window.chrome && window.chrome.webview) {
                                window.chrome.webview.postMessage(JSON.stringify(geojson));
                            } else {
                                console.error('WebView2 no está disponible');
                            }
                        });
                        
                        map.on(L.Draw.Event.DELETED, function() {
                            if (drawnItems.getLayers().length === 0) {
                                if (window.chrome && window.chrome.webview) {
                                    window.chrome.webview.postMessage(""CLEAR"");
                                }
                            }
                        });
                        
                        console.log('Mapa completamente inicializado');
                    }
                    
                    // Inicializar cuando el DOM esté listo
                    document.addEventListener('DOMContentLoaded', initMap);
                </script>
            </body>
            </html>";
        }

        private void MapWebView_WebMessageReceived(object sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e)
        {
            {
                var msg = e.TryGetWebMessageAsString();
                Debug.WriteLine($"Mensaje recibido: {msg}");

                if (msg == "CLEAR")
                {
                    _currentGeoJson = "";
                    geoJsonTextBox.Text = "";
                    return;
                }

                try
                {
                    // Procesar el GeoJSON
                    var jsonDoc = JsonDocument.Parse(msg);
                    var root = jsonDoc.RootElement;

                    // Simplificar la estructura para enviar a Groq
                    var simplifiedGeoJson = new JsonObject
                    {
                        ["type"] = root.GetProperty("geometry").GetProperty("type").GetString(),
                        ["coordinates"] = JsonNode.Parse(root.GetProperty("geometry").GetProperty("coordinates").ToString())
                    };

                    _currentGeoJson = simplifiedGeoJson.ToJsonString();
                    geoJsonTextBox.Text = JsonSerializer.Serialize(
                        JsonDocument.Parse(_currentGeoJson).RootElement,
                        new JsonSerializerOptions { WriteIndented = true }
                    );
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error procesando GeoJSON: {ex.Message}");
                    MessageBox.Show($"Error al procesar la zona: {ex.Message}", "Error de formato",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private async void analyzeButton_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(_currentGeoJson))
            {
                MessageBox.Show("Por favor dibuje una zona en el mapa primero", "Zona no seleccionada",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            analyzeButton.Enabled = false;
            loadingIndicator.Visible = true;
            resultsTabControl.Visible = false; // Ocultar resultados hasta tener respuesta

            try
            {
                var context = $"Área {(urbanRadioButton.Checked ? "urbana" : "rural")} del municipio de Jutiapa, " +
                             $"población aproximada: {populationNumeric.Value} habitantes.";

                var analysisTypes = new System.Collections.Generic.List<string>();
                if (infraCheckBox.Checked) analysisTypes.Add("infraestructura");
                if (healthCheckBox.Checked) analysisTypes.Add("salud");
                if (educationCheckBox.Checked) analysisTypes.Add("educación");
                if (businessCheckBox.Checked) analysisTypes.Add("emprendimiento");

                var response = await _groqService.AnalyzeZoneAsync(_currentGeoJson, context, analysisTypes.ToArray());
                DisplayResults(response);
                resultsTabControl.Visible = true; // Mostrar resultados
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al analizar la zona: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                analyzeButton.Enabled = true;
                loadingIndicator.Visible = false;
            }
        }

        private void DisplayResults(GroqResponse response)
        {
            void PopulateListBox(ListBox listBox, List<string> items, string emptyMessage)
            {
                listBox.Items.Clear();

                if (items == null || items.Count == 0)
                {
                    listBox.Items.Add(emptyMessage);
                    listBox.ForeColor = Color.Red;
                    return;
                }

                foreach (var item in items)
                    listBox.Items.Add(item);

                listBox.ForeColor = SystemColors.WindowText;
            }

            PopulateListBox(problemsListBox, response.ProblemasComunes,
                           "No se identificaron problemas comunes");

            PopulateListBox(investmentsListBox, response.SugerenciasInversion,
                           "No se identificaron sugerencias de inversión");

            PopulateListBox(projectsListBox, response.IdeasProyectos,
                           "No se identificaron ideas de proyectos");
        }
        

        private void aboutButton_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        // Botón para abrir herramientas de desarrollo (agregar en el diseñador)
        private void btnDevTools_Click(object sender, EventArgs e)
        {
            try
            {
                mapWebView.CoreWebView2.OpenDevToolsWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error abriendo DevTools: {ex.Message}");
            }
        }
    }





}