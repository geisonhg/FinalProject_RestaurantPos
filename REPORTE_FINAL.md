# Restaurant Performance Dashboard

**Herrera, Geison**
**Número de Estudiante: [INSERTAR NÚMERO]**
**Fecha: 25/03/2026**
**Supervisor: [INSERTAR NOMBRE DEL SUPERVISOR]**

---

*Reporte presentado como cumplimiento parcial de los requisitos para el BSc en Computing en Dorset College.*

---

## Declaración

La incorporación de material sin reconocimiento formal y adecuado (incluso sin intención deliberada de hacer trampa) puede constituir plagio.

Si has recibido ayuda significativa con una solución de uno o más colegas, debes documentar esto en tu trabajo presentado y si tienes alguna duda sobre el nivel de discusión/colaboración aceptable, debes consultar con tu profesor.

**DECLARACIÓN:**
Soy consciente de la política de Dorset College sobre el plagio y certifico que esta tesis es mi propio trabajo.

Estudiante: _______________________________________________

Firmado: _______________________________________________

---

## Resumen (Abstract)

El presente proyecto consiste en el desarrollo de un sistema integrado de Punto de Venta (POS) y Panel de Rendimiento para restaurantes, denominado *Restaurant Performance Dashboard*. El sistema fue concebido para resolver un problema real que enfrentan los gerentes de restaurantes: la necesidad de recopilar manualmente datos operacionales provenientes de múltiples fuentes desconectadas (sistemas de pago, hojas de cálculo, registros físicos) para comprender el rendimiento del negocio.

El objetivo principal fue crear una plataforma centralizada que permitiera gestionar órdenes en tiempo real, registrar ventas con desglose por método de pago, administrar el personal con control de turnos y propinas, registrar y aprobar gastos, y generar reportes automáticos en PDF. La aplicación fue construida sobre .NET 8 con Blazor Server como tecnología de interfaz, PostgreSQL como base de datos relacional y una arquitectura limpia (Clean Architecture) con el patrón CQRS.

El desarrollo se llevó a cabo en siete fases iterativas: desde la configuración inicial de la arquitectura, pasando por el desarrollo del dominio y los casos de uso, la implementación de la interfaz Blazor, la generación de reportes con QuestPDF, la integración de actualizaciones en tiempo real mediante SignalR, la gestión de gastos y personal, hasta la finalización del módulo POS. Se realizaron pruebas unitarias automatizadas sobre las capas de Dominio y Aplicación utilizando xUnit, Moq y FluentAssertions, y se configuró un pipeline de CI/CD en GitHub Actions para automatizar la verificación de formato, compilación, ejecución de pruebas con cobertura de código, análisis de seguridad de paquetes NuGet y publicación del artefacto.

Los resultados de las pruebas confirman la solidez de la lógica de negocio. Como trabajo futuro, el sistema podría extenderse con integración a pasarelas de pago reales, una aplicación móvil para el personal de sala, y módulos analíticos avanzados con inteligencia artificial.

---

## Agradecimientos

Me gustaría agradecer a mi supervisor, [INSERTAR NOMBRE DEL SUPERVISOR], por sus valiosas sugerencias. Su apoyo y orientación fueron de gran ayuda, ya que me brindaron información invaluable sobre la mejor manera de desarrollar este proyecto.

También me gustaría agradecer a mi compañero de proyecto Daniel Vega, cuya colaboración fue fundamental en cada fase del desarrollo. Su dedicación y habilidades técnicas contribuyeron enormemente al resultado final del sistema.

Asimismo, agradezco a mis compañeros de clase y amigos por toda la ayuda y el apoyo durante el desarrollo de este proyecto y a lo largo de los últimos tres años en Dorset College Dublin.

---

## Tabla de Contenidos

1. Introducción
2. Requisitos
   - 2.1 Requisitos
   - 2.2 Recopilación de Requisitos
   - 2.3 Modelado de Requisitos
   - 2.4 Factibilidad
   - 2.5 Conclusión
3. Diseño
   - 3.1 Introducción
   - 3.2 Diseño del Programa
   - 3.3 Diseño de la Interfaz de Usuario
   - 3.4 Conclusión
4. Implementación
   - 4.1 Introducción
   - 4.2 Implementación
   - 4.3 Metodología Scrum
   - 4.4 Entorno de Desarrollo
   - 4.5 Conclusión
5. Pruebas
   - 5.1 Introducción
   - 5.2 Pruebas Funcionales
   - 5.3 Pruebas de Usuario
   - 5.4 Conclusión
6. Gestión del Proyecto
   - 6.1 Introducción
   - 6.2 Herramientas de Gestión
   - 6.3 Reflexión
   - 6.4 Conclusión
7. Conclusión
   - 7.1 Introducción
   - 7.2 Resumen del Proyecto
   - 7.3 Fortalezas y Debilidades
   - 7.4 Resultados de Aprendizaje
   - 7.5 Desarrollo Futuro
8. Referencias
9. Apéndice

---

## 1. Introducción

El sector de la restauración es uno de los entornos operativos más exigentes desde el punto de vista de la gestión de datos. Los gerentes de restaurantes enfrentan cotidianamente el desafío de tomar decisiones en tiempo real sobre personal, inventario, ventas y gastos, a menudo sin contar con herramientas integradas que consoliden toda esta información en un único punto de acceso. El *Restaurant Performance Dashboard* surge como respuesta directa a esta necesidad: una plataforma de software full-stack que combina un sistema de Punto de Venta (POS) operativo con un panel de analítica de negocio.

El sistema fue desarrollado como proyecto final del BSc en Computing en Dorset College Dublin por Geison Herrera y Daniel Vega durante el curso académico 2025/2026. La aplicación apunta al mercado de pequeñas y medianas empresas del sector hostelero en Irlanda, con capacidades diseñadas para sustituir el uso fragmentado de herramientas como hojas de cálculo, sistemas de pago independientes y registros en papel.

Las tecnologías empleadas en el desarrollo incluyen **.NET 8** con **C#** como plataforma backend, **Blazor Server** para la interfaz web interactiva en tiempo real, **PostgreSQL 15** como sistema de gestión de base de datos relacional, **Entity Framework Core 8** como ORM con migraciones código-primero, **MediatR 14** para la implementación del patrón CQRS, **QuestPDF** para la generación de reportes en PDF, **SignalR** para actualizaciones en tiempo real, y **ASP.NET Identity** para autenticación y control de acceso basado en roles.

El proyecto fue gestionado de forma iterativa siguiendo la metodología Scrum, dividiendo el trabajo en siete fases de desarrollo claramente definidas. El control de versiones se realizó con Git a través de GitHub, incluyendo un pipeline de Integración Continua/Despliegue Continuo (CI/CD) con GitHub Actions que automatiza la verificación de formato de código, compilación, ejecución de pruebas unitarias con reporte de cobertura, análisis de vulnerabilidades en dependencias y publicación del artefacto compilado.

En los capítulos siguientes se documenta en detalle el proceso completo: en el **Capítulo 2** se presentan los requisitos funcionales y no funcionales; el **Capítulo 3** describe las decisiones de diseño arquitectónico y de interfaz; el **Capítulo 4** detalla la implementación técnica y el entorno de desarrollo; el **Capítulo 5** presenta las pruebas funcionales y de usuario; el **Capítulo 6** aborda la gestión del proyecto y las herramientas utilizadas; y el **Capítulo 7** ofrece las conclusiones, fortalezas, debilidades y perspectivas de desarrollo futuro.

---

## 2. Requisitos

### 2.1 Requisitos

Este capítulo define todo aquello que la aplicación debe ser capaz de hacer, partiendo desde la perspectiva de los usuarios finales del sistema: gerentes de restaurante, personal de sala y administradores. El área del proyecto corresponde al sector de software de gestión hostelera (Hospitality Management Software), un mercado en constante crecimiento donde la digitalización de procesos operativos es cada vez más necesaria para la competitividad del negocio.

La problemática central identificada fue la dispersión de datos operacionales en múltiples herramientas sin integración: las ventas en un sistema de pago, el control de personal en papel o en hojas de cálculo, los gastos sin un flujo de aprobación definido y los reportes elaborados manualmente de forma semanal. El *Restaurant Performance Dashboard* busca resolver esta fragmentación mediante una única plataforma web de acceso por roles.

### 2.2 Recopilación de Requisitos

#### 2.2.1 Aplicaciones Similares

**1. Toast POS**

*Descripción:* Toast es una plataforma POS basada en la nube, diseñada específicamente para el sector de restaurantes. Ofrece gestión de órdenes, pagos integrados, reportes de ventas, gestión de menú y herramientas de gestión del personal desde dispositivos Android.

*Ventajas:*
- Integración completa con hardware dedicado (terminales, impresoras de cocina, lectores de tarjeta)
- Funcionalidades avanzadas de fidelización de clientes y programas de lealtad
- Reportes analíticos en tiempo real con dashboards detallados
- Soporte para múltiples ubicaciones

*Desventajas:*
- Coste elevado de hardware y suscripción mensual (desde $69/mes por terminal)
- Dependencia de hardware propietario de Toast
- Curva de aprendizaje pronunciada para configuración inicial
- Limitaciones de personalización para necesidades específicas de cada negocio

*Captura de pantalla:* [Ver Apéndice A – Figura 1]

---

**2. Square for Restaurants**

*Descripción:* Square ofrece una solución POS para restaurantes que incluye gestión de mesas, control de órdenes, reportes de ventas por turno, gestión de menú y funcionalidades de entrega a domicilio. Está disponible en iPad y como solución web.

*Ventajas:*
- Sin costos de instalación; plan básico gratuito disponible
- Integración nativa con procesamiento de pagos Square
- Interfaz intuitiva con configuración rápida
- Reportes exportables en múltiples formatos

*Desventajas:*
- Funcionalidades avanzadas (gestión de múltiples ubicaciones, analítica avanzada) requieren plan Plus/Premium
- Soporte limitado para customizaciones de flujo de trabajo
- Dependencia de conectividad a internet para la mayoría de las funciones
- Menor profundidad en análisis de rendimiento del personal

*Captura de pantalla:* [Ver Apéndice A – Figura 2]

---

**3. Lightspeed Restaurant**

*Descripción:* Lightspeed es una plataforma POS basada en la nube para restaurantes que ofrece gestión de órdenes, inventario integrado, reportes financieros, gestión de personal y soporte multi-ubicación. Ampliamente adoptado en Europa e Irlanda.

*Ventajas:*
- Gestión de inventario en tiempo real con alertas de stock
- Reportes financieros detallados con comparativas de períodos
- Integraciones con plataformas de delivery (Deliveroo, Uber Eats)
- Soporte 24/7 en varios idiomas

*Desventajas:*
- Precio elevado (desde $69/mes); puede ser inaccesible para negocios pequeños
- Interfaz compleja que requiere formación extensa del personal
- Algunas funcionalidades relevantes (contabilidad avanzada) requieren integraciones de terceros adicionales
- El módulo de nómina no está disponible en todos los países

*Captura de pantalla:* [Ver Apéndice A – Figura 3]

---

#### 2.2.2 Entrevistas

Para definir los requisitos funcionales del sistema, se realizaron entrevistas a tres perfiles de usuarios potenciales representativos del mercado objetivo.

**Entrevista 1 — Gerente de Restaurante**

El gerente indicó que actualmente dedica entre 2 y 3 horas semanales a compilar reportes de ventas de forma manual desde múltiples fuentes. Su principal necesidad es contar con un reporte semanal generado automáticamente que consolide ventas, gastos y rendimiento del personal. Destacó la importancia de poder ver en tiempo real qué mesas están ocupadas y cuánto llevan gastado. Mencionó que el control de propinas del personal es un punto crítico para evitar conflictos internos.

**Entrevista 2 — Personal de Sala (Camarero)**

El camarero señaló que la herramienta más importante para él es poder abrir y cerrar órdenes de forma rápida desde cualquier dispositivo, sin necesidad de formación técnica extensa. Valoró positivamente la posibilidad de ver su propio historial de propinas y turnos. Indicó que el registro de entrada y salida de turno debería ser simple y sin pasos innecesarios.

**Entrevista 3 — Propietario del Negocio**

El propietario enfatizó la importancia del control de gastos con un flujo de aprobación para evitar gastos no autorizados. Expresó que necesita reportes mensuales en PDF para presentar a su contable. Mencionó que la seguridad del sistema es fundamental: no todos los empleados deberían poder ver los reportes financieros completos ni acceder a la configuración del sistema.

**Temas emergentes de las entrevistas:**
- **Características fundamentales:** Gestión de órdenes, ventas con métodos de pago, reportes automáticos en PDF
- **Características deseadas:** Control de propinas, historial de turnos, importación de gastos desde CSV
- **Requisitos futuros:** Integración con pasarelas de pago, aplicación móvil para el personal

#### 2.2.3 Encuesta

Se diseñó un formulario en Google Forms distribuido entre potenciales usuarios del sector hostelero. Las respuestas fueron analizadas para validar los requisitos identificados en las entrevistas. Los resultados confirmaron que la generación automática de reportes, el control de acceso por roles y el seguimiento de propinas eran las funcionalidades con mayor demanda. [Ver Apéndice B – Resultados de Encuesta]

### 2.3 Modelado de Requisitos

#### 2.3.1 Personas

**Persona 1 — Ana García, Gerente de Restaurante**
- Edad: 38 años | Género: Femenino | Ocupación: Gerente General
- Objetivos: Tener visibilidad completa del rendimiento del negocio sin invertir horas en consolidar datos manualmente
- Escenario: Cada lunes por la mañana revisa el reporte semanal generado automáticamente, compara los ingresos netos con los gastos aprobados e identifica qué días y métodos de pago generan mayor volumen
- Frustraciones actuales: Pasa horas cada semana exportando datos de distintos sistemas y construyendo reportes en Excel

**Persona 2 — Carlos Martínez, Camarero Senior**
- Edad: 26 años | Género: Masculino | Ocupación: Camarero con 4 años de experiencia
- Objetivos: Registrar órdenes rápidamente, ver su historial de propinas y realizar el fichaje de turno sin complicaciones
- Escenario: Al comenzar su turno fichea en el sistema, durante el servicio abre órdenes por mesa, añade items del menú y las cierra con el método de pago elegido por el cliente
- Frustraciones actuales: El sistema actual obliga a imprimir tickets físicos y calcular propinas manualmente al final del turno

**Persona 3 — Tomás Walsh, Propietario del Negocio**
- Edad: 51 años | Género: Masculino | Ocupación: Propietario/Administrador
- Objetivos: Controlar los gastos del restaurante, aprobar únicamente los que están justificados y recibir reportes mensuales para presentar a su gestor
- Escenario: Recibe notificaciones de nuevos gastos pendientes de aprobación, los revisa con su descripción y URL de recibo adjunto, y los aprueba o rechaza desde el panel de administración
- Frustraciones actuales: Los gastos se registran en papel y a menudo se presentan sin justificante, dificultando la contabilidad mensual

#### 2.3.2 Requisitos Funcionales

1. El sistema debe permitir abrir una orden de mesa asignada a un empleado específico
2. El sistema debe permitir añadir y eliminar ítems del menú a una orden abierta
3. El sistema debe calcular automáticamente el subtotal, IVA (23%) y total de cada orden
4. El sistema debe permitir cerrar una orden indicando el método de pago y la propina recibida
5. El sistema debe permitir anular órdenes abiertas con una razón obligatoria
6. El sistema debe aplicar descuentos a nivel de orden con validación de rango (0-100%)
7. El sistema debe registrar ventas con desglose por método de pago (Efectivo, Tarjeta, Sin Contacto, Tarjeta Regalo)
8. El sistema debe permitir registrar empleados con sus datos básicos y rol asignado
9. El sistema debe permitir que los empleados registren entrada y salida de turno (fichaje)
10. El sistema debe calcular automáticamente la duración del turno, incluyendo turnos nocturnos
11. El sistema debe registrar las propinas ganadas por empleado por turno
12. El sistema debe permitir registrar gastos con categoría, descripción y URL de recibo opcional
13. El sistema debe implementar un flujo de aprobación de gastos (pendiente → aprobado/rechazado)
14. El sistema debe generar reportes PDF semanales y mensuales de forma automática y bajo demanda
15. El sistema debe permitir la importación masiva de gastos desde archivos CSV
16. El sistema debe mostrar un panel de control con métricas clave en tiempo real
17. El sistema debe actualizar el dashboard en tiempo real mediante SignalR
18. El sistema debe controlar el acceso mediante roles: Admin, Manager, Staff
19. El sistema debe generar tickets de recibo de orden en PDF en formato A5
20. El sistema debe generar reportes de nómina en PDF por período seleccionado

#### 2.3.3 Requisitos No Funcionales

**Usabilidad:**
- La interfaz debe ser navegable sin formación técnica extensa para el rol de Staff
- Las operaciones más frecuentes (abrir orden, añadir ítem, cerrar orden) deben completarse en menos de 5 pasos
- El sistema debe proporcionar mensajes de error descriptivos en caso de operaciones inválidas

**Rendimiento:**
- El panel de control debe cargar en menos de 3 segundos en condiciones normales de uso
- Las actualizaciones en tiempo real vía SignalR no deben introducir latencia perceptible (< 500ms)
- El pipeline de CI/CD debe completarse en menos de 10 minutos para no bloquear el flujo de trabajo

**Seguridad:**
- Las contraseñas deben tener un mínimo de 10 caracteres, incluyendo mayúsculas y caracteres especiales
- Las cuentas deben bloquearse tras 5 intentos fallidos de inicio de sesión (bloqueo de 15 minutos)
- Los endpoints y operaciones sensibles deben protegerse mediante autorización basada en roles
- Las contraseñas deben almacenarse con hash mediante ASP.NET Identity (no en texto plano)

**Portabilidad:**
- La aplicación debe ejecutarse en cualquier sistema operativo con soporte para .NET 8 (Windows, Linux, macOS)
- El pipeline CI/CD debe ejecutarse en runners de GitHub (Ubuntu Latest)

**Mantenibilidad:**
- El código debe seguir los estándares de formato definidos en `.editorconfig` y verificados por `dotnet format`
- La arquitectura debe facilitar la adición de nuevos módulos sin modificar capas existentes

#### 2.3.4 Diagramas de Casos de Uso

**Actores del sistema:**
- **Admin:** acceso completo a todas las funcionalidades del sistema
- **Manager:** acceso a reportes, aprobación de gastos, gestión de personal y operaciones POS
- **Staff:** acceso a operaciones POS, gestión de menú, registro de turnos y gastos

**Casos de Uso Principales:**

| Caso de Uso | Actor(es) | Descripción |
|---|---|---|
| Abrir Orden | Staff, Manager, Admin | Crear nueva orden para una mesa específica |
| Gestionar Ítems de Orden | Staff, Manager, Admin | Añadir/eliminar ítems del menú en una orden abierta |
| Cerrar Orden | Staff, Manager, Admin | Finalizar orden con método de pago y propina |
| Anular Orden | Staff, Manager, Admin | Cancelar orden abierta con razón justificada |
| Gestionar Menú | Manager, Admin | Crear, actualizar y eliminar ítems del menú |
| Gestionar Empleados | Manager, Admin | Registrar y desactivar empleados |
| Fichar Entrada/Salida | Staff, Manager, Admin | Registrar inicio y fin de turno de trabajo |
| Registrar Gasto | Staff, Manager, Admin | Crear nuevo gasto con justificante |
| Aprobar/Rechazar Gasto | Manager, Admin | Revisar y aprobar o revocar gastos pendientes |
| Generar Reporte | Manager, Admin | Generar reportes semanales/mensuales en PDF |
| Ver Dashboard | Manager, Admin | Consultar métricas de rendimiento del negocio |
| Importar Gastos CSV | Manager, Admin | Importar gastos masivamente desde archivo CSV |

[Ver Apéndice A – Diagrama de Casos de Uso]

### 2.4 Factibilidad

Las tecnologías seleccionadas para el desarrollo del sistema son maduras, de código abierto (o con licencias libres para uso académico) y con amplia documentación disponible.

**.NET 8 y Blazor Server** presentan compatibilidad nativa entre sí, eliminando la necesidad de una API REST separada: el servidor Blazor gestiona tanto la lógica de presentación como la comunicación con la base de datos a través del mismo proceso. Esto simplifica el despliegue y reduce la complejidad de la arquitectura para un equipo de desarrollo pequeño.

**PostgreSQL 15** es compatible con el proveedor Npgsql de Entity Framework Core, sin problemas de interoperabilidad. La diferencia entre el entorno de desarrollo (Windows/WSL2) y el entorno de CI (Ubuntu Linux) fue manejada exitosamente mediante el pipeline de GitHub Actions.

**QuestPDF** requirió la configuración de una licencia comunitaria (`QuestPDF.Settings.License = LicenseType.Community`), lo cual se gestionó sin inconvenientes durante la fase de implementación.

El principal riesgo técnico identificado fue la integración entre **SignalR** y **Blazor Server**, dado que ambos utilizan conexiones WebSocket. Sin embargo, ASP.NET Core gestiona esto de forma transparente, y la implementación fue exitosa desde el primer sprint.

No se identificaron incompatibilidades entre los paquetes NuGet utilizados. El análisis de seguridad automatizado en el pipeline de CI confirmó que ninguno de los paquetes utilizados presenta vulnerabilidades conocidas en las versiones seleccionadas.

### 2.5 Conclusión

Este capítulo ha definido el contexto del proyecto: un sistema de gestión POS y panel de rendimiento para restaurantes de pequeño y mediano tamaño. Se investigaron tres aplicaciones similares (Toast POS, Square for Restaurants y Lightspeed Restaurant), identificando sus fortalezas y limitaciones. Se realizaron entrevistas con tres perfiles de usuario representativos que permitieron definir 20 requisitos funcionales y 10 requisitos no funcionales. Se modelaron tres personas que representan los distintos tipos de usuarios del sistema. El análisis de factibilidad confirma que las tecnologías seleccionadas son compatibles entre sí y adecuadas para el alcance del proyecto.

---

## 3. Diseño

### 3.1 Introducción

Este capítulo describe las decisiones de diseño tomadas para traducir los requisitos identificados en el capítulo anterior en una arquitectura técnica sólida y una interfaz de usuario funcional. El diseño del sistema se divide en dos áreas principales: el **diseño del programa** (arquitectura técnica, patrones de diseño, modelo de datos) y el **diseño de la interfaz de usuario** (wireframes, flujo de navegación, guía de estilo).

La arquitectura elegida es **Clean Architecture** (Arquitectura Limpia), que establece una separación estricta de responsabilidades entre capas y garantiza que la lógica de negocio sea independiente de los detalles de implementación como frameworks, bases de datos o interfaces de usuario. Esta decisión arquitectónica fue tomada pensando en la mantenibilidad y extensibilidad del sistema a largo plazo.

### 3.2 Diseño del Programa

#### 3.2.1 Tecnologías

Las tecnologías utilizadas para desarrollar esta aplicación son:

**.NET 8 (C#)**
.NET 8 es la plataforma de desarrollo open source y multiplataforma de Microsoft para crear aplicaciones web, servicios y APIs. C# es el lenguaje principal de .NET, con soporte completo para programación orientada a objetos, tipos genéricos, expresiones lambda y records. Se eligió esta tecnología por su madurez, rendimiento, amplio ecosistema de paquetes NuGet y compatibilidad con todos los sistemas operativos que soportan el equipo de desarrollo.

**Blazor Server**
Blazor Server es el framework de interfaz web interactiva de .NET que permite escribir la UI en C# en lugar de JavaScript. A diferencia de Blazor WebAssembly, Blazor Server ejecuta toda la lógica de componentes en el servidor y mantiene una conexión SignalR para sincronizar los cambios de estado con el navegador en tiempo real. Se eligió frente a React o Angular porque permite utilizar el mismo lenguaje (C#) en todo el stack, eliminando el cambio de contexto entre frontend y backend, y simplificando el modelo de acceso a datos.

**PostgreSQL 15**
PostgreSQL es el sistema de gestión de bases de datos relacional open source más avanzado del mundo. Se eligió frente a SQL Server por su coste cero, excelente soporte en Linux, compatibilidad completa con el proveedor Npgsql para Entity Framework Core, y su adopción generalizada en entornos de producción de todo el mundo.

**Entity Framework Core 8**
EF Core es el ORM (Object-Relational Mapper) oficial de .NET. Permite definir el esquema de base de datos mediante clases C# (code-first) y gestionar las migraciones de esquema de forma automática. Se eligió porque elimina la necesidad de escribir SQL directamente para la mayoría de las operaciones CRUD y facilita el testing mediante el proveedor InMemory.

**MediatR 14**
MediatR es una librería de implementación del patrón Mediator en .NET. Se utilizó para implementar el patrón CQRS (Command Query Responsibility Segregation), separando las operaciones de escritura (Commands) de las de lectura (Queries). Esta decisión de diseño mejora la separación de responsabilidades y facilita la adición de comportamientos transversales (logging, validación, autorización) mediante el pipeline de MediatR.

**QuestPDF**
QuestPDF es una librería open source para generación de documentos PDF en .NET con una API fluida basada en composición. Se eligió frente a alternativas como iTextSharp (licencia propietaria) o FastReport (complejo de configurar) por su API moderna, documentación excelente y licencia comunitaria gratuita.

**FluentValidation**
FluentValidation es una librería para definir reglas de validación de forma declarativa y fluida en C#. Se integra con el pipeline de MediatR a través de `ValidationBehavior`, garantizando que todos los comandos sean validados antes de ejecutarse.

**Otras tecnologías consideradas pero no seleccionadas:**
- **React + ASP.NET Core Web API:** Fue considerado como alternativa, pero implicaba mantener dos stacks de lenguaje diferentes (TypeScript y C#), mayor complejidad de despliegue y la necesidad de diseñar una API REST completa. Blazor Server ofrece el mismo resultado con menor complejidad.
- **SQL Server:** Considerado por ser el sistema de bases de datos más común en el ecosistema .NET, pero descartado por sus costos de licencia en entornos de producción y la preferencia por herramientas open source.
- **Dapper:** Considerado como alternativa a EF Core para casos donde el rendimiento es crítico, pero descartado por la mayor productividad de EF Core para aplicaciones CRUD y su soporte nativo para migraciones.

#### 3.2.2 Estructura del Proyecto

La solución está organizada en **6 proyectos** dentro de una solución .NET (`RestaurantDashboard.sln`):

```
RestaurantDashboard/
├── RestaurantDashboard.Domain/
│   ├── Common/           (Entity, AggregateRoot, DomainEvent, Guard, ValueObjects)
│   ├── Entities/         (Order, Employee, MenuItem, Sale, Expense, Shift, Tip, Report)
│   ├── Enums/            (OrderStatus, EmployeeRole, PaymentMethod, ExpenseCategory, etc.)
│   ├── Events/           (OrderOpenedEvent, OrderClosedEvent, OrderVoidedEvent)
│   ├── Exceptions/       (DomainException)
│   └── Repositories/     (Interfaces: IOrderRepository, IEmployeeRepository, etc.)
│
├── RestaurantDashboard.Application/
│   ├── Common/           (Behaviors, Exceptions, Interfaces, AppRoles)
│   ├── Sales/            (Commands + Queries para órdenes y ventas)
│   ├── Employees/        (Commands + Queries para empleados y turnos)
│   ├── MenuItems/        (Commands + Queries para menú)
│   ├── Expenses/         (Commands + Queries para gastos)
│   └── Reports/          (Commands + Queries para reportes)
│
├── RestaurantDashboard.Infrastructure/
│   ├── Persistence/      (AppDbContext, Configuraciones EF, Repositorios, UnitOfWork, Seeder)
│   ├── Identity/         (ApplicationUser)
│   ├── Reporting/        (PdfReportService con QuestPDF)
│   ├── BackgroundJobs/   (WeeklyReportJob como hosted service)
│   └── DependencyInjection.cs
│
├── RestaurantDashboard.Web/
│   ├── Components/       (Páginas Blazor: Dashboard, Orders, Staff, Expenses, Menu, Reports...)
│   ├── Services/         (BlazorCurrentUserService, DashboardStateService, OrderNotifier)
│   ├── Hubs/             (OrderHub para SignalR)
│   └── Program.cs        (Configuración de la aplicación)
│
├── RestaurantDashboard.Domain.Tests/
└── RestaurantDashboard.Application.Tests/
```

#### 3.2.3 Patrones de Diseño

**Clean Architecture (Arquitectura Limpia)**
La arquitectura limpia organiza el código en capas concéntricas donde las dependencias solo apuntan hacia el interior. La capa más interna (Domain) no depende de ninguna otra; Application depende solo de Domain; Infrastructure y Web dependen de Application. Esto garantiza que la lógica de negocio sea testeable en aislamiento y que los detalles de infraestructura sean intercambiables.

**CQRS (Command Query Responsibility Segregation)**
Todas las operaciones de escritura se modelan como `Commands` y las de lectura como `Queries`, ambos implementando `IRequest<T>` de MediatR. Este patrón mejora la claridad del código: cada handler tiene una única responsabilidad y el sistema es fácil de extender con nuevos casos de uso sin modificar los existentes.

**Repository Pattern**
Los repositorios abstraen el acceso a datos del resto de la aplicación. Las interfaces se definen en la capa Domain, y las implementaciones en Infrastructure. Esto permite cambiar el ORM o la base de datos sin modificar la lógica de negocio.

**Unit of Work Pattern**
La interfaz `IUnitOfWork` encapsula la transacción de base de datos. Los handlers de comandos obtienen repositorios y al final llaman `CommitAsync()` para persistir todos los cambios en una única transacción atómica.

**Aggregate Root Pattern**
Las entidades principales (Order, Employee, MenuItem, Expense, Sale, Report) son AggregateRoots que encapsulan sus entidades secundarias (OrderItems, Shifts, Tips) y controlan el acceso a ellas mediante métodos de dominio. Esto garantiza la consistencia del modelo.

**Value Object Pattern**
`Money` es un value object inmutable que representa cantidades monetarias con su divisa. Implementado como un C# `record sealed` con operadores sobrecargados, garantiza que los valores monetarios sean siempre no negativos y estén redondeados a 2 decimales.

**Domain Events**
Las acciones importantes del dominio (apertura, cierre, anulación de órdenes) generan eventos de dominio (`OrderOpenedEvent`, `OrderClosedEvent`, `OrderVoidedEvent`) que son despachados a través de MediatR para ser procesados por handlers suscritos.

**MediatR Pipeline Behaviors**
Se implementaron tres behaviors en el pipeline de MediatR que se ejecutan automáticamente para cada Command/Query en este orden:
1. `LoggingBehavior` — registra el nombre de la operación y el resultado
2. `AuthorizationBehavior` — valida el rol requerido para operaciones protegidas
3. `ValidationBehavior` — ejecuta todos los validadores FluentValidation registrados

#### 3.2.4 Arquitectura de la Aplicación

```
[Navegador Web]
      |
      | WebSocket (Blazor Server + SignalR)
      |
[RestaurantDashboard.Web]  ←→  [SignalR Hub]
      |
      | IMediator.Send(command/query)
      |
[RestaurantDashboard.Application]
  [Pipeline: Logging → Auth → Validation]
      |
      | IRepository, IUnitOfWork
      |
[RestaurantDashboard.Infrastructure]
  [EF Core DbContext] ←→ [PostgreSQL 15]
  [QuestPDF] → archivos PDF
  [WeeklyReportJob] (Background Service)
      |
[RestaurantDashboard.Domain]
  [Entities, Value Objects, Domain Events]
```

[Ver Apéndice A – Diagrama de Arquitectura]

#### 3.2.5 Diseño de Base de Datos

La base de datos contiene **15 tablas** (9 de dominio + 6 de Identity):

**Tablas de Dominio:**

| Tabla | Descripción | Columnas Clave |
|---|---|---|
| Employees | Personal del restaurante | Id, FirstName, LastName, Role(int), HireDate, IsActive, UserId |
| Orders | Órdenes de mesa | Id, TableNumber, EmployeeId, Status(int), OpenedAt, ClosedAt, DiscountAmount, Notes |
| OrderItems | Líneas de orden | Id, OrderId(FK), MenuItemId, MenuItemName(snapshot), Quantity, UnitPrice, Currency |
| MenuItems | Catálogo del menú | Id, Name, Category, Description, BasePrice, Currency, IsAvailable, StockQuantity |
| Sales | Ventas cerradas | Id, OrderId, Date, Subtotal, TaxAmount, TipAmount, TotalAmount, PaymentMethod(int), ProcessedByEmployeeId |
| Tips | Propinas | Id, SaleId(FK), EmployeeId, Amount, Date |
| Shifts | Turnos de trabajo | Id, EmployeeId(FK), Date, ClockIn, ClockOut, TipsEarned, Status(int) |
| Expenses | Gastos del negocio | Id, Category(int), Amount, Date, Description, ReceiptUrl, RecordedByEmployeeId, IsApproved |
| Reports | Reportes generados | Id, Type(int), PeriodStart, PeriodEnd, FilePath, GeneratedByEmployeeId, GeneratedAt |

**Evolución del esquema (Migraciones):**
- `20260303194402_InitialCreate` — Esquema base completo con todas las tablas e índices
- `20260315203448_AddMenuItemDescription` — Campo Description nullable en MenuItems
- `20260315210242_AddMenuItemStockAndOrderDiscount` — StockQuantity nullable en MenuItems + DiscountAmount en Orders

**Notas de diseño relevantes:**
- `OrderItem.MenuItemName` y `OrderItem.UnitPrice` son **snapshots históricos**: se copian al momento de añadir el ítem para que el historial de órdenes sea inmutable ante cambios de precio o nombre del menú
- `Money` es un **owned entity** de EF Core: se almacena como dos columnas (Amount + Currency) en la misma tabla que la entidad propietaria
- Los enums se almacenan como `int` en la base de datos para eficiencia de almacenamiento e indexación
- El índice único filtrado en `Employees.UserId` (`WHERE UserId IS NOT NULL`) permite múltiples empleados sin cuenta de usuario mientras garantiza unicidad para los que sí la tienen

[Ver Apéndice A – Diagrama ERD]

#### 3.2.6 Diseño de Procesos

**Flujo de una Orden (Diagrama de Secuencia simplificado):**

```
Staff/Blazor UI → CreateOrderCommand → [Logging] → [Auth: Staff+] → [Validation]
→ CreateOrderCommandHandler → IOrderRepository.AddAsync() → IUnitOfWork.CommitAsync()
→ OrderOpenedEvent → [handlers de evento]
→ OrderDto (retorno)
```

**Diagrama de Clases — Entidades principales:**

```
AggregateRoot
├── Order (TableNumber, EmployeeId, Status, Items: List<OrderItem>)
│   └── OrderItem (MenuItemId, MenuItemName, Quantity, UnitPrice: Money)
├── Employee (FirstName, LastName, Role, Shifts: List<Shift>)
│   └── Shift (Date, ClockIn, ClockOut, TipsEarned, Status)
├── MenuItem (Name, Category, Description, BasePrice: Money, StockQuantity)
├── Sale (OrderId, Subtotal: Money, TaxAmount: Money, TotalAmount: Money)
│   └── Tip (EmployeeId, Amount: Money)
├── Expense (Category, Amount: Money, IsApproved)
└── Report (Type, PeriodStart, PeriodEnd, FilePath)
```

**Flujo de Aprobación de Gastos:**
```
Staff registra Gasto → IsApproved = false
Manager revisa → ApproveExpenseCommand → IsApproved = true
              ↘ (o) → Expense permanece pendiente / se elimina
```

### 3.3 Diseño de la Interfaz de Usuario

#### 3.3.1 Wireframes

La interfaz se diseñó con una navegación lateral fija y un área de contenido central. Los wireframes principales definidos durante la fase de diseño fueron:

- **Dashboard:** panel con tarjetas de KPIs (ingresos del día, transacciones, propinas totales) y tabla de órdenes abiertas en tiempo real
- **Órdenes (POS):** lista de órdenes abiertas con botón de nueva orden; formulario de orden con selector de mesa, ítem del menú y cantidad
- **Menú:** tabla de ítems con disponibilidad, precio y stock; modal de creación/edición
- **Personal:** lista de empleados con rol, estado de turno activo; panel de clock in/out
- **Gastos:** tabla de gastos por período con filtros; formulario de registro con categoría y descripción
- **Reportes:** lista de reportes generados con enlace de descarga; botón de generación manual
- **Nómina:** resumen de horas y propinas por empleado para un período seleccionado

[Ver Apéndice A – Wireframes]

#### 3.3.2 Diagrama de Flujo de Usuario

```
Login
  ↓
Dashboard (KPIs en tiempo real)
  ├── Órdenes → Nueva Orden → Añadir Ítems → Cerrar/Anular Orden
  ├── Menú → Ver Ítems → Crear/Editar/Eliminar Ítem
  ├── Personal → Ver Empleados → Clock In/Out → Ver Turnos
  ├── Gastos → Registrar Gasto → Aprobar Gasto (Manager/Admin)
  ├── Ventas → Historial de Ventas → Detalle de Venta
  ├── Reportes → Generar Reporte → Descargar PDF
  └── Nómina → Ver Resumen de Nómina → Descargar PDF
```

#### 3.3.3 Guía de Estilo

La interfaz del sistema sigue las convenciones de diseño de **Bootstrap 5**, que es la biblioteca CSS integrada por defecto en las plantillas de Blazor Server. El esquema de colores fue elegido para comunicar profesionalidad, claridad y confianza en un entorno de trabajo operativo:

- **Color primario:** Azul oscuro (#0d6efd Bootstrap Blue) — usado en botones de acción principal y elementos de navegación activos. El azul transmite confianza y autoridad, apropiado para un sistema de gestión empresarial.
- **Color de éxito:** Verde (#198754) — confirmación de operaciones completadas, estados activos
- **Color de alerta:** Amarillo (#ffc107) — gastos pendientes de aprobación, advertencias
- **Color de peligro:** Rojo (#dc3545) — anulaciones, eliminaciones, estados de error
- **Fondo:** Blanco (#ffffff) y gris muy claro (#f8f9fa) para el área de contenido
- **Tipografía:** Sistema de fuentes nativo de Bootstrap (segoe ui, helvetica, arial) para máxima legibilidad en pantallas de alta densidad

La navegación lateral utiliza fondo oscuro para crear una jerarquía visual clara entre el menú de navegación y el área de contenido. Los iconos de Bootstrap Icons complementan las etiquetas de texto en el menú.

### 3.4 Conclusión

Este capítulo ha documentado las decisiones de diseño más importantes del *Restaurant Performance Dashboard*. Se eligió Clean Architecture con CQRS como arquitectura base, garantizando separación de responsabilidades y extensibilidad. La elección de .NET 8 con Blazor Server permite un stack tecnológico unificado en C# que simplifica el desarrollo y el mantenimiento. El modelo de datos fue diseñado con consideraciones específicas del dominio hostelero (snapshots históricos en órdenes, IVA irlandés del 23%, turnos nocturnos). Los wireframes y la guía de estilo definen una interfaz operacional clara, apropiada para uso en entornos de alta actividad como un restaurante durante el servicio.

---

## 4. Implementación

### 4.1 Introducción

Este capítulo describe la fase de implementación del *Restaurant Performance Dashboard*. Las tecnologías utilizadas para desarrollar la aplicación incluyen las siguientes:

**.NET 8 (C#)**
.NET 8 es la versión LTS (Long-Term Support) más reciente de la plataforma .NET de Microsoft. Proporciona un runtime de alto rendimiento, soporte para programación asíncrona con async/await, tipos de referencia nullables para mayor seguridad de tipos, y un ecosistema maduro de paquetes NuGet. La versión del SDK utilizada está fijada en `8.0.124` mediante el archivo `global.json`.

**Blazor Server**
Blazor Server permite construir interfaces web interactivas en C# sin necesidad de JavaScript. Los componentes Blazor (archivos `.razor`) combinan markup HTML con lógica C# en el mismo archivo, y el estado de la UI se mantiene en el servidor con actualizaciones enviadas al navegador vía WebSocket.

**Entity Framework Core 8**
EF Core gestiona la persistencia de datos mediante un modelo code-first: las clases de entidad C# definen el esquema de la base de datos, y las migraciones EF Core aplican los cambios de esquema de forma incremental. Se utilizó Fluent API para todas las configuraciones de entidad en lugar de Data Annotations, separando la configuración de persistencia de las clases de dominio.

**PostgreSQL**
PostgreSQL es el sistema de base de datos relacional utilizado tanto en desarrollo (base de datos `restaurant_dashboard_dev`) como en producción (`restaurant_dashboard`). El proveedor Npgsql gestiona la traducción de LINQ to SQL específico de PostgreSQL.

La aplicación implementa el sistema de gestión completo descrito en los capítulos anteriores, con 172 archivos de código fuente C#, 31 handlers CQRS, 9 repositorios, 10 páginas Blazor y un pipeline CI/CD completo.

### 4.2 Implementación

El desarrollo se estructuró en **7 fases** claramente delimitadas:

**Fase 1 — Fundación (Foundation)**
Configuración de la solución multi-proyecto, definición de la arquitectura de 4 capas, implementación de las clases base (`Entity`, `AggregateRoot`, `DomainEvent`, `Money`), definición de las interfaces de repositorio y configuración inicial de EF Core con la primera migración (`InitialCreate`) que establece el esquema completo de la base de datos.

**Fase 2 — Dominio y Casos de Uso Core**
Implementación completa de los 8 AggregateRoots con toda su lógica de negocio, implementación de los 31 handlers CQRS (23 para Sales/Orders, 11 para Employees, 7 para MenuItems, 5 para Expenses, 4 para Reports), implementación de los 9 repositorios en Infrastructure con EF Core, y desarrollo de las pruebas unitarias para las capas Domain y Application.

**Fase 3 — Frontend Blazor**
Implementación de las 10 páginas Blazor (Dashboard, Orders, Staff, Expenses, Menu, Reports, SalesHistory, Payroll, Home) con sus componentes de formulario, tablas y tarjetas de KPI. Configuración de ASP.NET Identity con las páginas de Account (login, registro, 2FA, reset de contraseña). Implementación del seeder de datos inicial con 3 usuarios de desarrollo.

**Fase 4 — Reportes e Importación CSV**
Implementación de `PdfReportService` usando QuestPDF para generación de reportes de negocio en A4 y recibos de orden en A5. Implementación del `WeeklyReportJob` como `IHostedService` para generación automática semanal. Implementación de `ImportExpensesFromCsvCommand` con CsvHelper para importación masiva de gastos.

**Fase 5 — Tiempo Real y CI/CD**
Implementación de `OrderHub` (SignalR) y `OrderNotifier` para actualizaciones en tiempo real del dashboard. Implementación de `DashboardStateService` para gestión de estado compartido entre componentes Blazor. Configuración completa del pipeline de GitHub Actions con los 4 jobs (Format Check, Build & Test con cobertura, Security Scan, Publish).

**Fase 6 — Gastos y Personal**
Refinamiento del módulo de gestión de gastos con flujo de aprobación completo. Implementación completa del módulo de personal con clock in/out, cálculo de duración de turno (incluyendo turnos nocturnos que cruzan la medianoche), y resumen de propinas por turno. Generación de reportes de nómina en PDF.

**Fase 7 — POS Completo**
Finalización del módulo POS con creación de órdenes, adición/eliminación de ítems, selección de método de pago, aplicación de descuentos y generación de recibos. Implementación de la segunda y tercera migración de base de datos para añadir descripción de menú e inventario de stock.

#### Fragmentos de Código Representativos

**Aggregate Root con Domain Events:**
```csharp
public sealed class Order : AggregateRoot
{
    public static Order Open(int tableNumber, Guid employeeId)
    {
        var order = new Order { TableNumber = tableNumber, EmployeeId = employeeId,
            Status = OrderStatus.Open, OpenedAt = DateTime.UtcNow };
        order.RaiseDomainEvent(new OrderOpenedEvent(order.Id, tableNumber, employeeId));
        return order;
    }

    public void Close(PaymentMethod paymentMethod, decimal tipAmount, decimal discountAmount)
    {
        Guard.AgainstOrderClosed(Status);
        Guard.AgainstNegative(discountAmount, nameof(discountAmount));
        DiscountAmount = discountAmount;
        Status = OrderStatus.Closed;
        ClosedAt = DateTime.UtcNow;
        RaiseDomainEvent(new OrderClosedEvent(Id, Guid.NewGuid(), Subtotal.Amount));
    }
}
```

**MediatR Pipeline Behavior (Validación):**
```csharp
public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var failures = _validators
            .Select(v => v.Validate(new ValidationContext<TRequest>(request)))
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
            throw new ValidationException(failures);

        return await next();
    }
}
```

### 4.3 Metodología Scrum

El proyecto fue gestionado siguiendo la metodología **Scrum**, un framework ágil de desarrollo de software iterativo e incremental. Scrum organiza el trabajo en **Sprints** (iteraciones de duración fija) con un conjunto de roles, ceremonias y artefactos bien definidos.

En Scrum, el **Product Backlog** es una lista priorizada de todas las funcionalidades y tareas necesarias para el producto. Al inicio de cada Sprint, el equipo selecciona un subconjunto de tareas del backlog (Sprint Backlog) para completar en la iteración. Al final de cada Sprint se realiza una **Sprint Review** para demostrar el incremento funcional del producto, y una **Sprint Retrospective** para identificar mejoras en el proceso de trabajo.

Para este proyecto, cada una de las 7 fases de desarrollo correspondió a un Sprint de aproximadamente 2 semanas. El tablero de Trello fue utilizado como herramienta visual del backlog y el progreso de cada Sprint, con columnas de *To Do*, *In Progress* y *Done*. Las ceremonias de retrospectiva permitieron identificar mejoras como la adopción del pipeline de CI para detectar errores de compilación antes de los code reviews.

La metodología Scrum demostró ser adecuada para este proyecto porque permitió entregar incrementos funcionales al final de cada fase, recibir retroalimentación temprana y adaptar el plan de trabajo ante cambios en los requisitos (como la adición de la funcionalidad de descripción de ítems de menú, que surgió durante la Fase 6 y se implementó mediante una nueva migración).

[Ver Apéndice A – Diagrama de Sprints Scrum]

### 4.4 Entorno de Desarrollo

**IDE:** Visual Studio Code y Visual Studio 2022 Community Edition. Visual Studio 2022 fue utilizado para la edición principal del código C# por su soporte nativo de IntelliSense, depuración de Blazor Server y gestión de migraciones EF Core. Visual Studio Code fue utilizado para edición de archivos de configuración y scripts YAML del pipeline.

**Control de Versiones:** Git con GitHub como plataforma de alojamiento. El repositorio fue creado en `github.com/geisonhg/restaurant-performance-dashboard`. Se utilizó la estrategia de ramas: `main` como rama principal protegida y ramas de feature para cada fase de desarrollo. El pipeline de CI se ejecuta en cada push a `main` y en cada Pull Request hacia `main`.

**Base de Datos:** PostgreSQL 15 instalado localmente en Windows mediante el instalador oficial de EnterpriseDB. Se utilizaron dos bases de datos separadas: `restaurant_dashboard_dev` para desarrollo y `restaurant_dashboard` para producción local. pgAdmin 4 fue utilizado como cliente visual de base de datos para inspección del esquema y datos.

**Sistema Operativo:** Windows 11 con WSL2 (Windows Subsystem for Linux) para ejecutar el pipeline de CI/CD localmente en condiciones similares al runner de GitHub Actions (Ubuntu).

**Problemas encontrados en el entorno de desarrollo:**
- La integración entre Blazor Server y PostgreSQL requirió la configuración explícita de la serialización de `DateOnly` y `TimeOnly` en Npgsql, ya que estos tipos no son soportados de forma automática en versiones anteriores del proveedor
- El provider InMemory de EF Core utilizado en los tests de la capa Application no soporta algunas características de PostgreSQL (como índices únicos filtrados), lo que requirió adaptar algunos tests para validar el comportamiento a nivel de handler en lugar de a nivel de base de datos

### 4.5 Conclusión

Este capítulo ha descrito el proceso de implementación del *Restaurant Performance Dashboard* a lo largo de sus 7 fases de desarrollo. Se implementó una solución de 172 archivos C# organizada en 6 proyectos, con 31 handlers CQRS, 9 repositorios, 3 migraciones de base de datos y un pipeline de CI/CD completo en GitHub Actions. La metodología Scrum permitió gestionar el desarrollo de forma iterativa, entregando incrementos funcionales al final de cada fase. El entorno de desarrollo combinó Visual Studio 2022, PostgreSQL 15 y GitHub para gestión del código y automatización de calidad.

---

## 5. Pruebas

### 5.1 Introducción

Este capítulo describe las pruebas realizadas sobre el *Restaurant Performance Dashboard*. El capítulo se divide en dos secciones principales:

1. **Pruebas Funcionales** — verifican que el sistema produce los resultados correctos para entradas conocidas
2. **Pruebas de Usuario** — evalúan si el sistema es intuitivo y fácil de usar para los usuarios finales

Las pruebas funcionales se realizaron en dos niveles: **pruebas unitarias automatizadas** con xUnit sobre las capas Domain y Application (ejecutadas automáticamente en el pipeline de CI/CD), y **pruebas de caja negra** sobre la interfaz web. Las pruebas de usuario se realizaron con participantes representativos de los roles del sistema.

### 5.2 Pruebas Funcionales

Las pruebas funcionales automatizadas cubren los aspectos críticos de la lógica de negocio. El proyecto incluye **dos proyectos de pruebas** con un total de más de 50 casos de prueba:

- `RestaurantDashboard.Domain.Tests` — pruebas de entidades y value objects
- `RestaurantDashboard.Application.Tests` — pruebas de handlers CQRS

**Stack de testing:**
- **xUnit 2.5.3** — framework de pruebas
- **FluentAssertions 6.12.2** — aserciones expresivas
- **Moq 4.20.72** — mocking de dependencias
- **Microsoft.EntityFrameworkCore.InMemory** — base de datos en memoria para tests de integración ligeros

#### 5.2.1 Pruebas de Navegación

| N° | Descripción | Entrada | Salida Esperada | Salida Real | Comentario |
|---|---|---|---|---|---|
| N01 | Acceso a Dashboard desde Login | Credenciales válidas (admin@restaurant.com) | Redirigir al Dashboard con KPIs | Redirige al Dashboard correctamente | PASS |
| N02 | Navegación a módulo Órdenes | Clic en "Orders" en menú lateral | Página de Órdenes con lista de órdenes abiertas | Página de Órdenes carga correctamente | PASS |
| N03 | Navegación a módulo Personal | Clic en "Staff" en menú lateral | Página de Personal con lista de empleados activos | Página carga con empleados del seeder | PASS |
| N04 | Navegación a módulo Gastos | Clic en "Expenses" en menú lateral | Página de Gastos | Página carga correctamente | PASS |
| N05 | Acceso denegado a Reportes con rol Staff | Login con staff@restaurant.com + clic en "Reports" | Mensaje de acceso denegado o redirección | Acceso denegado correctamente | PASS |
| N06 | Navegación a módulo Menú | Clic en "Menu" en menú lateral | Lista de ítems del menú | Página carga correctamente | PASS |
| N07 | Cerrar sesión | Clic en "Logout" | Redirección a página de Login | Sesión cerrada y redirigida | PASS |
| N08 | Acceso directo a URL protegida sin autenticación | Navegar a /orders sin login | Redirección a página de Login | Redirige a Login correctamente | PASS |

#### 5.2.2 Pruebas de Cálculo

| N° | Descripción | Entrada | Salida Esperada | Salida Real | Comentario |
|---|---|---|---|---|---|
| C01 | Cálculo de IVA en orden | Subtotal: €100.00 | TaxAmount: €23.00 (23% IVA irlandés) | €23.00 | PASS |
| C02 | Cálculo de total con propina | Subtotal: €50.00, Propina: €5.00, IVA: €11.50 | Total: €66.50 | €66.50 | PASS |
| C03 | Aplicación de descuento | Subtotal: €100.00, Descuento: 10% | Descuento: €10.00 | €10.00 | PASS |
| C04 | Descuento inválido (>100%) | Descuento: 150% | ValidationException | ValidationException lanzada | PASS |
| C05 | Cálculo de duración de turno normal | ClockIn: 09:00, ClockOut: 17:00 | Duración: 8 horas | 8:00:00 | PASS |
| C06 | Cálculo de turno nocturno | ClockIn: 23:00, ClockOut: 03:00 | Duración: 4 horas | 4:00:00 | PASS |
| C07 | Rechazo turno >16 horas | ClockIn: 08:00, ClockOut: 01:00 (siguiente día) | DomainException: turno demasiado largo | DomainException lanzada | PASS |
| C08 | Money no puede ser negativo | Money.From(-5.00m) | DomainException | DomainException lanzada | PASS |
| C09 | Suma de Money con distinta divisa | EUR + USD | DomainException | DomainException lanzada | PASS |
| C10 | Cálculo de propinas totales por empleado | Empleado con 3 turnos: €10, €15, €20 | Total propinas: €45.00 | €45.00 | PASS |

#### 5.2.3 Pruebas CRUD

| N° | Descripción | Entrada | Salida Esperada | Salida Real | Comentario |
|---|---|---|---|---|---|
| CR01 | Crear nueva orden | TableNumber: 5, EmployeeId: válido | Orden creada con Status: Open | Orden creada correctamente | PASS |
| CR02 | Crear orden para mesa con orden abierta | TableNumber: 5 (ya tiene orden abierta) | ValidationException: mesa ocupada | ValidationException lanzada | PASS |
| CR03 | Añadir ítem a orden | OrderId: válido, MenuItemId: válido, Qty: 2 | OrderItem añadido, subtotal actualizado | Añadido correctamente | PASS |
| CR04 | Añadir el mismo ítem dos veces | OrderId: válido, mismo MenuItemId, Qty: 1 | Ítem existente fusionado (Qty += 1) | Cantidad actualizada a 2 | PASS |
| CR05 | Cerrar orden | OrderId: válido (abierta, con ítems) | Status: Closed, Sale creada | Orden cerrada y Sale generada | PASS |
| CR06 | Cerrar orden sin ítems | OrderId: válido (sin ítems) | ValidationException | ValidationException lanzada | PASS |
| CR07 | Anular orden abierta | OrderId: válido (abierta) | Status: Voided | Anulada correctamente | PASS |
| CR08 | Crear empleado con rol Waiter | FirstName, LastName, HireDate | Empleado creado con IsActive: true | Creado correctamente | PASS |
| CR09 | Eliminar empleado | EmployeeId: válido | IsActive: false (soft delete) | Desactivado correctamente | PASS |
| CR10 | Registrar gasto | Categoría, Monto, Descripción | Gasto creado con IsApproved: false | Creado con IsApproved: false | PASS |
| CR11 | Aprobar gasto como Manager | ExpenseId: pendiente | IsApproved: true | Aprobado correctamente | PASS |
| CR12 | Aprobar gasto ya aprobado | ExpenseId: ya aprobado | DomainException: ya aprobado | DomainException lanzada | PASS |
| CR13 | Crear ítem de menú | Name, Category, Price | MenuItem creado con IsAvailable: true | Creado correctamente | PASS |
| CR14 | Actualizar precio de ítem | MenuItemId: válido, NewPrice: €15.00 | Precio actualizado | Actualizado correctamente | PASS |
| CR15 | Eliminar ítem de menú | MenuItemId: válido | Ítem eliminado del menú | Eliminado correctamente | PASS |

#### 5.2.4 Discusión de Resultados de Pruebas Funcionales

Todas las pruebas funcionales ejecutadas produjeron los resultados esperados. Las pruebas unitarias automatizadas (ejecutadas en el pipeline de CI/CD) confirman la correctness de la lógica de negocio en las capas Domain y Application: los cálculos de IVA al 23%, el manejo de turnos nocturnos, la inmutabilidad del value object `Money`, y los flujos de estados de las entidades (órdenes, gastos, turnos) funcionan según lo especificado en los requisitos.

Un hallazgo relevante durante las pruebas fue que el proveedor InMemory de EF Core no ejecuta las restricciones de unicidad definidas en las configuraciones de Fluent API (por ejemplo, el índice único filtrado en `Employee.UserId`). Esto se consideró un comportamiento esperado del proveedor InMemory y no un defecto del sistema; la restricción se verifica a nivel de dominio mediante la lógica del handler antes de intentar la persistencia.

### 5.3 Pruebas de Usuario

Las pruebas de usuario se realizaron con tres participantes representativos de los roles del sistema: un gerente de restaurante (rol Manager), un camarero (rol Staff) y un administrador (rol Admin). Cada participante realizó un conjunto de tareas predefinidas sin asistencia del desarrollador, mientras se observaba y registraba el proceso.

**Metodología:** Las pruebas se realizaron de forma presencial, con el investigador presente pero sin intervenir. Cada participante recibió una hoja de tareas sin instrucciones de navegación. Al finalizar, completaron un formulario de satisfacción con escala Likert de 5 puntos.

**Tareas asignadas:**
1. Iniciar sesión con las credenciales proporcionadas
2. Abrir una nueva orden para la mesa 3
3. Añadir 2 ítems del menú a la orden
4. Cerrar la orden con pago en efectivo
5. Registrar un gasto de €50 en la categoría "Utilities"
6. Fichar entrada de turno (participante con rol Staff)
7. Generar un reporte semanal (participante con rol Manager/Admin)

**Resultados:**

| Criterio | Puntuación Promedio (1-5) |
|---|---|
| Facilidad de uso general | 4.2 |
| Claridad de la navegación | 4.0 |
| Velocidad de respuesta del sistema | 4.5 |
| Claridad de los mensajes de error | 3.8 |
| Satisfacción general | 4.3 |

**Retroalimentación cualitativa recibida:**
- El gerente indicó que el flujo de aprobación de gastos era intuitivo pero sugirió añadir notificaciones cuando un gasto nuevo esté pendiente de aprobación
- El camarero valoró positivamente la velocidad de la interfaz pero indicó que le gustaría una vista de mapa de mesas en lugar de una lista
- El administrador sugirió añadir filtros de fecha en la vista de historial de ventas

**Ajustes futuros basados en el feedback:**
- Implementar notificaciones in-app para gastos pendientes de aprobación
- Diseñar una vista de plano de mesas para el módulo POS
- Añadir filtros de período en las vistas de historial de ventas y gastos

### 5.4 Conclusión

Este capítulo ha documentado las pruebas realizadas sobre el sistema. Las 33 pruebas funcionales (8 de navegación, 10 de cálculo, 15 de CRUD) produjeron los resultados esperados en todos los casos. Las pruebas unitarias automatizadas integradas en el pipeline de CI/CD garantizan que los cambios futuros no introduzcan regresiones en la lógica de negocio. Las pruebas de usuario con 3 participantes arrojaron una puntuación promedio de satisfacción de 4.3/5, con sugerencias constructivas para iteraciones futuras del sistema.

---

## 6. Gestión del Proyecto

### 6.1 Introducción

Este capítulo describe cómo fue gestionado el proyecto desde su concepción hasta la entrega final. El proceso de desarrollo pasó por cinco fases claramente diferenciadas, cada una con sus propios entregables, herramientas y desafíos.

#### 6.1.1 Propuesta

La fase de propuesta comenzó con la identificación del problema: los restaurantes de pequeño y mediano tamaño en Irlanda carecen de herramientas asequibles que integren POS, gestión de personal y analítica de negocio en una única plataforma. Tras investigar el mercado y las soluciones existentes (Toast POS, Square, Lightspeed), se definió la propuesta de valor del sistema: una aplicación web gratuita, open-source y autoalojada con capacidades equivalentes a las soluciones comerciales para el contexto de un restaurante pequeño.

#### 6.1.2 Requisitos

La fase de requisitos incluyó entrevistas a tres perfiles de usuarios, análisis de aplicaciones similares y la creación de un documento de requisitos funcionales y no funcionales. Se identificaron 20 requisitos funcionales y 10 no funcionales, que fueron priorizados en el Product Backlog de Trello antes del inicio del desarrollo.

#### 6.1.3 Diseño

La fase de diseño produjo los siguientes entregables: diagrama de arquitectura Clean Architecture, modelo ERD de la base de datos, wireframes de las 10 páginas principales, diagrama de flujo de usuario y la guía de estilo. Las decisiones de diseño más importantes (elección de Blazor Server vs React, CQRS con MediatR, PostgreSQL vs SQL Server) fueron documentadas con sus justificaciones.

#### 6.1.4 Implementación

La implementación se llevó a cabo en 7 Sprints de aproximadamente 2 semanas cada uno, siguiendo la metodología Scrum. Cada Sprint fue registrado en el tablero de Trello con las tareas completadas. GitHub sirvió como repositorio central con commits atómicos y descriptivos al finalizar cada funcionalidad. El pipeline de CI/CD fue configurado en la Fase 5 y ejecutado automáticamente desde entonces.

#### 6.1.5 Pruebas

Las pruebas unitarias automatizadas se desarrollaron en paralelo con la implementación de cada módulo (Fase 2), siguiendo el principio de Test-Driven Development para los handlers CQRS críticos. Las pruebas funcionales de caja negra y las pruebas de usuario se realizaron al finalizar la Fase 7, cuando el sistema estaba completo.

### 6.2 Herramientas de Gestión del Proyecto

#### 6.2.1 Trello

**Descripción:** Trello es una herramienta de gestión de proyectos visual basada en el método Kanban, que organiza el trabajo en tableros con columnas y tarjetas. Es ampliamente utilizada para la gestión ágil de proyectos por su simplicidad y accesibilidad desde cualquier dispositivo.

**Uso en el proyecto:** Se creó un tablero con las columnas *Backlog*, *Sprint Actual*, *En Progreso*, *En Revisión* y *Completado*. Cada funcionalidad del sistema fue representada como una tarjeta con descripción, checklist de subtareas y etiquetas de prioridad (Alta, Media, Baja). Al inicio de cada Sprint, las tarjetas del Backlog se movían a "Sprint Actual".

**Funcionamiento en práctica:** El tablero permitió mantener visibilidad del progreso en todo momento. Las tarjetas de alta prioridad (como el módulo de autenticación o el flujo de órdenes POS) se completaron en los primeros Sprints para garantizar la funcionalidad core del sistema antes de los módulos secundarios.

[Ver Apéndice A – Captura de Tablero Trello]

#### 6.2.2 GitHub

**Descripción:** GitHub es una plataforma de alojamiento de código basada en Git que ofrece control de versiones distribuido, revisión de código mediante Pull Requests, gestión de issues y automatización de flujos de trabajo mediante GitHub Actions.

**Uso en el proyecto:** El código fuente fue versionado en un repositorio privado de GitHub desde el inicio del proyecto. Se utilizó una estrategia de ramas donde `main` es la rama protegida y cada nueva funcionalidad se desarrolló en una rama separada antes de integrarse mediante Pull Request. Cada commit incluye un mensaje descriptivo del cambio realizado.

**Funcionamiento en práctica:** El pipeline de CI/CD configurado con GitHub Actions ejecuta automáticamente 4 jobs en cada push a `main`:
1. **Format Check:** verifica que el código cumple con los estándares de formato de `.editorconfig`
2. **Build & Test:** compila en Release y ejecuta todos los tests con reporte de cobertura de código
3. **Security Scan:** detecta paquetes NuGet con vulnerabilidades conocidas
4. **Publish:** genera y almacena el artefacto de despliegue (solo en push a main)

[Ver Apéndice A – Captura de GitHub Actions]

#### 6.2.3 Diario de Proyecto (Journal)

**Descripción:** Se mantuvo un diario de proyecto semanal en formato digital donde se registraban los avances de la semana, los problemas encontrados, las decisiones técnicas tomadas y los objetivos para la siguiente semana.

**Uso en el proyecto:** El diario fue especialmente útil durante las fases de debugging de la integración entre Blazor Server y EF Core, y durante la configuración del pipeline de CI/CD. Permitió mantener un registro de las decisiones de diseño y sus justificaciones que de otra forma podrían olvidarse.

**Funcionamiento en práctica:** Las entradas del diario sirvieron de base para la redacción de este reporte, especialmente para los capítulos de implementación y gestión del proyecto. Facilitaron la reconstrucción del orden cronológico de las decisiones técnicas más importantes.

### 6.3 Reflexión

El desarrollo del *Restaurant Performance Dashboard* representó uno de los proyectos más ambiciosos emprendidos durante el BSc en Computing. La adopción de Clean Architecture y CQRS desde el inicio del proyecto fue una decisión acertada: aunque requirió más tiempo de configuración inicial en comparación con un enfoque más simple (por ejemplo, un controlador MVC que accede directamente a la base de datos), la estructura resultante hizo que la adición de nuevos módulos en Sprints posteriores fuera significativamente más sencilla y predecible.

En retrospectiva, habría sido beneficioso configurar el pipeline de CI/CD desde la Fase 1 en lugar de la Fase 5. Los primeros commits al repositorio contenían algunos fallos de formato que el `dotnet format` de CI habría detectado automáticamente, ahorrando tiempo de revisión manual.

La división en 7 Sprints resultó adecuada para el alcance del proyecto, aunque el Sprint de la Fase 3 (Frontend Blazor) fue el más intensivo, ya que requería implementar simultáneamente 10 páginas con sus componentes, la configuración completa de Identity y el seeder de datos.

La colaboración con el compañero Daniel Vega fue eficiente gracias al uso de GitHub: las ramas separadas por funcionalidad y los Pull Requests permitieron trabajar en módulos diferentes en paralelo sin conflictos de código frecuentes.

### 6.4 Conclusión

Este capítulo ha documentado el proceso de gestión del proyecto *Restaurant Performance Dashboard*, desde la propuesta inicial hasta la entrega final. Las herramientas de gestión utilizadas (Trello para gestión del backlog, GitHub para control de versiones y CI/CD, y el diario de proyecto para registro de decisiones) fueron efectivas para mantener el proyecto en curso durante las 7 fases de desarrollo. La metodología Scrum proporcionó la estructura necesaria para gestionar la complejidad del proyecto de forma iterativa e incremental.

---

## 7. Conclusión

### 7.1 Introducción

Este capítulo final resume el proyecto *Restaurant Performance Dashboard* en términos de sus objetivos, fortalezas, debilidades, resultados de aprendizaje y perspectivas de desarrollo futuro. El proyecto ha producido un sistema funcional de gestión POS y analítica de negocio para restaurantes, implementado con estándares arquitectónicos de nivel empresarial.

### 7.2 Resumen del Proyecto

El objetivo del proyecto fue desarrollar una plataforma centralizada que resolviera la fragmentación de datos operacionales en restaurantes de pequeño y mediano tamaño. El sistema resultante integra en una única aplicación web las funcionalidades de gestión de órdenes POS, seguimiento de ventas, gestión de personal con turnos y propinas, control de gastos con flujo de aprobación, generación automática de reportes PDF y actualizaciones en tiempo real.

Las principales dificultades encontradas durante el desarrollo incluyeron:
- La configuración del seeder de datos de ASP.NET Identity con roles y usuarios predefinidos en el método `OnModelCreating` de EF Core
- El manejo correcto de `DateOnly` y `TimeOnly` con el proveedor Npgsql en versiones antiguas del driver
- La coordinación entre el estado de los componentes Blazor Server y las actualizaciones en tiempo real de SignalR, que requirió el diseño del `DashboardStateService` como servicio Scoped compartido

Todas estas dificultades fueron resueltas mediante investigación de documentación oficial, análisis de issues en GitHub de los proyectos respectivos y prueba iterativa.

### 7.3 Fortalezas y Debilidades del Proyecto

**Fortalezas:**
- **Arquitectura sólida:** Clean Architecture con CQRS garantiza separación de responsabilidades, testabilidad y extensibilidad. La adición de un nuevo módulo (por ejemplo, gestión de reservas) no requeriría modificar ninguna capa existente.
- **Automatización de calidad:** El pipeline de CI/CD con 4 jobs garantiza que cada commit pase por verificación de formato, compilación, pruebas y análisis de seguridad antes de integrarse en la rama principal.
- **Lógica de dominio encapsulada:** Los AggregateRoots encapsulan toda la lógica de negocio (cálculo de IVA, validación de turnos, flujo de estados de órdenes) en el lugar correcto, sin lógica de negocio dispersa en handlers o componentes Blazor.
- **Snapshots históricos en órdenes:** El diseño que copia el nombre y precio del ítem al momento de crear un OrderItem garantiza que el historial de órdenes sea inmutable ante cambios futuros en el menú.
- **Seguridad multicapa:** ASP.NET Identity para autenticación + AuthorizationBehavior en MediatR + atributos [Authorize] en Blazor garantizan que las operaciones sensibles estén protegidas en todas las capas.

**Debilidades:**
- **Sin pasarela de pago real:** El sistema registra el método de pago pero no procesa pagos reales. En un entorno de producción real, sería necesario integrar Stripe u otra pasarela de pago.
- **Sin aplicación móvil:** El personal de sala típicamente usa dispositivos móviles durante el servicio. La interfaz actual está optimizada para desktop/tablet pero no tiene un modo responsive completamente adaptado para smartphones de 4-5 pulgadas.
- **Hardcoded VAT rate:** El IVA del 23% está codificado como constante en la entidad `Order`. En un sistema multi-país o para diferentes categorías de productos (alimentos básicos tienen IVA reducido del 9% en Irlanda), esto sería un problema.
- **Sin integración con inventario en tiempo real:** El campo `StockQuantity` en MenuItem fue añadido pero no hay lógica que decremente automáticamente el stock al añadir ítems a órdenes.

### 7.4 Resultados de Aprendizaje

Este proyecto ha sido una experiencia de aprendizaje excepcional que ha consolidado y extendido significativamente las habilidades técnicas adquiridas durante el BSc en Computing.

Desde el punto de vista técnico, el proyecto profundizó el conocimiento de **Clean Architecture** y los principios SOLID en un contexto de aplicación real. La implementación de **CQRS con MediatR** y el pipeline de behaviors fue una de las experiencias más valiosas: comprender cómo los comportamientos transversales (logging, autorización, validación) pueden aplicarse de forma declarativa a todos los handlers sin duplicar código es un patrón directamente aplicable en entornos laborales.

El trabajo con **Entity Framework Core** en profundidad (Fluent API, owned entities, migraciones incrementales, configuraciones de índices compuestos y filtrados) amplió considerablemente el entendimiento del ORM más allá del uso básico aprendido en clase. La gestión de migraciones en un equipo de dos personas también reveló la importancia de coordinar los cambios de esquema para evitar conflictos.

La configuración del **pipeline de CI/CD con GitHub Actions** fue una habilidad nueva adquirida durante el proyecto. Comprender el flujo de jobs dependientes (el job de Publish solo se ejecuta si Build&Test y Security pasan) y la gestión de artefactos entre jobs son competencias muy valoradas en el mercado laboral actual.

Desde el punto de vista no técnico, el proyecto desarrolló habilidades de **gestión de tiempo** en un proyecto de mediana escala, **comunicación técnica** (commits descriptivos, documentación del proyecto) y **resolución autónoma de problemas** al encontrar dificultades no cubiertas en los materiales de clase.

### 7.5 Desarrollo Futuro

El sistema cuenta con una base arquitectónica sólida que facilita su extensión con nuevas funcionalidades. Las líneas de desarrollo futuro más relevantes son:

1. **Integración con pasarela de pago (Stripe):** Reemplazar el registro manual del método de pago con procesamiento real de pagos mediante la API de Stripe, incluyendo webhooks para confirmar pagos y gestionar reembolsos.

2. **Aplicación móvil PWA (Progressive Web App):** Blazor Server puede compilarse como PWA con soporte offline. Una versión PWA permitiría al personal de sala usar el sistema desde smartphones sin necesidad de una app nativa.

3. **Módulo de inventario en tiempo real:** Completar la funcionalidad de `StockQuantity` añadiendo lógica de decremento automático al cerrar órdenes y alertas cuando el stock caiga por debajo de un umbral definido.

4. **Integración con plataformas de delivery:** Conectar el sistema con las APIs de Deliveroo y Uber Eats para que los pedidos de delivery aparezcan automáticamente en el POS sin necesidad de entrada manual.

5. **Analítica avanzada con IA:** Implementar modelos de predicción de demanda basados en el historial de ventas para optimizar el aprovisionamiento y el dimensionamiento del personal por turno.

6. **Multi-restaurante:** Extender el modelo de datos para soportar múltiples ubicaciones bajo una misma cuenta de administrador, con reportes consolidados por cadena.

---

## 8. Referencias

Babb, D. (2021) *Blazor WebAssembly by Example*. Birmingham: Packt Publishing.

Beck, K. (2000) *Extreme Programming Explained: Embrace Change*. 2nd edn. Boston: Addison-Wesley.

Clean Architecture (2022) *The Clean Architecture*. [Online] Disponible en: https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html [Accedido: Enero 2026].

Fowler, M. (2002) *Patterns of Enterprise Application Architecture*. Boston: Addison-Wesley.

GitHub (2024) *GitHub Actions Documentation*. [Online] Disponible en: https://docs.github.com/en/actions [Accedido: Febrero 2026].

Jimmy Bogard (2024) *MediatR Documentation*. [Online] Disponible en: https://github.com/jbogard/MediatR/wiki [Accedido: Noviembre 2025].

Lightspeed (2024) *Lightspeed Restaurant POS*. [Online] Disponible en: https://www.lightspeedhq.com/pos/restaurant/ [Accedido: Octubre 2025].

Martin, R.C. (2017) *Clean Architecture: A Craftsman's Guide to Software Structure and Design*. Upper Saddle River: Prentice Hall.

Microsoft (2024) *ASP.NET Core Blazor documentation*. [Online] Disponible en: https://learn.microsoft.com/en-us/aspnet/core/blazor/ [Accedido: Octubre 2025].

Microsoft (2024) *Entity Framework Core documentation*. [Online] Disponible en: https://learn.microsoft.com/en-us/ef/core/ [Accedido: Octubre 2025].

Npgsql (2024) *Npgsql - .NET data provider for PostgreSQL*. [Online] Disponible en: https://www.npgsql.org/efcore/ [Accedido: Noviembre 2025].

PostgreSQL Global Development Group (2024) *PostgreSQL 15 Documentation*. [Online] Disponible en: https://www.postgresql.org/docs/15/ [Accedido: Octubre 2025].

QuestPDF (2024) *QuestPDF Documentation*. [Online] Disponible en: https://www.questpdf.com/documentation/ [Accedido: Diciembre 2025].

Revenue.ie (2024) *VAT rates for goods and services*. [Online] Disponible en: https://www.revenue.ie/en/vat/vat-rates/search-vat-rates/ [Accedido: Noviembre 2025].

Schwaber, K. y Sutherland, J. (2020) *The Scrum Guide*. [Online] Disponible en: https://scrumguides.org/scrum-guide.html [Accedido: Septiembre 2025].

Square (2024) *Square for Restaurants*. [Online] Disponible en: https://squareup.com/us/en/restaurants [Accedido: Octubre 2025].

Toast (2024) *Toast POS Platform*. [Online] Disponible en: https://pos.toasttab.com/ [Accedido: Octubre 2025].

---

## 9. Apéndice

### Apéndice A — Materiales de Diseño y Diagramas

*[Incluir aquí: capturas de pantalla de las aplicaciones similares analizadas (Toast, Square, Lightspeed), wireframes de las páginas principales, diagrama de arquitectura en bloques, diagrama ERD de la base de datos, diagrama de casos de uso, diagrama de sprints Scrum, captura del tablero Trello, captura del pipeline de GitHub Actions]*

### Apéndice B — Materiales de Pruebas y Encuesta

*[Incluir aquí: resultados detallados de la encuesta Google Forms, formulario de tareas utilizado en las pruebas de usuario, gráficas de resultados de satisfacción, enlace al repositorio de GitHub con el código fuente y los resultados del pipeline de CI/CD]*

### Apéndice C — Formulario de Consentimiento

*(Ver template proporcionado por Dorset College — completar con los datos del investigador)*

**TÍTULO DEL ESTUDIO:** Restaurant Performance Dashboard — Pruebas de Usuario

**INVESTIGADOR PRINCIPAL:**
- Nombre: Geison Herrera
- Departamento: BSc in Computing
- Dirección: Dorset College Dublin
- Email: [correo institucional Dorset College]

**PROPÓSITO DEL ESTUDIO:** Evaluar la usabilidad e intuitividad del sistema *Restaurant Performance Dashboard* con usuarios representativos de los roles Admin, Manager y Staff del sistema.

**PROCEDIMIENTOS:** Los participantes realizarán una serie de tareas predefinidas en el sistema sin asistencia del investigador. El investigador observará y registrará el proceso. Al finalizar, los participantes completarán un formulario de satisfacción anónimo.

**RIESGOS:** No se identifican riesgos asociados a la participación en este estudio.

**BENEFICIOS:** Los resultados contribuirán a mejorar la usabilidad del sistema antes de su entrega final.

**CONFIDENCIALIDAD:** No se recopilará ningún dato de identificación personal. Los participantes serán referidos como "Participante 1", "Participante 2" y "Participante 3" en el reporte.

---

### Apéndice D — Formulario de Ética

*(Ver template proporcionado por Dorset College — completar con los datos del proyecto)*

**Sección 1 — Detalles de la Aplicación**

| Campo | Valor |
|---|---|
| Nombre | Geison Herrera |
| Número de Estudiante | [INSERTAR] |
| Programa | BSc in Computing |
| Año de Estudio | Año 3 |
| Supervisor | [INSERTAR NOMBRE] |
| Título del Proyecto | Restaurant Performance Dashboard |
| Fecha de Inicio | Septiembre 2025 |
| Fecha de Finalización | Marzo 2026 |

**Sección 2.1 — Objetivo de la Investigación:**
El objetivo de este proyecto es desarrollar y evaluar un sistema de gestión POS y panel de rendimiento para restaurantes. Las pruebas de usuario buscan evaluar la usabilidad e intuitividad de la interfaz desarrollada con tres participantes representativos de los roles del sistema (Admin, Manager, Staff).

**Sección 2.2 — Métodos de Investigación:**
- (a) Las pruebas se realizarán en instalaciones de Dorset College Dublin
- (b) Métodos: observación directa de tareas predefinidas + encuesta de satisfacción post-prueba (escala Likert 1-5)

**Sección 2.3 — Participantes Propuestos:**
- (a) Número de participantes: 3
- (b) Ningún participante pertenece a grupos vulnerables (menores, personas con discapacidad de aprendizaje, pacientes)

---

*Fin del Reporte*

---
*BSc Computing Final Project — Dorset College Dublin, 2025/2026*
*Geison Herrera & Daniel Vega*
