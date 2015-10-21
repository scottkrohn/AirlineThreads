using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AirlineApplication
{

    class Airline
    {
        // Events and delgates 
        public delegate void PriceDropEventHandler(object sender, EventArgs e);
        public event PriceDropEventHandler PriceDrop;

        // Data members
        private string airlineName;     // used as the thread name
        private int previousPrice;
        private int priceDropCounter;
		// Reference to MultiCellBuffer to get orders.
		private MultiCellBuffer buffer;
        
        // Constructor
        public Airline(string name, MultiCellBuffer buf) 
        {
            this.airlineName = name;
            previousPrice = -1;   // -1 denotes no price has been set
            priceDropCounter = 0;
			this.buffer = buf;
        }

        /* Function to be called when threading begins. Runs until the priceDropCounter has
		 * reached a specified value, then the thread ends. Each iteration of the loop produces
		 * a price, checks if the price is lower than the previous price, then calls OnPriceDrop 
		 * if it is lower. 
		*/
        public void runAirline()
        {
            while(priceDropCounter < 20){
                int newPrice = pricingModel();
                Console.WriteLine("Updated Price: {0}", newPrice);
                if(checkPriceDrop(newPrice))
                {
					Console.WriteLine("\n******* PRICE DROP OCCURED *******\n");
                    OnPriceDrop(newPrice);
                    previousPrice = newPrice;
                }

				// Sleep inbetween iterations to slow down output on screen so it's readable.
                Thread.Sleep(50);
            }
        }

        // Return true if the price has been lowered since the previous price.
        public bool checkPriceDrop(int newPrice) 
        {
            // If this is the first price that's been set.
            if (previousPrice == -1) 
            {
                previousPrice = newPrice;
                return false;
            }
            else if(newPrice < previousPrice)
            {
                priceDropCounter++;
                return true;
            }
            previousPrice = newPrice;
            return false;
        }

        public int getPreviousPrice() 
        {
            return previousPrice;
        }

        // Returns a random number between [100, 900] which is the ticket price.
        public int pricingModel() 
        {
            Random rnd = new Random();
            return rnd.Next(100, 900);
        }

		// Emits the PriceDrop function and sends some information with it.
        public virtual void OnPriceDrop(int newPrice) 
        {
            // Check if there are any subscribers to the event.
            if(PriceDrop != null)
            {
                // args contains pricing information to be sent to a TravelAgency object.
                PriceDropEventArgs args = new PriceDropEventArgs(previousPrice, newPrice);
                PriceDrop(this, args);    // emit the event
            }
        }

		// Function called when the MultiCellBuffer class emits an OrderSubmit() event.
		// Airline gets the encoded order, decodes it, then sends it to OrderProcessing to be completed.
		public void OnOrderSubmitted(object sender, EventArgs e)
		{
			try
			{
                string encodedOrderString;
				OrderObject decodedOrder;
                encodedOrderString = buffer.getOneCell();
				decodedOrder = Decoder.decodeOrder(encodedOrderString);
				OrderProcessing orderProcessor = new OrderProcessing(decodedOrder);
				// Process the order in a thread.
				Thread t1 = new Thread(orderProcessor.processOrder);
				t1.Start();
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
    }

	// Class to store information about the current/previous price when a price drop occurs.
	// This is used to send pricing information with a PriceDrop() event.
    public class PriceDropEventArgs : EventArgs 
    {
        int previousPrice;
        int newLowPrice;

        public PriceDropEventArgs(int prev, int low) 
        {
            this.previousPrice = prev;
            this.newLowPrice = low;
        }

        public int getPrevious() 
        {
            return previousPrice;
        }

        public int getLow() 
        {
            return newLowPrice;
        }
    }
}