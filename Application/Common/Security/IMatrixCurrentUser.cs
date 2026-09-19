using Domain.Entities.QuestionBank;

namespace Application.Common.Security;

public interface IMatrixCurrentUser
{
    MatrixActor Actor { get; }
}

