namespace Application.Common;

public sealed class MatrixApplicationException : InvalidOperationException
{
    public MatrixApplicationException(string code, string message)
        : base(message)
    {
        Code = code;
    }

    public string Code { get; }
}

