namespace Domain.Entities.QuestionBank;

public sealed class MatrixDomainException : InvalidOperationException
{
    public MatrixDomainException(string code, string message)
        : base(message)
    {
        Code = code;
    }

    public string Code { get; }
}

