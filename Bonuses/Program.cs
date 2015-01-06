using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Bonuses
{
    class Program
    {
        /// <summary>
        /// Class Represents bonuses
        /// </summary>
        public class Bonus
        {

            #region Private Properties

            private int min;
            private int max;
            private double percentage;

            #endregion // Private Properties

            #region Ctor

            public Bonus(int min, int max, double percentage)
            {
                this.min = min;
                this.max = max;
                this.percentage = percentage;
            }

            #endregion // Ctor

            #region Public Properties

            public int Min
            {
                get { return this.min; }
            }

            public int Max
            {
                get { return this.max; }
            }

            public double Percentage
            {
                get { return this.percentage; }
            }

            #endregion // Public Properties

        }

        /// <summary>
        /// Class Represents sales Categories
        /// </summary>
        public class Category
        {
            #region Private Properties

            private string description;
            private double price;
            private List<Bonus> bonuses;

            #endregion // Private Properties

            #region Ctor

            public Category(string description, double price)
            {   
                this.description = description;
                this.price = price;
                this.bonuses = new List<Bonus>();
            }

            #endregion // Ctor

            #region Public properties

            public string Description
            {
                get { return this.description; }
            }

            public double Price
            {
                get { return this.price; }
            }


            public List<Bonus> Bonuses
            {
                get { return this.bonuses; }
            }

            #endregion // Public Properties

            #region Public Methods


            /// <summary>
            /// addBonus() : 
            /// </summary>
            /// <param name="min"></param>
            /// <param name="max"></param>
            /// <param name="percentage"></param>
            public void addBonus(int min, int max, double percentage)
            {
                if ((min < 0 || min > int.MaxValue) || (max < 0 || max > int.MaxValue) && (percentage < 0))
                    throw new ApplicationException();

                this.bonuses.Add(new Bonus(min, max, percentage));
            }

            /// <summary>
            /// findBonusLevel() :
            /// returns the bonus percentage for the correct level
            /// </summary>
            /// <param name="num"></param>
            /// <returns></returns>
            public double findBonusLevel(int num)
            {
                if (num < 0 || num > int.MaxValue)
                    throw new ApplicationException();

                foreach (Bonus b in this.Bonuses)
                {
                    if (num >= b.Min && num <= b.Max)
                    {
                        return b.Percentage;
                    }

                }

                return 0;
            }

            #endregion // Public Methods

        }

        /// <summary>
        /// Class Represents an order
        /// </summary>
        private class Order
        {
            #region Private Properties

            private Dictionary<string, int> order;

            private List<Category> categories;

            #endregion // Private Properties

            #region Ctor

            public Order(Dictionary<string, int> order, List<Category> categories)
            {
                this.order = order;
                this.categories = categories;
            }

            #endregion Ctor

            #region Public Methods

            /// <summary>
            /// caculateBonus() :
            /// </summary>
            /// <returns></returns>
            public double calculateBonus()
            {
                double bonus = 0;

                if (this.order == null || this.categories == null)
                    throw new ApplicationException();

                foreach (KeyValuePair<string, int> entry in this.order)
                {
                    double perc = categories.Find(n => n.Description.Equals(entry.Key)).findBonusLevel(entry.Value);


                    if (perc != 0)
                    {
                        bonus += entry.Value * categories.Find(n => n.Description.Equals(entry.Key)).Price * perc;
                    }

                }

                return bonus;
            }

            #endregion Public Methods

        }

        /// <summary>
        /// main() : 
        /// Application entry point
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // create software categories and bonuses
            // using int.MaxValue as a lazy infinity
            List<Category> categories = new List<Category>();

            Category tmp = new Category("Servers", 10000);
                     
            tmp.addBonus(1, 5, .01);
            tmp.addBonus(6, 10, .02);
            tmp.addBonus(11, int.MaxValue, .05);
            categories.Add(tmp);

            tmp = new Category("Data Management", 5000);
            tmp.addBonus(1, 5, .01);
            tmp.addBonus(6, 10, .02);
            tmp.addBonus(11, int.MaxValue, .05);
            categories.Add(tmp);

            tmp = new Category("Office Automation", 3000);
            tmp.addBonus(1, 10, .02);
            tmp.addBonus(11, 20, .04);
            tmp.addBonus(21, int.MaxValue, .05);
            categories.Add(tmp);


            tmp = new Category("Development", 1500);
            tmp.addBonus(1, 3, .01);
            tmp.addBonus(4, 7, .03);
            tmp.addBonus(8, int.MaxValue, .05);
            categories.Add(tmp);


            // Main Menu
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Main Menu" + Environment.NewLine);
                Console.WriteLine("1) Display Categories and Bonuses" + Environment.NewLine);
                Console.WriteLine("2) Enter Order info to calculate bonus" + Environment.NewLine);
                Console.WriteLine("0) Exit" + Environment.NewLine);

                string result = Console.ReadLine();

                switch (result)
                {
                    case "1":

                        Console.Clear();

                        foreach (Category c in categories)
                        {
                            Console.WriteLine(c.Description + Environment.NewLine);

                            foreach (Bonus b in c.Bonuses)
                            {
                                if (b.Max < int.MaxValue)
                                {
                                    Console.Write(String.Format("{0:P1}", b.Percentage) + " (" + b.Min + " - " + b.Max + ")\t");
                                }
                                else
                                {
                                    Console.Write(String.Format("{0:P1}", b.Percentage) + " (" + b.Min + "+)");
                                }
                            }

                            Console.WriteLine(Environment.NewLine);
                        }

                        Console.WriteLine(Environment.NewLine + "Press any key to continue.");
                        Console.ReadKey();


                        break;

                    case "2":

                        Console.Clear();

                        Dictionary<string, int> order = new Dictionary<string, int>();

                        foreach (Category c in categories)
                        {
                            Console.WriteLine("Please enter number sold in category " + c.Description + ": " + Environment.NewLine +
                                "(non-numeric characters will default to 0)");

                            string s = Console.ReadLine();

                            int num;

                            int.TryParse(s, out num);

                            // ignore empty categories to save iterations
                            if (num != 0)
                            {
                                order.Add(c.Description, num);
                            }

                            Console.WriteLine(Environment.NewLine);
                        }

                        double finalBonus;

                        // Create order and calculate bonuses
                        Order o = new Order(order, categories);

                        finalBonus = o.calculateBonus();

                        Console.WriteLine("Final bonus total for this order: " + "$" + finalBonus + Environment.NewLine);
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadKey();

                        break;

                    case "0":

                        return;

                    default:

                        break;


                }
            }

        }

        #region Unit Testing

        /// <summary>
        /// NUnit Category Testing
        /// </summary>
        [TestFixture]
        public class CategoryTest
        {
            
            Category source;
            

            [SetUp]
            public void Init()
            {
                source = new Category("testSource", 500);
            }

            [Test]
            public void AddBonusTest()
            {
                source.addBonus(1, 3, .02);
            }

            [Test]
            [ExpectedException(typeof(ApplicationException))]
            public void addBonusNegativeValuesTest()
            {   
                try
                {
                    source.addBonus(-1, -1, -1);
                }
                catch (ApplicationException expected)
                {
                }


            }

            [Test]
            [ExpectedException(typeof(ApplicationException))]
            public void addBonusZeroValuesTest()
            {
                try
                {
                    source.addBonus(0, 0, 0);
                }
                catch (ApplicationException expected)
                {
                }
            }

            [Test]
            [ExpectedException(typeof(ApplicationException))]
            public void findBonusLevelNegativeValueTest()
            {
                try
                {
                    source.findBonusLevel(-1);
                }
                catch(ApplicationException expected)
                {
                }
            }

            [Test]
            public void findBonusLevelZeroValueTest()
            {
                double result = source.findBonusLevel(0);

                Assert.AreEqual(0, result);
            }


            [Test]
            public void findBonusLevelTest()
            {
                double result = source.findBonusLevel(2);

                Assert.AreEqual(.02, result, .0001);
            }
                
        }


        /// <summary>
        /// NUnit Order Testing
        /// </summary>
        [TestFixture]
        public class OrderTest
        {
            Category source;

            [SetUp]
            public void Init()
            {
                source = new Category("testSource", 500);
                source.addBonus(1, 3, .02);
            }

            [Test]
            public void calulateBonusTest()
            {
                List<Category> c = new List<Category>();
                c.Add(source);

                Dictionary<string, int> order = new Dictionary<string, int>();

                order.Add("testSource", 1);

                Order o = new Order(order, c);

                double result = o.calculateBonus();
                Assert.AreEqual(10, result, .0001);
            }

            [Test]
            [ExpectedException(typeof(ApplicationException))]
            public void calculateBonusNullProperties()
            {
                Order o = new Order(null, null);

                o.calculateBonus();
            }

        }

        #endregion //Unit Testing
    
    }


}

