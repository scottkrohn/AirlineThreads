using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineApplication
{
    class Decoder
    {
        public static OrderObject decodeOrder(string order) {
            // Split the string using '.' as the delimiter.
            string[] attributes = order.Split('.');

            // Get attributes from the array.
            string id = attributes[0];
            int cardNo = Convert.ToInt32(attributes[1]);
            int amount = Convert.ToInt32(attributes[2]);
            double cost = Convert.ToDouble(attributes[3]);
            return new OrderObject(id, cardNo, amount, cost);
        }
    }
}
