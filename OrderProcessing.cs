using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineApplication
{
    class OrderProcessing
    {
        static double TAX_MULTIPLIER = .087;    // made up tax amount for the airline
		OrderObject order;

		public OrderProcessing(OrderObject o)
		{
			this.order = o;
		}

        public void processOrder()
        {
            if (validateCardNo(order.getCardNo()))
            {
				sendCompletedOrder(order, calculateTotalPrice(order));
            }
        }

        private double calculateTotalPrice(OrderObject order)
        {
            double costLessTax = order.getAmount() * order.getUnitPrice();
            double taxAmount = costLessTax * TAX_MULTIPLIER;
            return costLessTax + taxAmount;
        }

        // Check if the credit card number is between 10000-30000
        private bool validateCardNo(int cardNo)
        {
            if (10000 <= cardNo && cardNo <= 30000)
            {
                return true;
            }
            return false;
        }

        private void sendCompletedOrder(OrderObject order, double totalCost)
        {
            Console.WriteLine("Completed Order: {0}\t{1} tickets @ ${2}\tTOTAL: ${3}", order.getId(), order.getAmount(), order.getUnitPrice(), totalCost); 
        }

    }
}
