using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace GDataStructures
{
    class QueueTests
    {
        private GQueue<int> intQueue;
        private GQueue<int> resizeQueue;
        private GQueue<int> circularIndexQueue;
        private GQueue<string> stringQueue;
            
        [SetUp]
        public void InitialiseTests()
        {
            intQueue = new GQueue<int>();
            intQueue.Enqueue(1);
            intQueue.Enqueue(2);
            intQueue.Enqueue(3);

            resizeQueue = new GQueue<int>(3);
            resizeQueue.Enqueue(1);
            resizeQueue.Enqueue(2);
            resizeQueue.Enqueue(3);

            circularIndexQueue = new GQueue<int>(4);
            circularIndexQueue.Enqueue(1);
            circularIndexQueue.Enqueue(2);
            circularIndexQueue.Enqueue(3);
            circularIndexQueue.Enqueue(4);

            stringQueue = new GQueue<string>();
            stringQueue.Enqueue("first");
            stringQueue.Enqueue("second");
            stringQueue.Enqueue("third");
        }

        [Test]
        public void TestIntQueue()
        {
            Assert.AreEqual(intQueue.Count(),3);
            
            Assert.AreEqual(intQueue.Dequeue(),1);
            Assert.AreEqual(intQueue.Count(), 2);
            
            Assert.AreEqual(intQueue.Dequeue(), 2);
            Assert.AreEqual(intQueue.Count(), 1);

            Assert.AreEqual(intQueue.Dequeue(), 3);
            Assert.AreEqual(intQueue.Count(), 0);
        }

        [Test]
        public void TestResizeQueue()
        {
            //Initial size of resizeQueue is 3 so adding another item should force a resize:
            resizeQueue.Enqueue(4);
            Assert.AreEqual(resizeQueue.Count(),4);
            //resize adds space for 50 items so check capacity is 53:
            Assert.AreEqual(resizeQueue.Capacity(), 53);

            resizeQueue.Enqueue(5);
            Assert.AreEqual(resizeQueue.Count(),5);
            //another resize should not be necessary so capacity should still be 53:
            Assert.AreEqual(resizeQueue.Capacity(), 53);

            Assert.AreEqual(resizeQueue.Dequeue(),1);
            Assert.AreEqual(resizeQueue.Count(), 4);

            Assert.AreEqual(resizeQueue.Dequeue(), 2);
            Assert.AreEqual(resizeQueue.Dequeue(), 3);
            Assert.AreEqual(resizeQueue.Dequeue(), 4);
            Assert.AreEqual(resizeQueue.Count(), 1);
        }

        [Test]
        public void TestCircularIndexing()
        {
            Assert.AreEqual(circularIndexQueue.Capacity(),4);
            Assert.AreEqual(circularIndexQueue.Dequeue(),1);
            circularIndexQueue.Enqueue(5);
            Assert.AreEqual(circularIndexQueue.Capacity(), 4);  //no resize needed
            Assert.AreEqual(circularIndexQueue.Dequeue(), 2);
            Assert.AreEqual(circularIndexQueue.Dequeue(), 3);
            circularIndexQueue.Enqueue(6);
            Assert.AreEqual(circularIndexQueue.Capacity(), 4);  //no resize needed
            Assert.AreEqual(circularIndexQueue.Count(), 3);  
        }

        [Test]
        public void TestStringQueue()
        {
            Assert.AreEqual(stringQueue.Count(), 3);
            Assert.AreEqual(stringQueue.Dequeue(),"first");

            Assert.AreEqual(stringQueue.Count(), 2);
            Assert.AreEqual(stringQueue.Dequeue(), "second");

            Assert.AreEqual(stringQueue.Count(), 1);
            Assert.AreEqual(stringQueue.Dequeue(), "third");

            Assert.AreEqual(stringQueue.Count(), 0);

            stringQueue.Enqueue("fourth");
            Assert.AreEqual(stringQueue.Dequeue(), "fourth");
        }
    }
}
