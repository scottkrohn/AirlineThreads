using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AirlineApplication
{
    class TravelAgency
    {

		// Data Members
        private string agencyName;
		private Thread airline;
		private MultiCellBuffer buffer;
		private bool priceDropOccured;
		private EventArgs currentEventArgs;
        private List<OrderObject> completedOrders;

		// Constructor
        public TravelAgency(string name, Thread airline, MultiCellBuffer buf) 
        {
            this.completedOrders = new List<OrderObject>();
            this.agencyName = name;
			this.airline = airline;
			this.buffer = buf;
			this.priceDropOccured = false;
        }

        // When a price drops occurs, set the flag and store the details of the price drop.
        public void OnPriceDrop(object sender, EventArgs e) 
        {
			priceDropOccured = true;
			currentEventArgs = e;
        }

        // Generates a credit card number that's between 10000-30000
        public int generateCreditNo() 
        {
            Random rnd = new Random();
            return rnd.Next(10000, 30000);
        }

        /* This function runs in a thread and will remain alive until all of the airline threads
         * have ended. The function checks whether or not a price drop event has occured by 
         * checking the value of 'priceDropOccured'. If a price drop occurs then this function 
         * creates an OrderObject and sends it to the MultiCellBuffer.
         */
        public void runAgency()
        {
			// Run the thread while the airline thread is still alive.
			while(airline.IsAlive)
			{
				if(priceDropOccured)
				{
                    // Get the previous price and new price (used for calculating ticket need)
					int previousPrice = ((PriceDropEventArgs)currentEventArgs).getPrevious();
					int lowPrice = ((PriceDropEventArgs)currentEventArgs).getLow();
                    int ticketNeed = calculateTicketNeed(previousPrice, lowPrice);
					OrderObject order = new OrderObject(agencyName, generateCreditNo(), ticketNeed, lowPrice);
					string encodedOrderString= Encoder.encodeOrder(order);
                    lock (buffer) 
                    {
                        buffer.setOneCell(encodedOrderString);
                    }
                    // Reset the priceDropOccured data member once it's been handeled.
					priceDropOccured = false;
				}
			}
        }

		// Calculates the number of tickets needed based on the change in the current/previous price.
        public int calculateTicketNeed(int previousPrice, int currentPrice) 
        {
            int priceDifference = previousPrice - currentPrice;
			Random rnd = new Random(DateTime.Now.Millisecond);
            int ticketNeed = 0;

            if(0 < priceDifference && priceDifference <= 200) 
            {   // difference between $1-$200
				ticketNeed = RandomGenerator.next(10, 20);
            }
            else if(201 < priceDifference && priceDifference <= 400) 
            { // difference between $201-$400
				ticketNeed = RandomGenerator.next(21, 40);
            }
            else if(401 < priceDifference && priceDifference <= 600) 
            { // difference between $401-$600
                ticketNeed = RandomGenerator.next(41, 60);
            }
            else if(601 < priceDifference && priceDifference <= 800) 
            { // difference between $601-$800
                ticketNeed = RandomGenerator.next(61, 80);
            }
            else if(801 < priceDifference && priceDifference <= 900) 
            { // difference between $801-$900
                ticketNeed = RandomGenerator.next(81, 100);
            }
            return ticketNeed;
        }
    }

    // Class to generate random numbers (used to calcualte ticket order amounts)
	static class RandomGenerator
	{
		static Random rnd = new Random();
		public static int next(int low, int high)
		{
			return rnd.Next(low, high);
		}
	}
}