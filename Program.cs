using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AirlineApplication
{
    class Program
    {
        static void Main(string[] args)
        {
			// buffer will be used by the airlines and travel agencies
			MultiCellBuffer buffer = new MultiCellBuffer();
			// 1 Airline for individual project.
            Airline a1 = new Airline("Krohn Airlines", buffer);
            Thread t1 = new Thread(a1.runAirline);	// airline thread
			
			// 5 Travel agencies for individual project.
            TravelAgency ta1 = new TravelAgency("Cheap Travel Help", t1, buffer);
            TravelAgency ta2 = new TravelAgency("Island Getaways", t1, buffer);
            TravelAgency ta3 = new TravelAgency("Northern Adventures", t1, buffer);
            TravelAgency ta4 = new TravelAgency("Deep Sea Excursions", t1, buffer);
            TravelAgency ta5 = new TravelAgency("Down South Travel", t1, buffer);
			// Travel agency threads
			Thread t2 = new Thread(ta1.runAgency);
			Thread t3 = new Thread(ta2.runAgency);
			Thread t4 = new Thread(ta3.runAgency);
			Thread t5 = new Thread(ta4.runAgency);
			Thread t6 = new Thread(ta5.runAgency);

			// Subscribe all the travel agencies to the price drop event in Airline
            a1.PriceDrop += ta1.OnPriceDrop;
            a1.PriceDrop += ta2.OnPriceDrop;
			a1.PriceDrop += ta3.OnPriceDrop;
			a1.PriceDrop += ta4.OnPriceDrop;
			a1.PriceDrop += ta5.OnPriceDrop;

			// Subscribe the airline to the OrderSubmit event in the buffer
            buffer.OrderSubmit += a1.OnOrderSubmitted;

			// Start all the threads
            t1.Start();
			t2.Start();
			t3.Start();
			t4.Start();
			t5.Start();
			t6.Start();
        }
    }
}
