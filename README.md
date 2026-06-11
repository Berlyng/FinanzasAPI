# Finanzas API - v1.0.0

¡Bienvenido a **Finanzas API**! Esta es la primera versión estable (`v1.0.0`) de una solución backend robusta diseñada para la gestión, control y análisis de finanzas personales. La API permite registrar transacciones, gestionar presupuestos con alertas inteligentes y generar reportes financieros avanzados optimizados directamente desde la base de datos.

## 🚀 Características Principales

* **Gestión de Transacciones:** Registro completo de ingresos y gastos con categorización dinámica.
* **Control de Presupuestos Inteligente:**
  * Asignación de límites mensuales por categoría.
  * **Sistema de alertas automático** según el porcentaje de uso: `Safe` (Seguro), `Warning` (Advertencia), `Critical` (Crítico) y `Exceeded` (Excedido).
  * **Algoritmo de proyección:** Extrapola el gasto actual al cierre del mes para predecir si el usuario excederá su límite.
* **Módulo de Reportes Avanzados:**
  * Desglose porcentual y montos totales por categoría (`GetByCategoryAsync`).
  * Resumen mensual consolidado de ingresos, gastos y promedio diario (`GetSumaryAsync`).
  * Análisis de tendencias cronológicas e históricos de los últimos meses (`GetTrendAsync`).
* **Arquitectura Limpia:** Separación clara de responsabilidades utilizando el patrón de capas (Domain, Application, Infrastructure, WebAPI).

---

## 🛠️ Stack Tecnológico

* **Lenguaje:** C# / .NET
* **Framework Principal:** ASP.NET Core Web API
* **ORM:** Entity Framework Core
* **Base de Datos:** SQL Server
* **Pruebas y Documentación:** Postman
* **Durante el ciclo de desarrollo se utilizaron herramientas de asistencia de Inteligencia Artificial (Gemini, Chatgpt y Cloud) como soporte para la optimización de consultas complejas de EF Core, revisión de buenas prácticas de código limpio y generación de documentación.

---

## ⚙️ Configuración e Instalación

### Prerrequisitos
* [.NET SDK](https://dotnet.microsoft.com/download) instalado.
* Instancia de SQL Server activa.

### Pasos para ejecutar en local

1. **Clonar el repositorio:**
   ```bash
   git clone [https://github.com/tu-usuario/finanzas-api.git](https://github.com/tu-usuario/finanzas-api.git)
   cd finanzas-api

2. **Configurar la cadena de conexión:**
     ```bash
     "ConnectionStrings": {DefaultConnection": "Server=TU_SERVIDOR;Database=FinanzasDb;Trusted_Connection=True;TrustServerCertificate=True;}
3. ** Ejecutar Migraciones:**
   ```bash
   dotnet ef database update --project FinananzasAPI.Infrastructure --startup-project FinananzasAPI.WebApi
4. ** Correr la aplicación: **
   ```bash
   dotnet run --project FinananzasAPI.WebApi

### 🧪 Pruebas con Postman
* **Ejemplos de Endpoints Clave
* **1. Obtener Resumen Mensual
* **Método: GET

* **Ruta: /api/reports/summary

* **Parámetros (Query): userId={guid}&month=6&year=2026

* **Respuesta Exitosa (200 OK):

  ```bash
    {
   "success": true,
   "data": {
     "month": 6,
     "year": 2026,
     "totalIncome": 50000.00,
     "totalExpense": 12450.00,
     "transactionCount": 15,
     "averageExpensePerDay": 415.00
   }}
   
### ✒️ Autor
* **Berlyng Manuel Yena García - Desarrollo Backend y Lógica de Negocio
   
