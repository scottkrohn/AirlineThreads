using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineApplication {
    class OrderObject {
        // Private data members
        private string senderId;    // id of the sending airline
        private int cardNo;         // credit card number
        private int amount;         // # of tickets to be ordered
        private double unitPrice;   // unit price of each ticket

        public OrderObject(string id, int card, int amt, double price) {
            this.senderId = id;
            this.cardNo = card;
            this.amount = amt;
            this.unitPrice = price;
        }

        public void setId(string id) {
            this.senderId = id;
        }

        public string getId() {
            return senderId;
        }

        public void setCardNo(int card) {
            this.cardNo = card;
        }

        public int getCardNo() {
            return cardNo;
        }

        public void setAmount(int amt) {
            this.amount = amt;
        }

        public int getAmount() {
            return amount;
        }

        public void setUnitPrice(double price) {
            this.unitPrice = price;
        }

        public double getUnitPrice() {
            return unitPrice;
        }

        public override string ToString() {
            return string.Format("{0}, {1}, {2}, {3}", senderId, cardNo, amount, unitPrice);
        }

    }
}
