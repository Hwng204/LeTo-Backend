using Application.DTOs;


namespace Application.Interfaces;

public interface IMatrixWorkbookExporter
{
    byte[] Create(MatrixResponse matrix, MatrixExportInfo? info = null);
}
