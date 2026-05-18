# CampusNet – Red Social Académica Dirigida

**Programación III** – Actividad Colaborativa II  
Simulación de una red social dirigida de usuarios académicos usando **Grafos Dirigidos** con arquitectura **MVC en C#**.

---

## Integrantes del equipo

SANTIAGO TORO AMARIRES
Modelo: clases `Vertex`, `Edge`, `Graph` – lógica de BFS/DFS/CRUD, Vista: clase `GraphView` – toda la impresión en consola. Controlador: clase `GraphController` – casos de uso y orquestación |

>
---

## Estructura del proyecto

```
CampusNet/
├── Model/
│   ├── Vertex.cs       # Usuario: Id, Name, Role (Estudiante/Profesor/Egresado)
│   ├── Edge.cs         # Arista dirigida: FromId → ToId
│   └── Graph.cs        # Grafo: lista de adyacencia + BFS, DFS, CRUD, métricas
├── View/
│   └── GraphView.cs    # Toda la salida por consola
├── Controller/
│   └── GraphController.cs  # 4 casos de uso, sin lógica en Main ni en View
├── Program.cs          # Solo instancia y llama al Controller
└── CampusNet.csproj
```

### Capas MVC

|---|---|---|
| **Model** | `Vertex`, `Edge`, `Graph` | Datos, algoritmos, sin Console |
| **View** | `GraphView` | Únicamente `Console.WriteLine` |
| **Controller** | `GraphController` | Orquesta M y V, sin lógica de negocio en `Main` |

---

## Requisitos

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6) o superior

---

## Instrucciones para ejecutar

```bash
# 1. Clonar el repositorio
git clone <URL_DEL_REPOSITORIO>
cd CampusNet

# 2. Restaurar dependencias (solo la primera vez)
dotnet restore

# 3. Compilar
dotnet build

# 4. Ejecutar
dotnet run
```


---

## Casos de uso implementados

### 1. Construcción del grafo
- 12 usuarios con roles variados (Estudiante, Profesor, Egresado)
- 18 relaciones de seguimiento dirigidas
- U01 grado de salida = 5 ≥ 4 ✓
- U02 grado de salida = 4 ≥ 4 ✓
- Ciclo dirigido: **U04 → U07 → U10 → U04** ✓
- U11 y U12 con grado de entrada 0 ✓
- Impresión de lista de adyacencia + tabla de grados

### 2. Recorridos
- **BFS** desde U01, U04, U08 — orden de visita y vértices alcanzados
- **DFS completo** — orden de descubrimiento e identificación de ciclos (back edges)

### 3. Consultas sociales
- Usuarios sin seguidores (in-degree = 0)
- Usuario más influyente (mayor in-degree)
- Usuario más activo (mayor out-degree)
- Verificación de alcanzabilidad con BFS

### 4. Operaciones CRUD
- Agregar / eliminar usuario (con control de duplicados)
- Agregar / eliminar arista dirigida (con control de duplicados)
- Actualizar nombre y rol de un usuario
- Impresión del grafo tras cada operación

---

## Evidencia de ejecución

```
════════════════════════════════════════════════════════════
  LISTA DE ADYACENCIA – CampusNet
════════════════════════════════════════════════════════════
  [U01] Ana Torres (Estudiante)  →  Carlos Ruiz(U02), Diana Lopez(U03), Esteban Mora(U04), Fernanda Gil(U05), Gabriel Soto(U06)
  [U02] Carlos Ruiz (Profesor)  →  Diana Lopez(U03), Helena Vargas(U07), Ivan Castro(U08), Julia Rios(U09)
  ...
  [U11] Laura Mendez (Estudiante)  →  (ninguno)
  [U12] Miguel Nunez (Profesor)  →  (ninguno)

  DFS COMPLETO
  Orden de descubrimiento: Ana Torres(U01) → Carlos Ruiz(U02) → ...
  ⚠  CICLOS DETECTADOS (aristas de retroceso):
       Ivan Castro(U08)  →→  Diana Lopez(U03)  (back edge)
       Esteban Mora(U04)  →→  Helena Vargas(U07)  (back edge)
       ...

  Usuarios más influyentes (grado de entrada = 3):  Diana Lopez (U03)
  Usuarios más activos    (grado de salida  = 5):  Ana Torres (U01)
```

---

## Checklist de entrega

- [x] Grafo dirigido con 12 vértices y 18 aristas
- [x] Impresión de lista de adyacencia
- [x] 3 BFS y 1 DFS completo
- [x] Ciclo dirigido detectable
- [x] Consultas: influyentes, activos, sin seguidores, alcanzabilidad
- [x] 3+ operaciones CRUD (agregar, eliminar, actualizar) con verificación de duplicados
- [x] Arquitectura MVC estricta (sin Console en Model, sin lógica en Main)
- [x] Repositorio público, compila y ejecuta sin cambios adicionales
