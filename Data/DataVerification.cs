using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WarrantyTracker.Models;
using WarrantyTracker.Data;

namespace WarrantyTracker.Services
{
    public static class DataVerification
    {
        public static async Task VerifyDataInsertion(PurchaseItem item)
        {
            try
            {
                using var context = new WarrantyContext();
                var savedItem = await context.PurchaseItems
                    .FirstOrDefaultAsync(p => p.SerialNumber == item.SerialNumber);

                if (savedItem != null)
                {
                    Console.WriteLine($"Found item in database: {savedItem.ItemName}");
                    Console.WriteLine($"Serial Number: {savedItem.SerialNumber}");
                    Console.WriteLine($"Purchase Date: {savedItem.PurchaseDate}");
                }
                else
                {
                    Console.WriteLine("Item not found in database!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Data verification error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }
    }
}