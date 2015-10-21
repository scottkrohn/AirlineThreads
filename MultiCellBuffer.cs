using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AirlineApplication
{
    class MultiCellBuffer
    {
        // Delegates and Events
        public delegate void OrderSubmitHandler(object sender, EventArgs e);
        public event OrderSubmitHandler OrderSubmit;

        // Data members
        private static Semaphore _pool;
        private MyBuffer buffer;
        
        // Functions

        // Constructor
        public MultiCellBuffer()
        {
            buffer = new MyBuffer();
			// Semaphore with 2 open spots
            _pool = new Semaphore(2, 2);
        }

        // Emits an OrderSubmit event when called.
        public virtual void OnOrderSubmit() 
        {
            if (OrderSubmit != null) 
            {
                OrderSubmit(this, EventArgs.Empty);
            }
        }

        // Set one cell of the buffer to the argument string.
        public void setOneCell(string encodedOrder)
        {
			// Use semaphore to get access to cell data.
            _pool.WaitOne();
			// Use ReaderWriterLock to lock the buffer while writing to the cell
            ReaderWriterLock rwl = new ReaderWriterLock();
            rwl.AcquireWriterLock(Timeout.Infinite);
            buffer.setCell(encodedOrder);
            rwl.ReleaseWriterLock();
            _pool.Release();
			// Emit event that says an order has been submitted to the MultiCellBuffer
            OnOrderSubmit();
        }

        public string getOneCell()
        {
			// Use Semaphore to access the cell data.
            string returnString;
            _pool.WaitOne();
			// Use ReaderWriterLock to lock the buffer while reading.
			ReaderWriterLock rwl = new ReaderWriterLock();
			rwl.AcquireReaderLock(Timeout.Infinite);
            returnString = buffer.getCell(); 
            rwl.ReleaseReaderLock();
            _pool.Release();
            return returnString;
       }

    }

    // Data structure that's used as a two cell buffer. Allows read/write access.
    class MyBuffer 
    {
        private string cellOne;
        private string cellTwo;
        private bool cellOneFull;
        private bool cellTwoFull;

        // Sets a cell if the cell isn't containing an unread string.
        public bool setCell(string str) 
        {
            if (!cellOneFull) 
            {
                cellOne = str;
                cellOneFull = true;
                return true;
            }
            else if (!cellTwoFull) 
            {
                cellTwo = str;
                cellTwoFull = true;
                return true;
            }
            return false;
        }

        // Gets the string from a cell and sets the flag that it's been read.
        public string getCell() 
        {
            if (cellOneFull) 
            {
                cellOneFull = false;
                return cellOne;
            }
            else if (cellTwoFull) 
            {
                cellTwoFull = false;
                return cellTwo;
            }
            return null;
        }
    }

}
