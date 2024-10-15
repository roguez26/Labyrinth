# Estándar de codificación para Tecnologías de Software

## Introducción

El propósito del presente estándar es establecer un conjunto de reglas y guías que sirvan como pauta para la consistencia, claridad y calidad del código en el proyecto que se va a realizar durante el curso actual. Estas reglas fueron seleccionadas para facilitar la lectura, el mantenimiento y la seguridad del código, y al mismo tiempo, minimizar los errores y que se mejore la colaboración entre los desarrolladores. Este estándar cubre aspectos clave como el nombrado de variables, constantes, clases, métodos, propiedades y namespaces. Estilo e indentación, basados en la guía que Microsoft provee para el lenguaje C#; así como, el uso de comentarios, el uso correcto de estructuras de control, manejo de excepciones, niveles de logueo y prácticas seguras de construcción.

## Propósito 

### Propósito general

El propósito general del presente trabajo es crear un estándar de codificación para el lenguaje de programación C# que sirva como guía para el equipo durante la construcción del proyecto de software “NOMBRE DEL PROYECTO”, asimismo, que permita a externos entender las reglas de estilo y decisiones de construcción que se tomaron. El idioma del código será inglés.

### Propósitos específicos
* Definir reglas claras y consistentes de sintaxis, nomenclatura y estilo para la codificación.
* Fomentar la legibilidad y la comprensión del código.
* Promover la coherencia y la calidad del código en todo el proyecto.

## Estructura del estándar
El presente estándar de codificación se encuentra estructurado de la siguiente forma:
Propósito
Sección donde se abarca el propósito general y específico por el cual se realiza el siguiente estándar.
Reglas de nombrado
Sección donde se abarcan las pautas que se tomaron para nombrar los diferentes elementos que se usarán dentro del código, como el nombrado de variables, clases, constantes, espacios de nombre (namespaces), etc. Por otra parte también se abordan los diferentes estilos utilizados en diferentes casos.
Estilo
Sección donde se aborda el estilo del código, incluyendo indentación, el uso de llaves de apertura y cierre, la longitud de líneas, las líneas y espacios en blancos. Mismo que ayuda a la legibilidad y entendimiento del código el cual permitirá a las personas entender de manera más clara el código escrito.
Comentarios 
En esta sección se abarca la manera de realizar comentarios adecuada que ha sido seleccionada para el presente estándar de codificación.
Comentarios de documentación
Explicación de comentarios de documentación destinados a explicar la funcionalidad de métodos, mismos que solo serán usados para exponer la funcionalidad de la interface del servicio utilizado y las clases que implementan esa interface.
Estructuras de control
Sección donde se ilustra la manera adecuada de utilizar estructuras de control según las buenas prácticas de Microsoft.
Manejo de excepciones
Sección donde se ahonda en la manera de manejar las excepciones usando y mostrando ejemplos correctos e incorrectos de como hacerlo.
Niveles de errores 
Para el manejo adecuado de excepciones se realizó una jerarquía de errores que serviran de guía para darle un manejo a cada dependiendo de su grado de gravedad.
Logueo de excepciones 
Asimismo, se utilizó una jerarquía de niveles de logging de Microsoft para realizar el registro de excepciones en una bitácora.
Prácticas seguras de construcción  
Sección donde se plasman diferentes prácticas y recomendaciones de construcción que tienen como finalidad fomentar la calidad, robustez y seguridad del código y que sirvan como guia para diferentes proyectos de C#.


## Contenido

1. [Reglas de nombrado](#reglas-de-nombrado)
2. [Namespaces](#namespaces)
3. [Estilo](#estilo)
4. [Comentarios](#comentarios)
5. [Estructuras de control](#estructuras-de-control)
6. [Bucles](#bucles)
7. [Manejo de excepciones](#manejo-de-excepciones)
8. [Logueo de excepciones](#logueo-de-excepciones)
9. [Prácticas seguras de construcción](#prácticas-seguras-de-construcción)
10. [Referencias bibliográficas](#referencias-bibliográficas)

## Reglas de nombrado
* Solo se puede declarar una variable por línea.
* Se utilizará la convención de nombrado PascalCase/UpperCamelCase para el nombramiento de **clases**, **interfaces**, **structs**, **delegates**, **constantes** y **nombres de métodos**

Ejemplo correcto:
```
public class CustomerManager
```
Ejemplo incorrecto:
```
public class customerManager
```

* Se utilizará la convención de nombrado camelCase para el nombramiento de parámetros de los métodos y variables locales.
Ejemplo correcto:
```
public void ProcessOrder(int orderId, string customerName)
{
    int totalItems = 5; 
    double totalPrice = 99.99;
}
```
Ejemplo incorrecto:
```
public void ProcessOrder(int OrderId, string CustomerName)  
{
    int TotalItems = 5;
    double TotalPrice = 99.99; 
}
```
* Cuando se nombre una interfaz se debe de iniciar con una “I” mayúscula.
Ejemplo correcto:
```
public interface IDataProcessor
```
Ejemplo incorrecto:
```
public interface DataProcessor
```
* Los identificadores de variables deben de comenzar con una letra o un guión bajo. A su vez, estos no deben de tener dos guiones bajos debido a que está reservado para los identificadores generados por el compilador y no se debe de usar para separar palabras (“snake_case”).
Ejemplo correcto:
```
private int orderNumber = 123;
private int _tempValue = 0;
```
Ejemplo incorrecto:
```
int __hiddenValue = 100;
string customer_name = "Jane Doe";
```
* Los campos de instancia privados comienzan con un guión bajo y el resto del texto se escribe en camelCase.
Ejemplo correcto:
```
private int _orderCount;
```
Ejemplo incorrecto:
```
private int orderCount;
```
* Usar PascalCase para nombrar miembros públicos de tipo, campos, propiedades, eventos y funciones locales.
Ejemplo correcto:
```
public string CustomerName { get; set; }
```
Ejemplo incorrecto:
```
public int orderCount;
```
* Utilizar PascalCase/UpperCamelCase para el nombrado de constantes, evitar el uso de “Screaming Caps”. 
Ejemplo correcto:
```
public const int MaxItems = 100;
```
Ejemplo incorrecto:
```
public const int MAX_ITEMS = 100; 
```
* Se podrán usar palabras reservadas para nombres de identificadores, solo si es necesario y se deberá de utilizar el prefijo “@”.
Ejemplo correcto:
```
private int @int = 10;
```
Ejemplo incorrecto:
```
private int int = 10;
```
* Utilizar nombres con significado y descriptivos para variables, métodos, clases y espacios de nombre. Evitar el uso de abreviaciones a excepción de las que son nombres.
Ejemplo correcto:
```
private string customerEmailAddress;
```
Ejemplo incorrecto:
```
private string str;
```
* No omitir los modificadores de acceso
Ejemplo correcto:
```
public int orderCount;
```
Ejemplo incorrecto:
```
int orderCount;
```
Los nombres de las clases deben de ser sustantivos simples, completos y singulares.
Ejemplo correcto:
```
public class Customer
{
}
```
Ejemplo incorrecto:
```
public class Data
{
}
```
* Al nombrar colecciones de datos se deben de utilizar un sustantivo en plural que haga referencia al tipo de datos que almacena.
Ejemplo correcto:
```
List<int> numbers = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
```
Ejemplo incorrecto:
```
List<int> number = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
```

## Namespaces
* Evitar nombres genéricos

Ejemplo correcto: `TreeNode`
Ejemplo incorrecto: `Node`

* Calificar los nombres de tipos genéricos con un prefijo o sufijo que los haga únicos

Ejemplo correcto: `EmailMessage`
Ejemplo incorrecto: `Message`

* No repetir nombres de tipo dentro de un mismo modelo de aplicación
Ejemplo incorrecto: `Ya existe un tipo “Page” en “System.Web.UI”`

* Evitar conflictos con namespaces principales
Ejemplo incorrecto: `System.IO ya existe `

* Evitar que los tipos dentro de un grupo tecnológico entren en conflicto entre sí.
* Evitar conflictos con nombres de tipos entre namespaces tecnológicos y modelos de aplicación, si se desarrolla una tecnología que podría usarse en un modelo de aplicación evitar usar nombres que ya existan en ese namespace


## Estilo
### Indentación
* Se deben de utilizar cuatro espacios como unidad de indentación.
* El patrón de indentación debe de seguirse en todo el código, sin excepciones.
### Llaves de apertura y cierre
* La llave de apertura se debe de colocar en la línea debajo de la declaración de la estructura, alineado al mismo nivel de indentado.
* Tanto las llaves de aperturas como las de cierre deben estar en una línea separada y alineadas al indentado de la estructura a la que pertenecen.
* Toda declaración o estructura de control debe contener llaves, independientemente del número de líneas de código que contengan.

Ejemplo correcto:
```
if (orderCount > 0)
{
    Console.WriteLine("Processing order...");
}
```
Ejemplo incorrecto:
```
if (orderCount > 0)
    Console.WriteLine("Processing order..."); 

```
### Longitud de líneas
* Se deben evitar líneas de código de más de 120 caracteres.
  
### Espacios en blanco
Se debe de usar un espacio en blanco en los siguientes casos:
Antes y después de un operador, a excepción de operadores unarios y el “.”.
  
Ejemplo correcto:
```
int result =  number1 + number2;
```
Ejemplo incorrecto:
```
int result = number1+number2;
```
* Después de una coma.
Ejemplo correcto:
```
public void PrintValues(int valor1, int valor2, int valor3)
```
Ejemplo incorrecto:
```
public void PrintValues(int valor1,int valor2,int valor3)
```
* Después de una palabra clave.
Ejemplo correcto:
```
if (x > 0)
```
Ejemplo incorrecto:
```
if(x>0)
```
* Las expresiones de una operación for.
Ejemplo correcto:
```
for (int i = 0; i < 10; i++)  
{
    Console.WriteLine(i);
}
```
Ejemplo incorrecto:
```
for(int i=0;i<10;i++)  
{
    Console.WriteLine(i);
}
```
### Líneas en blanco 
Usar líneas en blanco para los siguientes casos:
* Para separar secciones de lógica de código que hagan cosas similares
Ejemplo correcto:
```
public class Example
{
    // Declaración de variables
    private int _orderCount;
    private string _customerName;

    // Métodos
    public void InitializeOrder(int count, string name)
    {
        orderCount = count;
        customerName = name;
    }

    public void PrintOrderDetails()
    {
        Console.WriteLine($"Order Count: {orderCount}");
        Console.WriteLine($"Customer Name: {customerName}");
    }
}
```
Ejemplo incorrecto:
```
public class Example
{
    private int orderCount;
    private string customerName;
    public void InitializeOrder(int count, string name)
    {
        orderCount = count;
        customerName = name;
    }
    public void PrintOrderDetails()
    {
        Console.WriteLine($"Order Count: {orderCount}");
        Console.WriteLine($"Customer Name: {customerName}");
    }
}
```
* Para separar métodos.
Ejemplo correcto:
```
public class Example
{
    public void ProcessOrder(int orderId)
    {
        // Código para procesar el pedido
    }

    public void CancelOrder(int orderId)
    {
        // Código para cancelar el pedido
    }
}
```
Ejemplo incorrecto:
```
public class Example
{
    public void ProcessOrder(int orderId)
    {
        // Código para procesar el pedido
    }
    public void CancelOrder(int orderId)
    {
        // Código para cancelar el pedido
    }
}
```
## Comentarios
Evitar el uso de comentarios en la misma línea para explicar el código, el buen código es autoexplicativo.

### Comentarios en bloque
Se utilizará el formato “/*  */” para realizar comentarios que necesiten más de una línea de código.
Ejemplo correcto:
```
/* 
* El bucle for se utiliza en lugar de foreach para evitar la sobrecarga 
* de enumeradores en grandes conjuntos de datos.
*/
```
Ejemplo incorrecto:
```
/* 
* El bucle for se utiliza en lugar de foreach para evitar la sobrecarga 
* /
```
### Comentarios de línea
Se utilizará el formato “//” para realizar comentarios que solamente necesiten de una línea de código.

Ejemplo correcto:
```
// Se utiliza while debido a que sólo se necesita un objeto que cumpla la condición
```
Ejemplo incorrecto:
```
// La función retornará el valor final del objeto
// Los parámetros que se necesitan es el precio y el descuento
```
### Comentarios de documentación 
Solo se utilizaran para la interface que expone la funcionalidad del servicio y las clases que implementan esa interface.
Se deben utilizar comentarios de C# con etiquetas XML internas y seguir las siguientes directivas:
* Las líneas deben ser precedidas por “///” para ser aceptadas como comentarios XML.
* Las etiquetas pueden ser de dos tipos:
    * Elementos de documentación: etiquetas que se relacionan con los elementos de la API del programa que se documenta. Como son `<summary>`, `<param>` o `<exception>`. Suelen tener atributos como `name` o `cref`.
    * Formato / referencia: etiquetas que permiten definir el formato de la documentación debe tener, o crear referencias a elementos del código. Como son `<code>`, `<list>` o `<param>`.

Ejemplo sencillo:
```
/// <summary> 
/// Validates the user's credentials. 
/// </summary>
```
Ejemplo de varias líneas: 
```
/// <param name="numbers">The list of integers to multiply.</param> 
/// <returns>The product of the numbers.</returns> 
/// <example> 
/// <code> 
/// var calculator = new Calculator(); 
/// int result = calculator.Multiply(new List<int> { 1, 2, 3, 4 }); 
/// Console.WriteLine(result); 
/// Output: 24 
/// </code> 
/// </example>
```
## Estructuras de control
* Todas las estructuras de control deben tener al menos una línea de código dentro de estas, asimismo, se debe de utilizar “{}” incluso si solo hay una instrucción que sigue a la estructura de control
### If
* La palabra reservada “If” debe estar en una línea separada 
* La palabra reservada “If” debe estar separada de la condición por un espacio en blanco 

Ejemplo correcto:
```
if (condición)
{
    // código
}
else
{
    // código
}
```
Ejemplo incorrecto: 
```
if(condición)
    // código
```
### Switch 
* Siempre se debe incluir un caso “Default” para manejar valores inesperados.
* La expresión dentro del switch debe ser una variable o expresión que se pueda evaluar.
* Cada “case” debe terminar con “break”, “return”, “continue” o “goto”.
* Incluir un caso “Default” para manejar casos no previstos.
* Cada case y el código dentro de él deben estar correctamente indentados y en su propia línea para mejorar la legibilidad.

Ejemplo correcto:
```
switch (dayNumber)
{
    case 1:
        Console.WriteLine("Monday");
        break;
    case 2:
        Console.WriteLine("Tuesday");
        break;
    case 3:
        Console.WriteLine("Wednesday");
        break;
    case 4:
        Console.WriteLine("Thursday");
        break;
    case 5:
        Console.WriteLine("Friday");
        break;
    case 6:
        Console.WriteLine("Saturday");
        break;
    case 7:
        Console.WriteLine("Sunday");
        break;
    default:
        Console.WriteLine("Invalid day number");
        break;
}
```
Ejemplo incorrecto:
```
switch (dayNumber)
{
    case 1:
        Console.WriteLine("Monday")
        break
    case 2:
        Console.WriteLine("Tuesday"); break;
    case 3: Console.WriteLine("Wednesday"); break;
    case 4:
        Console.WriteLine("Thursday");
        break;
    case 5: Console.WriteLine("Friday"); break
    case 6: Console.WriteLine("Saturday"); break;
    case 7: Console.WriteLine("Sunday"); break;
    default:
        Console.WriteLine("Invalid day number"); break;
}
```
## Bucles
### For
* La palabra “for” y los paréntesis deben estar separados por un espacio y las expresiones en un bucle for deberán estar separadas por espacios en blanco.
* La llave de apertura se coloca en la siguiente línea y la primera instrucción se coloca en la línea posterior a esta.

Ejemplo correcto: 
```
for (int i = 0; i < 3; i++)
{
    Console.Write(i);
}
```
Ejemplo incorrecto:
```
for (int i=0;i<10;i++)
{
System.out.println(i);}
```
### While
* La palabra reservada “while” debe aparecer en su propia línea, seguida inmediatamente por la expresión condicional, misma que deberá estar separada por un espacio.
* La llave de apertura se coloca en la siguiente línea y la primera instrucción se coloca en la línea posterior a esta.
* La llave de cierre comienza en una nueva línea, indentada para que coincida con su declaración de apertura correspondiente.

Ejemplo correcto:
```
while (true)
{
    // código 
}
```
Ejemplo incorrecto:
```
while(true){
    // código 
}
```
### Do-While
* La palabra reservada “do” debe aparecer en su propia línea, en la siguiente línea se debe colocar la llave de apertura.
Posterior a la llave de apertura se deben de colocar las instrucciones en una línea nueva.
* En una línea nueva después de las instrucciones se debe colocar la llave de cierre, seguido de un espacio y la condición de salida.

Ejemplo correcto:
```
do 
{
    // código 
} while (true);
```
Ejemplo incorrecto:
```
do{
// código }
while(true);
```

## Manejo de excepciones
* Usar un bloque try en las líneas de código que puedan tirar una excepción.
* Siempre incluir un bloque catch después de un bloque try si se espera una excepción.
* Solo usar excepciones que puedan ser atrapadas apropiadamente, evitar el uso de excepciones genéricas.
* Liberar los recursos que no se utilicen usando “finally” o “using”, usar “finally” cuando no se implementa iDisposable

Ejemplo correcto:
```
try
{
    // Código que puede lanzar una excepción
    reader = new StreamReader(filePath);
    string content = reader.ReadToEnd();
    Console.WriteLine(content);
}
catch (FileNotFoundException exception)
{
    // Manejo específico de excepción para archivo no encontrado
    Console.WriteLine($"File not found: {exception.Message}");
}
catch (IOException exception)
{
    // Manejo específico de excepción para errores de entrada/salida
    Console.WriteLine($"I/O error: {exception.Message}");
}
finally
{
    // Liberación de recursos
    if (reader != null)
    {
        reader.Close();
    }
    Console.WriteLine("File processing complete.");
}
```

Ejemplo incorrecto:
```
try
{
    reader = new StreamReader(filePath);
    string content = reader.ReadToEnd();
    Console.WriteLine(content);
}
catch (FileNotFoundException exception)
{
    Console.WriteLine($"File not found: {exception.Message}");
}
catch (IOException exception)
{
    Console.WriteLine($"I/O error: {exception.Message}");
} // No se liberan los recursos
```
## Niveles de errores
Se utilizarán los siguientes niveles de error para clasificar su grado de gravedad:

* Fatal:
Errores que causan que la aplicación se tenga que detener de manera inmediata. 
Ejemplo: Fallo en la conexión a la base de datos principal

* Error:
Problemas graves que afecten la funcionalidad, pero que no detienen la aplicación por completo. 
Ejemplo: Error al guardar un registro en la base de datos, pero la aplicación sigue operando

* Warning:
Situaciones que podrían llevar a un error en el futuro. No impiden el funcionamiento del código, pero son indicativos de posibles problemas.
Ejemplo: Entradas de datos inválidas que aún permiten continuar con la ejecución, pero podrían causar problemas si no se corrigen.

## Logueo de excepciones 
Se utilizará la librería “log4net” para llevar el registro de la bitácora de excepciones. Se debe respetar el nivel de log de la siguiente tabla:

| Nivel de logueo | Valor | Descripción                                                                                                                                                       |
| --------------- | ----- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Trace**       | 0     | **Contiene los mensajes más detallados. Estos mensajes pueden contener datos confidenciales de la aplicación.**                                                   |
| **Debug**       | 1     | **Para depuración y desarrollo**                                                                                                                                  |
| **Information** | 2     | **Realiza un seguimiento del flujo general de la aplicación.**                                                                                                    |
| **Warning**     | 3     | **Por eventos anormales o inesperados. Por lo general, incluye errores o condiciones que no hacen que se produzca un error en la aplicación.**                    |
| **Error**       | 4     | **Para errores y excepciones que no se pueden controlar. Estos mensajes indican un error en la operación o solicitud actual, no un error en toda la aplicación.** |
| **Critical**    | 5     | **Para fallos que requieren atención inmediata.**                                                                                                                 |
## Prácticas seguras de construcción 
* Validación de entradas: Se deben validar todas las entradas del usuario para evitar inyección de código, se validará para cada tipo de dato.
* Manejo adecuado de excepciones: Se deben de atrapar las excepciones específicas para el caso que se necesite y manejarla de acuerdo a la jerarquía de excepciones especificado en la sección “Niveles de errores”.
* Priorizar programar de manera en que se pueda evitar excepciones, en lugar de centrarse en atraparlas. 
    * Ejemplo correcto: Usar arreglos y utilizar validaciones para evitar un índice fuera del rango del tamaño del arreglo.
    * Ejemplo incorrecto: No realizar validaciones y atrapar una excepción de tipo IndexOutOfRangeException
* Manejar de manera correcta los recursos, liberarlos después de utilizarlos ya sea utilizando “using” o “finally”.
* Seguridad de datos sensibles, se utilizarán tecnologías de cifrado para protegerlos. Por otra parte, se deben de guardar en archivos de configuración separados.
* Implementar pruebas unitarias, cada método debe estar probado con al menos un caso exitoso y dos fallidos.
* Las librerías de terceros que se utilicen no deben tener problemas de seguridad conocidos.
* Permisos de acceso a la base de datos, utilizar usuarios con diferentes niveles de permisos de acuerdo a su nivel de acceso en el sistema.
* Número de parámetros en los métodos: Se recomienda utilizar tres parámetros en la firma de los métodos para disminuir la carga cognitiva en el lector, sin embargo, se pueden utilizar más conforme se vea necesario. 

## Referencias bibliográficas 
* Microsoft. (2022). Coding guidelines. Microsoft Learn. https://learn.microsoft.com/en-us/mixed-reality/world-locking-tools/documentation/howtos/codingconventions
* Microsoft. (2023). C# identifier naming rules and conventions. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names
* Microsoft. (2023). Exception handling in C#. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/exceptions/exception-handling
* Microsoft. (2023). Statements. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/statements
* Microsoft. (2024). Logging in .NET Core. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line
* Microsoft. (2024). Use whitespace to improve readability. Microsoft Learn. https://learn.microsoft.com/en-us/training/modules/csharp-readable-code/4-exercise-use-whitespace
