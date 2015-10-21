using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineApplication
{
    // Convert an OrderObject into a string.
    // The string is formatted into id.cardno.amount.price
    class Encoder
    {
        public static string encodeOrder(OrderObject order) {
            string encodedOrderString = string.Format("{0}.{1}.{2}.{3}", 
                                        order.getId(), order.getCardNo(), order.getAmount(), order.getUnitPrice());
            return encodedOrderString;
        }
    }
}
