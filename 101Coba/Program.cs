using System.Diagnostics;
using System;
using System.Threading;

class Restaurant
{
    private static AutoResetEvent _autoResetEvent = new AutoResetEvent(false); // true untuk memastikan thread pertama langsung dapat akses meja
    private static AutoResetEvent _autoResetEvent2 = new AutoResetEvent(false); // true untuk memastikan thread pertama langsung dapat Hotel
  //  private static List<string> _customerNames = new List<string> { "Alice", "Bob", "Charlie", "Dave", "Eve", "Frank", "Grace", "Harry", "Ivy", "Jack" };

    public static void Main()
    {
        Console.Write("Enter the number of customers Restaurant: ");
        int numOfCustomers = int.Parse(Console.ReadLine());
        List<string> customerNames = new List<string>();
        for (int i = 0; i < numOfCustomers; i++)
        {
            Console.Write($"Enter the name of customer {i + 1}: ");
             string name = Console.ReadLine();
            customerNames.Add(name);
        }
    
       Console.Write("Enter the number of customers Hotel: ");
        int numOfHotelCustomers = int.Parse(Console.ReadLine());
        List<string> customerHotelNames = new List<string>();
        for (int i = 0; i < numOfHotelCustomers; i++)
        {
            Console.Write($"Enter the name of Hotel customer {i + 1}: ");
             string name = Console.ReadLine();
            customerHotelNames.Add(name);
        }

        for (int i = 0; i < customerHotelNames.Count; i++)
        {
            Thread thread2 = new Thread(new ParameterizedThreadStart(OrderHotel));
            thread2.Start(new Tuple<int, string>(i, customerHotelNames[i]));
        }
        for (int i = 0; i < customerNames.Count; i++)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(OrderFood));
            thread.Start(new Tuple<int, string>(i, customerNames[i]));
        }
    }

    private static void OrderFood (object customerInfo)
    {
         Tuple<int,string> customerTuple = (Tuple<int,string>)customerInfo;
         int customerNumber = customerTuple.Item1;
         string customerName = customerTuple.Item2;
         Console.WriteLine ($"Customer {customerName} sudah tiba di restaurant");
        
        _autoResetEvent.WaitOne ();

        Console.WriteLine($"Cutomer {customerName} sudah mendapatkan meja");
        Thread.Sleep(5000);
        Console.WriteLine ($"Customer {customerName} sudah selesai makan");
        _autoResetEvent.Set();
    }

     private static void OrderHotel (object customerHotelInfo)
    {
         Tuple<int,string> customerHotelTuple = (Tuple<int,string>)customerHotelInfo;
         int customerHotelNumber = customerHotelTuple.Item1;
         string customerHotelName = customerHotelTuple.Item2;
         Console.WriteLine ($"Customer {customerHotelName} sudah tiba di Hotel");
        
        _autoResetEvent2.WaitOne ();

        Console.WriteLine($"Cutomer {customerHotelName} sudah mendapatkan kamar");
        Thread.Sleep(10000);
        Console.WriteLine ($"Customer {customerHotelName} sudah selesai menggunakan kamar");
        _autoResetEvent2.Set();
    }
}
