using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace JutiapaCommunityApp
{
    public class GroqService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _apiUrl = "https://api.groq.com/openai/v1/chat/completions";
        private readonly string _model = "llama3-70b-8192";

        public GroqService()
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                UseProxy = false,
                ServerCertificateCustomValidationCallback = (sender, cert, chain, errors) => true
            })
            {
                Timeout = TimeSpan.FromSeconds(120)
            };

            // Cargar configuración
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _apiKey = config["GroqConfig:ApiKey"] ?? throw new Exception("API key no configurada");
        }

        public async Task<GroqResponse> AnalyzeZoneAsync(string geoJson, string context, string[] analysisTypes)
        {
            int attempt = 0;
            const int maxAttempts = 2;

            while (attempt < maxAttempts)
            {
                try
                {
                    // 1. Validar y simplificar GeoJSON
                    var simpleGeoJson = SimplifyGeoJson(geoJson);
                    SaveDebugFile($"geojson_simplified_attempt{attempt}.json", simpleGeoJson);

                    // 2. Generar prompt optimizado
                    var prompt = GenerateStructuredPrompt(simpleGeoJson, context, analysisTypes);
                    SaveDebugFile($"groq_prompt_attempt{attempt}.txt", prompt);

                    // 3. Construir solicitud API
                    var requestBody = new
                    {
                        model = _model,
                        messages = new[]
                        {
                            new { role = "system", content = GetSystemInstructions() },
                            new { role = "user", content = prompt }
                        },
                        response_format = new { type = "json_object" },
                        temperature = 0.2,
                        max_tokens = 2000
                    };

                    var jsonRequest = JsonSerializer.Serialize(requestBody);
                    SaveDebugFile($"groq_request_attempt{attempt}.json", jsonRequest);

                    // 4. Enviar solicitud
                    var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

                    // 5. Recibir respuesta
                    var response = await _httpClient.PostAsync(_apiUrl, content);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    SaveDebugFile($"groq_response_attempt{attempt}.json", jsonResponse);

                    // 6. Manejar respuesta
                    if (response.IsSuccessStatusCode)
                    {
                        var result = ParseAndCleanResponse(jsonResponse);

                        // Validar contenido útil
                        if (result.ProblemasComunes.Count > 0 ||
                            result.SugerenciasInversion.Count > 0 ||
                            result.IdeasProyectos.Count > 0)
                        {
                            return result;
                        }
                    }

                    // 7. Manejar errores específicos de Groq
                    var errorResponse = HandleGroqSpecificErrors(jsonResponse);
                    if (errorResponse != null) return errorResponse;
                }
                catch (Exception ex)
                {
                    SaveDebugFile($"service_error_attempt{attempt}.txt",
                        $"Mensaje: {ex.Message}\n\nStack Trace: {ex.StackTrace}\n\n{(ex.InnerException?.ToString() ?? "")}");

                    if (attempt == maxAttempts - 1)
                    {
                        throw new ApplicationException("Error analizando la zona después de reintentos", ex);
                    }
                }

                attempt++;
                await Task.Delay(1000); // Espera breve entre intentos
            }

            throw new ApplicationException("No se pudo obtener una respuesta válida después de múltiples intentos");
        }

        private GroqResponse HandleGroqSpecificErrors(string jsonResponse)
        {
            try
            {
                using var doc = JsonDocument.Parse(jsonResponse);
                var root = doc.RootElement;

                if (root.TryGetProperty("error", out var error))
                {
                    // Caso especial: Groq devuelve el JSON en "failed_generation"
                    if (error.TryGetProperty("failed_generation", out var failedGen) &&
                        !string.IsNullOrWhiteSpace(failedGen.GetString()))
                    {
                        var cleanContent = ExtractPureJson(failedGen.GetString()!);
                        SaveDebugFile("groq_fallback_content.json", cleanContent);

                        var result = JsonSerializer.Deserialize<GroqResponse>(cleanContent);
                        if (result != null && (
                            result.ProblemasComunes.Count > 0 ||
                            result.SugerenciasInversion.Count > 0 ||
                            result.IdeasProyectos.Count > 0))
                        {
                            return result;
                        }
                    }
                }
            }
            catch
            {
                // Ignorar errores en el manejo de errores
            }

            return null!;
        }

        private string SimplifyGeoJson(string geoJson)
        {
            try
            {
                using var doc = JsonDocument.Parse(geoJson);
                var root = doc.RootElement;
                return JsonSerializer.Serialize(new
                {
                    type = root.GetProperty("type").GetString(),
                    coordinates = root.GetProperty("coordinates").ToString()
                });
            }
            catch
            {
                return geoJson;
            }
        }

        private string GenerateStructuredPrompt(string geoJson, string context, string[] analysisTypes)
        {
            var areaType = context.Contains("urbana") ? "Urbana" : "Rural";
            var populationMatch = Regex.Match(context, @"población aproximada: (\d+)");
            var population = populationMatch.Success ? populationMatch.Groups[1].Value : "0";

            return $@"
RESPONDER EXCLUSIVAMENTE CON ESTE FORMATO JSON (sin comentarios, sin ```):
{{
  ""problemas_comunes"": [""Ejemplo 1"", ""Ejemplo 2""],
  ""sugerencias_inversion"": [""Ejemplo A"", ""Ejemplo B""],
  ""ideas_proyectos"": [""Ejemplo X"", ""Ejemplo Y""]
}}

REGLAS TÉCNICAS:
- Sin comas después del último elemento en arrays
- Sin saltos de línea dentro de strings
- Máximo 5 elementos por array
- Usar comillas dobles siempre
- Osea un json puro

CONTEXTO:
- Municipio: Jutiapa, Guatemala ({areaType})
- Población: {population}
- Áreas a analizar: {string.Join(", ", analysisTypes)}

GENERAR PARA:
1. Problemas urgentes (3-5)
2. Inversiones <$5000 (3-5)
3. Proyectos sostenibles (3-5)";
        }

        private string GetSystemInstructions()
        {
            return @"Eres un experto en desarrollo comunitario para Guatemala. Reglas:
1. Respuesta SOLO en formato JSON válido
2. Estructura: 
{
  ""problemas_comunes"": [""texto""],
  ""sugerencias_inversion"": [""texto""],
  ""ideas_proyectos"": [""texto""]
}
3. NUNCA usar ```json o bloques de código
4. Máximo 5 items por categoría
5. No incluir texto adicional fuera del JSON";
        }

        private GroqResponse ParseAndCleanResponse(string jsonResponse)
        {
            try
            {
                using var doc = JsonDocument.Parse(jsonResponse);
                var root = doc.RootElement;

                // Obtener contenido real de la respuesta
                var content = root.GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString()!;

                // Limpiar y procesar
                var cleanContent = ExtractPureJson(content);
                SaveDebugFile("groq_content_clean.json", cleanContent);

                return JsonSerializer.Deserialize<GroqResponse>(cleanContent)
                    ?? throw new Exception("Deserialización devolvió nulo");
            }
            catch (Exception ex)
            {
                SaveDebugFile("parse_error.json", $"Error: {ex.Message}\nResponse: {jsonResponse}");
                throw;
            }
        }

        private string ExtractPureJson(string content)
        {
            // Paso 1: Limpieza básica
            content = content.Replace("```json", "")
                             .Replace("```", "")
                             .Trim();

            // Paso 2: Intentar extraer JSON válido
            var jsonMatch = Regex.Match(content, @"\{[\s\S]*\}");
            if (!jsonMatch.Success) return "{}";

            string json = jsonMatch.Value;
            SaveDebugFile("raw_extracted.json", json);

            // Paso 3: Corrección automática de errores comunes
            return FixJsonErrors(json);
        }

        private string FixJsonErrors(string json)
        {
            // Corrección 1: Comas extra en arrays
            json = Regex.Replace(json, @",\s*([}\]])", "$1");

            // Corrección 2: Llaves mal cerradas en arrays
            json = Regex.Replace(json, @"(\]\s*,\s*)?\}\s*,\s*""([a-z_]+)""\s*:",
                m => $"]}},\"{m.Groups[2].Value}\":");

            // Corrección 3: Arrays mal cerrados
            json = Regex.Replace(json, @"(\w+""\s*:\s*\[[^\]]*)\}", "$1]}");

            // Corrección 4: Comillas faltantes en keys
            json = Regex.Replace(json, @"([\{,])\s*([a-zA-Z_]+)\s*:", "$1\"$2\":");

            // Validación final
            try
            {
                using var doc = JsonDocument.Parse(json);
                return json;
            }
            catch
            {
                // Último recurso: reconstruir estructura básica
                return BuildFallbackJson(json);
            }
        }

        private string BuildFallbackJson(string brokenJson)
        {
            try
            {
                var problems = ExtractList(brokenJson, "problemas_comunes");
                var investments = ExtractList(brokenJson, "sugerencias_inversion");
                var projects = ExtractList(brokenJson, "ideas_proyectos");

                return JsonSerializer.Serialize(new GroqResponse
                {
                    ProblemasComunes = problems,
                    SugerenciasInversion = investments,
                    IdeasProyectos = projects
                });
            }
            catch
            {
                return "{}";
            }
        }

        private List<string> ExtractList(string json, string key)
        {
            var pattern = $@"""{key}""\s*:\s*\[([^\]]*)\]";
            var match = Regex.Match(json, pattern);

            if (!match.Success) return new List<string>();

            var items = match.Groups[1].Value.Split(',')
                .Select(item => item.Trim(' ', '"', '\n', '\r'))
                .Where(item => !string.IsNullOrWhiteSpace(item))
                .ToList();

            return items;
        }

        private void SaveDebugFile(string fileName, string content)
        {
            try
            {
                var debugDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DebugLogs");
                Directory.CreateDirectory(debugDir);
                var path = Path.Combine(debugDir, fileName);
                File.WriteAllText(path, content);
                Debug.WriteLine($"DEBUG SAVED: {path}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LOG FAILURE: {ex.Message}");
                try
                {
                    var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    File.WriteAllText(Path.Combine(desktop, $"EMERGENCY_{fileName}"),
                        $"Log error: {ex.Message}\n\n{content}");
                }
                catch { /* Ignorar errores finales */ }
            }
        }
    }

    public class GroqResponse
    {
        public List<string> ProblemasComunes { get; set; } = new List<string>();
        public List<string> SugerenciasInversion { get; set; } = new List<string>();
        public List<string> IdeasProyectos { get; set; } = new List<string>();
    }
}