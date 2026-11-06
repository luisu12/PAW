using PAW.Tasks;

namespace PAW.TestxUnit;

// Esta clase de pruebas valida el comportamiento de los métodos utilitarios Sync en PAW.Tasks.
// Sigue el estilo de xUnit con [Fact] para casos individuales y [Theory] para pruebas con datos.
public class SyncTests
{
    // Verifica que ExecuteItems devuelve exactamente la cantidad de filas generadas
    // según el argumento secs.
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    public async Task ExecuteItems_ShouldReturnExpectedCount(int secs)
    {
        // Preparación
        const string identifier = "TST";

        // Acción
        var result = await Sync.ExecuteItems(secs, identifier);

        // Verificación
        Assert.Equal(secs, result.Count());
    }

    // Verifica que cada fila generada inicie con el identificador proporcionado
    // y el formato de prefijo esperado.
    [Fact]
    public async Task ExecuteItems_ShouldPrefixEachItemWithIdentifier()
    {
        // Preparación
        const int secs = 3;
        const string identifier = "AAA";

        // Acción
        var result = (await Sync.ExecuteItems(secs, identifier)).ToList();

        // Verificación
        Assert.All(result, item => Assert.StartsWith("AAA - Second ", item));
    }

    // Verifica que Execute finaliza correctamente para un valor positivo típico.
    // Esto confirma que la ruta asíncrona no lanza excepciones inesperadas.
    [Fact]
    public async Task Execute_ShouldCompleteWithoutThrowing()
    {
        // Preparación
        const int secs = 2;
        const string identifier = "BBB";

        // Acción
        var exception = await Record.ExceptionAsync(() => Sync.Execute(secs, identifier));

        // Verificación
        Assert.Null(exception);
    }

    // Verifica que ExecuteItems devuelve una colección vacía cuando secs es cero.
    [Fact]
    public async Task ExecuteItems_WhenSecsIsZero_ShouldReturnEmpty()
    {
        // Preparación
        const int secs = 0;

        // Acción
        var result = await Sync.ExecuteItems(secs, "EMPTY");

        // Verificación
        Assert.Empty(result);
    }

    // Verifica que ExecuteEval finaliza correctamente sin lanzar excepciones.
    [Fact]
    public async Task ExecuteEval_ShouldCompleteWithoutThrowing()
    {
        // Preparación
        const int secs = 1001;
        const string identifier = "EVAL";

        // Acción
        var exception = await Record.ExceptionAsync(() => Sync.ExecuteEval(secs, identifier));

        // Verificación
        Assert.Null(exception);
    }

    // Verifica que ExecuteEval escribe en consola cuando el índice es múltiplo de 1000
    // (al inicio y luego en la iteración 1001 para secs = 1001).
    [Fact]
    public async Task ExecuteEval_ShouldWriteExpectedMilestoneMessages()
    {
        // Preparación
        const int secs = 1001;
        const string identifier = "EVAL";
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        try
        {
            // Acción
            await Sync.ExecuteEval(secs, identifier);
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        // Verificación
        var output = writer.ToString();
        Assert.Contains("EVAL - Second 1", output);
        Assert.Contains("EVAL - Second 1001", output);
    }
}
