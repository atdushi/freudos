namespace FreudOS.Core;

/// <summary>
/// Исключение, выбрасываемое, когда Эго не справляется с давлением Id, 
/// и подавленный материал прорывается в виде системных симптомов.
/// </summary>
public class LibidoLeakException : Exception
{
    public LibidoLeakException(string message) : base(message) { }
}