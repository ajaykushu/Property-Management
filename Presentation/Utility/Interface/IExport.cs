using Presentation.ConstModal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Presentation.Utiliity.Interface
{
    public interface IExport<T>
    {
        /// <summary>
        /// accept  Object for creating csv
        /// </summary>
        /// <returns>string File</returns>
        Task<byte[]> CreateCSV(T model);

        Task<byte[]> CreateListCSV(List<T> model);
        Task<byte[]> CreateExcel(T model);
        Task<byte[]> CreateListExcel(List<T> workOrderDetail);
    }
}