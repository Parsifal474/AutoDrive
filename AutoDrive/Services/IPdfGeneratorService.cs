using System.Threading.Tasks;
using AutoDrive.Models;

namespace AutoDrive.Services
{
    public interface IPdfGeneratorService
    {
        Task<byte[]> GenerateContractAsync(Student student, decimal totalAmount);
        Task<byte[]> GenerateCertificateAsync(Student student);
    }
}