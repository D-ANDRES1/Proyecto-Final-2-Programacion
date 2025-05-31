## Arquitectura de la Aplicación

Componentes principales:
1. Capa de Presentación (Windows Forms)
   - MainForm: Interfaz de usuario principal
   - AboutForm: Documentación técnica

2. Servicio Groq
   - Comunicación con API Groq
   - Construcción de prompts
   - Parseo de respuestas JSON

3. Integración de Mapas
   - WebView2 con Leaflet/OpenStreetMap
   - Captura de geometrías GeoJSON

Flujo de datos:
Usuario → Dibuja zona en mapa → Captura GeoJSON → 
Construye prompt → Llama a Groq API → 
Procesa respuesta → Muestra resultados

Tecnologías:
- .NET 8.0
- Windows Forms
- WebView2 (Edge Chromium)
- Leaflet 1.9.4 + OpenStreetMap
- Groq API (Mixtral 8x7b)