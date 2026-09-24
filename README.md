# 🚗 Accidentes de Madrid

Proyecto realizado en C# para analizar los accidentes de tráfico de Madrid de los años **2024, 2025 y 2026**.

El objetivo de la práctica es leer los tres ficheros CSV, combinar sus datos y realizar **30 consultas con LINQ/PLINQ** y las mismas **30 consultas utilizando DataFrame**, comparando finalmente sus tiempos de ejecución.

---

## 🛠️ Tecnologías utilizadas

- C#
- .NET 10
- LINQ
- CsvHelper
- Microsoft.Data.Analysis
- Microsoft.Extensions.DependencyInjection
- Scrutor
- Docker
- Docker Compose

---

## 📂 Lectura de los datos

Los datos se encuentran divididos en tres ficheros CSV dentro de la carpeta data.
He decidido leer los tres ficheros de forma **asíncrona y concurrente**.

Para ello se crea una tarea de lectura para cada fichero y después se espera a que todas terminen utilizando:

```csharp
Task.WhenAll(...)
```

He elegido este enfoque porque la lectura de archivos es una operación de entrada/salida y así no es necesario esperar a que termine completamente un fichero para empezar con el siguiente.

Una vez terminadas las lecturas, los datos de los tres CSV se combinan en una única colección de accidentes.

---

## 📦 Repository

La lectura de los CSV se realiza desde un **Repository**.

He decidido hacerlo así para separar la parte encargada de obtener los datos del resto de la aplicación.

El flujo es aproximadamente:

```text
CSV
 ↓
Repository
 ↓
List<Accidente>
 ↓
Service
```

Esto también permite que `Program.cs` quede más limpio.

---

## 🗺️ Mapper

Los datos de estos CSV en concreto no vienen preparados para utilizarlos directamente.

Por ejemplo, hay que transformar:

- Fechas y horas.
- Número de la calle.
- Sexo.
- Tipo de accidente.
- Código de lesividad.
- Positivo en alcohol.
- Positivo en drogas.

Por este motivo he creado un **Mapper** encargado de transformar los datos del CSV al modelo `Accidente`, teniendo uno especifico para el repositorio y otro para utilizarlo con las consultas en DataFrame.

De esta forma, cuando los datos llegan al resto de la aplicación ya están correctamente tipados.

---

## 🔎 Consultas LINQ

Las **30 consultas LINQ** se realizan desde el `Service`.

He decidido colocarlas ahí para evitar tener toda la lógica dentro de `Program.cs`.

De esta forma `Program` se encarga principalmente de iniciar la aplicación, obtener las dependencias, cargar los datos y llamar al servicio.

```text
Program
   ↓
Service
   ↓
Consultas LINQ
```

---

## 📊 DataFrame

Para la segunda parte de la práctica se utiliza:

```text
Microsoft.Data.Analysis
```

Los CSV se cargan utilizando:

```csharp
DataFrame.LoadCsv(...)
```

Después se combinan los tres DataFrame para poder realizar las mismas consultas que se hacen con LINQ.

---

## 🗺️ DataFrameMapper

He creado un **Mapper específico para DataFrame**.

He tomado esta decisión porque algunos datos necesitan ser transformados antes de poder realizar correctamente las consultas.

El funcionamiento es:

```text
CSV
 ↓
DataFrame.LoadCsv
 ↓
DataFrameMapper
 ↓
DataFrame preparado
 ↓
Consultas
```

Me parecía una forma sencilla de separar la carga de los datos de su transformación.

---

## 💉 Inyección de dependencias

Para la inyección de dependencias he utilizado:

```text
Microsoft.Extensions.DependencyInjection
```

junto con:

```text
Scrutor
```

He decidido utilizar **Scrutor** porque quería probar esta librería y porque permite registrar las dependencias automáticamente mediante el escaneo de las clases.

Tanto el `Repository` como el `Service` se registran utilizando un ciclo de vida **Scoped**. He utilizado un ciclo de vida Scoped porque quiero que el Repository y el Service mantengan la misma instancia durante una ejecución o ámbito de trabajo concreto, sin llegar a compartirla durante toda la vida de la aplicación.

De esta forma no es necesario registrar manualmente cada implementación dentro del programa.

---


## ⏱️ Comparativa de tiempos

La aplicación utiliza `Stopwatch` para medir el tiempo empleado por cada enfoque.

Al finalizar se muestra una comparativa similar a:

```text
LINQ       → 12000 ms
DataFrame  → 6000 ms
```

Esto permite comprobar cuál de las dos formas de realizar las consultas ha sido más rápida durante la ejecución.

---

## 🐳 Docker

El proyecto también incluye:

```text
Dockerfile
docker-compose.yml
.dockerignore
```

Para construir la imagen:

```bash
docker compose build
```

Para ejecutar la aplicación:

```bash
docker compose up
```

Al ser una aplicación de consola, el contenedor ejecuta el programa, muestra los resultados y termina automáticamente.

---

## 💭 Conclusión

Con esta práctica he podido trabajar con una cantidad grande de datos reales y probar dos formas diferentes de consultarlos.

Por un lado, **LINQ** permite trabajar de una forma muy cómoda con colecciones de objetos de C#.

Por otro lado, **DataFrame** permite trabajar con los datos de una forma más orientada a filas y columnas.

También he podido practicar la lectura asíncrona de ficheros, el uso de mappers, repositorios, servicios, inyección de dependencias con Scrutor y Docker.