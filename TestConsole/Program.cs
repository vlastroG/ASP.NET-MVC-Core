using AddIns;
using RazorPagesGB.Models;

var safe_list = new ConcurrentList<Product>();
Product product1 = new();
Product product2 = new();
Product product3 = new();

safe_list.Add(product1);
safe_list.Add(product2);
safe_list.Add(product3);

var all = safe_list.GetAll();

safe_list.Remove(product2);

foreach (var item in safe_list)
{
    Console.WriteLine($"ID:\t{item.Id}");
}
all = safe_list.GetAll();

safe_list.Clear();

all = safe_list.GetAll();


